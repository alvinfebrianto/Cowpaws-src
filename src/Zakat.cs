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
    public partial class Zakat : Form
    {
        public Zakat()                  // constructor class untuk form Zakat
        {
            InitializeComponent();      // inisialisasi komponen pada form Zakat
        }

        private void label11_Click(object sender, EventArgs e)      // method untuk menampilkan form Cows ketika tombol menu Cows diklik
        {
            Cows Ob = new Cows();       // membuat objek baru dari class Cows
            Ob.Show();                  // menampilkan objek Cows
            this.Hide();                // menyembunyikan form saat ini
        }

        private void label12_Click(object sender, EventArgs e)
        {
            Produksi Ob = new Produksi();
            Ob.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            Kesehatan Ob = new Kesehatan();
            Ob.Show();
            this.Hide();
        }

        private void label14_Click(object sender, EventArgs e)
        {
            Breeding Ob = new Breeding();
            Ob.Show();
            this.Hide();
        }

        private void label15_Click(object sender, EventArgs e)
        {
            Penjualan Ob = new Penjualan();
            Ob.Show();
            this.Hide();
        }

        private void label16_Click(object sender, EventArgs e)
        {
            Keuangan Ob = new Keuangan();
            Ob.Show();
            this.Hide();
        }

        private Timer timer = new Timer();      // membuat objek Timer baru

        private void JumlahSapi7_OnValueChanged(object sender, EventArgs e)   // method yang dipanggil ketika nilai pada input JumlahSapi7 diubah
        {
            timer.Stop();       // menghentikan Timer agar tidak terjadi overlap saat input diubah
            timer.Start();      // memulai Timer kembali

            if (string.IsNullOrEmpty(JumlahSapi7.Text))     // cek apakah input kosong
            {
                Zakat7.Text = "";
                Hasil7.Text = "";
            }
        }

        private void timer_Tick(object sender, EventArgs e)     // method yang dipanggil setelah Timer berakhir
        {
            timer.Stop();                           // menghentikan Timer agar tidak berjalan terus menerus
            string sapiStr = JumlahSapi7.Text;      // mengambil nilai dari input JumlahSapi7
            
            if (string.IsNullOrEmpty(sapiStr))      // cek apakah input kosong
            {
                Zakat7.Text = "";
                Hasil7.Text = "";
                return;
            }

            float sapi = indonesianNumberToFloat(sapiStr);      // mengubah nilai input menjadi float menggunakan fungsi indonesianNumberToFloat()
            if (sapi < 30)                                      // menentukan kategori zakat berdasarkan nilai input sapi
            {
                Zakat7.Text = "0";
                Hasil7.Text = "Sapi belum mencapai nishab. Tidak dikenakan kewajiban zakat.";
            }
            else if (sapi >= 30 && sapi < 40)
            {
                Zakat7.Text = "1";
                Hasil7.Text = "Zakat 1 ekor sapi jenis Tabi’.";
            }
            else if (sapi >= 40 && sapi < 60)
            {
                Zakat7.Text = "1";
                Hasil7.Text = "Zakat 1 ekor sapi jenis Musinnah.";
            }
            else if (sapi >= 60 && sapi < 70)
            {
                Zakat7.Text = "2";
                Hasil7.Text = "Zakat 2 ekor sapi jenis Tabi’.";
            }
            else if (sapi >= 70 && sapi < 80)
            {
                Zakat7.Text = "2";
                Hasil7.Text = "Zakat 1 ekor sapi Tabi’ dan 1 ekor sapi jenis Musinnah.";
            }
            else if (sapi >= 80 && sapi < 90)
            {
                Zakat7.Text = "2";
                Hasil7.Text = "Zakat 2 ekor sapi jenis Musinnah.";
            }
            else if (sapi >= 90 && sapi < 100)
            {
                Zakat7.Text = "3";
                Hasil7.Text = "Zakat 3 ekor sapi jenis Tabi’.";
            }
            else if (sapi >= 100 && sapi < 110)
            {
                Zakat7.Text = "3";
                Hasil7.Text = "Zakat 1 ekor sapi jenis Musinnah dan 2 ekor sapi jenis Tabi’.";
            }
            else if (sapi >= 110 && sapi < 120)
            {
                Zakat7.Text = "3";
                Hasil7.Text = "Zakat 1 ekor sapi jenis Tabi’ dan 2 ekor sapi jenis Musinnah.";
            }
            else if (sapi >= 120 && sapi < 130)
            {
                Zakat7.Text = "4 atau 3";
                Hasil7.Text = "Zakat 4 ekor sapi jenis Tabi’ atau 3 ekor sapi jenis Musinnah.";
            }
        }

        private float indonesianNumberToFloat(string str)    // method untuk mengonversi format angka Indonesia ke tipe float
        {
            str = str.Replace(".", "");                 // mengganti karakter "." dengan ""
            str = str.Replace(",", ".");                // mengganti karakter "," dengan "."
            float numfloat = 0.0f;
            if (!float.TryParse(str, out numfloat))     // melakukan parsing string ke tipe float
            {
                return 0.0f;                            // jika parsing gagal, return 0.0f
            }
            else
            {
                return numfloat;                        // jika parsing berhasil, return nilai float hasil parsing
            }
        }

        private void Zakat_Load(object sender, EventArgs e)         // method event handler untuk form load
        {
            timer.Interval = 500;                                   // set interval timer
            timer.Tick += new EventHandler(timer_Tick);             // tambahkan event handler untuk timer tick
        }

        private void BtnClose7_Click(object sender, EventArgs e)    // method untuk menghandle event click pada tombol Close
        {
            Application.Exit();
        }

        private void Logout7_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Logout
        {
            Login login = new Login();      // membuat instance objek Login
            login.Show();                   // menampilkan form Login
            this.Hide();
        }
    }
}