using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DTO;
using Data;
using Dapper;
using RaspberryPiCore.ADC;
using RaspberryPiCore.i2cdotnet;



namespace LogicLayer
{
   public class Logic
   {
      #region Objekt referencer og atributter
      // UI og DB-UI <<Boundary>>

      private DataConnection dataConnection;
      private SqlDBDataAccess DBaccess;
      private SqliteDataAccess Liteaccess;

      // DTO Klasser <<Domain>>
      private EKG_Maaling maaling;
      private Patient_CPR Patient;

      // RPI komponenter <<Boundary>>
      private ADC1015 ADC;

      //Atributter 
      string EKGID;
      short StartMaaling;
      short Time;
      short Historik;
      short MaksCount;
      short Port;
      List<byte> EKGData;
      #endregion 

      //Konstruktor med oprettelse af relevante referencer og 
      public Logic()
      {
         DBaccess = new SqlDBDataAccess();
         Liteaccess = new SqliteDataAccess();

         ADC = new ADC1015();
         Patient = new Patient_CPR("NN", "NCPR");

         //Atribut værdier oprettes
         EKGID = "1011";
         StartMaaling = 0;
         Time = 1;
         Historik = 2;
         MaksCount = 2;
         Port = 0;
      }

      // Metoden skal returnere det patient_CPR objekt som datalaget returnere. 
      public Patient_CPR getpatientCPR()
      {
         Patient = DBaccess.loadPatient(EKGID);
         return Patient;
      }

      //Metoden som opretter en EKGmåling, samtidig med informationsskrivning på displayet. 
      public List<byte> EKGmaalingCreate()
      {
         List<byte> byteliste = new List<byte>();

         byte periode = 50;
         byte samplerate = 20;

         //antalMaalinger er antallet af målinger som ekgmåleren tager over perioden på 50 sekunder. 
         int AntalMaalinger = periode * samplerate;


         for (int i = 0; i < AntalMaalinger; i++)
         {
            byte sample = 0;
            sample = Convert.ToByte((ADC.readADC_SingleEnded(0) / 2048.0) * 6.144);
            byteliste.Add(sample);

            Thread.Sleep(1000 / (Convert.ToInt32(samplerate) - 4));
         }

         return byteliste;
      }

      //Metoden skal sende en EKGmåling, og returnere en specifik byte, alt efter - 
      // om metoden kunne sende en EKG målingen eller ej.
      public byte EKGMSendt(EKG_Maaling _Maaling)
      {

         try
         {
            DBaccess.EKGM_DB_Sendt(_Maaling);
            return 0; // Upload til online database 
         }
         catch
         {
            Console.WriteLine("Opload til online DB lykkes ikke");
            try
            {
               Liteaccess.EKGM_lite_Sendt(_Maaling);
               return 1; // Upload til Lokal database 
            }
            catch
            {
               Console.WriteLine("Opload til lokal DB lykkes ikke");
               return 2; // Upload fejlede til lokal og online database 
            }
         }
      }


   }
}

