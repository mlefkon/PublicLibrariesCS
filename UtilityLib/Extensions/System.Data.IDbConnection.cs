// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System.Data;

namespace UtilityLib {
    public static partial class IDbConnectionExtensions {
        public static int ExecuteNonQuery(this IDbConnection Conn, string CommandText, IDataParameter[] CommandParams = null) { 
            using (System.Data.IDbCommand cmd = Conn.CreateCommand()) {
                cmd.CommandText = CommandText;
                if (CommandParams != null) foreach (IDataParameter param in CommandParams) cmd.Parameters.Add(param);
                return cmd.ExecuteNonQuery();
            }
        }
        public static object ExecuteScalar(this IDbConnection Conn, string CommandText, IDataParameter[] CommandParams = null) { 
            using (System.Data.IDbCommand cmd = Conn.CreateCommand()) {
                cmd.CommandText = CommandText;
                if (CommandParams != null) foreach (IDataParameter param in CommandParams) cmd.Parameters.Add(param);
                return cmd.ExecuteScalar();
            }
        }
    }
}
