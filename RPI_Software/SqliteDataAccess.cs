using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Assemblies;

namespace RPI_Software
{
    public class SqliteDataAccess
    {
      public static List<string> patient()
      {
         using (IDbConnection cnn = new SQLiteConnection(LoadConnectString()))
         {
            var Output = cnn.Query<string>("select * from Person", new DynamicParameters());

            return Output.ToList();
         }
      }

      public static void SaveEGM_måling(EKG_Maaling EKG)
      {
         using (IDbConnection cnn = new SQLiteConnection(LoadConnectString()))
         {
            cnn.Execute("insert into Person (FirstName, LastName, BIT) values (@FirstName, @LastName, @array)", EKG);
         }
      }



      private static string LoadConnectString(string ID = "Default")
      {
         //Skal places i app.config - dette er tilhørende framework, men da dette er et Core program har den ikke denne app.config. 

      //     < connectionStrings >
      //     < add name = "Default" connectionString = "Data Source=.\DemoDB.db;Version=3;" providerName = "System.Data.SqlClient" />
     
      //     </ connectionStrings >
         //


         //return ConfigurationManager.ConnectionStrings[ID].ConnectionString;
      }
   }
}
