using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BUS
{
    public static class KhachHangBUS
    {
        public static KhachHangDTO GetByCCCD(string cccd) => KhachHangDAL.GetByCCCD(cccd);

        public static int ThemKhachHang(KhachHangDTO kh)
        {
            if (string.IsNullOrWhiteSpace(kh.HoTen))
                throw new Exception("Họ tên không được để trống!");
            if (string.IsNullOrWhiteSpace(kh.CCCD))
                throw new Exception("CCCD không được để trống!");
            return KhachHangDAL.Insert(kh);
        }
    }
}