using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Veresiye
{
    public partial class MüsteriDetay : Form
    {
        int tiklamaSayisi = 0; //tek sayılarda açık çift sayılarda kapalı form her yüklendiğinde 0 olur
        public MüsteriDetay()
        {
            InitializeComponent();
        }
        SQLiteConnection con = new SQLiteConnection("Data Source=" + Application.StartupPath + "\\" + "veresiye.db"); //veritabanı yolu

        private void MüsteriDetay_Load(object sender, EventArgs e)
        {
            MüsteriBilgileriDoldur();
        }

        private void MüsteriBilgileriDoldur()
        { 
            SQLiteCommand cmd = new SQLiteCommand("select * from MÜSTERİ where ID=@p1", con);
            cmd.Parameters.AddWithValue("@p1", global.CariKod); //Cari kodu seçili olan müşterinin ad bilgisini alıyoruz
            
            try
            {
                if (!(con.State == ConnectionState.Open))
                    con.Open();

                    string query = "select Tarih as 'SON İŞLEM TARİH',Tur as 'İŞLEM TÜRÜ',Aciklama as 'AÇIKLAMA',Alacak as 'ALACAK', Borc as 'BORÇ' FROM MÜSTERİVERESİYE WHERE ID = @ID";
                    using (SQLiteCommand cmd2 = new SQLiteCommand(query, con))
                    {
                    //seçili kişinin verilerini datatableye atıyoruz.
                        cmd2.Parameters.AddWithValue("@ID", global.CariKod);

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd2))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // DataTable'i DataGridView'e atayın
                            dataGridView1.DataSource = dataTable;
                        
                        }
                    }
                SQLiteDataReader dr = null;
                dr = cmd.ExecuteReader(); //sadece verileri alıyoruz
                while (dr.Read()) 
                {
                   button1.Text = dr["UNVAN"].ToString(); //kişinin adını butona yazdırıyoruz.  
                }
               
            }
            catch (Exception ex) 
            {
                MessageBox.Show("HATA! " + ex.ToString());
            }
            con.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //SORGULAMA BUTONU
            tiklamaSayisi++;
            if (tiklamaSayisi % 2 == 1) //tek ise açık
            {
                groupBox2.Visible = true;
                dataGridView1.Location = new Point(12, 190);
                dataGridView1.Height = 280;
                button9.Text = "Sorguyu Kapat";
            }
            else
            {   //çift ise kapalı
                groupBox2.Visible = false;
                dataGridView1.Location = new Point(12, 120);
                dataGridView1.Height = 350;
                button9.Text = "Sorgula";
            }
            //DEFAULT DEĞERLER
            comboBox1.SelectedIndex = 0;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MüsteriDetayEkle m = new MüsteriDetayEkle();
            m.ShowDialog();
        }
    }
}
