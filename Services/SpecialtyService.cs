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
using FileService;

namespace TherapistService.Services;

public class SpecialtyService(TherapistServiceContext context,
							  IValidator<AddSpecialtyDto> addValidator,
							  IValidator<EditSpecialtyDto> editValidator,
							  IValidator<AddFileDto> fileValidator) : ISpecialtyService, IDisposable
{

	private readonly TherapistServiceContext _context = context;
	private readonly FileService.FileService _fileService = new(fileValidator);

	private readonly IValidator<AddSpecialtyDto> _addValidator = addValidator;
	private readonly IValidator<EditSpecialtyDto> _editValidator = editValidator;


	public async Task<Result> GetAll()
	{
		List<Specialty> Specialties = await _context.Specialties.ToListAsync();
		return CustomResults.SuccessOperation(Specialties.Adapt<List<SpecialtyInfo>>());
	}


	public async Task<Result> Add(AddSpecialtyDto model)
	{
		ValidationResult validationResult = _addValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		Specialty specialty = model.Adapt<Specialty>();

		Result fileResult = await _fileService.Add(new(specialty.Id, model.Image, "Specialty"));
		if (!fileResult.Status)
			return fileResult;

		specialty.ImagePath = fileResult.Data?.ToString() ?? "";

		try
		{
			await _context.Specialties.AddAsync(specialty);
			await _context.SaveChangesAsync();

			return CustomResults.SuccessCreation(specialty.Adapt<SpecialtyInfo>());
		}
		catch (Exception e)
		{
			_fileService.Delete(specialty.ImagePath);
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> Edit(EditSpecialtyDto model)
	{
		ValidationResult validationResult = _editValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		Specialty? oldData = await _context.Specialties.SingleOrDefaultAsync(l => l.Id == model.Id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		string oldPath = oldData.ImagePath;

		_context.Entry(oldData).State = EntityState.Detached;
		oldData = model.Adapt<Specialty>();
		oldData.ImagePath = oldPath;

		if (model.Image != null)
		{
			Result fileResult = await _fileService.Add(new(oldData.Id, model.Image, "Doctor"));
			if (!fileResult.Status)
				return fileResult;

			oldData.ImagePath = fileResult.Data?.ToString() ?? "";
		}

		try
		{
			_context.Specialties.Update(oldData);
			await _context.SaveChangesAsync();

			if (model.Image != null)
			{
				Result fileResult = _fileService.Delete(oldPath);
				if (!fileResult.Status)
					return fileResult;
			}

			return CustomResults.SuccessUpdate(oldData.Adapt<SpecialtyInfo>());
		}
		catch (Exception e)
		{
			if (model.Image != null)
				_fileService.Delete(oldData.ImagePath);

			return CustomErrors.InternalServer(e.Message);
		}
	}


	public void Dispose()
	{
		_context.Dispose();
	}
}
