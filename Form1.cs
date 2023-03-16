using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
namespace CRUDMysqlDatabase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            string connection = "server=localhost;user id=root;password=;database=zeamart";
            string query = "SELECT * FROM tbl_barang";
            MySqlConnection conn = new MySqlConnection(connection);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            dt.Columns.Add("PICTURE", Type.GetType("System.Byte[]"));

            foreach (DataRow row in dt.Rows)
            {
                row["PICTURE"] = File.ReadAllBytes(Application.StartupPath + @"\Image\" + Path.GetFileName(row["IMAGE"].ToString()));
            }
            dataGridView1.DataSource = dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string connection = "server=localhost;user id=root;password=;database=zeamart";
            string query = "INSERT INTO tbl_barang(Tipe_Barang,Merk_Barang,Harga_Beli,Harga_Jual,Stok_Barang,IMAGE)VALUES('"+ this.TIPEBARANG.Text + "','" + this.MERKBARANG.Text + "','" + this.HARGABELI.Text + "','" + this.HARGAJUAL.Text + "','" + this.STOKBARANG.Text + "','" + Path.GetFileName(pictureBox1.ImageLocation) + "')";
            MySqlConnection conn = new MySqlConnection(connection);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader dr;
            conn.Open();
            dr = cmd.ExecuteReader();
            MessageBox.Show("Succesfully saved");
            conn.Close();
            File.Copy(imageText.Text, Application.StartupPath + @"\Image\" + Path.GetFileName(pictureBox1.ImageLocation));
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string connection = "server=localhost;user id=root;password=;database=zeamart";
            string query = "UPDATE tbl_barang SET Tipe_Barang='" + this.TIPEBARANG.Text + "',Merk_Barang='" + this.MERKBARANG.Text + "',Harga_Beli='" + this.HARGABELI.Text + "',Harga_Jual='" + this.HARGAJUAL.Text + "',Stok_Barang='" + this.STOKBARANG.Text + "' WHERE ID='"+ this.ID.Text +"'";
            MySqlConnection conn = new MySqlConnection(connection);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader dr;
            conn.Open();
            dr = cmd.ExecuteReader();
            MessageBox.Show("Record has been updated successfully");
            conn.Close();
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            string connection = "server=localhost;user id=root;password=;database=zeamart";
            string query = "SELECT * FROM tbl_barang";
            MySqlConnection conn = new MySqlConnection(connection);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            dt.Columns.Add("PICTURE", Type.GetType("System.Byte[]"));

            foreach(DataRow row in dt.Rows)
            {
                row["PICTURE"] = File.ReadAllBytes(Application.StartupPath + @"\Image\" + Path.GetFileName(row["IMAGE"].ToString()));
            }
            dataGridView1.DataSource = dt;
        }

        public void id_increment()
        {
            MySqlConnection connection = new MySqlConnection("server=localhost;user id=root;password=;database=zeamart");
            MySqlScript script = new MySqlScript(connection, "SET @id :=0;\r\nUPDATE tbl_barang SET id = @id := (@id + 1);\r\nALTER TABLE `tbl_barang` AUTO_INCREMENT = 1;");
            script.Execute();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string connection = "server=localhost;user id=root;password=;database=zeamart";
            string query = "DELETE  FROM tbl_barang WHERE ID = '"+this.ID.Text+"'";
            MySqlConnection conn = new MySqlConnection(connection);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader dr;
            conn.Open();
            dr = cmd.ExecuteReader();
            MessageBox.Show("Data has been succesfully deleted!");
            id_increment();
            conn.Close();    
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string connection = "server=localhost;user id=root;password=;database=zeamart";
            MySqlConnection con = new MySqlConnection(connection);
            MySqlDataAdapter da;
            DataTable dt;
            con.Open();
            da = new MySqlDataAdapter("SELECT * FROM tbl_barang WHERE Merk_Barang LIKE '" + this.textBox1.Text + "%' OR Tipe_Barang LIKE '" + this.textBox1.Text + "%'", con);
            dt = new DataTable();
            da.Fill(dt);
            dt.Columns.Add("PICTURE", Type.GetType("System.Byte[]"));

            foreach (DataRow row in dt.Rows)
            {
                row["PICTURE"] = File.ReadAllBytes(Application.StartupPath + @"\Image\" + Path.GetFileName(row["IMAGE"].ToString()));
            }
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfd = new OpenFileDialog();
            openfd.Filter = "Image Files(*.jpg;*.jpeg;*.gif;)  |  *.jpg;*.jpeg;*.gif;";
            if(openfd.ShowDialog()==DialogResult.OK)
            {
                imageText.Text = openfd.FileName;
                pictureBox1.Image = new Bitmap(openfd.FileName);
                pictureBox1.ImageLocation = openfd.FileName;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            String ID = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            String TipeBarang = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            String MerkBarang = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            String HargaBeli = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            String HargaJual = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            String StokBarang = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            this.ID.Text = ID;
            this.TIPEBARANG.Text = TipeBarang;
            this.MERKBARANG.Text = MerkBarang;
            this.HARGABELI.Text = HargaBeli;
            this.HARGAJUAL.Text = HargaJual;
            this.STOKBARANG.Text = StokBarang;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void MERKBARANG_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
