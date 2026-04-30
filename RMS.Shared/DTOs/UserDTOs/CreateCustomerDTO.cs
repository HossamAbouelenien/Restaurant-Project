namespace RMS.Shared.DTOs.UserDTOs
{
    public class CreateCustomerDTO
    {
        
        public string Name { get; set; }= string.Empty;
        public string PhoneNumber { get; set; }=string.Empty;

        public int? BranchId { get; set; }
    }
}
