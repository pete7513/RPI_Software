namespace DTO
{
   public class Patient_CPR
   {
      public string PatientName { get; set; }
      public string CPR { get; set; }

      public Patient_CPR(string Name, string CPR)
      {
         PatientName = Name;
         this.CPR = CPR; 
      }
   }
}
