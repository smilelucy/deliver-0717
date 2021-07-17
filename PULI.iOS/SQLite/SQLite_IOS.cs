using Foundation;
using PULI.iOS.SQLite;
using PULI.Services.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_IOS))]
namespace PULI.iOS.SQLite
{
    public class SQLite_IOS : ISQLite
    {
        public SQLite_IOS()
        {
        }

        #region ISQLite implementation
        SQLiteConnection ISQLite.GetConnection()
        {
            var sqliteFilename = "DoggyDB.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            var path = Path.Combine(libraryPath, sqliteFilename);

            var conn = new SQLiteConnection(path);

            // Return the database connection 
            return conn;
        }

        #endregion
    }
}