using TherapistService.Data;
using TherapistService.DTOs;
using TherapistService.Entities;
using TherapistService.Interfaces;
using TherapistService.Models;
using TherapistService.Shared;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace TherapistService.Services;

public class VisitPlanService(TherapistServiceContext context,
							  IValidator<VisitPlanDto> dataValidator,
							  IValidator<VisitPlanFilterDto> filterValidator) : IVisitPlanService, IDisposable
{
	private readonly TherapistServiceContext _context = context;
	private readonly IValidator<VisitPlanDto> _dataValidator = dataValidator;
	private readonly IValidator<VisitPlanFilterDto> _filterValidator = filterValidator;

	public async Task<Result> GetAll(VisitPlanFilterDto model)
	{
		ValidationResult validationResult = _filterValidator.Validate(model);
		if (validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		bool isTherapistExist = await _context.Therapists.AnyAsync(d => d.Id == model.TherapistId);
		if (isTherapistExist)
			return CustomErrors.NotFoundData();

		List<VisitPlan> visitPlans = _context.VisitPlans.Where(vp => vp.TherapistId == model.TherapistId &&
																	 vp.Date >= model.FromDate &&
																	 vp.Date <= model.ToDate)
														.ToList();

		return CustomResults.SuccessOperation(visitPlans.Adapt<List<VisitPlanDto>>());
	}

	public async Task<Result> Add(VisitPlanDto model)
	{
		ValidationResult validationResult = _dataValidator.Validate(model);
		if (validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		try
		{
			VisitPlan visitPlan = model.Adapt<VisitPlan>();
			await _context.VisitPlans.AddAsync(visitPlan);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessCreation(visitPlan.Adapt<VisitPlanDto>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> Edit(VisitPlanDto model)
	{
		ValidationResult validationResult = _dataValidator.Validate(model);
		if (validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		if (model.Id == null)
			return CustomErrors.InvalidData("Id not Assigned!");

		VisitPlan? oldData = await _context.VisitPlans.SingleOrDefaultAsync(l => l.Id == model.Id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			_context.Entry(oldData).State = EntityState.Detached;

			VisitPlan visitPlan = model.Adapt<VisitPlan>();
			_context.VisitPlans.Update(visitPlan);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessUpdate(visitPlan.Adapt<VisitPlanDto>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> Delete(Guid Id)
	{
		VisitPlan? visitPlan = await _context.VisitPlans.SingleOrDefaultAsync(vp => vp.Id == Id);
		if (visitPlan == null)
			return CustomErrors.NotFoundData();

		try
		{
			_context.VisitPlans.Remove(visitPlan);
			await _context.SaveChangesAsync();

			return CustomResults.SuccessDelete();
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public void Dispose()
	{
		_context.Dispose();
	}
}
