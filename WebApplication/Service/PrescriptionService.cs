using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.DTOs;

namespace WebApplication.Service;

public class PrescriptionService : IPrescriptionService
{
        private DatabaseContext _context;

    public PrescriptionService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ActionResult<GetPatientDto>> GetPatientDetails(int id)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Prescription_Medicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == id);

        if (patient == null)
        {
            return new NotFoundResult();
        }

        var patientDto = new GetPatientDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = patient.Prescriptions.OrderBy(p => p.DueDate)
                .Select(p => new GetPrescriptionDto()
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date,
                    DueDate = p.DueDate,
                    Doctor = new GetDoctorDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName,
                        LastName = p.Doctor.LastName,
                    },
                    Medicaments = p.Prescription_Medicaments
                        .Select(pm => new GetMedicamentDto
                        {
                            IdMedicament = pm.Medicament.IdMedicament,
                            Name = pm.Medicament.Name,
                            Description = pm.Medicament.Description,
                            Dose = pm.Dose
                        }).ToList()
                }).ToList()
        };
        return new OkObjectResult(patientDto);
    }

    public async Task<ActionResult> AddPrescription(PrescriptionDTO prescriptionDto)
    {
        if (prescriptionDto.DueDate < prescriptionDto.Date)
        {
            return new BadRequestObjectResult("Data ważności nie może być wcześniej niż data wystawienia.");
        }

        if (prescriptionDto.Medicaments.Count > 10)
        {
            return new BadRequestObjectResult("Przekroczone maksymalną liczbę leków (10) na recepcie.");
        }
        
        var medicamentIds = prescriptionDto.Medicaments.Select(m => m.IdMedicament).ToList();
        var existingMedicaments = await _context.Medicaments
            .Where(m => medicamentIds.Contains(m.IdMedicament))
            .Select(m => m.IdMedicament)
            .ToListAsync();
        if (existingMedicaments.Count != medicamentIds.Count)
        {
            return new BadRequestObjectResult("Któreś z podanych leków nie istnieją w bazie danych.");
        }

        var doctor = await _context.Doctors.FindAsync(prescriptionDto.Doctor.IdDoctor);
        if (doctor == null)
            return new BadRequestObjectResult("Doktor o podanym ID nie istnieje.");
        
        var patient = await _context.Patients.FirstOrDefaultAsync(p =>
            prescriptionDto.Patient.IdPatient.HasValue && 
            p.IdPatient == prescriptionDto.Patient.IdPatient);

        if (patient == null)
        {
            patient = new Models.Patient
            {
                FirstName = prescriptionDto.Patient.FirstName,
                LastName = prescriptionDto.Patient.LastName,
                BirthDate = prescriptionDto.Patient.BirthDate
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        var prescription = new Models.Prescription
        {
            Date = prescriptionDto.Date,
            DueDate = prescriptionDto.DueDate,
            IdDoctor = prescriptionDto.Doctor.IdDoctor,
            IdPatient = patient.IdPatient
        };
        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        foreach (var medicamentDto in prescriptionDto.Medicaments)
        {
            var medicament = new Models.Prescription_Medicament()
            {
                IdMedicament = medicamentDto.IdMedicament,
                Dose = medicamentDto.Dose,
                Details = medicamentDto.Details,
                IdPrescription = prescription.IdPrescription
            };
            _context.Prescription_Medicaments.Add(medicament);
        }
        
        await _context.SaveChangesAsync();
        return new CreatedAtActionResult("GetPatientDetails", "Prescription", new {id = patient.IdPatient}, "Recepta dodana.");
    }
    
}