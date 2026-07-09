using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
// Hãy thay thế đường dẫn Namespace này bằng Namespace chứa DB Context của dự án bạn
using QuanLySieuThiMINI.Model;

namespace QuanLySieuThiMINI.ViewModel
{
    public class NhanVienViewModel : BaseViewModel
    {
        private ObservableCollection<NhanVien> _dsnv;
        public ObservableCollection<NhanVien> dsnv
        {
            get => _dsnv;
            set { _dsnv = value; OnPropertyChanged(); }
        }

        // BỔ SUNG: Danh sách Chức vụ để nạp lên ComboBox ngoài giao diện XAML
        private ObservableCollection<ChucVu> _dscv;
        public ObservableCollection<ChucVu> dscv
        {
            get => _dscv;
            set { _dscv = value; OnPropertyChanged(); }
        }

        private NhanVien _selectedNV;
        public NhanVien SelectedNV
        {
            get => _selectedNV;
            set
            {
                _selectedNV = value;
                OnPropertyChanged();

                // Mẹo nhỏ: Nếu người dùng bỏ chọn, ta lập tức cấp mới một thực thể rỗng 
                // để các ô TextBox dưới XAML không bị crash hoặc dính lỗi null reference.
                if (_selectedNV == null)
                {
                    _selectedNV = new NhanVien { NgaySinh = DateTime.Now };
                    OnPropertyChanged();
                }
            }
        }

        // Khai báo Commands cho 3 nút ấn
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        public NhanVienViewModel()
        {
            SelectedNV = new NhanVien { NgaySinh = DateTime.Now };
            LoadData();
            AddCommand = new RelayCommand(p => ExecuteAdd(), p => SelectedNV != null && !string.IsNullOrEmpty(SelectedNV.MaNV) && !string.IsNullOrEmpty(SelectedNV.TenNV));
            DeleteCommand = new RelayCommand(p => ExecuteDelete(), p => SelectedNV != null && CheckNhanVienTonTai(SelectedNV.MaNV));
            UpdateCommand = new RelayCommand(p => ExecuteUpdate(), p => SelectedNV != null && CheckNhanVienTonTai(SelectedNV.MaNV));
        }
        private void LoadData()
        {
            try
            {
                using (var db = new QuanLySieuThiMiniEntities())
                {
                    dsnv = new ObservableCollection<NhanVien>(db.NhanViens.ToList());
                    dscv = new ObservableCollection<ChucVu>(db.ChucVus.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nạp dữ liệu từ SQL: {ex.Message}", "Lỗi hệ thống");
            }
        }
        private bool CheckNhanVienTonTai(string maNV)
        {
            if (string.IsNullOrEmpty(maNV)) return false;
            using (var db = new QuanLySieuThiMiniEntities())
            {
                return db.NhanViens.Any(x => x.MaNV == maNV);
            }
        }
        private void ExecuteAdd()
        {
            if (string.IsNullOrWhiteSpace(SelectedNV.MaNV) || string.IsNullOrWhiteSpace(SelectedNV.TenNV))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã Nhân Viên và Họ Tên trước khi lưu!", "Thiếu thông tin");
                return;
            }

            try
            {
                using (var db = new QuanLySieuThiMiniEntities())
                {
                    if (db.NhanViens.Any(x => x.MaNV == SelectedNV.MaNV))
                    {
                        MessageBox.Show("Mã nhân viên này đã tồn tại trong SQL! Không thể thêm mới.", "Trùng lặp mã");
                        return;
                    }
                    if (db.NhanViens.Any(x => x.SDT == SelectedNV.SDT))
                    {
                        MessageBox.Show("Số điện thoại này đã có người sử dụng. Vui lòng kiểm tra lại!", "Lỗi trùng SĐT");
                        return;
                    }
                    if (db.NhanViens.Any(x => x.CCCD == SelectedNV.CCCD))
                    {
                        MessageBox.Show("Số CCCD này đã tồn tại trong hệ thống!", "Lỗi trùng CCCD");
                        return;
                    }
                    var newNV = new NhanVien
                    {
                        MaNV = SelectedNV.MaNV,
                        MaChucVu = SelectedNV.MaChucVu ?? "CV04",
                        TenNV = SelectedNV.TenNV,
                        NgaySinh = SelectedNV.NgaySinh,
                        GioiTinh = SelectedNV.GioiTinh ?? "Nam",
                        CCCD = SelectedNV.CCCD,
                        DiaChi = SelectedNV.DiaChi,
                        SDT = SelectedNV.SDT
                    };

                    db.NhanViens.Add(newNV);
                    db.SaveChanges();

                    MessageBox.Show("Thêm nhân viên mới vào SQL Server thành công!", "Thành công");

                    LoadData();
                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm dữ liệu vào SQL: {ex.Message}", "Thất bại");
            }
        }
        private void ExecuteDelete()
        {
            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa hẳn nhân viên [{SelectedNV.TenNV}]?\n\nCẢNH BÁO: Toàn bộ Hóa Đơn và Phiếu Nhập Hàng do nhân viên này lập cũng sẽ bị xóa vĩnh viễn!",
                                         "Xác nhận xóa nguy hiểm", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new QuanLySieuThiMiniEntities())
                    {
                        var target = db.NhanViens.SingleOrDefault(x => x.MaNV == SelectedNV.MaNV);
                        if (target != null)
                        {
                            var danhSachNhapHang = db.NhapHangs.Where(x => x.MaNV == target.MaNV).ToList();
                            if (danhSachNhapHang.Any())
                            {
                                db.NhapHangs.RemoveRange(danhSachNhapHang);
                            }
                            var danhSachHoaDon = db.HoaDonBanHangs.Where(x => x.MaNV == target.MaNV).ToList();
                            if (danhSachHoaDon.Any())
                            {
                                db.HoaDonBanHangs.RemoveRange(danhSachHoaDon);
                            }
                            db.NhanViens.Remove(target);
                            db.SaveChanges();

                            MessageBox.Show("Đã xóa tận gốc nhân viên và các dữ liệu liên quan!", "Thành công");

                            LoadData();
                            SelectedNV = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi bất ngờ xảy ra khi xóa: {ex.Message}", "Lỗi hệ thống");
                }
            }
        }
        private void ExecuteUpdate()
        {
            try
            {
                using (var db = new QuanLySieuThiMiniEntities())
                {
                    var target = db.NhanViens.SingleOrDefault(x => x.MaNV == SelectedNV.MaNV);
                    if (target != null)
                    {
                        target.TenNV = SelectedNV.TenNV;
                        target.NgaySinh = SelectedNV.NgaySinh;
                        target.GioiTinh = SelectedNV.GioiTinh;
                        target.SDT = SelectedNV.SDT;
                        target.CCCD = SelectedNV.CCCD;
                        target.DiaChi = SelectedNV.DiaChi;
                        target.MaChucVu = SelectedNV.MaChucVu ?? target.MaChucVu;
                        db.SaveChanges(); 

                        MessageBox.Show("Cập nhật thông tin nhân viên xuống SQL thành công!", "Thành công");
                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật SQL: {ex.Message}", "Thất bại");
            }
        }
        private void ResetForm()
        {
            SelectedNV = null;
        }
    }
}