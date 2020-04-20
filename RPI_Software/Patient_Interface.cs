using System;
using System.Collections.Generic;
using System.Text;
using RaspberryPiCore.LCD;
using RaspberryPiCore.TWIST;
using System.Threading;
using System.Timers;

namespace RPI_Software
{
   public class Patient_Interface
   {
      public SerLCD serLCD; 

      public Patient_Interface()
      {
         serLCD = new SerLCD();
      }

      public void ShowStartMenu(string navn)
      {
         // Når programmet starter skal denne metode kaldes, den skal tænde display og vise de forskellige menuer på det.
         serLCD.lcdDisplay();
         Console.WriteLine("Display");
         Thread.Sleep(5000);
         serLCD.lcdHome();
         serLCD.lcdGotoXY(0, 1);
         Console.WriteLine("Velkommen");
         serLCD.lcdPrint("Velkommen " + navn + "  - programmet starter");
         Thread.Sleep(5000);
         serLCD.lcdClear();
      }

      public void ShowStartMåling()
      {
         // Når programmet har lavet en måling kaldes den metode, som viser en skærm med den 
         serLCD.lcdDisplay();
         Thread.Sleep(500); 
         serLCD.lcdGotoXY(0, 1);
         serLCD.lcdPrint("Start EKG-måling");
         serLCD.lcdGotoXY(0, 2);
         serLCD.lcdPrint("Kl. " + Time());
         serLCD.lcdGotoXY(0, 3);
         serLCD.lcdPrint("Vis historik"); 
      }

      public void ShowTime()
      {
         // En metode der kan vise brugeren dato og klokken (ekstra)
         serLCD.lcdDisplay();
         Thread.Sleep(500);

         serLCD.lcdGotoXY(0, 0);
         serLCD.lcdPrint("Start EKG-måling");
         serLCD.lcdGotoXY(0, 1);
         serLCD.lcdPrint("Kl. "+Time());
         serLCD.lcdGotoXY(0, 2);
         serLCD.lcdPrint("Vis historik");
      }

      public string Time()
      { 
         return DateTime.Now.ToShortTimeString();
      }

      public void ShowHistory()
      {
         // En metode der skal kunne vise brugeren tidspunktet på hvornår han har lavet en EKG-måling
         serLCD.lcdDisplay();
         Thread.Sleep(500);
     
         serLCD.lcdGotoXY(0, 0);
         serLCD.lcdPrint("Kl. " + Time());
         serLCD.lcdGotoXY(0, 1);
         serLCD.lcdPrint("Vis historik");
      }

      public void CountDown10()
      {
         // Denne metode er en nedtællingsmetode der skal få displayet til at at indikere at en måling er startet, og brugeren har 10
         // sekunder til at gøre sig klar
         serLCD.lcdSetBackLight(255, 255, 0);
         for (int i = 10; i > 0; i--)
         {
            serLCD.lcdPrint("Ekg-målingen går igang om " + i + " sekunder");
            Thread.Sleep(1000);
         }
      }

      public void CountDown50(byte tal)
      {
         // Denne metode er en nedtællingsmetode der skal få displayet til at at indikere at en måling er igang   
         serLCD.lcdPrint("Ekg-målingen er færdig om " + tal + " sekunder");
      }

      public void screenColor(byte red, byte green, byte blue)
      {
         serLCD.lcdSetBackLight(red, green, blue); 
      }

      public void Besked(byte beskedNummer)
      {
         if (beskedNummer == 0)
         {
            serLCD.lcdDisplay();
            serLCD.lcdPrint("Din måling er afsendt til den online Data base"); 
         }
         if (beskedNummer == 1)
         {
            serLCD.lcdDisplay();
            serLCD.lcdPrint("Din måling er afsendt til den lokale Data base");
         }
         if (beskedNummer == 2)
         {
            serLCD.lcdDisplay();
            serLCD.lcdPrint("Din måling er ikke afsendt til den online Data base");
         }
      }

      public void ReadingDone()
      {
         // Denne metode skal få displayet til at indikere at måligen er slut.
         serLCD.lcdClear();
         serLCD.lcdPrint("Ekg-måling færdig");
         Thread.Sleep(8000); 
      }





      // Denne metode er slettet, da read() funktionen ligger i controller klassen 
      //public void Read()
      //{
      //   Denne metode skal registrere hvilken skærm type der er på display og aflæse denne, hvorefter den aktiverer den valgte metode
      //   til det der står på displayet

      //   if (twist.isPressed() == true)
      //   {
      //      string state = "choosen screen";
      //      switch (state)
      //      {
      //         case serLCD.lcdPrint("Start EKG-måling"):

      //            break;
      //      }
      //   }
      //}
   }
}
