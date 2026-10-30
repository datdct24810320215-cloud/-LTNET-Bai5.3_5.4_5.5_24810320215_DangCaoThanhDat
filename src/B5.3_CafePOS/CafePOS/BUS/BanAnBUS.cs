using System.Collections.Generic;
using CafePOS.DAL;
using CafePOS.DTO;

namespace CafePOS.BUS
{
    public class BanAnBUS
    {
        private readonly BanAnDAL _dal = new BanAnDAL();
        public List<BanAnDTO> GetAll() => _dal.GetAll();
        public void CapNhatTrangThai(int maBan, string tt) => _dal.UpdateTrangThai(maBan, tt);
    }
}