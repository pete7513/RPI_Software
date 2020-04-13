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
        public SerLCD serLCD = new SerLCD(114);
        public TWIST twist = new TWIST(63);


        public void StartMenu(string navn)
        {
            // Når programmet starter skal denne metode kaldes, den skal tænde display og vise de forskellige menuer på det.
            serLCD.lcdDisplay();
            serLCD.lcdPrint("Velkommen " + navn + "  - programmet starter");
            Thread.Sleep(5000);
            serLCD.lcdClear();
            serLCD.lcdPrint("Start EKG-måling");
        }

        public void ActiveMenu()
        {
            // Når programmet har lavet en måling kaldes den metode, som viser en skærm med den 
            serLCD.lcdDisplay();
            serLCD.lcdPrint("Start EKG-måling");
        }

        public void ShowTime()
        {
            // En metode der kan vise brugeren dato og klokken (ekstra)
            serLCD.lcdPrint(DateTime.Now.ToString());
        }


        public void ShowHistory()
        {
            // En metode der skal kunne vise brugeren tidspunktet på hvornår han har lavet en EKG-måling
            serLCD.lcdPrint("");
        }

        public void Read()
        {
            // Denne metode skal registrere hvilken skærm type der er på display og aflæse denne, hvorefter den aktiverer den valgte metode
            // til det der står på displayet

            if (twist.isPressed() == true)
            {
                string state = "choosen screen";
                switch (state)
                {
                    case serLCD.lcdPrint("Start EKG-måling"):
                        //
                        break;
                }
            }
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
            serLCD.lcdSetBackLight(0, 255, 0);
            serLCD.lcdPrint("Ekg-målingen er færdig om " + tal + " sekunder");
        }

        public void ReadingDone()
        {
            // Denne metode skal få displayet til at indikere at måligen er slut.
            serLCD.lcdClear();
            serLCD.lcdPrint("Ekg-måling færdig");
        }
    }
}
