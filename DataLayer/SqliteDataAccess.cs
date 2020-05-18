using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Data; 

namespace Data
{
    public class SqliteDataAccess
    {
      public SqliteDataAccess()
      {

      }

      public void EKGM_lite_Sendt(EKG_Maaling maaling)
      {
         // ingen implementering 
      }

      private string LoadConnectString(string ID = "Default")
      {
         // ingen implementering. 
         return null; 
      }
   }
}
