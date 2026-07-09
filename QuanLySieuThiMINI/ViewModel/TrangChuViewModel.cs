using QuanLySieuThiMINI.View;
using System.Windows;
using System.Windows.Input;

namespace QuanLySieuThiMINI.ViewModel
{
    public class TrangChuViewModel : BaseViewModel
    {
        public ICommand MoTrangBanHangCommand { get; set; }
        public ICommand MoTrangMatHangCommand { get; set; }

        public TrangChuViewModel()
        {
            TenNhanVien=LuuDangNhap.TenNhanVien;
            VaiTro=LuuDangNhap.VaiTro;  
            MoTrangBanHangCommand = new RelayCommand(p =>
            {
                QuanLySieuThiMINIView view = new QuanLySieuThiMINIView();
                view.Show();
                Window currentWindow = p as Window;
                currentWindow?.Close();
            });
            MoTrangMatHangCommand = new RelayCommand(p =>
            {
                QuanLyMatHangView view = new QuanLyMatHangView();
                view.Show();
                Window currentWindow = p as Window;
                currentWindow?.Close();
            });
        }
        private string _tenNhanVien;
        public string TenNhanVien
        {
            get => _tenNhanVien;
            set { _tenNhanVien = value; OnPropertyChanged(); }
        }

        private string _vaiTro;
        public string VaiTro
        {
            get => _vaiTro;
            set
            {
                _vaiTro = value;
                OnPropertyChanged();
                CapNhatQuyen();
            }
        }
        public bool IsQuanLy => (VaiTro ?? "").Trim() == "Quản lý";
        public bool IsThuNgan => (VaiTro ?? "").Trim() == "Thu ngân";
        public bool IsKho => (VaiTro ?? "").Trim() == "Kho";
        public bool IsBanHang => (VaiTro ?? "").Trim() == "Bán hàng";
        public bool CanVaoBanHang => IsQuanLy || IsThuNgan || IsBanHang;
        public bool CanVaoQuanLy => IsQuanLy || IsKho;
        private void CapNhatQuyen()
        {
            OnPropertyChanged(nameof(CanVaoBanHang));
            OnPropertyChanged(nameof(CanVaoQuanLy));
        }
    }
}