using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace conexionPostgresql.Clases
{
    internal class Cconexion
    {
        NpgsqlConnection conex = new NpgsqlConnection();
        static string servidor="localhost";
        static string bd="Curso";
        static string usuario="postgres";
        static string password="kchy1234";
        static string puerto="5432";

        String cadenaConexion = "server=" + servidor + ";" + "port=" + puerto + ";" + "user id=" + usuario + ";" + "password=" + password + ";" + "database=" + bd + ";";

        public NpgsqlConnection establecerConexion()
        {
            

            try
            {
                conex.ConnectionString= cadenaConexion;
                conex.Open();
                MessageBox.Show("Se conecto a la BD");

            }
            catch (NpgsqlException e)
            {
                MessageBox.Show("No se pudo conectar a la base de datos");                
            }
            return conex;
        }
    }
}
