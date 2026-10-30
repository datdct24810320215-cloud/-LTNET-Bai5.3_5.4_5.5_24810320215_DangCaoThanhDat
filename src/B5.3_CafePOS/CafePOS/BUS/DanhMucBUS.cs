using System;
using System.Collections.Generic;
using CafePOS.DAL;
using CafePOS.DTO;

namespace CafePOS.BUS
{
    public class DanhMucBUS
    {
        private readonly DanhMucDAL _dal = new DanhMucDAL();

        public List<DanhMucDTO> GetAll() => _dal.GetAll();

        public void Them(string ten)
        {
            if (string.IsNullOrWhiteSpace(ten))
                throw new Exception("Tên danh mục không được để trống!");
            _dal.Insert(ten.Trim());
        }

        public void Sua(int ma, string ten)
        {
            if (string.IsNullOrWhiteSpace(ten))
                throw new Exception("Tên danh mục không được để trống!");
            _dal.Update(ma, ten.Trim());
        }

        public void Xoa(int ma) => _dal.Delete(ma);
    }
}