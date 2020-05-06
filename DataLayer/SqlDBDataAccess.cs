using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using DTO;
using System.Linq;

namespace Data
{
   public class SqlDBDataAccess
   {
      private string connectionStringST = @"Data Source=st-i4dab.uni.au.dk;Initial Catalog=F20ST2ITS2201908477;Integrated Security=False;User ID=F20ST2ITS2201908477;Password=F20ST2ITS2201908477;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
      private SqlConnection connection;
      private SqlCommand command;
      private string sql = null;
      private SqlDataReader dataReader;

      private EKG_Maaling maaling;
      private Patient_CPR Patient;

     private List<DateTime> historikDato;


      //Kontruktor for den online database
      public SqlDBDataAccess()
      {
         connection = new SqlConnection(connectionStringST);
      }

      //Hente informationer omkring den patient som er tilknyttet EKGmåleren.
      public Patient_CPR loadPatient(string EKGID)
      {
         try
         {
            connection.Open();
            sql = "Select navn from dbo.EKGPatient where EKGID = " + EKGID;

            command = new SqlCommand(sql, connection);
            dataReader = command.ExecuteReader();

            if (dataReader.Read())
               Patient = new Patient_CPR(Convert.ToString(dataReader["navn"]), Convert.ToString(dataReader["CPR"]));

            dataReader.Close();
            command.Dispose();
            connection.Close();
            return Patient;
         }
         catch
         {
            return null;
         }
      }

      public List<DateTime> loadHistorik(string cpr)
      {
         connection.Open();
         historikDato = new List<DateTime>();
         sql = "SELECT TOP 3 tidsstempel FROM EKGDATA WHERE CPR = '"+cpr+"' ORDER BY Dato DESC;";

         using (command = new SqlCommand(sql, connection))
         {
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    if (dataReader["CPR"].ToString() == cpr)
                    {
                        historikDato.Add(Convert.ToDateTime(dataReader["tidsstempel"]));
                    }
                }
            }
         connection.Close();
         return historikDato;
      }


      //Skal kunne uploade en EKG måling, som er tilknyttet patienten. 
      public void EKGM_DB_Sendt(EKG_Maaling _Maaling)
      {
         double[] array = new double[1100];
         array = _Maaling.EKG_Data; 

         connection.Open();

         string insertStringParam = @"INSERT INTO dbo.EKGDATA (tidsstempel, CPR, EKG_data) 
                                      VALUES(@Datetime,@CPR,@EKGdata)";

         using (command = new SqlCommand(insertStringParam, connection))
         {
            command.Parameters.AddWithValue("@Datetime", _Maaling.DateTime);
            command.Parameters.AddWithValue("@CPR", _Maaling.CPR);
            command.Parameters.AddWithValue("@EKGdata", array.SelectMany(value => BitConverter.GetBytes(value)).ToArray());
            command.ExecuteReader(); 
         }
         connection.Close();
      }

      //private SqlConnection OpenConnectionST
      //{
      //   get
      //   {
      //      var con = new SqlConnection(@"Data Source=i4dab.ase.au.dk;Initial Catalog=F17ST2PRJ2OffEKGDatabase;Integrated Security=False;User ID=F17ST2PRJ2OffEKGDatabase;Password=F17ST2PRJ2OffEKGDatabase;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False");

      //      con.Open();

      //      return con;

      //   }

      //}

      public void TestListSave()

      {

         //string insertStringParam = @"INSERT INTO EKGDATA (raa_data, ekgmaaleid, samplerate_hz, interval_sec, data_format, bin_eller_tekst, maaleformat_type, start_tid)

         //                                                     OUTPUT INSERTED.ekgdataid  //Får databasen til at returnere værdien for primærnøglen ekgdataid som pga af IDENTITY funktionen generes automatisk af database (Kan ikke skrives fra klienten af)

         //                           VALUES(@data, 164, 400, 3600, N'2015-04-27', '1', N'double', CONVERT(DATETIME, '2015-04-27 12:34:43', 102))"; //Alle værdier pånær raa_data er her kodet helt specifikt





         List<int> data = new List<int>();

         data.Add(464646);

         data.Add(464646);

         data.Add(464646);

         data.Add(464646);

         data.Add(464646);

         data.Add(464646);

         data.Add(464646);

         data.Add(464646);



         //   using (SqlCommand cmd = new SqlCommand(insertStringParam, OpenConnectionST))

         //   {

         //      // Get your parameters ready                    

         //      cmd.Parameters.AddWithValue("@data", data.ToArray().SelectMany(value => BitConverter.GetBytes(value)).ToArray());
         //      long id = (long)cmd.ExecuteScalar(); //Returns the identity of the new tuple/record  64 bit/8 bytes
         //   }

         //}
      }
   }
}
