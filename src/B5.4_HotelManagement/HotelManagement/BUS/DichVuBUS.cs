using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BUS
{
    public static class DichVuBUS
    {
        public static List<DichVuDTO> GetAll() => DichVuDAL.GetAll();
    }
}