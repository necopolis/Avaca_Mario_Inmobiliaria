using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Avaca_Mario_Inmobiliaria.Models
{
    public class UsuarioData : RepositorioBase
    {
        public UsuarioData(IConfiguration configuration) : base(configuration)
        {

        }
		public int Alta(Usuario e)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Usuario (Nombre, Apellido, Avatar, Email, Clave, Rol) " +
					$"VALUES (@nombre, @apellido, @avatar, @email, @clave, @rol);" +
					"SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					if (String.IsNullOrEmpty(e.Avatar))
						command.Parameters.AddWithValue("@avatar", DBNull.Value);
					else
						command.Parameters.AddWithValue("@avatar", e.Avatar);
					command.Parameters.AddWithValue("@email", e.Email);
					command.Parameters.AddWithValue("@clave", e.Clave);
					command.Parameters.AddWithValue("@rol", e.Rol);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					e.Id = res;
					connection.Close();
				}
			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Usuario WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public int Modificacion(Usuario e)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"UPDATE Usuario SET Nombre=@nombre, Apellido=@apellido, Avatar=@avatar, Email=@email, Clave=@clave, Rol=@rol " +
					$"WHERE Id = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{

					command.Parameters.AddWithValue("@nombre", e.Nombre);
					command.Parameters.AddWithValue("@apellido", e.Apellido);
					command.Parameters.AddWithValue("@avatar", e.Avatar);
					command.Parameters.AddWithValue("@email", e.Email);
					command.Parameters.AddWithValue("@clave", e.Clave);
					command.Parameters.AddWithValue("@rol", e.Rol);
					command.Parameters.AddWithValue("@id", e.Id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public IList<Usuario> ObtenerTodos()
		{
			IList<Usuario> res = new List<Usuario>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Avatar, Email, Clave, Rol" +
					$" FROM Usuario";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Usuario e = new Usuario
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Avatar = reader["Avatar"].ToString(),
							Email = reader.GetString(4),
							Clave = reader.GetString(5),
							Rol = reader.GetInt32(6),
						};
						res.Add(e);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Usuario ObtenerPorId(int id)
		{
			Usuario e = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Avatar, Email, Clave, Rol FROM Usuario" +
					$" WHERE Id=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@id", id);

					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Usuario
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Avatar = reader["Avatar"].ToString(),
							Email = reader.GetString(4),
							Clave = reader.GetString(5),
							Rol = reader.GetInt32(6),
						};
					}
					connection.Close();
				}
			}
			return e;
		}

		public Usuario ObtenerPorEmail(string email)
		{
			Usuario e = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = @"SELECT Id, Nombre, Apellido, Avatar, Email, Clave, Rol FROM Usuario
							WHERE Email=@email";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{

					command.Parameters.AddWithValue("@email", email);
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						e = new Usuario
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Avatar = reader["Avatar"].ToString(),
							Email = reader.GetString(4),
							Clave = reader.GetString(5),
							Rol = reader.GetInt32(6),
						};
					}
					connection.Close();
				}
			}
			return e;
		}

		public int Update(Usuario u, bool editoRol, bool editAvatar) 
		{
			int res = -1;
			string sql;
			int flag = 0;
            if (!editoRol && !editAvatar)
            {
				sql = @"UPDATE Usuario SET Nombre=@Nombre, Apellido=@Apellido, Email=@Email
						WHERE Id=@Id;";
			}
            else if (editoRol && !editAvatar)
            {
				sql = @"UPDATE Usuario SET Nombre=@Nombre, Apellido=@Apellido, Email=@Email, Rol=@Rol
								WHERE Id=@Id;";
				flag = 1;
			}
            else if (!editoRol && editAvatar)
            {
				sql = @"UPDATE Usuario SET Nombre=@Nombre, Apellido=@Apellido, Email=@Email,
							Avatar = @Avatar WHERE Id=@Id;";
				flag = 2;
			}
            else
            {
				sql= @"UPDATE Usuario SET Nombre=@Nombre, Apellido=@Apellido, Email=@Email,
					Rol =@Rol, Avatar=@Avatar WHERE Id=@Id;";
				flag = 3;
			}

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm= new SqlCommand(sql, conn))
                {
                    switch (flag)
                    {
						case 0:
							{
								comm.Parameters.AddWithValue("@Id", u.Id);
								comm.Parameters.AddWithValue("@Nombre", u.Nombre);
								comm.Parameters.AddWithValue("@Apellido", u.Apellido);
								comm.Parameters.AddWithValue("@Email", u.Email);
							}
                            break;
						case 1:
							{
								comm.Parameters.AddWithValue("@Id", u.Id);
								comm.Parameters.AddWithValue("@Nombre", u.Nombre);
								comm.Parameters.AddWithValue("@Apellido", u.Apellido);
								comm.Parameters.AddWithValue("@Email", u.Email);
								comm.Parameters.AddWithValue("@Rol", u.Rol);

							}
							break;
						case 2:
							{
								comm.Parameters.AddWithValue("@Id", u.Id);
								comm.Parameters.AddWithValue("@Nombre", u.Nombre);
								comm.Parameters.AddWithValue("@Apellido", u.Apellido);
								comm.Parameters.AddWithValue("@Email", u.Email);
								comm.Parameters.AddWithValue("@Avatar", u.Avatar);
							}
							break;
						case 3:
							{
								comm.Parameters.AddWithValue("@Id", u.Id);
								comm.Parameters.AddWithValue("@Nombre", u.Nombre);
								comm.Parameters.AddWithValue("@Apellido", u.Apellido);
								comm.Parameters.AddWithValue("@Email", u.Email);
								comm.Parameters.AddWithValue("@Rol", u.Rol);
								comm.Parameters.AddWithValue("@Avatar", u.Avatar);
							}
							break;
					}
					conn.Open();
					res = comm.ExecuteNonQuery();
					conn.Close();
                }
            }
			return res;
		}

        internal int UpdatePass(int id, string passNuevaHashed)
        {
			int res = -1;
            using (SqlConnection conn= new SqlConnection(connectionString))
            {
				string sql =@"UPDATE Usuario SET Clave = @Clave WHERE Id=@Id";

                using (SqlCommand comm=new SqlCommand(sql, conn))
                {
					comm.Parameters.AddWithValue("@Id", id);
					comm.Parameters.AddWithValue("@Clave", passNuevaHashed);

					conn.Open();
					res = comm.ExecuteNonQuery();
					conn.Close();
                }
            }
            return res;
        }
    }
}
