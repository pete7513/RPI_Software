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
         using (IDbConnection cnn = new SQLiteConnection(LoadConnectString()))
         {
            cnn.Execute("insert into Person (FirstName, LastName, BIT) values (@FirstName, @LastName, @array)", maaling);
         }
      }

      private string LoadConnectString(string ID = "Default")
      {
         //Dataccess skal ske i en app config fil, som kun findes i framework what to do?? 

         return null;   //ConfigurationManager.ConnectionStrings[ID].ConnectionString;
      }
   }
}
