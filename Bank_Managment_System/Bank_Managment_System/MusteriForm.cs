using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Resources.ResXFileRef;



namespace Bank_Managment_System
{
    
    partial class MusteriForm : Form
    {
        public static OleDbConnection _db;
        Musteri _Musteri;
        public MusteriForm(DataTable mus_table , int KayitNo, OleDbConnection db)
        {
            InitializeComponent();
            _db = db;
            _Musteri = new Musteri(KayitNo, mus_table, db);
            giris_welcome.Text = "Hoşgeldiniz : "+ _Musteri.ad+" " + _Musteri.soyad+" / Müşteri Numarası : "+_Musteri.no;
            DovizHesaplariTanimla();
            BankaHesaplariTanimla();
        }

        private void para_cek_onay_button_Click(object sender, EventArgs e)
        {
            
        }

        private void iban_onay_button_Click(object sender, EventArgs e)
        {
            int KaynakHesapNo = Convert.ToInt32(ParaTransferKaynakBankaHesapcomboBox.Text);
            int HedefHesapNo = Convert.ToInt32(hedef_hesap_textBox.Text);
            double TransferMiktar = Convert.ToDouble(transfer_miktar_textBox.Text);
            Boolean HesapKontrol = HesaplarKontrol(KaynakHesapNo, HedefHesapNo);
            Boolean BakiyeMiktarKontrol = BakiyeKontrol(KaynakHesapNo, TransferMiktar);

            if ( HesapKontrol == true && BakiyeMiktarKontrol == true )
            {
                if(TransferMiktar<=500)
                {
                    double BankaTutar = 1;
                    if (MessageBox.Show("Bankanız bu işlem için sizden " + BankaTutar + " TRY kesinti yapacaktır.İşlemi Onaylıyor musunuz?", "Message", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _Musteri.HesapParaGonder(HedefHesapNo, BankaTutar, TransferMiktar);
                    }
                    
                }
                else if (TransferMiktar > 500 && TransferMiktar <= 1000)
                {
                    double BankaTutar = 5;
                    if (MessageBox.Show("Bankanız bu işlem için sizden " + BankaTutar + " TRY kesinti yapacaktır.İşlemi Onaylıyor musunuz?", "Message", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _Musteri.HesapParaGonder(HedefHesapNo, BankaTutar, TransferMiktar);
                    }
                }
                else if (TransferMiktar > 1000 && TransferMiktar <= 10000)
                {
                    double BankaTutar = 10;
                    if (MessageBox.Show("Bankanız bu işlem için sizden " + BankaTutar + " TRY kesinti yapacaktır.İşlemi Onaylıyor musunuz?", "Message", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _Musteri.HesapParaGonder(HedefHesapNo, BankaTutar, TransferMiktar);
                    }
                }
                else
                {
                    double BankaTutar = 50;
                    if (MessageBox.Show("Bankanız bu işlem için sizden " + BankaTutar + " TRY kesinti yapacaktır.İşlemi Onaylıyor musunuz?", "Message", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _Musteri.HesapParaGonder(HedefHesapNo, BankaTutar, TransferMiktar);
                    }
                }


            }
            else if ( HesapKontrol == false) MessageBox.Show("Geçersiz Hesap No");
            else MessageBox.Show("Yetersiz bakiye");
        }

        private bool BakiyeKontrol(int kaynakHesapNo, double Miktar)
        {
            foreach (BankaHesap BH in _Musteri._BankaHesaplar)
            {
                if (BH.HesapNo == kaynakHesapNo)
                {
                    if (BH.HesapMiktar >= Miktar)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool HesaplarKontrol(int kaynakHesapNo, int hedefHesapNo)
        {
            Boolean kaynakHesapKontrol = BankaHesapKontrol(kaynakHesapNo);
            string query = "select * from banka_hesaplar where hesap_no=" + hedefHesapNo + "";
            OleDbCommand command = new OleDbCommand(query, _db);
            _db.Open();
            OleDbDataReader reader = command.ExecuteReader();
            Boolean hedefHesapKontrol = reader.Read();
            reader.Close(); _db.Close();
            if (kaynakHesapKontrol && hedefHesapKontrol) return true;
            else return false;
        }

        private void BankaHesaplariTanimla()
        {
            string query = "select musteriler.mus_id,banka_hesaplar.hesap_no, banka_hesaplar.bh_miktar" +
             " from (musteriler inner join hesaplar on musteriler.mus_id=hesaplar.mus_id)" +
             " inner join banka_hesaplar on banka_hesaplar.hesap_no=hesaplar.hesap_no where musteriler.mus_id=" + _Musteri.no + "";
            _db.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            DataTable _table = new DataTable();
            adtr.Fill(_table);
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                _Musteri._BankaHesap = new BankaHesap(_table, i, _db);
                _Musteri._BankaHesaplar.Add(_Musteri._BankaHesap);
            }
            _db.Close();
        }

        private void DovizHesaplariTanimla()
        {
            string query = "select musteriler.mus_id,doviz_hesaplar.hesap_no, doviz_hesaplar.tl_miktar, doviz_hesaplar.dolar_miktar," +
            " doviz_hesaplar.euro_miktar, doviz_hesaplar.sterlin_miktar from (musteriler inner join hesaplar on musteriler.mus_id=hesaplar.mus_id)" +
            " inner join doviz_hesaplar on doviz_hesaplar.hesap_no=hesaplar.hesap_no where musteriler.mus_id=" + _Musteri.no + "";
            _db.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            DataTable _table = new DataTable();
            adtr.Fill(_table);
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                _Musteri._DovizHesap = new DovizHesap(_table, i, _db);
                _Musteri._DovizHesaplar.Add(_Musteri._DovizHesap);
            }
            _db.Close();
        }

        private void para_transfer_hesap_listele_Click(object sender, EventArgs e)
        {
            foreach (BankaHesap BH in _Musteri._BankaHesaplar)
            {
                if (_Musteri._BankaHesaplar.Count == ParaTransferKaynakBankaHesapcomboBox.Items.Count) break;
                ParaTransferKaynakBankaHesapcomboBox.Items.Add(BH.HesapNo);
            }
            string query = "select hesaplar.mus_id as MusteriNo,hesaplar.hesap_no as HesapNo,hesaplar.hesap_tur as HesapTürü, banka_hesaplar.bh_miktar as Miktar from hesaplar inner join banka_hesaplar on hesaplar.hesap_no=banka_hesaplar.hesap_no where hesaplar.mus_id=" + _Musteri.no + "";
            _db.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            DataTable _table = new DataTable();
            _table.Clear();
            adtr.Fill(_table);
            para_transferleri_hesaplarım_DataGridView.DataSource = _table;
            _db.Close();
        }

        private void Gonderilecek_hesaplar_button_Click(object sender, EventArgs e)
        {
            string query = "select musteriler.mus_ad as Ad, musteriler.mus_soyad as Soyad , hesaplar.hesap_no as HesapNo,hesaplar.hesap_tur as HesapTürü, banka_hesaplar.bh_miktar as Miktar from (hesaplar inner join musteriler on hesaplar.mus_id=musteriler.mus_id) inner join banka_hesaplar on hesaplar.hesap_no=banka_hesaplar.hesap_no where hesaplar.hesap_tur='" + "Banka"+"'";
            _db.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            DataTable _table = new DataTable();
            _table.Clear();
            adtr.Fill(_table);
            gonderilebilecek_hesaplar_DataGridView.DataSource = _table;
            _db.Close();
        }

        private void arama_textBox_TextChanged(object sender, EventArgs e)
        {
            _db.Open();
            string query = "select musteriler.mus_ad as Ad, musteriler.mus_soyad as Soyad , hesaplar.hesap_no as HesapNo,hesaplar.hesap_tur as HesapTürü from hesaplar inner join musteriler on hesaplar.mus_id=musteriler.mus_id where musteriler.mus_ad like '"+ arama_textBox.Text+"%'or  hesaplar.hesap_no like '" + arama_textBox.Text + "%'";
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            DataTable _table = new DataTable();
            // _table.Clear();
            adtr.Fill(_table);
            gonderilebilecek_hesaplar_DataGridView.DataSource = _table;
            _db.Close();
        }


        private void To_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string from = from_comboBox.Text;
            string to = To_comboBox.Text;
            API_Obj Test = Rates.Import(from);
            switch (to)
            {
                case "USD" :
                    birimFiyat_textBox.Text = Convert.ToString(Test.conversion_rates.USD);
                    break;
                case "TRY" :
                    birimFiyat_textBox.Text = Convert.ToString(Test.conversion_rates.TRY);
                    break;
                case "EUR":
                    birimFiyat_textBox.Text = Convert.ToString(Test.conversion_rates.EUR);
                    break;
                case "GBP":
                    birimFiyat_textBox.Text = Convert.ToString(Test.conversion_rates.GBP);
                    break;
                default:
                    break;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            double ToplamTutar = 0;
            if (donusturulecek_tutar_textBox.Text != "")
            {
                ToplamTutar = Convert.ToDouble(birimFiyat_textBox.Text) * Convert.ToDouble(donusturulecek_tutar_textBox.Text);
                donusum_textBox.Text = Convert.ToString(ToplamTutar);
            }
            else donusum_textBox.Text = "";
            DovizHesapKontrol(Convert.ToInt32(dovizDonusturmeDovizHesapNoCombobox.Text));
            if( DovizDonusturmeHesapKontrol(from_comboBox.Text,donusturulecek_tutar_textBox.Text) == false ) MessageBox.Show("Dönüştürülecek olan para biriminizde yeterli miktar yoktur.");
        }

        private bool DovizDonusturmeHesapKontrol(string from,string DonusturulecekTutar)
        {
            if (DonusturulecekTutar != "")
            {
                switch (from)
                {
                    case "USD":
                        if (_Musteri._DovizHesap.Dollar >= Convert.ToDouble(DonusturulecekTutar)) return true;
                        else return false;
                        //break;
                    case "TRY":
                        if (_Musteri._DovizHesap.TL >= Convert.ToDouble(DonusturulecekTutar)) return true;
                        else return false;
                        //break;
                    case "EUR":
                        if (_Musteri._DovizHesap.Euro >= Convert.ToDouble(DonusturulecekTutar)) return true;
                        else return false;
                        //break;
                    case "GBP":
                        if (_Musteri._DovizHesap.Sterlin >= Convert.ToDouble(DonusturulecekTutar)) return true;
                        else return false;
                        //break;
                    default:
                        break;
                }
            }
            return true;   
        }

        private void DovizDonusturOnay_Button_Click(object sender, EventArgs e)
        {
            int DovizHesapNo = Convert.ToInt32(dovizDonusturmeDovizHesapNoCombobox.Text);
            bool dovizHesapKontrol = DovizHesapKontrol(DovizHesapNo);
            bool dovizHesapBakiyeKontrol = DovizDonusturmeHesapKontrol(from_comboBox.Text, donusturulecek_tutar_textBox.Text);
            if (dovizHesapKontrol == true && dovizHesapBakiyeKontrol == true)
            {
                _Musteri.DovizTra(from_comboBox.Text, To_comboBox.Text, donusturulecek_tutar_textBox.Text,donusum_textBox.Text);
            }
            else MessageBox.Show("Dönüştürülecek olan para biriminizde yeterli miktar yoktur.");
        }

        private void DovizHesapListele_button_Click(object sender, EventArgs e)
        {
            foreach (DovizHesap DH in _Musteri._DovizHesaplar)
            {
                if (_Musteri._DovizHesaplar.Count == dovizDonusturmeDovizHesapNoCombobox.Items.Count) break;
                dovizDonusturmeDovizHesapNoCombobox.Items.Add(DH.HesapNo);
            }
            string query = "select hesaplar.mus_id as MusteriNo,hesaplar.hesap_no as HesapNo,doviz_hesaplar.tl_miktar as Tl,doviz_hesaplar.dolar_miktar as Dollar,doviz_hesaplar.euro_miktar as Euro,doviz_hesaplar.sterlin_miktar as Sterlin from hesaplar inner join doviz_hesaplar on hesaplar.hesap_no=doviz_hesaplar.hesap_no where hesaplar.mus_id=" + _Musteri.no + "";
            _db.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            DataTable _table = new DataTable();
            adtr.Fill(_table);
            DovizHesaplar_dataGridView.DataSource = _table;
            _db.Close();
        }

        private void DovizBankaHesaplarListele_Click(object sender, EventArgs e)
        {
            foreach (DovizHesap DH in _Musteri._DovizHesaplar)
            {
                if (_Musteri._DovizHesaplar.Count == DovizAlDovizHesapNocomboBox.Items.Count) break;
                DovizAlDovizHesapNocomboBox.Items.Add(DH.HesapNo);
            }
            foreach (BankaHesap BH in _Musteri._BankaHesaplar)
            {
                if (_Musteri._BankaHesaplar.Count == DovizAlBankaHesapNocomboBox.Items.Count) break;
                DovizAlBankaHesapNocomboBox.Items.Add(BH.HesapNo);
            }
            string query = "select hesaplar.mus_id as MusteriNo,hesaplar.hesap_no as HesapNo,hesaplar.hesap_tur as HesapTürü, banka_hesaplar.bh_miktar as Miktar from hesaplar inner join banka_hesaplar on hesaplar.hesap_no=banka_hesaplar.hesap_no where hesaplar.mus_id=" + _Musteri.no + "";
            _db.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            DataTable _table = new DataTable();
            adtr.Fill(_table);
            DovizBankaHesapdataGridView.DataSource = _table;
            _db.Close();
        }

        private void DovizAlTutartextBox_TextChanged(object sender, EventArgs e)
        {
            string AlinanDoviz = DovizAlcomboBox.Text;
            API_Obj Test = Rates.Import("TRY");
            if (DovizAlTutartextBox.Text != "")
            {
                switch (AlinanDoviz)
                {
                    case "USD":
                        DovizAlDonusumTutartextBox.Text = Convert.ToString(Test.conversion_rates.USD * Convert.ToDouble(DovizAlTutartextBox.Text));
                    break;
                    case "TRY":
                        DovizAlDonusumTutartextBox.Text = Convert.ToString(Test.conversion_rates.TRY * Convert.ToDouble(DovizAlTutartextBox.Text));
                    break;
                    case "EUR":
                        DovizAlDonusumTutartextBox.Text = Convert.ToString(Test.conversion_rates.EUR * Convert.ToDouble(DovizAlTutartextBox.Text));
                    break;
                    case "GBP":
                        DovizAlDonusumTutartextBox.Text = Convert.ToString(Test.conversion_rates.GBP * Convert.ToDouble(DovizAlTutartextBox.Text));
                    break;
                    default:
                        break;
                }

            }
            else DovizAlDonusumTutartextBox.Text = "";
        }

        private void DovizAlOnayButton_Click(object sender, EventArgs e)
        {
            int BankaHesapNo = Convert.ToInt32(DovizAlBankaHesapNocomboBox.Text);
            int DovizHesapNo = Convert.ToInt32(DovizAlDovizHesapNocomboBox.Text);
            bool dovizHesapKontrol = DovizHesapKontrol(DovizHesapNo);
            bool bankaHesapKontrol = BankaHesapKontrol(BankaHesapNo);
            double Tutar = Convert.ToDouble(DovizAlTutartextBox.Text);
            bool bakiyeKontrol = BakiyeKontrol(BankaHesapNo, Tutar);
            double DonusumTutar = Convert.ToDouble(DovizAlDonusumTutartextBox.Text);
            string DonusturulecekParaBirimi = DovizAlcomboBox.Text;
            if (bankaHesapKontrol == true && bakiyeKontrol == true && dovizHesapKontrol ==true) _Musteri.DovizAl(DonusturulecekParaBirimi, Tutar, DonusumTutar);
            else if (bankaHesapKontrol == false || dovizHesapKontrol == false) MessageBox.Show("Geçersiz Hesap No");
            else MessageBox.Show("Yetersiz bakiye");

        }

        private bool BankaHesapKontrol(int hesapNo)
        {
            foreach (BankaHesap BH in _Musteri._BankaHesaplar)
            {
                if (BH.HesapNo == hesapNo)
                {
                    _Musteri._BankaHesap = BH;
                    return true;
                }
            }
            return false;
        }
        private bool DovizHesapKontrol(int hesapNo)
        {
            foreach (DovizHesap DH in _Musteri._DovizHesaplar)
            {
                if (DH.HesapNo == hesapNo)
                {
                    _Musteri._DovizHesap = DH;
                    return true;
                }
            }
            return false;
        }

        private void hesap_hareketleri_listele_button_Click(object sender, EventArgs e)
        {
            _db.Open();
            string sql = "SELECT hh_tarih as Tarih,hh_mus_id as MusNo,hh_hesap_no as HesapNo,hh_ad as Ad,hh_soyad as Soyad,hh_metin as Aciklama  FROM hesap_hareketleri Where hh_tarih BETWEEN @date1 and @date2 and hh_mus_id=@no";
            OleDbDataAdapter adtr = new OleDbDataAdapter(sql, _db);
            adtr.SelectCommand.Parameters.AddWithValue("@date1", dateTimePicker1.Value.ToString("dd/MM/yyyy"));
            adtr.SelectCommand.Parameters.AddWithValue("@date2", dateTimePicker2.Value.ToString("dd/MM/yyyy"));
            adtr.SelectCommand.Parameters.AddWithValue("@no", _Musteri.no);
            DataTable _table = new DataTable();
            adtr.Fill(_table);
            _db.Close();
            HesapHareketleriDataGridView.DataSource = _table;
            for (int i = 0; i < HesapHareketleriDataGridView.Columns.Count; i++)
            {
                if (i == 5) HesapHareketleriDataGridView.Columns[i].Width = 500;
                else HesapHareketleriDataGridView.Columns[i].Width = 85;
            }
            
        }

        private void KrediHesaplabutton_Click(object sender, EventArgs e)
        {

        }

        private void RaporListelebutton_Click(object sender, EventArgs e)
        {
            string query = "select * from krediHesapla where mus_id=" + _Musteri.no + "";
            OleDbCommand command = new OleDbCommand(query, _db);
            _db.Open();
            OleDbDataReader reader = command.ExecuteReader();
            Double HesapMiktar = 0;
            if(reader.Read() == false)
            {
                HesapMiktar = TlDonustur();
                Random rnd = new Random();
                int random_borc = rnd.Next(2000, 10000);
                int random_maas = rnd.Next(2000, 10000);
                OleDbCommand InsertCommand = new OleDbCommand("insert into krediHesapla (mus_id,mus_borc,mus_kasa_miktar,mus_maas) values('" + _Musteri.no + "','" + random_borc + "','" + HesapMiktar + "','" + random_maas + "')", _db);
                if (InsertCommand.ExecuteNonQuery() == 0) MessageBox.Show("Beklenmedik bir hata oluştu");

            }
            reader.Close();
            HesapMiktar = TlDonustur();
            OleDbCommand command2 = new OleDbCommand("update krediHesapla set mus_kasa_miktar ='" + HesapMiktar + "' where mus_id = " + _Musteri.no + "", _db);
            command2.ExecuteNonQuery();
            OleDbDataAdapter adtr = new OleDbDataAdapter("select mus_id as MusNo,mus_borc as Borc,mus_kasa_miktar as HesapMiktar,mus_maas as Maas from krediHesapla where mus_id=" + _Musteri.no+"", _db);
            DataTable _table = new DataTable();
            adtr.Fill(_table);
            KrediHesabıListeledataGridView.DataSource = _table;
            _db.Close(); 
        }

        private double TlDonustur()
        {
            double BankaHesapMiktar = 0;
            foreach (BankaHesap BH in _Musteri._BankaHesaplar)
            {
                BankaHesapMiktar += BH.HesapMiktar;
            }
            API_Obj Test = Rates.Import("USD");
            BankaHesapMiktar += _Musteri._DovizHesap.Dollar * Test.conversion_rates.TRY;
            API_Obj Test2 = Rates.Import("EUR");
            BankaHesapMiktar += _Musteri._DovizHesap.Euro * Test2.conversion_rates.TRY;
            API_Obj Test3 = Rates.Import("GBP");
            BankaHesapMiktar += _Musteri._DovizHesap.Sterlin * Test3.conversion_rates.TRY;
            BankaHesapMiktar += _Musteri._DovizHesap.TL;
            return BankaHesapMiktar;
        }

        private void musteri_listele_onay_button_Click(object sender, EventArgs e)
        {
            groupBox10.Visible = true;
            string query = "select * from musteriler where mus_id=" + _Musteri.no + "";
            _db.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter(query, _db);
            DataTable _table = new DataTable();
            adtr.Fill(_table);
            musteri_bilgileri_ad_textBox.Text = _table.Rows[0]["mus_ad"].ToString();
            musteri_bilgileri_soyad_textBox.Text = _table.Rows[0]["mus_soyad"].ToString();
            musteri_bilgileri_adres_textBox.Text = _table.Rows[0]["mus_adres"].ToString();
            textBox2musteri_bilgileri_tel_textBox.Text = _table.Rows[0]["mus_tel"].ToString();
            musteri_bilgileri_mus_no_textBox.Text = _Musteri.no.ToString();
            musteri_bilgileri_tc_textbox.Text = _Musteri.tc.ToString();
            _db.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (musteri_bilgileri_sifre_textbox.PasswordChar == '\0')
            {
                button4.BringToFront();
                musteri_bilgileri_sifre_textbox.PasswordChar = '*';
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (musteri_bilgileri_sifre_textbox.PasswordChar == '*')
            {
                button1.BringToFront();
                musteri_bilgileri_sifre_textbox.PasswordChar = '\0';
            }
        }

        private void musteri_bilgi_guncelle_button_Click(object sender, EventArgs e)
        {
            _db.Open();
            if (musteri_bilgileri_sifre_textbox.Text == _Musteri.sifre)
            {
                OleDbCommand command = new OleDbCommand("update musteriler set mus_tel ='" + textBox2musteri_bilgileri_tel_textBox.Text + "', mus_adres='" + musteri_bilgileri_adres_textBox.Text + "' where mus_id = " + _Musteri.no + "", _db);
                if (command.ExecuteNonQuery() != 0) MessageBox.Show("Güncelleme işleminiz başarılı");
                else MessageBox.Show("Güncelleme Başarısız!");
            }
            else MessageBox.Show("Şifrenizi yanlış girdiniz!");
            _db.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            groupBox11.Visible = true;
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (MusSifreDegistirEskiSifretextBox.PasswordChar == '*')
            {
                button5.BringToFront();
                MusSifreDegistirEskiSifretextBox.PasswordChar = '\0';
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (MusSifreDegistirEskiSifretextBox.PasswordChar == '\0')
            {
                button6.BringToFront();
                MusSifreDegistirEskiSifretextBox.PasswordChar = '*';
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (MusSifreDegistirYeniSifretextBox.PasswordChar == '*')
            {
                button12.BringToFront();
                MusSifreDegistirYeniSifretextBox.PasswordChar = '\0';
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (MusSifreDegistirYeniSifretextBox.PasswordChar == '\0')
            {
                button11.BringToFront();
                MusSifreDegistirYeniSifretextBox.PasswordChar = '*';
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _db.Open();
            if (_Musteri.sifre == MusSifreDegistirEskiSifretextBox.Text)
            {
                OleDbCommand command = new OleDbCommand("update musteriler set mus_sifre ='" + MusSifreDegistirYeniSifretextBox.Text + "' where mus_id = " + _Musteri.no + "", _db);
                if (command.ExecuteNonQuery() != 0) MessageBox.Show("Şifre Güncelleme Işlemi Başarılı");
                else MessageBox.Show("Şifre Güncelleme Işlemi Başarısız!");
            }
            else MessageBox.Show("Eski Şifrenizi Doğru Giriniz!");
            _db.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
 
}
