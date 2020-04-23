using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DTO;
using Data;
using Dapper;
using RaspberryPiCore.ADC;
using RaspberryPiCore.i2cdotnet;
using RaspberryPiCore.LCD;
using RaspberryPiCore.TWIST;


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
      private TWIST endcoder;
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

         endcoder.setLimit(MaksCount);
      }

      public Patient_CPR getpatientCPR()
      {
         Patient = DBaccess.loadPatient(EKGID);
         return Patient;
      }

      //Metoden som opretter en EKGmåling, samtidig med informationsskrivning på displayet. 
      public List<byte> EKGmaalingCreate()
      {
         List<byte> byteliste = new List<byte>();
         int malingtæller = 0;


         byte nedtællingstal = 50;
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
            malingtæller++;

            if (malingtæller == 20)
            {
               --nedtællingstal;
               Interface.CountDown50(nedtællingstal);
               malingtæller = 0;
            }

         }

         return byteliste;
      }


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

