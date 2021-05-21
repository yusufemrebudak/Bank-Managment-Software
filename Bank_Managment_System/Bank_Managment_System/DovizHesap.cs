using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace Bank_Managment_System
{
    class DovizHesap:Hesap
    {
        public double TL;
        public double Dollar;
        public double Euro;
        public double Sterlin;

        public DovizHesap(DataTable _table, int KayitNo, OleDbConnection _db)
        {
                this.HesapNo = Convert.ToInt32(_table.Rows[KayitNo]["hesap_no"].ToString());
                this.TL = Convert.ToDouble(_table.Rows[KayitNo]["tl_miktar"].ToString());
                this.Dollar = Convert.ToDouble(_table.Rows[KayitNo]["dolar_miktar"].ToString());
                this.Euro = Convert.ToDouble(_table.Rows[KayitNo]["euro_miktar"].ToString());
                this.Sterlin = Convert.ToDouble(_table.Rows[KayitNo]["sterlin_miktar"].ToString());
        }

    }
}
