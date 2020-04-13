using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
   public class EKG_Maaling
   {
      public string PatientName { get; set; }
      public string CPR { get; set; }
      public DateTime DateTime { get; set; }
      public List<byte> EKG_maalingen { get; set; }

      public EKG_Maaling(string patient, DateTime dateTime, List<byte> eKG_maalingen)
      {
         NamePatient = patient;
         DateTime = dateTime;
         EKG_maalingen = eKG_maalingen; 
      }

   }
}
