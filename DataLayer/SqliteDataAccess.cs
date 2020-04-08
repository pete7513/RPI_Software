using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class SqliteDataAccess
    {
      public static List<Personmodel> GetPeople()
      {
         using (IDbConnection cnn = new SQLiteConnection(LoadConnectString()))
         {
            var Output = cnn.Query<Personmodel>("select * from Person", new DynamicParameters());
            return Output.ToList();
         }
      }


      public static List<int> GetList()
      {
         using (IDbConnection cnn = new SQLiteConnection(LoadConnectString()))
         {
           List<int> vs = new List<int>(); 
           var Output = cnn.Execute("select * from Person", new DynamicParameters());
            
           vs.Add(Output);
            return vs; 
         }
      }
    

      public static void EKGMSendt(ekg)
      {
         using (IDbConnection cnn = new SQLiteConnection(LoadConnectString()))
         {
            cnn.Execute("insert into Person (FirstName, LastName, BIT) values (@FirstName, @LastName, @array)", p);
         }
      }

      private static string LoadConnectString(string ID = "Default")
      {
         //Dataccess skal ske i en app config fil, som kun findes i framework what to do?? 


         return ConfigurationManager.ConnectionStrings[ID].ConnectionString;
      }
   }
}
