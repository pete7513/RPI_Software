﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
   public class EKG_Maaling
   {
      public string PatientName { get; set; }
      public string CPR { get; set; }
      public DateTime DateTime { get; set; }
      //public List<byte> EKG_Data { get; set; }
      public double[] EKG_Data;

      public EKG_Maaling(string patient, string CPR, DateTime dateTime, double[] ekg_maalingen /*List<byte> eKG_maalingen*/)
      {
         PatientName = patient;
         this.CPR = CPR; 
         DateTime = dateTime;
         EKG_Data = ekg_maalingen; 
      }

   }
}
