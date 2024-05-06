using TherapistService.Data;
using TherapistService.DTOs;
using TherapistService.Entities;
using TherapistService.Enums;
using TherapistService.Models;
using TherapistService.Shared;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MedicalHealthPlus.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TherapistService;

public class TherapistService(TherapistServiceContext context,
						   IValidator<AddTherapistDto> addValidator,
						   IValidator<EditTherapistDto> editValidator) : ITherapistService, IDisposable
{
	private readonly TherapistServiceContext _context = context;
	private readonly IValidator<AddTherapistDto> _addValidator = addValidator;
	private readonly IValidator<EditTherapistDto> _editValidator = editValidator;

	private async Task<Result> Get(Guid id)
	{
		if (id == Guid.Empty)
			return CustomErrors.InvalidData("Id not Assigned!");

		Therapist? Therapist = await _context.Therapists.SingleOrDefaultAsync(d => d.Id == id &&
																		  													d.Status == TherapistStatus.Confirmed);
		if (Therapist == null)
			return CustomErrors.NotFoundData();

		return CustomResults.SuccessOperation(Therapist);
	}
	public async Task<Result> GetInfo(Guid id)
	{
		Result result = await Get(id);

		if (result.Status)
			return CustomResults.SuccessOperation(result.Data.Adapt<TherapistInfo>());
		else
			return result;
	}
	public async Task<Result> GetDetail(Guid id)
	{
		Result result = await Get(id);

		if (result.Status)
			return CustomResults.SuccessOperation(result.Data.Adapt<TherapistDetail>());
		else
			return result;
	}

	private static DateTime? GetNearestVisitDate(ICollection<VisitPlan>? visitPlans)
	{
		if (visitPlans == null || visitPlans.Count == 0)
			return null;

		DateTime? dateTime = null;

		IEnumerable<DateTime> dates = from vp in visitPlans
									  where vp.Date.CompareTo(DateOnly.FromDateTime(DateTime.Now)) <= 0 &&
											vp.StartTime.CompareTo(TimeOnly.FromDateTime(DateTime.Now)) <= 0
									  select new DateTime(vp.Date, vp.StartTime);
		dateTime = dates.FirstOrDefault();

		return dateTime;
	}
	public async Task<Result> GetAllRecommends()
	{
		List<RecommendedTherapist> Therapists = await _context.Therapists.IgnoreAutoIncludes()
																												.Include(d => d.Specialty)
																												.Where(d => d.Status == TherapistStatus.Confirmed)
																												.Select(d => new RecommendedTherapist
																												{
																													Id = d.Id,
																													FullName = $"{d.Name} {d.Family}",
																													SpecialtyTitle = d.Specialty.Title,
																													Rate = d.Rate,
																													LineStatus = d.LineStatus
																												})
																												.OrderByDescending(d => d.Rate)
																												.Take(6)
																												.ToListAsync();

		return CustomResults.SuccessOperation(Therapists);
	}
	public async Task<Result> GetAllInfo(TherapistFilterDto model)
	{
		IQueryable<TherapistInfo> query = from Therapist in _context.Therapists
										  select new TherapistInfo
										  {
											  Name = Therapist.Name,
											  Family = Therapist.Family,
											  FullName = $"{Therapist.Name} {Therapist.Family}",
											  MedicalSysCode = Therapist.MedicalSysCode,
											  Content = Therapist.Content,
											  SpecialtyId = Therapist.SpecialtyId,
											  SpecialtyTitle = Therapist.Specialty.Title,
											  Rate = Therapist.Rate,
											  RaterCount = Therapist.RaterCount,
											  SuccessConsolationCount = Therapist.SuccessConsolationCount,
											  SuccessReservationCount = Therapist.SuccessReservationCount,
											  HasTelCounseling = Therapist.HasTelCounseling,
											  HasTextCounseling = Therapist.HasTextCounseling,
											  OnlinePlans = Therapist.OnlinePlans != null ? Therapist.OnlinePlans.Adapt<ICollection<OnlinePlanDto>>() : null,
											  AcceptVisit = Therapist.AcceptVisit,
											  ClinicId = Therapist.ClinicId,
											  Clinic = Therapist.Clinic != null ? Therapist.Clinic.Adapt<ClinicDto>() : null,
											  VisitPlans = Therapist.VisitPlans != null ? Therapist.VisitPlans.Adapt<ICollection<VisitPlanDto>>() : null,
											  NearestVisitDate = GetNearestVisitDate(Therapist.VisitPlans),
											  LineStatus = Therapist.LineStatus
										  };
		query = model.ServiceType switch
		{
			TherapistServiceType.HasTelCounseling => query.Where(d => d.HasTelCounseling),
			TherapistServiceType.HasTextCounseling => query.Where(d => d.HasTextCounseling),
			TherapistServiceType.HasReservation => query.Where(d => d.AcceptVisit),
			TherapistServiceType.All => query,
			_ => query
		};

		query = model.Order switch
		{
			TherapistFilterOrder.NearestReservationTime => query.OrderByDescending(d => d.NearestVisitDate),
			TherapistFilterOrder.Rate => query.OrderByDescending(d => d.Rate),
			TherapistFilterOrder.SuccessReservationCount => query.OrderByDescending(d => d.SuccessReservationCount),
			TherapistFilterOrder.Default => query.OrderBy(d => d.Name),
			_ => query.OrderBy(d => d.Name)
		};

		if (model.LocationId != null)
			query = query.Where(d => d.Clinic != null &&
								d.Clinic.LocationId == model.LocationId);

		return CustomResults.SuccessOperation(await query.ToListAsync());
	}
	public async Task<Result> GetAllDetails(TherapistFilterDto model)
	{
		IQueryable<TherapistInfo> query = from Therapist in _context.Therapists
										  select new TherapistInfo
										  {
											  Name = Therapist.Name,
											  Family = Therapist.Family,
											  FullName = $"{Therapist.Name} {Therapist.Family}",
											  MedicalSysCode = Therapist.MedicalSysCode,
											  Content = Therapist.Content,
											  SpecialtyId = Therapist.SpecialtyId,
											  SpecialtyTitle = Therapist.Specialty.Title,
											  Rate = Therapist.Rate,
											  RaterCount = Therapist.RaterCount,
											  SuccessConsolationCount = Therapist.SuccessConsolationCount,
											  SuccessReservationCount = Therapist.SuccessReservationCount,
											  HasTelCounseling = Therapist.HasTelCounseling,
											  HasTextCounseling = Therapist.HasTextCounseling,
											  OnlinePlans = Therapist.OnlinePlans != null ? Therapist.OnlinePlans.Adapt<ICollection<OnlinePlanDto>>() : null,
											  AcceptVisit = Therapist.AcceptVisit,
											  ClinicId = Therapist.ClinicId,
											  Clinic = Therapist.Clinic != null ? Therapist.Clinic.Adapt<ClinicDto>() : null,
											  VisitPlans = Therapist.VisitPlans != null ? Therapist.VisitPlans.Adapt<ICollection<VisitPlanDto>>() : null,
											  NearestVisitDate = GetNearestVisitDate(Therapist.VisitPlans),
											  LineStatus = Therapist.LineStatus
										  };
		query = model.ServiceType switch
		{
			TherapistServiceType.HasTelCounseling => query.Where(d => d.HasTelCounseling),
			TherapistServiceType.HasTextCounseling => query.Where(d => d.HasTextCounseling),
			TherapistServiceType.HasReservation => query.Where(d => d.AcceptVisit),
			TherapistServiceType.All => query,
			_ => query
		};

		query = model.Order switch
		{
			TherapistFilterOrder.NearestReservationTime => query.OrderByDescending(d => d.NearestVisitDate),
			TherapistFilterOrder.Rate => query.OrderByDescending(d => d.Rate),
			TherapistFilterOrder.SuccessReservationCount => query.OrderByDescending(d => d.SuccessReservationCount),
			TherapistFilterOrder.Default => query.OrderBy(d => d.Name),
			_ => query.OrderBy(d => d.Name)
		};

		if (model.LocationId != null)
			query = query.Where(d => d.Clinic != null &&
								d.Clinic.LocationId == model.LocationId);

		return CustomResults.SuccessOperation(await query.ToListAsync());
	}

	public async Task<Result> Add(AddTherapistDto model)
	{
		ValidationResult validationResult = _addValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		try
		{
			Therapist Therapist = model.Adapt<Therapist>();
			await _context.Therapists.AddAsync(Therapist);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessCreation(Therapist.Adapt<TherapistDetail>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> Edit(EditTherapistDto model)
	{
		ValidationResult validationResult = _editValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		Therapist? oldData = await _context.Therapists.SingleOrDefaultAsync(d => d.Id == model.Id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			_context.Entry(oldData).State = EntityState.Detached;

			Therapist Therapist = model.Adapt<Therapist>();
			_context.Therapists.Update(Therapist);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessUpdate(Therapist.Adapt<TherapistInfo>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}
	public async Task<Result> ChangeStatus(Guid id, TherapistStatus status)
	{
		Therapist? oldData = await _context.Therapists.SingleOrDefaultAsync(d => d.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.Therapists.Where(d => d.Id == id)
									 					 .ExecuteUpdateAsync(setters => setters.SetProperty(d => d.Status, status));

			await _context.SaveChangesAsync();

			if (effectedRowCount == 1)
				return CustomResults.SuccessUpdate(oldData.Adapt<TherapistInfo>());
			else
				return CustomErrors.NotFoundData();
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}
	public async Task<Result> ChangeLineStatus(Guid id, TherapistLineStatus lineStatus)
	{
		Therapist? oldData = await _context.Therapists.SingleOrDefaultAsync(d => d.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.Therapists.Where(d => d.Id == id)
									 					 .ExecuteUpdateAsync(setters => setters.SetProperty(d => d.LineStatus, lineStatus));

			await _context.SaveChangesAsync();

			if (effectedRowCount == 1)
				return CustomResults.SuccessUpdate(oldData.Adapt<TherapistInfo>());
			else
				return CustomErrors.NotFoundData();
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
