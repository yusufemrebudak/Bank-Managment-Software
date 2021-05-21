using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Managment_System
{
    interface IYoneticiOzellikleri
    {
        void MusteriSil();
        void MusteriGuncelle();
        void MusteriListele();
        void HesapSil();
        void HesapAc(int MusNo, string HesapTip);
        void KrediBasvuruOnayla();
        void KrediBilgileriGuncelle();
        void KrediBasvuruReddet();

    }
}
