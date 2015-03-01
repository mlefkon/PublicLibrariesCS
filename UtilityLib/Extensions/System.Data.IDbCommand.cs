// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Text.RegularExpressions;
using System.Data;

namespace UtilityLib {
    public static partial class IDbCommandExtensions {
        public static string ToSQL(this IDbCommand Cmd) { 
            string sql = Cmd.CommandText.ReplaceCR(" ");
            foreach (IDataParameter param in Cmd.Parameters) {
                string val = "";
                if (Cmd.Connection.GetType().Namespace == "System.Data.SQLite") {
                    // special formatting for certain types if necessary
                }
                if (val == "") {
                    switch (param.DbType) {
                        case DbType.Boolean:
                            val = Convert.ToBoolean(param.Value) ? "1" : "0";
                            break;
                        case DbType.Date:
                        case DbType.DateTime:
                        case DbType.DateTime2:
                        case DbType.DateTimeOffset:
                        case DbType.Time:
                            val = '"' + Convert.ToDateTime(param.Value).ToString("yyyy-MM-dd HH:mm:ss.FFFFFFFK") + '"';
                            break;
                        case DbType.AnsiString:
                        case DbType.AnsiStringFixedLength:
                        case DbType.Guid:
                        case DbType.String:
                        case DbType.StringFixedLength:
                        case DbType.Xml:
                            val = '"' + param.Value.ToString() + '"';
                            break;
                        default:
                            val = param.Value.ToString();
                            break;
                    }
                }
                sql = Regex.Replace(sql, @"(?<=(\W|^))" + param.ParameterName + @"(?=(\W|$))", val);
            }
            return sql;
        } 
    }
}
