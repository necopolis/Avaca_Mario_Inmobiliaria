using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{

    public class InquilinoData
    {
        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\\Users\\MArio\\source\\repos\\Inmobiliaria-GitHub\\Avaca_Mario_Inmobiliaria\\Avaca_Mario_Inmobiliaria\\Base de Datos\\BDInmobiliariaAvaca.mdf";
        public InquilinoData()
        {

        }

        public int Alta(Inquilino inquilino)
        {
            int res = -1;
            using (SqlConnection conn= new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Inquilino (DNI, Nombre, Apellido, Telefono, Email)
                                VALUES (@DNI, @Nombre, @Apellido, @Telefono, @Email);
                                SELECT SCOPE_IDENTITY();";

                using (SqlCommand comm  = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@DNI", inquilino.DNI);
                    comm.Parameters.AddWithValue("@Nombre", inquilino.Nombre);
                    comm.Parameters.AddWithValue("@Apellido", inquilino.Apellido);
                    comm.Parameters.AddWithValue("@Telefono", inquilino.Telefono);
                    comm.Parameters.AddWithValue("@Email", inquilino.Email);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    inquilino.Id = res;
                }
            }
            return res;
        }

        public int Modificar(int id, Inquilino inquilino)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Inquilino 
                               SET 
                                DNI = @DNI, Nombre=@Nombre, Apellido=@Apellido, Telefono=@Telefono, Email=@Email
                              WHERE
                                 Id = @Id";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@DNI", inquilino.DNI);
                    comm.Parameters.AddWithValue("@Nombre", inquilino.Nombre);
                    comm.Parameters.AddWithValue("@Apellido", inquilino.Apellido);
                    comm.Parameters.AddWithValue("@Telefono", inquilino.Telefono);
                    comm.Parameters.AddWithValue("@Email", inquilino.Email);
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteNonQuery());
                    conn.Close();
                }
            }
            return res;
        }

        public Inquilino ObtenerPorId(int id)
        {
            Inquilino i = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, DNI, Nombre, Apellido, Telefono, Email FROM Inquilino 
                                WHERE Id=@id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Inquilino
                        {
                            Id = reader.GetInt32(0),
                            DNI = (string)reader[nameof(Inquilino.DNI)],
                            Nombre = (string)reader[nameof(Inquilino.Nombre)],
                            Apellido = (string)reader[nameof(Inquilino.Apellido)],
                            Telefono = (string)reader[nameof(Inquilino.Telefono)],
                            Email = (string)reader[nameof(Inquilino.Email)]
                        };
                    }
                    conn.Close();
                }
            }
            return i;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"DELETE FROM Inquilino WHERE Id = @Id ;";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteNonQuery());
                    conn.Close();
                }
            }
            return res;
        }

        public IList<Inquilino> ObtenerTodos()
        {
            IList<Inquilino> res =new List<Inquilino>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, DNI, Nombre, Apellido, Telefono, Email
                              FROM Inquilino";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = new Inquilino
                        {
                            Id = reader.GetInt32(0),
                            DNI = (string)reader[nameof(Inquilino.DNI)],
                            Nombre = (string)reader[nameof(Inquilino.Nombre)],
                            Apellido = (string)reader[nameof(Inquilino.Apellido)],
                            Telefono = (string)reader[nameof(Inquilino.Telefono)],
                            Email = (string)reader[nameof(Inquilino.Email)]
                        };
                        res.Add(i);
                    }
                    conn.Close();
                }
            }
            return res;
        }

    }
}
