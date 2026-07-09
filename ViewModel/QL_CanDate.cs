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
    public class HangSapHetHanDTO
    {
        public string MaCanDate { get; set; }
        public string MaMH { get; set; }
        public string TenMH { get; set; }
        public int SoLuong { get; set; }
        public DateTime NgayHetHan { get; set; }
    }

    public class HuyHangDTO
    {
        public int MaHangHuy { get; set; }
        public string MaMH { get; set; }
        public string TenMH { get; set; }
        public int SoLuong { get; set; }
        public string LyDo { get; set; }
        public DateTime NgayHuy { get; set; }
        public string TrangThaiDuyet { get; set; }
    }

    public class QL_CanDate : BaseViewModel
    {
        QuanLySieuThiMiniEntities db = new QuanLySieuThiMiniEntities();
        private static QL_CanDate instance;
        public static QL_CanDate Instance
        {
            get
            {
                if (instance == null) instance = new QL_CanDate();
                return instance;
            }
        }

        private ObservableCollection<HangSapHetHanDTO> dsSapHetHan;
        public ObservableCollection<HangSapHetHanDTO> DSSapHetHan
        {
            get => dsSapHetHan;
            set { dsSapHetHan = value; OnPropertyChanged(nameof(DSSapHetHan)); }
        }

        private ObservableCollection<HuyHangDTO> dsHuyHang;
        public ObservableCollection<HuyHangDTO> DSHuyHang
        {
            get => dsHuyHang;
            set { dsHuyHang = value; OnPropertyChanged(nameof(DSHuyHang)); }
        }

        public ICommand KiemTraCanDateCommand { get; set; }
        public ICommand LapPhieuHuyCommand { get; set; }

        public QL_CanDate()
        {
            DSSapHetHan = new ObservableCollection<HangSapHetHanDTO>();
            DSHuyHang = new ObservableCollection<HuyHangDTO>();

            KiemTraCanDateCommand = new RelayCommand((p) => {
                CapNhatDuLieuCanDate();
            });

            // SỬA TẠI ĐÂY: Đưa việc khởi tạo Content vào trong luồng thực thi khi click để tránh lỗi nạp tĩnh
            // SỬA TẠI ĐÂY: Đưa việc khởi tạo Content vào trong luồng thực thi khi click để tránh lỗi nạp tĩnh
            LapPhieuHuyCommand = new RelayCommand((p) => {
                try
                {
                    Window hostWindow = new Window
                    {
                        Title = "Xử lý mặt hàng lỗi",
                        Width = 650,
                        Height = 650,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        ResizeMode = ResizeMode.NoResize
                    };
                    var vmHuy = HuyHoaDonViewModel.Instance;
                    if (vmHuy != null)
                    {
                        vmHuy.TuKhoaHD = "";
                        vmHuy.HoaDonHienTai = null;
                        if (vmHuy.DSChiTiet != null)
                        {
                            vmHuy.DSChiTiet.Clear();
                        }
                        vmHuy.LyDoHuy = "";
                        vmHuy.TrangThaiXuLyIndex = 0;
                    }

                    var giaodienHuy = new View.UC_HuyHoaDon();
                    hostWindow.Content = giaodienHuy;

                    hostWindow.ShowDialog();
                    LoadDuLieuKho();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể mở cửa sổ lập phiếu hủy: " + ex.Message, "Lỗi hệ thống");
                }
            });

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            LoadDuLieuKho();
        }

        private void CapNhatDuLieuCanDate()
        {
            try
            {
                using (var localDb = new QuanLySieuThiMiniEntities())
                {
                    // Xóa sạch dữ liệu cũ trong bảng cảnh báo trước khi quét mới
                    localDb.HangSapHetHans.RemoveRange(localDb.HangSapHetHans);
                    localDb.SaveChanges();

                    DateTime homNay = DateTime.Now.Date;
                    DateTime moc15Ngay = homNay.AddDays(15);

                    var dsCanDate = localDb.ChiTietNhapHangs
                                           .Where(x => x.NgayHetHan >= homNay && x.NgayHetHan <= moc15Ngay)
                                           .OrderBy(x => x.NgayHetHan)
                                           .ToList();

                    int stt = 1;
                    var dsDaThem = new System.Collections.Generic.HashSet<string>();

                    foreach (var loHang in dsCanDate)
                    {
                        var matHang = localDb.MatHangs.FirstOrDefault(m => m.MaMH == loHang.MaMH);
                        int tonKhoHienTai = matHang != null ? matHang.SoLuong : 0;

                        if (tonKhoHienTai > 0 && !dsDaThem.Contains(loHang.MaMH))
                        {
                            var canhBao = new HangSapHetHan()
                            {
                                MaCanDate = "CD" + stt.ToString("D3"),
                                MaMH = loHang.MaMH,
                                SoLuong = tonKhoHienTai,
                                NgayHetHan = loHang.NgayHetHan,
                                ViTri = matHang != null && !string.IsNullOrEmpty(matHang.ViTriQuay) ? matHang.ViTriQuay : "Chưa xếp kệ"
                            };

                            localDb.HangSapHetHans.Add(canhBao);
                            dsDaThem.Add(loHang.MaMH);
                            stt++;
                        }
                    }

                    localDb.SaveChanges();
                    LoadDuLieuKho();
                    MessageBox.Show("Đã quét thành công!\nDanh sách hàng cận date (dưới 15 ngày) đã được cập nhật.", "Hoàn tất", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Dữ liệu không hợp lệ khi lưu vào Database:");

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        sb.AppendLine($"- Cột [{validationError.PropertyName}]: {validationError.ErrorMessage}");
                    }
                }
                MessageBox.Show(sb.ToString(), "Lỗi Ràng Buộc Dữ Liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadDuLieuKho()
        {
            try
            {
                db = new QuanLySieuThiMiniEntities();

                var sapHetHanList = (from h in db.HangSapHetHans
                                     join m in db.MatHangs on h.MaMH equals m.MaMH
                                     select new HangSapHetHanDTO
                                     {
                                         MaCanDate = h.MaCanDate,
                                         MaMH = h.MaMH,
                                         TenMH = m.TenMH,
                                         SoLuong = h.SoLuong,
                                         NgayHetHan = h.NgayHetHan
                                     }).ToList();

                DSSapHetHan = new ObservableCollection<HangSapHetHanDTO>(sapHetHanList);

                var huyHangList = (from h in db.HuyHangs
                                   join m in db.MatHangs on h.MaMH equals m.MaMH
                                   select new HuyHangDTO
                                   {
                                       MaHangHuy = h.MaHangHuy,
                                       MaMH = h.MaMH,
                                       TenMH = m.TenMH,
                                       SoLuong = h.SoLuong,
                                       LyDo = h.LyDo,
                                       NgayHuy = h.NgayHuy,
                                       TrangThaiDuyet = h.Duyet ? "Đã duyệt" : "Chờ duyệt"
                                   }).OrderByDescending(x => x.NgayHuy).ToList();

                DSHuyHang = new ObservableCollection<HuyHangDTO>(huyHangList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu kho: " + ex.Message, "Thông báo lỗi");
            }
        }
    }
}