# 🏪 Quản Lý Siêu Thị MINI

![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![WPF](https://img.shields.io/badge/WPF-0078D6?style=for-the-badge&logo=windows&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![VS2022](https://img.shields.io/badge/VS%202022-5C2D91?style=for-the-badge&logo=visual-studio&logoColor=white)

> 🚀 Phần mềm quản lý siêu thị mini - WPF + C# + MVVM + SQL Server

---

## ✨ Tính năng

| Chức năng | Mô tả |
|-----------|-------|
| 🔐 Đăng nhập | Xác thực người dùng, phân quyền |
| 📦 Quản lý mặt hàng | Thêm, sửa, xóa, tìm kiếm sản phẩm |
| 👥 Quản lý nhân viên | Quản lý thông tin nhân viên |
| 🧾 Quản lý hóa đơn | Tạo và quản lý hóa đơn bán hàng |
| 📊 Quản lý nhập hàng | Nhập kho, quản lý tồn kho |
| 💰 Bán hàng | Giao diện bán hàng nhanh |
| 📈 Báo cáo thống kê | Doanh thu, tồn kho |
| 🖨️ In hóa đơn | Xuất báo cáo RDLC |

---

## 🛠️ Công nghệ

- .NET Framework 4.7.2
- Visual Studio 2022
- WPF + MVVM
- SQL Server 2019
- Entity Framework
- RDLC Report

---

## 📁 Cấu trúc thư mục

```
QuanLySieuThiMINI/
├── Model/              # Entities + Database
├── View/               # Giao diện XAML
├── ViewModel/          # Logic nghiệp vụ
├── Images/             # Ảnh, icon
├── Report/             # Báo cáo RDLC
├── Database/           # Script SQL (chứa file .sql)
├── Properties/         # Cấu hình
├── App.config          # Connection string
└── README.md           # Hướng dẫn
```

---

## 🗄️ Database

Database `QuanLySieuThiMini` gồm 14 bảng:

| Bảng | Mô tả |
|------|-------|
| `ChucVu` | Chức vụ nhân viên |
| `NhanVien` | Thông tin nhân viên |
| `KhachHang` | Thông tin khách hàng |
| `MatHang` | Quản lý mặt hàng (100 sản phẩm) |
| `HoaDonTam` | Hóa đơn tạm thời |
| `HoaDonBanHang` | Hóa đơn bán hàng |
| `ChiTietBanHang` | Chi tiết hóa đơn |
| `NhaCungCap` | Nhà cung cấp |
| `NhapHang` | Phiếu nhập hàng |
| `ChiTietNhapHang` | Chi tiết nhập hàng |
| `HuyHang` | Hàng bị hủy |
| `HangSapHetHan` | Hàng sắp hết hạn |
| `TaiKhoanDangNhap` | Tài khoản đăng nhập |
| `ChiTietHoaDonTam` | Chi tiết hóa đơn tạm |

> 📄 Script database có sẵn trong thư mục `Database/QuanLySieuThiMini.sql`

---

## 🚀 Hướng dẫn clone và chạy

### 1. Clone repository

```bash
git clone https://github.com/LyHaiNguyen1210/QuanLySieuThiMINI.git
cd QuanLySieuThiMINI
```

### 2. Mở project

Double-click file **`QuanLySieuThiMINI.slnx`** để mở bằng Visual Studio 2022.

### 3. Restore NuGet

**Tools → NuGet Package Manager → Restore Packages**

### 4. Tạo database

- Mở **SQL Server Management Studio (SSMS)**
- Kết nối SQL Server
- Mở file `Database/QuanLySieuThiMini.sql`
- Nhấn **F5** chạy script

### 5. Cập nhật Connection String (nếu cần)

Sửa file `App.config` theo cấu hình SQL Server của bạn:

```xml
<connectionStrings>
    <add name="QuanLySieuThiMiniEntities" 
         connectionString="metadata=res://*/Model.QLST.csdl|res://*/Model.QLST.ssdl|res://*/Model.QLST.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=DESKTOP-KSHJGVN\SQLEXPRESS;initial catalog=QuanLySieuThiMini;persist security info=True;user id=sa;password=123;trustservercertificate=True;MultipleActiveResultSets=True;App=EntityFramework&quot;"
         providerName="System.Data.EntityClient" />
</connectionStrings>
```

### 6. Chạy chương trình

Nhấn **F5** trong Visual Studio 2022.

---

## 👤 Tài khoản đăng nhập

| Username | Password | Vai trò |
|----------|----------|---------|
| `admin` | `123` | Quản lý |
| `thungan` | `123` | Thu ngân |
| `kho` | `123` | Kho |
| `banhang` | `123` | Bán hàng |

---


⭐ **Star ủng hộ nếu thấy hữu ích!** ⭐