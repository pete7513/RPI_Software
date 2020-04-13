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


        public void ShowMenu(string navn )
        {
            // Når programmet starter skal denne metode kaldes, den skal tænde display og vise de forskellige menuer på det.
            serLCD.lcdDisplay();
            serLCD.lcdPrint("Velkommen "+navn+"  - programmet starter");
            Thread.Sleep(5000);
            serLCD.lcdClear();
            serLCD.lcdPrint("Start EKG-måling");
        }

        public void Read()
        {
            // Denne metode skal registrere hvilken skærm type der er på display og aflæse denne, hvorefter den aktiverer den valgte metode
            // til det der står på displayet
            if (twist.isPressed() == true)
            {
            }
         /*            switch (_tilstand)
 {
     case Tilstand.STARTET:
         break;

     case Tilstand.STOPPET:
         _timer.Start();
         _tilstand = Tilstand.STARTET;
         break;
 } */
         twist.isMoved();
        }

        public void CountDown10()
        {
            //// Create a timer with a two second interval.
            //aTimer = new System.Timers.Timer(2000);
            //// Hook up the Elapsed event for the timer. 
            //aTimer.Elapsed += OnTimedEvent;
            //aTimer.AutoReset = true;
            //aTimer.Enabled = true;
            // Denne metode er en nedtællingsmetode der skal få displayet til at at indikere at en måling er startet, og brugeren har 10
            // sekunder til at gøre sig klar
            int counter = 10;
            //aTimer.Interval = 1000;

            serLCD.lcdSetBackLight(255, 255, 0);
            for (int i = 10; i > 0; i--)
            {
                serLCD.lcdPrint("Ekg-målingen går igang om " + i + " sekunder");
                Thread.Sleep(1000);
            }
       
        }

        //private int counter = 60;
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    int counter = 60;
        //    timer1 = new Timer();
        //    timer1.Tick += new EventHandler(timer1_Tick);
        //    timer1.Interval = 1000; // 1 second
        //    timer1.Start();
        //    label1.Text = counter.ToString();
        //}

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
