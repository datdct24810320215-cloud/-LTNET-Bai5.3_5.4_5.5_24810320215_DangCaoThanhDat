USE master;
GO
IF DB_ID('HotelDB') IS NOT NULL
    DROP DATABASE HotelDB;
GO
CREATE DATABASE HotelDB;
GO
USE HotelDB;
GO

-- 1. Phong
CREATE TABLE Phong (
    SoPhong     VARCHAR(10)     PRIMARY KEY,
    LoaiPhong   NVARCHAR(50)    NOT NULL,
    GiaTheoGio  DECIMAL(18,2)   NOT NULL,
    TrangThai   NVARCHAR(30)    NOT NULL
);

-- 2. KhachHang
CREATE TABLE KhachHang (
    MaKH        INT             IDENTITY(1,1) PRIMARY KEY,
    HoTen       NVARCHAR(100)   NOT NULL,
    CCCD        VARCHAR(20)     NOT NULL UNIQUE,
    SoDienThoai VARCHAR(15)
);

-- 3. DichVu
CREATE TABLE DichVu (
    MaDV    INT             IDENTITY(1,1) PRIMARY KEY,
    TenDV   NVARCHAR(100)   NOT NULL,
    DonGia  DECIMAL(18,2)   NOT NULL
);

-- 4. PhieuDatPhong
CREATE TABLE PhieuDatPhong (
    MaPhieu             INT IDENTITY(1,1) PRIMARY KEY,
    SoPhong             VARCHAR(10)     NOT NULL,
    MaKH                INT             NOT NULL,
    NgayNhanPhong       DATETIME        NOT NULL,
    NgayTraPhong        DATETIME        NULL,
    TongTienThanhToan   DECIMAL(18,2)   NULL,
    CONSTRAINT FK_PDP_Phong     FOREIGN KEY (SoPhong) REFERENCES Phong(SoPhong),
    CONSTRAINT FK_PDP_KhachHang FOREIGN KEY (MaKH)    REFERENCES KhachHang(MaKH)
);

-- 5. SuDungDichVu
CREATE TABLE SuDungDichVu (
    MaSD    INT IDENTITY(1,1) PRIMARY KEY,
    MaPhieu INT             NOT NULL,
    MaDV    INT             NOT NULL,
    SoLuong INT             NOT NULL,
    ThoiGian DATETIME       NOT NULL,
    CONSTRAINT FK_SDDV_Phieu  FOREIGN KEY (MaPhieu) REFERENCES PhieuDatPhong(MaPhieu),
    CONSTRAINT FK_SDDV_DichVu FOREIGN KEY (MaDV)    REFERENCES DichVu(MaDV)
);
GO

-- ====== DỮ LIỆU MẪU ======
INSERT INTO Phong VALUES
('P101', N'Đơn', 50000,  N'Trống'),
('P102', N'Đơn', 50000,  N'Trống'),
('P103', N'Đơn', 50000,  N'Đang ở'),
('P201', N'Đôi', 80000,  N'Trống'),
('P202', N'Đôi', 80000,  N'Đang ở'),
('P301', N'VIP', 150000, N'Trống'),
('P302', N'VIP', 150000, N'Đang dọn dẹp');

INSERT INTO KhachHang (HoTen, CCCD, SoDienThoai) VALUES
(N'Nguyễn Văn A', '012345678901', '0901111111'),
(N'Trần Thị B',   '012345678902', '0902222222'),
(N'Lê Văn C',     '012345678903', '0903333333');

INSERT INTO DichVu (TenDV, DonGia) VALUES
(N'Nước suối',  10000),
(N'Coca Cola',  15000),
(N'Giặt là',    30000),
(N'Cơm tấm',    45000),
(N'Mì gói',     12000);

INSERT INTO PhieuDatPhong (SoPhong, MaKH, NgayNhanPhong) VALUES
('P103', 1, DATEADD(HOUR, -2, GETDATE())),
('P202', 2, DATEADD(HOUR, -1, GETDATE()));

INSERT INTO SuDungDichVu (MaPhieu, MaDV, SoLuong, ThoiGian) VALUES
(1, 1, 2, GETDATE()),
(1, 3, 1, GETDATE());
GO