
using MySql.Data.MySqlClient;
using System.Data;

namespace Inmobiliaria.Models;

public class RepositorioPropietario
{
    public RepositorioPropietario()
    {
    }

    public List<Propietario> GetPropietarios(MySqlDatabase mySqlDatabase)
    {
        var propietarios = new List<Propietario>();
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdPropietario, Nombre, Apellido, Direccion, Telefono, Dni, Email, Clave, Avatar FROM Propietario";

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var propietario = new Propietario
                    {
                        IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
                        Nombre = reader.GetString(nameof(Propietario.Nombre)),
                        Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        Direccion = reader.GetString(nameof(Propietario.Direccion)),
                        Telefono = reader.GetString(nameof(Propietario.Telefono)),
                        Dni = reader.GetString(nameof(Propietario.Dni)),
                        Email = reader.GetString(nameof(Propietario.Email)),
                        Clave = reader.GetString(nameof(Propietario.Clave)),
                        Avatar = reader.GetString(nameof(Propietario.Avatar))
                    };
                    propietarios.Add(propietario);
                }

            }
        }
        return propietarios;
    }

    public Propietario GetPropietario(MySqlDatabase mySqlDatabase, int id)
    {
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdPropietario, Nombre, Apellido, Direccion, Telefono, Dni, Email, Clave, Avatar 
                                FROM Propietario WHERE IdPropietario = @IdPropietario";
            cmd.Parameters.AddWithValue("@IdPropietario", id);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var propietario = new Propietario
                    {
                        IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
                        Nombre = reader.GetString(nameof(Propietario.Nombre)),
                        Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        Direccion = reader.GetString(nameof(Propietario.Direccion)),
                        Telefono = reader.GetString(nameof(Propietario.Telefono)),
                        Dni = reader.GetString(nameof(Propietario.Dni)),
                        Email = reader.GetString(nameof(Propietario.Email)),
                        Clave = reader.GetString(nameof(Propietario.Clave)),
                        Avatar = reader.GetString(nameof(Propietario.Avatar))
                    };
                    mySqlDatabase.Dispose();
                    return propietario;
                }
            }
        }
        return null;
    }

    public Propietario GetPropietarioPorEmail(MySqlDatabase mySqlDatabase, string email)
    {
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdPropietario, Nombre, Apellido, Direccion, Telefono, Dni, Email, Clave, Avatar 
                                FROM Propietario WHERE Email = @Email";
            cmd.Parameters.AddWithValue("@Email", email);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var propietario = new Propietario
                    {
                        IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
                        Nombre = reader.GetString(nameof(Propietario.Nombre)),
                        Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        Direccion = reader.GetString(nameof(Propietario.Direccion)),
                        Telefono = reader.GetString(nameof(Propietario.Telefono)),
                        Dni = reader.GetString(nameof(Propietario.Dni)),
                        Email = reader.GetString(nameof(Propietario.Email)),
                        Clave = reader.GetString(nameof(Propietario.Clave)),
                        Avatar = reader.GetString(nameof(Propietario.Avatar))
                    };
                    mySqlDatabase.Dispose();
                    return propietario;
                }
            }
        }
        return null;
    }

    public int CreatePropietario(MySqlDatabase mySqlDatabase, Propietario createPropietario)
    {
        int res = -1;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"INSERT INTO Propietario (Nombre, Apellido, Direccion, Telefono, Dni, Email, , Clave, Avatar) 
                                VALUES (@Nombre, @Apellido, @Direccion, @Telefono, @Dni, @Email, @Clave, @Avatar);
                                SELECT LAST_INSERT_ID();";

            cmd.Parameters.AddWithValue("@Nombre", createPropietario.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", createPropietario.Apellido);
            cmd.Parameters.AddWithValue("@Direccion", createPropietario.Direccion);
            cmd.Parameters.AddWithValue("@Telefono", createPropietario.Telefono);
            cmd.Parameters.AddWithValue("@Dni", createPropietario.Dni);
            cmd.Parameters.AddWithValue("@Email", createPropietario.Email);
            cmd.Parameters.AddWithValue("@Clave", createPropietario.Clave);
            cmd.Parameters.AddWithValue("@Avatar", createPropietario.Avatar);
            
            res = Convert.ToInt32(cmd.ExecuteScalar());
            createPropietario.IdPropietario = res;
            
        }
        return res;
        
    }

    public int UpdatePropietario(MySqlDatabase mySqlDatabase, Propietario Propietario)
    {
        int res = -1;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
        
            cmd.CommandText = @"UPDATE Propietario SET Nombre = @Nombre, Apellido = @Apellido, Direccion = @Direccion, Telefono = @Telefono, Dni = @Dni, Email = @Email, Clave = @Clave, Avatar = @Avatar
                                WHERE IdPropietario = @IdPropietario;";

            cmd.Parameters.AddWithValue("@IdPropietario", Propietario.IdPropietario);
            cmd.Parameters.AddWithValue("@Nombre", Propietario.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", Propietario.Apellido);
            cmd.Parameters.AddWithValue("@Direccion", Propietario.Direccion);
            cmd.Parameters.AddWithValue("@Telefono", Propietario.Telefono);
            cmd.Parameters.AddWithValue("@Dni", Propietario.Dni);
            cmd.Parameters.AddWithValue("@Email", Propietario.Email);
            cmd.Parameters.AddWithValue("@Clave", Propietario.Clave);
            cmd.Parameters.AddWithValue("@Avatar", Propietario.Avatar);

            res = Convert.ToInt32(cmd.ExecuteNonQuery());
        }

        return res;       
    }

    public int DeletePropietario( MySqlDatabase mySqlDatabase, int id)
    {
        int res = -1;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"DELETE FROM Propietario WHERE IdPropietario = @IdPropietario";
            cmd.Parameters.AddWithValue("@IdPropietario", id);

            res = Convert.ToInt32(cmd.ExecuteNonQuery());
            
        }
        return res;
    }

    public List<Propietario> BuscarPropietario(MySqlDatabase mySqlDatabase, string nombreCompleto){
        var propietarios = new List<Propietario>();
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdPropietario, Nombre, Apellido, Direccion, Telefono, Dni, Email, Clave, Avatar
                            FROM Propietario
                            WHERE CONCAT(Nombre, ' ', Apellido) LIKE @nombreCompleto
                            LIMIT 10";
            cmd.Parameters.AddWithValue("@nombreCompleto", "%" + nombreCompleto + "%");
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var propietario = new Propietario
                    {
                        IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
                        Nombre = reader.GetString(nameof(Propietario.Nombre)),
                        Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        Direccion = reader.GetString(nameof(Propietario.Direccion)),
                        Telefono = reader.GetString(nameof(Propietario.Telefono)),
                        Dni = reader.GetString(nameof(Propietario.Dni)),
                        Email = reader.GetString(nameof(Propietario.Email)),
                        Clave = reader.GetString(nameof(Propietario.Clave)),
                        Avatar = reader.GetString(nameof(Propietario.Avatar))
                    };
                    propietarios.Add(propietario);
                }

            }
        }
        return propietarios;
    }
}