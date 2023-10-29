using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingconcerns.Logging.Log4Net.Loggers
{
    public class DatabaseLogger:LoggerServiceBase
    {
        public DatabaseLogger():base("DatabaseLogger")
        {
            
        }
    }
}
