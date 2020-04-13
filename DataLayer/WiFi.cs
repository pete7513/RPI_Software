using System;
using System.Collections.Generic;
using System.Text;
using Data;
using DTO; 

namespace RPI_Software
{
   class WiFi
   {
      private SqlDBDataAccess access; 
      public WiFi()
      {
         access = new SqlDBDataAccess();
      }

      public void EKGMSendt(EKG_Maaling _Maaling)
      {
         if (/*internetconetion = true*/)
            SqlDBDataAccess.//funktion; 
         else
            SqliteDataAccess.EKGMSendt(_Maaling);

      }

      public Patient_CPR PatientCPR(string EKGID)
      {
         access.loadPatient();

         Patient_CPR item = access.loadPatient();

         return item; 
      }
   }
}
