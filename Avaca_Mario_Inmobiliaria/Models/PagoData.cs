using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{
    public class PagoData : RepositorioBase
    {
        //string connectionString = "Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\\Users\\MArio\\source\\repos\\Inmobiliaria-GitHub\\Avaca_Mario_Inmobiliaria\\Avaca_Mario_Inmobiliaria\\Base de Datos\\BDInmobiliariaAvaca.mdf";
        public PagoData(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Pago pago)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Pago (NumeroPago, Importe, ContratoId)
                                VALUES (@NumeroPago, @Importe, @ContratoId);
                                SELECT SCOPE_IDENTITY();";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@NumeroPago", pago.NumeroPago);
                    comm.Parameters.AddWithValue("@Importe", pago.Importe);
                    comm.Parameters.AddWithValue("@ContratoId", pago.ContratoId);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    pago.Id = res;
                }
            }
            return res;

        }

        public int Put(Pago pago)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Pago (NumeroPago, Importe, ContratoId)
                                VALUES (@NumeroPago, @Importe, @ContratoId);
                                SELECT SCOPE_IDENTITY();";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@NumeroPago", pago.NumeroPago);
                    comm.Parameters.AddWithValue("@Importe", pago.Importe);
                    comm.Parameters.AddWithValue("@ContratoId", pago.ContratoId);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    pago.Id = res;
                }
            }
            return res;

        }
        public IList<Pago> ObtenerTodos()
        {

            IList<Pago> res = new List<Pago>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT p.Id, p.NumeroPago, p.FechaPago, p.Importe, p.ContratoId,
                    c.FechaInicio, c.FechaFin,
                    m.Id,m.Direccion, m.Precio,
                    inq.Id, inq.Nombre, inq.Apellido
                    FROM Pago p INNER JOIN Contrato c ON p.ContratoId = c.Id
                    INNER JOIN Inmueble m ON c.InmuebleId = m.Id
                    INNER JOIN Inquilino inq ON c.InquilinoId = inq.Id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago pago = new Pago
                        {
                            Id = reader.GetInt32(0),
                            NumeroPago = reader.GetInt32(1),
                            FechaPago = reader.GetDateTime(2),
                            Importe = reader.GetDecimal(3),
                            ContratoId = reader.GetInt32(4),
                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32(4),
                                FechaInicio = reader.GetDateTime(5),
                                FechaFin = reader.GetDateTime(6),
                                Inquilino = new Inquilino {
                                     Id= reader.GetInt32(10),
                                     Nombre= reader.GetString(11),
                                     Apellido= reader.GetString(12),

                                },
                                Inmueble = new Inmueble {
                                    Id = reader.GetInt32(7),
                                    Direccion= reader.GetString(8),
                                    Precio= reader.GetDecimal(9)
                                }
                            }
                        };
                        res.Add(pago);
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
                string sql = $"DELETE FROM Pago WHERE IdPago = @id";
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
        public int Cancelar(int id)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM Pago WHERE IdPago = @id";
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
        public int Modificacion(Pago pago)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Pago SET
                    NumeroPago=@NumeroPago, FechaPago=@FechaPago, Importe=@Importe, CantAmbiente=@CantAmbiente, Precio=@Precio, ContratoId=@ContratoId " +
                    "WHERE Id = @Id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@NumeroPago", pago.NumeroPago);
                    comm.Parameters.AddWithValue("@FechaPago", pago.FechaPago);
                    comm.Parameters.AddWithValue("@Importe", pago.Importe);
                    comm.Parameters.AddWithValue("@ContratoId", pago.ContratoId);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public Pago ObtenerPorId(int id)
        {
            Pago pago = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT p.Id, p.NumeroPago, p.FechaPago, p.Importe, p.ContratoId,
                    c.FechaInicio, c.FechaFin,
                    m.Id,m.Direccion, m.Precio,
                    inq.Id, inq.Nombre, inq.Apellido
                    FROM Pago p INNER JOIN Contrato c ON p.ContratoId = c.Id
                    INNER JOIN Inmueble m ON c.InmuebleId = m.Id
                    INNER JOIN Inquilino inq ON c.InquilinoId = inq.Id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        pago = new Pago
                        {
                            Id = reader.GetInt32(0),
                            NumeroPago = reader.GetInt32(1),
                            FechaPago = reader.GetDateTime(2),
                            Importe = reader.GetDecimal(3),
                            ContratoId = reader.GetInt32(4),
                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32(4),
                                FechaInicio = reader.GetDateTime(5),
                                FechaFin = reader.GetDateTime(6),
                                Inquilino = new Inquilino
                                {
                                    Id = reader.GetInt32(10),
                                    Nombre = reader.GetString(11),
                                    Apellido = reader.GetString(12),

                                },
                                Inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32(7),
                                    Direccion = reader.GetString(8),
                                    Precio = reader.GetDecimal(9)
                                }
                            }
                        };
                    }
                    conn.Close();
                }
            }
            return pago;
        }
    }
}
