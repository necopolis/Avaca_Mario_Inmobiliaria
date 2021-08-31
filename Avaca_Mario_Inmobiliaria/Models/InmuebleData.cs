using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{
    public class InmuebleData : RepositorioBase
    {

        public InmuebleData(IConfiguration configuration): base (configuration)
        {

        }

        public int Alta(Inmueble inmueble) {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Inmueble (Direccion, Uso, Tipo, CantAmbiente, Precio, Activo, PropietarioId)
                                VALUES (@Direccion, @Uso, @Tipo, @CantAmbiente, @Precio, @Activo, @PropietarioId);
                                SELECT SCOPE_IDENTITY();";
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
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    inmueble.Id = res;
                }
            }
            return res;
        
        }
        public IList<Inmueble> ObtenerTodos() {
            
            IList<Inmueble> res = new List<Inmueble>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT i.Id, i.Direccion, i.Uso, i.Tipo, i.CantAmbiente, i.Precio, i.Activo, i.PropietarioId,
                    p.Nombre, p.Apellido
                    FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmueble inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Uso = reader.GetString(2),
                            Tipo = reader.GetString(3),
                            CantAmbiente = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Activo = reader.GetBoolean(6),
                            PropietarioId=reader.GetInt32(7),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
                            }
                        };
                        res.Add(inmueble);
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
                string sql =  @"UPDATE Inmueble SET
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
                string sql = @"SELECT i.Id, i.Direccion, i.Uso, i.Tipo, i.CantAmbiente, i.Precio, i.Activo, i.PropietarioId , p.Nombre, p.Apellido
                     FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id
                    WHERE i.Id=@Id";
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
                            Direccion = reader.GetString(1),
                            Uso = reader.GetString(2),
                            Tipo = reader.GetString(3),
                            CantAmbiente = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Activo = reader.GetBoolean(6),
                            PropietarioId = reader.GetInt32(7),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(7),
                                Nombre = reader.GetString(8),
                                Apellido = reader.GetString(9),
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
