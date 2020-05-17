﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using DTO;
using System.Linq;

namespace Data
{
   //192.168.0.218\SQLEXPRES

   //Tjenester start server
   // SQL server genstart i configarations manager 

   public class SqlDBDataAccess
   {
      private string connectionStringST = @"Data Source=st-i4dab.uni.au.dk;Initial Catalog=F20ST2ITS2201908477;Integrated Security=False;User ID=F20ST2ITS2201908477;Password=F20ST2ITS2201908477;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
      private string connectionStringSTL = @"Data Source=ASBJORN-LENOVO\SQLEXPRESS;Initial Catalog=F20ST2ITS2201908477;Integrated Security=False;User ID=F2020ST2ITS2201908477;Password=F20ST2ITS2201908477;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
      private string connectionStringLDB = @"Data Source=192.168.0.218\SQLEXPRESS;Initial Catalog=F20ST2ITS2201908477;Integrated Security=False;User ID=F2020ST2ITS2201908477;Password=F20ST2ITS2201908477;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
    
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
         connection = new SqlConnection(connectionStringLDB);
      }

      //Hente informationer omkring den patient som er tilknyttet EKGmåleren.
      public Patient_CPR loadPatient(string EKGID)
      {
         //try
         //{
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
         //}
         //catch
         //{
         //   Patient.PatientName = "NN";
         //   Patient.CPR = "123456-7890";
         //   return Patient; 
         //}
      }

      //Hente informationer (Datetime) omkring patientens 3 seneste målinger med EKG målerne. 
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

         string insertStringParam = @"INSERT INTO dbo.EKGDATA (tidsstempel, CPR, EKG_data, samplerate_hz, interval_sec,data_format, bin_eller_tekst, maaleformat_type,start_tid, maalenehed) 
                                      VALUES(@Datetime,@CPR,@EKGdata,@samplerate_hz,@interval_sec,@data_format,@bin_eller_tekst,@maaleformat_type,@start_tid,@maalenehed)";

         using (command = new SqlCommand(insertStringParam, connection))
         {
            command.Parameters.AddWithValue("@Datetime", _Maaling.DateTime);
            command.Parameters.AddWithValue("@CPR", _Maaling.CPR);
            command.Parameters.AddWithValue("@EKGdata", array.SelectMany(value => BitConverter.GetBytes(value)).ToArray());
            command.Parameters.AddWithValue("@samplerate_hz", maaling.Samplerate);
            command.Parameters.AddWithValue("@interval_sec", maaling.Periode);
            command.Parameters.AddWithValue("@data_format", maaling.Dataformat);
            command.Parameters.AddWithValue("@bin_eller_tekst", maaling.Bin_text);
            command.Parameters.AddWithValue("@maaleformat_type", maaling.Maaletype);
            command.Parameters.AddWithValue("@start_tid", maaling.DateTime);
            command.Parameters.AddWithValue("@maalenehed", maaling.EKGID); 

            command.ExecuteReader(); 
         }
         connection.Close();
      }

   }
}
