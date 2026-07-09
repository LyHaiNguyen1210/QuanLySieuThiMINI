using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLySieuThiMINI.ViewModel;
namespace QuanLySieuThiMINI.Model
{
    public class GioHangItem : BaseViewModel
    {
        private int stt;
        private string maMH;
        private string tenMH;
        private int soLuong = 1;
        private decimal donGiaSauThue;
        private int phanTramGiam;

        public int STT
        {
            get => stt;
            set { stt = value; OnPropertyChanged(nameof(STT)); }
        }

        public string MaMH
        {
            get => maMH;
            set { maMH = value; OnPropertyChanged(nameof(MaMH)); }
        }

        public string TenMH
        {
            get => tenMH;
            set { tenMH = value; OnPropertyChanged(nameof(TenMH)); }
        }

        public int SoLuong
        {
            get => soLuong;
            set
            {
                if (value < 1) value = 1;
                soLuong = value;

                OnPropertyChanged(nameof(SoLuong));
                OnPropertyChanged(nameof(TienGoc));
                OnPropertyChanged(nameof(TienGiam));
                OnPropertyChanged(nameof(ThanhTien));
            }
        }

        public decimal DonGiaSauThue
        {
            get => donGiaSauThue;
            set
            {
                donGiaSauThue = value;

                OnPropertyChanged(nameof(DonGiaSauThue));
                OnPropertyChanged(nameof(TienGoc));
                OnPropertyChanged(nameof(TienGiam));
                OnPropertyChanged(nameof(ThanhTien));
            }
        }

        public int PhanTramGiam
        {
            get => phanTramGiam;
            set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;
                phanTramGiam = value;

                OnPropertyChanged(nameof(PhanTramGiam));
                OnPropertyChanged(nameof(TienGiam));
                OnPropertyChanged(nameof(ThanhTien));
            }
        }
        public DateTime ThoiGianTao { get; set; }
        public decimal TienGoc => SoLuong * DonGiaSauThue;
        public decimal TienGiam => TienGoc * (decimal)PhanTramGiam / 100m;

        public decimal ThanhTien => TienGoc - TienGiam;
    }
}
