using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using DTO; 


namespace Data
{
   public class SqlDBDataAccess
   {
      public SqlDBDataAccess()
      {

      }

      //Hente informationer omkring den patient som er tilknyttet EKGmåleren.
      public Patient_CPR loadPatient()
      {

         return null; 
      }


      public void EKGM_DB_Sendt(EKG_Maaling _Maaling)
      { 
    

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
