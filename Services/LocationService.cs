﻿using TherapistService.Data;
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

public class LocationService(TherapistServiceContext context,
							 IValidator<LocationDto> dataValidator) : ILocationService, IDisposable
{
	private readonly TherapistServiceContext _context = context;
	private readonly IValidator<LocationDto> _dataValidator = dataValidator;


	public async Task<Result> GetAll(Guid? parentId)
	{
		List<Location> locations = await _context.Locations.Where(l => l.ParentId == parentId)
														   .ToListAsync();

		return CustomResults.SuccessOperation(locations.Adapt<List<LocationDto>>());
	}


	public async Task<Result> Add(LocationDto model)
	{
		ValidationResult validationResult = _dataValidator.Validate(model);
		if (validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		try
		{
			Location location = model.Adapt<Location>();
			await _context.Locations.AddAsync(location);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessCreation(location.Adapt<LocationDto>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> Edit(LocationDto model)
	{
		ValidationResult validationResult = _dataValidator.Validate(model);
		if (validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		if (model.Id == null)
			return CustomErrors.InvalidData("Id not Assigned!");

		Location? oldData = await _context.Locations.SingleOrDefaultAsync(l => l.Id == model.Id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			_context.Entry(oldData).State = EntityState.Detached;

			Location location = model.Adapt<Location>();
			_context.Locations.Update(location);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessUpdate(location.Adapt<LocationDto>());
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
