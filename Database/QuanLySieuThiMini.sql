
CREATE DATABASE QuanLySieuThiMini;
GO
USE QuanLySieuThiMini;
GO

-------------------------------------------------------
-- 1. BẢNG CHỨC VỤ
-------------------------------------------------------
CREATE TABLE ChucVu
(
    MaChucVu VARCHAR(50) PRIMARY KEY,
    TenChucVu NVARCHAR(50) NOT NULL
);
GO

INSERT INTO ChucVu (MaChucVu, TenChucVu) VALUES
('CV01', N'Quản lý'),
('CV02', N'Thu ngân'),
('CV03', N'Kho'),
('CV04', N'Bán hàng');
GO


-------------------------------------------------------
-- 2. BẢNG NHÂN VIÊN
-------------------------------------------------------
CREATE TABLE NhanVien
(
    MaNV VARCHAR(50) PRIMARY KEY NOT NULL,
    MaChucVu VARCHAR(50) NOT NULL,
    TenNV NVARCHAR(50) NOT NULL,
    NgaySinh DATE NOT NULL,
    GioiTinh NVARCHAR(10) NOT NULL,
    CCCD VARCHAR(20) NOT NULL UNIQUE,
    DiaChi NVARCHAR(100) NOT NULL,
    SDT VARCHAR(20) NOT NULL UNIQUE,
    CONSTRAINT FK_NhanVien_ChucVu FOREIGN KEY (MaChucVu) REFERENCES ChucVu(MaChucVu)
);
GO

INSERT INTO NhanVien (MaNV, MaChucVu, TenNV, NgaySinh, GioiTinh, CCCD, DiaChi, SDT) VALUES
('NV01', 'CV01', N'Nguyễn Văn A', '2000-01-10', N'Nam', '111', N'HCM', '0901'),
('NV02', 'CV02', N'Trần Thị B', '2001-02-20', N'Nữ', '222', N'HCM', '0902'),
('NV03', 'CV03', N'Lê Văn C', '1999-03-15', N'Nam', '333', N'BD', '0903'),
('NV04', 'CV04', N'Phạm Thị D', '2002-05-05', N'Nữ', '444', N'DN', '0904'),
('NV05', 'CV04', N'Đặng Văn E', '2001-11-11', N'Nam', '555', N'HCM', '0905');
GO


-------------------------------------------------------
-- 3. BẢNG KHÁCH HÀNG (ĐÃ SỬA NOT NULL EMAIL)
-------------------------------------------------------
CREATE TABLE KhachHang
(
    MaKH VARCHAR(50) PRIMARY KEY,
    TenKH NVARCHAR(50) NOT NULL,
    NgaySinh DATE NOT NULL,
    GioiTinh NVARCHAR(10) NOT NULL,
    DiaChi NVARCHAR(100) NOT NULL,
    SDT VARCHAR(20) NOT NULL UNIQUE,
    Email VARCHAR(100) NOT NULL UNIQUE, 
    DiemThuong INT NOT NULL DEFAULT 0 CHECK (DiemThuong >= 0),
    TrangThai NVARCHAR(50) NOT NULL DEFAULT N'Đang hoạt động'
);
GO

INSERT INTO KhachHang (MaKH, TenKH, NgaySinh, GioiTinh, DiaChi, SDT, Email, DiemThuong) VALUES
('KH01', N'Lý Hải Nguyên', '2003-10-10', N'Nam', N'Hồ Chí Minh', '0786925518', 'hainguyen@gmail.com', 10),
('KH02', N'Huỳnh Thành Lưu', '2000-08-08', N'Nữ', N'Hồ Chí Minh', '0922000002', 'thanhluu@gmail.com', 20),
('KH03', N'Nguyễn Hồ Sĩ Nguyễn', '1998-07-07', N'Nam', N'Bình Dương', '0933000003', 'singuyen@gmail.com', 5),
('KH04', N'Nguyễn Quốc Việt', '2002-06-06', N'Nữ', N'Đà Nẵng', '0944000004', 'quocviet@gmail.com', 0),
('KH05', N'Phan Tâm Nguyên', '1997-03-03', N'Nam', N'Hồ Chí Minh', '0955000005', 'tamnguyen@gmail.com', 15);
GO


