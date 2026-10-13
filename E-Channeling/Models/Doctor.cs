namespace E_Channeling.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Specialization { get; set; } = string.Empty;

        public string Qualification { get; set; } = string.Empty;

        public int Experience { get; set; }

        public decimal ConsultationFee { get; set; }

        public string Description { get; set; } = string.Empty;

        public User User { get; set; } = null!;
    }
}
