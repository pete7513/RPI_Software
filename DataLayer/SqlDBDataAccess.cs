using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DTO;
using System.Linq;

namespace Data
{
   //192.168.0.218\SQLEXPRES

   //Tjenester start server
   // SQL server genstart i configarations manager 

   public class SqlDBDataAccess
   {
      //Lokal database på Skolens server. 
      private readonly string connectionStringST = @"Data Source=st-i4dab.uni.au.dk;Initial Catalog=F20ST2ITS2201908477;Integrated Security=False;User ID=F20ST2ITS2201908477;Password=F20ST2ITS2201908477;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
      // Lokal DataBase på Asbjørns computer 
      private readonly string connectionStringLDB = @"Data Source=192.168.0.218\SQLEXPRESS;Initial Catalog=F20ST2ITS2201908477;Integrated Security=False;User ID=F2020ST2ITS2201908477;Password=F20ST2ITS2201908477;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";

      private SqlConnection connection;
      private SqlCommand command;
      private string sql = null;
      private SqlDataReader dataReader;

      private Patient_CPR Patient;

      private List<DateTime> historikDato;

      //Kontruktor for den online database
      public SqlDBDataAccess()
      {
         connection = new SqlConnection(connectionStringST);
      }

      //Hente informationer omkring den patient som er tilknyttet EKGmåleren.
      public Patient_CPR LoadPatient(string EKGID)
      {
         try
         {
            connection.Open();
            sql = "Select navn, CPR from dbo.EKGPatient where EKGID = " + EKGID;

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
            Patient.PatientName = "NN";
            Patient.CPR = "123456-7890";
            return Patient;
         }
      }

      //Hente informationer (Datetime) omkring patientens 3 seneste målinger med EKG målerne. 
      public List<DateTime> LoadHistorik(string cpr)
      {
         connection.Open();
         historikDato = new List<DateTime>();
         sql = "SELECT TOP 3 tidsstempel FROM EKGDATA WHERE CPR = '" + cpr + "' ORDER BY tidsstempel DESC;";

         using (command = new SqlCommand(sql, connection))
         {
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
            
                  historikDato.Add(Convert.ToDateTime(dataReader["tidsstempel"]));

            }
         }
         connection.Close();
         return historikDato;
      }

      //Skal kunne uploade en EKG måling, som er tilknyttet patienten. 
      public void EKGM_DB_Sendt(EKG_Maaling _Maaling)
      {
         double[] array = new double[250];
         array = _Maaling.EKG_Data;

         connection.Open();

         string insertStringParam = @"INSERT INTO dbo.EKGDATA (tidsstempel, CPR, EKG_data, samplerate_hz, interval_sec,data_format, bin_eller_tekst, maaleformat_type,start_tid, maalenehed) 
                                      VALUES(@Datetime, @CPR, @EKGdata, @Samplerate_hz, @Interval_sec, @Data_format, @Bin_eller_tekst, @Maaleformat_type, @Start_tid, @Maalenehed)";

         Console.WriteLine(_Maaling.DateTime);
         using (command = new SqlCommand(insertStringParam, connection))
         {
            command.Parameters.AddWithValue("@Datetime", _Maaling.DateTime);
            command.Parameters.AddWithValue("@CPR", _Maaling.CPR);
            command.Parameters.AddWithValue("@EKGdata", array.SelectMany(value => BitConverter.GetBytes(value)).ToArray());
            command.Parameters.AddWithValue("@Samplerate_hz", _Maaling.Samplerate);
            command.Parameters.AddWithValue("@Interval_sec", _Maaling.Periode);
            command.Parameters.AddWithValue("@Data_format", _Maaling.Dataformat);
            command.Parameters.AddWithValue("@Bin_eller_tekst", _Maaling.Bin_text);
            command.Parameters.AddWithValue("@Maaleformat_type", _Maaling.Maaletype);
            command.Parameters.AddWithValue("@Start_tid", _Maaling.DateTime);
            command.Parameters.AddWithValue("@Maalenehed", _Maaling.EKGID);
            command.ExecuteScalar();
         }
         connection.Close();
      }
   }
}
