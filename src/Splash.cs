using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cowpaws
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }

        int mulai = 0;  // variabel untuk menyimpan nilai mulai

        private void timer1_Tick(object sender, EventArgs e)    // method yang akan dipanggil secara berkala pada interval tertentu
        {
            mulai += 1;                     // variabel mulai diincrement setiap kali timer1_Tick dipanggil
            ProgressBar.Value = mulai;      // nilai ProgressBar diperbarui menjadi variabel mulai
            if (ProgressBar.Value == 100)   // jika nilai ProgressBar mencapai 100, maka loading selesai
            {
                ProgressBar.Value = 0;      // nilai ProgressBar diatur kembali ke 0
                timer1.Stop();
                Login log = new Login();    // form Login dipanggil
                this.Hide();
                log.Show();
            }
        }

        private void Splash_Load(object sender, EventArgs e)    // method yang dipanggil saat form Splash di-load
        {
            timer1.Interval = 15;   // interval pada timer1 diatur menjadi 15 milidetik
            timer1.Start();
        }
    }
}