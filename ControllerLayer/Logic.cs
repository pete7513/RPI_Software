using System;
using System.Collections.Generic;
using System.Threading;
using DTO;
using Data;
using RaspberryPiCore.ADC;

namespace LogicLayer
{
   public class Logic
   {
      #region Objekt referencer og atributter
      // UI og DB-UI <<Boundary>>

      private SqlDBDataAccess DBaccess;
      private SqliteDataAccess Liteaccess;

      // DTO Klasser <<Domain>>
      private EKG_Maaling maaling;
      private Patient_CPR Patient;

      // RPI komponenter <<Boundary>>
      private ADC1015 ADC;

      //Atributter 
      private string EKGID;
      private List<DateTime> Dato = null;
      private double sample;
      private int taeller; 

      private double[] EKGData;
      #endregion

      //Konstruktor med oprettelse af relevante referencer 
      public Logic()
      {
         DBaccess = new SqlDBDataAccess();
         //Liteaccess = new SqliteDataAccess();

         ADC = new ADC1015();
         Patient = new Patient_CPR("NN", "NCPR");

         //Atribut værdier oprettes
         EKGID = "1011";
         Dato = new List<DateTime>();
         Dato = null; 
         taeller = 0;
      }

      // Metoden skal returnere det patient_CPR objekt som datalaget returnere. 
      public Patient_CPR GetPatientCPR()
      {
         Patient = DBaccess.LoadPatient(EKGID);
         return Patient;
      }

      //Metoden som opretter en EKGmåling, samtidig med informationsskrivning på displayet. 
      public EKG_Maaling EKGmaalingCreate()
      {
         EKGData = new double[250];

         int periode = 10;
         int samplerate = 25;
         sample = 0; 

         //antalMaalinger er antallet af målinger som ekgmåleren tager over perioden på 50 sekunder. 
         int AntalMaalinger = periode * samplerate;


         Console.WriteLine("20 sek vent");
         //Thread.Sleep(20000);
        
         for (int i = 0; i < AntalMaalinger; i++)
         {
            sample = 0; 
            sample = Convert.ToDouble((ADC.readADC_SingleEnded(0) / 2048.0) * 6.144);
            EKGData[i] = sample;

            Thread.Sleep(1000 / (Convert.ToInt32(samplerate) - 4));
         }
         maaling = new EKG_Maaling(Patient.PatientName, Patient.CPR, DateTime.Now, EKGData, "Andet", samplerate, periode,"B","double",Convert.ToInt32(EKGID));
       
         Console.WriteLine("20 sek vent");
         //Thread.Sleep(20000);

         return maaling;
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

      // Metoden skal returnere det objekt som metoden LoadHistorik() returnerer i datalaget. 
      public List<DateTime> Historik(string cpr)
      {
         try
         {
            Dato = DBaccess.LoadHistorik(cpr);
         }
         catch
         {
            Console.WriteLine("Download af historik fra DB mislykkes");
         }
         return Dato;
      }

      //Metoden skal beregne batteristatus og returnere en specifik byte alt efter brug af batteri. 
      public byte BatteristatusHent()
      {     
            int[] tid = new int[999999];
            int minut = 0;
            double batterikapacitet = 1200000; /*mA minutter*/

            tid[taeller] = DateTime.Now.Minute; 

            if (minut > 0)
            {
                //Målingen er fortaget efter 20 min brug af batteriet
                minut = 20;

                //double strøm_mA = ((ADC.readADC_SingleEnded(1) / 2048.0) * 6.144) /*V*/ / 1 /*ohm*/;
                double strøm_mA = ((20000 / 2048.0) * 6.144) / 1 ;
                double strømBrugt_mAm = strøm_mA * minut;

                batterikapacitet = (batterikapacitet - strømBrugt_mAm);
            }

            ++taeller; 
            if (batterikapacitet > 960000)
                return 5;
            else if (batterikapacitet > 720000)
                return 4;
            else if (batterikapacitet > 480000)
                return 3;
            else if (batterikapacitet > 240000)
                return 2;
            else if (batterikapacitet <= 240000)
                return 1;
            else //Burde aldrig kunne lade sig gøre. 
                return 0;

            
      }
   }
}

