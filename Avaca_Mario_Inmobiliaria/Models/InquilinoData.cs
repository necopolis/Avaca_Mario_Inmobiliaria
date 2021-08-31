using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{

    public class InquilinoData : RepositorioBase
    {
        //string connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\\Users\\MArio\\source\\repos\\Inmobiliaria-GitHub\\Avaca_Mario_Inmobiliaria\\Avaca_Mario_Inmobiliaria\\Base de Datos\\BDInmobiliariaAvaca.mdf";
        public InquilinoData(IConfiguration configuration): base (configuration)
        {

        }

        public int Alta(Inquilino inquilino)
        {
            int res = -1;
            using (SqlConnection conn= new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Inquilino (DNI, Nombre, Apellido, Telefono, Email, Activo)
                                VALUES (@DNI, @Nombre, @Apellido, @Telefono, @Email, @Activo);
                                SELECT SCOPE_IDENTITY();";

                using (SqlCommand comm  = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@DNI", inquilino.DNI);
                    comm.Parameters.AddWithValue("@Nombre", inquilino.Nombre);
                    comm.Parameters.AddWithValue("@Apellido", inquilino.Apellido);
                    comm.Parameters.AddWithValue("@Telefono", inquilino.Telefono);
                    comm.Parameters.AddWithValue("@Email", inquilino.Email);
                    comm.Parameters.AddWithValue("@Activo", inquilino.Activo);
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
                                DNI = @DNI, Nombre=@Nombre, Apellido=@Apellido, Telefono=@Telefono, Email=@Email, Activo=@Activo
                              WHERE
                                 Id = @Id";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@DNI", inquilino.DNI);
                    comm.Parameters.AddWithValue("@Nombre", inquilino.Nombre);
                    comm.Parameters.AddWithValue("@Apellido", inquilino.Apellido);
                    comm.Parameters.AddWithValue("@Telefono", inquilino.Telefono);
                    comm.Parameters.AddWithValue("@Email", inquilino.Email);
                    comm.Parameters.AddWithValue("@Activo", inquilino.Activo);
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
                string sql = @"SELECT Id, DNI, Nombre, Apellido, Telefono, Email, Activo FROM Inquilino 
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
                            Email = (string)reader[nameof(Inquilino.Email)],
                            Activo = (bool)reader[nameof(Inquilino.Activo)]
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
                string sql = @"SELECT Id, DNI, Nombre, Apellido, Telefono, Email, Activo
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
                            Email = (string)reader[nameof(Inquilino.Email)],
                            Activo = (bool)reader[nameof(Inquilino.Activo)]
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
