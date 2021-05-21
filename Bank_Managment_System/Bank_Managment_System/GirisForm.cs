using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;


namespace Bank_Managment_System
{
    
    public partial class GirisForm : Form
    {
        
        public GirisForm()
        {
            InitializeComponent();
        }

        private void musteri_giris_button_Click(object sender, EventArgs e)
        {
            OleDbConnection db = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Bank_DataBase.mdb");
            db.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter("select * from musteriler", db);

            DataTable mus_table = new DataTable();
            adtr.Fill(mus_table);
            db.Close();
            int KayitNo = MusLoginKontrol(mus_table, musteri_no_textbox.Text, musteri_sifre_textbox.Text);
            if (KayitNo == -1) MessageBox.Show("MüşteriNo veya Şifre Hatalı");
            else
            {
                Form1 form1 = Application.OpenForms["Form1"] as Form1; // form1 e gitmek için
                Panel panel1 = form1.Controls["panel1"] as Panel;
                panel1.Controls.Clear();

                MusteriForm musteri_form = new MusteriForm(mus_table, KayitNo, db);
                musteri_form.TopLevel = false;
                panel1.Controls.Add(musteri_form);
                musteri_form.Show();
            }
        }

        private int MusLoginKontrol(DataTable mus_table, string no, string sifre)
        {
            int i = 0;
            for (; i < mus_table.Rows.Count; i++)
            {
                if (mus_table.Rows[i]["mus_id"].ToString() == no && mus_table.Rows[i]["mus_sifre"].ToString() == sifre) break;
            }
            if (i == mus_table.Rows.Count) return -1;
            else return i;
        }

        private void yonetici_giris_button_Click(object sender, EventArgs e)
        {
            OleDbConnection db = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Bank_DataBase.mdb");
            db.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter("select * from yoneticiler", db);
            DataTable yon_table = new DataTable();
            adtr.Fill(yon_table);
            db.Close();
            int KayitNo = YonLoginKontrol(yon_table, yonetici_no_textBox.Text, yonetici_sifre_textBox.Text);
            if (KayitNo == -1) MessageBox.Show("YöneticiNo veya Şifre Hatalı");
            else
            {
                Form1 form1 = Application.OpenForms["Form1"] as Form1; // form1 e gitmek için
                Panel panel1 = form1.Controls["panel1"] as Panel;
                panel1.Controls.Clear();

                YoneticiForm yonetici_form = new YoneticiForm(yon_table, KayitNo, db);
                yonetici_form.TopLevel = false;
                panel1.Controls.Add(yonetici_form);
                yonetici_form.Show();

            }
        }

        private int YonLoginKontrol(DataTable yon_table, string no, string sifre)
        {
            int i = 0;
            for (; i < yon_table.Rows.Count; i++)
            {
                if (yon_table.Rows[i]["yon_id"].ToString() == no && yon_table.Rows[i]["yon_sifre"].ToString() == sifre) break;
            }
            if (i == yon_table.Rows.Count) return -1;
            else return i;
        }

        private void MusteriOlButton_Click(object sender, EventArgs e)
        {
            OleDbConnection db = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Bank_DataBase.mdb");
            string query = "select * from musteriler where mus_tc='" + MusteriOlTCtextBox.Text +"'";
            db.Open();
            OleDbCommand command = new OleDbCommand(query, db);
            OleDbDataReader reader = command.ExecuteReader();
            Boolean HesapKontrol = reader.Read();
            reader.Close();
            if (HesapKontrol) MessageBox.Show("Bankamıza Kayıtlısınız.");
            else
            {
                OleDbCommand InsertCommand = new OleDbCommand("insert into musteriler(mus_ad,mus_soyad,mus_tc,mus_adres,mus_tel,mus_sifre) values('" + MusteriOlAdtextBox.Text + "','" + MusteriolSoyadTextBox.Text + "','" + MusteriOlTCtextBox.Text + "','" +MusteriOladrestextBox.Text + "','"+MusteriOlteltextBox.Text + "','" + MusteriOlSifretextBox.Text + "') ", db);
                if (InsertCommand.ExecuteNonQuery() == 0) MessageBox.Show("Kayıt gerçekleştirilemedi.");
                else MessageBox.Show("Bankamıza Kayıt Oldunuz.");
            }
            db.Close();
        }
    }
}
