
using MySql.Data.MySqlClient;

namespace Inmobiliaria.Models;

public class RepositorioUsuario
{
    public RepositorioUsuario()
    {
    }

    public List<Usuario> GetUsuarios(MySqlDatabase mySqlDatabase)
    {
        var usuarios = new List<Usuario>();
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdUsuario, Nombre, Apellido, Avatar, Email, Rol, Clave, Dni, Telefono 
                                FROM Usuario";

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var usuario = new Usuario
                    {
                        IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),
                        Nombre = reader.GetString(nameof(Usuario.Nombre)),
                        Apellido = reader.GetString(nameof(Usuario.Apellido)),
                        Clave = reader.GetString(nameof(Usuario.Clave)),
                        Avatar = reader.GetString(nameof(Usuario.Avatar)),
                        Email = reader.GetString(nameof(Usuario.Email)),
                        Rol = reader.GetInt32(nameof(Usuario.Rol)),
                        Dni = reader.GetString(nameof(Usuario.Dni)),
                        Telefono = reader.GetString(nameof(Usuario.Telefono))
                    };
                    usuarios.Add(usuario);
                }

            }
        }
        return usuarios;
    }

    public Usuario GetUsuario(MySqlDatabase mySqlDatabase, int id)
    {
        Usuario? usuario = null;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdUsuario, Nombre, Apellido, Clave, Avatar, Email, Rol, Dni, Telefono
                                FROM Usuario WHERE IdUsuario = @id";
            cmd.Parameters.AddWithValue("@id", id);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),
                        Nombre = reader.GetString(nameof(Usuario.Nombre)),
                        Apellido = reader.GetString(nameof(Usuario.Apellido)),
                        Clave = reader.GetString(nameof(Usuario.Clave)),
                        Avatar = reader.GetString(nameof(Usuario.Avatar)),
                        Email = reader.GetString(nameof(Usuario.Email)),
                        Rol = reader.GetInt32(nameof(Usuario.Rol)),
                        Dni = reader.GetString(nameof(Usuario.Dni)),
                        Telefono = reader.GetString(nameof(Usuario.Telefono))
                    };
                }
            }
        }
        return usuario;
    }

    public int CreateUsuario(MySqlDatabase mySqlDatabase, Usuario usuario)
    {
        int res = -1;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"INSERT INTO Usuario (Nombre, Apellido, Clave, Avatar, Email, Rol, Dni, Telefono) 
                                VALUES (@Nombre, @Apellido, @Clave, @Avatar, @Email, @Rol, @Dni, @Telefono);
                                SELECT LAST_INSERT_ID();";
            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            cmd.Parameters.AddWithValue("@Clave", usuario.Clave);
            cmd.Parameters.AddWithValue("@Avatar", usuario.Avatar);
            cmd.Parameters.AddWithValue("@Email", usuario.Email);
            cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
            cmd.Parameters.AddWithValue("@Dni", usuario.Dni);
            cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);

            res = Convert.ToInt32(cmd.ExecuteScalar());
            usuario.IdUsuario = res;
        }
        return res;
    }

    public int UpdateUsuario(MySqlDatabase mySqlDatabase, Usuario usuario)
    {
        int res = -1;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"UPDATE Usuario SET Nombre=@Nombre, Apellido=@Apellido, Clave=@Clave, Avatar=@Avatar, Email=@Email, Rol=@Rol, Dni=@Dni, Telefono=@Telefono 
                                WHERE IdUsuario = @IdUsuario";
            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            cmd.Parameters.AddWithValue("@Clave", usuario.Clave);
            cmd.Parameters.AddWithValue("@Avatar", usuario.Avatar);
            cmd.Parameters.AddWithValue("@Email", usuario.Email);
            cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
            cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
            cmd.Parameters.AddWithValue("@Dni", usuario.Dni);
            cmd.Parameters.AddWithValue("@Telefono", usuario.Telefono);

            res = Convert.ToInt32(cmd.ExecuteNonQuery());
        }
        return res;
    }

    public int DeleteUsuario(MySqlDatabase mySqlDatabase, int id)
    {
        int res = -1;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"DELETE FROM Usuario WHERE IdUsuario = @id";
            cmd.Parameters.AddWithValue("@id", id);
            
            res = Convert.ToInt32(cmd.ExecuteNonQuery());
        }
        return res;
    }

    public Usuario ObtenerPorEmail(MySqlDatabase mySqlDatabase, string email){
        Usuario? usuario = null;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdUsuario, Nombre, Apellido, Clave, Avatar, Email, Rol, Dni, Telefono 
                                FROM Usuario WHERE Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),
                        Nombre = reader.GetString(nameof(Usuario.Nombre)),
                        Apellido = reader.GetString(nameof(Usuario.Apellido)),
                        Clave = reader.GetString(nameof(Usuario.Clave)),
                        Avatar = reader.GetString(nameof(Usuario.Avatar)),
                        Email = reader.GetString(nameof(Usuario.Email)),
                        Rol = reader.GetInt32(nameof(Usuario.Rol)),
                        Dni = reader.GetString(nameof(Usuario.Dni)),
                        Telefono = reader.GetString(nameof(Usuario.Telefono))
                    };
                }
            }
        }
        return usuario;
    }
    
}
