using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace conexionPostgresql
{
    public partial class Form1 : Form
    {
        /*private string servidor2 = "localhost";
        private string bd = "Curso";
        private string usuario = "postgres";
        private string password = "kchy1234";
        private string puerto = "5432";*/

        private string connstring = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};","localhost",5432,"postgres","kchy1234","Curso");
        
        private NpgsqlConnection conn;
        private string sql;
        private NpgsqlCommand cmd;
        private DataTable dt;
        private int rowIndex = -1;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConexion_Click(object sender, EventArgs e)
        {
            Clases.Cconexion objetoConexion=new Clases.Cconexion();
            objetoConexion.establecerConexion();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
            Select();
        }

        private void Select()
        {
               
            try
            {
                conn.Open();
                sql = @"select * from st_select()";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                dgvData.DataSource = null;
                dgvData.DataSource = dt;
                conn.Close();


            }
            catch (Exception ex)
            {

                conn.Close();
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                rowIndex = e.RowIndex;
                txtNombre.Text = dgvData.Rows[e.RowIndex].Cells["firstname"].Value.ToString();
                txtNombre2.Text = dgvData.Rows[e.RowIndex].Cells["midname"].Value.ToString();
                txtApellido.Text = dgvData.Rows[e.RowIndex].Cells["lastname"].Value.ToString();
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            rowIndex = -1;
            txtNombre.Enabled=txtNombre2.Enabled=txtApellido.Enabled=true;
            txtNombre.Text = txtNombre2.Text = txtApellido.Text = null;
            txtNombre.Select();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (rowIndex<0)
            {
                MessageBox.Show("elije algo");
                return;
            }
            txtNombre.Enabled = txtNombre2.Enabled = txtApellido.Enabled = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (rowIndex < 0)
            {
                MessageBox.Show("elije algo");
                return;
            }

            try
            {
                conn.Open();
                sql = @"select * from st_delete(:_id)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id",int.Parse(dgvData.Rows[rowIndex].Cells["id"].ToString()));
                if ((int)cmd.ExecuteScalar()==1)
                {
                    MessageBox.Show("se elimino");
                    rowIndex= -1;
                    Select();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("no se eliminó");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int result = 0;
            if (rowIndex < 0)
            {
                try
                {
                    conn.Open();
                    sql = @"select * from st_insert(:_firstname,:_midname,:_lastname)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_firstname", txtNombre.Text);
                    cmd.Parameters.AddWithValue("_midname", txtNombre2.Text);
                    cmd.Parameters.AddWithValue("_lastname", txtApellido.Text);
                    result = (int)cmd.ExecuteScalar();
                    conn.Close();
                    if (result == 1)
                    {
                        MessageBox.Show("insertar si vale");
                        Select();
                    }
                    else
                    {
                        MessageBox.Show("insertar no vale");
                    }

                }
                catch (Exception ex)
                {
                    conn.Close();
                    MessageBox.Show("fallo insertar");
                }
            }
            else
            {
                try
                {
                    conn.Open();
                    sql = @"select * from st_update(:_id,:_firstname,:_midname,:_lastname)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_id", int.Parse(dgvData.Rows[rowIndex].Cells["id"].Value.ToString()));
                    cmd.Parameters.AddWithValue("_firstname", txtNombre.Text);
                    cmd.Parameters.AddWithValue("_midname", txtNombre2.Text);
                    cmd.Parameters.AddWithValue("_lastname", txtApellido.Text);
                    result = (int)cmd.ExecuteScalar();
                    conn.Close();
                    if (result == 1)
                    {
                        MessageBox.Show("update si vale");
                        Select();
                    }
                    else
                    {
                        MessageBox.Show("update no vale");
                    }

                }
                catch (Exception ex)
                {
                    conn.Close();
                    MessageBox.Show("fallo update");
                }
            }
                result = 0;
            txtNombre.Text = txtNombre2.Text = txtApellido.Text = null;
            txtNombre.Enabled = txtNombre2.Enabled = txtApellido.Enabled = false;
        }
    }
}
