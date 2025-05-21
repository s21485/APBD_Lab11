
using Microsoft.AspNetCore.Mvc;
using WebApplication.DTOs;
using WebApplication.Service;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpGet("Patient/{id}")]
        public async Task<ActionResult<GetPatientDto>> GetPatientDetails(int id)
        {
            return await _prescriptionService.GetPatientDetails(id);
        }

        [HttpPost]
        public async Task<ActionResult> AddPrescription(PrescriptionDTO prescriptionDto)
        {
            return await _prescriptionService.AddPrescription(prescriptionDto);
        }
    }
}