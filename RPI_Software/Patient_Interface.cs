using System;
using System.Collections.Generic;
using System.Text;
using RaspberryPiCore.LCD;
using RaspberryPiCore;
using System.Threading;

namespace RPI_Software
{
   public class Patient_Interface
   {
      private SerLCD Display;
    
      public Patient_Interface()
      {
         Display = new SerLCD();
      }

      public void ShowStartMenu(string navn)
      {
         // Når programmet starter skal denne metode kaldes, den skal tænde display og vise de forskellige menuer på det.
         Display.lcdDisplay();
         Display.lcdGotoXY(0, 0);
         Display.lcdGotoXY(0, 0);


         Console.WriteLine("Velkommen");
         Display.lcdGotoXY(0, 1);
         Display.lcdGotoXY(0, 1);
         Display.lcdPrint("    Velkommen " + navn);
         Display.lcdGotoXY(0, 2);
         Display.lcdGotoXY(0, 2);
         Display.lcdPrint("   Programmet starter...");

         Thread.Sleep(5000);
         Display.lcdClear(); 
      }

      public void ShowStartMaaling()
      {
         // Når programmet har lavet en måling kaldes den metode, som viser en skærm med den 
         Display.lcdClear();
         Display.lcdHome();

         Thread.Sleep(500);
         Display.lcdGotoXY(0, 1);
         Display.lcdPrint(" - Start EKG-maaling");
         Display.lcdGotoXY(0, 2);
         Display.lcdPrint("   Kl. " + DateTime.Now.ToShortTimeString());
         Display.lcdGotoXY(0, 3);
         Display.lcdPrint("   Vis historik");
      }

      public void ShowTime()
      {
         // En metode der kan vise brugeren dato og klokken (ekstra)
         Display.lcdClear();
         Display.lcdHome();
         Thread.Sleep(500);

         Display.lcdGotoXY(0, 0);
         Display.lcdPrint("   Start EKG-maaling");
         Display.lcdGotoXY(0, 1);
         Display.lcdPrint(" - Kl. " + DateTime.Now.ToShortTimeString());
         Display.lcdGotoXY(0, 2);
         Display.lcdPrint("   Vis historik");
      }

      public void ShowHistory()
      {
         // En metode der skal kunne vise brugeren tidspunktet på hvornår han har lavet en EKG-måling
         Display.lcdClear();
         Display.lcdHome();
         Thread.Sleep(500);

         Display.lcdGotoXY(0, 0);
         Display.lcdPrint("   Kl. " + DateTime.Now.ToShortTimeString());
         Display.lcdGotoXY(0, 1);
         Display.lcdPrint(" - Vis historik");
      }

      public void CountDown10()
      {
         // Denne metode er en nedtællingsmetode der skal få displayet til at at indikere at en måling er startet, og brugeren har 10
         // sekunder til at gøre sig klar
         for (int i = 10; i > 0; i--)
         {
            Display.lcdGotoXY(0, 1);
            Display.lcdPrint("Ekg-maalingen gaar igang");
            Display.lcdGotoXY(0, 2);
            Display.lcdPrint("om " + i + " sekunder"); 
            Thread.Sleep(1000);
         }
      }

      public void CountDown50()
      {
         // Denne metode er en nedtællingsmetode der skal få displayet til at at indikere at en måling er igang   

         Display.lcdGotoXY(0, 1);
         Display.lcdPrint("Ekg-maalingen er faerdig om 50 sekunder");
         Display.lcdGotoXY(0, 2);
         Display.lcdPrint("Du bedes venligst forholde dig i ro");
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
            Console.WriteLine("Afsendelse til Online DB lykkes");
            Display.lcdGotoXY(0, 1);
            Display.lcdPrint("Din maaling er afsendt");
            Display.lcdGotoXY(0, 2);
            Display.lcdPrint("til den online Data base");
         }
         if (beskedNummer == 1)
         {
            Display.lcdGotoXY(0, 1);
            Display.lcdPrint("Din maaling er afsendt");
            Display.lcdGotoXY(0, 2);
            Display.lcdPrint("til den lokale Data base");
         }
         if (beskedNummer == 2)
         {
            Display.lcdGotoXY(0, 1); 
            Display.lcdPrint("Din maaling er ");
            Display.lcdGotoXY(0, 2);
            Display.lcdPrint("ikke afsendt");
         }

         //Beskeden vises på displayet et øjeblik 
         Thread.Sleep(8000);
      }

      public void ReadingDone()
      {
         // Denne metode skal få displayet til at indikere at måligen er slut.
         Display.lcdClear();
         Display.lcdPrint(" Din Ekg-maaling er færdig");
         Thread.Sleep(8000);
      }

      public void ShowHistorik(List<DateTime> dato)
      {
         // Denne metode skal få displayet til at vise de 3 sidste målinger
         Display.lcdClear();
         byte i = 1;
         foreach (DateTime item in dato)
         {
            Display.lcdGotoXY(0, i);
            Display.lcdPrint(item.ToString());
            i++;
         }

      }

   }
}
