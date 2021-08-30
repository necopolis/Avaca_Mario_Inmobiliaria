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
                string sql = @"INSERT INTO Contrato (FechaInicio, FechaFin, IniquilinoId, GaranteId, InmuebleId)
                                VALUES (@FechaInicio, @FechaFin, @InquilinoId, @GaranteId, @InmuebleId);
                                SELECT SCOPE_IDENTITY();";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
                    comm.Parameters.AddWithValue("@FechaInicio", contrato.FechaFin);
                    comm.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
                    comm.Parameters.AddWithValue("@GaranteId", contrato.GaranteId);
                    comm.Parameters.AddWithValue("@InmuebleId", contrato.InmuebleId);
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

            IList<Contrato> res = new List<Contrato>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT c.FechaInicio, c.FechaFin, m.Precio,
                    p.Apellido, i.Nombre, i.Apellido, g.Apellido, m.Id ,m.Direccion
                    FROM Contrato c INNER JOIN Inquilino i ON c.InquilinoId = c.Id
                    INNER JOIN Inmueble m ON c.InmuebleId = m.Id
                    INNER JOIN Propietario p ON m.PropietarioId = p.Id
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
                            FechaInicio = reader.GetDateTime(1),
                            FechaFin = reader.GetDateTime(2),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
                            },
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(7),
                                Direccion = reader.GetString(2),
                                Duenio = new Propietario {
                                Nombre = reader.GetString(7),
                                }
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
                string sql = @"DELETE FROM Inmueble WHERE Id = @Id";
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
        public int Modificacion(Inmueble inmueble)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Inmueble SET
                    Direccion=@Direccion, Uso=@Uso, Tipo=@Tipo, CantAmbiente=@CantAmbiente, Precio=@Precio,Activo=@Activo, PropietarioId=@PropietarioId " +
                    "WHERE Id = @Id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
                    comm.Parameters.AddWithValue("@Uso", inmueble.Uso);
                    comm.Parameters.AddWithValue("@Tipo", inmueble.Tipo);
                    comm.Parameters.AddWithValue("@CantAmbiente", inmueble.CantAmbiente);
                    comm.Parameters.AddWithValue("@Precio", inmueble.Precio);
                    comm.Parameters.AddWithValue("@Activo", inmueble.Activo);
                    comm.Parameters.AddWithValue("@PropietarioId", inmueble.PropietarioId);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public Inmueble ObtenerPorId(int id)
        {
            Inmueble inmueble = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Direccion, Uso, Tipo, CantAmbiente, Precio,Activo, PropietarioId , p.Nombre, p.Apellido
                     FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id
                    WHERE Id=@Id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = (string)reader[nameof(Inmueble.Direccion)],
                            Uso = (string)reader[nameof(Inmueble.Uso)],
                            Tipo = (string)reader[nameof(Inmueble.Tipo)],
                            CantAmbiente = (int)reader[nameof(Inmueble.CantAmbiente)],
                            Precio = (decimal)reader[nameof(Inmueble.Precio)],
                            Activo = (bool)reader[nameof(Inmueble.Activo)],
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8),
                            }
                        };
                    }
                    conn.Close();
                }
            }
            return inmueble;
        }


    }
}
