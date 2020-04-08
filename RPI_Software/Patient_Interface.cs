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
        SerLCD serLCD = new SerLCD();
        TWIST twist = new TWIST();

        public void ShowMenu()
        {
            // Når programmet starter skal denne metode kaldes, den skal tænde display og vise de forskellige menuer på det.
            serLCD.lcdDisplay();
            serLCD.lcdPrint("Velkommen - programmet starter");
            Thread.Sleep(5000);
            serLCD.lcdClear();
            serLCD.lcdPrint("Start EKG-måling");
        }

        public void Read()
        {
            // Denne metode skal registrere hvilken skærm type der er på display og aflæse denne, hvorefter den aktiverer den valgte metode
            // til det der står på displayet
            string screen = "Start EKG-måling";
            if (twist.isPressed() == true)
            {

            }
            
        }

        public int CountDown10()
        { 
            // Denne metode er en nedtællingsmetode der skal få displayet til at at indikere at en måling er startet, og brugeren har 10
            // sekunder til at gøre sig klar
        }

        public int CountDown50()
        {
            // Denne metode er en nedtællingsmetode der skal få displayet til at at indikere at en måling er igang, og brugeren har 10
            // sekunder til at gøre sig klar
        }

        public void ReadingDone()
        { 
            // Denne metode skal få displayet til at indikere at måligen er slut.
        }


    }
}
