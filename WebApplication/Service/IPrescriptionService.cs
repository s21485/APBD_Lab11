using Microsoft.AspNetCore.Mvc;
using WebApplication.DTOs;

namespace WebApplication.Service;

public interface IPrescriptionService
{
    Task<ActionResult<GetPatientDto>> GetPatientDetails(int id);
    Task<ActionResult> AddPrescription(PrescriptionDTO prescription);
}