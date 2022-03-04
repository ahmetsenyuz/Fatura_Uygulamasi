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
    public partial class frmIlce : Form
    {
        public frmIlce()
        {
            InitializeComponent();
        }
        FaturalarEntities db = new FaturalarEntities();
        Ilce ss;
        private void frmIlce_Load(object sender, EventArgs e)
        {
            Doldur();
            cbDoldur();
        }

        private void cbDoldur()
        {
            comboBox1.DataSource = db.Set<Sehir>().Select(x => new
            {
                Id = x.SehirId,
                Ad = x.SehirAd
            }).ToList();
            comboBox1.ValueMember = "Id";
            comboBox1.DisplayMember = "Ad";
        }

        private void Doldur()
        {
            dataGridView1.DataSource = db.Set<Ilce>().Select(x => new
            {
                Id = x.IlceId,
                Ad = x.IlceAd,
                Sehir = x.Sehir.SehirAd
            }).ToList();
            txtId.ReadOnly = true;
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int a = (int)dataGridView1.CurrentRow.Cells[0].Value;
            ss = db.Set<Ilce>().Find(a);
            txtId.Text = ss.IlceId.ToString();
            txtAd.Text = ss.IlceAd;
            comboBox1.SelectedValue = ss.Sehir.SehirId;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            Ilce yi = new Ilce();
            yi.IlceAd = txtAd.Text;
            yi.SehirId = Convert.ToInt32(comboBox1.SelectedValue);
            db.Set<Ilce>().Add(yi);
            db.SaveChanges();
            Doldur();
        }

        private void btnGuncel_Click(object sender, EventArgs e)
        {
            ss.IlceAd = txtAd.Text;
            ss.SehirId = Convert.ToInt32(comboBox1.SelectedValue);
            db.SaveChanges();
            Doldur();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            db.Set<Ilce>().Remove(ss);
            db.SaveChanges();
            Doldur();
        }
    }
}
