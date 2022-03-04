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
    public partial class frmFatura : Form
    {
        public frmFatura()
        {
            InitializeComponent();
        }
        int gelenFatNo;
        FaturalarEntities db = new FaturalarEntities();
        FatDetay sfd;
        bool _sevk;
        ComboBox cbUrun = new ComboBox();
        Label lbUrun = new Label();
        Button btnOk = new Button();
        public frmFatura(int fatNo,bool sevk)
        {
            InitializeComponent();//formda eklenen consa bunu yazmak şart
            gelenFatNo = fatNo;
            _sevk = sevk;
            btnEkle.Enabled = !_sevk;
            btnGuncel.Enabled = !_sevk;
            btnSil.Enabled = !_sevk;
            Miktar.Visible = !_sevk;
            txtMiktar.Visible = !_sevk;
            btnSevk.Enabled = !_sevk; //fatura sevk edilmişse düzenleme tuşlarını kapatıyoruz.
        }

        private void frmFatura_Load(object sender, EventArgs e)
        {
            if (_sevk == true)
            {
                lbSevk.Text = "BU FATURA SEVK EDİLMİŞ";
            }
            else
            {
                lbSevk.Text = "BU FATURA SEVK EDİLMEMİŞ";
            }
            FatMaster();
            FatDetay(gelenFatNo);
            
        }
        private void FatDetay(int gelenFatNo)
        {
            var a = db.Set<FatMaster>().Where(x => x.FatNo == gelenFatNo).Select(x => new
            { x.Musteri.Ilce.IlceAd, x.Musteri.Ilce.Sehir.SehirAd, x.Musteri.Adres }).ToList();
            listBox1.Items.Clear();
            foreach (var item in a)
            {
                listBox1.Items.Add(item.IlceAd);
                listBox1.Items.Add(item.SehirAd);
                listBox1.Items.Add(item.Adres);
            }
            var fdlist = db.Set<FatDetay>().Select(x => new
            {
                Id = x.FatNo,
                UrunId = x.UrunId,
                Urun = x.Urun.UrunAd,
                Fiyat = x.Bfiyat,
                Miktar = x.Miktar+" "+x.Urun.Birim.BirimAd,
                Toplam =(decimal)x.Miktar*x.Bfiyat
            }).Where(x=> x.Id == gelenFatNo).ToList();
            dataGridView2.DataSource = fdlist;
            for (int i = 0; i < 6; i++)
            {
                dataGridView2.AutoResizeColumn(i);
            }
            //decimal top = 0.0m;
            //foreach (var item in fdlist)
            //{
            //    top = top + item.Toplam.Value;
            //}
            //txtToplam.Text = top.ToString(); ya daaaaaa
            txtToplam.Text = fdlist.Sum(x => x.Toplam).ToString();
        }

        private void FatMaster()
        {
            FatMaster secFat = db.Set<FatMaster>().Find(gelenFatNo);//gelen fatura no'lu fatmasteri çekiyoruz.
            txtFatNo.Text = gelenFatNo.ToString();
            txtTarih.Text = secFat.Tarih.ToString();
            txtAd.Text = secFat.Musteri.MusteriAd;
            txtSoyad.Text = secFat.Musteri.MusteriSoyad;
            txtAdres.Text = secFat.Musteri.Adres;
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int sec1 = (int)dataGridView2.CurrentRow.Cells[0].Value;//2 tane primery key var. O yüzden bulurken hem 1 hem de 2 değerlerine karşılık gelenleri buldurduk.
            int sec2 = (int)dataGridView2.CurrentRow.Cells[1].Value;
            sfd = db.Set<FatDetay>().Find(sec1,sec2);
            txtMiktar.Text = sfd.Miktar.ToString();
        }

        private void btnGuncel_Click(object sender, EventArgs e)
        {
            sfd.Miktar = Convert.ToInt32(txtMiktar.Text);
            db.SaveChanges();
            FatDetay(gelenFatNo);
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            Combo();
            flowLayoutPanel1.Visible = true;
            btnOk.MouseClick += BtnOk_MouseClick;
        }

        private void BtnOk_MouseClick(object sender, MouseEventArgs e)
        {
            //1. Yol
            //try
            //{
            //    FatDetay urunEkle = new FatDetay();
            //    urunEkle.FatNo = gelenFatNo;
            //    urunEkle.UrunId = Convert.ToInt32(cbUrun.SelectedValue);
            //    urunEkle.Miktar = Convert.ToInt32(txtMiktar.Text);
            //    Urun a = db.Set<Urun>().Find(cbUrun.SelectedValue);
            //    urunEkle.Bfiyat = a.BFiyat;
            //    db.Set<FatDetay>().Add(urunEkle);
            //    db.SaveChanges();
            //    FatDetay(gelenFatNo);
            //    flowLayoutPanel1.Visible = false;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //******************************************************************
            //2. Yol
            FatDetay urunEkle = new FatDetay();
            Urun secU = db.Set<Urun>().Find(cbUrun.SelectedValue);
            FatDetay fdCheck = db.Set<FatDetay>().Find(gelenFatNo, secU.UrunId);
            if (fdCheck == null)
            {
                urunEkle.FatNo = gelenFatNo;
                urunEkle.UrunId = Convert.ToInt32(cbUrun.SelectedValue);
                urunEkle.Miktar = Convert.ToInt32(txtMiktar.Text);
                urunEkle.Bfiyat = secU.BFiyat;
                db.Set<FatDetay>().Add(urunEkle);
                db.SaveChanges();
                FatDetay(gelenFatNo);
            }
            else
            {
                MessageBox.Show("Bu ürün zaten sepetinizde mevcut");
            }
            flowLayoutPanel1.Visible = false;
        }

        private void Combo()
        {
            lbUrun.Text = "Ürün";
            btnOk.Text = "Onayla";
            cbUrun.DataSource = db.Set<Urun>().Select(x => new
            {
                Id = x.UrunId,
                Ad = x.UrunAd
            }).ToList();
            cbUrun.DisplayMember = "Ad";
            cbUrun.ValueMember = "Id";
            this.flowLayoutPanel1.Controls.Add(lbUrun);
            flowLayoutPanel1.Controls.Add(cbUrun);
            flowLayoutPanel1.Controls.Add(btnOk);
            txtMiktar.Text = "1";
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            db.Set<FatDetay>().Remove(sfd);
            db.SaveChanges();
            FatDetay(gelenFatNo);
        }

        private void btnSevk_Click(object sender, EventArgs e)
        {
            db.Set<FatMaster>().Find(gelenFatNo).SevkTarih = DateTime.Now;
            db.SaveChanges();
            MessageBox.Show("İrsaliye Basıldı Sevkiyat Hazır");
        }
    }
}
