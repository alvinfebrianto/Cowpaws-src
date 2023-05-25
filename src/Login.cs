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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\db_cowpaws2.mdf;Integrated Security=True;Connect Timeout=30");

        private void label3_Click(object sender, EventArgs e)   // method untuk mengosongkan username dan password ketika tombol Reset di-klik
        {
            Username0.Text = "";
            Password0.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)  // method yang akan dipanggil saat tombol Login di-klik
        {
            if (Username0.Text == "" || Password0.Text == "")                   // validasi apakah username dan password kosong
            {
                MessageBox.Show("Masukkan username dan password");
            }
            else
            {
                if (Role0.SelectedIndex == -1)                                  // validasi apakah Role telah dipilih atau tidak
                {
                    MessageBox.Show("Pilih posisi pekerjaan");
                }
                else if (Role0.SelectedItem.ToString() == "Admin")              // jika Role dipilih sebagai Admin
                {
                    if (Username0.Text == "Admin" && Password0.Text == "Admin") // validasi apakah username dan password sesuai dengan yang ditentukan
                    {
                        Pegawai prod = new Pegawai();       // membuat instance form Pegawai
                        prod.Show();                        // menampilkan form Pegawai
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Username dan password salah");
                    }
                }
                else    // jika Role dipilih sebagai Pegawai
                {
                    Con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM t_pegawai WHERE Nama='" + Username0.Text + "' AND Password='" + Password0.Text + "'", Con);  // membuat instance SqlDataAdapter
                    DataTable dt = new DataTable();                         // membuat instance DataTable
                    sda.Fill(dt);                                           // mengisi DataTable dengan data dari SqlDataAdapter
                    if (dt.Rows[0][0].ToString() == "1")                    // jika data Pegawai dengan username dan password yang sesuai ditemukan
                    {
                        Cows Cow = new Cows();                              // membuat instance form Cows
                        Cow.Show();                                         // menampilkan form Cows
                        this.Hide();
                    }
                    else  // jika data Pegawai tidak ditemukan
                    {
                        MessageBox.Show("Username dan password salah");
                    }
                    Con.Close();
                }
            }
        }

        private void BtnClose0_Click(object sender, EventArgs e)    // method yang akan dipanggil saat tombol close di-klik
        {
            Application.Exit();
        }
    }
}