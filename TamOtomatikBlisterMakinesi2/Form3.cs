using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;


namespace TamOtomatikBlisterMakinesi2
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
       
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\Veriler\\Fiksturler.mdb");
        OleDbDataAdapter da;
        DataSet ds;

        int i = 0;



        private void Form3_Load(object sender, EventArgs e)
        {

            ShowData1("Select * from fikstur1");





            /*******************************************************Gizlenen Alanlar****************************************************/

            // btnSave.Visible = false;


        }

        private void ShowData1(string veri)
        {
            /*
            OleDbDataReader oku = new OleDbDataReader("Select * From fikstur1");
            OleDbDataAdapter da = new OleDbDataAdapter("Select * from fikstur1", conn);
            DataSet ds = new DataSet();
            conn.Open();
            da.Fill(ds,"fikstur1");
            dataGridView1.DataSource = ds.Tables["fikstur1"];
            conn.Close();*/

            da = new OleDbDataAdapter(veri, conn);
            ds = new DataSet();
            conn.Open();
            da.Fill(ds, "fikstur1");
            dataGridView1.DataSource = ds.Tables["fikstur1"];
            dataGridView2.DataSource = ds.Tables["fikstur1"];
            conn.Close();

        }



        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void DataAdd()
        {
            OleDbCommand com = new OleDbCommand("insert into fikstur1 (posNo1, x, y, wBas, wBit, wSure)values (@posNo1, @x, @y, @wBas, @wBit, @wSure)", conn);
            conn.Open();

            // textBox5.Text = da
            // com.Parameters.AddWithValue("@posNo1", textBox5.Text);
            com.Parameters.AddWithValue("@x", textBox10.Text);
            com.Parameters.AddWithValue("@y", textBox9.Text);
            com.Parameters.AddWithValue("@wBas", textBox8.Text);
            com.Parameters.AddWithValue("@wBit", textBox7.Text);
            com.Parameters.AddWithValue("@wSure", textBox6.Text);
            com.ExecuteNonQuery();
            conn.Close();
        }


        private void button3_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows[0].HeaderCell.Value = dataGridView1.Rows.Count;

            if (textBox5.Text != "")
            {
                if (textBox6.Text != "" || textBox7.Text != "" || textBox8.Text != "" || textBox9.Text != "" || textBox10.Text != "")
                {
                    DataAdd();
                }
                else
                    MessageBox.Show("Eksik veri girişi vardır !!");
            }
            else
                MessageBox.Show("Eksik veri girişi vardır !!");

            ShowData1("Select * from fikstur1");
        }



        private void button4_Click(object sender, EventArgs e)
        {

            int selectedIndex = dataGridView1.CurrentCell.RowIndex;
            if (selectedIndex > -1)
            {
                dataGridView1.Rows.RemoveAt(selectedIndex);
                dataGridView1.Refresh();
            }

            conn.Open();
            OleDbCommand com = new OleDbCommand("delete from fikstur1 where posNo1=@posNo1", conn);
            com.Parameters.AddWithValue("@posNo1", textBox5.Text);
            com.ExecuteNonQuery();
            conn.Close();


            ShowData1("Select * from fikstur1");
        }

        private void button9_Click(object sender, EventArgs e)
        {

            dataGridView2.Rows.Add();
            dataGridView2.Rows[0].HeaderCell.Value = dataGridView2.Rows.Count;
            ShowData1("Select * from fikstur1");
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            conn.Open();
            i++;
            // OleDbCommand com = new OleDbCommand("insert into fikstur1(x, y, wBas, wBit, wSure)values (@x, @y, @wBas, @wBit, @wSure)", conn);
            OleDbCommand com = new OleDbCommand("insert into fikstur1(posNo1, x)values (@posNo1, @x)", conn);

            //com.Parameters.AddWithValue("@posNo1", dataGridView1.Rows[0]);
            //com.Parameters.AddWithValue("@x", dataGridView1.Rows[1]);
            //com.Parameters.AddWithValue("@y", dataGridView1.Rows[2]);
            //com.Parameters.AddWithValue("@wBas", dataGridView1.Rows[3]);
            //com.Parameters.AddWithValue("@wBit", dataGridView1.Rows[4]);
            //com.Parameters.AddWithValue("@wSure", dataGridView1.Rows[5]);
            com.Parameters.AddWithValue("@posNo1", i);
            com.Parameters.AddWithValue("@x", textBox10.Text);
            com.Parameters.AddWithValue("@y", textBox9.Text);
            com.Parameters.AddWithValue("@wBas", textBox8.Text);
            com.Parameters.AddWithValue("@wBit", textBox7.Text);
            com.Parameters.AddWithValue("@wSure", textBox6.Text);

            com.ExecuteNonQuery();

            ShowData1("Select * from fikstur1");
            conn.Close();



        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //    textBox10.Text = dataGridView1.Rows[1].ToString();
            //    textBox9.Text = dataGridView1.Rows[2].ToString();
            //    textBox8.Text = dataGridView1.Rows[3].ToString();
            //    textBox7.Text = dataGridView1.Rows[4].ToString();
            //    textBox6.Text = dataGridView1.Rows[5].ToString();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {

            textBox5.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox6.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            textBox7.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox8.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox9.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox10.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            /*
            conn.Open();

            OleDbCommand com = new OleDbCommand("Update fikstur1 set x ='" + textBox10.Text + "', y ='" + textBox9.Text + "', wBas ='" + textBox8.Text + "', wBit ='" + textBox7.Text + "', wSure ='" + textBox6.Text + "'  ", conn);

            com.ExecuteNonQuery();
            conn.Close();
            ShowData1("Select * from fikstur1");*/

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

       
    }
}
