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
                    comm.Parameters.AddWithValue("@Activo", 1);
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
                            Uso = reader.GetInt32(2),
                            Tipo = reader.GetInt32(3),
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

        /// <summary>
        /// Trae todos los inmuebles Activo=1 junto con su dueño
        /// </summary>
        /// <returns>
        /// Devuelve una lista de inmuebles inmuebles.count
        /// </returns>
        public IList<Inmueble> ObtenerTodosActivos()
        {

            IList<Inmueble> res = new List<Inmueble>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT i.Id, i.Direccion, i.Uso, i.Tipo, i.CantAmbiente, i.Precio, i.Activo, i.PropietarioId,
                    p.Nombre, p.Apellido
                    FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id
                    WHERE i.Activo=1";
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
                            Uso = reader.GetInt32(2),
                            Tipo = reader.GetInt32(3),
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
                        res.Add(inmueble);
                    }
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
                    sql = @"DELETE FROM Inmueble WHERE Id=@Id";
                }
                else
                {
                    sql = @"UPDATE Inmueble 
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
        public int Modificacion(Inmueble inmueble)
        {
            int res = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Inmueble SET
                    Direccion=@Direccion, Uso=@Uso, Tipo=@Tipo, CantAmbiente=@CantAmbiente, Precio=@Precio, PropietarioId=@PropietarioId, Activo=@Activo
                    WHERE Id = @Id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
                    comm.Parameters.AddWithValue("@Uso", inmueble.Uso);
                    comm.Parameters.AddWithValue("@Tipo", inmueble.Tipo);
                    comm.Parameters.AddWithValue("@CantAmbiente", inmueble.CantAmbiente);
                    comm.Parameters.AddWithValue("@Precio", inmueble.Precio);
                    comm.Parameters.AddWithValue("@PropietarioId", inmueble.PropietarioId);
                    comm.Parameters.AddWithValue("@Id", inmueble.Id);
                    comm.Parameters.AddWithValue("@Activo", inmueble.Activo);
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
                            Uso = reader.GetInt32(2),
                            Tipo = reader.GetInt32(3),
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

        internal bool NoTieneContrato(int id)
        {
            bool res = false;
            string sql = @"SELECT c.Id FROM contrato c WHERE c.InmuebleId = @Id
                        AND c.Activo = 1 AND getdate() BETWEEN c.FechaInicio AND c.FechaFin;
";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    res = (reader.HasRows) ? true : false;
                    conn.Close();
                }
            }
            return res;
        }

        internal bool TienePropietario(int id)
        {
            bool res = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT DISTINCT i.PropietarioId FROM Inmueble i
                                WHERE i.PropietarioId=@Id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    if (comm.ExecuteNonQuery() > 0)
                    {
                        res = true;
                    }
                }
            }
            return res;
        }

        internal IList<Inmueble> ListaInmPropietario(int id)
        {
            IList<Inmueble> res = new List<Inmueble>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT i.Id, i.Direccion, i.Uso, i.Tipo, i.CantAmbiente, i.Precio, i.Activo, i.PropietarioId,
                    p.Nombre, p.Apellido
                    FROM Inmueble i INNER JOIN Propietario p ON i.PropietarioId = p.Id
                    WHERE i.PropietarioId=@Id";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmueble inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Uso = reader.GetInt32(2),
                            Tipo = reader.GetInt32(3),
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
                        res.Add(inmueble);
                    }
                    conn.Close();
                }
            }
            return res;
        }
        /// <summary>
        /// Obtiene todos los validos entre las fechas
        /// </summary>
        /// <returns></returns>
        public IList<Inmueble> ObtenerTodosValidos()
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT i.Id, i.Direccion, i.Tipo, i.Uso, i.CantAmbiente, i.Precio,
				            i.Activo, i.PropietarioId, p.Nombre, p.DNI, p.Email  
                            FROM Inmueble i
                            INNER JOIN Propietario p ON i.PropietarioId = p.Id
                            WHERE i.Activo = 1
                            AND i.Id NOT IN (
	                            SELECT DISTINCT c.InmuebleId
	                            FROM Contrato c
	                            WHERE c.Activo = 1
	                            AND getdate() BETWEEN c.FechaInicio AND c.FechaFin
                            );";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Tipo = reader.GetInt32(2),
                            Uso = reader.GetInt32(3),
                            CantAmbiente = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Activo = reader.GetBoolean(6),
                            PropietarioId = reader.GetInt32(7),
                        };

                        var p = new Propietario
                        {
                            Id = reader.GetInt32(7),
                            Nombre = reader.GetString(8),
                            DNI = reader.GetString(9),
                            Email = reader.GetString(10),
                        };

                        i.Duenio = p;

                        res.Add(i);
                    }
                    conn.Close();
                }
            }
            return res;
        }
        public bool InmubleHabilitado(int id)
        {
            bool res = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT i.Id
                        FROM Inmueble i
                        WHERE i.Activo = 1
                        AND i.Id NOT IN (
	                        SELECT DISTINCT c.InmuebleId
	                        FROM Contrato c
	                        WHERE c.Activo = 1
	                        AND getdate() BETWEEN c.FechaInicio AND c.FechaFin
                        ) 
                        AND i.Id=@id;";
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    res = reader.Read()? true: false;
                    conn.Close();
                }
            }
            
            return res;
        }
        public bool isTaken(int id)
        {
            bool res = false;
            string sql = @"SELECT c.Id FROM contrato c WHERE c.InmuebleId = @id
                        AND c.Valido = 1 AND current_date() BETWEEN c.FechaInicio AND c.FechaFin;
";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    res = (reader.HasRows) ? true : false;
                    conn.Close();
                }
            }
            return res;
        }

        public IList<Inmueble> InmuebleAbilitadosPOrFecha(DateTime desde, DateTime hasta) 
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = @"SELECT i2.Id, i2.Direccion,i2.Tipo, i2.Uso,i2.CantAmbiente,i2.Precio, i2.Activo, i2.PropietarioId, p.Nombre, p.Apellido FROM propietario p INNER JOIN (
                                SELECT DISTINCT i.* FROM inmueble i INNER JOIN contrato c ON (i.Id = c.IdInmueble)
                                AND c.FechaIncio NOT BETWEEN @desde AND @hasta
                                AND c.FechaFin NOT BETWEEN @desde AND @hasta
                                AND i.Disponible = 1
                                UNION
                                SELECT i.* FROM inmueble i WHERE i.Id NOT IN (
	                                SELECT DISTINCT c.IdInmueble FROM contrato c 	
                                )
                                AND i.Activo = 1) i2
                                ON (i2.PropietarioId = p.Id);";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@desde", desde);
                    comm.Parameters.AddWithValue("@hasta", hasta);
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        var i = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Tipo = reader.GetInt32(2),
                            Uso = reader.GetInt32(3),
                            CantAmbiente = reader.GetInt32(4),
                            Precio = reader.GetDecimal(5),
                            Activo = reader.GetBoolean(6),
                            PropietarioId = reader.GetInt32(7),
                        };

                        var p = new Propietario
                        {
                            Id = reader.GetInt32(7),
                            Nombre = reader.GetString(8),
                            DNI = reader.GetString(9),
                            Email = reader.GetString(10),
                        };

                        i.Duenio = p;

                        res.Add(i);
                    }
                    conn.Close();
                }
            }
            return res;
        }
        /// <summary>
        /// Trae la lista de contratos asociado a un inmueble
        /// </summary>
        /// <param name="idInmueble"></param>
        /// <returns>
        /// Retorna una lista de contratos
        /// </returns>

        internal IList<Contrato> ContratoInmuebles(int idInmueble)
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
                                INNER JOIN Inquilino i2 ON c.InquilinoId = i2.Id 
                                WHERE i.Id = @Id ";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Id", idInmueble);
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
                            PropietarioId = reader.GetInt32(13),
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
    }
}
