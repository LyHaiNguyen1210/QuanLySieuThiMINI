using QuanLySieuThiMINI.Model;
using QuanLySieuThiMINI.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLySieuThiMINI.ViewModel
{
    public class TatCaMatHangViewModel : BaseViewModel
    {
        QuanLySieuThiMiniEntities db = new QuanLySieuThiMiniEntities();

        private ObservableCollection<MatHang> _dsMH;
        public ObservableCollection<MatHang> dsMH
        {
            get { return _dsMH; }
            set
            {
                _dsMH = value;
                OnPropertyChanged(nameof(dsMH));
            }
        }
        private ObservableCollection<NhapHang> _dsNH;
        public ObservableCollection<NhapHang> dsNH
        {
            get => _dsNH;
            set
            {
                _dsNH = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand LoadAllCommand { get; set; }
        public RelayCommand LocHoaMyPham { get; set; }
        public RelayCommand LocThucPhamKho { get; set; }
        public RelayCommand LocBanhKeo { get; set; }
        public RelayCommand LocThitCaHaiSan { get; set; }
        public RelayCommand LocRauCuQua { get; set; }
        public RelayCommand LocMiChao { get; set; }
        public RelayCommand LocSuaNuoc { get; set; }
        public RelayCommand LocGiaDung { get; set; }
        public RelayCommand LocVanPhongPham { get; set; }
        public RelayCommand NhapKho { get; set; }
        public RelayCommand KiemDate { get; set; }
        public TatCaMatHangViewModel()
        {
            dsMH = new ObservableCollection<MatHang>(db.MatHangs.ToList());
            LoadAllCommand = new RelayCommand(p =>
            {
                dsMH = new ObservableCollection<MatHang>(
                   db.MatHangs
                   .AsNoTracking()
                   .ToList()
                   .OrderBy(x =>
                   {
                       if (x.MaMH != null && x.MaMH.Length > 2 && int.TryParse(x.MaMH.Substring(2), out int num))
                           return num;
                       return int.MaxValue;
                   })
                   .ToList()
               );
                var view = new UC_ChiTietMatHang();
                view.DataContext = this;
                CurrentView = view;
            });

            LocThitCaHaiSan = new RelayCommand(p =>
            {
                dsMH = new ObservableCollection<MatHang>(
                    db.MatHangs.AsNoTracking().Where(x => x.Barcode == "88888888").ToList()
                );
                var view = new UC_ThitCa();
                view.DataContext = this;  
                CurrentView = view;
            });
            LocSuaNuoc = new RelayCommand(p =>
            {
                dsMH = new ObservableCollection<MatHang>(
                    db.MatHangs.AsNoTracking().Where(x => x.Barcode == "55555555"||x.Barcode == "44444444").ToList()
                );
                var view = new UC_SuaNuocGiaiKhat();
                view.DataContext = this;
                CurrentView = view;

            });
            LocMiChao = new RelayCommand(p =>
            {
                dsMH = new ObservableCollection<MatHang>(
                    db.MatHangs.AsNoTracking().Where(x => x.Barcode == "77777777").ToList()
                );
                var view = new UC_MiChao();
                view.DataContext = this;
                CurrentView = view;

            });
            LocHoaMyPham = new RelayCommand(p =>
            {
                dsMH = new ObservableCollection<MatHang>(
                    db.MatHangs.AsNoTracking()
                    .Where(x => x.Barcode == "11111111")
                    .ToList()
                );

                var view = new UC_HoaMyPham();
                view.DataContext = this;
                CurrentView = view;
            });

            LocThucPhamKho = new RelayCommand(p =>
            {
                dsMH = new ObservableCollection<MatHang>(
                    db.MatHangs.AsNoTracking()
                    .Where(x => x.Barcode == "22222222")
                    .ToList()
                );

                var view = new UC_ThucPhamKho();
                view.DataContext = this;
                CurrentView = view;
            });

            LocBanhKeo = new RelayCommand(p =>
            {
                dsMH = new ObservableCollection<MatHang>(
                    db.MatHangs.AsNoTracking()
                    .Where(x => x.Barcode == "33333333")
                    .ToList()
                );

                var view = new UC_BanhKeo();
                view.DataContext = this;
                CurrentView = view;
            });

            LocRauCuQua = new RelayCommand(p =>
            {
                dsMH = new ObservableCollection<MatHang>(
                    db.MatHangs.AsNoTracking()
                    .Where(x => x.Barcode == "99999999")
                    .ToList()
                );

                var view = new UC_RauCuQua();
                view.DataContext = this;
                CurrentView = view;
            });

            LocGiaDung = new RelayCommand(p =>
            {
                dsMH = new ObservableCollection<MatHang>(
                    db.MatHangs.AsNoTracking()
                    .Where(x => x.Barcode == "66666666")
                    .ToList()
                );

                var view = new UC_GiaDung();
                view.DataContext = this;
                CurrentView = view;
            });

            LocVanPhongPham = new RelayCommand(p =>
            {
                dsMH = new ObservableCollection<MatHang>(
                    db.MatHangs.AsNoTracking().Where(x => x.Barcode == "00000000").ToList().OrderBy(x =>
                    {
                        if (x.MaMH != null && x.MaMH.Length > 2 && int.TryParse(x.MaMH.Substring(2), out int num))
                            return num;
                        return int.MaxValue;
                    }).ToList()
                );

                var view = new UC_VanPhongPham();
                view.DataContext = this;
                CurrentView = view;
            });
            NhapKho = new RelayCommand(p =>
            {
                dsNH = new ObservableCollection<NhapHang>(db.NhapHangs.ToList());
                dsNCC = new ObservableCollection<NhaCungCap>(db.NhaCungCaps.ToList());
                SelectedNH = dsNH.FirstOrDefault();
                var view = new UC_NhapKho();
                view.DataContext = this;
                CurrentView = view;
            });
            KiemDate = new RelayCommand(p =>
            {
                dsNH = new ObservableCollection<NhapHang>(
                    db.NhapHangs.ToList()
                );
                var view = new UC_CanDate();
                view.DataContext = this;
                CurrentView = view;
            });
        }
        private MatHang selectedMH;
        public MatHang SelectedMH
        {
            get => selectedMH;
            set
            {
                selectedMH = value;
                OnPropertyChanged();
            }
        }
        private NhapHang selectedNH;
        public NhapHang SelectedNH
        {
            get => selectedNH;
            set
            {
                selectedNH = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<NhaCungCap> dsNCC { get; set; }
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }
    }
}
