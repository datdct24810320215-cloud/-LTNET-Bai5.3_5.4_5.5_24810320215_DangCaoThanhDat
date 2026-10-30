-- =============================================
-- ScriptDB.sql - Bài 5.5: Quản lý Xuất - Nhập Kho
-- =============================================
IF DB_ID('InventoryDB') IS NOT NULL
    DROP DATABASE InventoryDB;
GO
CREATE DATABASE InventoryDB;
GO
USE InventoryDB;
GO

-- 1. Nhà cung cấp
CREATE TABLE NhaCungCap (
    MaNCC     INT PRIMARY KEY IDENTITY(1,1),
    TenNCC    NVARCHAR(150) NOT NULL,
    DienThoai VARCHAR(15) UNIQUE,
    DiaChi    NVARCHAR(255)
);

-- 2. Vật tư
CREATE TABLE VatTu (
    MaVatTu      VARCHAR(20) PRIMARY KEY,
    TenVatTu     NVARCHAR(150) NOT NULL,
    DVT          NVARCHAR(20),
    SoLuongTon   DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (SoLuongTon >= 0),
    NguongAnToan DECIMAL(18,2) NOT NULL DEFAULT 10
);

-- 3. Phiếu kho
CREATE TABLE PhieuKho (
    MaPhieu   INT PRIMARY KEY IDENTITY(1,1),
    LoaiPhieu NVARCHAR(10) NOT NULL CHECK (LoaiPhieu IN ('NHAP','XUAT')),
    NgayLap   DATETIME DEFAULT GETDATE(),
    MaNCC     INT NULL FOREIGN KEY REFERENCES NhaCungCap(MaNCC),
    TongTien  DECIMAL(18,2) NOT NULL DEFAULT 0
);

-- 4. Chi tiết phiếu kho
CREATE TABLE ChiTietPhieuKho (
    MaPhieu  INT NOT NULL FOREIGN KEY REFERENCES PhieuKho(MaPhieu),
    MaVatTu  VARCHAR(20) NOT NULL FOREIGN KEY REFERENCES VatTu(MaVatTu),
    SoLuong  DECIMAL(18,2) NOT NULL CHECK (SoLuong > 0),
    DonGia   DECIMAL(18,2) NOT NULL CHECK (DonGia >= 0),
    PRIMARY KEY (MaPhieu, MaVatTu)
);
GO

-- ============ DỮ LIỆU MẪU ============
INSERT INTO NhaCungCap (TenNCC, DienThoai, DiaChi) VALUES
(N'Công ty Xi măng Hà Tiên', '0901234567', N'123 Nguyễn Huệ, Q1, TP.HCM'),
(N'Công ty Thép Hòa Phát',   '0912345678', N'456 Lê Lợi, Q3, TP.HCM'),
(N'Công ty VLXD Đồng Tâm',   '0923456789', N'789 Trần Hưng Đạo, Q5, TP.HCM');

INSERT INTO VatTu (MaVatTu, TenVatTu, DVT, SoLuongTon, NguongAnToan) VALUES
('VT001', N'Xi măng Hà Tiên PCB40', N'Bao',   100, 20),
('VT002', N'Sắt thép cây D12',      N'Kg',    500, 100),
('VT003', N'Gạch đỏ 2 lỗ',          N'Viên', 1000, 200),
('VT004', N'Cát vàng hạt lớn',      N'Khối',   50, 15),
('VT005', N'Đá 1x2',                N'Khối',    8, 20);
GO
PRINT N'>>> Khởi tạo CSDL InventoryDB thành công!';