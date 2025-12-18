using AutoMapper;
using TutoringPlatform.Models;
using TutoringPlatform.Models.Entities;
using TutoringPlatform.Services.Interfaces;
using TutoringPlatform.Repositories.Interfaces;
using TutoringPlatform.Services.DTOs.Subject;
using Microsoft.EntityFrameworkCore;

namespace TutoringPlatform.Services;

public class SubjectService(
	ISubjectRepository repository,
	TutoringDbContext context,
	IMapper mapper,
	ILogger<SubjectService> logger
) : BaseService<Subject, SubjectDto, CreateSubjectDto, UpdateSubjectDto>(repository, context, mapper, logger), ISubjectService
{
	private readonly ISubjectRepository _subjectRepository = repository;

    public override async Task<SubjectDto> CreateAsync(CreateSubjectDto createDto)
    {
        _logger.LogInformation("Creating subject {SubjectName}", createDto.Name);

        var existing = await _subjectRepository.GetByNameAsync(createDto.Name);
        if (existing is not null)
        {
            throw new InvalidOperationException($"Subject with name {createDto.Name} already exists");
        }

        var subject = _mapper.Map<Subject>(createDto);
        await _context.Subjects.AddAsync(subject);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Subject {SubjectId} created successfully", subject.SubjectId);

        var createdSubject = await _subjectRepository.GetByIdAsync(subject.SubjectId);
        return _mapper.Map<SubjectDto>(createdSubject);
    }

    public override async Task<SubjectDto> UpdateAsync(int id, UpdateSubjectDto updateDto)
    {
        var subject = await _context.Subjects
            .FirstOrDefaultAsync(s => s.SubjectId == id)
            ?? throw new KeyNotFoundException($"Subject with id {id} not found");

        var duplicateName = await _context.Subjects
            .AnyAsync(s => s.SubjectId != id && s.Name == updateDto.Name);

        if (duplicateName)
        {
            throw new InvalidOperationException($"Subject with name {updateDto.Name} already exists");
        }

        subject.Name = updateDto.Name;
        subject.Category = updateDto.Category;
        subject.Description = updateDto.Description;
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Subject {SubjectId} updated successfully", id);
        return _mapper.Map<SubjectDto>(subject);
    }

    public override async Task DeleteAsync(int id)
    {
        var subject = await _context.Subjects
            .Include(s => s.TutorSubjects)
            .FirstOrDefaultAsync(s => s.SubjectId == id)
            ?? throw new KeyNotFoundException($"Subject with id {id} not found");

        if (subject.TutorSubjects.Any())
        {
            throw new InvalidOperationException("Cannot delete a subject that has tutor assignments");
        }

        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Subject {SubjectId} deleted successfully", id);
    }

    public async Task<SubjectDto?> GetByNameAsync(string name)
    {
        var subject = await _subjectRepository.GetByNameAsync(name);
        return subject is null ? null : _mapper.Map<SubjectDto>(subject);
    }

    public async Task<IEnumerable<SubjectDto>> GetByCategoryAsync(string category)
    {
        var subjects = await _subjectRepository.GetByCategoryAsync(category);
        return _mapper.Map<IEnumerable<SubjectDto>>(subjects);
    }
}