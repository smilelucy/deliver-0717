using PULI.Droid.SQLite;
using PULI.Services.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_Android))]
namespace PULI.Droid.SQLite
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android()
        {
            
        }
        #region ISQLite implementation
        // 初始化
        SQLiteConnection ISQLite.GetConnection()
        {
            var sqliteFilename = "DoggyDB.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);

            var conn = new SQLiteConnection(path);

            // Return the database connection 
            return conn;
        }
        #endregion
    }
}