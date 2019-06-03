using Coreflow.Interfaces;
using Coreflow.Objects;
using MySql.Data.MySqlClient;

namespace Coreflow.Activities.MySQL
{

    [DisplayMeta("Connect to MySQL", "MySQL", "fa-database")]
    public class ConnectToMySQL : ICodeActivity
    {
        public MySqlConnection Execute(
            string Host, 
            int Port, 
            string Database, 
            string Username, 
            string Password)
        {
            string connString = $"Server={Host};port={Port};Database={Database};User Id={Username};password={Password}";

            MySqlConnection conn = new MySqlConnection(connString);
            conn.Open();
            return conn;
        }
    }
}
