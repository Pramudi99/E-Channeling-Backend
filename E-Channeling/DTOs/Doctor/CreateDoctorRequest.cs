namespace E_Channeling.DTOs.Doctor
{
    public class CreateDoctorRequest
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;

        public string Qualification { get; set; } = string.Empty;

        public int Experience { get; set; }

        public decimal ConsultationFee { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
