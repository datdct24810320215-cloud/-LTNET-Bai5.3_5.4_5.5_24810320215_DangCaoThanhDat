namespace HotelManagement.DTO
{
    public class PhieuDatPhongDTO
    {
        public int MaPhieu { get; set; }
        public string SoPhong { get; set; }
        public int MaKH { get; set; }
        public DateTime NgayNhanPhong { get; set; }
        public DateTime? NgayTraPhong { get; set; }
        public decimal? TongTienThanhToan { get; set; }

        // Cột hiển thị (không lưu DB)
        public string TenKH { get; set; }
        public string LoaiPhong { get; set; }
        public decimal GiaTheoGio { get; set; }
        public string Display => $"#{MaPhieu} - {SoPhong} - {TenKH}";
    }
}