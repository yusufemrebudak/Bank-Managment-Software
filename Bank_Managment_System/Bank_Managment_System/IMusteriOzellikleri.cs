using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Managment_System
{
    interface IMusteriOzellikleri
    {
        void KrediHesapla(double miktar);
        void KrediBasvur();
        void HesapParaGonder(int HedefHesapNo, double BankaTutar, double miktar);
        void DovizAl(string DonusturulecekParaBirimi, double Tutar, double DonusumTutar);
        void DovizTra(string DonusenParaBirimi,string DonusturulecekParaBirimi ,string DonusturulecekParaMiktar, string DonusumTutari);
        void HesaplarimListele();
        
    }
}
