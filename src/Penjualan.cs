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
    public partial class Penjualan : Form
    {
        public Penjualan()              // constructor class untuk form Penjualan
        {
            InitializeComponent();      // inisialisasi komponen pada form Penjualan
            FillEmployeeId();           // method dijalankan saat form Produksi diinisialisasi
            Populate();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\db_cowpaws2.mdf;Integrated Security=True;Connect Timeout=30");

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

        private void Clear()            // method untuk menghapus teks pada textbox
        {
            Pelanggan5.Text = "";       // menghapus teks di textbox Nama Pelanggan
            Telpon5.Text = "";
            Harga5.Text = "";
            Jumlah5.Text = "";
            Total5.Text = "";
            Usia5.Text = "";
        }

        private void FillEmployeeId()       // method untuk mengisi data ID Pegawai dari database ke dalam combobox Id Pegawai
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT id FROM t_pegawai", Con);   // membuat command untuk mengambil data id dari tabel t_pegawai
            SqlDataReader Rdr;                                                  // deklarasi objek SqlDataReader
            Rdr = cmd.ExecuteReader();                                          // menjalankan command dan mendapatkan data dalam bentuk reader
            DataTable dt = new DataTable();                                     // membuat objek DataTable
            dt.Columns.Add("id", typeof(int));                                  // menambahkan kolom id ke dalam objek DataTable
            dt.Load(Rdr);                                                       // memuat data dari reader ke dalam objek DataTable
            Id5.ValueMember = "id";                                             // mengatur nilai yang akan disimpan sebagai value dari combobox Id Pegawai
            Id5.DataSource = dt;                                                // mengatur sumber data dari combobox Id Pegawai
            Con.Close();
        }

        private void Populate()     // method untuk mengisi DataGridView dengan data dari tabel t_penjualan
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM t_penjualan";                 // query untuk mengambil data dari tabel t_penjualan
                SqlDataAdapter sda = new SqlDataAdapter(query, Con);        // membuat adapter SQL dengan query dan koneksi
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);     // membuat builder command SQL
                var ds = new DataSet();                                     // membuat objek dataset
                sda.Fill(ds);                                               // mengisi dataset dengan data dari tabel t_penjualan
                PenjualanDGV5.DataSource = ds.Tables[0];                    // mengatur DataGridView dengan data dari dataset
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

        private void InsertData()       // method untuk memasukkan data penjualan ke dalam database
        {
            try
            {
                if (Con.State == ConnectionState.Open)      // cek apakah koneksi sedang terbuka, jika iya maka tutup
                {
                    Con.Close();
                }

                Con.Open();
                string Query = "INSERT INTO t_pemasukan VALUES (@Tanggal, @Tipe, @Jumlah, @IdPegawai)";     // query untuk memasukkan data ke tabel t_pemasukan
                SqlCommand cmd = new SqlCommand(Query, Con);
                cmd.Parameters.AddWithValue("@Tanggal", Tanggal5.Value.Date);               // menambahkan parameter ke dalam command
                cmd.Parameters.AddWithValue("@Tipe", "Penjualan");
                cmd.Parameters.AddWithValue("@Jumlah", Convert.ToInt32(Total5.Text));
                cmd.Parameters.AddWithValue("@IdPegawai", Id5.SelectedValue.ToString());
                cmd.ExecuteNonQuery();
                Con.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Simpan
        {
            if (Id5.SelectedIndex == -1 || Pelanggan5.Text == "" || Telpon5.Text == "" || Harga5.Text == "" || Jumlah5.Text == "" || Usia5.Text == "")      // validasi apakah data yang diinputkan sudah lengkap atau tidak
            {
                MessageBox.Show("Data belum lengkap");
            }
            else
            {
                try
                {
                    Con.Open();

                    int total = Convert.ToInt32(Harga5.Text) * Convert.ToInt32(Jumlah5.Text);   // menghitung total harga
                    Total5.Text = total.ToString();

                    string Query = "INSERT INTO t_penjualan VALUES (@Id, @Tanggal, @Pelanggan, @NoTelpPelanggan, @Harga, @Jumlah, @Total, @Usia)";  // query untuk memasukkan data ke dalam tabel t_penjualan
                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@Id", Id5.SelectedValue.ToString());       // menambahkan nilai parameter ke dalam objek SqlCommand dengan menggunakan AddWithValue
                    cmd.Parameters.AddWithValue("@Tanggal", Tanggal5.Value.Date);
                    cmd.Parameters.AddWithValue("@Pelanggan", Pelanggan5.Text);
                    cmd.Parameters.AddWithValue("@NoTelpPelanggan", Telpon5.Text);
                    cmd.Parameters.AddWithValue("@Harga", Harga5.Text);
                    cmd.Parameters.AddWithValue("@Jumlah", Jumlah5.Text);
                    cmd.Parameters.AddWithValue("@Total", Total5.Text);
                    cmd.Parameters.AddWithValue("@Usia", Usia5.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil disimpan");

                    InsertData();       // menyimpan data ke tabel t_pemasukan

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

        private void button4_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Clear
        {
            Clear();        // memanggil method Clear untuk mengosongkan data yang ada di form
        }

        private void Penjualan_Load(object sender, EventArgs e)     // method untuk menyesuaikan lebar kolom pada DataGridView saat aplikasi di-load
        {
            int columnCount = PenjualanDGV5.Columns.Count;                            // mengambil jumlah kolom pada DataGridView
            int totalWidth = PenjualanDGV5.Width - PenjualanDGV5.RowHeadersWidth;     // menghitung total lebar DataGridView dengan mengurangi lebar header row

            for (int i = 0; i < columnCount; i++)       // loop untuk mengatur lebar kolom pada DataGridView
            {
                if (i == 0)
                {
                    PenjualanDGV5.Columns[i].Width = (int)(totalWidth * 0.02);      // set lebar kolom ke 2% dari total lebar DataGridView
                }
                else if (i == 1)
                {
                    PenjualanDGV5.Columns[i].Width = (int)(totalWidth * 0.05);
                }
                else if (i == 6 || i == 7)
                {
                    PenjualanDGV5.Columns[i].Width = (int)(totalWidth * 0.04);
                }
                else if (i == 2)
                {
                    PenjualanDGV5.Columns[i].Width = (int)(totalWidth * 0.06);
                }
                else if (i == 5)
                {
                    PenjualanDGV5.Columns[i].Width = (int)(totalWidth * 0.03);
                }
                else if (i == 3)
                {
                    PenjualanDGV5.Columns[i].Width = (int)(totalWidth * 0.09);
                }
                else if (i == 4)
                {
                    PenjualanDGV5.Columns[i].Width = (int)(totalWidth * 0.08);
                }
                else if (i == 8)
                {
                    PenjualanDGV5.Columns[i].Width = (int)(totalWidth * 0.06);
                }
            }
        }

        private void BtnClose5_Click(object sender, EventArgs e)    // method untuk menghandle event click pada tombol Close
        {
            Application.Exit();
        }

        private void Logout5_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Logout
        {
            Login login = new Login();      // membuat instance objek Login
            login.Show();                   // menampilkan form Login
            this.Hide();
        }
    }
}