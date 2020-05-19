using System;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Collections.Generic;
using System.Threading;
using DTO;
using LogicLayer;

//Using RPI; 

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

      //Atributter 
      private static short startMaaling = 0;
      private static short Time = 1;
      private static short Historik = 2;
      private static short turn = 0;
      private static List<DateTime> history;

      //RPI batteristatus 
      //RPI rpi = new RPI(); 
      //Key knap = new Key(rpi); 
      //Led LD1 = new Led()
      //Led LD2 = new Led()
      //Led LD3 = new Led()
      //Led LD4 = new Led()
      //Led LD5 = new Led()

      #endregion

      //THE MAIN PROGRAM 
      static void Main(string[] args)
      {
         initialisere();

         while (1 == 1)
         {
            //if (Knap.ispressed == true)
            //{ 
                 Batteristatus();
            //}

            History(Patient.CPR); 
            Console.WriteLine("Start maaling");
            StartMaaling();

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

         //Start sekvens vises og hovedmenuen vises efter.
         Interface.ScreenColor(255, 255, 0);
         endcoder.setCount(0);

         //metode til at hente patient informationer - retur værdi DTO patient
         Patient = Logic.getpatientCPR(); 

         Console.WriteLine("Velkommen " + Patient.PatientName); 
         Interface.ShowStartMenu(Patient.PatientName);
         Interface.ShowStartMaaling();
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
               Interface.ShowStartMaaling();
               Console.WriteLine("-1"); 
            }
            else if (endcoder.getCount() == startMaaling) //0
            {
               Interface.ShowStartMaaling();
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
            Console.WriteLine("Connection to display failed.");
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
               Console.WriteLine("Start måling er trykket");
               StartMaaling();
            }

            else if (endcoder.getCount() == Time) //1
            {
               // Her skal tiden blot vises men ved at trykke er der ingen funktion. 
            }

            else if (endcoder.getCount() == Historik) //2
            {
               Console.WriteLine("Vis historik er trykket");
               History(Patient.CPR); 
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
         Console.WriteLine("Baggrundsfarve gul");
         Interface.ScreenColor(255, 255, 0);

         //Påbegynder nedtælling 
         Console.WriteLine("10 sekunder vent");
         Interface.CountDown10();

         //Set backlightColor = Grøn
         Console.WriteLine("Baggrundsfarve grøm");
         Interface.ScreenColor(0, 255, 0);

         // Bruger information omkring at der er 50 sekunder til EKG målingen er færdig. 
         Console.WriteLine("50 sekunder vent ");
         Interface.CountDown50();

         //Oprettrelse af en EKG objekt
         Console.WriteLine("Oprettelse af EKGMaaling");
         maaling = Logic.EKGmaalingCreate();

         //Set backlightColor = rød
         Console.WriteLine("Baggrundsfarve rød");
         Interface.ScreenColor(255, 0, 0);

         //Skærmen viser at målingen er færdig
         Console.WriteLine("Reading done");
         Console.WriteLine("8 sekunder vent"); 
         Interface.ReadingDone();

         //Forsendelse af EKG måling og retur værdien er hvilken database som EKG målingen er blevet lagt op i
         //returværiden placeres i besked metode til interfacet.
         Console.WriteLine("EKGMaaling afsendelse");
         Interface.Besked(Logic.EKGMSendt(maaling));


         //menuen vises 
         Console.WriteLine("Start maling menu vises");
         Interface.ShowStartMaaling();
      }

      //Metoden er den metode som bliver kaldt når der bliver trykket "Vis historik" på displayet
      static void History(string CPR)
      {
            history = Logic.historik(CPR);
            Interface.ShowHistorik(history);
      }

      //Skal tænde for et specifik antal LED'er alt efter hvilket byte "tal"
      static void Batteristatus()
      {
            byte tal = Logic.BatteristatusHent();
            switch (tal)
            {
                case 5:
                    //LD1.on(),LD2.on(),LD3.on(),LD4.on(),LD5.on();
                    Console.WriteLine("5 Led'er lyser");
                    break;
                case 4:
                    //LD1.on(),LD2.on(),LD3.on(),LD4.on(),LD5.off();
                    Console.WriteLine("4 Led'er lyser");
                    break;
                case 3:
                    //LD1.on(),LD2.on(),LD3.on(),LD4.off(),LD5.off();
                    Console.WriteLine("3 Led'er lyser");
                    break;
                case 2:
                    //LD1.on(),LD2.on(),LD3.off(),LD4.off(),LD5.off();
                    Console.WriteLine("2 Led'er lyser");
                    break;
                case 1:
                    //LD1.on(),LD2.off(),LD3.off(),LD4.off(),LD5.off();
                    Console.WriteLine("1 Led'er lyser");
                    break;
                case 0:
                    //LD1.off(),LD2.off(),LD3.off(),LD4.off(),LD5.off();
                    Console.WriteLine("0 Led'er lyser");
                    break;
            }
        }
   }
}