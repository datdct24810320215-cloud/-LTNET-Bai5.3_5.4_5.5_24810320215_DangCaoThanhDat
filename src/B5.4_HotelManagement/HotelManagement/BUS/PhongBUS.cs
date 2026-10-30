using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BUS
{
    public static class PhongBUS
    {
        public static List<PhongDTO> GetAll() => PhongDAL.GetAll();
        public static List<PhongDTO> GetTrong(string l) => PhongDAL.GetTrong(l);
        public static System.Data.DataTable GetAllAsDataTable() => PhongDAL.GetAllAsDataTable();
        public static PhongDTO GetBySoPhong(string sp) => PhongDAL.GetBySoPhong(sp);
    }
}