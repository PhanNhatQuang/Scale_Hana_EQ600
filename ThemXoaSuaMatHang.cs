using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Termie
{
    public partial class ThemXoaSuaMatHang : Form
    {
        public ThemXoaSuaMatHang()
        {
            InitializeComponent();
        }

        private void ThemXoaSuaMatHang_Load(object sender, EventArgs e)
        {
            this.dataGridView2.DataSource = MatHangManager.s_DanhSachMatHang;
            this.dataGridView2.Columns["Name"].HeaderText = "Tên";
            this.dataGridView2.Columns["Price"].HeaderText = "Giá";
            this.dataGridView2.Columns["HotKey"].HeaderText = "Phím tắt";
            this.Edit_container.Visible = false;
            this.cbb_HotKey_Add.SelectedIndex = 0;
            this.cbb_HotKey_Edit.SelectedIndex = 0;
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa mặt hàng.\nDữ liệu sau khi xóa sẽ không thể khôi phục được.", "Cảnh báo", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //do something
                int ID = int.Parse(this.dataGridView2.CurrentRow.Cells["ID"].Value.ToString());
                SqlHelper.DeleteMatHang(ID);
                MatHangManager.refresh();
                this.dataGridView2.DataSource = MatHangManager.s_DanhSachMatHang;
                MessageBox.Show("Xóa mặt hàng thành công.");
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
            

        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            string value = this.dataGridView2.CurrentRow.Cells["Price"].Value.ToString();
            int ivalue;
            bool isValidValue = int.TryParse(value,
                             NumberStyles.Integer | NumberStyles.AllowThousands,
                             CultureInfo.GetCultureInfo("en-US"),
                             out ivalue);
            this.txb_Name_Edit.Text = this.dataGridView2.CurrentRow.Cells["Name"].Value.ToString();
            this.numeric_Price_Edit.Value = ivalue;
            this.cbb_HotKey_Edit.SelectedIndex = int.Parse(this.dataGridView2.CurrentRow.Cells["HotKey"].Value.ToString());
            this.Edit_container.Visible = true;
            this.Add_container.Visible = false;

        }

        private void btn_SaveEdit_Click(object sender, EventArgs e)
        {
            if (this.cbb_HotKey_Edit.SelectedIndex != 0)
            {
                foreach (DataRow dr in MatHangManager.s_DanhSachMatHang.Rows)
                {
                    //string a = dr["HotKey"].ToString();
                    if (int.Parse(dr["HotKey"].ToString()) == this.cbb_HotKey_Edit.SelectedIndex)
                    {
                        string value = dr["Price"].ToString();
                        int ivalue;
                        bool isValidValue = int.TryParse(value,
                                         NumberStyles.Integer | NumberStyles.AllowThousands,
                                         CultureInfo.GetCultureInfo("en-US"),
                                         out ivalue);
                        SqlHelper.UpdateMatHang(int.Parse(dr["ID"].ToString()),
                                               dr["Name"].ToString(),
                                               ivalue,
                                               0);
                    }
                }
            }
            SqlHelper.UpdateMatHang(int.Parse(this.dataGridView2.CurrentRow.Cells["ID"].Value.ToString()),this.txb_Name_Edit.Text, (int)this.numeric_Price_Edit.Value, this.cbb_HotKey_Edit.SelectedIndex);
            MatHangManager.refresh();
            this.dataGridView2.DataSource = MatHangManager.s_DanhSachMatHang;
            this.Edit_container.Visible = false;
            this.Add_container.Visible = true;
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (!this.txb_Name_Add.Text.Equals(""))
            {
                
                if (this.cbb_HotKey_Add.SelectedIndex != 0)
                {
                   foreach (DataRow dr in MatHangManager.s_DanhSachMatHang.Rows)
                   {
                        string a = dr["HotKey"].ToString();
                        if (int.Parse(dr["HotKey"].ToString()) == this.cbb_HotKey_Add.SelectedIndex)
                        {
                            string value = dr["Price"].ToString();
                            int ivalue;
                            bool isValidValue = int.TryParse(value,
                                             NumberStyles.Integer | NumberStyles.AllowThousands,
                                             CultureInfo.GetCultureInfo("en-US"),
                                             out ivalue);

                            SqlHelper.UpdateMatHang(int.Parse(dr["ID"].ToString()),
                                                   dr["Name"].ToString(),
                                                   ivalue,
                                                   0);
                        }
                    }
                }
                SqlHelper.InsertMatHang(this.txb_Name_Add.Text, (int)this.numeric_Price_Add.Value, this.cbb_HotKey_Add.SelectedIndex);
                MatHangManager.refresh();
                this.dataGridView2.DataSource = MatHangManager.s_DanhSachMatHang;
            }
        }
    }
}
