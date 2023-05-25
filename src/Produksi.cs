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
    public partial class Produksi : Form
    {
        public Produksi()               // constructor class untuk form Produksi
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

        private void Clear()        // method untuk menghapus teks pada textbox
        {
            Nama2.Text = "";        // menghapus teks di textbox Nama
            Pagi2.Text = "";
            Siang2.Text = "";
            Malam2.Text = "";
            Total2.Text = "";
            key = 0;                // mengatur nilai key menjadi 0
        }

        private void Populate()     // method untuk mengisi DataGridView dengan data dari tabel t_produksi
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM t_produksi";                  // query untuk mengambil data dari tabel t_produksi
                SqlDataAdapter sda = new SqlDataAdapter(query, Con);        // membuat adapter SQL dengan query dan koneksi
                SqlCommandBuilder builder = new SqlCommandBuilder(sda);     // membuat builder command SQL
                var ds = new DataSet();                                     // membuat objek dataset
                sda.Fill(ds);                                               // mengisi dataset dengan data dari tabel t_produksi
                ProduksiDGV2.DataSource = ds.Tables[0];                     // mengatur DataGridView dengan data dari dataset
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
            Id2.ValueMember = "id";                                         // mengatur nilai dari ValueMember pada objek Id2
            Id2.DataSource = dt;

            Con.Close();
        }

        private void GetCowName()       // method untuk mengambil nilai nama dari sapi berdasarkan id
        {
            Con.Open();
            string query = "SELECT * FROM t_cows WHERE id=" + Id2.SelectedValue.ToString() + "";    // membuat query SELECT
            SqlCommand cmd = new SqlCommand(query, Con);                                            // membuat objek SqlCommand dan mengeksekusi query SELECT
            DataTable dt = new DataTable();                                                         // membuat objek DataTable
            SqlDataAdapter sda = new SqlDataAdapter(cmd);                                           // membuat objek SqlDataAdapter
            sda.Fill(dt);                                                                           // memuat data dari objek SqlDataAdapter ke objek DataTable
            foreach (DataRow dr in dt.Rows)                                                         // melakukan loop untuk setiap baris data pada objek DataTable
            {
                Nama2.Text = dr["Nama"].ToString();                                                 // mengisi nilai pada textbox Nama2 dengan nilai Nama pada baris data
            }

            Con.Close();
        }

        private void Id2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetCowName();       // memanggil method GetCowName
        }

        private void Malam2_Leave(object sender, EventArgs e)   // method untuk mengeksekusi perhitungan total produksi sapi pada saat user mengakhiri input pada textbox Malam
        {
            int total = Convert.ToInt32(Pagi2.Text) + Convert.ToInt32(Siang2.Text) + Convert.ToInt32(Malam2.Text);  // menghitung total produksi sapi mulai Pagi, Siang, dan Malam
            Total2.Text = "" + total;
        }

        private void button1_Click(object sender, EventArgs e)  // method untuk menghandle event click pada tombol Simpan
        {
            if (Id2.SelectedIndex == -1 || Nama2.Text == "" || Pagi2.Text == "" || Siang2.Text == "" || Malam2.Text == "" || Total2.Text == "")     // validasi apakah data yang diinputkan sudah lengkap atau tidak
            {
                MessageBox.Show("Data belum lengkap");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "INSERT INTO t_produksi VALUES (@Id, @Nama, @Pagi, @Siang, @Malam, @Total, @Tanggal)";   // query untuk memasukkan data ke dalam tabel t_produksi
                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@Id", Id2.SelectedValue.ToString());   // menambahkan nilai parameter ke dalam objek SqlCommand dengan menggunakan AddWithValue
                    cmd.Parameters.AddWithValue("@Nama", Nama2.Text);
                    cmd.Parameters.AddWithValue("@Pagi", Pagi2.Text);
                    cmd.Parameters.AddWithValue("@Siang", Siang2.Text);
                    cmd.Parameters.AddWithValue("@Malam", Malam2.Text);
                    cmd.Parameters.AddWithValue("@Total", Total2.Text);
                    cmd.Parameters.AddWithValue("@Tanggal", Tanggal2.Value.Date);
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
            if (Id2.SelectedIndex == -1 || Nama2.Text == "" || Pagi2.Text == "" || Siang2.Text == "" || Malam2.Text == "" || Total2.Text == "")     // validasi apakah data yang diinputkan sudah lengkap atau tidak
            {
                MessageBox.Show("Data belum lengkap");
            }
            else
            {
                try
                {
                    Con.Open();
                    string Query = "UPDATE t_produksi SET Nama=@Nama, Pagi=@Pagi, Siang=@Siang, Malam=@Malam, Total=@Total, Tanggal=@Tanggal WHERE id=@Id;";    // query untuk melakukan update data ke dalam tabel t_produksi
                    SqlCommand cmd = new SqlCommand(Query, Con);
                    cmd.Parameters.AddWithValue("@Id", key);                // menambahkan nilai parameter ke dalam objek SqlCommand dengan menggunakan AddWithValue
                    cmd.Parameters.AddWithValue("@Nama", Nama2.Text);
                    cmd.Parameters.AddWithValue("@Pagi", Pagi2.Text);
                    cmd.Parameters.AddWithValue("@Siang", Siang2.Text);
                    cmd.Parameters.AddWithValue("@Malam", Malam2.Text);
                    cmd.Parameters.AddWithValue("@Total", Total2.Text);
                    cmd.Parameters.AddWithValue("@Tanggal", Tanggal2.Value);
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
                    string Query = "DELETE FROM t_produksi WHERE id=" + key + ";";  // query untuk menghapus data dari tabel t_produksi
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
            Clear();    // memanggil method Clear untuk mengosongkan data yang ada di form
        }

        int key = 0;    // membuat variabel key dengan nilai 0
        private void ProduksiDGV2_CellContentClick(object sender, DataGridViewCellEventArgs e)  // method yang dijalankan ketika pengguna mengklik salah satu baris data pada DataGridView
        {
            Id2.Text = ProduksiDGV2.SelectedRows[0].Cells[1].Value.ToString();      // menampilkan data yang dipilih ke dalam combobox Id Sapi
            Nama2.Text = ProduksiDGV2.SelectedRows[0].Cells[2].Value.ToString();
            Pagi2.Text = ProduksiDGV2.SelectedRows[0].Cells[3].Value.ToString();
            Siang2.Text = ProduksiDGV2.SelectedRows[0].Cells[4].Value.ToString();
            Malam2.Text = ProduksiDGV2.SelectedRows[0].Cells[5].Value.ToString();
            Total2.Text = ProduksiDGV2.SelectedRows[0].Cells[6].Value.ToString();
            Tanggal2.Text = ProduksiDGV2.SelectedRows[0].Cells[7].Value.ToString();
            if (Nama2.Text == "")       // validasi apakah textbox Nama2 kosong atau tidak
            {
                key = 0;                // variabel key diisi dengan nilai 0
            }
            else
            {
                key = Convert.ToInt32(ProduksiDGV2.SelectedRows[0].Cells[0].Value.ToString());  // variabel key diisi dengan nilai id yang dipilih
            }
        }

        private void Produksi_Load(object sender, EventArgs e)      // method untuk menyesuaikan lebar kolom pada DataGridView saat aplikasi di-load
        {
            int columnCount = ProduksiDGV2.Columns.Count;           // mendapatkan jumlah kolom di DataGridView
            for (int i = 0; i < columnCount; i++)                   // set lebar kolom 0 dan 3-6
            {
                if (i == 1 || i == 3 || i == 4 || i == 5 || i == 6)
                {
                    ProduksiDGV2.Columns[i].Width = 17;             // set lebar kolom ke 17
                }
            }
            
            ProduksiDGV2.Columns[0].Width = 10; // atur lebar kolom 0 ke 10
            ProduksiDGV2.Columns[2].Width = 100;
            ProduksiDGV2.Columns[7].Width = 83;
        }

        private void BtnClose2_Click(object sender, EventArgs e)    // method untuk menghandle event click pada tombol Close
        {
            Application.Exit();
        }

        private void Logout2_Click(object sender, EventArgs e)  // method untuk menghandle event click pada tombol Logout
        {
            Login login = new Login();      // membuat instance objek Login
            login.Show();                   // menampilkan form Login
            this.Hide();
        }
    }
}