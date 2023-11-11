
using MySql.Data.MySqlClient;

namespace Inmobiliaria.Models;

public class RepositorioInmueble
{
    public RepositorioInmueble()
    {
    }

    public List<Inmueble> GetInmuebles(MySqlDatabase mySqlDatabase)
    {
        var inmuebles = new List<Inmueble>();
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT i.IdInmueble, i.Tipo, i.Direccion, i.Coordenadas, i.Precio, i.Ambientes, i.Uso, i.Activo, i.IdPropietario,
                                p.Nombre, p.Apellido
                                FROM Inmueble i INNER JOIN Propietario p ON i.IdPropietario = p.IdPropietario";

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var inmueble = new Inmueble
                    {
                        IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                        Tipo = reader.GetString(nameof(Inmueble.Tipo)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Uso = reader.GetString(nameof(Inmueble.Uso)),
                        Activo = reader.GetBoolean(nameof(Inmueble.Activo)),
                        IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                        Propietario = new Propietario
                        {
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        }
                    };
                    inmuebles.Add(inmueble);
                }

            }
        }
        return inmuebles;
    }

    public Inmueble GetInmueble(MySqlDatabase mySqlDatabase, int id)
    {
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT i.IdInmueble, i.Tipo, i.Direccion, i.Coordenadas, i.Precio, i.Ambientes, i.Uso, i.Activo, i.IdPropietario, 
                                p.Nombre, p.Apellido
                                FROM Inmueble i INNER JOIN Propietario p ON i.IdPropietario = p.IdPropietario
                                WHERE IdInmueble = @IdInmueble";
            cmd.Parameters.AddWithValue("@IdInmueble", id);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var inmueble = new Inmueble
                    {
                        IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                        Tipo = reader.GetString(nameof(Inmueble.Tipo)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Uso = reader.GetString(nameof(Inmueble.Uso)),
                        Activo = reader.GetBoolean(nameof(Inmueble.Activo)),
                        IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                        Propietario = new Propietario
                        {
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        }
                    };

                    return inmueble;
                }
            }
        }
        return null;
    }

    public int CreateInmueble(MySqlDatabase mySqlDatabase, Inmueble CreateInmueble)
    {
        Console.WriteLine(CreateInmueble.ToString());
        int res = -1;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {

            cmd.CommandText = @"INSERT INTO Inmueble (Tipo, 
                                                    Direccion, 
                                                    Coordenadas, 
                                                    Precio, 
                                                    Ambientes, 
                                                    Uso, 
                                                    Activo, 
                                                    IdPropietario) 
                                VALUES (@Tipo, @Direccion, @Coordenadas, @Precio, @Ambientes, @Uso, @Activo, @IdPropietario);
                                SELECT LAST_INSERT_ID();";

            cmd.Parameters.AddWithValue("@Tipo", CreateInmueble.Tipo);
            cmd.Parameters.AddWithValue("@Direccion", CreateInmueble.Direccion);
            cmd.Parameters.AddWithValue("@Coordenadas", CreateInmueble.Coordenadas);
            cmd.Parameters.AddWithValue("@Precio", CreateInmueble.Precio);
            cmd.Parameters.AddWithValue("@Ambientes", CreateInmueble.Ambientes);
            cmd.Parameters.AddWithValue("@Uso", CreateInmueble.Uso);
            cmd.Parameters.AddWithValue("@Activo", CreateInmueble.Activo);
            cmd.Parameters.AddWithValue("@IdPropietario", CreateInmueble.IdPropietario);

            res = Convert.ToInt32(cmd.ExecuteScalar());
            CreateInmueble.IdInmueble = res;
        }
        return res;
    }

    public int UpdateInmueble(MySqlDatabase mySqlDatabase, Inmueble inmueble)
    {
        int res = -1;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {

            cmd.CommandText = @"UPDATE Inmueble SET Tipo = @Tipo, 
                                                    Direccion = @Direccion,
                                                    Coordenadas = @Coordenadas, 
                                                    Precio = @Precio, 
                                                    Ambientes = @Ambientes, 
                                                    Uso = @Uso, 
                                                    Activo = @Activo, 
                                                    IdPropietario = @IdPropietario
                                WHERE IdInmueble = @IdInmueble;";

            cmd.Parameters.AddWithValue("@IdInmueble", inmueble.IdInmueble);
            cmd.Parameters.AddWithValue("@Tipo", inmueble.Tipo);
            cmd.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
            cmd.Parameters.AddWithValue("@Coordenadas", inmueble.Coordenadas);
            cmd.Parameters.AddWithValue("@Precio", inmueble.Precio);
            cmd.Parameters.AddWithValue("@Ambientes", inmueble.Ambientes);
            cmd.Parameters.AddWithValue("@Uso", inmueble.Uso);
            cmd.Parameters.AddWithValue("@Activo", inmueble.Activo);
            cmd.Parameters.AddWithValue("@IdPropietario", inmueble.IdPropietario);

            res = Convert.ToInt32(cmd.ExecuteNonQuery());
        }
        return res;
    }

    public int DeleteInmueble(MySqlDatabase mySqlDatabase, int id)
    {
        int res = -1;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"DELETE FROM Inmueble WHERE IdInmueble = @IdInmueble";
            cmd.Parameters.AddWithValue("@IdInmueble", id);

            res = Convert.ToInt32(cmd.ExecuteNonQuery());
        }
        return res;
    }

    public List<Inmueble> BuscarInmueble(MySqlDatabase mySqlDatabase, string busqueda, string buscarPor)
    {
        var inmuebles = new List<Inmueble>();
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdInmueble, Tipo, Direccion, Coordenadas, Precio, Ambientes, Uso, Activo, IdPropietario 
                            FROM Inmueble 
                            WHERE " + buscarPor + " LIKE @busqueda AND Activo = 1 LIMIT 10";

            /* cmd.Parameters.AddWithValue("@buscarPor", buscarPor); */
            cmd.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var inmueble = new Inmueble
                    {
                        IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                        Tipo = reader.GetString(nameof(Inmueble.Tipo)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Uso = reader.GetString(nameof(Inmueble.Uso)),
                        Activo = reader.GetBoolean(nameof(Inmueble.Activo)),
                        IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                    };
                    inmuebles.Add(inmueble);
                }
            }
        }
        return inmuebles;
    }

    public List<Inmueble> BuscarInmuebles(MySqlDatabase mySqlDatabase, string busqueda, string buscarPor)
    {
        var inmuebles = new List<Inmueble>();
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT i.IdInmueble, i.Tipo, i.Direccion, i.Coordenadas, i.Precio, i.Ambientes, i.Uso, i.Activo, i.IdPropietario,
                    p.Nombre, p.Apellido
                    FROM Inmueble i 
                    JOIN Propietario p ON i.IdPropietario = p.IdPropietario
                    WHERE i.Activo = 1 LIMIT 10";

            if(buscarPor == "Propietario"){
                cmd.CommandText = @"SELECT i.IdInmueble, i.Tipo, i.Direccion, i.Coordenadas, i.Precio, i.Ambientes, i.Uso, i.Activo, i.IdPropietario, 
                                p.Nombre, p.Apellido
                                FROM Inmueble i
                                JOIN Propietario p ON i.IdPropietario = p.IdPropietario
                                WHERE (CONCAT(p.Nombre, ' ', p.Apellido) LIKE @busqueda) LIMIT 10";
                
                cmd.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");
            }
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var inmueble = new Inmueble
                    {
                        IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                        Tipo = reader.GetString(nameof(Inmueble.Tipo)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Uso = reader.GetString(nameof(Inmueble.Uso)),
                        Activo = reader.GetBoolean(nameof(Inmueble.Activo)),
                        IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                        Propietario = new Propietario
                        {
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        }
                    };
                    inmuebles.Add(inmueble);
                }
            }
        }
        return inmuebles;
    }

    public List<Inmueble> BuscarInmueblesSinContrato(MySqlDatabase mySqlDatabase, string fechaInicio, string fechaFin)
    {
        var inmuebles = new List<Inmueble>();
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT i.IdInmueble, i.Tipo,i.Direccion, i.Coordenadas, i.Precio, i.Ambientes, i.Uso, i.Activo, i.IdPropietario,
                                p.Nombre, p.Apellido
                                FROM Inmueble i
                                JOIN Propietario p ON i.IdPropietario = p.IdPropietario
                                WHERE i.Activo = 1 AND i.IdInmueble NOT IN (
                                    SELECT c.IdInmueble
                                    FROM Contrato c
                                    WHERE (c.FechaInicio BETWEEN @fechaInicio AND @fechaFin)
                                        OR (c.FechaFin BETWEEN @fechaInicio AND @fechaFin)
                                )";
            cmd.Parameters.AddWithValue("@fechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var inmueble = new Inmueble
                    {
                        IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                        Tipo = reader.GetString(nameof(Inmueble.Tipo)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                        Uso = reader.GetString(nameof(Inmueble.Uso)),
                        Activo = reader.GetBoolean(nameof(Inmueble.Activo)),
                        IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                        Propietario = new Propietario
                        {
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        }
                    };
                    inmuebles.Add(inmueble);
                }
            }
        }
        return inmuebles;
    }

}