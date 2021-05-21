using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;

namespace Bank_Managment_System
{
  

    class Musteri : Kisi, IMusteriOzellikleri
    {
        public List<DovizHesap> _DovizHesaplar = new List<DovizHesap>();
        public List<BankaHesap> _BankaHesaplar = new List<BankaHesap>();
        public DovizHesap _DovizHesap;
        public BankaHesap _BankaHesap;
        //public KrediHesap _KrediHesap;

        public static OleDbConnection _db;

        public Musteri(int KayitNo, DataTable mus_table, OleDbConnection db)
        {
            _db = db;
            this.no = Convert.ToInt32(mus_table.Rows[KayitNo]["mus_id"].ToString());
            this.ad = mus_table.Rows[KayitNo]["mus_ad"].ToString();
            this.soyad = mus_table.Rows[KayitNo]["mus_soyad"].ToString();
            this.tc = mus_table.Rows[KayitNo]["mus_tc"].ToString();
            this.adres = mus_table.Rows[KayitNo]["mus_adres"].ToString();
            this.tel = mus_table.Rows[KayitNo]["mus_tel"].ToString();
            this.sifre = mus_table.Rows[KayitNo]["mus_sifre"].ToString();
        }

        public void DovizAl(string DonusturulecekParaBirimi, double Tutar, double DonusumTutar)
        {
            _BankaHesap.HesapMiktar -= Tutar;
            OleDbCommand Command = new OleDbCommand("update banka_hesaplar set bh_miktar ='" + _BankaHesap.HesapMiktar + "' where hesap_no = " + _BankaHesap.HesapNo + "", _db);
            _db.Open();
            Command.ExecuteNonQuery();
            double BankaTutar = DonusumTutar * 0.6;
            switch (DonusturulecekParaBirimi)
            {
                case "USD":
                    _DovizHesap.Dollar += DonusumTutar;
                    
                    OleDbCommand UsdKomut = new OleDbCommand("update doviz_hesaplar set dolar_miktar ='" + _DovizHesap.Dollar + "' where hesap_no = " + _DovizHesap.HesapNo + "", _db);
                    if (UsdKomut.ExecuteNonQuery() != 0)
                    {
                        MessageBox.Show("Döviz Dönüşüm İşlemi Başarılı/VeriTabanı Güncellendi");
                        string metin = _BankaHesap.HesapNo + " Numaralı Banka Hesabınız aracılığıyla  " + Tutar + " TRY değerinde " + DonusturulecekParaBirimi + " aldınız." + _DovizHesap.HesapNo + " Nolu Döviz Hesabınıza yatan " + DonusturulecekParaBirimi + " Tutarı : " + DonusumTutar;
                        string metin2 = _BankaHesap.HesapNo + " Numaralı Banka Hesabından DÖVİZ ALIMI gerçekleşmiştir.";
                        HesapHareketEkle(_BankaHesap.HesapNo, metin, metin2, BankaTutar);
                    }
                    break;
                case "TRY":
                    _DovizHesap.TL += DonusumTutar;
                    OleDbCommand TryKomut = new OleDbCommand("update doviz_hesaplar set tl_miktar ='" + _DovizHesap.TL + "' where hesap_no = " + _DovizHesap.HesapNo + "", _db);
                    if (TryKomut.ExecuteNonQuery() != 0)
                    {
                        MessageBox.Show("Döviz Dönüşüm İşlemi Başarılı/VeriTabanı Güncellendi");
                        string metin = _BankaHesap.HesapNo + " Numaralı Banka Hesabınız aracılığıyla  " + Tutar + " TRY değerinde " + DonusturulecekParaBirimi + " aldınız." + _DovizHesap.HesapNo + " Nolu Döviz Hesabınıza yatan " + DonusturulecekParaBirimi + " Tutarı : " + DonusumTutar;
                        string metin2 = _BankaHesap.HesapNo + " Numaralı Banka Hesabından DÖVİZ ALIMI gerçekleşmiştir.";
                        HesapHareketEkle(_BankaHesap.HesapNo, metin, metin2, BankaTutar);
                    }
                    break;
                case "EUR":
                    _DovizHesap.Euro += DonusumTutar;
                    OleDbCommand EuroKomut = new OleDbCommand("update doviz_hesaplar set euro_miktar ='" + _DovizHesap.Euro + "' where hesap_no = " + _DovizHesap.HesapNo + "", _db);
                    if (EuroKomut.ExecuteNonQuery() != 0)
                    {
                        MessageBox.Show("Döviz Dönüşüm İşlemi Başarılı/VeriTabanı Güncellendi");
                        string metin = _BankaHesap.HesapNo + " Numaralı Banka Hesabınız aracılığıyla  " + Tutar + " TRY değerinde " + DonusturulecekParaBirimi + " aldınız." + _DovizHesap.HesapNo + " Nolu Döviz Hesabınıza yatan " + DonusturulecekParaBirimi + " Tutarı : " + DonusumTutar;
                        string metin2 = _BankaHesap.HesapNo + " Numaralı Banka Hesabından DÖVİZ ALIMI gerçekleşmiştir.";
                        HesapHareketEkle(_BankaHesap.HesapNo, metin, metin2,BankaTutar);
                    }
                    break;
                case "GBP":
                    _DovizHesap.Sterlin += DonusumTutar;
                    OleDbCommand StrKomut = new OleDbCommand("update doviz_hesaplar set sterlin_miktar ='" + _DovizHesap.Sterlin + "' where hesap_no = " + _DovizHesap.HesapNo + "", _db);
                    if (StrKomut.ExecuteNonQuery() != 0)
                    {
                        MessageBox.Show("Döviz Dönüşüm İşlemi Başarılı/VeriTabanı Güncellendi");
                        string metin = _BankaHesap.HesapNo + " Numaralı Banka Hesabınız aracılığıyla  " + Tutar + " TRY değerinde " + DonusturulecekParaBirimi + " aldınız." + _DovizHesap.HesapNo + " Nolu Döviz Hesabınıza yatan " + DonusturulecekParaBirimi + " Tutarı : " + DonusumTutar;
                        string metin2 = _BankaHesap.HesapNo + " Numaralı Banka Hesabından DÖVİZ ALIMI gerçekleşmiştir.";

                        HesapHareketEkle(_BankaHesap.HesapNo, metin, metin2, BankaTutar);
                    }
                    break;
                default:
                    break;
            }
            _db.Close();
        }

