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
using FileService;

namespace TherapistService;

public class TherapistService(TherapistServiceContext context,
						   IValidator<AddTherapistDto> addValidator,
						   IValidator<EditTherapistDto> editValidator,
						   IValidator<AddFileDto> fileValidator) : ITherapistService, IDisposable
{
	private readonly TherapistServiceContext _context = context;
	private readonly FileService.FileService _fileService = new(fileValidator);
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
																													ImagePath = d.ImagePath,
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
										 .Where(d => d.Status == TherapistStatus.Confirmed)
										.Skip((model.PageIndex - 1) * model.PageSize)
										.Take(model.PageSize)
										  select new TherapistInfo
										  {
											  ImagePath = Therapist.ImagePath,
											  Name = Therapist.Name,
											  Family = Therapist.Family,
											  FullName = $"{Therapist.Name} {Therapist.Family}",
											  CounselingSysCode = Therapist.CounselingSysCode,
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
										.Skip((model.PageIndex - 1) * model.PageSize)
										.Take(model.PageSize)
										  select new TherapistInfo
										  {
											  ImagePath = Therapist.ImagePath,
											  Name = Therapist.Name,
											  Family = Therapist.Family,
											  FullName = $"{Therapist.Name} {Therapist.Family}",
											  CounselingSysCode = Therapist.CounselingSysCode,
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

		Specialty? specialty = await _context.Specialties.SingleOrDefaultAsync(s => s.Id == model.SpecialtyId);
		if (specialty == null)
			return CustomErrors.NotFoundData("تخصص انتخاب شده یافت نشد!");

		Therapist therapist = model.Adapt<Therapist>();


		Result fileResult = await _fileService.Add(new(therapist.Id, model.Image, "Doctor"));
		if (!fileResult.Status)
			return fileResult;

		therapist.ImagePath = fileResult.Data?.ToString() ?? "";

		try
		{
			await _context.Therapists.AddAsync(therapist);
			await _context.SaveChangesAsync();

			therapist.Specialty = specialty;
			return CustomResults.SuccessCreation(therapist.Adapt<TherapistDetail>());
		}
		catch (Exception e)
		{
			_fileService.Delete(therapist.ImagePath);
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

		string oldPath = oldData.ImagePath;
		_context.Entry(oldData).State = EntityState.Detached;
		oldData = model.Adapt<Therapist>();
		oldData.ImagePath = oldPath;

		if (model.Image != null)
		{
			Result fileResult = await _fileService.Add(new(oldData.Id, model.Image, "Therapist"));
			if (!fileResult.Status)
				return fileResult;

			oldData.ImagePath = fileResult.Data?.ToString() ?? "";
		}

		try
		{
			_context.Therapists.Update(oldData);
			await _context.SaveChangesAsync();

			return CustomResults.SuccessUpdate(oldData.Adapt<TherapistInfo>());
		}
		catch (Exception e)
		{
			if (model.Image != null)
				_fileService.Delete(oldData.ImagePath);

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
