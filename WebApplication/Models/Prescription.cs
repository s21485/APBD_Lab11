using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }
    
    [Required]
    public int IdPatient { get; set; }
    
    [Required]
    public int IdDoctor { get; set; }
    
    public ICollection<Prescription_Medicament> Prescription_Medicaments { get; set; }
    
    [ForeignKey("IdDoctor")]
    public Doctor Doctor { get; set; }
    
    [ForeignKey("IdPatient")]
    public Patient Patient { get; set; }
    
}