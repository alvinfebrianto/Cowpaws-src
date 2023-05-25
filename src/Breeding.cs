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
    public partial class Breeding : Form
    {
        public Breeding()               // constructor class untuk form Breeding
        {
            InitializeComponent();      // inisialisasi komponen pada form Breeding
            FillCowId();                // method dijalankan saat form Breeding diinisialisasi
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

        private void Clear()
        {
            Nama4.Text = "";
            Usia4.Text = "";
            key = 0;
        }

        private void Populate()     // method untuk mengisi DataGridView dengan data dari tabel t_breeding
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM t_breeding";                  // query untuk mengambil data dari tabel t_breeding
                SqlDataAdapter sda = new SqlDataAdapter(query, Con);        // membuat adapter SQL dengan query dan koneksi
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);     // membuat builder command SQL
                var ds = new DataSet();                                     // membuat objek dataset
                sda.Fill(ds);                                               // mengisi dataset dengan data dari tabel t_breeding
                BreedingDGV4.DataSource = ds.Tables[0];                     // mengatur DataGridView dengan data dari dataset
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
            Id4.ValueMember = "id";                                         // mengatur nilai dari ValueMember pada objek Id4
            Id4.DataSource = dt;

            Con.Close();
        }

        private void GetCowName()       // method untuk mengambil nilai nama dari sapi berdasarkan id
        {
            Con.Open();
            string query = "SELECT * FROM t_cows WHERE id=" + Id4.SelectedValue.ToString() + "";    // membuat query SELECT
            SqlCommand cmd = new SqlCommand(query, Con);                                            // membuat objek SqlCommand dan mengeksekusi query SELECT
            DataTable dt = new DataTable();                                                         // membuat objek DataTable
            SqlDataAdapter sda = new SqlDataAdapter(cmd);                                           // membuat objek SqlDataAdapter
            sda.Fill(dt);                                                                           // memuat data dari objek SqlDataAdapter ke objek DataTable
            foreach (DataRow dr in dt.Rows)                                                         // melakukan loop untuk setiap baris data pada objek DataTable
            {
                Nama4.Text = dr["Nama"].ToString();                                                 // mengisi nilai pada textbox Nama4 dengan nilai Nama pada baris data
                Usia4.Text = dr["Usia"].ToString();
            }

            Con.Close();
        }

        private void Id4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetCowName();       // memanggil method GetCowName
        }

        private void button1_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Simpan
        {
            if (Id4.SelectedIndex == -1 || Nama4.Text == "" || Usia4.Text == "")    // validasi apakah data yang diinputkan sudah lengkap atau tidak
            {
                MessageBox.Show("Data belum lengkap");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO t_breeding VALUES (@Id, @Nama, @Usia, @TanggalBirahi, @TanggalKawin, @TanggalHamil, @PerkiraanKelahiran, @TanggalLahir)";   // query untuk memasukkan data ke dalam tabel t_breeding
                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@Id", Id4.SelectedValue.ToString());           // menambahkan nilai parameter ke dalam objek SqlCommand dengan menggunakan AddWithValue
                    cmd.Parameters.AddWithValue("@Nama", Nama4.Text);
                    cmd.Parameters.AddWithValue("@Usia", Usia4.Text);
                    cmd.Parameters.AddWithValue("@TanggalBirahi", Birahi4.Value.Date);
                    cmd.Parameters.AddWithValue("@TanggalKawin", Kawin4.Value.Date);
                    cmd.Parameters.AddWithValue("@TanggalHamil", Hamil4.Value.Date);
                    cmd.Parameters.AddWithValue("@PerkiraanKelahiran", Perkiraan4.Value.Date);
                    cmd.Parameters.AddWithValue("@TanggalLahir", Lahir4.Value.Date);
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
            if (Id4.SelectedIndex == -1 || Nama4.Text == "" || Usia4.Text == "")    // validasi apakah data yang diinputkan sudah lengkap atau tidak
            {
                MessageBox.Show("Data belum lengkap");
            }
            else
            {
                try
                {
                    Con.Open();
                    // buat query untuk meng-update data di tabel "t_breeding"
                    string Query = "UPDATE t_breeding SET Nama=@Nama, Usia=@Usia, TanggalBirahi=@TanggalBirahi, TanggalKawin=@TanggalKawin, TanggalHamil=@TanggalHamil, PerkiraanKelahiran=@PerkiraanKelahiran, TanggalLahir=@TanggalLahir WHERE id=@id;";  // query untuk melakukan update data ke dalam tabel t_breeding
                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@Id", key);                                    // menambahkan nilai parameter ke dalam objek SqlCommand dengan menggunakan AddWithValue
                    cmd.Parameters.AddWithValue("@Nama", Nama4.Text);
                    cmd.Parameters.AddWithValue("@Usia", Usia4.Text);
                    cmd.Parameters.AddWithValue("@TanggalBirahi", Birahi4.Value.Date);
                    cmd.Parameters.AddWithValue("@TanggalKawin", Kawin4.Value.Date);
                    cmd.Parameters.AddWithValue("@TanggalHamil", Hamil4.Value.Date);
                    cmd.Parameters.AddWithValue("@PerkiraanKelahiran", Perkiraan4.Value.Date);
                    cmd.Parameters.AddWithValue("@TanggalLahir", Lahir4.Value.Date);
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
                    string Query = "DELETE FROM t_breeding WHERE id=" + key + ";";  // query untuk menghapus data dari tabel t_breeding
                    SqlCommand cmd = new SqlCommand(Query, Con);                    // membuat objek SqlCommand
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
            Clear();        // memanggil method Clear untuk mengosongkan data yang ada di form
        }

        int key = 0;        // membuat variabel key dengan nilai 0
        private void BreedingDGV4_CellContentClick(object sender, DataGridViewCellEventArgs e)  // method yang dijalankan ketika pengguna mengklik salah satu baris data pada DataGridView
        {
            Id4.Text = BreedingDGV4.SelectedRows[0].Cells[1].Value.ToString();          // menampilkan data yang dipilih ke dalam combobox Id Sapi
            Nama4.Text = BreedingDGV4.SelectedRows[0].Cells[2].Value.ToString();
            Usia4.Text = BreedingDGV4.SelectedRows[0].Cells[3].Value.ToString();
            Birahi4.Text = BreedingDGV4.SelectedRows[0].Cells[4].Value.ToString();
            Kawin4.Text = BreedingDGV4.SelectedRows[0].Cells[5].Value.ToString();
            Hamil4.Text = BreedingDGV4.SelectedRows[0].Cells[6].Value.ToString();
            Perkiraan4.Text = BreedingDGV4.SelectedRows[0].Cells[7].Value.ToString();
            Lahir4.Text = BreedingDGV4.SelectedRows[0].Cells[8].Value.ToString();
            if (Nama4.Text == "")       // validasi apakah textbox Nama4 kosong atau tidak
            {
                key = 0;                // variabel key diisi dengan nilai 0
            }
            else
            {
                key = Convert.ToInt32(BreedingDGV4.SelectedRows[0].Cells[0].Value.ToString());  // variabel key diisi dengan nilai id yang dipilih
            }
        }

        private void Breeding_Load(object sender, EventArgs e)      // method untuk menyesuaikan lebar kolom pada DataGridView saat aplikasi di-load
        {
            int columnCount = BreedingDGV4.Columns.Count;                           // mengambil jumlah kolom pada DataGridView
            int totalWidth = BreedingDGV4.Width - BreedingDGV4.RowHeadersWidth;     // menghitung total lebar DataGridView dengan mengurangi lebar header row

            for (int i = 0; i < columnCount; i++)       // loop untuk mengatur lebar kolom pada DataGridView
            {
                if (i == 0)
                {
                    BreedingDGV4.Columns[i].Width = (int)(totalWidth * 0.08); // set lebar kolom ke 8% dari total lebar DataGridView
                }
                else if (i == 3)
                {
                    BreedingDGV4.Columns[i].Width = (int)(totalWidth * 0.07);
                }
                else if (i == 1)
                {
                    BreedingDGV4.Columns[i].Width = (int)(totalWidth * 0.09);
                }
                else if (i == 2)
                {
                    BreedingDGV4.Columns[i].Width = (int)(totalWidth * 0.18);
                }
                else if (i == 7)
                {
                    BreedingDGV4.Columns[i].Width = (int)(totalWidth * 0.12);
                }
                else if (i == 4 || i == 5 || i == 6)
                {
                    BreedingDGV4.Columns[i].Width = (int)(totalWidth * 0.17);
                }
                else if (i == 8)
                {
                    BreedingDGV4.Columns[i].Width = (int)(totalWidth * 0.14);
                }
            }
        }

        private void BtnClose4_Click(object sender, EventArgs e)    // method untuk menghandle event click pada tombol Close
        {
            Application.Exit();
        }

        private void Logout4_Click(object sender, EventArgs e)      // method untuk menghandle event click pada tombol Logout
        {
            Login login = new Login();      // membuat instance objek Login
            login.Show();                   // menampilkan form Login
            this.Hide();
        }
    }
}