using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using QuanLySieuThiMINI.Model;

namespace QuanLySieuThiMINI.ViewModel
{
    public class TaiKhoanViewModel : BaseViewModel
    {
        private ObservableCollection<TaiKhoanDangNhap> _dstk;
        public ObservableCollection<TaiKhoanDangNhap> dstk
        {
            get => _dstk;
            set { _dstk = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ChucVu> _dscv;
        public ObservableCollection<ChucVu> dscv
        {
            get => _dscv;
            set { _dscv = value; OnPropertyChanged(); }
        }

        private TaiKhoanDangNhap _selectedTK;
        public TaiKhoanDangNhap SelectedTK
        {
            get => _selectedTK;
            set
            {
                _selectedTK = value;
                OnPropertyChanged();

                if (_selectedTK == null)
                {
                    _selectedTK = new TaiKhoanDangNhap();
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public TaiKhoanViewModel()
        {
            SelectedTK = new TaiKhoanDangNhap();
            LoadData();

            SaveCommand = new RelayCommand(p => ExecuteSave());
            DeleteCommand = new RelayCommand(p => ExecuteDelete(), p => SelectedTK != null && !string.IsNullOrEmpty(SelectedTK.TenDangNhap));
        }

        private void LoadData()
        {
            try
            {
                using (var db = new QuanLySieuThiMiniEntities())
                {
                    dstk = new ObservableCollection<TaiKhoanDangNhap>(db.TaiKhoanDangNhaps.ToList());
                    dscv = new ObservableCollection<ChucVu>(db.ChucVus.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nạp dữ liệu: {ex.Message}");
            }
        }

        private void ExecuteSave()
        {
            if (string.IsNullOrWhiteSpace(SelectedTK.TenDangNhap) || string.IsNullOrWhiteSpace(SelectedTK.MaNV))
            {
                MessageBox.Show("Tên đăng nhập và Mã Nhân Viên không được để trống!", "Cảnh báo");
                return;
            }

            try
            {
                using (var db = new QuanLySieuThiMiniEntities())
                {
                    if (!db.NhanViens.Any(nv => nv.MaNV == SelectedTK.MaNV))
                    {
                        MessageBox.Show($"Mã nhân viên '{SelectedTK.MaNV}' không tồn tại. Vui lòng nhập đúng Mã NV!", "Lỗi ràng buộc");
                        return;
                    }
                    var target = db.TaiKhoanDangNhaps.SingleOrDefault(x => x.TenDangNhap == SelectedTK.TenDangNhap);

                    if (target != null)
                    {
                        target.MatKhau = SelectedTK.MatKhau;
                        target.Email = SelectedTK.Email;
                        target.VaiTro = SelectedTK.VaiTro;
                        target.MaNV = SelectedTK.MaNV;

                        db.SaveChanges();
                        MessageBox.Show("Cập nhật thông tin tài khoản thành công!", "Thành công");
                    }
                    else
                    {
                        var newTK = new TaiKhoanDangNhap
                        {
                            TenDangNhap = SelectedTK.TenDangNhap,
                            MatKhau = SelectedTK.MatKhau ?? "123",
                            Email = SelectedTK.Email ?? "",
                            VaiTro = SelectedTK.VaiTro ?? "Bán hàng",
                            MaNV = SelectedTK.MaNV
                        };

                        db.TaiKhoanDangNhaps.Add(newTK);
                        db.SaveChanges();
                        MessageBox.Show("Tạo tài khoản mới thành công!", "Thành công");
                    }

                    LoadData();
                    SelectedTK = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.Message}", "Thất bại");
            }
        }

        private void ExecuteDelete()
        {
            if (string.IsNullOrWhiteSpace(SelectedTK.TenDangNhap)) return;

            if (MessageBox.Show($"Xóa tài khoản '{SelectedTK.TenDangNhap}'?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var db = new QuanLySieuThiMiniEntities())
                    {
                        var target = db.TaiKhoanDangNhaps.SingleOrDefault(x => x.TenDangNhap == SelectedTK.TenDangNhap);
                        if (target != null)
                        {
                            db.TaiKhoanDangNhaps.Remove(target);
                            db.SaveChanges();
                            MessageBox.Show("Đã xóa tài khoản!");
                            LoadData();
                            SelectedTK = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể xóa: {ex.Message}");
                }
            }
        }
    }
}