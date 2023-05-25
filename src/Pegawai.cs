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
    public partial class Pegawai : Form
    {
        public Pegawai()                // constructor class untuk form Pegawai
        {
            InitializeComponent();      // inisialisasi komponen pada form Pegawai
            Populate();                 // method dijalankan saat form Cows diinisialisasi
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\db_cowpaws2.mdf;Integrated Security=True;Connect Timeout=30");

        private void Clear()                // method untuk menghapus teks pada textbox
        {
            Nama8.Text = "";                // menghapus teks di textbox Nama
            Gender8.SelectedIndex = -1;
            Telpon8.Text = "";
            Password8.Text = "";
            key = 0;                        // mengatur nilai key menjadi 0
        }

        private void Populate()         // method untuk mengisi DataGridView dengan data dari tabel t_pegawai
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM t_pegawai";                      // query untuk mengambil data dari tabel t_cows
                SqlDataAdapter sda = new SqlDataAdapter(query, Con);           // membuat adapter SQL dengan query dan koneksi
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);        // membuat builder command SQL
                var ds = new DataSet();                                        // membuat objek dataset
                sda.Fill(ds);                                                  // mengisi dataset dengan data dari tabel t_cows
                PegawaiDGV8.DataSource = ds.Tables[0];                         // mengatur DataGridView dengan data dari dataset
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

        private void button1_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Simpan
        {
            if (Nama8.Text == "" || Gender8.SelectedIndex == -1 || Telpon8.Text == "" || Password8.Text == "")      // validasi apakah data yang diinputkan sudah lengkap atau tidak
            {
                MessageBox.Show("Data belum lengkap");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO t_pegawai VALUES ('" + Nama8.Text + "','" + Gender8.SelectedItem.ToString() + "','" + Telpon8.Text + "','" + Password8.Text + "')";   // query untuk memasukkan data ke dalam tabel t_pegawai
                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil disimpan");

                    Con.Close();
                    Populate();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Edit
        {
            if (Nama8.Text == "" || Gender8.SelectedIndex == -1 || Telpon8.Text == "" || Password8.Text == "")      // validasi apakah data yang diinputkan sudah lengkap atau tidak
            {
                MessageBox.Show("Data belum lengkap");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "UPDATE t_pegawai SET Nama='" + Nama8.Text + "',Gender='" + Gender8.SelectedItem.ToString() + "',NoTelpon='" + Telpon8.Text + "',Password='" + Password8.Text + "' WHERE id=" + key + ";";    // query untuk melakukan update data ke dalam tabel t_pegawai
                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil diupdate");

                    Con.Close();
                    Populate();
                    Clear();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Hapus
        {
            if (key == 0)                                           // validasi apakah variabel key memiliki nilai 0 atau tidak
            {
                MessageBox.Show("Pilih data yang akan dihapus");    // jika iya, maka munculkan pesan kesalahan
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "DELETE FROM t_pegawai WHERE id=" + key + ";";       // query untuk menghapus data dari tabel t_pegawai
                    SqlCommand cmd = new SqlCommand(Query, Con);                        // membuat objek SqlCommand
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

        private void button4_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Clear
        {
            Clear();    // memanggil method Clear untuk mengosongkan data yang ada di form
        }

        int key = 0;    // membuat variabel key dengan nilai 0
        private void PegawaiDGV8_CellContentClick(object sender, DataGridViewCellEventArgs e)    // method yang dijalankan ketika pengguna mengklik salah satu baris data pada DataGridView
        {
            Nama8.Text = PegawaiDGV8.SelectedRows[0].Cells[1].Value.ToString();         // menampilkan data yang dipilih ke dalam textbox Nama
            Gender8.Text = PegawaiDGV8.SelectedRows[0].Cells[2].Value.ToString();
            Telpon8.Text = PegawaiDGV8.SelectedRows[0].Cells[3].Value.ToString();
            Password8.Text = PegawaiDGV8.SelectedRows[0].Cells[4].Value.ToString();
            if (Nama8.Text == "")       // validasi apakah textbox Nama1 kosong atau tidak
            {
                key = 0;                // variabel key diisi dengan nilai 0
            }
            else
            {
                key = Convert.ToInt32(PegawaiDGV8.SelectedRows[0].Cells[0].Value.ToString());    // variabel key diisi dengan nilai ID yang dipilih
            }
        }

        private void Pegawai_Load(object sender, EventArgs e)       // method untuk menyesuaikan lebar kolom pada DataGridView saat aplikasi di-load
        {
            int columnCount = PegawaiDGV8.Columns.Count;                        // mengambil jumlah kolom pada DataGridView
            int totalWidth = PegawaiDGV8.Width - PegawaiDGV8.RowHeadersWidth;   // menghitung total lebar DataGridView dengan mengurangi lebar header row

            for (int i = 0; i < columnCount; i++)       // loop untuk mengatur lebar kolom pada DataGridView
            {
                if (i == 0)
                {
                    PegawaiDGV8.Columns[i].Width = (int)(totalWidth * 0.02); // set lebar kolom ke 2% dari total lebar DataGridView
                }
                else if (i == 2)
                {
                    PegawaiDGV8.Columns[i].Width = (int)(totalWidth * 0.04);
                }
                else if (i == 1)
                {
                    PegawaiDGV8.Columns[i].Width = (int)(totalWidth * 0.18);
                }
                else if (i == 3)
                {
                    PegawaiDGV8.Columns[i].Width = (int)(totalWidth * 0.06);
                }
                else if (i == 4)
                {
                    PegawaiDGV8.Columns[i].Width = (int)(totalWidth * 0.26);
                }
            }
        }

        private void BtnClose8_Click(object sender, EventArgs e)     // method untuk menghandle event click pada tombol Close
        {
            Application.Exit();
        }

        private void Logout8_Click(object sender, EventArgs e)       // method untuk menghandle event click pada tombol Logout
        {
            Login login = new Login();      // membuat instance objek Login
            login.Show();                   // menampilkan form Login
            this.Hide();
        }
    }
}