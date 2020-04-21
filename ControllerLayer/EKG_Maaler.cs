using System;
using System.Collections.Generic;
using System.Text;
using System.Threading; 
using RPI_Software;
using DTO;
using Data;
using Dapper;
using RaspberryPiCore.ADC;
using RaspberryPiCore.i2cdotnet;
using RaspberryPiCore.LCD;
using RaspberryPiCore.TWIST;


namespace ControllerLayer
{
   class EKG_Maaler
   {
      #region Objekt referencer og atributter
      // UI og DB-UI <<Boundary>>
      private Patient_Interface Interface;
      private DataConnection dataConnection;

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
      public EKG_Maaler()
      {
         Interface = new Patient_Interface();
         dataConnection = new DataConnection();
         endcoder = new TWIST();
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

      // Her kører programmet i
      public void Main()
      {
         initialisere();

         while (endcoder.getCount() < 5)
         {
            if (endcoder.isMoved() == true)
            {
               IsMoved();
            }
            if (endcoder.isPressed() == true)
            {
               IsPressed();
            }
         }
      }

      //Initialitering af displayet med navn på patienten, tilhørende EKG måleren. 
      public void initialisere()
      {
         // metode til at hente patient informationer - retur værdi DTO patient
         Patient = dataConnection.getPatientCPR(EKGID);

         //Start sekvens vises og hovedmenuen vises efter. 
         Interface.ShowStartMenu(Patient.PatientName);
         Interface.ShowStartMåling();
      }

      //Metoden for hvis Endcoderen er drejet
      public void IsMoved()
      {
         try
         {
            // set count og get cout  
            // Skal muligvis ændres til states // Switch
            if (endcoder.getCount() == -1) //-1
            {
               endcoder.setCount(-1);
               Interface.ShowStartMåling();
            }
            else if (endcoder.getCount() == StartMaaling) //0
            {
               Interface.ShowStartMåling();
            }

            else if (endcoder.getCount() == Time) //1
            {
               Interface.ShowTime();
            }

            else if (endcoder.getCount() == Historik) //2
            {
               Interface.ShowHistory();
            }
            else if (endcoder.getCount() == 3) //3
            {
               endcoder.setCount(2);
               Interface.ShowHistory();
            }
         }
         catch
         {
            Console.WriteLine("ERROR display fuckop");
         }
      }

      //Metoden for hvis Endcoderen er trykket på 
      public void IsPressed()
      {
         try
         {
            //Dette er hvis endconderen bliver trykket på
            if (endcoder.getCount() == StartMaaling) //0
            {
               //Metode for sig
               StartMåling();
            }

            else if (endcoder.getCount() == Time) //1
            {
               // Her skal tiden blot vises men ved at trykke er der ingen funktion. 
            }

            else if (endcoder.getCount() == Historik) //2
            {
               // Her skal skrives noget kode ift. at kunne vise en historik. 
            }
         }
         catch
         {
            Console.WriteLine("ERROR - Endcoder pressed fail"); 
         }
      }

      //Metoden er den metode som bliver kaldt når der bliver trykket "start måling" på displayet
      public void StartMåling()
      {
         //Set backlightColor = Gul
         Interface.ScreenColor(255, 255, 0);

         //Påbegynder nedtælling 
         Interface.CountDown10();

         //Set backlightColor = Grøn
         Interface.ScreenColor(0, 255, 0);

         //Oprettrelse af en EKG data liste
         EKGData = EKGmaalingCreate();

         //menuen vises 
         Interface.ShowStartMåling();

         //Oprettelse af nyt EKG måling opjekt med den nye liste
         maaling = new EKG_Maaling(Patient.PatientName, Patient.CPR, DateTime.Now, EKGData);
         try
         {
            //Forsendelse af EKG måling og retur værdien er hvilken database som EKG målingen er blevet lagt op i
            byte besked = dataConnection.EKGMSendt(maaling);
            Interface.Besked(besked);
         }
         catch
         {
            Interface.Besked(2);
         }

         //Beskeden vises på displayet et øjeblik 
         Thread.Sleep(8000);

         //menuen vises 
         Interface.ShowStartMåling();
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
         Interface.ReadingDone();
         return byteliste;
      }

   }
}
