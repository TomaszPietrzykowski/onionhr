﻿using Microsoft.EntityFrameworkCore;
using OnionHR.Application.Contracts.Persistance;
using OnionHR.Domain;
using OnionHR.Persistence.DatabaseContext;

namespace OnionHR.Persistence.Repositories;

public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
{
    public LeaveAllocationRepository(HrDbContext context) : base(context)
    {

    }

    public async Task AddAllocations(List<LeaveAllocation> allocations)
    {
        await _context.AddRangeAsync(allocations);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> AllocationExists(string userId, int leaveTypeId, int period)
    {
        return await _context.LeaveAllocations.AnyAsync(q => q.EmployeeId == userId
                && q.LeaveTypeId == leaveTypeId
                && q.Period == period);
    }

    public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails()
    {
        var leaveAllocations = await _context.LeaveAllocations
            .Include(q => q.LeaveType)
            .ToListAsync();

        return leaveAllocations;
    }

    public async Task<List<LeaveAllocation>> GetLeaveAllocationsWithDetails(string userId)
    {
        var leaveAllocations = await _context.LeaveAllocations
            .Where(q => q.EmployeeId == userId)
            .Include(q => q.LeaveType)
            .ToListAsync();

        return leaveAllocations;
    }

    public async Task<LeaveAllocation> GetLeaveAllocationWithDetails(int id)
    {
        var leaveAllocation = await _context.LeaveAllocations
            .Include(q => q.LeaveType)
            .FirstOrDefaultAsync(q => q.Id == id);

        return leaveAllocation;
    }

    public async Task<List<LeaveAllocation>> GetUserAllocations(string userId, int leaveTypeId)
    {
        var leaveAllocations = await _context.LeaveAllocations
            .Where(q => q.EmployeeId == userId && q.LeaveTypeId == leaveTypeId)
            .ToListAsync();

        return leaveAllocations;
    }
}