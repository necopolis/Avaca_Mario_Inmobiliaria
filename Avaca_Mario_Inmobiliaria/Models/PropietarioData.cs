using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{

    public class PropietarioData : RepositorioBase
    {
        //string connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\\Users\\MArio\\source\\repos\\Inmobiliaria-GitHub\\Avaca_Mario_Inmobiliaria\\Avaca_Mario_Inmobiliaria\\Base de Datos\\BDInmobiliariaAvaca.mdf";
        public PropietarioData(IConfiguration configuration) : base (configuration)
        {

        }

        public int Alta(Propietario propietario)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Propietario (DNI, Nombre, Apellido, Telefono, Email, Activo)
                                VALUES (@DNI, @Nombre, @Apellido, @Telefono, @Email, @Activo);
                                SELECT SCOPE_IDENTITY();";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@DNI", propietario.DNI);
                    comm.Parameters.AddWithValue("@Nombre", propietario.Nombre);
                    comm.Parameters.AddWithValue("@Apellido", propietario.Apellido);
                    comm.Parameters.AddWithValue("@Telefono", propietario.Telefono);
                    comm.Parameters.AddWithValue("@Email", propietario.Email);
                    comm.Parameters.AddWithValue("@Activo", 1);
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
                                DNI = @DNI, Nombre=@Nombre, Apellido=@Apellido, Telefono=@Telefono, Email=@Email, Activo=@Activo
                              WHERE
                                 Id = @Id";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@DNI", propietario.DNI);
                    comm.Parameters.AddWithValue("@Nombre", propietario.Nombre);
                    comm.Parameters.AddWithValue("@Apellido", propietario.Apellido);
                    comm.Parameters.AddWithValue("@Telefono", propietario.Telefono);
                    comm.Parameters.AddWithValue("@Email", propietario.Email);
                    comm.Parameters.AddWithValue("@Activo", propietario.Activo);
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
                string sql = @"SELECT Id, DNI, Nombre, Apellido, Telefono, Email, Activo FROM Propietario 
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
                            Email = (string)reader[nameof(Propietario.Email)],
                            Activo = (bool)reader[nameof(Propietario.Activo)],
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
                string sql = @"UPDATE Propietario 
                               SET 
                                Activo=0
                              WHERE
                                 Id = @Id";

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
        public int Baja(int id, bool admin)
        {
            int res = -1;
            string sql;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (admin)
                {
                    sql = @"DELETE FROM Propietario WHERE Id=@Id";
                }
                else
                {
                    sql = @"UPDATE Propietario 
                               SET 
                                 Activo=0
                              WHERE
                                 Id = @Id";
                }

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

        internal bool TienePropiedades(int id)
        {
            bool res = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT DISTINCT PropietarioId FROM Inmueble
                                WHERE PropietarioId=@Id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        res = true;
                    }
                }
            }
            return res;
        }

        public IList<Propietario> ObtenerTodos()
        {
            IList<Propietario> res = new List<Propietario>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, DNI, Nombre, Apellido, Telefono, Email, Activo
                              FROM Propietario";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        Propietario p = new Propietario
                        {
                            Id = reader.GetInt32(0),
                            DNI = (string)reader[nameof(Propietario.DNI)],
                            Nombre = (string)reader[nameof(Propietario.Nombre)],
                            Apellido = (string)reader[nameof(Propietario.Apellido)],
                            Telefono = (string)reader[nameof(Propietario.Telefono)],
                            Email = (string)reader[nameof(Propietario.Email)],
                            Activo = (bool)reader[nameof(Propietario.Activo)]
                        };
                        res.Add(p);
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public IList<Propietario> ObtenerTodosActivos()
        {
            IList<Propietario> res = new List<Propietario>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, DNI, Nombre, Apellido, Telefono, Email, Activo
                              FROM Propietario
                              WHERE Activo=1";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        Propietario p = new Propietario
                        {
                            Id = reader.GetInt32(0),
                            DNI = (string)reader[nameof(Propietario.DNI)],
                            Nombre = (string)reader[nameof(Propietario.Nombre)],
                            Apellido = (string)reader[nameof(Propietario.Apellido)],
                            Telefono = (string)reader[nameof(Propietario.Telefono)],
                            Email = (string)reader[nameof(Propietario.Email)],
                            Activo = (bool)reader[nameof(Propietario.Activo)]
                        };
                        res.Add(p);
                    }
                    conn.Close();
                }
            }
            return res;
        }
    }
}
