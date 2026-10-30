using System;
using System.Collections.Generic;
using CafePOS.DAL;
using CafePOS.DTO;

namespace CafePOS.BUS
{
    public class MonAnBUS
    {
        private readonly MonAnDAL _dal = new MonAnDAL();

        public List<MonAnDTO> GetAll() => _dal.GetAll();
        public List<MonAnDTO> GetByDanhMuc(int maDM) => _dal.GetByDanhMuc(maDM);

        public void Them(string ten, int maDM, decimal gia)
        {
            if (string.IsNullOrWhiteSpace(ten))
                throw new Exception("Tên món không được để trống!");
            if (gia < 0)
                throw new Exception("Đơn giá không được âm!");
            _dal.Insert(ten.Trim(), maDM, gia);
        }

        public void Sua(int ma, string ten, int maDM, decimal gia)
        {
            if (string.IsNullOrWhiteSpace(ten))
                throw new Exception("Tên món không được để trống!");
            if (gia < 0)
                throw new Exception("Đơn giá không được âm!");
            _dal.Update(ma, ten.Trim(), maDM, gia);
        }

        public void Xoa(int ma) => _dal.Delete(ma);
    }
}