using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedResources.Settings
{
    public class AppSettings
    {
        public DatabaseSettings DatabaseSettings { get; set; } = new DatabaseSettings();
        public DictonarySettings DictonarySettings { get; set; } = new DictonarySettings();
    }

    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
    }

    public class DictonarySettings
    {
        public string APIUrl { get; set; } = string.Empty;
        public string RandomWordAPIUrl  { get; set; } = string.Empty;
    }
}
