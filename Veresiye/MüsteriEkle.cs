using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SQLite;
using System.Collections;

namespace Veresiye
{
    public partial class MüsteriEkle : Form
    {
        public MüsteriEkle()
        {
            InitializeComponent();
        }
        SQLiteConnection con = new SQLiteConnection("Data Source=" + Application.StartupPath + "\\" + "veresiye.db"); //veritabanı bağlantısını kuruyoruz
       
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void MüsteriEkle_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.SelectedIndex = 0; //aktif yapıyoruz
            global.YeniCariKod++; //Müşteri cari kodunu arttır
            textBox1.Text = global.YeniCariKod.ToString(); 

        }
        private void button2_Click(object sender, EventArgs e)
        {
            kaydet();  
            MüsteriEkle_Load(sender, e);
        }

        public void kaydet()
        {
           
            int hesapDurumu = 0; //aktif yaptık 
            
            if (comboBox1.SelectedIndex == 1)
            {
                hesapDurumu = 1; //pasif yaptık
            }

            if (string.IsNullOrEmpty(textBox2.Text)) //unvan boş olamaz
            {
                MessageBox.Show("Lütfen Gerekli Unvan Alanını Doldurunuz", "Öğretmen İşlemleri", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               // MessageBox.Show(textBox2.Text);//
                tabPage1.Show();
                textBox2.Focus();
            }
            else
            {

                dateTimePicker1.Format = DateTimePickerFormat.Short; //tarih formatını değiştir
                SQLiteCommand cmd1 = new SQLiteCommand("insert into MÜSTERİVERESİYE(ID) values (@p1)", con);
                cmd1.Parameters.AddWithValue("@p1", textBox1.Text);
                SQLiteCommand cmd = new SQLiteCommand("insert into MÜSTERİ   (ID,TARİH,UNVAN,TELEFON,ADRES,İL,İLCE,LİMİT,HESAP,MüsteriNot,BAKİYE) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11)" , con);
                cmd.Parameters.AddWithValue("@p1", textBox1.Text);
                cmd.Parameters.AddWithValue("@p2", dateTimePicker1.Text);//tarih
                cmd.Parameters.AddWithValue("@p3", textBox2.Text);
                cmd.Parameters.AddWithValue("@p4", maskedTextBox1.Text); //tel
                cmd.Parameters.AddWithValue("@p5", textBox4.Text);
                cmd.Parameters.AddWithValue("@p6", textBox5.Text);
                cmd.Parameters.AddWithValue("@p7", textBox6.Text);
                cmd.Parameters.AddWithValue("@p8", LimitDüzenle(textBox7)); //düzenlenen limit değerini veritabanına atıyoruz
                cmd.Parameters.AddWithValue("@p9", hesapDurumu);
                cmd.Parameters.AddWithValue("@p10", textBox3.Text);
                cmd.Parameters.AddWithValue("@p11", "0,00 TL"); //BAKİYE
                dateTimePicker1.Format = DateTimePickerFormat.Long; //tarih formatını eski uzun haline alıyoruz

                try
                {
                    if (!(con.State == ConnectionState.Open))
                        con.Open();
                    cmd.ExecuteNonQuery(); //sadece verileri gönderiyoruz
                    cmd1.ExecuteNonQuery(); //sadece verileri gönderiyoruz
                    MessageBox.Show("Kayıt İşlemi Başarılı", "Cari İşlemleri", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //yeni kayıt için temizliyoruz
                    textBox2.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                    maskedTextBox1.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                finally { con.Close(); }
            }
        }

        public string LimitDüzenle(System.Windows.Forms.TextBox textbox) //Limit Değerini düzenliyoruz (Doğru)
        {
            string sayi = "";
            foreach(char c in textbox.Text)
            {
                if((c.ToString() != ".") && (c.ToString() != " " ) && (c.ToString() != "T") && (c.ToString() != "L"))
                {
                    if(c.ToString() == "," )
                    {
                        sayi += "."; //FLOAT SAYİ OLMASI İÇİN
                    }
                    else
                    {
                        sayi += c.ToString();
                    }
                } 
            }
            return sayi;
        }

        private void maskedTextBox1_Click(object sender, EventArgs e)
        { //DEĞİŞTİR 
            maskedTextBox1.Text = "0";
            maskedTextBox1.SelectionStart = 1; // İmleci başa al
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); //kapat
        }

        //burdan
        bool ilktiklama = true;
        private void textBox7_Click(object sender, EventArgs e)
        {
            if (ilktiklama)
            {
                textBox7.SelectAll();
                textBox7.SelectionStart = textBox7.TextLength;
            }
            textBox7.Text = txtboxduzenle(textBox7);

            ilktiklama = false;
        }

        private string txtboxduzenle(System.Windows.Forms.TextBox text)
        {
            string metin = "";
            foreach (char s in text.Text)
            {
                if (s == ',')
                {
                    return metin;
                }
                metin += s;
            }
            return metin;
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            if (textBox7.Text == "")
            {
                textBox7.Text = textBox7.Text + "0,00 TL";
            }
            else
            {
                textBox7.Text = textBox7.Text + ",00 TL";
            }
        }
        //buraya

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Text = ""; //not kısmı silme
        }

        private void button3_Click(object sender, EventArgs e)
        {
            kaydet();
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Sadece rakamlar ve silme izin veriyoruz
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Bu karakteri reddediyoruz
            }
        }
    }
}
