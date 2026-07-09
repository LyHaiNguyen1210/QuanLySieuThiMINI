using QuanLySieuThiMINI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLySieuThiMINI.ViewModel
{
    public class ChiTietNhapDTO : BaseViewModel
    {
        public string MaMH { get; set; }
        public string TenMH { get; set; }

        private int soLuong;
        public int SoLuong { get => soLuong; set { soLuong = value; OnPropertyChanged(nameof(SoLuong)); } }

        private DateTime ngayHetHan;
        public DateTime NgayHetHan { get => ngayHetHan; set { ngayHetHan = value; OnPropertyChanged(nameof(NgayHetHan)); } }
    }

    public class QL_NhapHang : BaseViewModel
    {
        private string maPhieuVuaLuu = "";
        private static QL_NhapHang instance;
        public static QL_NhapHang Instance => instance ?? (instance = new QL_NhapHang());

        private List<MatHang> _toanBoMatHang;

        private ObservableCollection<NhaCungCap> dsNhaCungCap;
        public ObservableCollection<NhaCungCap> DSNhaCungCap
        {
            get => dsNhaCungCap;
            set
            {
                dsNhaCungCap = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MatHang> dsMatHang;
        public ObservableCollection<MatHang> DSMatHang
        {
            get => dsMatHang;
            set
            {
                dsMatHang = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ChiTietNhapDTO> dsChiTietNhap;
        public ObservableCollection<ChiTietNhapDTO> DSChiTietNhap
        {
            get => dsChiTietNhap;
            set
            {
                dsChiTietNhap = value;
                OnPropertyChanged();
            }
        }

        private NhaCungCap selectedNhaCungCap;
        public NhaCungCap SelectedNhaCungCap
        {
            get => selectedNhaCungCap;
            set { selectedNhaCungCap = value; OnPropertyChanged(nameof(SelectedNhaCungCap)); LocMatHangTheoNhaCungCap(); }
        }

        // --- CÁC THUỘC TÍNH MỚI CHO TÍNH NĂNG NHẬP MÃ ---
        private string tuKhoaMaMH;
        public string TuKhoaMaMH
        {
            get => tuKhoaMaMH;
            set { tuKhoaMaMH = value; OnPropertyChanged(nameof(TuKhoaMaMH)); TimKiemMatHangTheoMa(); }
        }

        private string tenMatHangDaTimThay;
        public string TenMatHangDaTimThay { get => tenMatHangDaTimThay; set { tenMatHangDaTimThay = value; OnPropertyChanged(nameof(TenMatHangDaTimThay)); } }

        private MatHang selectedMatHang;
        public MatHang SelectedMatHang
        {
            get => selectedMatHang;
            set
            {
                selectedMatHang = value;
                OnPropertyChanged();
            }
        }
        private string soLuongNhap = "1";
        public string SoLuongNhap
        {
            get => soLuongNhap;
            set
            {
                soLuongNhap = value;
                OnPropertyChanged();
            }
        }
        private DateTime ngayHetHanNhap = DateTime.Now.AddMonths(6);
        public DateTime NgayHetHanNhap
        {
            get => ngayHetHanNhap;
            set
            {
                ngayHetHanNhap = value;
                OnPropertyChanged();
            }
        }
        public string TenNhanVienLap { get; set; } = LuuDangNhap.TenNhanVien;
        public ICommand ThemVaoPhieuCommand { get; set; }
        public ICommand XoaChiTietCommand { get; set; }
        public ICommand LuuPhieuNhapCommand { get; set; }
        public ICommand InPhieuNhapCommand { get; set; }
        public QL_NhapHang()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
            DSChiTietNhap = new ObservableCollection<ChiTietNhapDTO>();
            ThemVaoPhieuCommand = new RelayCommand(_ => ThemVaoPhieu());
            XoaChiTietCommand = new RelayCommand(p => XoaChiTiet(p as ChiTietNhapDTO));
            LuuPhieuNhapCommand = new RelayCommand(_ => LuuPhieuNhap());
            InPhieuNhapCommand = new RelayCommand(_ => InPhieuNhap());
            LoadData();
        }

        public void LoadData()
        {
            using (var db = new QuanLySieuThiMiniEntities())
            {
                DSNhaCungCap = new ObservableCollection<NhaCungCap>(db.NhaCungCaps.ToList());
                _toanBoMatHang = db.MatHangs.ToList();
                DSMatHang = new ObservableCollection<MatHang>();
            }
        }

        private void LocMatHangTheoNhaCungCap()
        {
            DSMatHang = new ObservableCollection<MatHang>(_toanBoMatHang);

            TuKhoaMaMH = "";
        }

        private void TimKiemMatHangTheoMa()
        {
            if (string.IsNullOrWhiteSpace(TuKhoaMaMH))
            {
                SelectedMatHang = null;
                TenMatHangDaTimThay = "";
                return;
            }

            var mh = DSMatHang.FirstOrDefault(x =>
                x.MaMH.ToLower() == TuKhoaMaMH.Trim().ToLower()||x.TenMH.ToLower()==TuKhoaMaMH.Trim());

            if (mh != null)
            {
                SelectedMatHang = mh;
                TenMatHangDaTimThay = mh.TenMH;
            }
            else
            {
                SelectedMatHang = null;
                TenMatHangDaTimThay = "Không tìm thấy!";
            }
        }

        private void ThemVaoPhieu()
        {
            if (SelectedMatHang == null) { MessageBox.Show("Vui lòng chọn mặt hàng hợp lệ!"); return; }
            if (!int.TryParse(SoLuongNhap, out int sl) || sl <= 0) { MessageBox.Show("Số lượng không hợp lệ!"); return; }
            if (NgayHetHanNhap.Date <= DateTime.Now.Date) { MessageBox.Show("Ngày hết hạn phải lớn hơn ngày hiện tại!"); return; }
            var existing = DSChiTietNhap.FirstOrDefault(x => x.MaMH == SelectedMatHang.MaMH && x.NgayHetHan.Date == NgayHetHanNhap.Date);
            if (existing != null) existing.SoLuong += sl;
            else DSChiTietNhap.Add(new ChiTietNhapDTO { MaMH = SelectedMatHang.MaMH, TenMH = SelectedMatHang.TenMH, SoLuong = sl, NgayHetHan = NgayHetHanNhap });

            TuKhoaMaMH = "";
        }

        private void XoaChiTiet(ChiTietNhapDTO item) => DSChiTietNhap.Remove(item);

        private void LuuPhieuNhap()
        {
            if (SelectedNhaCungCap == null || DSChiTietNhap.Count == 0) return;
            string maPhieu = "NH_" + DateTime.Now.ToString("ddMMyyHHmmss");

            using (var db = new QuanLySieuThiMiniEntities())
            {
                db.NhapHangs.Add(new NhapHang { MaNhapHang = maPhieu, NgayNhap = DateTime.Now, MaNCC = SelectedNhaCungCap.MaNCC, MaNV = LuuDangNhap.MaNV });
                foreach (var item in DSChiTietNhap)
                {
                    db.ChiTietNhapHangs.Add(new ChiTietNhapHang
                    {
                        MaNhapHang = maPhieu,
                        MaMH = item.MaMH,
                        SoLuong = item.SoLuong,
                        NgayNhap = DateTime.Now,
                        NgayHetHan = item.NgayHetHan
                    });
                    var m = db.MatHangs.Find(item.MaMH);
                    if (m != null) m.SoLuong += item.SoLuong;
                }
                db.SaveChanges();
            }
            maPhieuVuaLuu = maPhieu;
            MessageBox.Show("Lưu phiếu nhập kho thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            DSChiTietNhap.Clear();
            SelectedNhaCungCap = null;
            TuKhoaMaMH = "";
            TenMatHangDaTimThay = "";
            SoLuongNhap = "1";
            NgayHetHanNhap = DateTime.Now.AddMonths(6);
        }
        private void InPhieuNhap()
        {
            if (string.IsNullOrEmpty(maPhieuVuaLuu))
            {
                MessageBox.Show("Chưa có phiếu nhập nào được lưu! Fen phải bấm Lưu Phiếu trước khi In nha.", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }   

            try
            {
                View.NhapHangReportView frm = new View.NhapHangReportView(maPhieuVuaLuu);
                frm.Owner = Application.Current.MainWindow;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form in: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
