using System;
using System.Data;
using CafePOS.DAL;

namespace CafePOS.BUS
{
    public class ChiTietHoaDonBUS
    {
        private readonly ChiTietHoaDonDAL _dal = new ChiTietHoaDonDAL();

        public DataTable GetByHoaDon(int maHoaDon) => _dal.GetByHoaDon(maHoaDon);
        public decimal TinhTongTien(int maHoaDon) => _dal.TinhTongTien(maHoaDon);

        public void ThemMon(int maHoaDon, int maMon, int soLuong, decimal donGia)
        {
            if (soLuong <= 0)
                throw new Exception("Số lượng phải lớn hơn 0!");
            _dal.Them(maHoaDon, maMon, soLuong, donGia);
        }

        public void XoaMon(int maChiTiet) => _dal.Xoa(maChiTiet);
    }
}