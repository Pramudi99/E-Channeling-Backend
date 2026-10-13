using System.ComponentModel.DataAnnotations;

namespace E_Channeling.DTOs.Doctor
{
    public class UpdateDoctorRequest
    {
        
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Specialization { get; set; } = string.Empty;

        [Required]
        public string Qualification { get; set; } = string.Empty;

        [Range(0, 60)]
        public int Experience { get; set; }

        [Range(typeof(decimal), "0", "1000000")]
        public decimal ConsultationFee { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}