        public void DovizTra(string DonusenParaBirimi, string DonusturulecekParaBirimi, string DonusturulecekParaMiktar, string DonusumTutari)
        {
            double DonusturulenMiktar = Convert.ToDouble(DonusturulecekParaMiktar);
            double DonusenMiktar = Convert.ToDouble(DonusumTutari);
            _db.Open();
            switch (DonusenParaBirimi)
            {
                case "USD":
                    _DovizHesap.Dollar -= DonusturulenMiktar;
                    OleDbCommand UsdKomut = new OleDbCommand("update doviz_hesaplar set dolar_miktar ='" + _DovizHesap.Dollar + "' where hesap_no = " + _DovizHesap.HesapNo + "", _db);
                    if (UsdKomut.ExecuteNonQuery() == 0) MessageBox.Show("Veritabanı Güncellenemedi.İşlem Başarısız");
                    break;
                case "TRY":
                    _DovizHesap.TL -= DonusturulenMiktar;
                    OleDbCommand TryKomut = new OleDbCommand("update doviz_hesaplar set tl_miktar ='" + _DovizHesap.TL + "' where hesap_no = " + _DovizHesap.HesapNo + "", _db);
                    if (TryKomut.ExecuteNonQuery() == 0) MessageBox.Show("Veritabanı Güncellenemedi.İşlem Başarısız");
                    break;
                case "EUR":
                    _DovizHesap.Euro -= DonusturulenMiktar;
                    OleDbCommand EuroKomut = new OleDbCommand("update doviz_hesaplar set euro_miktar ='" + _DovizHesap.Euro + "' where hesap_no = " + _DovizHesap.HesapNo + "", _db);
                    if (EuroKomut.ExecuteNonQuery() == 0) MessageBox.Show("Veritabanı Güncellenemedi.İşlem Başarısız");
                    break;
                case "GBP":
                    _DovizHesap.Sterlin -= DonusturulenMiktar;
                    OleDbCommand StrKomut = new OleDbCommand("update doviz_hesaplar set sterlin_miktar ='" + _DovizHesap.Sterlin + "' where hesap_no = " + _DovizHesap.HesapNo + "", _db);
                    if (StrKomut.ExecuteNonQuery() == 0) MessageBox.Show("Veritabanı Güncellenemedi.İşlem Başarısız");
                    break;
                default:
                    break;
            }
            _db.Close();
            _db.Open();
            switch (DonusturulecekParaBirimi)
            {
                case "USD":
                    _DovizHesap.Dollar += DonusenMiktar;
                    OleDbCommand UsdKomut = new OleDbCommand("update doviz_hesaplar set dolar_miktar ='" + _DovizHesap.Dollar + "' where hesap_no = " + _DovizHesap.HesapNo + "", _db);
                    if (UsdKomut.ExecuteNonQuery() != 0)
                    {
                        MessageBox.Show("Döviz Dönüşüm İşlemi Başarılı/VeriTabanı Güncellendi");
                        string metin = _DovizHesap.HesapNo + " Numaralı Döviz Hesabınız aracılığıyla " + DonusturulecekParaMiktar + " " + DonusenParaBirimi + " tutarından ==>" + DonusumTutari + " " + DonusturulecekParaBirimi + " DÖVİZ DÖNÜŞTÜRME işlemi gerçekleştirildi";
                        string metin2 = _DovizHesap.HesapNo + " Numaralı Döviz Hesabında DÖVİZ DÖNÜŞTÜRME işlemi gerçekleşmiştir.";
                        HesapHareketEkle(_DovizHesap.HesapNo, metin, metin2, 0);
                    }
                    break;
                case "TRY":
                    _DovizHesap.TL += DonusenMiktar;
                    OleDbCommand TryKomut = new OleDbCommand("update doviz_hesaplar set tl_miktar ='" + _DovizHesap.TL + "' where hesap_no = " + _DovizHesap.HesapNo + "", _db);
                    if (TryKomut.ExecuteNonQuery() != 0)
                    {
                        MessageBox.Show("Döviz Dönüşüm İşlemi Başarılı/VeriTabanı Güncellendi");
                        string metin = _DovizHesap.HesapNo + " Numaralı Döviz Hesabınız aracılığıyla " + DonusturulecekParaMiktar + " " + DonusenParaBirimi + " tutarından ==>" + DonusumTutari + " " + DonusturulecekParaBirimi + " DÖVİZ DÖNÜŞTÜRME işlemi gerçekleştirildi";
                        string metin2 = _DovizHesap.HesapNo + " Numaralı Döviz Hesabında DÖVİZ DÖNÜŞTÜRME işlemi gerçekleşmiştir.";
                        HesapHareketEkle(_DovizHesap.HesapNo, metin, metin2, 0);
                    }
                    break;
                case "EUR":
                    _DovizHesap.Euro += DonusenMiktar;
                    OleDbCommand EuroKomut = new OleDbCommand("update doviz_hesaplar set euro_miktar ='" + _DovizHesap.Euro + "' where hesap_no = " + _DovizHesap.HesapNo + "", _db);
                    if (EuroKomut.ExecuteNonQuery() != 0)
                    {
                        MessageBox.Show("Döviz Dönüşüm İşlemi Başarılı/VeriTabanı Güncellendi");
                        string metin = _DovizHesap.HesapNo+" Numaralı Döviz Hesabınız aracılığıyla " + DonusturulecekParaMiktar + " " + DonusenParaBirimi + " tutarından ==>" + DonusumTutari + " " + DonusturulecekParaBirimi + " DÖVİZ DÖNÜŞTÜRME işlemi gerçekleştirildi";
                        string metin2 = _DovizHesap.HesapNo + " Numaralı Döviz Hesabında DÖVİZ DÖNÜŞTÜRME işlemi gerçekleşmiştir.";
                        HesapHareketEkle(_DovizHesap.HesapNo, metin, metin2, 0);
                    }

                    break;
                case "GBP":
                    _DovizHesap.Sterlin += DonusenMiktar;
                    OleDbCommand StrKomut = new OleDbCommand("update doviz_hesaplar set sterlin_miktar ='" + _DovizHesap.Sterlin + "' where hesap_no = " + _DovizHesap.HesapNo + "", _db);
                    if (StrKomut.ExecuteNonQuery() != 0)
                    {
                        MessageBox.Show("Döviz Dönüşüm İşlemi Başarılı/VeriTabanı Güncellendi");
                        string metin = _DovizHesap.HesapNo + " Numaralı Döviz Hesabınız aracılığıyla " + DonusturulecekParaMiktar + " " + DonusenParaBirimi + " tutarından ==>" + DonusumTutari + " " + DonusturulecekParaBirimi + " DÖVİZ DÖNÜŞTÜRME işlemi gerçekleştirildi";
                        string metin2 = _DovizHesap.HesapNo + " Numaralı Döviz Hesabında DÖVİZ DÖNÜŞTÜRME işlemi gerçekleşmiştir.";
                        HesapHareketEkle(_DovizHesap.HesapNo, metin, metin2, 0);
                    }
                    break;
                default:
                    break;
            }
            _db.Close();

        }
        private void HesapHareketEkle(int hesapNo, string metin, string metin2,double BankaTutar)
        {
            OleDbCommand InsertCommand = new OleDbCommand("insert into hesap_hareketleri(hh_mus_id,hh_hesap_no,hh_ad,hh_soyad,hh_metin,hh_banka_metin,hh_banka_tutar) values('"+no+"','"+hesapNo+"','"+ad+"','"+soyad+"','" +metin+"','" +metin2+"','" + BankaTutar+"') ",_db);
            if (InsertCommand.ExecuteNonQuery() == 0) MessageBox.Show("Hareket Veritabanına eklenemedi");  
        }