-------------------------------------------------------
-- 4. BẢNG MẶT HÀNG
-------------------------------------------------------
CREATE TABLE MatHang
(
    MaMH VARCHAR(50) PRIMARY KEY NOT NULL,
    Nhom NVARCHAR(100) NOT NULL,
    TenMH NVARCHAR(50) NOT NULL,
    DonViTinh NVARCHAR(20) NOT NULL,
    GiaNhap INT NOT NULL CHECK (GiaNhap >= 0),
    DonGia INT NOT NULL CHECK (DonGia >= 0),
    VAT FLOAT NOT NULL DEFAULT 0.1,
    SoLuong INT NOT NULL CHECK (SoLuong >= 0),
    DonGiaSauThue AS (CAST(DonGia * (1 + VAT) AS INT)), 
    ViTriQuay NVARCHAR(50),
    HinhAnh NVARCHAR(255),
    Barcode VARCHAR(50)
);
GO

INSERT INTO MatHang (MaMH, Nhom, TenMH, DonViTinh, GiaNhap, DonGia, VAT, SoLuong, ViTriQuay, HinhAnh, Barcode) VALUES
('MH01', N'Hóa mỹ phẩm', N'Dầu gội Clear 630g', N'Chai', 140000, 155000, 0.1, 50, N'Kệ A1', NULL, '11111111'),
('MH02', N'Hóa mỹ phẩm', N'Sữa tắm Lifebuoy', N'Chai', 120000, 135000, 0.1, 40, N'Kệ A1', NULL, '11111111'),
('MH03', N'Hóa mỹ phẩm', N'Kem đánh răng PS', N'Hộp', 30000, 35000, 0.1, 100, N'Kệ A1', NULL, '11111111'),
('MH04', N'Hóa mỹ phẩm', N'Nước giặt Ariel 3kg', N'Túi', 175000, 195000, 0.1, 30, N'Kệ A2', NULL, '11111111'),
('MH05', N'Hóa mỹ phẩm', N'Nước rửa chén Sunlight', N'Chai', 22000, 28000, 0.1, 80, N'Kệ A2', NULL, '11111111'),
('MH06', N'Hóa mỹ phẩm', N'Sữa rửa mặt Oxy', N'Tuýp', 50000, 65000, 0.1, 60, N'Kệ A3', NULL, '11111111'),
('MH07', N'Hóa mỹ phẩm', N'Lăn khử mùi X-Men', N'Chai', 35000, 45000, 0.1, 70, N'Kệ A3', NULL, '11111111'),
('MH08', N'Hóa mỹ phẩm', N'Nước xả vải Downy', N'Túi', 160000, 185000, 0.1, 40, N'Kệ A2', NULL, '11111111'),
('MH09', N'Hóa mỹ phẩm', N'Nước lau sàn Sunlight', N'Chai', 25000, 32000, 0.1, 90, N'Kệ A2', NULL, '11111111'),
('MH10', N'Hóa mỹ phẩm', N'Dầu xả Dove', N'Chai', 125000, 145000, 0.1, 50, N'Kệ A1', NULL, '11111111'),
('MH11', N'Thực phẩm khô', N'Gạo ST25 5kg', N'Túi', 160000, 185000, 0.05, 100, N'Kệ B1', NULL, '22222222'),
('MH12', N'Thực phẩm khô', N'Dầu ăn Simply 1L', N'Chai', 50000, 58000, 0.1, 200, N'Kệ B2', NULL, '22222222'),
('MH13', N'Thực phẩm khô', N'Nước mắm Chinsu', N'Chai', 35000, 42000, 0.1, 150, N'Kệ B2', NULL, '22222222'),
('MH14', N'Thực phẩm khô', N'Hạt nêm Knorr 900g', N'Gói', 70000, 85000, 0.1, 80, N'Kệ B2', NULL, '22222222'),
('MH15', N'Thực phẩm khô', N'Tương ớt Cholimex', N'Chai', 10000, 15000, 0.1, 300, N'Kệ B2', NULL, '22222222'),
('MH16', N'Thực phẩm khô', N'Bột ngọt Vedan', N'Gói', 28000, 35000, 0.1, 120, N'Kệ B2', NULL, '22222222'),
('MH17', N'Thực phẩm khô', N'Đường trắng tinh luyện', N'Gói', 22000, 26000, 0.1, 200, N'Kệ B1', NULL, '22222222'),
('MH18', N'Thực phẩm khô', N'Muối i-ốt', N'Gói', 4000, 6000, 0.1, 500, N'Kệ B1', NULL, '22222222'),
('MH19', N'Thực phẩm khô', N'Miến Phú Hương', N'Gói', 9000, 12000, 0.1, 100, N'Kệ B3', NULL, '22222222'),
('MH20', N'Thực phẩm khô', N'Bún tươi đóng gói', N'Gói', 14000, 18000, 0.1, 80, N'Kệ B3', NULL, '22222222'),
('MH21', N'Thịt, cá, hải sản', N'Thịt ba rọi heo', N'Kg', 140000, 165000, 0.1, 40, N'Tủ mát 1', NULL, '88888888'),
('MH22', N'Thịt, cá, hải sản', N'Thăn bò Úc', N'Kg', 300000, 350000, 0.1, 20, N'Tủ mát 1', NULL, '88888888'),
('MH23', N'Thịt, cá, hải sản', N'Gà ta nguyên con', N'Con', 150000, 180000, 0.1, 30, N'Tủ mát 1', NULL, '88888888'),
('MH24', N'Thịt, cá, hải sản', N'Tôm sú tươi', N'Kg', 240000, 280000, 0.1, 25, N'Tủ mát 2', NULL, '88888888'),
('MH25', N'Thịt, cá, hải sản', N'Cá thu cắt khúc', N'Kg', 190000, 220000, 0.1, 15, N'Tủ mát 2', NULL, '88888888'),
('MH26', N'Thịt, cá, hải sản', N'Sườn non heo', N'Kg', 165000, 190000, 0.1, 35, N'Tủ mát 1', NULL, '88888888'),
('MH27', N'Thịt, cá, hải sản', N'Mực ống tươi', N'Kg', 210000, 250000, 0.1, 20, N'Tủ mát 2', NULL, '88888888'),
('MH28', N'Thịt, cá, hải sản', N'Trứng gà (Vỉ 10)', N'Vỉ', 25000, 32000, 0.1, 200, N'Kệ mát', NULL, '88888888'),
('MH29', N'Thịt, cá, hải sản', N'Trứng vịt (Vỉ 10)', N'Vỉ', 28000, 35000, 0.1, 150, N'Kệ mát', NULL, '88888888'),
('MH30', N'Thịt, cá, hải sản', N'Cua thịt Cà Mau', N'Kg', 380000, 450000, 0.1, 10, N'Bể hải sản', NULL, '88888888'),
('MH31', N'Rau, củ, quả', N'Bắp cabbage trắng', N'Kg', 15000, 20000, 0.1, 100, N'Kệ rau', NULL, '99999999'),
('MH32', N'Rau, củ, quả', N'Cà chua Đà Lạt', N'Kg', 25000, 35000, 0.1, 80, N'Kệ rau', NULL, '99999999'),
('MH33', N'Rau, củ, quả', N'Khoai tây vàng', N'Kg', 20000, 28000, 0.1, 120, N'Kệ củ', NULL, '99999999'),
('MH34', N'Rau, củ, quả', N'Củ cải trắng', N'Kg', 12000, 18000, 0.1, 150, N'Kệ củ', NULL, '99999999'),
('MH35', N'Rau, củ, quả', N'Dưa hấu đỏ', N'Kg', 10000, 15000, 0.1, 200, N'Kệ trái cây', NULL, '99999999'),
('MH36', N'Rau, củ, quả', N'Xoài cát Hòa Lộc', N'Kg', 65000, 85000, 0.1, 50, N'Kệ trái cây', NULL, '99999999'),
('MH37', N'Rau, củ, quả', N'Cam sành', N'Kg', 20000, 30000, 0.1, 100, N'Kệ trái cây', NULL, '99999999'),
('MH38', N'Rau, củ, quả', N'Hành tây', N'Kg', 15000, 22000, 0.1, 150, N'Kệ củ', NULL, '99999999'),
('MH39', N'Rau, củ, quả', N'Tỏi Lý Sơn', N'Kg', 90000, 120000, 0.1, 40, N'Kệ gia vị tươi', NULL, '99999999'),
('MH40', N'Rau, củ, quả', N'Ớt hiểm', N'Kg', 30000, 45000, 0.1, 60, N'Kệ gia vị tươi', NULL, '99999999'),
('MH41', N'Mì, cháo, phở', N'Mì Hảo Hảo Tôm Cay', N'Gói', 3500, 4500, 0.1, 500, N'Kệ C1', NULL, '77777777'),
('MH42', N'Mì, cháo, phở', N'Mì Omachi Xốt Bò', N'Gói', 6500, 8500, 0.1, 300, N'Kệ C1', NULL, '77777777'),
('MH43', N'Mì, cháo, phở', N'Phở Đệ Nhất', N'Gói', 7500, 9000, 0.1, 200, N'Kệ C2', NULL, '77777777'),
('MH44', N'Mì, cháo, phở', N'Cháo Gấu Đỏ', N'Gói', 4000, 5500, 0.1, 400, N'Kệ C3', NULL, '77777777'),
('MH45', N'Mì, cháo, phở', N'Mì ly Modern', N'Ly', 9500, 12000, 0.1, 150, N'Kệ C1', NULL, '77777777'),
('MH46', N'Mì, cháo, phở', N'Hủ tiếu Nam Vang', N'Gói', 8000, 10000, 0.1, 180, N'Kệ C2', NULL, '77777777'),
('MH47', N'Mì, cháo, phở', N'Mì trộn Indomie', N'Gói', 4500, 6000, 0.1, 250, N'Kệ C1', NULL, '77777777'),
('MH48', N'Mì, cháo, phở', N'Cháo thịt bằm tươi', N'Gói', 11000, 15000, 0.1, 100, N'Kệ C3', NULL, '77777777'),
('MH49', N'Mì, cháo, phở', N'Mì cay Koreno', N'Gói', 11000, 14000, 0.1, 200, N'Kệ C1', NULL, '77777777'),
('MH50', N'Mì, cháo, phở', N'Phở khô gói', N'Gói', 8500, 11000, 0.1, 120, N'Kệ C2', NULL, '77777777'),
('MH51', N'Bánh kẹo', N'Bánh quy Cosy', N'Gói', 35000, 45000, 0.1, 100, N'Kệ D1', NULL, '33333333'),
('MH52', N'Bánh kẹo', N'Kẹo Alpenliebe', N'Gói', 12000, 18000, 0.1, 200, N'Kệ D2', NULL, '33333333'),
('MH53', N'Bánh kẹo', N'Socola KitKat', N'Thanh', 8000, 12000, 0.1, 150, N'Kệ D1', NULL, '33333333'),
('MH54', N'Bánh kẹo', N'Bánh gạo One One', N'Gói', 18000, 25000, 0.1, 80, N'Kệ D1', NULL, '33333333'),
('MH55', N'Bánh kẹo', N'Snack Lay vị khoai tây', N'Gói', 10000, 15000, 0.1, 300, N'Kệ D2', NULL, '33333333'),
('MH56', N'Bánh kẹo', N'Kẹo cao su Doublemint', N'Hũ', 15000, 22000, 0.1, 120, N'Kệ D2', NULL, '33333333'),
('MH57', N'Bánh kẹo', N'Bánh trứng Tipo', N'Gói', 28000, 38000, 0.1, 70, N'Kệ D1', NULL, '33333333'),
('MH58', N'Bánh kẹo', N'Thạch rau câu Long Hải', N'Gói', 22000, 32000, 0.1, 90, N'Kệ D2', NULL, '33333333'),
('MH59', N'Bánh kẹo', N'Bánh que Pocky', N'Hộp', 14000, 20000, 0.1, 110, N'Kệ D1', NULL, '33333333'),
('MH60', N'Bánh kẹo', N'Kẹo dẻo chip chip', N'Gói', 6000, 10000, 0.1, 250, N'Kệ D2', NULL, '33333333'),
('MH61', N'Nước giải khát', N'Pepsi 1.5L', N'Chai', 14000, 18000, 0.1, 150, N'Kệ E1', NULL, '44444444'),
('MH62', N'Nước giải khát', N'Trà xanh không độ', N'Chai', 7000, 10000, 0.1, 200, N'Kệ E1', NULL, '44444444'),
('MH63', N'Nước giải khát', N'Nước tăng lực Sting', N'Chai', 7500, 10000, 0.1, 300, N'Kệ E1', NULL, '44444444'),
('MH64', N'Nước giải khát', N'Bia Heineken', N'Lon', 16500, 19000, 0.1, 500, N'Kệ E2', NULL, '44444444'),
('MH65', N'Nước giải khát', N'Bia 333', N'Lon', 12000, 14000, 0.1, 400, N'Kệ E2', NULL, '44444444'),
('MH66', N'Nước giải khát', N'Nước ép cam Twister', N'Chai', 9000, 12000, 0.1, 180, N'Kệ E1', NULL, '44444444'),
('MH67', N'Nước giải khát', N'Nước khoáng Lavie', N'Chai', 3000, 5000, 0.1, 600, N'Kệ E1', NULL, '44444444'),
('MH68', N'Nước giải khát', N'Sữa hạt đậu nành Fami', N'Bịch', 3500, 5000, 0.1, 400, N'Kệ E3', NULL, '44444444'),
('MH69', N'Nước giải khát', N'Trà sữa đóng chai', N'Chai', 11000, 15000, 0.1, 120, N'Kệ E1', NULL, '44444444'),
('MH70', N'Nước giải khát', N'Bia Sài Gòn Special', N'Lon', 13500, 16000, 0.1, 350, N'Kệ E2', NULL, '44444444'),
('MH71', N'Sữa', N'Sữa Milo ít đường', N'Hộp', 6000, 8000, 0.1, 400, N'Kệ F1', NULL, '55555555'),
('MH72', N'Sữa', N'Sữa chua Vinamilk có đường', N'Hũ', 5000, 7000, 0.1, 300, N'Tủ lạnh 1', NULL, '55555555'),
('MH73', N'Sữa', N'Sữa tươi TH True Milk', N'Lốc', 26000, 32000, 0.1, 150, N'Kệ F1', NULL, '55555555'),
('MH74', N'Sữa', N'Sữa đặc Ông Thọ', N'Lon', 20000, 25000, 0.1, 100, N'Kệ F2', NULL, '55555555'),
('MH75', N'Sữa', N'Váng sữa Monte', N'Hũ', 12000, 15000, 0.1, 120, N'Tủ lạnh 1', NULL, '55555555'),
('MH76', N'Sữa', N'Phô mai con bò cười', N'Hộp', 28000, 35000, 0.1, 80, N'Tủ lạnh 1', NULL, '55555555'),
('MH77', N'Sữa', N'Sữa bột Abbott Grow', N'Hộp', 320000, 380000, 0.1, 30, N'Kệ F2', NULL, '55555555'),
('MH78', N'Sữa', N'Sữa đậu xanh Nutifood', N'Bịch', 4000, 6000, 0.1, 200, N'Kệ F1', NULL, '55555555'),
('MH79', N'Sữa', N'Sữa chua uống Susu', N'Chai', 3500, 5000, 0.1, 250, N'Kệ F1', NULL, '55555555'),
('MH80', N'Sữa', N'Bơ lạt Anchor', N'Khối', 50000, 65000, 0.1, 40, N'Tủ lạnh 1', NULL, '55555555'),
('MH81', N'Gia dụng', N'Chảo chống dính Sunhouse', N'Cái', 90000, 120000, 0.1, 40, N'Kệ G1', NULL, '66666666'),
('MH82', N'Gia dụng', N'Bộ lau nhà 360', N'Bộ', 190000, 250000, 0.1, 20, N'Góc gia dụng', NULL, '66666666'),
('MH83', N'Gia dụng', N'Bát sứ Minh Long', N'Cái', 18000, 25000, 0.1, 200, N'Kệ G2', NULL, '66666666'),
('MH84', N'Gia dụng', N'Đũa tre xuất khẩu', N'Vỉ', 12000, 20000, 0.1, 150, N'Kệ G2', NULL, '66666666'),
('MH85', N'Gia dụng', N'Thớt nhựa cao cấp', N'Cái', 32000, 45000, 0.1, 60, N'Kệ G1', NULL, '66666666'),
('MH86', N'Gia dụng', N'Khăn lau đa năng', N'Cái', 8000, 15000, 0.1, 300, N'Kệ G1', NULL, '66666666'),
('MH87', N'Gia dụng', N'Ổ cắm điện Điện Quang', N'Cái', 65000, 85000, 0.1, 50, N'Kệ G3', NULL, '66666666'),
('MH88', N'Gia dụng', N'Bóng đèn LED 12W', N'Cái', 25000, 35000, 0.1, 100, N'Kệ G3', NULL, '66666666'),
('MH89', N'Gia dụng', N'Móc quần áo nhựa', N'Bộ', 20000, 30000, 0.1, 120, N'Góc gia dụng', NULL, '66666666'),
('MH90', N'Gia dụng', N'Thùng rác thông minh', N'Cái', 145000, 180000, 0.1, 25, N'Góc gia dụng', NULL, '66666666'),
('MH91', N'Văn phòng phẩm', N'Vở 96 trang', N'Cuốn', 5000, 8000, 0.1, 500, N'Kệ H1', NULL, '00000000'),
('MH92', N'Văn phòng phẩm', N'Bút bi Thiên Long', N'Cây', 2500, 4000, 0.1, 1000, N'Kệ H1', NULL, '00000000'),
('MH93', N'Văn phòng phẩm', N'Bút chì 2B', N'Cây', 2000, 3000, 0.1, 800, N'Kệ H1', NULL, '00000000'),
('MH94', N'Văn phòng phẩm', N'Thước kẻ 20cm', N'Cây', 3000, 5000, 0.1, 400, N'Kệ H2', NULL, '00000000'),
('MH95', N'Văn phòng phẩm', N'Gôm tẩy', N'Cục', 1500, 3000, 0.1, 600, N'Kệ H2', NULL, '00000000'),
('MH96', N'Văn phòng phẩm', N'Giấy A4 Double A', N'Ram', 72000, 85000, 0.1, 100, N'Góc văn phòng', NULL, '00000000'),
('MH97', N'Văn phòng phẩm', N'Kéo văn phòng', N'Cái', 10000, 15000, 0.1, 120, N'Kệ H2', NULL, '00000000'),
('MH98', N'Văn phòng phẩm', N'Băng keo trong', N'Cuộn', 8000, 12000, 0.1, 200, N'Kệ H2', NULL, '00000000'),
('MH99', N'Văn phòng phẩm', N'Cặp tài liệu', N'Cái', 35000, 45000, 0.1, 60, N'Góc văn phòng', NULL, '00000000'),
('MH100', N'Văn phòng phẩm', N'Hồ dán nước', N'Lọ', 4000, 6000, 0.1, 300, N'Kệ H1', NULL, '00000000');
GO
GO
-------------------------------------------------------
-- 5. BẢNG HÓA ĐƠN TẠM
-------------------------------------------------------
CREATE TABLE HoaDonTam
(
    MaHDTam VARCHAR(50) PRIMARY KEY NOT NULL,
    MaKH VARCHAR(50) NULL,
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0,
    NgayLapHD DATETIME NOT NULL DEFAULT GETDATE(),
    TenKH NVARCHAR(50) NOT NULL
);
GO

