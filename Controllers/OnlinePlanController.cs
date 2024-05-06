using FluentValidation;
using TherapistService.Data;
using TherapistService.DTOs;
using TherapistService.Models;
using Microsoft.AspNetCore.Mvc;
using TherapistService.Services;

namespace TherapistService.Controllers;

[ApiController]
[Route("[controller]")]
public class OnlinePlanController(TherapistServiceContext context,
							  IValidator<OnlinePlanDto> dataValidator) : ControllerBase
{
	private readonly OnlinePlanService _service = new(context, dataValidator);

	[HttpGet]
	[Route("/[controller]/{therapistId}")]
	public async Task<IActionResult> Get(Guid TherapistId)
	{
		Result result = await _service.GetAll(TherapistId);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPut]
	public async Task<IActionResult> Put(OnlinePlanDto model)
	{
		Result result = await _service.Add(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Post(OnlinePlanDto model)
	{
		Result result = await _service.Edit(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpDelete]
	[Route("/[controller]/{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		Result result = await _service.Delete(id);
		return StatusCode(result.StatusCode, result.Data);
	}
}
