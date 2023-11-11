
using MySql.Data.MySqlClient;

namespace Inmobiliaria.Models;

public class RepositorioPago
{
    public RepositorioPago()
    {
    }

    public List<Pago> GetPagos(MySqlDatabase mySqlDatabase)
    {
        var pagos = new List<Pago>();
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdPago, Importe, Fecha, NumeroPago, IdContrato
                            FROM Pago";

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var pago = new Pago
                    {
                        IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                        Importe = reader.GetDecimal(nameof(Pago.Importe)),
                        NumeroPago = reader.GetInt32(nameof(Pago.NumeroPago)),
                        Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                        IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                    };
                    pagos.Add(pago);
                }

            }
        }
        return pagos;
    }

    public Pago GetPago(MySqlDatabase mySqlDatabase, int id)
    {
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdPago, Importe, Fecha, NumeroPago, IdContrato
                            FROM Pago
                            WHERE IdPago = @IdPago";
            cmd.Parameters.AddWithValue("@IdPago", id);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var pago = new Pago
                    {
                        IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                        Importe = reader.GetDecimal(nameof(Pago.Importe)),
                        NumeroPago = reader.GetInt32(nameof(Pago.NumeroPago)),
                        Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                        IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                    };

                    return pago;
                }
            }
        }
        return null;
    }

    public Pago GetPagoPorContrato(MySqlDatabase mySqlDatabase, int id)
    {
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdPago, Importe, Fecha, NumeroPago, IdContrato
                            FROM Pago
                            WHERE IdContrato = @IdContrato";
            cmd.Parameters.AddWithValue("@IdContrato", id);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var pago = new Pago
                    {
                        IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                        Importe = reader.GetDecimal(nameof(Pago.Importe)),
                        NumeroPago = reader.GetInt32(nameof(Pago.NumeroPago)),
                        Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                        IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                    };

                    return pago;
                }
            }
        }
        return null;
    }

    public int CreatePago(MySqlDatabase mySqlDatabase, Pago createPago)
    {
        var fecha = createPago.Fecha.ToString("yyyy-MM-dd HH:mm:ss");
        int res = -1;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {

            cmd.CommandText = @"INSERT INTO Pago (IdContrato, Fecha, NumeroPago, Importe) 
                            VALUES (@IdContrato, @Fecha, @NumeroPago, @Importe);
                            SELECT LAST_INSERT_ID();";

            cmd.Parameters.AddWithValue("@IdContrato", createPago.IdContrato);
            cmd.Parameters.AddWithValue("@Fecha", fecha);
            cmd.Parameters.AddWithValue("@NumeroPago", createPago.NumeroPago);
            cmd.Parameters.AddWithValue("@Importe", createPago.Importe);

            res = Convert.ToInt32(cmd.ExecuteScalar());

            createPago.IdPago = res;
        }

        return res;
    }

    public int UpdatePago(MySqlDatabase mySqlDatabase, Pago pago)
    {
        var fecha = pago.Fecha.ToString("yyyy-MM-dd HH:mm:ss");
        int res = -1;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {

            cmd.CommandText = @"UPDATE Pago SET Importe = @Importe, Fecha = @Fecha, IdContrato = @IdContrato, NumeroPago = @NumeroPago
                            WHERE IdPago = @IdPago;";

            cmd.Parameters.AddWithValue("@IdPago", pago.IdPago);
            cmd.Parameters.AddWithValue("@Importe", pago.Importe);
            cmd.Parameters.AddWithValue("@NumeroPago", pago.NumeroPago);
            cmd.Parameters.AddWithValue("@Fecha", fecha);
            cmd.Parameters.AddWithValue("@IdContrato", pago.IdContrato);

            res = cmd.ExecuteNonQuery();
        }
        return res;
    }

    public int DeletePago(MySqlDatabase mySqlDatabase, int id)
    {
        int res = -1;
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {

            cmd.CommandText = @"DELETE FROM Pago WHERE IdPago = @IdPago;";

            cmd.Parameters.AddWithValue("@IdPago", id);

            res = cmd.ExecuteNonQuery();
        }
        return res;
    }

    public List<Pago> BuscarPagos(MySqlDatabase mySqlDatabase, int codigo)
    {
        var pagos = new List<Pago>();
        using (var cmd = mySqlDatabase.Connection.CreateCommand() as MySqlCommand)
        {
            cmd.CommandText = @"SELECT IdPago, Importe, Fecha, NumeroPago, IdContrato
                                FROM Pago
                                WHERE IdContrato = @codigo";
            cmd.Parameters.AddWithValue("@codigo", codigo);
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var pago = new Pago
                    {
                        IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                        Importe = reader.GetDecimal(nameof(Pago.Importe)),
                        NumeroPago = reader.GetInt32(nameof(Pago.NumeroPago)),
                        Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                        IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                    };
                    pagos.Add(pago);
                }

            }
        }
        return pagos;
    }

}