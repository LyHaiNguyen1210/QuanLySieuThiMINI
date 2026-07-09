    using QuanLySieuThiMINI.Model;
using QuanLySieuThiMINI.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLySieuThiMINI.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public QL_MatHang MatHangVM { get; set; }
        public QL_NhanSu NhanSu { get; set; }
        public QL_NhapHang NhapHangVM { get; set; }
        public QL_HoaDon HoaDonVM { get; set; }
        public QL_CanDate CanDateVM { get; set; }
        public RelayCommand ChuyenViewCommand { get; set; }
        public RelayCommand QuayLaiTrangChuCommand { get; set; }

        public MainViewModel()
        {
            MatHangVM = new QL_MatHang();
            NhanSu = new QL_NhanSu();
            NhapHangVM = new QL_NhapHang();
            HoaDonVM = QL_HoaDon.Instance;
            CanDateVM = QL_CanDate.Instance;
            VaiTro = LuuDangNhap.VaiTro;

            CurrentView = MatHangVM;

            ChuyenViewCommand = new RelayCommand(p =>
            {
                if (p != null)
                {
                    CurrentView = p;
                }
            });

            QuayLaiTrangChuCommand = new RelayCommand(p =>
            {
                TrangChuView tc = new TrangChuView();
                tc.Show();

                System.Windows.Application.Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w.DataContext == this)?
                    .Close();
            });
        }

        private string _vaiTro;
        public string VaiTro
        {
            get => _vaiTro;
            set
            {
                _vaiTro = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanQuanLyNhanSu));
            }
        }

        public bool CanQuanLyNhanSu => (VaiTro ?? "").Trim() == "Quản lý";
    }
}