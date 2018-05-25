using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Termie
{
    public partial class form_Report : Form
    {
        public form_Report()
        {
            InitializeComponent();
        }
        private DataTable m_table_Report = new DataTable();
        private void datagridview1_setHeaderName()
        {
            this.dataGridView1.Columns["ID"].Visible = false;
            this.dataGridView1.Columns["Name"].HeaderText = "Tên";
            this.dataGridView1.Columns["Value"].HeaderText = "Giá trị";
            this.dataGridView1.Columns["Unit"].HeaderText = "Đơn vị";
            this.dataGridView1.Columns["DateTime"].HeaderText = "Ngày lưu";
            this.dataGridView1.Columns["ThanhTien"].HeaderText = "Thành tiền";
        }
        private void btn_Report_Click(object sender, EventArgs e)
        {
            //if (this.checkBox1.Checked)
            //{
            //    SqlHelper.getAllValues();
            //    m_table_Report = SqlHelper.s_table_AllModels;
                
            //}
            //else
            {
                SqlHelper.getAllValuesFromTo(this.dateTimeFrom.Value, this.dateTimePicker1.Value);
                m_table_Report = SqlHelper.s_table_ModelsFromTo;
            }
            this.dataGridView1.DataSource = m_table_Report;
            datagridview1_setHeaderName();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Excel_Export.Export(m_table_Report,"Report", "HỆ THỐNG CÂN KỸ THUẬT CƯỜNG THỊNH PHÁT");
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.dataGridView1.DataSource;
            if (dt.Rows.Count > 0)
            {
                this.btn_Export.Enabled = true;
                this.btn_XoaDuLieu.Enabled = true;
            }
            else
            {
                this.btn_Export.Enabled = false;
                this.btn_XoaDuLieu.Enabled = false;
            }
        }

        private void btn_XoaDuLieu_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu.\nDữ liệu sau khi xóa sẽ không thể khôi phục được.", "Cảnh báo", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //do something
                SqlHelper.DeleteAllValuesFromTo(this.dateTimeFrom.Value, this.dateTimePicker1.Value);
                MessageBox.Show("Xóa dữ liệu thành công.");
                btn_Report_Click(sender, e);
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
            
        }
    }
}
