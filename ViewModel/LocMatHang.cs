using QuanLySieuThiMINI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySieuThiMINI.ViewModel
{
    public class LocMatHang:BaseViewModel
    {
        QuanLySieuThiMiniEntities db = new QuanLySieuThiMiniEntities();

        private ObservableCollection<MatHang> _dsMH;
        public ObservableCollection<MatHang> dsMH
        {
            get => _dsMH;
            set { _dsMH = value; OnPropertyChanged(); }
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
        public LocMatHang()
        {
            dsMH = new ObservableCollection<MatHang>();

            LoadAllCommand = new RelayCommand(p => LoadAll());
            LocHoaMyPham = new RelayCommand(p => Loc("11111111"));
            LocThucPhamKho = new RelayCommand(p => Loc("22222222"));
            LocBanhKeo = new RelayCommand(p => Loc("33333333"));
            LocMiChao = new RelayCommand(p => Loc("77777777"));
            LocThitCaHaiSan = new RelayCommand(p => Loc("88888888"));
            LocRauCuQua = new RelayCommand(p => Loc("99999999"));
            LocGiaDung = new RelayCommand(p => Loc("66666666"));
            LocVanPhongPham = new RelayCommand(p => LoadVanPhongPham());
            LocSuaNuoc = new RelayCommand(p => LocSua());
            LoadAll();

        }

        public void LoadAll()
        {
            dsMH = new ObservableCollection<MatHang>(
                db.MatHangs.AsNoTracking().ToList().OrderBy(x =>
                {
                    if (x.MaMH != null && x.MaMH.Length > 2 && int.TryParse(x.MaMH.Substring(2), out int num))
                        return num;
                    return int.MaxValue;
                }).ToList()
            );
        }

        public void Loc(string barcode)
        {
            dsMH = new ObservableCollection<MatHang>(
                db.MatHangs.AsNoTracking().Where(x => x.Barcode == barcode).ToList()
            );
        }

        private void LoadVanPhongPham()
        {
            dsMH = new ObservableCollection<MatHang>(
                db.MatHangs.AsNoTracking()
                .Where(x => x.Barcode == "00000000")
                .ToList()
                .OrderBy(x =>
                {
                    if (x.MaMH != null && x.MaMH.Length > 2 && int.TryParse(x.MaMH.Substring(2), out int num))
                        return num;
                    return int.MaxValue;
                }).ToList()
            );
        }

        private void LocSua()
        {
            dsMH = new ObservableCollection<MatHang>(
                db.MatHangs.AsNoTracking()
                .Where(x => x.Barcode == "55555555" || x.Barcode == "44444444")
                .ToList()
            );
        }
    }
}

