using Microsoft.Win32;
using QuanLySieuThiMINI.Model;
using QuanLySieuThiMINI.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLySieuThiMINI.ViewModel
{
    public class QL_MatHang : BaseViewModel
    {
        private QuanLySieuThiMiniEntities _db = new QuanLySieuThiMiniEntities();

        private string _maMH;
        public string MaMH { get => _maMH; set { _maMH = value; OnPropertyChanged(nameof(MaMH)); } }

        private string _tenMH;
        public string TenMH { get => _tenMH; set { _tenMH = value; OnPropertyChanged(nameof(TenMH)); } }

        private int _giaNhap;
        public int GiaNhap { get => _giaNhap; set { _giaNhap = value; OnPropertyChanged(nameof(GiaNhap)); } }

        private int _donGia;
        public int DonGia { get => _donGia; set { _donGia = value; OnPropertyChanged(nameof(DonGia)); TinhGiaBanCuoi(); } }

        private double _vat = 0.10;
        public double VAT { get => _vat; set { _vat = value; OnPropertyChanged(nameof(VAT)); TinhGiaBanCuoi(); } }

        private int _giaBanCuoi;
        public int GiaBanCuoi { get => _giaBanCuoi; set { _giaBanCuoi = value; OnPropertyChanged(nameof(GiaBanCuoi)); } }

        private string _nhomHang;
        public string NhomHang { get => _nhomHang; set { _nhomHang = value; OnPropertyChanged(nameof(NhomHang)); } }

        private string _dvt;
        public string DVT { get => _dvt; set { _dvt = value; OnPropertyChanged(nameof(DVT)); } }

        private string _hinhAnhRaw;
        public string HinhAnhRaw { get => _hinhAnhRaw; set { _hinhAnhRaw = value; OnPropertyChanged(nameof(HinhAnhRaw)); } }

        private ObservableCollection<MatHang> _dsMH;
        public ObservableCollection<MatHang> dsMH
        {
            get => _dsMH;
            set { _dsMH = value; OnPropertyChanged(nameof(dsMH)); }
        }

        private ObservableCollection<string> _danhSachNhomHang;
        public ObservableCollection<string> DanhSachNhomHang { get => _danhSachNhomHang; set { _danhSachNhomHang = value; OnPropertyChanged(nameof(DanhSachNhomHang)); } }

        private MatHang _selectedItem;
        public MatHang SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                if (_selectedItem != null)
                {
                    MaMH = _selectedItem.MaMH;
                    TenMH = _selectedItem.TenMH;
                    GiaNhap = Convert.ToInt32(_selectedItem.GiaNhap);
                    DonGia = Convert.ToInt32(_selectedItem.DonGia);
                    VAT = Convert.ToDouble(_selectedItem.VAT);
                    NhomHang = _selectedItem.Nhom;
                    DVT = _selectedItem.DonViTinh;
                    HinhAnhRaw = _selectedItem.HinhAnh;
                }
            }
        }

        private ObservableCollection<NhapHang> _dsNH;
        public ObservableCollection<NhapHang> dsNH
        {
            get => _dsNH;
            set { _dsNH = value; OnPropertyChanged(); }
        }

        public ICommand ChonAnhCommand { get; set; }
        public ICommand ThemCommand { get; set; }
        public ICommand SuaCommand { get; set; }
        public ICommand XoaCommand { get; set; }
        public ICommand LamMoiCommand { get; set; }

        public QL_MatHang()
        {
            LoadData();

            ChonAnhCommand = new RelayCommand((p) => {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Hình ảnh (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";
                if (openFileDialog.ShowDialog() == true)
                {
                    HinhAnhRaw = openFileDialog.FileName;
                }
            });

            ThemCommand = new RelayCommand((p) => {
                if (string.IsNullOrWhiteSpace(MaMH) || string.IsNullOrWhiteSpace(TenMH))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ Mã và Tên mặt hàng", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrWhiteSpace(HinhAnhRaw))
                {
                    MessageBox.Show("Vui lòng chọn hình ảnh cho mặt hàng trước khi thêm", "Cảnh báo thiếu ảnh", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string checkMa = MaMH.Trim();
                if (_db.MatHangs.Any(x => x.MaMH == checkMa))
                {
                    MessageBox.Show($"Mã mặt hàng '{checkMa}' đã tồn tại trong hệ thống", "Lỗi trùng mã", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var mhMoi = new MatHang()
                {
                    MaMH = checkMa,
                    TenMH = TenMH.Trim(),
                    Nhom = NhomHang?.Trim() ?? "Khác",
                    DonViTinh = DVT ?? "Cái",
                    GiaNhap = GiaNhap,
                    DonGia = DonGia,
                    VAT = VAT,
                    SoLuong = 0,
                    HinhAnh = HinhAnhRaw
                };

                _db.MatHangs.Add(mhMoi);
                _db.SaveChanges();
                LoadData();
                LamMoiForm();
                MessageBox.Show("Đã thêm mới mặt hàng thành công", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            SuaCommand = new RelayCommand((p) => {
                if (SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn một mặt hàng từ danh sách để cập nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (MaMH.Trim() != SelectedItem.MaMH.Trim())
                {
                    MessageBox.Show("Không được phép thay đổi Mã mặt hàng", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    MaMH = SelectedItem.MaMH;
                    return;
                }

                var mh = _db.MatHangs.FirstOrDefault(x => x.MaMH == SelectedItem.MaMH);
                if (mh != null)
                {
                    mh.TenMH = TenMH.Trim();
                    mh.GiaNhap = GiaNhap;
                    mh.DonGia = DonGia;
                    mh.VAT = VAT;
                    mh.Nhom = NhomHang?.Trim();
                    mh.DonViTinh = DVT;
                    mh.HinhAnh = HinhAnhRaw;

                    _db.SaveChanges();
                    LoadData();
                    LamMoiForm();
                    MessageBox.Show("Cập nhật thông tin mặt hàng thành công", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            });

            XoaCommand = new RelayCommand((p) => {
                if (SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn một mặt hàng từ danh sách cần xóa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn xóa mặt hàng này?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var mh = _db.MatHangs.FirstOrDefault(x => x.MaMH == SelectedItem.MaMH);
                        if (mh != null)
                        {
                            _db.MatHangs.Remove(mh);
                            _db.SaveChanges();
                            LoadData();
                            LamMoiForm();
                            MessageBox.Show("Xóa mặt hàng thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Không thể xóa sản phẩm này vì đã có lịch sử liên kết với hóa đơn hoặc phiếu nhập kho", "Lỗi ràng buộc CSDL", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });

            LamMoiCommand = new RelayCommand((p) => {
                LamMoiForm();
            });
        }

        private void LoadData()
        {
            if (_db == null) return;
            dsMH = new ObservableCollection<MatHang>(_db.MatHangs.ToList());

            var dsNhomTuMatHang = _db.MatHangs.Where(m => !string.IsNullOrEmpty(m.Nhom)).Select(m => m.Nhom).ToList();
            var dsNhomTuNCC = _db.NhaCungCaps.Where(n => n.TrangThai == "Đang hoạt động" && !string.IsNullOrEmpty(n.LinhVuc)).Select(n => n.LinhVuc).ToList();
            var dsGopChung = dsNhomTuMatHang.Concat(dsNhomTuNCC).Distinct().OrderBy(x => x).ToList();

            DanhSachNhomHang = new ObservableCollection<string>(dsGopChung);
        }

        private void TinhGiaBanCuoi()
        {
            GiaBanCuoi = (int)Math.Round(DonGia * (1 + VAT));
        }

        private void LamMoiForm()
        {
            SelectedItem = null;
            MaMH = string.Empty;
            TenMH = string.Empty;
            GiaNhap = 0;
            DonGia = 0;
            VAT = 0.10;
            NhomHang = string.Empty;
            DVT = string.Empty;
            HinhAnhRaw = null;
        }
    }
}