using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{
    public class ContratoData : RepositorioBase
    {
        public ContratoData(IConfiguration configuration): base (configuration)
        {
                
        }
        public int Alta(Contrato contrato)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Contrato (FechaInicio, FechaFin, InquilinoId, GaranteId, InmuebleId, Activo)
                                VALUES (@FechaInicio, @FechaFin, @InquilinoId, @GaranteId, @InmuebleId, @Activo);
                                SELECT SCOPE_IDENTITY();";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
                    comm.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
                    comm.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
                    comm.Parameters.AddWithValue("@GaranteId", contrato.GaranteId);
                    comm.Parameters.AddWithValue("@InmuebleId", contrato.InmuebleId);
                    comm.Parameters.AddWithValue("@Activo", 1);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    contrato.Id = res;
                }
            }
            return res;

        }
        public IList<Contrato> ObtenerTodos()
        {
            /***
             * ANOTACION AL PARECER NO ESTARIA FUNCIONANDO (casteo)reader[nameof(Clase.atributo)]
             * Tuve que cambiar a reader.GetTipo(N°orden)
             **/
            IList<Contrato> res = new List<Contrato>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT c.Id, c.FechaInicio, c.FechaFin, c.InmuebleId, c.InquilinoId, c.GaranteId,
                                i.Id, i.DNI, i.Apellido,
                                g.Id, g.DNI, g.Apellido,
                                m.Id ,m.Direccion, m.Precio, m.Uso, c.Activo
                                FROM Contrato c INNER JOIN Inquilino i ON c.InquilinoId = i.Id
                                INNER JOIN Inmueble m ON c.InmuebleId = m.Id
                                INNER JOIN Garante g ON c.GaranteId = g.Id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        Contrato contrato = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaInicio = (DateTime)reader[nameof(Contrato.FechaInicio)],
                            FechaFin = (DateTime)reader[nameof(Contrato.FechaFin)],
                            InmuebleId = reader.GetInt32(3),
                            InquilinoId = reader.GetInt32(4),
                            GaranteId = reader.GetInt32(5),
                            Activo = reader.GetBoolean(16),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(6),
                                DNI = reader.GetString(7),
                                Apellido = reader.GetString(8),
                            },
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(12),
                                Direccion = reader.GetString(13),
                                Precio=reader.GetDecimal(14),
                                Uso=reader.GetString(15)
                            },
                            
                            Garante= new Garante
                            {
                                Id= reader.GetInt32(9),
                                DNI = reader.GetString(10),
                                Apellido = reader.GetString(11),
                            }
                        };
                        res.Add(contrato);
                    }
                    conn.Close();
                }
            }
            return res;
        }


        public int Baja(int id)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Contrato SET
                             Activo=0
                             WHERE Id = @Id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int Modificacion(Contrato contrato)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Contrato SET
                    FechaInicio=@FechaInicio, FechaFin=@FechaFin, InquilinoId=@InquilinoId, GaranteId=@GaranteId, InmuebleId=@InmuebleId
                    WHERE Id = @Id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
                    comm.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
                    comm.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
                    comm.Parameters.AddWithValue("@GaranteId", contrato.GaranteId);
                    comm.Parameters.AddWithValue("@InmuebleId", contrato.InmuebleId);
                    comm.Parameters.AddWithValue("@Id", contrato.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public Contrato ObtenerPorId(int id)
        {
            Contrato contrato = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT c.Id, c.FechaInicio, c.FechaFin, c.InmuebleId, c.InquilinoId, c.GaranteId,
                                i.DNI, i.Apellido,
                                g.DNI, g.Apellido,
                                m.Direccion, m.Precio, m.Uso, c.Activo
                                FROM Contrato c INNER JOIN Inquilino i ON c.InquilinoId = i.Id
                                INNER JOIN Inmueble m ON c.InmuebleId = m.Id
                                INNER JOIN Garante g ON c.GaranteId = g.Id
                                WHERE c.Id=@Id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        contrato = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaInicio = reader.GetDateTime(1),
                            FechaFin = reader.GetDateTime(2),
                            InmuebleId = reader.GetInt32(3),
                            InquilinoId = reader.GetInt32(4),
                            GaranteId = reader.GetInt32(5),
                            Activo = reader.GetBoolean(13),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(4),
                                DNI = reader.GetString(6),
                                Apellido = reader.GetString(7),
                            },
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(3),
                                Direccion = reader.GetString(10),
                                Precio = reader.GetDecimal(11),
                                Uso = reader.GetString(12),
                            },

                            Garante = new Garante
                            {
                                Id = reader.GetInt32(5),
                                DNI = reader.GetString(8) ,
                                Apellido = reader.GetString(9),
                            }
                        };
                    }
                    conn.Close();
                }
            }
            return contrato;
        }

        public IList<Contrato> AllByInquilino(int id)
        {
            IList<Contrato> lista = new List<Contrato>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT 
                                c.Id, c.InmuebleId, c.InquilinoId, c.FechaInicio, c.FechaFin, c.GaranteId, 
                                i.Id, i.Direccion, i.Precio, 
                                i2.Nombre, i2.Apellido,
                                g.Nombre, g.Apellido,
                                p.Id, p.Nombre, p.Apellido
                                FROM Contrato c INNER JOIN Inmueble i ON c.InmuebleId = i.Id
                                INNER JOIN Propietario p ON i.PropietarioId = p.Id
                                INNER JOIN Garante g ON c.GaranteId = g.Id
                                INNER JOIN Inquilino i2 ON c.InquilinoId = i2.Id WHERE i2.Id = @id ";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();

                    while (reader.Read())
                    {
                        Propietario p = new Propietario
                        {
                            Id = reader.GetInt32(13),
                            Nombre = reader.GetString(14),
                            Apellido = reader.GetString(15),
                        };

                        Inmueble i = new Inmueble
                        {
                            Id = reader.GetInt32(6),
                            Direccion = reader.GetString(7),
                            Precio = reader.GetDecimal(8),
                            PropietarioId= reader.GetInt32(13),
                            Duenio = p
                        };

                        Inquilino i2 = new Inquilino
                        {
                            Id = reader.GetInt32(2),
                            Nombre = reader.GetString(9),
                            Apellido = reader.GetString(10)
                        };
                        Garante g = new Garante
                        {
                            Nombre = reader.GetString(11),
                            Apellido = reader.GetString(12)
                        };

                        Contrato c = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            InmuebleId = reader.GetInt32(1),
                            InquilinoId = reader.GetInt32(2),
                            FechaInicio = reader.GetDateTime(3),
                            FechaFin = reader.GetDateTime(4),
                            Inmueble = i,
                            Inquilino = i2,
                            Garante = g
                        };
                        

                        lista.Add(c);
                    }

                    conn.Close();
                }

            }

            return lista;
        }

        public Boolean Check(int id) {
            Boolean res= false;
            using (SqlConnection conn = new SqlConnection(connectionString)) 
            {
                //
                string sql = @"SELECT * FROM Contrato c WHERE c.Id=@id";
                using (SqlCommand comm= new SqlCommand(sql, conn)) 
                {
                    comm.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();

                    res = (reader.HasRows) ? true : false;
                    //Revisar si el contrato esta vigente y cambiar res
                    //Traer todos 
                }
            }
                return res;
        }

        public IList<Contrato> AllByInquilino(int id, bool valid)
        {
            /***
             * ANOTACION AL PARECER NO ESTARIA FUNCIONANDO (casteo)reader[nameof(Clase.atributo)]
             * Tuve que cambiar a reader.GetTipo(N°orden)
             **/
            IList<Contrato> res = new List<Contrato>();
            string sql;


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (!valid)
                {
                    sql = @"SELECT c.Id, c.FechaInicio, c.FechaFin, c.InmuebleId, c.InquilinoId, c.GaranteId,
                                i.Id, i.DNI, i.Apellido,
                                g.Id, g.DNI, g.Apellido,
                                m.Id ,m.Direccion, m.Precio, m.Uso, c.Activo
                                FROM Contrato c INNER JOIN Inquilino i ON c.InquilinoId = i.Id
                                INNER JOIN Inmueble m ON c.InmuebleId = m.Id
                                INNER JOIN Garante g ON c.GaranteId = g.Id
                                WHERE i2.Id=@id";
                }
                else
                {
                    sql = @"SELECT c.Id, c.IdInmueble, c.IdInquilino, c.Desde, c.Hasta, c.NombreGarante, c.Valido,
                            i.IdPropietario, i.Direccion, p.Nombre,
                            i2.Nombre
                            FROM Contrato c
                            INNER JOIN Inmueble i ON c.IdInmueble = i.Id
                            INNER JOIN Propietario p ON i.IdPropietario = p.Id
                            INNER JOIN Inquilino i2 ON c.IdInquilino = i2.Id
                            WHERE c.Valido = 1
                            AND current_date() BETWEEN c.Desde AND c.Hasta
                            AND i2.Id = @id;";
                }

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        Contrato contrato = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaInicio = (DateTime)reader[nameof(Contrato.FechaInicio)],
                            FechaFin = (DateTime)reader[nameof(Contrato.FechaFin)],
                            InmuebleId = reader.GetInt32(3),
                            InquilinoId = reader.GetInt32(4),
                            GaranteId = reader.GetInt32(5),
                            Activo = reader.GetBoolean(16),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(6),
                                DNI = reader.GetString(7),
                                Apellido = reader.GetString(8),
                            },
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(12),
                                Direccion = reader.GetString(13),
                                Precio = reader.GetDecimal(14),
                                Uso = reader.GetString(15)
                            },

                            Garante = new Garante
                            {
                                Id = reader.GetInt32(9),
                                DNI = reader.GetString(10),
                                Apellido = reader.GetString(11),
                            }
                        };
                        res.Add(contrato);
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public bool fechasCorrectas(int id, DateTime desde, DateTime hasta) 
        {
            bool res = false;

            using (SqlConnection conn = new SqlConnection(connectionString)) 
            {
                string sql = @"SELECT c.Id FROM Contrato c
                                WHERE (c.FechaInicio BETWEEN @desde AND @hasta
                                OR c.FechaFin BETWEEN @desde AND @hasta)
                                AND c.Activo =1 AND c.InmuebleId = @id";
                using (SqlCommand comm=new SqlCommand(sql, conn)) 
                {
                    comm.Parameters.AddWithValue("@desde", desde);
                    comm.Parameters.AddWithValue("@hasta", hasta);
                    comm.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    res = reader.Read();
                }
            }
            return res;
        }

    }
}
