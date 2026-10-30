namespace HotelManagement.DTO
{
    public class DichVuDTO
    {
        public int MaDV { get; set; }
        public string TenDV { get; set; }
        public decimal DonGia { get; set; }
        public override string ToString() => $"{TenDV} - {DonGia:N0}đ";
    }
}