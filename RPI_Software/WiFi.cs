using System;
using System.Collections.Generic;
using System.Text;

namespace RPI_Software
{
   class WiFi
   {
      public WiFi()
      {



      }

      public string Patient(string ID)
      {
         //Her henter 
         List<string> Navneliste = SqliteDataAccess.patient()
         
         return "name"; 


      }

      public void EKGMSendt(EKG_Maaling _Maaling)
      { 
         //Kode som kan sende EKG måling til database. 

         // Listen skal dog laves om til en bytearray før den afstedes 

      }



   }
}
