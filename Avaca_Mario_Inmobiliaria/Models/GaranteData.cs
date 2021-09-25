using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{
    public class GaranteData : RepositorioBase
    {
        public GaranteData(IConfiguration configuration): base(configuration)
        {

        }

        public int Alta(Garante garante)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Garante (DNI, Nombre, Apellido, Telefono, Email, LugarTrabajo, Sueldo, Activo)
                                VALUES (@DNI, @Nombre, @Apellido, @Telefono, @Email, @LugarTrabajo, @Sueldo, @Activo);
                                SELECT SCOPE_IDENTITY();";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@DNI", garante.DNI);
                    comm.Parameters.AddWithValue("@Nombre", garante.Nombre);
                    comm.Parameters.AddWithValue("@Apellido", garante.Apellido);
                    comm.Parameters.AddWithValue("@Telefono", garante.Telefono);
                    comm.Parameters.AddWithValue("@Email", garante.Email);
                    comm.Parameters.AddWithValue("@LugarTrabajo", garante.LugarTrabajo);
                    comm.Parameters.AddWithValue("@Sueldo", garante.Sueldo);
                    comm.Parameters.AddWithValue("@Activo", 1);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    garante.Id = res;
                }
            }
            return res;
        }

        public int Modificar(int id, Garante garante)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Garante 
                               SET 
                                DNI = @DNI, Nombre=@Nombre, Apellido=@Apellido, Telefono=@Telefono, Email=@Email, 
                                LugarTrabajo=@LugarTrabajo, Sueldo=@Sueldo, Activo=@Activo
                                    WHERE
                                        Id = @Id";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@DNI", garante.DNI);
                    comm.Parameters.AddWithValue("@Nombre", garante.Nombre);
                    comm.Parameters.AddWithValue("@Apellido", garante.Apellido);
                    comm.Parameters.AddWithValue("@Telefono", garante.Telefono);
                    comm.Parameters.AddWithValue("@Email", garante.Email);
                    comm.Parameters.AddWithValue("@LugarTrabajo", garante.LugarTrabajo);
                    comm.Parameters.AddWithValue("@Sueldo", garante.Sueldo);
                    comm.Parameters.AddWithValue("@Activo", garante.Activo);
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteNonQuery());
                    conn.Close();
                }
            }
            return res;
        }

        public Garante ObtenerPorId(int id)
        {
            Garante i = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, DNI, Nombre, Apellido, Telefono, Email, LugarTrabajo, Sueldo, Activo FROM Garante 
                                WHERE Id=@id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Garante
                        {
                            Id = reader.GetInt32(0),
                            DNI = (string)reader[nameof(Garante.DNI)],
                            Nombre = (string)reader[nameof(Garante.Nombre)],
                            Apellido = (string)reader[nameof(Garante.Apellido)],
                            Telefono = (string)reader[nameof(Garante.Telefono)],
                            Email = (string)reader[nameof(Garante.Email)],
                            LugarTrabajo = (string)reader[nameof(Garante.LugarTrabajo)],
                            Sueldo = (decimal)reader[nameof(Garante.Sueldo)],
                            Activo = (bool)reader[nameof(Garante.Activo)]
                        };
                    }
                    conn.Close();
                }
            }
            return i;
        }

        public int Baja(int id, bool admin)
        {
            int res = -1;
            string sql;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (admin)
                {
                    sql = @"DELETE FROM Garante WHERE Id=@Id";
                }
                else
                {
                    sql = @"UPDATE Garante
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

        internal bool TieneContrato(int id)
        {
            bool res = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT DISTINCT GaranteId FROM Contrato
                                WHERE GaranteId=@Id";
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

        public IList<Garante> ObtenerTodos()
        {
            IList<Garante> res = new List<Garante>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, DNI, Nombre, Apellido, Telefono, Email, LugarTrabajo, Sueldo, Activo
                              FROM Garante";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = new Garante
                        {
                            Id = reader.GetInt32(0),
                            DNI = (string)reader[nameof(Garante.DNI)],
                            Nombre = (string)reader[nameof(Garante.Nombre)],
                            Apellido = (string)reader[nameof(Garante.Apellido)],
                            Telefono = (string)reader[nameof(Garante.Telefono)],
                            Email = (string)reader[nameof(Garante.Email)],
                            LugarTrabajo = (string)reader[nameof(Garante.LugarTrabajo)],
                            Sueldo = (decimal)reader[nameof(Garante.Sueldo)],
                            Activo = (bool)reader[nameof(Garante.Activo)]
                        };
                        res.Add(i);
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public IList<Garante> ObtenerTodosActivos()
        {
            IList<Garante> res = new List<Garante>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, DNI, Nombre, Apellido, Telefono, Email, LugarTrabajo, Sueldo, Activo
                              FROM Garante
                              WHERE Activo=1";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = new Garante
                        {
                            Id = reader.GetInt32(0),
                            DNI = (string)reader[nameof(Garante.DNI)],
                            Nombre = (string)reader[nameof(Garante.Nombre)],
                            Apellido = (string)reader[nameof(Garante.Apellido)],
                            Telefono = (string)reader[nameof(Garante.Telefono)],
                            Email = (string)reader[nameof(Garante.Email)],
                            LugarTrabajo = (string)reader[nameof(Garante.LugarTrabajo)],
                            Sueldo = (decimal)reader[nameof(Garante.Sueldo)],
                            Activo = (bool)reader[nameof(Garante.Activo)]
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
