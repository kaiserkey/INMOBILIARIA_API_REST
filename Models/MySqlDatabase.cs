
using MySql.Data.MySqlClient;

namespace Inmobiliaria.Models
{
    public class MySqlDatabase : IDisposable
    {
        public MySqlConnection Connection;
        public string connectionString = "Server=127.0.0.1;Port=3307;database=Inmobiliaria;uid=root;password=kaiserkey;SslMode=none";
        public MySqlDatabase()
        {
            Connection = new MySqlConnection(connectionString);
            this.Connection.Open();
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}