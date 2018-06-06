using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace JNogueira.Bufunfa.Infraestrutura.Dados
{
    /// <summary>
    /// Datacontext para utilização do banco MySql
    /// </summary>
    public class BufunfaDataContext : IDisposable
    {
        public MySqlConnection Connection { get; set; }

        public BufunfaDataContext(string connectionString)
        {
            this.Connection = new MySqlConnection(connectionString);
            this.Connection.Open();
        }

        public void Dispose()
        {
            if (this.Connection.State != ConnectionState.Closed)
                this.Connection.Close();
        }
    }
}
