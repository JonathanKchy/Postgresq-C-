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
                txtNombre.Text = dgvData.Rows[e.RowIndex].Cells["firstname"].Value.ToString();
                txtNombre2.Text = dgvData.Rows[e.RowIndex].Cells["midname"].Value.ToString();
                txtApellido.Text = dgvData.Rows[e.RowIndex].Cells["lastname"].Value.ToString();
            }
        }
    }
}
