using QuanLySieuThiMINI.Model;
using QuanLySieuThiMINI.View;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System;

namespace QuanLySieuThiMINI.ViewModel
{
    public class DangNhapViewModel : BaseViewModel
    {
        private QuanLySieuThiMiniEntities db = new QuanLySieuThiMiniEntities();

        private string _maNV;
        public string MaNV
        {
            get => _maNV;
            set { _maNV = value; OnPropertyChanged(); }
        }

        private string _thongBao;
        public string ThongBao
        {
            get => _thongBao;
            set { _thongBao = value; OnPropertyChanged(); }
        }

        public ICommand DangNhapCommand { get; set; }
        public ICommand MoQuenMatKhauCommand { get; set; }

        public DangNhapViewModel()
        {
            DangNhapCommand = new RelayCommand(p => DangNhap(p));
        }

        private void DangNhap(object p)
        {
            ThongBao = "";

            if (string.IsNullOrWhiteSpace(MaNV))
            {
                ThongBao = "Tên đăng nhập không được để trống.";
                return;
            }
            var pwb = p as PasswordBox;
            string matKhau = pwb?.Password;

            if (string.IsNullOrWhiteSpace(matKhau))
            {
                ThongBao = "Mật khẩu không được để trống.";
                return;
            }

            var nhanVien = db.TaiKhoanDangNhaps.FirstOrDefault(x => x.TenDangNhap == MaNV && x.MatKhau == matKhau);

            if (nhanVien != null)
            {
                ThongBao = "Đăng nhập thành công";
                var nv=db.NhanViens.FirstOrDefault(x=>x.MaNV==nhanVien.MaNV);
                if (nv == null)
                {
                    ThongBao = "Tài khoản không liên kết nhân viên!";
                    return;
                }
                LuuDangNhap.MaNV = nhanVien.MaNV;
                LuuDangNhap.TenNhanVien = nv.TenNV;
                LuuDangNhap.VaiTro = nhanVien.VaiTro;

                TrangChuView main = new TrangChuView();
                main.Show();

                Application.Current.Windows.OfType<DangNhapView>().FirstOrDefault()?.Close();
            }
            else
            {
                ThongBao = "Sai tên đăng nhập hoặc mật khẩu. Vui lòng nhập lại";
            }
        }
    }
}