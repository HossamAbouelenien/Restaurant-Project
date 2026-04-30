namespace RMS.Shared.DTOs.TableDTOs
{
   public class CreateTableDTO
    {
         
      
        public int BranchId { get; set; }
        public string TableNumber { get; set; } = default!;
        public int Capacity { get; set; }
    }
}

