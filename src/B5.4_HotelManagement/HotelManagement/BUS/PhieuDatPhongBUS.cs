using HotelManagement.DAL;
using HotelManagement.DTO;

namespace HotelManagement.BUS
{
    public static class PhieuDatPhongBUS
    {
        public static int CheckIn(PhieuDatPhongDTO p)
        {
            if (string.IsNullOrEmpty(p.SoPhong))
                throw new Exception("Chưa chọn phòng!");
            if (p.MaKH <= 0)
                throw new Exception("Chưa có khách hàng!");

            int maPhieu = PhieuDatPhongDAL.Insert(p);
            PhongDAL.UpdateTrangThai(p.SoPhong, "Đang ở");
            return maPhieu;
        }

        public static List<PhieuDatPhongDTO> GetAllActive() => PhieuDatPhongDAL.GetAllActive();

        public static decimal TinhTongTien(PhieuDatPhongDTO p, out decimal tienPhong, out decimal tienDV)
        {
            // Thời gian ở (làm tròn lên giờ)
            double hours = (DateTime.Now - p.NgayNhanPhong).TotalHours;
            int billedHours = (int)Math.Ceiling(hours);
            if (billedHours < 1) billedHours = 1;

            tienPhong = billedHours * p.GiaTheoGio;

            tienDV = 0;
            var ds = SuDungDichVuDAL.GetByMaPhieu(p.MaPhieu);
            foreach (var s in ds) tienDV += s.ThanhTien;

            return tienPhong + tienDV;
        }

        public static void Checkout(PhieuDatPhongDTO p, decimal tongTien)
        {
            PhieuDatPhongDAL.Checkout(p.MaPhieu, p.SoPhong, DateTime.Now, tongTien);
        }
    }
}