        public void HesaplarimListele()
        {
            throw new NotImplementedException();
        }

        public void HesapParaGonder(int HedefHesapNo,double BankaTutar ,double miktar)
        {
            _BankaHesap.HesapMiktar -= miktar;
            _db.Open();
            OleDbCommand GonderilenHesapKomut = new OleDbCommand("update banka_hesaplar set bh_miktar = bh_miktar +'" + (miktar-BankaTutar) + "' where hesap_no = " + HedefHesapNo + "", _db);
            OleDbCommand KaynakHesapKomut = new OleDbCommand("update banka_hesaplar set bh_miktar ='" + _BankaHesap.HesapMiktar + "' where hesap_no = " + _BankaHesap.HesapNo + "", _db);
            if (GonderilenHesapKomut.ExecuteNonQuery() != 0 && KaynakHesapKomut.ExecuteNonQuery() != 0)
            { 
                MessageBox.Show("Para Transfer İşlemi Başarılı/VeriTabanı Güncellendi");
                string metin = _BankaHesap.HesapNo + " Numaralı Banka Hesabınızdan " +HedefHesapNo +" Numaralı Hesaba " + miktar +" TRY gönderildi.";
                string metin2 = _BankaHesap.HesapNo + " Numaralı Banka Hesabından PARA TRANSFER işlemi gerçekleşmiştir.";
                HesapHareketEkle(_BankaHesap.HesapNo, metin, metin2, BankaTutar);
            }
            else MessageBox.Show("Beklenmedik bir arıza oluştu/İşlem gerçekleşemedi.");
            _db.Close();
        }

        public void KrediBasvur()
        {
            throw new NotImplementedException();
        }

        public void KrediHesapla(double miktar)
        {
            throw new NotImplementedException();
        }

       
    }
}
