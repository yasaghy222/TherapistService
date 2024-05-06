using FluentValidation;
using TherapistService.Data;
using TherapistService.DTOs;
using TherapistService.Models;
using Microsoft.AspNetCore.Mvc;
using TherapistService.Enums;
using TherapistService.Services;

namespace TherapistService.Controllers;

[ApiController]
[Route("[controller]")]
public class ClinicController(TherapistServiceContext context,
							  IValidator<ClinicDto> dataValidator) : ControllerBase
{
	private readonly ClinicService _service = new(context, dataValidator);

	[HttpPut]
	public async Task<IActionResult> Put(ClinicDto model)
	{
		Result result = await _service.Add(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Post(ClinicDto model)
	{
		Result result = await _service.Edit(model);
		return StatusCode(result.StatusCode, result.Data);
	}
}
