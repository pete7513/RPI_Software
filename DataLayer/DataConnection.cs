using System;
using System.Collections.Generic;
using System.Text;
using Data;
using DTO; 

namespace Data
{
   public class DataConnection
   {
      private SqlDBDataAccess DBaccess;
      private SqliteDataAccess Liteaccess; 

      public DataConnection()
      {
         DBaccess = new SqlDBDataAccess();
         Liteaccess = new SqliteDataAccess(); 
      }

      public byte EKGMSendt(EKG_Maaling _Maaling)
      {
         try
         {
            DBaccess.EKGM_DB_Sendt(_Maaling);
            return 0; 
         }
         catch
         {
            Liteaccess.EKGM_lite_Sendt(_Maaling);
            return 1; 
         }       
      }







      public Patient_CPR PatientCPR(string EKGID)
      {
         try
         {
            //Patient_CPR item = DBaccess.loadPatient();
            //return item;
            return null; 
         }
         catch
         {
            Patient_CPR item = null;
            return item;
         }
      }
   }
}
