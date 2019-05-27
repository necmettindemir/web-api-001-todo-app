using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TodoDB.Library;

namespace TodoDB.Data
{
    public abstract class BaseRepoMSSQL
    {
        protected readonly string _connStr;
        protected readonly string _secretKey;

        protected readonly IDbConnection _dbConn;
        protected readonly Utilities _utilities;


        public BaseRepoMSSQL(string connStr, string secretKey)
        {
            _connStr = connStr;
            _secretKey = secretKey;

            _dbConn = new SqlConnection(_connStr);
            _utilities = new Utilities();
        }

    }
}
