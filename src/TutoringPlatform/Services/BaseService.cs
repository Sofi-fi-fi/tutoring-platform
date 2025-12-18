using AutoMapper;
using TutoringPlatform.Models;
using TutoringPlatform.Services.Interfaces;
using TutoringPlatform.Repositories.Interfaces;

namespace TutoringPlatform.Services;

public abstract class BaseService<TEntity, TDto, TCreateDto, TUpdateDto>(
	IRepository<TEntity> repository,
	TutoringDbContext context,
	IMapper mapper,
	ILogger logger)
	: IBaseService<TDto, TCreateDto, TUpdateDto>
    where TEntity : class
{
    protected readonly IRepository<TEntity> _repository = repository;
    protected readonly TutoringDbContext _context = context;
    protected readonly IMapper _mapper = mapper;
    protected readonly ILogger _logger = logger;

    public virtual async Task<TDto?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? default : _mapper.Map<TDto>(entity);
    }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<TDto>>(entities);
    }

    public virtual async Task<TDto> CreateAsync(TCreateDto createDto)
    {
        var entity = _mapper.Map<TEntity>(createDto);
        var created = await _repository.AddAsync(entity);
        return _mapper.Map<TDto>(created);
    }

    public virtual async Task<TDto> UpdateAsync(int id, TUpdateDto updateDto)
    {
        var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Entity with id {id} not found");
		_mapper.Map(updateDto, entity);
        await _repository.UpdateAsync(entity);
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Entity with id {id} not found");
		await _repository.DeleteAsync(entity);
    }
}