﻿using Backend.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Backend.Entities;

namespace Backend.Services;

public class TodoTasksService
{
    private readonly ApplicationDbContext _applicationDbContext;

    public TodoTasksService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<IList<TodoTask>> ListAsync()
    {
        return await _applicationDbContext.TodoTasks
            .OrderBy(entity => entity.Id)
            .ToListAsync();
    }

    public async Task<TodoTask?> GetAsync(int id)
    {
        return await _applicationDbContext.TodoTasks.FindAsync(id);
    }

    public async Task<int> AddAsync(TodoTask todoTask)
    {
        var entityEntry = await _applicationDbContext.AddAsync(todoTask);
        await _applicationDbContext.SaveChangesAsync();
        return entityEntry.Entity.Id;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var rowsDeleted = await _applicationDbContext.TodoTasks
            .Where(t => t.Id == id)
            .ExecuteDeleteAsync();

        return rowsDeleted > 0;
    }
}