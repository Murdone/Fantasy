﻿using Fantasy.Backend.Repositories.Interfaces;
using Fantasy.Backend.UnitsOfWork.interfaces;
using Fantasy.shared.Responses;

namespace Fantasy.Backend.UnitsOfWork.Implementations;

public class GenericUnitOfWork<T> : IGenericUnitOfWork<T> where T : class
{
    private readonly IGenericRepository<T> _repository;

    public GenericUnitOfWork(IGenericRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual async Task<ActionsResponse<T>> AddAsync(T model) => await _repository.AddAsync(model);

    public virtual async Task<ActionsResponse<T>> DeleteAsync(int id) => await _repository.DeleteAsync(id);

    public virtual async Task<ActionsResponse<IEnumerable<T>>> GetAsync() => await _repository.GetAsync();

    public virtual async Task<ActionsResponse<T>> GetAsync(int id) => await _repository.GetAsync(id);

    public virtual async Task<ActionsResponse<T>> UpdateAsync(T model) => await _repository.UpdateAsync(model);
}