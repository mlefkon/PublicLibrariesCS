// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Linq;
using System.Collections.Generic;

namespace UtilityLib {
    public class SqlMigrations {
        private SortedSet<SqlMigration> _migrations = new SortedSet<SqlMigration>();
        public void AddMigration(SqlMigration Migration) { _migrations.Add(Migration); }
        public void AddMigration(IEnumerable<SqlMigration> Migrations) { Migrations.ForEach(m => _migrations.Add(m)); }
        public IEnumerable<string> GetUpdgradeSql(System.Version DbVersion) { // Note: Don't check DbVersion here since it reflects the Engine.Assembly.Version, so it may be greater than Max.AppVersion.
            if (DbVersion == null) return _migrations.Select(m => m.MigrateSql);
            if (DbVersion >  _migrations.Max.AppVersion) throw new Exception("Error: Cannot upgrade Database version (" + DbVersion + ") to a lower version (" + _migrations.Max.AppVersion + ")");
            //if (DbVersion == _migrations.Max.AppVersion) throw new Exception("Error: Database version (" + DbVersion + ") is already the same as the upgrade (" + _migrations.Max.AppVersion + ")");
            return _migrations.Where(m => m.AppVersion.CompareTo(DbVersion) > 0).Select(m => m.MigrateSql);
        }
    }
    public class SqlMigration {
        public System.Version AppVersion {get; set;}
        public string MigrateSql {get; set;}
        public SqlMigration(string SqlAppVersion, string SQL) {
            this.AppVersion = new Version(SqlAppVersion);
            this.MigrateSql = SQL;
        }
        public override string ToString() { return AppVersion.ToString(); }
        public override int GetHashCode() { return AppVersion.GetHashCode(); }
        public override bool Equals(object obj) { return (obj as SqlMigration) != null && (obj as SqlMigration).AppVersion.Equals(AppVersion); }
    }
}
