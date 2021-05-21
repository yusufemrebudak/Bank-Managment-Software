using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank_Managment_System
{
    class Yonetici : Kisi, IYoneticiOzellikleri
    {
        public static OleDbConnection _db;
        public Yonetici(DataTable yon_table, int KayitNo, OleDbConnection db)
        {
            _db = db;
            this.no = Convert.ToInt32(yon_table.Rows[KayitNo]["yon_id"].ToString());
            this.ad = yon_table.Rows[KayitNo]["yon_ad"].ToString();
            this.soyad = yon_table.Rows[KayitNo]["yon_soyad"].ToString();
            this.tc = yon_table.Rows[KayitNo]["yon_tc"].ToString();
            this.adres = yon_table.Rows[KayitNo]["yon_adres"].ToString();
            this.tel = yon_table.Rows[KayitNo]["yon_tel"].ToString();
            this.sifre = yon_table.Rows[KayitNo]["yon_sifre"].ToString();
        }
        public void HesapAc(int MusNo,string HesapTip)
        {
            int HesapNo = 0;
            while (true)
            {
                Random rndm = new Random();
                HesapNo = rndm.Next(100, 200);
                string query = "select * from hesaplar where hesap_no=" + HesapNo + "";
                OleDbCommand command = new OleDbCommand(query, _db);
                OleDbDataReader reader = command.ExecuteReader();
                Boolean hedefHesapKontrol = reader.Read();
                if (hedefHesapKontrol == false)
                {
                    reader.Close();
                    break;
                }
            }
            OleDbCommand Command2 = new OleDbCommand("insert into hesaplar(mus_id,hesap_no,hesap_tur) values('" + MusNo + "','" + HesapNo + "','" + HesapTip + "')", _db);
            if (HesapTip == "Banka")
            {
                OleDbCommand Command3 = new OleDbCommand("insert into banka_hesaplar(hesap_no,bh_miktar) values('" + HesapNo + "','" + 0 + "')", _db);
                if (Command2.ExecuteNonQuery() == 0 || Command3.ExecuteNonQuery() == 0) MessageBox.Show("Yeni Hesap Açılamadı.Bir sorun oluştu!");
                else MessageBox.Show(MusNo + " Musteri Nolu müşteri için :  " + HesapNo + " Numaralı yeni " + HesapTip + " Hesabı açılmıştır.");

            }
            else if(HesapTip == "Doviz")
            {
                OleDbCommand Command3 = new OleDbCommand("insert into doviz_hesaplar(hesap_no,tl_miktar,dolar_miktar,euro_miktar,sterlin_miktar) values('" + HesapNo + "','"+0+"','"+0+"','"+0+"','"+0+"')", _db);
                if (Command2.ExecuteNonQuery() == 0 || Command3.ExecuteNonQuery() == 0) MessageBox.Show("Yeni Hesap Açılamadı.Bir sorun oluştu!");
                else MessageBox.Show(MusNo + " Musteri Nolu müşteri için :  " + HesapNo + " Numaralı yeni " + HesapTip + " Hesabı açılmıştır.");
            }
        }

        public void HesapSil()
        {
            throw new NotImplementedException();
        }

        public void KrediBasvuruOnayla()
        {
            throw new NotImplementedException();
        }

        public void KrediBasvuruReddet()
        {
            throw new NotImplementedException();
        }

        public void KrediBilgileriGuncelle()
        {
            throw new NotImplementedException();
        }

        public void MusteriGuncelle()
        {
            throw new NotImplementedException();
        }

        public void MusteriListele()
        {
            throw new NotImplementedException();
        }

        public void MusteriSil()
        {
            throw new NotImplementedException();
        }
    }
}
