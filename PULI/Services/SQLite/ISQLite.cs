using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PULI.Services.SQLite
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
