﻿using System;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Collections.Generic;
using System.Threading;
using DTO;
using LogicLayer;

// DebugAdapterHost.Launch /LaunchJson:"C:\Users\asbjo\source\repos\RPI_Software\RPI_Software\AFolder\launch.json" /EngineGuid:541B8A8A-6081-4506-9F0A-1CE771DEBC04 

namespace RPI_Software
{
   class Program // Controller klasse 
   {
      #region Objekt referencer og atributter
      // UI <<Boundary >> og Logic <<Controller>>
      private static Patient_Interface Interface;
      private static Logic Logic; 

      // DTO Klasser <<Domain>>
      private static EKG_Maaling maaling;
      private static Patient_CPR Patient;

      // RPI komponenter <<Boundary>>
      private static TWIST endcoder;
      private static ADC1015 ADC;

      //Atributter 
      private static short startMaaling = 0;
      private static short Time = 1;
      private static short Historik = 2;
      private static short MaksCount;
      private static short turn = 0;
      //static List<byte> EKGData;
      private static double[] EKGData;
      private static List<DateTime> history;

      #endregion

      //THE MAIN PROGRAM 
      static void Main(string[] args)
      {
         initialisere();

         StartMaaling(); 

         while (1 == 1)
         {            
            if (turn == endcoder.getCount())
            {

            }
            else
            {
               turn = endcoder.getCount();
               IsMoved();
            }
            if (endcoder.isPressed() == true) 
               IsPressed();
         }
      }

      //Metoden starter displayet op og henter patientinformationer. 
      static void initialisere()
      {
         //Objekter oprettes.
         Interface = new Patient_Interface();
         endcoder = new TWIST();
         Logic = new Logic();

         // metode til at hente patient informationer - retur værdi DTO patient
         Patient = Logic.getpatientCPR();
         endcoder.setCount(0);

         //Start sekvens vises og hovedmenuen vises efter.
         Interface.ScreenColor(255, 0, 0); 

         Interface.ShowStartMenu(Patient.PatientName);
         Interface.ShowStartMåling();
      }

      //Metoden for hvis Endcoderen er drejet
      static void IsMoved()
      {
         try
         {
            // set count og get cout  
            // Skal muligvis ændres til states // Switch
            if (endcoder.getCount() == -1) //-1
            {
               endcoder.setCount(0);
               Interface.ShowStartMåling();
               Console.WriteLine("-1"); 
            }
            else if (endcoder.getCount() == startMaaling) //0
            {
               Interface.ShowStartMåling();
               Console.WriteLine("0");
            }

            else if (endcoder.getCount() == Time) //1
            {
               Interface.ShowTime();
               Console.WriteLine("1");
            }

            else if (endcoder.getCount() == Historik) //2
            {
               Interface.ShowHistory();
               Console.WriteLine("2");
            }
            else if (endcoder.getCount() == 3) //3
            {
               endcoder.setCount(2);
               Interface.ShowHistory();
               Console.WriteLine("3");
            }
         }
         catch
         {
            Console.WriteLine("ERROR display fuckop");
         }
      }

      //Metoden for hvis Endcoderen er trykket på 
      static void IsPressed()
      {
         try
         {
            //Dette er hvis endconderen bliver trykket på
            if (endcoder.getCount() == startMaaling) //0
            {
               //Metode for sig
               StartMaaling();
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
      static void StartMaaling()
      {
         //Set backlightColor = Gul
         Interface.ScreenColor(255, 255, 0);

         //Påbegynder nedtælling 
         Interface.CountDown10();

         //Set backlightColor = Grøn
         Interface.ScreenColor(0, 255, 0);

         // Bruger information omkring at der er 50 sekunder til EKG målingen er færdig. 
         Interface.CountDown50();

         //Oprettrelse af en EKG data liste
         EKGData = Logic.EKGmaalingCreate();

         //Skærmen viser at målingen er færdig
         Interface.ReadingDone();

         //menuen vises 
         Interface.ShowStartMåling();

         //Oprettelse af nyt EKG måling opjekt med den nye liste
         maaling = new EKG_Maaling(Patient.PatientName, Patient.CPR, DateTime.Now, EKGData);

         //Forsendelse af EKG måling og retur værdien er hvilken database som EKG målingen er blevet lagt op i
         byte besked = Logic.EKGMSendt(maaling);
         Interface.Besked(besked);

         //Beskeden vises på displayet et øjeblik 
         Thread.Sleep(8000);

         //menuen vises 
         Interface.ShowStartMåling();
      }

      static void History(string CPR)
      {
            history = Logic.historik(CPR);
            Interface.ShowHistorik(history);
      }
   }
}