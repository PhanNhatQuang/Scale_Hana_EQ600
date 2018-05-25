using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Termie
{
    public static class MatHangManager
    {        
        public static DataTable s_DanhSachMatHang = new DataTable();
        public static void init()
        {
            s_DanhSachMatHang = SqlHelper.getAllMatHang();
        }
        public static void refresh()
        {
            s_DanhSachMatHang = SqlHelper.getAllMatHang();
        }
        public static DataTable getMatHangByName(string name)
        {
            return SqlHelper.getMatHangByName(name);
        }
    }
}
