using QuanLySieuThiMINI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLySieuThiMINI.ViewModel
{
    public class HuyHoaDonViewModel : BaseViewModel
    {
        private QuanLySieuThiMiniEntities db = new QuanLySieuThiMiniEntities();
        private static HuyHoaDonViewModel instance;
        public static HuyHoaDonViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HuyHoaDonViewModel();
                }
                return instance;
            }
        }

        private string tuKhoaHD;
        public string TuKhoaHD
        {
            get => tuKhoaHD;
            set { tuKhoaHD = value; OnPropertyChanged(nameof(TuKhoaHD)); }
        }

        private HoaDonBanHang hoaDonHienTai;
        public HoaDonBanHang HoaDonHienTai
        {
            get => hoaDonHienTai;
            set { hoaDonHienTai = value; OnPropertyChanged(nameof(HoaDonHienTai)); }
        }

        private ObservableCollection<ChiTietBanHang> dsChiTiet;
        public ObservableCollection<ChiTietBanHang> DSChiTiet
        {
            get => dsChiTiet;
            set { dsChiTiet = value; OnPropertyChanged(nameof(DSChiTiet)); }
        }

        private ChiTietBanHang selectedChiTiet;
        public ChiTietBanHang SelectedChiTiet
        {
            get => selectedChiTiet;
            set { selectedChiTiet = value; OnPropertyChanged(nameof(SelectedChiTiet)); }
        }

        private string lyDoHuy;
        public string LyDoHuy
        {
            get => lyDoHuy;
            set { lyDoHuy = value; OnPropertyChanged(nameof(LyDoHuy)); }
        }

        private int trangThaiXuLyIndex;
        public int TrangThaiXuLyIndex
        {
            get => trangThaiXuLyIndex;
            set { trangThaiXuLyIndex = value; OnPropertyChanged(nameof(TrangThaiXuLyIndex)); }
        }

        public ICommand TimKiemCommand { get; set; }
        public ICommand XacNhanHuyCommand { get; set; }
        public ICommand DongCommand { get; set; }

        public HuyHoaDonViewModel()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject())) return;
            DSChiTiet = new ObservableCollection<ChiTietBanHang>();
            TrangThaiXuLyIndex = 0;

            TimKiemCommand = new RelayCommand((p) => { TimKiemHoaDon(); });
            XacNhanHuyCommand = new RelayCommand((p) => { ThucHienHuySanPham(p); });

            DongCommand = new RelayCommand((p) => {
                var uc = p as UserControl;
                if (uc != null)
                {
                    Window parentWindow = Window.GetWindow(uc);
                    parentWindow?.Close();
                }
            });
        }

        private void TimKiemHoaDon()
        {
            if (string.IsNullOrWhiteSpace(TuKhoaHD)) return;

            db = new QuanLySieuThiMiniEntities();
            var hd = db.HoaDonBanHangs.FirstOrDefault(x => x.MaHDBanHang == TuKhoaHD);

            if (hd == null)
            {
                MessageBox.Show("Không tìm thấy Hóa đơn này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                HoaDonHienTai = null;
                DSChiTiet.Clear();
                return;
            }

            HoaDonHienTai = hd;
            LoadChiTietGrid(hd.MaHDBanHang);
        }

        private void LoadChiTietGrid(string maHD)
        {
            var details = db.ChiTietBanHangs.Where(x => x.MaHDBanHang == maHD).ToList();
            DSChiTiet = new ObservableCollection<ChiTietBanHang>(details);
        }

        private void ThucHienHuySanPham(object p)
        {
            if (HoaDonHienTai == null) return;

            if (SelectedChiTiet == null)
            {
                MessageBox.Show("Vui lòng click chọn 1 mặt hàng trong danh sách để hủy!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(LyDoHuy))
            {
                MessageBox.Show("Vui lòng nhập lý do hàng bị lỗi!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var yn = MessageBox.Show($"Bạn có chắc muốn LẬP PHIẾU HỦY cho mặt hàng {SelectedChiTiet.MaMH} và loại nó ra khỏi Hóa đơn {HoaDonHienTai.MaHDBanHang}?",
                                     "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (yn == MessageBoxResult.Yes)
            {
                try
                {
                    // 1. Thêm mặt hàng bị lỗi vào bảng Hủy Hàng
                    db.HuyHangs.Add(new HuyHang()
                    {
                        MaMH = SelectedChiTiet.MaMH,
                        SoLuong = SelectedChiTiet.SoLuong,
                        LyDo = $"{LyDoHuy} (Rút từ HĐ: {HoaDonHienTai.MaHDBanHang})",
                        NgayHuy = DateTime.Now,
                        Duyet = TrangThaiXuLyIndex == 1
                    });

                    // 2. Xóa chi tiết mặt hàng đó khỏi cơ sở dữ liệu
                    var ctToRemove = db.ChiTietBanHangs.Find(SelectedChiTiet.STT);
                    if (ctToRemove != null)
                    {
                        db.ChiTietBanHangs.Remove(ctToRemove);
                    }

                    // 3. Tính toán lại Tổng tiền dựa trên danh sách bộ nhớ đang hiển thị UI (Tránh xung đột Db)
                    var currentListExceptDeleted = DSChiTiet.Where(x => x.STT != SelectedChiTiet.STT).ToList();
                    var hdToUpdate = db.HoaDonBanHangs.Find(HoaDonHienTai.MaHDBanHang);

                    if (currentListExceptDeleted.Count == 0)
                    {
                        if (hdToUpdate != null) db.HoaDonBanHangs.Remove(hdToUpdate);
                        MessageBox.Show("Sản phẩm đã được hủy. Vì hóa đơn trống nên hệ thống đã tự động xóa hóa đơn này!", "Hoàn tất", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (hdToUpdate != null)
                        {
                            decimal tongMoi = currentListExceptDeleted.Sum(x => x.ThanhTien) - hdToUpdate.TienGiamPoints;
                            hdToUpdate.TongTien = tongMoi < 0 ? 0 : tongMoi;
                        }
                        MessageBox.Show("Lập phiếu hủy sản phẩm thành công! Hóa đơn đã được cập nhật lại tổng tiền.", "Hoàn tất", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    db.SaveChanges();

                    // Cập nhật lại UI thông qua các instance singleton của bạn
                    if (QL_HoaDon.Instance != null) QL_HoaDon.Instance.LoadDanhSachHoaDon();
                    if (QL_CanDate.Instance != null) QL_CanDate.Instance.LoadDuLieuKho();

                    LoadChiTietGrid(HoaDonHienTai.MaHDBanHang);
                    LyDoHuy = "";
                    TrangThaiXuLyIndex = 0;

                    // 4. KIỂM TRA ĐÓNG CỬA SỔ NẾU HẾT HÀNG
                    if (DSChiTiet.Count == 0)
                    {
                        var uc = p as UserControl;
                        if (uc != null)
                        {
                            Window parentWindow = Window.GetWindow(uc);
                            parentWindow?.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi hủy: " + ex.Message, "Lỗi SQL", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}