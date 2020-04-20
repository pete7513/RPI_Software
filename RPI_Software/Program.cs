using System;
using RaspberryPiCore.ADC;
using RaspberryPiCore.TWIST;
using RaspberryPiCore.LCD;
using System.Collections.Generic;
using System.Threading;
using DTO;
using Data;

namespace RPI_Software
{
    class Program
    {
      static ADC1015 ADC; 
      static Patient_Interface UI_Interface;
      static Patient_CPR Patient;
      static DataConnection dataConnection;
      static EKG_Maaling maaling; 

        static void Main(string[] args)
        {
         initialitiere();
         //string state = "state";

         //while (1 == 1)
         //{
         //   if (UI_Interface.twist.isMoved() == true)
         //   { 
         //      // set count og get cout   



         //   }

         //   if (UI_Interface.twist.isPressed() == true) 
         //   {
         //      //state = UI_Interface.Read();
               
         //      if (state == "start måling")
         //      {
         //         UI_Interface.CountDown10(); 
         //         maaling.EKG_Data = EKGmaalingCreate();
         //         dataConnection.EKGMSendt(maaling);
         //         UI_Interface.ShowMenu(Patient.PatientName) ;
         //      }
         //   }
         //}

        }


      public static void initialitiere()
      {
         //string EKGID = "1011";
         UI_Interface = new Patient_Interface();
         //dataConnection = new DataConnection();

         //Patient = dataConnection.PatientCPR(EKGID);

         //maaling = new EKG_Maaling(Patient.PatientName, Patient.CPR, DateTime.Now, null);

         //UI_Interface.ShowMenu(Patient.PatientName); 

         UI_Interface.ShowMenu("name"); 
      }


      public static List<byte> EKGmaalingCreate()
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
               UI_Interface.CountDown50(nedtællingstal);
               malingtæller = 0;
            }
         }
         UI_Interface.ReadingDone(); 

         return byteliste; 
      }
    }
}
