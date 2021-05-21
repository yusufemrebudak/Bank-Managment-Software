using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_Managment_System
{
    class BankaHesap:Hesap
    {
        public double HesapMiktar;
        public BankaHesap(DataTable _table, int KayitNo, OleDbConnection _db)
        {
            this.HesapNo = Convert.ToInt32(_table.Rows[KayitNo]["hesap_no"].ToString());
            this.HesapMiktar = Convert.ToDouble(_table.Rows[KayitNo]["bh_miktar"].ToString());

        }
    }
}
