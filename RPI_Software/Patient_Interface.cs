using System;
using System.Collections.Generic;
using System.Text;
using RaspberryPiCore.LCD;
using RaspberryPiCore.TWIST;
using System.Threading;

namespace RPI_Software
{
    class Patient_Interface
    {
        SerLCD serLCD = new SerLCD(114);
        TWIST twist = new TWIST(63);

        public void ShowMenu()
        {
            // Når programmet starter skal denne metode kaldes, den skal tænde display og vise de forskellige menuer på det.
            serLCD.lcdDisplay();
            serLCD.lcdPrint("Velkommen - programmet starter");
            Thread.Sleep(5000);
            serLCD.lcdClear();
            serLCD.lcdPrint("Start EKG-måling");
        }

        public string Read()
        {
            // Denne metode skal registrere hvilken skærm type der er på display og aflæse denne, hvorefter den aktiverer den valgte metode
            // til det der står på displayet
                if (twist.isPressed() == true)
                {
                    return serLCD.lcdPrint("Start EKG-måling")
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
            CountDown50();
            ReadingDone();
        }

        public void CountDown50()
        {
            // Denne metode er en nedtællingsmetode der skal få displayet til at at indikere at en måling er igang
            serLCD.lcdSetBackLight(0, 255, 0);
            for (int i = 50; i > 0; i--)
            {
                serLCD.lcdPrint("Ekg-målingen er færdig om " + i + " sekunder");
                Thread.Sleep(1000);
            }
        }

        public void ReadingDone()
        {
            // Denne metode skal få displayet til at indikere at måligen er slut.
            serLCD.lcdSetBackLight(0, 0, 0);
            serLCD.lcdPrint("Ekg-måling færdig");
        }
    }
}
