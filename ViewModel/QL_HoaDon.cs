using QuanLySieuThiMINI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLySieuThiMINI.ViewModel
{
    public class QL_HoaDon : BaseViewModel
    {
        QuanLySieuThiMiniEntities db = new QuanLySieuThiMiniEntities();
        private static QL_HoaDon instance;
        public static QL_HoaDon Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new QL_HoaDon();
                }
                return instance;
            }
        }

        // 1. Thêm thuộc tính Từ Ngày và Đến Ngày
        private DateTime _tuNgay;
        public DateTime TuNgay
        {
            get => _tuNgay;
            set { _tuNgay = value; OnPropertyChanged(nameof(TuNgay)); }
        }

        private DateTime _denNgay;
        public DateTime DenNgay
        {
            get => _denNgay;
            set { _denNgay = value; OnPropertyChanged(nameof(DenNgay)); }
        }

        // 2. Thêm Command để thực hiện lọc
        public ICommand LocHoaDonCommand { get; set; }

        private ObservableCollection<HoaDonBanHang> dsHoaDon;
        public ObservableCollection<HoaDonBanHang> DSHoaDon
        {
            get => dsHoaDon;
            set { dsHoaDon = value; OnPropertyChanged(nameof(DSHoaDon)); }
        }

        private ObservableCollection<ChiTietBanHang> dsChiTietHoaDon;
        public ObservableCollection<ChiTietBanHang> DSChiTietHoaDon
        {
            get => dsChiTietHoaDon;
            set { dsChiTietHoaDon = value; OnPropertyChanged(nameof(DSChiTietHoaDon)); }
        }

        private HoaDonBanHang selectedHoaDon;
        public HoaDonBanHang SelectedHoaDon
        {
            get => selectedHoaDon;
            set
            {
                if (selectedHoaDon != value)
                {
                    selectedHoaDon = value;
                    OnPropertyChanged(nameof(SelectedHoaDon));
                    LoadChiTietHoaDon();
                }
            }
        }

        public QL_HoaDon()
        {
            DSChiTietHoaDon = new ObservableCollection<ChiTietBanHang>();
            DSHoaDon = new ObservableCollection<HoaDonBanHang>();

            // Sửa lại giá trị mặc định: 30 ngày trước so với hiện tại
            TuNgay = DateTime.Now.AddDays(-30).Date;
            DenNgay = DateTime.Now.Date;

            // Khởi tạo Command lọc
            LocHoaDonCommand = new RelayCommand((p) => { LoadDanhSachHoaDon(); }, (p) => true);

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }

            try
            {
                db = new QuanLySieuThiMiniEntities();
                LoadDanhSachHoaDon();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo dữ liệu: " + ex.Message, "Thông báo lỗi");
            }
        }

        // 4. Sửa hàm load danh sách để lọc theo khoảng thời gian
        public void LoadDanhSachHoaDon()
        {
            if (db == null) return;
            try
            {
                // Đảm bảo DenNgay lấy hết thời gian của ngày đó (đến 23:59:59)
                DateTime startDate = TuNgay.Date;
                DateTime endDate = DenNgay.Date.AddDays(1).AddTicks(-1);

                var filtered = db.HoaDonBanHangs
                    .Where(hd => hd.NgayLapHD >= startDate && hd.NgayLapHD <= endDate)
                    .OrderByDescending(hd => hd.NgayLapHD)
                    .ToList();

                DSHoaDon = new ObservableCollection<HoaDonBanHang>(filtered);
  
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách hóa đơn: " + ex.Message, "Thông báo lỗi");
            }
        }

        private void LoadChiTietHoaDon()
        {
            DSChiTietHoaDon.Clear();
            if (SelectedHoaDon == null || db == null) return;

            try
            {
                var details = db.ChiTietBanHangs
                    .Where(ct => ct.MaHDBanHang == SelectedHoaDon.MaHDBanHang)
                    .ToList();

                DSChiTietHoaDon = new ObservableCollection<ChiTietBanHang>(details);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết hóa đơn: " + ex.Message, "Thông báo lỗi");
            }
        }
    }
}
