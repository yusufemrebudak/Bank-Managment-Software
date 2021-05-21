using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank_Managment_System
{
    public partial class YoneticiForm : Form
    {
        public static OleDbConnection _db;
        Yonetici _Yonetici;
        public YoneticiForm()
        {
            InitializeComponent();
        }
        public YoneticiForm(DataTable yon_table, int KayitNo, OleDbConnection db)
        {
            InitializeComponent();
            _db = db;
            _Yonetici = new Yonetici(yon_table, KayitNo, db);
            welcome_manager_label.Text = "Hoşgeldiniz : " + _Yonetici.ad + " " + _Yonetici.soyad + " / Müşteri Numarası : " + _Yonetici.no;
        }

        private void banka_islem_hareketleri_button_Click(object sender, EventArgs e)
        {
            string query = "select hh_tarih as Tarih,hh_mus_id as MusNo,hh_ad as Ad,hh_soyad as Soyad,hh_banka_metin as Acıklama,hh_banka_tutar as BankaKazanc from hesap_hareketleri where hh_tarih BETWEEN @date1 and @date2";
            _db.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            adtr.SelectCommand.Parameters.AddWithValue("@date1", dateTimePicker1.Value.ToString("dd/MM/yyyy"));
            adtr.SelectCommand.Parameters.AddWithValue("@date2", dateTimePicker2.Value.ToString("dd/MM/yyyy"));
            DataTable _table = new DataTable();
            adtr.Fill(_table);
            YoneticiBankaHareketleriDataGridView.DataSource = _table;
            _db.Close();
            YoneticiBankaHareketleriDataGridView.Columns[0].Width = 80;
            YoneticiBankaHareketleriDataGridView.Columns[1].Width = 70;
            YoneticiBankaHareketleriDataGridView.Columns[2].Width = 70;
            YoneticiBankaHareketleriDataGridView.Columns[3].Width = 70;
            YoneticiBankaHareketleriDataGridView.Columns[4].Width = 480;
            YoneticiBankaHareketleriDataGridView.Columns[5].Width = 140;
            double BankButce = 0;
            for (int i = 0; i < YoneticiBankaHareketleriDataGridView.Rows.Count; i++) BankButce += Convert.ToDouble(YoneticiBankaHareketleriDataGridView.Rows[i].Cells[5].Value);  
            Toplamlabel.Text = "Banka Hesap : "+ Convert.ToString(BankButce)+" TL";
        }

        private void hesap_ac_onayla_button_Click(object sender, EventArgs e)
        {
            int MusNo = Convert.ToInt32(hesap_ac_musteri_no_textBox.Text);
            string ManagerSifre = hesap_ac_yonetici_sifre_textBox.Text;
            string query = "select * from musteriler where mus_id=" + MusNo + "";
            _db.Open();
            OleDbCommand command = new OleDbCommand(query, _db);
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read() == true)
            {
                if (_Yonetici.sifre == ManagerSifre)
                {
                    if (BankaHesapradioButton.Checked == true)  _Yonetici.HesapAc(MusNo, "Banka");
                    
                    else if (DovizradioButton.Checked == true)  _Yonetici.HesapAc(MusNo, "Doviz");
                    
                    else MessageBox.Show("Hesap Tipi Seçmelisiniz!");
                }
                else MessageBox.Show("Yönetici şifreniz YANLIŞ!");
            }
            else MessageBox.Show("İlgili müşteri bulunamadı.");
            _db.Close();
        }

        private void musteri_listele_onay_button_Click(object sender, EventArgs e)
        {
            string query = "select mus_id as MusNo,mus_ad as Ad,mus_soyad as Soyad,mus_tc as Tc,mus_adres as Adres,mus_tel as Tel from musteriler ";
            _db.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            DataTable _table = new DataTable();
            adtr.Fill(_table);
            dataGridView3.DataSource = _table;
            _db.Close();

        }

        private void arama_textBox_TextChanged(object sender, EventArgs e)
        {
            _db.Open();
            string query = "select mus_id as MusNo,mus_ad as Ad,mus_soyad as Soyad,mus_tc as Tc,mus_adres as Adres,mus_tel as Tel from musteriler where mus_ad like '" + arama_textBox.Text + "%'or  mus_tc like '" + arama_textBox.Text + "%'or mus_adres like'" + arama_textBox.Text + "%'";
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            DataTable _table = new DataTable();
            adtr.Fill(_table);
            dataGridView3.DataSource = _table;
            _db.Close();
        }

        private void musteri_sil_musteri_listele_button_Click(object sender, EventArgs e)
        {
            string query = "select mus_id as MusNo,mus_ad as Ad,mus_soyad as Soyad,mus_tc as Tc,mus_adres as Adres,mus_tel as Tel from musteriler ";
            _db.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            DataTable _table = new DataTable();
            adtr.Fill(_table);
            dataGridView2.DataSource = _table;
            _db.Close();
        }

        private void musteri_sil_button_Click(object sender, EventArgs e)
        {
            int MusNo = Convert.ToInt32(musteri_sil_mus_no_textBox.Text);
            string YonSifre = musteri_sil_yonetici_sifre_textBox.Text;
            string query = "select * from hesaplar where mus_id= " + MusNo + "";
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            DataTable _table = new DataTable();
            adtr.Fill(_table);
            _db.Open();
            if (YonSifre == _Yonetici.sifre)
            {
                for (int i = 0; i < _table.Rows.Count; i++)
                {
                    int HesapNo = Convert.ToInt32(_table.Rows[i]["hesap_no"].ToString());
                    string HesapTur = _table.Rows[i]["hesap_tur"].ToString();
                    if (HesapTur == "Banka")
                    {
                        OleDbCommand SilBankaHesaplarCommand = new OleDbCommand("delete * from banka_hesaplar where hesap_no = " + HesapNo + "", _db);
                        SilBankaHesaplarCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        OleDbCommand SilDovizHesaplarCommand = new OleDbCommand("delete * from doviz_hesaplar where hesap_no = " + HesapNo + "", _db);
                        SilDovizHesaplarCommand.ExecuteNonQuery();
                    }
                }
                OleDbCommand SilMusCommand = new OleDbCommand("delete * from musteriler where mus_id = " + MusNo + "", _db);
                OleDbCommand SilHesaplarCommand = new OleDbCommand("delete * from hesaplar where mus_id = " + MusNo + "", _db);
                if (SilMusCommand.ExecuteNonQuery() == 0 || SilHesaplarCommand.ExecuteNonQuery() == 0) MessageBox.Show("Hesap Silinemedi");
                else MessageBox.Show(MusNo+" Numaralı Müşteri Başarıyla Silindi");
            }
            else MessageBox.Show("Şifrenizi Yanlış girdiniz");
            _db.Close();
        }

        private void HesapListele_Click(object sender, EventArgs e)
        {
            _db.Open();
            string query = "select mus_id as MusNo, hesap_no as HesapNo, hesap_tur as HesapTürü from hesaplar";
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            DataTable _table = new DataTable();
            adtr.Fill(_table);
            dataGridView5.DataSource = _table;
            _db.Close();
        }

        private void hesap_sil_hesap_listele_button_Click(object sender, EventArgs e)
        {
            string query = "select musteriler.mus_ad as Ad, musteriler.mus_soyad as Soyad , hesaplar.hesap_no as HesapNo,hesaplar.hesap_tur as HesapTürü from hesaplar inner join musteriler on hesaplar.mus_id=musteriler.mus_id  ";
            _db.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            DataTable _table = new DataTable();
            _table.Clear();
            adtr.Fill(_table);
            dataGridView6.DataSource = _table;
            _db.Close();
        }

        private void hesap_sil_button_Click(object sender, EventArgs e)
        {
            int HesapNo = Convert.ToInt32(hesap_sil_hesap_no_textBox.Text);
            string YonSifre = hesap_sil_yonetici_sifre_textBox.Text;
            string query = "select * from hesaplar where hesap_no= " + HesapNo + "";
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            DataTable _table = new DataTable();
            adtr.Fill(_table);
            string HesapTur =_table.Rows[0]["hesap_tur"].ToString();
            _db.Open();
            if (YonSifre == _Yonetici.sifre)
            {
                if (HesapTur == "Banka")
                {
                    OleDbCommand SildBankaHesapCommand = new OleDbCommand("delete * from banka_hesaplar where hesap_no = " + HesapNo + "", _db);
                    SildBankaHesapCommand.ExecuteNonQuery();
                }
                else
                {
                    OleDbCommand SilDovizHesapCommand = new OleDbCommand("delete * from doviz_hesaplar where hesap_no = " + HesapNo + "", _db);
                    SilDovizHesapCommand.ExecuteNonQuery();

                }
                OleDbCommand SilHesapCommand = new OleDbCommand("delete * from hesaplar where hesap_no = " + HesapNo + "", _db);
                if (SilHesapCommand.ExecuteNonQuery() == 0) MessageBox.Show(HesapNo + " Numaralı Hesap Silinemedi");
                else MessageBox.Show("Hesap Silindi/İşlem Başarılı");
                _db.Close();
            }
        }
    }
}
