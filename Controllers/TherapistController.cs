using FluentValidation;
using TherapistService.Data;
using TherapistService.DTOs;
using TherapistService.Models;
using Microsoft.AspNetCore.Mvc;
using TherapistService.Enums;

namespace TherapistService.Controllers;

[ApiController]
[Route("[controller]")]
public class TherapistController(TherapistServiceContext context,
							  IValidator<AddTherapistDto> addValidator,
							  IValidator<EditTherapistDto> editValidator) : ControllerBase
{
	private readonly TherapistService _service = new(context, addValidator, editValidator);

	[HttpGet]
	[Route("/[controller]/{type}/{id}")]
	public async Task<IActionResult> Get(GetTherapistType type, Guid id)
	{
		Result result = type switch
		{
			GetTherapistType.Info => await _service.GetInfo(id),
			GetTherapistType.Detail => await _service.GetDetail(id),
			_ => await _service.GetInfo(id)
		};
		return StatusCode(result.StatusCode, result.Data);
	}
	[HttpGet]
	[Route("/[controller]/{type}")]
	public async Task<IActionResult> Get(GetTherapistType type,
																	[FromQuery] TherapistServiceType serviceType,
																	[FromQuery] TherapistFilterOrder order,
																	[FromQuery] Guid? locationId)
	{
		TherapistFilterDto filterDto = new(serviceType, order, locationId);

		Result result = type switch
		{
			GetTherapistType.Info => await _service.GetAllInfo(filterDto),
			GetTherapistType.Detail => await _service.GetAllDetails(filterDto),
			GetTherapistType.Recommended => await _service.GetAllRecommends(),
			_ => await _service.GetAllInfo(filterDto)
		};
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPut]
	public async Task<IActionResult> Put([FromForm] AddTherapistDto model)
	{
		Result result = await _service.Add(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromForm] EditTherapistDto model)
	{
		Result result = await _service.Edit(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPatch]
	[Route("/[controller]/Status/{id}/{status}")]
	public async Task<IActionResult> Patch(Guid id, TherapistStatus status)
	{
		Result result = await _service.ChangeStatus(id, status);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPatch]
	[Route("/[controller]/lineStatus/{id}/{status}")]
	public async Task<IActionResult> Patch(Guid id, TherapistLineStatus status)
	{
		Result result = await _service.ChangeLineStatus(id, status);
		return StatusCode(result.StatusCode, result.Data);
	}
}
