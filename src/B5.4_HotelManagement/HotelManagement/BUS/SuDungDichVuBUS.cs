using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BUS
{
    public static class SuDungDichVuBUS
    {
        public static List<SuDungDichVuDTO> GetByMaPhieu(int mp) => SuDungDichVuDAL.GetByMaPhieu(mp);

        public static void ThemDichVu(int maPhieu, int maDV, int soLuong)
        {
            if (maPhieu <= 0) throw new Exception("Chưa chọn phiếu!");
            if (maDV <= 0) throw new Exception("Chưa chọn dịch vụ!");
            if (soLuong <= 0) throw new Exception("Số lượng phải > 0!");

            SuDungDichVuDAL.Insert(new SuDungDichVuDTO
            {
                MaPhieu = maPhieu,
                MaDV = maDV,
                SoLuong = soLuong,
                ThoiGian = DateTime.Now
            });
        }
    }
}