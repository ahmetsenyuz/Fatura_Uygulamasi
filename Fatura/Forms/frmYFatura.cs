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
    public partial class frmYFatura : Form
    {
        public frmYFatura()
        {
            InitializeComponent();
        }
        FaturalarEntities db = new FaturalarEntities();

        private void frmYFatura_Load(object sender, EventArgs e)
        {
            ComboBox();
        }
        private void FMaster()
        {
            FatMaster fm = new FatMaster();
            fm.Tarih = DateTime.Now;
            fm.MusteriId = (int)comboBox1.SelectedValue;
            db.Set<FatMaster>().Add(fm);
            db.SaveChanges();
            frmFatura frm = new frmFatura(fm.FatNo,false);
            frm.Show();
        }

        private void ComboBox()
        {
            comboBox1.DataSource = db.Set<Musteri>().Select(x => new
            {
                Id = x.MusteriId,
                Ad = x.MusteriAd+" "+x.MusteriSoyad
            }).ToList();
            comboBox1.ValueMember = "Id";
            comboBox1.DisplayMember = "Ad";
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            FMaster();
        }
    }
}
