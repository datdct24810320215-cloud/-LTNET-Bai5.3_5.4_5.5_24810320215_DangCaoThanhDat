namespace HotelManagement.DTO
{
    public class SuDungDichVuDTO
    {
        public int MaSD { get; set; }
        public int MaPhieu { get; set; }
        public int MaDV { get; set; }
        public int SoLuong { get; set; }
        public DateTime ThoiGian { get; set; }

        // Hiển thị
        public string TenDV { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
    }
}