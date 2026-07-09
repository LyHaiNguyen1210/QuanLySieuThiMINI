using QuanLySieuThiMINI.Model;
using QuanLySieuThiMINI.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySieuThiMINI.ViewModel
{
    public class QL_NhanSu : BaseViewModel
    {
        QuanLySieuThiMiniEntities db = new QuanLySieuThiMiniEntities();
        public ObservableCollection<TaiKhoanDangNhap> dstk { get; set; }
        public ObservableCollection<NhanVien> dsnv { get; set; }
        public ObservableCollection<ChucVu> dscv { get; set; }

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }
        public RelayCommand LoadTK { get; set; }
        public RelayCommand LoadNV { get; set; }
        public RelayCommand LoadCV { get; set; }
        private void LoadData()
        {
            dstk = new ObservableCollection<TaiKhoanDangNhap>(db.TaiKhoanDangNhaps.ToList());
            dsnv = new ObservableCollection<NhanVien>(db.NhanViens.ToList());
            dscv = new ObservableCollection<ChucVu>(db.ChucVus.ToList());
        }
        public QL_NhanSu()
        {
            LoadData();
            LoadTK = new RelayCommand(p =>
            {
                var view = new UC_TaiKhoan();
                view.DataContext = new TaiKhoanViewModel();

                CurrentView = view;
            });
            LoadNV = new RelayCommand(p =>
            {
                var view = new UC_NhanVien();
                view.DataContext = new NhanVienViewModel();

                CurrentView = view;
            });
            LoadCV = new RelayCommand(p =>
            {
                var view = new UC_ChucVu();
                view.DataContext = new ChucVuViewModel();

                CurrentView = view;
            });
        }
        private TaiKhoanDangNhap _selectedTK;
        public TaiKhoanDangNhap SelectedTK
        {
            get => _selectedTK;
            set
            {
                _selectedTK = value;
                OnPropertyChanged();
            }
        }

        private NhanVien _selectedNV;
        public NhanVien SelectedNV
        {
            get => _selectedNV;
            set
            {
                _selectedNV = value;
                OnPropertyChanged();
            }
        }

        private ChucVu _selectedCV;
        public ChucVu SelectedCV
        {
            get => _selectedCV;
            set
            {
                _selectedCV = value;
                OnPropertyChanged();
            }
        }
    }
}
