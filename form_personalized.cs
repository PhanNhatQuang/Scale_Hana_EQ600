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
    public partial class form_personalized : Form
    {
        public form_personalized()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            int subA = this.txb_detail_comA.Text.Length / 2;
            int subB = this.txb_detail_comB.Text.Length / 2;
            string detailA1 = this.txb_detail_comA.Text.Substring(0, subA);
            string detailA2 = this.txb_detail_comA.Text.Substring(subA, this.txb_detail_comA.Text.Length - detailA1.Length);
            string detailB1 = this.txb_detail_comB.Text.Substring(0, subB);
            string detailB2 = this.txb_detail_comB.Text.Substring(subB, this.txb_detail_comB.Text.Length - detailB1.Length);

            Settings.personalized.s_app_name = Uri.EscapeDataString(this.txb_AppName.Text);
            Settings.personalized.s_com_name_A = Uri.EscapeDataString(this.txb_name_comA.Text);
            Settings.personalized.s_com_detail_A = Uri.EscapeDataString(detailA1);
            Settings.personalized.s_com_detail_A_2 = Uri.EscapeDataString(detailA2);
            Settings.personalized.s_com_name_B = Uri.EscapeDataString(this.txb_name_comB.Text);
            Settings.personalized.s_com_detail_B = Uri.EscapeDataString(detailB1);
            Settings.personalized.s_com_detail_B_2 = Uri.EscapeDataString(detailB2);


            Settings.Write_personalized();
            MessageBox.Show("Lưu thông tin thành công.");
            this.Close();
        }

        private void form_personalized_Load(object sender, EventArgs e)
        {
            this.txb_AppName.Text = Uri.UnescapeDataString(Settings.personalized.s_app_name);
            this.txb_name_comA.Text = Uri.UnescapeDataString(Settings.personalized.s_com_name_A);
            this.txb_detail_comA.Text = Uri.UnescapeDataString(Settings.personalized.s_com_detail_A) + Uri.UnescapeDataString(Settings.personalized.s_com_detail_A_2);
            this.txb_name_comB.Text = Uri.UnescapeDataString(Settings.personalized.s_com_name_B);
            this.txb_detail_comB.Text = Uri.UnescapeDataString(Settings.personalized.s_com_detail_B) + Uri.UnescapeDataString(Settings.personalized.s_com_detail_B_2);
        }
    }
}
