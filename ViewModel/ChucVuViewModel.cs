using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using QuanLySieuThiMINI.Model;

namespace QuanLySieuThiMINI.ViewModel
{
    public class ChucVuViewModel : BaseViewModel
    {
        private ObservableCollection<ChucVu> _dscv;
        public ObservableCollection<ChucVu> dscv
        {
            get => _dscv;
            set { _dscv = value; OnPropertyChanged(); }
        }

        private ChucVu _selectedCV;
        public ChucVu SelectedCV
        {
            get => _selectedCV;
            set
            {
                _selectedCV = value;
                OnPropertyChanged();
                if (_selectedCV == null)
                {
                    _selectedCV = new ChucVu();
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddCommand { get; set; }

        public ChucVuViewModel()
        {
            SelectedCV = new ChucVu();
            LoadData();
            AddCommand = new RelayCommand(p => ExecuteAdd());
        }

        private void LoadData()
        {
            try
            {
                using (var db = new QuanLySieuThiMiniEntities())
                {
                    dscv = new ObservableCollection<ChucVu>(db.ChucVus.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nạp dữ liệu: {ex.Message}");
            }
        }
        private void ExecuteAdd()
        {
            if (string.IsNullOrWhiteSpace(SelectedCV.MaChucVu) || string.IsNullOrWhiteSpace(SelectedCV.TenChucVu))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã chức vụ và Tên chức vụ!", "Thiếu thông tin");
                return;
            }
            try
            {
                using (var db = new QuanLySieuThiMiniEntities())
                {
                    if (db.ChucVus.Any(x => x.MaChucVu == SelectedCV.MaChucVu))
                    {
                        MessageBox.Show("Mã chức vụ này đã tồn tại trong hệ thống!", "Lỗi trùng lặp");
                        return;
                    }
                    var newCV = new ChucVu
                    {
                        MaChucVu = SelectedCV.MaChucVu,
                        TenChucVu = SelectedCV.TenChucVu
                    };
                    db.ChucVus.Add(newCV);
                    db.SaveChanges();
                    MessageBox.Show("Thêm chức vụ mới thành công!", "Thành công");
                    LoadData();
                    SelectedCV = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi lưu vào SQL: {ex.Message}", "Thất bại");
            }
        }
    }
}