INSERT INTO HoaDonTam (MaHDTam, MaKH, TongTien, NgayLapHD, TenKH) VALUES
('HDT01', NULL, 467500, '2026-05-20', N'Khách lẻ'),
('HDT02', NULL, 682000, '2026-05-21', N'Khách VIP');
GO


-------------------------------------------------------
-- 6. BẢNG HÓA ĐƠN BÁN HÀNG
------------------------------------------------------- 
CREATE TABLE HoaDonBanHang
(
    MaHDBanHang VARCHAR(50) PRIMARY KEY NOT NULL,
    MaKH VARCHAR(50) NULL,
    MaNV VARCHAR(50) NOT NULL,
    NgayLapHD DATETIME NOT NULL DEFAULT GETDATE(),
    Ca NVARCHAR(15) NOT NULL,
    TienGiamPoints DECIMAL(18,2) NOT NULL DEFAULT 0,  
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0,
    CONSTRAINT FK_HDBH_KhachHang FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    CONSTRAINT FK_HDBH_NhanVien FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
GO

-- CHÈN TRƯỚC HÓA ĐƠN GỐC
INSERT INTO HoaDonBanHang (MaHDBanHang, MaKH, MaNV, NgayLapHD, Ca, TienGiamPoints, TongTien) VALUES
('HD01', 'KH01', 'NV02', '2026-05-12', N'Sáng', 0, 489500),
('HD02', 'KH02', 'NV02', '2026-05-13', N'Chiều', 0, 1188000),
('HD03', 'KH03', 'NV02', '2026-05-14', N'Tối', 0, 173800),
('HD04', 'KH04', 'NV02', '2026-05-15', N'Sáng', 0, 660000),
('HD05', 'KH05', 'NV02', '2026-05-16', N'Chiều', 0, 229900),
('HD06', 'KH01', 'NV02', '2026-05-17', N'Tối', 0, 707500);
GO


-------------------------------------------------------
-- 7. BẢNG CHI TIẾT BÁN HÀNG (SỬA ĐỒNG BỘ MÃ HÓA ĐƠN)
-------------------------------------------------------
CREATE TABLE ChiTietBanHang
(
    STT INT IDENTITY(1,1) PRIMARY KEY,
    MaMH VARCHAR(50) NOT NULL,
    MaHDBanHang VARCHAR(50) NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    GiamGia FLOAT NOT NULL DEFAULT 0,
    ThanhTien DECIMAL(18,2) NOT NULL,
    CONSTRAINT UQ_CTBH UNIQUE (MaMH, MaHDBanHang),
    CONSTRAINT FK_CTBH_MatHang FOREIGN KEY (MaMH) REFERENCES MatHang(MaMH),
    CONSTRAINT FK_CTBH_HoaDon FOREIGN KEY (MaHDBanHang) REFERENCES HoaDonBanHang(MaHDBanHang) ON DELETE CASCADE
);
GO

-- Chèn chi tiết khớp chính xác 100% mã với bảng hóa đơn ở trên
INSERT INTO ChiTietBanHang (MaMH, MaHDBanHang, SoLuong, GiamGia, ThanhTien) VALUES
('MH01', 'HD01', 2, 0, 341000),   
('MH02', 'HD01', 1, 0, 148500),   
('MH03', 'HD02', 3, 0, 115500),   
('MH04', 'HD02', 5, 0, 1072500),  
('MH05', 'HD03', 1, 0, 30800),     
('MH06', 'HD03', 2, 0, 143000),   
('MH07', 'HD04', 1, 0, 49500),     
('MH08', 'HD04', 3, 0, 610500),   
('MH09', 'HD05', 2, 0, 70400),     
('MH10', 'HD05', 1, 0, 159500),   
('MH11', 'HD06', 2, 0, 388500),   
('MH12', 'HD06', 5, 0, 319000);   
GO


-------------------------------------------------------
-- 8. BẢNG NHÀ CUNG CẤP
-------------------------------------------------------
CREATE TABLE NhaCungCap
(
    MaNCC VARCHAR(50) PRIMARY KEY NOT NULL,
    TenNCC NVARCHAR(50) NOT NULL,
    DiaChi NVARCHAR(100) NOT NULL,
    SDT VARCHAR(20) NOT NULL,
	LinhVuc NVARCHAR(100) NOT NULL,
	TrangThai NVARCHAR(100) NOT NULL
);
GO
INSERT INTO NhaCungCap (MaNCC, TenNCC, DiaChi, SDT, LinhVuc, TrangThai) VALUES 
('NCC01', N'Công ty Thực phẩm A', N'HCM', '0981111111', N'Bánh kẹo', N'Đang hoạt động'), 
('NCC02', N'Công ty Nước giải khát B', N'Hà Nội', '0982222222', N'Nước uống', N'Đang hoạt động');
GO


-------------------------------------------------------
-- 9. BẢNG NHẬP HÀNG
-------------------------------------------------------
CREATE TABLE NhapHang
(
    MaNhapHang VARCHAR(50) PRIMARY KEY NOT NULL,
    NgayNhap DATETIME NOT NULL DEFAULT GETDATE(),
    MaNCC VARCHAR(50) NOT NULL,
    MaNV VARCHAR(50) NOT NULL,
    CONSTRAINT FK_NhapHang_NCC FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC),
    CONSTRAINT FK_NhapHang_NV FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
GO

INSERT INTO NhapHang (MaNhapHang, NgayNhap, MaNCC, MaNV) VALUES
('NH01', '2026-05-01', 'NCC01', 'NV03'),
('NH02', '2026-05-02', 'NCC02', 'NV03'),
('NH03', '2026-05-05', 'NCC01', 'NV03'),
('NH04', '2026-05-08', 'NCC02', 'NV03'),
('NH05', '2026-05-10', 'NCC01', 'NV03');
GO


-------------------------------------------------------
-- 10. BẢNG CHI TIẾT NHẬP HÀNG
-------------------------------------------------------
CREATE TABLE ChiTietNhapHang
(
    ID INT IDENTITY(1,1) PRIMARY KEY,
    MaNhapHang VARCHAR(50) NOT NULL,
    MaMH VARCHAR(50) NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    NgayNhap DATE NOT NULL,
	NgayHetHan DATE NOT NULL,
    CONSTRAINT UQ_CTNhapHang UNIQUE (MaNhapHang, MaMH),
    CONSTRAINT FK_CTNhapHang_NH FOREIGN KEY (MaNhapHang) REFERENCES NhapHang(MaNhapHang) ON DELETE CASCADE,
    CONSTRAINT FK_CTNhapHang_MH FOREIGN KEY (MaMH) REFERENCES MatHang(MaMH)
);
GO

INSERT INTO ChiTietNhapHang 
(MaNhapHang, MaMH, SoLuong, NgayNhap, NgayHetHan) 
VALUES
('NH01', 'MH01', 50, '2026-05-01', '2026-03-01'),
('NH01', 'MH02', 40, '2026-05-01', '2026-04-15'),
('NH02', 'MH03', 60, '2026-05-02', '2026-05-20'),
('NH03', 'MH04', 70, '2026-05-05', '2026-05-30'),
('NH03', 'MH05', 30, '2026-05-05', '2026-05-10');
GO

-------------------------------------------------------
-- 11. BẢNG HỦY HÀNG
-------------------------------------------------------
CREATE TABLE HuyHang
(
    MaHangHuy INT IDENTITY(1,1) PRIMARY KEY,
    MaMH VARCHAR(50) NOT NULL,
    SoLuong INT NOT NULL,
    LyDo NVARCHAR(MAX) NOT NULL,
    NgayHuy DATETIME NOT NULL DEFAULT GETDATE(),
    Duyet BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_HuyHang_MH FOREIGN KEY (MaMH) REFERENCES MatHang(MaMH)
);
GO


-------------------------------------------------------
-- 12. BẢNG HÀNG SẮP HẾT HẠN
-------------------------------------------------------
CREATE TABLE HangSapHetHan
(
    ID INT IDENTITY(1,1) PRIMARY KEY,
    MaCanDate VARCHAR(50) NOT NULL,
    MaMH VARCHAR(50) NOT NULL,
    SoLuong INT NOT NULL,
    NgayHetHan DATETIME NOT NULL,
    ViTri NVARCHAR(15) NOT NULL,
    CONSTRAINT UQ_HSHH UNIQUE (MaCanDate, MaMH),
    CONSTRAINT FK_HSHH_MH FOREIGN KEY (MaMH) REFERENCES MatHang(MaMH)
);
GO


-------------------------------------------------------
-- 13. BẢNG TÀI KHOẢN ĐĂNG NHẬP
-------------------------------------------------------
CREATE TABLE TaiKhoanDangNhap
(
    TenDangNhap VARCHAR(50) PRIMARY KEY,
    MatKhau VARCHAR(20) NOT NULL,
    VaiTro NVARCHAR(30) NOT NULL,
    MaNV VARCHAR(50) NOT NULL,
    Email VARCHAR(50) NOT NULL,
    CONSTRAINT FK_TK_NV FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV) ON DELETE CASCADE
);
GO

