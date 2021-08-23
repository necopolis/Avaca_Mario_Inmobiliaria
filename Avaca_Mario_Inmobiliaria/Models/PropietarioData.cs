using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{
    public class PropietarioData
    {

        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\\Users\\MArio\\source\\repos\\Inmobiliaria-GitHub\\Avaca_Mario_Inmobiliaria\\Avaca_Mario_Inmobiliaria\\Base de Datos\\BDInmobiliariaAvaca.mdf";
        public PropietarioData()
        {

        }

        public int Alta(Propietario propietario)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Propietario (DNI, Nombre, Apellido, Telefono, Email)
                                VALUES (@DNI, @Nombre, @Apellido, @Telefono, @Email);
                                SELECT SCOPE_IDENTITY();";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@DNI", propietario.DNI);
                    comm.Parameters.AddWithValue("@Nombre", propietario.Nombre);
                    comm.Parameters.AddWithValue("@Apellido", propietario.Apellido);
                    comm.Parameters.AddWithValue("@Telefono", propietario.Telefono);
                    comm.Parameters.AddWithValue("@Email", propietario.Email);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    propietario.Id = res;
                }
            }
            return res;
        }

        public int Modificar(int id, Propietario propietario)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Propietario 
                               SET 
                                DNI = @DNI, Nombre=@Nombre, Apellido=@Apellido, Telefono=@Telefono, Email=@Email
                              WHERE
                                 Id = @Id";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@DNI", propietario.DNI);
                    comm.Parameters.AddWithValue("@Nombre", propietario.Nombre);
                    comm.Parameters.AddWithValue("@Apellido", propietario.Apellido);
                    comm.Parameters.AddWithValue("@Telefono", propietario.Telefono);
                    comm.Parameters.AddWithValue("@Email", propietario.Email);
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteNonQuery());
                    conn.Close();
                }
            }
            return res;
        }

        public Propietario ObtenerPorId(int id)
        {
            Propietario i = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, DNI, Nombre, Apellido, Telefono, Email FROM Propietario 
                                WHERE Id=@id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Propietario
                        {
                            Id = reader.GetInt32(0),
                            DNI = (string)reader[nameof(Propietario.DNI)],
                            Nombre = (string)reader[nameof(Propietario.Nombre)],
                            Apellido = (string)reader[nameof(Propietario.Apellido)],
                            Telefono = (string)reader[nameof(Propietario.Telefono)],
                            Email = (string)reader[nameof(Propietario.Email)]
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
                string sql = @"DELETE FROM Propietario WHERE Id = @Id ;";

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

        public IList<Propietario> ObtenerTodos()
        {
            IList<Propietario> res = new List<Propietario>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, DNI, Nombre, Apellido, Telefono, Email
                              FROM Propietario";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = new Propietario
                        {
                            Id = reader.GetInt32(0),
                            DNI = (string)reader[nameof(Propietario.DNI)],
                            Nombre = (string)reader[nameof(Propietario.Nombre)],
                            Apellido = (string)reader[nameof(Propietario.Apellido)],
                            Telefono = (string)reader[nameof(Propietario.Telefono)],
                            Email = (string)reader[nameof(Propietario.Email)]
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
