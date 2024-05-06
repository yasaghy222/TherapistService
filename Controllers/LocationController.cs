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
public class LocationController(TherapistServiceContext context,
							  IValidator<LocationDto> dataValidator) : ControllerBase
{
	private readonly LocationService _service = new(context, dataValidator);

	[HttpGet]
	public async Task<IActionResult> Get(Guid? parentId)
	{
		Result result = await _service.GetAll(parentId);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPut]
	public async Task<IActionResult> Put(LocationDto model)
	{
		Result result = await _service.Add(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Post(LocationDto model)
	{
		Result result = await _service.Edit(model);
		return StatusCode(result.StatusCode, result.Data);
	}
}