INSERT INTO TaiKhoanDangNhap (TenDangNhap, MatKhau, VaiTro, MaNV, Email) VALUES
('admin', '123', N'Quản lý', 'NV01', 'admin@gmail.com'),
('thungan', '123', N'Thu ngân', 'NV02', 'tn@gmail.com'),
('kho', '123', N'Kho', 'NV03', 'kho@gmail.com'),
('banhang', '123', N'Bán hàng', 'NV04', 'bh@gmail.com');
GO


-------------------------------------------------------
-- 14. BẢNG CHI TIẾT HÓA ĐƠN TẠM
-------------------------------------------------------
CREATE TABLE ChiTietHoaDonTam
(
    STTChiTiet INT IDENTITY(1,1) PRIMARY KEY,
    MaHDTam VARCHAR(50) NOT NULL,
    MaMH VARCHAR(50) NOT NULL,
    SoLuong INT NOT NULL,
    ThanhTien DECIMAL(18,2) NOT NULL,
    GiamGia FLOAT NOT NULL DEFAULT 0,
    CONSTRAINT FK_CTHDT_HoaDonTam FOREIGN KEY (MaHDTam) REFERENCES HoaDonTam(MaHDTam) ON DELETE CASCADE,
    CONSTRAINT FK_CTHDT_MatHang FOREIGN KEY (MaMH) REFERENCES MatHang(MaMH)
);
GO

INSERT INTO ChiTietHoaDonTam (MaHDTam, MaMH, SoLuong, ThanhTien, GiamGia) VALUES
('HDT01', 'MH01', 1, 170500, 0),  
('HDT01', 'MH02', 2, 297000, 0),  
('HDT02', 'MH03', 1, 38500, 0),   
('HDT02', 'MH04', 3, 643500, 0);
GO