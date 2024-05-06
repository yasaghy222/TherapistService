using FluentValidation;
using TherapistService.Data;
using TherapistService.DTOs;
using TherapistService.Models;
using Microsoft.AspNetCore.Mvc;
using TherapistService.Services;

namespace TherapistService.Controllers;

[ApiController]
[Route("[controller]")]
public class VisitPlanController(TherapistServiceContext context,
							  IValidator<VisitPlanDto> dataValidator,
							  IValidator<VisitPlanFilterDto> filterValidator) : ControllerBase
{
	private readonly VisitPlanService _service = new(context, dataValidator, filterValidator);

	[HttpGet]
	[Route("/[controller]/{therapistId}")]
	public async Task<IActionResult> Get(Guid TherapistId, [FromQuery] DateOnly fromDate, [FromQuery] DateOnly toDate)
	{
		VisitPlanFilterDto filterDto = new(TherapistId, fromDate, toDate);
		Result result = await _service.GetAll(filterDto);

		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPut]
	public async Task<IActionResult> Put(VisitPlanDto model)
	{
		Result result = await _service.Add(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Post(VisitPlanDto model)
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
