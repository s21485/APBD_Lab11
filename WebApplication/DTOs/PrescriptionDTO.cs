namespace WebApplication.DTOs;

public class PrescriptionDTO
{
    public PatientDto Patient { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public DoctorDto Doctor { get; set; }
    public List<PrescriptionMedicamentDto> Medicaments { get; set; }
}