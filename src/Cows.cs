using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace cowpaws
{
    public partial class Cows : Form
    {
        public Cows()                   // constructor class untuk form Cows
        {
            InitializeComponent();      // inisialisasi komponen pada form Cows
            Populate();                 // method dijalankan saat form Cows diinisialisasi
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\db_cowpaws2.mdf;Integrated Security=True;Connect Timeout=30");

        private void label12_Click(object sender, EventArgs e)      // method untuk menampilkan form Produksi ketika tombol menu Produksi diklik
        {
            Produksi Ob = new Produksi();       // membuat objek baru dari class Produksi
            Ob.Show();                          // menampilkan objek Produksi
            this.Hide();                        // menyembunyikan form saat ini
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

        private void label17_Click(object sender, EventArgs e)
        {
            Zakat Ob = new Zakat();
            Ob.Show();
            this.Hide();
        }

        private void Populate()     // method untuk mengisi DataGridView dengan data dari tabel t_cows
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM t_cows";                      // query untuk mengambil data dari tabel t_cows
                SqlDataAdapter sda = new SqlDataAdapter(query, Con);        // membuat adapter SQL dengan query dan koneksi
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);     // membuat builder command SQL
                var ds = new DataSet();                                     // membuat objek dataset
                sda.Fill(ds);                                               // mengisi dataset dengan data dari tabel t_cows
                CowsDGV1.DataSource = ds.Tables[0];                         // mengatur DataGridView dengan data dari dataset
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat mengisi data: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }

        private void Clear()    // method untuk menghapus teks pada textbox
        {
            Nama1.Text = "";            // menghapus teks di textbox Nama
            LabelTelinga1.Text = "";
            Warna1.Text = "";
            Jenis1.Text = "";
            Usia1.Text = "";
            Berat1.Text = "";
            key = 0;                    // mengatur nilai key menjadi 0
        }

        private void Search()       // method untuk mencari data dengan nama yang mengandung teks pada fitur Filter
        {
            Con.Open();
            string query = "SELECT * FROM t_cows WHERE Nama LIKE '%" + Search1.Text + "%'";     // query untuk mencari data dari tabel t_cows dengan nama yang mengandung teks sama
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);                                // membuat adapter SQL dengan query dan koneksi
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);                             // membuat builder command SQL
            var ds = new DataSet();                                                             // membuat objek dataset
            sda.Fill(ds);                                                                       // mengisi dataset dengan data dari tabel t_cows yang sesuai dengan query
            CowsDGV1.DataSource = ds.Tables[0];                                                 // mengatur DataGridView dengan data dari dataset
            Con.Close();
        }

        private void Search1_OnValueChanged(object sender, EventArgs e)     // method yang dipanggil saat nilai teks pada Search1 berubah, untuk mencari data yang sesuai
        {
            Search();
        }

        int age = 0;                                                                                // inisialisasi variabel umur dengan nilai 0                  
        private void TanggalLahir1_ValueChanged(object sender, EventArgs e)                         // method yang dipanggil saat tanggal lahir sapi diubah, untuk menghitung umur sapi berdasarkan tanggal lahir
        {
            age = Convert.ToInt32((DateTime.Today.Date - TanggalLahir1.Value.Date).Days) / 365;     // menghitung umur sapi berdasarkan tanggal lahir
        }

        private void TanggalLahir1_MouseLeave(object sender, EventArgs e)                           // method yang dipanggil saat kursor keluar dari textbox TanggalLahir
        {
            age = Convert.ToInt32((DateTime.Today.Date - TanggalLahir1.Value.Date).Days) / 365;     
            Usia1.Text = "" + age;                                                                  // menampilkan umur sapi pada textbox Usia
        }

        private void button1_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Simpan
        {
            if (Nama1.Text == "" || LabelTelinga1.Text == "" || Warna1.Text == "" || Jenis1.Text == "" || Usia1.Text == "" || Berat1.Text == "")    // validasi apakah data yang diinputkan sudah lengkap atau tidak
            {
                MessageBox.Show("Data belum lengkap");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO t_cows VALUES (@Nama, @LabelTelinga, @Warna, @Jenis, @TanggalLahir, @Usia, @Berat)";    // query untuk memasukkan data ke dalam tabel t_cows
                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@Nama", Nama1.Text);                       // menambahkan nilai parameter ke dalam objek SqlCommand dengan menggunakan AddWithValue
                    cmd.Parameters.AddWithValue("@LabelTelinga", LabelTelinga1.Text);
                    cmd.Parameters.AddWithValue("@Warna", Warna1.Text);
                    cmd.Parameters.AddWithValue("@Jenis", Jenis1.Text);
                    cmd.Parameters.AddWithValue("@TanggalLahir", TanggalLahir1.Value.Date);
                    cmd.Parameters.AddWithValue("@Usia", age);
                    cmd.Parameters.AddWithValue("@Berat", Berat1.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil disimpan");

                    Con.Close();
                    Populate();     // menampilkan data terbaru pada DataGridView
                    Clear();        // membersihkan inputan pada form
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Edit
        {
            if (Nama1.Text == "" || LabelTelinga1.Text == "" || Warna1.Text == "" || Jenis1.Text == "" || Usia1.Text == "" || Berat1.Text == "")        // validasi apakah data yang diinputkan sudah lengkap atau tidak
            {
                MessageBox.Show("Data belum lengkap");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "UPDATE t_cows SET Nama=@Nama, LabelTelinga=@LabelTelinga, Warna=@Warna, Jenis=@Jenis, TanggalLahir=@TanggalLahir, Usia=@Usia, Berat=@Berat WHERE id=@id;";      // query untuk melakukan update data ke dalam tabel t_cows
                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@Id", key);
                    cmd.Parameters.AddWithValue("@Nama", Nama1.Text);                       // menambahkan nilai parameter ke dalam objek SqlCommand dengan menggunakan AddWithValue
                    cmd.Parameters.AddWithValue("@LabelTelinga", LabelTelinga1.Text);
                    cmd.Parameters.AddWithValue("@Warna", Warna1.Text);
                    cmd.Parameters.AddWithValue("@Jenis", Jenis1.Text);
                    cmd.Parameters.AddWithValue("@TanggalLahir", TanggalLahir1.Value.Date);
                    cmd.Parameters.AddWithValue("@Usia", age);
                    cmd.Parameters.AddWithValue("@Berat", Berat1.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil diupdate");

                    Con.Close();
                    Populate();     // menampilkan data terbaru pada DataGridView
                    Clear();        // membersihkan inputan pada form
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)  // method untuk menghandle event click pada tombol Hapus
        {
            if (key == 0)                                               // validasi apakah variabel key memiliki nilai 0 atau tidak
            {
                MessageBox.Show("Pilih data sapi yang akan dihapus");   // jika iya, maka munculkan pesan kesalahan
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "DELETE FROM t_cows WHERE id=" + key + ";";  // query untuk menghapus data dari tabel t_cows
                    SqlCommand cmd = new SqlCommand(Query, Con);                // membuat objek SqlCommand
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil dihapus");

                    Con.Close();
                    Populate();         // menampilkan data terbaru pada DataGridView
                    Clear();            // membersihkan inputan pada form
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)  // method untuk menghandle event click pada tombol Clear
        {
            Clear();    // memanggil method Clear untuk mengosongkan data yang ada di form
        }

        int key = 0;    // membuat variabel key dengan nilai 0
        private void CowsDGV1_CellContentClick(object sender, DataGridViewCellEventArgs e)      // method yang dijalankan ketika pengguna mengklik salah satu baris data pada DataGridView
        {
            Nama1.Text = CowsDGV1.SelectedRows[0].Cells[1].Value.ToString();            // menampilkan data yang dipilih ke dalam textbox Nama Sapi
            LabelTelinga1.Text = CowsDGV1.SelectedRows[0].Cells[2].Value.ToString();
            Warna1.Text = CowsDGV1.SelectedRows[0].Cells[3].Value.ToString();
            Jenis1.Text = CowsDGV1.SelectedRows[0].Cells[4].Value.ToString();
            TanggalLahir1.Text = CowsDGV1.SelectedRows[0].Cells[5].Value.ToString();
            Berat1.Text = CowsDGV1.SelectedRows[0].Cells[7].Value.ToString();
            if (Nama1.Text == "")       // validasi apakah textbox Nama1 kosong atau tidak
            {
                key = 0;                // variabel key diisi dengan nilai 0
                age = 0;
            }
            else
            {
                key = Convert.ToInt32(CowsDGV1.SelectedRows[0].Cells[0].Value.ToString()); // variabel key diisi dengan nilai ID sapi yang dipilih
                age = Convert.ToInt32(CowsDGV1.SelectedRows[0].Cells[6].Value.ToString());
            }
        }

        private void Cows_Load(object sender, EventArgs e)      // method untuk menyesuaikan lebar kolom pada DataGridView saat aplikasi di-load
        {
            int columnCount = CowsDGV1.Columns.Count;                       // mengambil jumlah kolom pada DataGridView
            int totalWidth = CowsDGV1.Width - CowsDGV1.RowHeadersWidth;     // menghitung total lebar DataGridView dengan mengurangi lebar header row

            for (int i = 0; i < columnCount; i++)   // loop untuk mengatur lebar kolom pada DataGridView
            {
                if (i == 0)
                {
                    CowsDGV1.Columns[i].Width = (int)(totalWidth * 0.02); // set lebar kolom ke 2% dari total lebar DataGridView
                }
                else if (i == 1)
                {
                    CowsDGV1.Columns[i].Width = (int)(totalWidth * 0.09);
                }
                else if (i == 2)
                {
                    CowsDGV1.Columns[i].Width = (int)(totalWidth * 0.05);
                }
                else if (i == 3)
                {
                    CowsDGV1.Columns[i].Width = (int)(totalWidth * 0.04);
                }
                else if (i == 4)
                {
                    CowsDGV1.Columns[i].Width = (int)(totalWidth * 0.05);
                }
                else if (i == 5)
                {
                    CowsDGV1.Columns[i].Width = (int)(totalWidth * 0.05);
                }
                else if (i == 6)
                {
                    CowsDGV1.Columns[i].Width = (int)(totalWidth * 0.02);
                }
                else if (i == 7)
                {
                    CowsDGV1.Columns[i].Width = (int)(totalWidth * 0.07);
                }
            }
        }

        private void BtnClose1_Click(object sender, EventArgs e)    // method untuk menghandle event click pada tombol Close
        {
            Application.Exit();
        }

        private void Logout1_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Logout
        {
            Login login = new Login();      // membuat instance objek Login
            login.Show();                   // menampilkan form Login
            this.Hide();
        }
    }
}