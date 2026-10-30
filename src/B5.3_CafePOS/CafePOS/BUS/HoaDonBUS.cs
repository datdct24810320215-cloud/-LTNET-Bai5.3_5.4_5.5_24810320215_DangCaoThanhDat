using System;
using System.Data;
using CafePOS.DAL;

namespace CafePOS.BUS
{
    public class HoaDonBUS
    {
        private readonly HoaDonDAL _dal = new HoaDonDAL();
        private readonly ChiTietHoaDonDAL _ctDal = new ChiTietHoaDonDAL();

        public int GetActiveHoaDon(int maBan) => _dal.GetActiveHoaDon(maBan);
        public int CreateHoaDon(int maBan) => _dal.CreateHoaDon(maBan);

        public void ThanhToan(int maHoaDon, int maBan)
        {
            decimal tong = _ctDal.TinhTongTien(maHoaDon);
            if (tong <= 0)
                throw new Exception("Hóa đơn chưa có món, không thể thanh toán!");
            _dal.ThanhToan(maHoaDon, maBan, tong);
        }

        public void DoiBan(int maHoaDon, int maBanCu, int maBanMoi)
        {
            _dal.DoiBan(maHoaDon, maBanCu, maBanMoi);
        }

        public DataTable BaoCaoTheoNgay(DateTime date) => _dal.GetHoaDonByDate(date);
    }
}