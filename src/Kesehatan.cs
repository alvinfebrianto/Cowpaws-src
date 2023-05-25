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
    public partial class Kesehatan : Form
    {
        public Kesehatan()              // constructor class untuk form Kesehatan
        {
            InitializeComponent();      // inisialisasi komponen pada form Produksi
            FillCowId();                // method dijalankan saat form Produksi diinisialisasi
            Populate();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\db_cowpaws2.mdf;Integrated Security=True;Connect Timeout=30");

        private void label11_Click(object sender, EventArgs e)  // method untuk menampilkan form Cows ketika tombol menu Cows diklik
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

        private void Clear()        // method untuk menghapus teks pada textbox
        {
            Nama3.Text = "";        // menghapus teks di textbox Nama
            Keluhan3.Text = "";
            Diagnosis3.Text = "";
            Treatment3.Text = "";
            Biaya3.Text = "";
            Vet3.Text = "";
            key = 0;                // mengatur nilai key menjadi 0
        }

        private void Populate()     // method untuk mengisi DataGridView dengan data dari tabel t_kesehatan
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM t_kesehatan";                 // query untuk mengambil data dari tabel t_kesehatan
                SqlDataAdapter sda = new SqlDataAdapter(query, Con);        // membuat adapter SQL dengan query dan koneksi
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);     // membuat builder command SQL
                var ds = new DataSet();                                     // membuat objek dataset
                sda.Fill(ds);                                               // mengisi dataset dengan data dari tabel t_kesehatan
                KesehatanDGV3.DataSource = ds.Tables[0];                    // mengatur DataGridView dengan data dari dataset
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

        private void FillCowId()        // method untuk mengisi nilai kolom id
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT id FROM t_cows", Con);  // membuat objek SqlCommand dan mengeksekusi query SELECT
            SqlDataReader Rdr;                                              // deklarasi objek SqlDataReader
            Rdr = cmd.ExecuteReader();                                      // mengeksekusi objek SqlCommand dan menyimpan hasilnya ke objek SqlDataReader
            DataTable dt = new DataTable();                                 // membuat objek DataTable
            dt.Columns.Add("id", typeof(int));                              // menambahkan kolom id ke objek DataTable
            dt.Load(Rdr);                                                   // memuat data dari objek SqlDataReader ke objek DataTable
            Id3.ValueMember = "id";                                         // mengatur nilai dari ValueMember pada objek Id3
            Id3.DataSource = dt;

            Con.Close();
        }

        private void GetCowName()       // method untuk mengambil nilai nama dari sapi berdasarkan id
        {
            Con.Open();
            string query = "SELECT * FROM t_cows WHERE id=" + Id3.SelectedValue.ToString() + "";    // membuat query SELECT
            SqlCommand cmd = new SqlCommand(query, Con);                                            // membuat objek SqlCommand dan mengeksekusi query SELECT
            DataTable dt = new DataTable();                                                         // membuat objek DataTable
            SqlDataAdapter sda = new SqlDataAdapter(cmd);                                           // membuat objek SqlDataAdapter
            sda.Fill(dt);                                                                           // memuat data dari objek SqlDataAdapter ke objek DataTable
            foreach (DataRow dr in dt.Rows)                                                         // melakukan loop untuk setiap baris data pada objek DataTable
            {
                Nama3.Text = dr["Nama"].ToString();                                                 // mengisi nilai pada textbox Nama3 dengan nilai Nama pada baris data
            }

            Con.Close();
        }

        private void Id3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetCowName();       // memanggil method GetCowName
        }

        private void button1_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Simpan
        {
            if (Id3.SelectedIndex == -1 || Nama3.Text == "" || Keluhan3.Text == "" || Diagnosis3.Text == "" || Treatment3.Text == "" || Biaya3.Text == "" || Vet3.Text == "")   // validasi apakah data yang diinputkan sudah lengkap atau tidak
            {
                MessageBox.Show("Data belum lengkap");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO t_kesehatan VALUES (@Id, @Nama, @Keluhan, @Diagnosis, @Tanggal, @Treatment, @Biaya, @Vet)";     // query untuk memasukkan data ke dalam tabel t_produksi
                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@Id", Id3.SelectedValue.ToString());   // menambahkan nilai parameter ke dalam objek SqlCommand dengan menggunakan AddWithValue
                    cmd.Parameters.AddWithValue("@Nama", Nama3.Text);
                    cmd.Parameters.AddWithValue("@Keluhan", Keluhan3.Text);
                    cmd.Parameters.AddWithValue("@Diagnosis", Diagnosis3.Text);
                    cmd.Parameters.AddWithValue("@Tanggal", Tanggal3.Value.Date);
                    cmd.Parameters.AddWithValue("@Treatment", Treatment3.Text);
                    cmd.Parameters.AddWithValue("@Biaya", Biaya3.Text);
                    cmd.Parameters.AddWithValue("@Vet", Vet3.Text);
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
            if (Id3.SelectedIndex == -1 || Nama3.Text == "" || Keluhan3.Text == "" || Diagnosis3.Text == "" || Treatment3.Text == "" || Biaya3.Text == "" || Vet3.Text == "")   // validasi apakah data yang diinputkan sudah lengkap atau tidak
            {
                MessageBox.Show("Data belum lengkap");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "UPDATE t_kesehatan SET Nama=@Nama, Keluhan=@Keluhan, Diagnosis=@Diagnosis, Tanggal=@Tanggal, Treatment=@Treatment, Biaya=@Biaya, Vet=@Vet WHERE id=@id;";   // query untuk melakukan update data ke dalam tabel t_kesehatan
                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@Id", key);                        // menambahkan nilai parameter ke dalam objek SqlCommand dengan menggunakan AddWithValue
                    cmd.Parameters.AddWithValue("@Nama", Nama3.Text);
                    cmd.Parameters.AddWithValue("@Keluhan", Keluhan3.Text);
                    cmd.Parameters.AddWithValue("@Diagnosis", Diagnosis3.Text);
                    cmd.Parameters.AddWithValue("@Tanggal", Tanggal3.Value.Date);
                    cmd.Parameters.AddWithValue("@Treatment", Treatment3.Text);
                    cmd.Parameters.AddWithValue("@Biaya", Biaya3.Text);
                    cmd.Parameters.AddWithValue("@Vet", Vet3.Text);
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

        private void button3_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Hapus
        {
            if (key == 0)                                           // validasi apakah variabel key memiliki nilai 0 atau tidak
            {
                MessageBox.Show("Pilih data yang akan dihapus");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "DELETE FROM t_kesehatan WHERE id=" + key + ";";  // query untuk menghapus data dari tabel t_kesehatan
                    SqlCommand cmd = new SqlCommand(Query, Con);                     // membuat objek SqlCommand
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
        private void KesehatanDGV3_CellContentClick(object sender, DataGridViewCellEventArgs e) // method yang dijalankan ketika pengguna mengklik salah satu baris data pada DataGridView
        {
            Id3.Text = KesehatanDGV3.SelectedRows[0].Cells[1].Value.ToString();         // menampilkan data yang dipilih ke dalam combobox Id Sapi
            Nama3.Text = KesehatanDGV3.SelectedRows[0].Cells[2].Value.ToString();
            Keluhan3.Text = KesehatanDGV3.SelectedRows[0].Cells[3].Value.ToString();
            Diagnosis3.Text = KesehatanDGV3.SelectedRows[0].Cells[4].Value.ToString();
            Tanggal3.Text = KesehatanDGV3.SelectedRows[0].Cells[5].Value.ToString();
            Treatment3.Text = KesehatanDGV3.SelectedRows[0].Cells[6].Value.ToString();
            Biaya3.Text = KesehatanDGV3.SelectedRows[0].Cells[7].Value.ToString();
            Vet3.Text = KesehatanDGV3.SelectedRows[0].Cells[8].Value.ToString();
            if (Nama3.Text == "")       // validasi apakah textbox Nama1 kosong atau tidak
            {
                key = 0;                // variabel key diisi dengan nilai 0
            }
            else
            {
                key = Convert.ToInt32(KesehatanDGV3.SelectedRows[0].Cells[0].Value.ToString());     // variabel key diisi dengan nilai id yang dipilih
            }
        }

        private void Kesehatan_Load(object sender, EventArgs e)     // method untuk menyesuaikan lebar kolom pada DataGridView saat aplikasi di-load
        {
            int columnCount = KesehatanDGV3.Columns.Count;                          // mengambil jumlah kolom pada DataGridView
            int totalWidth = KesehatanDGV3.Width - KesehatanDGV3.RowHeadersWidth;   // menghitung total lebar DataGridView dengan mengurangi lebar header row

            for (int i = 0; i < columnCount; i++)
            {
                if (i == 0)
                {
                    KesehatanDGV3.Columns[i].Width = (int)(totalWidth * 0.06); // set lebar kolom ke 6% dari total lebar DataGridView
                }
                else if (i == 1)
                {
                    KesehatanDGV3.Columns[i].Width = (int)(totalWidth * 0.09);
                }
                else if (i == 2)
                {
                    KesehatanDGV3.Columns[i].Width = (int)(totalWidth * 0.17);
                }
                else if (i == 6)
                {
                    KesehatanDGV3.Columns[i].Width = (int)(totalWidth * 0.2);
                }
                else if (i == 7)
                {
                    KesehatanDGV3.Columns[i].Width = (int)(totalWidth * 0.16);
                }
                else if (i == 3 || i == 4)
                {
                    KesehatanDGV3.Columns[i].Width = (int)(totalWidth * 0.14);
                }
                else if (i == 5)
                {
                    KesehatanDGV3.Columns[i].Width = (int)(totalWidth * 0.15);
                }
                else if (i == 8)
                {
                    KesehatanDGV3.Columns[i].Width = (int)(totalWidth * 0.12);
                }
            }
        }

        private void BtnClose3_Click(object sender, EventArgs e)    // method untuk menghandle event click pada tombol Close
        {
            Application.Exit();
        }

        private void Logout3_Click(object sender, EventArgs e)  // method untuk menghandle event click pada tombol Logout
        {
            Login login = new Login();      // membuat instance objek Login
            login.Show();                   // menampilkan form Login
            this.Hide();
        }
    }
}