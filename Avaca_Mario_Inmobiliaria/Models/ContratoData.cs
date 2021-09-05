using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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


    }
}
