using Fatura.Context;
using Fatura.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fatura
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        FaturalarEntities db = new FaturalarEntities();
        Label lb = new Label();
        TextBox txtFatNo = new TextBox();
        Button btnOK = new Button();
        private void ilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSehir frm = new frmSehir();
            frm.Show();
        }

        private void ilçeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIlce frm = new frmIlce();
            frm.Show();
        }

        private void ürünToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUrun frm = new frmUrun();
            frm.Show();
        }

        private void müşteriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMusteri frm = new frmMusteri();
            frm.Show();
        }

        private void birimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBirim frm = new frmBirim();
            frm.Show();
        }

        private void görüntüleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lb.Text = "Fatura No";
            btnOK.Text = "OK!";
            this.flowLayoutPanel1.Controls.Add(lb);
            this.flowLayoutPanel1.Controls.Add(txtFatNo);
            flowLayoutPanel1.Controls.Add(btnOK);
            btnOK.MouseClick += BtnOK_MouseClick;
        }

        private void BtnOK_MouseClick(object sender, MouseEventArgs e)
        {
            FatMaster fm = db.Set<FatMaster>().Find(Convert.ToInt32(txtFatNo.Text));
            if (fm == null)
            {
                MessageBox.Show("Böyle bir fatura mevcut değil.");
            }
            else 
            {
                if (fm.SevkTarih == null)
                {
                    frmFatura frm = new frmFatura(fm.FatNo,false);//Forma başka formdan değer gönderiyoruz!!!
                    frm.Show();//sevk edilmemişse false edilmişse true yolluyoruz
                }
                else
                {
                    frmFatura frm = new frmFatura(fm.FatNo, true);//Forma başka formdan değer gönderiyoruz!!!
                    frm.Show();
                }
            }
        }

        private void girişToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmYFatura frm = new frmYFatura();
            frm.Show();
        }
    }
}
