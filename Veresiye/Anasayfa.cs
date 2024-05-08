using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.SqlClient;

namespace Veresiye
{
    public partial class Anasayfa : Form
    {
        public Anasayfa()
        {
            InitializeComponent(); 
        }
        SQLiteConnection con = new SQLiteConnection("Data Source=" + Application.StartupPath + "\\" + "veresiye.db");

        private void Anasayfa_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;

            Arama();
            AnasayfaDoldur();

        }

        private void Arama()
        {
            switch(comboBox1.SelectedIndex)
            {
                case 0: 
                    label4.Text = "Unvan =";
                    textBox1.Text = "Unvan ile Arama Yapınız";
                    textBox1.Font = new Font(textBox1.Font, FontStyle.Italic);
                    break;

                case 1:
                    label4.Text = "Cari Kod =";
                    textBox1.Text = "Cari Kod ile Arama Yapınız";
                    textBox1.Font = new Font(textBox1.Font, FontStyle.Italic);
                    break;

                case 2:
                    label4.Text = "İl =";
                    textBox1.Text = "İl ile Arama Yapınız";
                    textBox1.Font = new Font(textBox1.Font, FontStyle.Italic);
                    break;

                case 3:
                    label4.Text = "İlçe =";
                    textBox1.Text = "İlçe İle Arama Yapınız";
                    textBox1.Font = new Font(textBox1.Font, FontStyle.Italic);
                    break;
            }
        }

        private void AnasayfaDoldur()
        {
            SQLiteDataAdapter da = new SQLiteDataAdapter("select ID as 'CARİ KOD',TARİH as 'SON İŞLEM TARİH',UNVAN as 'ÜNVAN',İL as 'İL', BAKİYE as 'BAKİYE' from MÜSTERİ", con);
            DataSet ds = new DataSet(); 
            if (!(con.State == ConnectionState.Open))
                con.Open();

            da.Fill(ds, "MÜSTERİ");
            dataGridView1.DataSource = ds.Tables["MÜSTERİ"]; //verileri dolduruyorum
            if (ds.Tables["MÜSTERİ"].Rows.Count > 0)
            {
                string sql = "SELECT ID FROM MÜSTERİ ORDER BY ROWID DESC LIMIT 1";

                using (SQLiteCommand command = new SQLiteCommand(sql, con))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                           global.YeniCariKod = Convert.ToInt32(reader["ID"]);
                        }
                    }
                }
            }

            else
            {
                Console.WriteLine("Tablo boş.");
            }
            con.Close();
            global.toplamCari = ds.Tables["MÜSTERİ"].Rows.Count; //satır sayısı uzunluğu
            label5.Text = "Toplam Kayıt Sayısı: " + global.toplamCari.ToString();
          
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show(dataGridView1.CurrentRow.Cells[2].Value.ToString() + " İsimli Kişiyi Silmek İstediğinize Emin Misiniz?", "Cari İşlemleri", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                con.Open();
                // DELETE FROM kullanarak veritabanından bir satırı silme

                SQLiteCommand cmd = new SQLiteCommand("DELETE FROM MÜSTERİ WHERE ID = @p1", con);
                cmd.Parameters.AddWithValue("@p1", global.CariKod);
                try
                {
                    if (!(con.State == ConnectionState.Open))
                        con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Silme İşlemi Başarılı", "Cari İşlemleri", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                finally { con.Close(); }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MüsteriDetay m = new MüsteriDetay();
            m.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MüsteriEkle m = new MüsteriEkle();
            m.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MüsteriDetay m = new MüsteriDetay();
            m.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Arama();
            AnasayfaDoldur();
        }

        
        private void textBox1_Click_1(object sender, EventArgs e)
        {
            textBox1.Clear();
            Font myfont = new Font("Arial", 10);
            textBox1.Font = myfont;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ara(textBox1.Text);
        }

        private void ara(object text1)
        {
            try
            {
                string sorgu = comboBox1.SelectedItem.ToString();
                //string filtre = comboBox3.SelectedItem.ToString();
                //string hesap = comboBox2.SelectedItem.ToString();
                string searchText = textBox1.Text;

                string query = "SELECT ID as 'CARİ KOD',TARİH as 'TARİH', UNVAN as 'ÜNVAN',İL as 'İL', BAKİYE as 'BAKİYE' FROM MÜSTERİ WHERE 1=1";

                if (sorgu == "Unvan ile Sorgu")
                {
                    query += $" AND UNVAN like '"+ searchText +"%'";
                }
                else if (sorgu == "Cari Kod ile Sorgu")
                {
                    query += $" AND ID like '" + searchText + "%'";
                }
                else if (sorgu == "İl ile Sorgu")
                {
                    query += $" AND İL like '" + searchText + "%'";
                }
                else if (sorgu == "İlçe ile sorgu")
                {
                    query = "SELECT ID as 'CARİ KOD',TARİH as 'TARİH', UNVAN as 'ÜNVAN',İL as 'İL',İLCE as 'İLÇE', BAKİYE as 'BAKİYE' FROM MÜSTERİ WHERE 1=1";

                    query += $" AND İLCE like '" + searchText + "%'";
                }

                string connectionString = "Data Source=" + Application.StartupPath + "\\" + "veresiye.db"; // SQLite veritabanı dosyasının yolunu belirtin
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // DataGridView'e verileri yükle
                            dataGridView1.DataSource = dataTable;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Anasayfa_Activated(object sender, EventArgs e)
        {
            AnasayfaDoldur();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (global.CariKod == 0)
            {
                MessageBox.Show("Lütfen Seçim Yapınız", "Cari İşlemleri", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MüsteriGüncelle m = new MüsteriGüncelle();
                m.ShowDialog();
            } 
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                global.CariKod = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value); //tıkladığımız carinin kodunu ATIYORUZ.    
            }
            catch (Exception)
            {
                MessageBox.Show("Lütfen Geçerli Bir Kayıt Seçiniz!", "Cari İşlemleri", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
