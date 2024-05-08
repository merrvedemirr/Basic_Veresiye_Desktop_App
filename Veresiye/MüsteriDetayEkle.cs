using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Veresiye
{
    public partial class MüsteriDetayEkle : Form
    {
        public MüsteriDetayEkle()
        {
            InitializeComponent();
            
        }

        private void MüsteriDetayEkle_Load(object sender, EventArgs e)
        {
           
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach(TabPage tab in tabControl1.Controls) 
            {
                if(tab == tabControl1.SelectedTab) 
                {
                    //BURAYA SEÇİLİ TABPAGENİN BAŞLIĞINI MAVİ YAPMA KODU GELECEK
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
