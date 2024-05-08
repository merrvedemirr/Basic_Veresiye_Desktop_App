using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Veresiye
{
    public partial class MüsteriGüncelle : Form
    {
        public MüsteriGüncelle()
        {
            InitializeComponent();
        }
        SQLiteConnection con = new SQLiteConnection("Data Source=" + Application.StartupPath + "\\" + "veresiye.db");//veritabanı bağlantısı
        MüsteriEkle m = new MüsteriEkle(); //limitduzenle kullanmak için !

        private void MüsteriGüncelle_Load(object sender, EventArgs e)
        {
            SQLiteDataReader dr = null;
            SQLiteCommand cmd = new SQLiteCommand("select * from MÜSTERİ where ID=@p1", con);
            cmd.Parameters.AddWithValue("@p1", global.CariKod); //Cari kodu seçili olan müşterinin bilgilerini alıyoruz

            try
            {
                if (!(con.State == ConnectionState.Open))
                    con.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read()) //seçilen kayıt bilgilerini ekranda göster
                {
                    textBox1.Text = dr["ID"].ToString();
                    textBox2.Text = dr["UNVAN"].ToString();
                    maskedTextBox1.Text = dr["TELEFON"].ToString();
                    textBox4.Text = dr["ADRES"].ToString();
                    textBox5.Text = dr["İL"].ToString();
                    textBox6.Text = dr["İLCE"].ToString();

                   string limit =  dr["LİMİT"].ToString();
                    textBox7.Text = LimitDüzenle(limit) + " TL";

                    if (dr["HESAP"].ToString()== "0")
                    {
                        comboBox1.SelectedIndex = 0;
                    }
                    else
                    {
                        comboBox1.SelectedIndex = 1;
                    }
                    textBox3.Text = dr["MüsteriNot"].ToString();
                }
            }
            catch(Exception ex) {
                MessageBox.Show("HATA! : "+ex.Message);
            }
        }
        
        private string LimitDüzenle(string limit) //veritabanına gelen limit değerindeki noktayı (,) yapıyor
        {
            string sayi = "";
           foreach(char s in limit)
           {
                if(s.ToString() == ".")
                {
                    sayi += ",";
                }
                else 
                {
                    sayi += s.ToString();
                }
           }
           return sayi;
        }

        public void kaydet() //(doğru)
        {
            int hesapDurumu = 0;

            if (comboBox1.SelectedIndex == 1)
            {
                hesapDurumu = 1;
            }

            if (string.IsNullOrEmpty(textBox2.Text))
                MessageBox.Show("Lütfen Gerekli Unvan Alanını Doldurunuz", "Öğretmen İşlemleri", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            
            else
            {
                dateTimePicker1.Format = DateTimePickerFormat.Short;
                SQLiteCommand cmd = new SQLiteCommand("Update MÜSTERİ set ID=@p1,TARİH=@p2,UNVAN=@p3,TELEFON=@p4,ADRES=@p5,İL=@p6,İLCE=@p7,LİMİT=@p8,HESAP=@p9,MüsteriNot=@p10 where ID=@p1", con);
                cmd.Parameters.AddWithValue("@p1", textBox1.Text);
                cmd.Parameters.AddWithValue("@p2", dateTimePicker1.Text);//tarih
                cmd.Parameters.AddWithValue("@p3", textBox2.Text);
                cmd.Parameters.AddWithValue("@p4", maskedTextBox1.Text); //tel
                cmd.Parameters.AddWithValue("@p5", textBox4.Text);
                cmd.Parameters.AddWithValue("@p6", textBox5.Text);
                cmd.Parameters.AddWithValue("@p7", textBox6.Text);
                cmd.Parameters.AddWithValue("@p8", m.LimitDüzenle(textBox7)); //limit
                cmd.Parameters.AddWithValue("@p9", hesapDurumu);
                cmd.Parameters.AddWithValue("@p10", textBox3.Text);
                dateTimePicker1.Format = DateTimePickerFormat.Long;

                try
                {
                    if (!(con.State == ConnectionState.Open))
                        con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Güncelleme İşlemi Başarılı", "Cari İşlemleri", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                finally { con.Close(); }
            }
        }
        

        private void button3_Click(object sender, EventArgs e)
        {
            
            kaydet();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            kaydet();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
        }
        private bool ilkTıklama = true;
        private void maskedTextBox1_Click(object sender, EventArgs e)
        { //DEĞİŞTİR
            if(ilkTıklama)
            {
                maskedTextBox1.SelectAll();
                ilkTıklama=false;
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); //kapat
        }
        //buraya
    }
}
