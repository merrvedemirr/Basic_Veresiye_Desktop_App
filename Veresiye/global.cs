using System;
using System.Collections.Generic;
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
    internal class global
    {
        public static int toplamCari = 0; //Toplam cari Sayısı 
        public static int CariKod = 0; //Tıklanılan Cari Kod
        public static int YeniCariKod = 0; //eklenen kişini carisi
    }
}
