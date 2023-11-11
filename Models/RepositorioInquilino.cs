
using MySql.Data.MySqlClient;

namespace Inmobiliaria.Models;

public class RepositorioInquilino
{
    public RepositorioInquilino()
    {
    }

    public List<Inquilino> GetInquilinos(MySqlDatabase mySqlDatabase)
    {
        var inquilinos = new List<Inquilino>();
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdInquilino, Nombre, Apellido, Telefono, Dni, Email, FechaNacimiento FROM Inquilino";

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var inquilino = new Inquilino
                    {
                        IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
                        Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                        Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                        Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                        Dni = reader.GetString(nameof(Inquilino.Dni)),
                        Email = reader.GetString(nameof(Inquilino.Email)),
                        FechaNacimiento = reader.GetDateTime(nameof(Inquilino.FechaNacimiento))
                    };
                    inquilinos.Add(inquilino);
                }

            }
        }
        return inquilinos;
    }

    public Inquilino GetInquilino(MySqlDatabase mySqlDatabase, int id)
    {
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdInquilino, Nombre, Apellido, Telefono, Dni, Email, FechaNacimiento 
                                FROM Inquilino WHERE IdInquilino = @IdInquilino";
            cmd.Parameters.AddWithValue("@IdInquilino", id);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var inquilino = new Inquilino
                    {
                        IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
                        Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                        Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                        Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                        Dni = reader.GetString(nameof(Inquilino.Dni)),
                        Email = reader.GetString(nameof(Inquilino.Email)),
                        FechaNacimiento = reader.GetDateTime(nameof(Inquilino.FechaNacimiento))
                    };
                    mySqlDatabase.Dispose();
                    return inquilino;
                }
            }
        }
        return null;
    }

    public Inquilino GetInquilinoPorEmail(MySqlDatabase mySqlDatabase, string email)
    {
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdInquilino, Nombre, Apellido, Telefono, Dni, Email, FechaNacimiento 
                                FROM Inquilino WHERE Email = @Email";
            cmd.Parameters.AddWithValue("@Email", email);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var inquilino = new Inquilino
                    {
                        IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
                        Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                        Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                        Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                        Dni = reader.GetString(nameof(Inquilino.Dni)),
                        Email = reader.GetString(nameof(Inquilino.Email)),
                        FechaNacimiento = reader.GetDateTime(nameof(Inquilino.FechaNacimiento))
                    };
                    mySqlDatabase.Dispose();
                    return inquilino;
                }
            }
        }
        return null;
    }

    public int CreateInquilino(MySqlDatabase mySqlDatabase, Inquilino CreateInquilino)
    {
        int res = -1;
        var fechaFormat = CreateInquilino.FechaNacimiento.ToString("yyyy-MM-dd HH:mm:ss");

        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"INSERT INTO Inquilino (Nombre, Apellido, Telefono, Dni, Email, FechaNacimiento) 
                                VALUES (@Nombre, @Apellido, @Telefono, @Dni, @Email, @FechaNacimiento);
                                SELECT LAST_INSERT_ID();";

            cmd.Parameters.AddWithValue("@Nombre", CreateInquilino.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", CreateInquilino.Apellido);
            cmd.Parameters.AddWithValue("@Telefono", CreateInquilino.Telefono);
            cmd.Parameters.AddWithValue("@Dni", CreateInquilino.Dni);
            cmd.Parameters.AddWithValue("@Email", CreateInquilino.Email);
            cmd.Parameters.AddWithValue("@FechaNacimiento", fechaFormat);

            res = Convert.ToInt32(cmd.ExecuteScalar());
            CreateInquilino.IdInquilino = res;
        }

        return res;
    }

    public int UpdateInquilino(MySqlDatabase mySqlDatabase, Inquilino Inquilino)
    {
        var fechaFormat = Inquilino.FechaNacimiento.ToString("yyyy-MM-dd HH:mm:ss");
        int res = -1;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {

            cmd.CommandText = @"UPDATE Inquilino SET Nombre = @Nombre, Apellido = @Apellido, Telefono = @Telefono, Dni = @Dni, Email = @Email, FechaNacimiento = @FechaNacimiento
                                WHERE IdInquilino = @IdInquilino;";

            cmd.Parameters.AddWithValue("@IdInquilino", Inquilino.IdInquilino);
            cmd.Parameters.AddWithValue("@Nombre", Inquilino.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", Inquilino.Apellido);
            cmd.Parameters.AddWithValue("@Telefono", Inquilino.Telefono);
            cmd.Parameters.AddWithValue("@Dni", Inquilino.Dni);
            cmd.Parameters.AddWithValue("@Email", Inquilino.Email);
            cmd.Parameters.AddWithValue("@FechaNacimiento", fechaFormat);

            res = Convert.ToInt32(cmd.ExecuteNonQuery());

        }
        return res;
    }

    public int DeleteInquilino(MySqlDatabase mySqlDatabase, int id)
    {
        int res = -1;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"DELETE FROM Inquilino WHERE IdInquilino = @IdInquilino";
            cmd.Parameters.AddWithValue("@IdInquilino", id);

            res = Convert.ToInt32(cmd.ExecuteNonQuery());
        }
        return res;
    }

    public List<Inquilino> BuscarInquilino(MySqlDatabase mySqlDatabase, string nombreCompleto)
    {
        var inquilinos = new List<Inquilino>();
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdInquilino, Nombre, Apellido, Email, Dni, Telefono, FechaNacimiento 
                            FROM Inquilino
                            WHERE CONCAT(Nombre, ' ', Apellido) LIKE @nombreCompleto
                            LIMIT 10";
            cmd.Parameters.AddWithValue("@nombreCompleto", "%" + nombreCompleto + "%");
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var inquilino = new Inquilino
                    {
                        IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
                        Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                        Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                        Email = reader.GetString(nameof(Inquilino.Email)),
                        Dni = reader.GetString(nameof(Inquilino.Dni)),
                        Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                        FechaNacimiento = reader.GetDateTime(nameof(Inquilino.FechaNacimiento))
                    };
                    inquilinos.Add(inquilino);
                }
            }
        }
        return inquilinos;
    }
}