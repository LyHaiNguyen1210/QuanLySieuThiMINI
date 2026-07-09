# 🏪 Quản Lý Siêu Thị MINI

[![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![WPF](https://img.shields.io/badge/WPF-0078D6?style=for-the-badge&logo=windows&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/en-us/sql-server/)

> Phần mềm quản lý siêu thị mini được xây dựng bằng **WPF** và **C#** với kiến trúc **MVVM**.

---

## ✨ Tính năng nổi bật

- 🔐 **Đăng nhập/Đăng xuất** an toàn
- 📦 **Quản lý sản phẩm** (Thêm, Sửa, Xóa, Tìm kiếm)
- 👥 **Quản lý nhân viên** với phân quyền
- 🧾 **Quản lý hóa đơn** và bán hàng
- 📊 **Báo cáo thống kê** doanh thu, tồn kho
- 💾 **Kết nối SQL Server** lưu trữ dữ liệu
- 🌓 **Giao diện hiện đại**, thân thiện

---

## 🛠️ Công nghệ sử dụng

| Công nghệ | Mô tả |
|-----------|-------|
| **.NET Framework 4.7.2** | Nền tảng phát triển |
| **WPF** | Xây dựng giao diện |
| **MVVM Pattern** | Kiến trúc phân tách logic |
| **SQL Server** | Quản lý cơ sở dữ liệu |
| **Entity Framework** | ORM kết nối database |

---

## 📁 Cấu trúc dự án

```
QuanLySieuThiMINI/
├── 📂 Model/          # Tầng dữ liệu (Entities, Database)
├── 📂 View/           # Tầng giao diện (Windows, UserControls)
├── 📂 ViewModel/      # Tầng logic nghiệp vụ (MVVM)
├── 📂 Images/         # Hình ảnh, icon
├── 📂 Report/         # Báo cáo, in ấn
├── 📂 Properties/     # Cấu hình dự án
├── 📄 App.xaml        # Cấu hình ứng dụng
├── 📄 packages.config # NuGet packages
└── 📄 README.md       # Tài liệu dự án
```

---

## 🚀 Hướng dẫn cài đặt

### Yêu cầu hệ thống
- Windows 7/8/10/11
- [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework)
- [SQL Server 2019+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2019+](https://visualstudio.microsoft.com/)

### Các bước chạy project

**1️⃣ Clone repository**
```bash
git clone https://github.com/LyHaiNguyen1210/QuanLySieuThiMINI.git
cd QuanLySieuThiMINI
```

**2️⃣ Mở solution bằng Visual Studio**
```bash
start QuanLySieuThiMINI.slnx
```

**3️⃣ Khôi phục NuGet packages**
```bash
dotnet restore
```

**4️⃣ Cập nhật connection string**  
Sửa file `App.config` với thông tin SQL Server của bạn

**5️⃣ Chạy ứng dụng**
```bash
dotnet run --project QuanLySieuThiMINI.csproj
```
Hoặc nhấn **F5** trong Visual Studio.

---

## 👤 Tài khoản đăng nhập mặc định

| Username | Password | Role |
|----------|----------|------|
| `admin` | `123456` | Quản trị viên |
| `staff` | `123456` | Nhân viên |

---

## 📧 Liên hệ

- **GitHub**: [@LyHaiNguyen1210](https://github.com/LyHaiNguyen1210)

---

⭐ **Nếu bạn thấy dự án hữu ích, hãy để lại một star nhé!** ⭐
