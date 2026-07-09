using QuanLySieuThiMINI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLySieuThiMINI.ViewModel
{
    public class QL_KhachHang : BaseViewModel
    {
        private QuanLySieuThiMiniEntities db;
        private ObservableCollection<KhachHang> _dsKhachHang;
        public ObservableCollection<KhachHang> DSKhachHang
        {
            get => _dsKhachHang;
            set
            {
                _dsKhachHang = value;
                OnPropertyChanged();
            }
        }
        private KhachHang _khachHangTimDuoc;
        public KhachHang KhachHangTimDuoc
        {
            get => _khachHangTimDuoc;
            set
            {
                _khachHangTimDuoc = value;

                if (value != null)
                {
                    SDTTim = value.SDT;
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(CoKhachHang));
            }
        }

        private string _sdtTim;
        public string SDTTim { get => _sdtTim; set { _sdtTim = value; OnPropertyChanged(); } }
        private string _tuKhoa;
        public string TuKhoa
        {
            get => _tuKhoa;
            set { _tuKhoa = value; OnPropertyChanged(); }
        }
        public bool CoKhachHang => KhachHangTimDuoc != null;
        public RelayCommand TimKiemKH { get; set; }
        public QL_KhachHang(QuanLySieuThiMiniEntities db)
        {
            this.db = db;

            DSKhachHang = new ObservableCollection<KhachHang>();

            TimKiemKH = new RelayCommand(p => TimKiemDS());

            LoadDanhSach();
        }

        public void LoadDanhSach()
        {
            var list = db.KhachHangs
                         .Where(kh => kh.TrangThai == "Đang hoạt động")
                         .ToList();

            DSKhachHang.Clear();

            foreach (var item in list)
            {
                DSKhachHang.Add(item);
            }
        }

        public void TimKiem()
        {
            if (string.IsNullOrEmpty(SDTTim))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!");
                return;
            }
            KhachHangTimDuoc = db.KhachHangs.FirstOrDefault(x => x.SDT == SDTTim);
            if (KhachHangTimDuoc == null) MessageBox.Show("Không tìm thấy khách hàng!");
        }
        private void TimKiemDS()
        {
            if (string.IsNullOrWhiteSpace(TuKhoa))
            {
                LoadDanhSach();
                return;
            }

            var keyword = TuKhoa.Trim().ToLower();

            var filtered = db.KhachHangs
                .Where(kh => kh.TrangThai == "Đang hoạt động" &&
                            (kh.TenKH.ToLower().Contains(keyword) ||
                             kh.SDT.Contains(keyword) ||
                             (kh.Email != null && kh.Email.ToLower().Contains(keyword))))
                .OrderBy(kh => kh.MaKH)
                .ToList();

            DSKhachHang.Clear();

            foreach (var item in filtered)
            {
                DSKhachHang.Add(item);
            }

            OnPropertyChanged(nameof(DSKhachHang));
        }
    }
}