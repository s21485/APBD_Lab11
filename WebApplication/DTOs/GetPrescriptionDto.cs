namespace WebApplication.DTOs;

public class GetPrescriptionDto
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public GetDoctorDto Doctor { get; set; }
    public List<GetMedicamentDto> Medicaments { get; set; }
}