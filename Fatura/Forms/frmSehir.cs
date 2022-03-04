using Fatura.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fatura.Forms
{
    public partial class frmSehir : Form
    {
        public frmSehir()
        {
            InitializeComponent();
        }
        FaturalarEntities db = new FaturalarEntities();
        Sehir ss;
        private void frmSehir_Load(object sender, EventArgs e)
        {
            Doldur();
        }

        private void Doldur()
        {
            dataGridView1.DataSource = db.Set<Sehir>().Select(x => new
            {
                Id = x.SehirId,
                Ad = x.SehirAd
            }).ToList();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int a = (int)dataGridView1.CurrentRow.Cells[0].Value;
            ss = db.Set<Sehir>().Find(a);
            txtId.Text = ss.SehirId.ToString();
            txtAd.Text = ss.SehirAd.ToString();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            Sehir ys = new Sehir();
            ys.SehirAd = txtAd.Text;
            ys.SehirId = Convert.ToInt32(txtId.Text);
            db.Set<Sehir>().Add(ys);
            db.SaveChanges();
            Doldur();
        }

        private void btnGuncel_Click(object sender, EventArgs e)
        {
            ss.SehirAd = txtAd.Text;
            ss.SehirId = Convert.ToInt32(txtId.Text);
            db.SaveChanges();
            Doldur();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            db.Set<Sehir>().Remove(ss);
            db.SaveChanges();
            Doldur();
        }
    }
}
