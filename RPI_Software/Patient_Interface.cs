using System;
using System.Collections.Generic;
using System.Text;
using RaspberryPiCore.LCD;
using RaspberryPiCore;
using RaspberryPiCore.i2cdotnet;
using System.Threading;

namespace RPI_Software
{
   public class Patient_Interface
   {
      private SerLCD Display;
      private I2c i2c; 

      public Patient_Interface()
      {
         Display = new SerLCD();
         i2c = new I2c(); 
         Display.changeAddress(80);
         Display.lcdDisplay();
         Display.lcdClear(); 
      }

      public void ShowStartMenu(string navn)
      {
         // Når programmet starter skal denne metode kaldes, den skal tænde display og vise de forskellige menuer på det.
         Console.WriteLine("Velkommen");
         Display.lcdGotoXY(0, 1);
         Display.lcdPrint("Velkommen " + navn + "  - programmet starter");
         Thread.Sleep(5000);
      }

      public void ShowStartMåling()
      {
         // Når programmet har lavet en måling kaldes den metode, som viser en skærm med den 
         Display.lcdClear();
         Display.lcdHome();

         Thread.Sleep(500);
         Display.lcdGotoXY(0, 1);
         Display.lcdPrint("Start EKG-måling");
         Display.lcdGotoXY(0, 2);
         Display.lcdPrint("Kl. " + DateTime.Now.ToShortTimeString());
         Display.lcdGotoXY(0, 3);
         Display.lcdPrint("Vis historik");
      }

      public void ShowTime()
      {
         // En metode der kan vise brugeren dato og klokken (ekstra)
         Display.lcdClear();
         Display.lcdHome();
         Thread.Sleep(500);

         Display.lcdGotoXY(0, 0);
         Display.lcdPrint("Start EKG-måling");
         Display.lcdGotoXY(0, 1);
         Display.lcdPrint("Kl. " + DateTime.Now.ToShortTimeString());
         Display.lcdGotoXY(0, 2);
         Display.lcdPrint("Vis historik");
      }

      public void ShowHistory()
      {
         // En metode der skal kunne vise brugeren tidspunktet på hvornår han har lavet en EKG-måling
         Display.lcdClear();
         Display.lcdHome();
         Thread.Sleep(500);

         Display.lcdGotoXY(0, 0);
         Display.lcdPrint("Kl. " + DateTime.Now.ToShortTimeString());
         Display.lcdGotoXY(0, 1);
         Display.lcdPrint("Vis historik");
      }

      public void CountDown10()
      {
         // Denne metode er en nedtællingsmetode der skal få displayet til at at indikere at en måling er startet, og brugeren har 10
         // sekunder til at gøre sig klar
         Display.lcdSetBackLight(255, 255, 0);
         for (int i = 10; i > 0; i--)
         {
            Display.lcdPrint("Ekg-målingen går igang om " + i + " sekunder");
            Thread.Sleep(1000);
         }
      }

      public void CountDown50()
      {
         // Denne metode er en nedtællingsmetode der skal få displayet til at at indikere at en måling er igang   
         Display.lcdPrint("Ekg-målingen er færdig om 50 sekunder");
         Display.lcdPrint("Du bedes venligts forholde dig i ro");
      }

      public void ScreenColor(byte red, byte green, byte blue)
      {
         Display.lcdSetBackLight(red, green, blue);
      }

      public void Besked(byte beskedNummer)
      {
         if (beskedNummer == 0)
         {
            Display.lcdDisplay();
            Display.lcdPrint("Din måling er afsendt til den online Data base");
         }
         if (beskedNummer == 1)
         {
            Display.lcdDisplay();
            Display.lcdPrint("Din måling er afsendt til den lokale Data base");
         }
         if (beskedNummer == 2)
         {
            Display.lcdDisplay();
            Display.lcdPrint("Din måling er ikke afsendt til den online eller den lokale Data base");
         }
      }

      public void ReadingDone()
      {
         // Denne metode skal få displayet til at indikere at måligen er slut.
         Display.lcdClear();
         Display.lcdPrint("Ekg-måling færdig");
         Thread.Sleep(8000);
      }

   }
}
