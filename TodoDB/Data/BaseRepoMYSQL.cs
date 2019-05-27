using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TodoDB.Library;
using MySql.Data.MySqlClient;

namespace TodoDB.Data
{
    public abstract class BaseRepoMYSQL
    {
        protected readonly string _connStr;
        protected readonly string _secretKey;

        protected readonly IDbConnection _dbConn;
        protected readonly Utilities _utilities;


        public BaseRepoMYSQL(string connStr, string secretKey)
        {
            _connStr = connStr;
            _secretKey = secretKey;
            
            _dbConn = new MySqlConnection(_connStr);
            _utilities = new Utilities();
        }

    }
}
