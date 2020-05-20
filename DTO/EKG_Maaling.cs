using System;

namespace DTO
{
   public class EKG_Maaling
   {
      public string PatientName { get; set; }
      public string CPR { get; set; }
      public DateTime DateTime { get; set; }
      public double[] EKG_Data { get; set; }
      public string Dataformat { get; set; }
      public int Samplerate { get; set; }
      public int Periode { get; set; }
      public string Bin_text { get; set; }
      public string Maaletype { get; set; }
      public int EKGID { get; set; }


      public EKG_Maaling(string patient, string CPR, DateTime dateTime, double[] ekg_maalingen, string dataformat, int samplerate, int periode, string bin_text, string maaletype, int eKGID)
      {
         PatientName = patient;
         this.CPR = CPR; 
         DateTime = dateTime;
         EKG_Data = ekg_maalingen;
         Dataformat = dataformat;
         Samplerate = samplerate;
         Periode = periode;
         Bin_text = bin_text;
         Maaletype = maaletype;
         EKGID = eKGID; 
      }

   }
}
