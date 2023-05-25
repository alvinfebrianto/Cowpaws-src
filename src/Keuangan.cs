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
    public partial class Keuangan : Form
    {
        public Keuangan()               // constructor class untuk form Keuangan
        {
            InitializeComponent();      // inisialisasi komponen pada form Keuangan
            FillEmployeeId();           // method dijalankan saat form Keuangan diinisialisasi
            PopulateEx();
            PopulateIn();
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

        private void label15_Click(object sender, EventArgs e)
        {
            Penjualan Ob = new Penjualan();
            Ob.Show();
            this.Hide();
        }

        private void label17_Click(object sender, EventArgs e)
        {
            Zakat Ob = new Zakat();
            Ob.Show();
            this.Hide();
        }

        private void FillEmployeeId()       // method untuk mengisi data ID Pegawai dari database ke dalam combobox ID
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT id FROM t_pegawai", Con);   // membuat command untuk mengambil data id dari tabel t_pegawai
            SqlDataReader Rdr;                                                  // deklarasi objek SqlDataReader
            Rdr = cmd.ExecuteReader();                                          // menjalankan command dan mendapatkan data dalam bentuk reader
            DataTable dt = new DataTable();                                     // membuat objek DataTable
            dt.Columns.Add("id", typeof(int));                                  // menambahkan kolom id ke dalam objek DataTable
            dt.Load(Rdr);                                                       // memuat data dari reader ke dalam objek DataTable
            Id6.ValueMember = "id";                                             // mengatur nilai yang akan disimpan sebagai value dari combobox ID
            Id6.DataSource = dt;                                                // mengatur sumber data dari combobox ID
            Con.Close();
        }

        private void PopulateEx()       // method untuk mengisi DataGridView dengan data dari tabel t_pengeluaran
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM t_pengeluaran";               // query untuk mengambil data dari tabel t_pengeluaran
                SqlDataAdapter sda = new SqlDataAdapter(query, Con);        // membuat adapter SQL dengan query dan koneksi
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);     // membuat builder command SQL
                var ds = new DataSet();                                     // membuat objek dataset
                sda.Fill(ds);                                               // mengisi dataset dengan data dari tabel t_pengeluaran
                ExDGV6.DataSource = ds.Tables[0];                           // mengatur DataGridView dengan data dari dataset
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

        private void PopulateIn()       // method untuk mengisi DataGridView dengan data dari tabel t_pemasukan
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM t_pemasukan";                 // query untuk mengambil data dari tabel t_pemasukan
                SqlDataAdapter sda = new SqlDataAdapter(query, Con);        // membuat adapter SQL dengan query dan koneksi
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);     // membuat builder command SQL
                var ds = new DataSet();                                     // membuat objek dataset
                sda.Fill(ds);                                               // mengisi dataset dengan data dari tabel t_pemasukan
                InDGV6.DataSource = ds.Tables[0];                           // mengatur DataGridView dengan data dari dataset
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

        private void FilterEx()     // method untuk memfilter data pengeluaran berdasarkan tanggal
        {
            Con.Open();
            string query = "SELECT * FROM t_pengeluaran WHERE Tanggal=@Tanggal";    // membuat query untuk menampilkan data pengeluaran berdasarkan tanggal
            SqlCommand cmd = new SqlCommand(query, Con);
            cmd.Parameters.AddWithValue("@Tanggal", FilterEx6.Value.Date);          // mengatur parameter Tanggal dengan nilai dari tombol FilterEx6
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ExDGV6.DataSource = ds.Tables[0];                                       // mengatur DataSource dari ExDGV6 dengan tabel hasil query
            Con.Close();
        }

        private void FilterIn()     // method untuk memfilter data pengeluaran berdasarkan tanggal
        {
            Con.Open();
            string query = "SELECT * FROM t_pemasukan WHERE Tanggal=@Tanggal";      // membuat query untuk menampilkan data pengeluaran berdasarkan tanggal
            SqlCommand cmd = new SqlCommand(query, Con);
            cmd.Parameters.AddWithValue("@Tanggal", FilterIn6.Value.Date);          // mengatur parameter Tanggal dengan nilai dari tombol Filterin6
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            InDGV6.DataSource = ds.Tables[0];                                       // mengatur DataSource dari InDGV6 dengan tabel hasil query
            Con.Close();
        }

        private void FilterEx6_ValueChanged(object sender, EventArgs e)     // method pada event handler untuk melakukan filter pada data pengeluaran ketika nilai tanggal diubah
        {
            FilterEx();
        }

        private void FilterIn6_ValueChanged(object sender, EventArgs e)     // method pada event handler untuk melakukan filter pada data pemasukan ketika nilai tanggal diubah
        {
            FilterIn();
        }

        private void pictureBox12_Click(object sender, EventArgs e)     // memanggil method PopulateEx saat tombol refresh diklik
        {
            PopulateEx();
        }

        private void pictureBox11_Click(object sender, EventArgs e)     // memanggil method PopulateIn saat tombol refresh diklik
        {
            PopulateIn();
        }

        private void button1_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Simpan
        {
            if (PengadaanEx6.SelectedIndex == -1 || JumlahEx6.Text == "" || Id6.SelectedIndex == -1)        // validasi apakah data yang diinputkan sudah lengkap atau tidak
            {
                MessageBox.Show("Data belum lengkap");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO t_pengeluaran VALUES (@Tanggal, @Pengadaan, @Jumlah, @IdPegawai)";      // query untuk memasukkan data ke dalam tabel t_pengeluaran
                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@Tanggal", TanggalEx6.Value.Date);                                                                                 // menambahkan nilai parameter ke dalam objek SqlCommand dengan menggunakan AddWithValue
                    cmd.Parameters.AddWithValue("@Pengadaan", PengadaanEx6.SelectedItem != null ? PengadaanEx6.SelectedItem.ToString() : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Jumlah", JumlahEx6.Text);
                    cmd.Parameters.AddWithValue("@IdPegawai", Id6.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil disimpan");

                    Con.Close();
                    PopulateEx();       // menampilkan data terbaru pada DataGridView
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Simpan
        {
            if (TipeIn6.SelectedIndex == -1 || JumlahIn6.Text == "" || Id6.SelectedIndex == -1)     // validasi apakah data yang diinputkan sudah lengkap atau tidak
            {
                MessageBox.Show("Data belum lengkap");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO t_pemasukan VALUES (@Tanggal, @Tipe, @Jumlah, @IdPegawai)";     // query untuk memasukkan data ke dalam tabel t_pemasukan
                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@Tanggal", TanggalIn6.Value.Date);                             // menambahkan nilai parameter ke dalam objek SqlCommand dengan menggunakan AddWithValue
                    cmd.Parameters.AddWithValue("@Tipe", TipeIn6.SelectedItem != null ? TipeIn6.SelectedItem.ToString() : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Jumlah", JumlahIn6.Text);
                    cmd.Parameters.AddWithValue("@IdPegawai", Id6.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil disimpan");

                    Con.Close();
                    PopulateIn();       // menampilkan data terbaru pada DataGridView
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void Keuangan_Load(object sender, EventArgs e)
        {
            int columnCountEx = ExDGV6.Columns.Count;                       // mengambil jumlah kolom pada DataGridView
            int columnCountIn = InDGV6.Columns.Count;
            int totalWidthEx = ExDGV6.Width - ExDGV6.RowHeadersWidth;       // menghitung total lebar DataGridView dengan mengurangi lebar header row 
            int totalWidthIn = InDGV6.Width - InDGV6.RowHeadersWidth;
            

            for (int i = 0; i < columnCountEx; i++)     // loop untuk mengatur lebar kolom pada DataGridView
            {
                if (i == 0)
                {
                    ExDGV6.Columns[i].Width = (int)(totalWidthEx * 0.02);   // set lebar kolom ke 2% dari total lebar DataGridView
                }
                else if (i == 1)
                {
                    ExDGV6.Columns[i].Width = (int)(totalWidthEx * 0.05);
                }
                else if (i == 2)
                {
                    ExDGV6.Columns[i].Width = (int)(totalWidthEx * 0.12);
                }
                else if (i == 3)
                {
                    ExDGV6.Columns[i].Width = (int)(totalWidthEx * 0.06);
                }
                else if (i == 4)
                {
                    ExDGV6.Columns[i].Width = (int)(totalWidthEx * 0.17);
                }
            }
            for (int i = 0; i < columnCountIn; i++)
            {
                if (i == 0)
                {
                    InDGV6.Columns[i].Width = (int)(totalWidthIn * 0.02);   // set lebar kolom ke 2% dari total lebar DataGridView
                }
                else if (i == 1)
                {
                    InDGV6.Columns[i].Width = (int)(totalWidthIn * 0.05);
                }
                else if (i == 2)
                {
                    InDGV6.Columns[i].Width = (int)(totalWidthIn * 0.12);
                }
                else if (i == 3)
                {
                    InDGV6.Columns[i].Width = (int)(totalWidthIn * 0.06);
                }
                else if (i == 4)
                {
                    InDGV6.Columns[i].Width = (int)(totalWidthIn * 0.17);
                }
            }
        }

        private void BtnClose6_Click(object sender, EventArgs e)    // method untuk menghandle event click pada tombol Close
        {
            Application.Exit();
        }

        private void Logout6_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Logout
        {
            Login login = new Login();      // membuat instance objek Login
            login.Show();                   // menampilkan form Login
            login.Show();
            this.Hide();
        }
    }
}