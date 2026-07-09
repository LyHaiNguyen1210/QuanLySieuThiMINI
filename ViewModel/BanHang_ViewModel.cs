using QuanLySieuThiMINI.Model;
using QuanLySieuThiMINI.View;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace QuanLySieuThiMINI.ViewModel
{
    public class BanHang_ViewModel : BaseViewModel
    {
        QuanLySieuThiMiniEntities db = new QuanLySieuThiMiniEntities();
        public RelayCommand HoanTatGiaoDich_Command { get; set; }
        public RelayCommand HuyThanhToan_Command { get; set; }

        public LocMatHang SanPhamVM { get; set; }
        public ObservableCollection<GioHangItem> GioHang { get; set; } = new ObservableCollection<GioHangItem>();
        public ObservableCollection<int> DanhSachGiamGia { get; set; }
        public QL_KhachHang QLKhachHangVM { get; set; }

        public RelayCommand QuayLaiTrangChuCommand { get; set; }
        public RelayCommand ThemVaoGio { get; set; }
        public RelayCommand XoaKhoiGioCommand { get; set; }
        public RelayCommand TimKhachHangCommand { get; set; }
        public RelayCommand ChuyenSangTabThanhToanCommand { get; set; }
        public RelayCommand QuayLaiTabBanHangCommand { get; set; }
        public RelayCommand LuuTam_Command { get; set; }
        public RelayCommand XoaHDTam_Command { get; set; }
        public RelayCommand TiepTucThanhToan_Command { get; set; }
        public RelayCommand ChuyenTabKhachHang_Command { get; set; }
        public RelayCommand ApDungGiamGiaCommand { get; set; }
        public RelayCommand HuyPhieu_Command { get; set; }
        public RelayCommand TimMatHang { get; set; }
        public RelayCommand NhapSoCommand { get; set; }
        public RelayCommand XoaKyTuCommand { get; set; }
        public RelayCommand XoaTatCaCommand { get; set; }
        private string hinhThucThanhToan = "Tiền mặt";
        public string HinhThucThanhToan
        {
            get => hinhThucThanhToan;
            set
            {
                hinhThucThanhToan = value;
                OnPropertyChanged(nameof(HinhThucThanhToan));
            }
        }
        private string _vaiTro;
        public string VaiTro { get => _vaiTro; set { _vaiTro = value; OnPropertyChanged(); } }

        private int _chuyenTab;
        public int ChuyenTab { get => _chuyenTab; set { _chuyenTab = value; OnPropertyChanged(); } }

        public BanHang_ViewModel()
        {
            HuyThanhToan_Command = new RelayCommand(p => { ChuyenTab = 0; });
            HoanTatGiaoDich_Command = new RelayCommand(p => { ThựcThiHoanTatGiaoDich(); });
            try
            {
                SanPhamVM = new LocMatHang();
                DanhSachGiamGia = new ObservableCollection<int> { 0, 5, 10, 15, 20 };
                VaiTro = LuuDangNhap.VaiTro;
                QLKhachHangVM = new QL_KhachHang(db);

                DSHoaDonTam = new ObservableCollection<HoaDonTam>(db.HoaDonTams.ToList());
                DSChiTietHDTam = new ObservableCollection<ChiTietHoaDonTam>();

                QuayLaiTrangChuCommand = new RelayCommand(p =>
                {
                    TrangChuView tc = new TrangChuView();
                    tc.Show();
                    System.Windows.Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is QuanLySieuThiMINIView)?.Close();
                });

                ThemVaoGio = new RelayCommand(p => ThemGioHang());

                XoaKhoiGioCommand = new RelayCommand(p =>
                {
                    var item = p as GioHangItem;
                    if (item == null) return;
                    GioHang.Remove(item);
                    CapNhatTongTien();
                });
                TimKhachHangCommand = new RelayCommand(p =>
                {
                    QLKhachHangVM.TimKiem();
                    OnPropertyChanged(nameof(DiemNhanDuoc));
                    OnPropertyChanged(nameof(TongThanhToan));
                    OnPropertyChanged(nameof(TienTraLai));
                });

                ChuyenSangTabThanhToanCommand = new RelayCommand(p => { ChuyenTab = 2; });
                QuayLaiTabBanHangCommand = new RelayCommand(p => { ChuyenTab = 0; });
                ChuyenTabKhachHang_Command = new RelayCommand(p => { ChuyenTab = 1; });

                NhapSoCommand = new RelayCommand(p => { NhapSo(p.ToString()); });
                XoaKyTuCommand = new RelayCommand(p => { XoaKyTu(); });
                XoaTatCaCommand = new RelayCommand(p => { XoaTatCa(); });

                ApDungGiamGiaCommand = new RelayCommand(p =>
                {
                    if (GiamGia == 0)
                    {
                        MessageBox.Show("Vui lòng chọn mức giảm giá trước khi áp dụng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    OnPropertyChanged(nameof(TongThanhToan));
                    OnPropertyChanged(nameof(TienTraLai));
                });

                LuuTam_Command = new RelayCommand(_ => LuuTam());
                XoaHDTam_Command = new RelayCommand(_ => XoaHDTam());
                TiepTucThanhToan_Command = new RelayCommand(_ => TiepTucThanhToan());
                HuyPhieu_Command = new RelayCommand(_ => HuyPhieu());
                TimMatHang = new RelayCommand(_ => TimKiemMatHang());
            }
            catch (Exception ex)
            {
            }
        }

        public void HuyPhieu()
        {
            var yn = MessageBox.Show("Bạn có muốn Hủy Phiếu hiện tại không?", "Thông Báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (yn == MessageBoxResult.Yes)
            {
                GioHang.Clear(); CapNhatTongTien();
            }
        }

        private int _giamGia;
        public int GiamGia
        {
            get => _giamGia;
            set
            {
                _giamGia = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TongThanhToan));
                OnPropertyChanged(nameof(TienTraLai));

                // Lấy khách hàng từ QLKhachHangVM
                if (SuDungToanBoDiem && QLKhachHangVM.KhachHangTimDuoc != null)
                {
                    decimal tienSauGiamGia = TongTienGioHang - (TongTienGioHang * value / 100);
                    DiemSuDung = Math.Min(QLKhachHangVM.KhachHangTimDuoc.DiemThuong, (int)(tienSauGiamGia / 1000));
                    DiemSuDungText = DiemSuDung.ToString();
                }
            }
        }

        private string tuKhoa;
        public string TuKhoa { get => tuKhoa; set { if (tuKhoa == value) return; tuKhoa = value; OnPropertyChanged(nameof(TuKhoa)); } }

        public void TimKiemMatHang()
        {
            if (SanPhamVM == null) return;
            if (SanPhamVM.dsMH == null) SanPhamVM.dsMH = new ObservableCollection<MatHang>();

            if (string.IsNullOrWhiteSpace(TuKhoa))
            {
                var tatCaMatHang = db.MatHangs.ToList();
                SanPhamVM.dsMH.Clear();
                foreach (var mh in tatCaMatHang) SanPhamVM.dsMH.Add(mh);
                return;
            }

            var ketQua = db.MatHangs.Where(x => x.MaMH == TuKhoa || x.TenMH.Contains(TuKhoa)).ToList();

            if (ketQua.Count == 0)
            {
                MessageBox.Show("Không tìm thấy sản phẩm nào khớp với từ khóa", "Thông báo");
                return;
            }

            SanPhamVM.dsMH.Clear();
            foreach (var mh in ketQua) SanPhamVM.dsMH.Add(mh);
            if (ketQua.Count == 1) SanPhamVM.SelectedMH = ketQua.First();
            TuKhoa = "";
        }

        public void ThemGioHang()
        {
            if (SanPhamVM.SelectedMH == null)
            {
                MessageBox.Show("Chưa chọn sản phẩm nào!");
                return;
            }

            var sp = SanPhamVM.SelectedMH;
            var item = GioHang.FirstOrDefault(x => x.MaMH == sp.MaMH);

            if (item != null)
            {
                if (item.SoLuong + 1 > sp.SoLuong)
                {
                    MessageBox.Show("Không đủ hàng trong kho!");
                    return;
                }
                item.SoLuong++;
            }
            else
            {
                GioHang.Add(new GioHangItem
                {
                    MaMH = sp.MaMH,
                    TenMH = sp.TenMH,
                    SoLuong = 1,
                    DonGiaSauThue = (decimal)sp.DonGiaSauThue,
                    PhanTramGiam = 0
                });
            }
            CapNhatTongTien();
        }

        private decimal _tongTienGioHang;
        public decimal TongTienGioHang
        {
            get => _tongTienGioHang;
            set
            {
                _tongTienGioHang = value;
                OnPropertyChanged();
                if (QLKhachHangVM.KhachHangTimDuoc != null)
                {
                    decimal tienSauGiamGia = value - (value * GiamGia / 100);
                    int diemToiDaCanDung = (int)(tienSauGiamGia / 1000);

                    if (SuDungToanBoDiem)
                    {
                        DiemSuDung = Math.Min(QLKhachHangVM.KhachHangTimDuoc.DiemThuong, diemToiDaCanDung);
                        DiemSuDungText = DiemSuDung.ToString();
                    }
                    else if (DiemSuDung > diemToiDaCanDung)
                    {
                        DiemSuDung = diemToiDaCanDung;
                        DiemSuDungText = DiemSuDung.ToString();
                    }
                }
                OnPropertyChanged(nameof(TongThanhToan));
                OnPropertyChanged(nameof(TienTraLai));
            }
        }

        private void CapNhatTongTien()
        {
            TongTienGioHang = GioHang.Sum(x => x.ThanhTien);
        }

        // Lấy điểm từ QLKhachHangVM
        public int DiemNhanDuoc => (QLKhachHangVM.KhachHangTimDuoc == null) ? 0 : (QLKhachHangVM.KhachHangTimDuoc.DiemThuong) + 5;

        private int _diemSuDung;
        public int DiemSuDung
        {
            get => _diemSuDung;
            set
            {
                _diemSuDung = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TongThanhToan));
                OnPropertyChanged(nameof(TienTraLai));
            }
        }

        public decimal TongThanhToan
        {
            get
            {
                decimal tienSauGiamGia = TongTienGioHang - (TongTienGioHang * GiamGia / 100);
                decimal giamTuDiem = DiemSuDung * 1000;
                decimal tong = tienSauGiamGia - giamTuDiem;
                return tong < 0 ? 0 : tong;
            }
        }

        private decimal _khachDua;
        public decimal KhachDua
        {
            get => _khachDua;
            set
            {
                _khachDua = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TienTraLai));
            }
        }

        public decimal TienTraLai
        {
            get
            {
                decimal tien = KhachDua - TongThanhToan;
                return tien < 0 ? 0 : tien;
            }
        }

        private string _diemSuDungText;
        public string DiemSuDungText
        {
            get => _diemSuDungText;
            set
            {
                if (_diemSuDungText == value) return;
                _diemSuDungText = value;
                OnPropertyChanged();
                DiemSuDung = int.TryParse(value, out var diem) ? diem : 0;
            }
        }

        private string _khachDuaText;
        public string KhachDuaText
        {
            get => _khachDuaText;
            set
            {
                if (_khachDuaText == value) return;
                _khachDuaText = value;
                OnPropertyChanged();
                KhachDua = decimal.TryParse(value, out var tien) ? tien : 0;
            }
        }

        private bool _suDungToanBoDiem;
        public bool SuDungToanBoDiem
        {
            get => _suDungToanBoDiem;
            set
            {
                _suDungToanBoDiem = value;
                OnPropertyChanged();

                if (value && QLKhachHangVM.KhachHangTimDuoc != null)
                {
                    decimal tienSauGiamGia = TongTienGioHang - (TongTienGioHang * GiamGia / 100);
                    DiemSuDung = Math.Min(QLKhachHangVM.KhachHangTimDuoc.DiemThuong, (int)(tienSauGiamGia / 1000));
                    DiemSuDungText = DiemSuDung.ToString();
                }
                else
                {
                    DiemSuDung = 0;
                    DiemSuDungText = "0";
                }
            }
        }

        public void NhapSo(string so) => KhachDuaText += so;
        public void XoaKyTu() { if (!string.IsNullOrEmpty(KhachDuaText)) KhachDuaText = KhachDuaText.Remove(KhachDuaText.Length - 1); }
        public void XoaTatCa() => KhachDuaText = "";

        private ObservableCollection<HoaDonTam> dsHoaDonTam;
        public ObservableCollection<HoaDonTam> DSHoaDonTam { get => dsHoaDonTam; set { dsHoaDonTam = value; OnPropertyChanged(nameof(DSHoaDonTam)); } }

        private ObservableCollection<ChiTietHoaDonTam> dsChiTietHDTam;
        public ObservableCollection<ChiTietHoaDonTam> DSChiTietHDTam { get => dsChiTietHDTam; set { dsChiTietHDTam = value; OnPropertyChanged(nameof(DSChiTietHDTam)); } }

        private HoaDonTam selected_HDTam;
        public HoaDonTam Selected_HDTam
        {
            get => selected_HDTam;
            set
            {
                if (selected_HDTam == value) return;
                selected_HDTam = value;
                OnPropertyChanged(nameof(Selected_HDTam));
                LoadChiTietHDTam();
            }
        }

        private void LuuTam()
        {
            if (GioHang.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var danhSachMa = db.HoaDonTams.Select(x => x.MaHDTam).ToList();
                int maxId = 0;
                foreach (var ma in danhSachMa)
                {
                    string phanSo = ma.Replace("HDT", "").Trim();
                    if (int.TryParse(phanSo, out int idhienTai) && idhienTai > maxId) maxId = idhienTai;
                }

                string maHDTam = "HDT" + (maxId + 1).ToString("D2");
                var kh = QLKhachHangVM.KhachHangTimDuoc; // Dùng khách từ QLKhachHangVM

                var hdTam = new HoaDonTam()
                {
                    MaHDTam = maHDTam,
                    MaKH = kh?.MaKH,
                    TongTien = TongThanhToan,
                    NgayLapHD = DateTime.Now,
                    TenKH = kh != null ? kh.TenKH : "Khách vãng lai"
                };
                db.HoaDonTams.Add(hdTam);

                foreach (var item in GioHang)
                {
                    db.ChiTietHoaDonTams.Add(new ChiTietHoaDonTam()
                    {
                        MaHDTam = maHDTam,
                        MaMH = item.MaMH,
                        SoLuong = item.SoLuong,
                        ThanhTien = item.ThanhTien,
                        GiamGia = GiamGia
                    });
                }
                db.SaveChanges();
                DSHoaDonTam = new ObservableCollection<HoaDonTam>(db.HoaDonTams.ToList());

                MessageBox.Show("Lưu hóa đơn tạm thành công!");
                GioHang.Clear(); CapNhatTongTien();

                // Clear dữ liệu khách
                QLKhachHangVM.KhachHangTimDuoc = null;
                QLKhachHangVM.SDTTim = "";
                SuDungToanBoDiem = false;
                DiemSuDung = 0;
                ChuyenTab = 0;
            }
            catch (Exception ex) { }
        }

        private void LoadChiTietHDTam()
        {
            DSChiTietHDTam.Clear();
            if (Selected_HDTam == null) return;
            var list = db.ChiTietHoaDonTams.Where(x => x.MaHDTam == Selected_HDTam.MaHDTam).ToList();
            foreach (var ct in list) DSChiTietHDTam.Add(ct);
        }

        private void XoaHDTam()
        {
            if (Selected_HDTam == null) return;
            var yn = MessageBox.Show($"Xóa Hóa đơn tạm {Selected_HDTam.MaHDTam}?", "Xác nhận", MessageBoxButton.YesNo);
            if (yn == MessageBoxResult.Yes)
            {
                try
                {
                    var hd = db.HoaDonTams.FirstOrDefault(x => x.MaHDTam == Selected_HDTam.MaHDTam);
                    if (hd != null)
                    {
                        db.HoaDonTams.Remove(hd); db.SaveChanges();
                        DSHoaDonTam.Remove(Selected_HDTam); DSChiTietHDTam.Clear();
                    }
                }
                catch (Exception ex) { }
            }
        }

        private void TiepTucThanhToan()
        {
            if (Selected_HDTam == null || GioHang.Count > 0) return;
            try
            {
                GioHang.Clear();
                int stt = 1;
                foreach (var item in DSChiTietHDTam)
                {
                    var mh = db.MatHangs.FirstOrDefault(x => x.MaMH == item.MaMH);
                    if (mh == null) continue;
                    GioHang.Add(new GioHangItem()
                    {
                        STT = stt++,
                        MaMH = item.MaMH,
                        TenMH = mh.TenMH,
                        SoLuong = item.SoLuong,
                        DonGiaSauThue = Convert.ToDecimal(mh.DonGiaSauThue),
                        PhanTramGiam = Convert.ToInt32(item.GiamGia)
                    });
                }

                if (!string.IsNullOrEmpty(Selected_HDTam.MaKH))
                {
                    var kh = db.KhachHangs.FirstOrDefault(x => x.MaKH == Selected_HDTam.MaKH);
                    if (kh != null)
                    {
                        QLKhachHangVM.SDTTim = kh.SDT;
                        QLKhachHangVM.KhachHangTimDuoc = kh;
                    }
                }
                else
                {
                    QLKhachHangVM.SDTTim = "";
                    QLKhachHangVM.KhachHangTimDuoc = null;
                }

                CapNhatTongTien();

                var hdXoa = db.HoaDonTams.FirstOrDefault(x => x.MaHDTam == Selected_HDTam.MaHDTam);
                if (hdXoa != null) { db.HoaDonTams.Remove(hdXoa); db.SaveChanges(); }
                DSHoaDonTam.Remove(Selected_HDTam);
                DSChiTietHDTam.Clear();
                ChuyenTab = 0;
            }
            catch (Exception ex) {  }
        }
        private void ThựcThiHoanTatGiaoDich()
        {
            if (GioHang.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống, không thể thanh toán!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (HinhThucThanhToan == "Tiền mặt")
            {
                if (KhachDua < TongThanhToan)
                {
                    MessageBox.Show("Khách đưa chưa đủ tiền!", "Lỗi thanh toán", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            try
            {
                var danhSachMaHD = db.HoaDonBanHangs.Select(x => x.MaHDBanHang).ToList();
                int maxId = 0;
                foreach (var ma in danhSachMaHD)
                {
                    string phanSo = ma.Replace("HD", "").Trim();
                    if (int.TryParse(phanSo, out int idhienTai) && idhienTai > maxId) maxId = idhienTai;
                }
                string maHoaDonMoi = "HD" + (maxId + 1).ToString("D2");
                string maNhanVien = "NV02";
                var hoaDonMoi = new HoaDonBanHang()
                {
                    MaHDBanHang = maHoaDonMoi,
                    MaNV = maNhanVien,
                    MaKH = QLKhachHangVM.KhachHangTimDuoc?.MaKH,
                    NgayLapHD = DateTime.Now,
                    Ca = "Sáng",
                    TienGiamPoints = DiemSuDung * 1000,
                    TongTien = TongThanhToan
                };
                db.HoaDonBanHangs.Add(hoaDonMoi);
                foreach (var item in GioHang)
                {
                    var chiTiet = new ChiTietBanHang()
                    {
                        MaHDBanHang = maHoaDonMoi,
                        MaMH = item.MaMH,
                        SoLuong = item.SoLuong,
                        GiamGia = item.PhanTramGiam,
                        ThanhTien = item.ThanhTien
                    };
                    db.ChiTietBanHangs.Add(chiTiet);

                    var matHangDb = db.MatHangs.FirstOrDefault(x => x.MaMH == item.MaMH);
                    if (matHangDb != null) matHangDb.SoLuong -= item.SoLuong;
                }
                if (QLKhachHangVM.KhachHangTimDuoc != null)
                {
                    var khDb = db.KhachHangs.FirstOrDefault(x => x.MaKH == QLKhachHangVM.KhachHangTimDuoc.MaKH);
                    if (khDb != null)
                    {
                        khDb.DiemThuong -= DiemSuDung;
                        khDb.DiemThuong += DiemNhanDuoc;
                    }
                }
                db.SaveChanges();
                MessageBox.Show("Thanh toán " + HinhThucThanhToan + " thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                HoaDonReportView printWindow = new HoaDonReportView(maHoaDonMoi);
                printWindow.ShowDialog();
                GioHang.Clear();
                CapNhatTongTien();
                KhachDuaText = "";
                GiamGia = 0;
                SuDungToanBoDiem = false;
                DiemSuDung = 0;
                DiemSuDungText = "0";
                QLKhachHangVM.SDTTim = "";
                QLKhachHangVM.KhachHangTimDuoc = null;
                ChuyenTab = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi trong quá trình thanh toán: " + ex.Message, "Thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}