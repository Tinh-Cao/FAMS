using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FAMS_GROUP2.Repositories.Repositories;

public class ClassAccountRepository:GenericRepository<ClassAccount>, IClassAccountRepository
{
    private readonly FamsDbContext _context;
    
    public ClassAccountRepository(FamsDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
    {
        _context = context;
    }

    public async Task<ClassAccount> GetClassAccountByClassId(int classId)
    {
        var classAccountList = await _context.ClassAccounts.ToListAsync();
        return classAccountList.FirstOrDefault(a => a.ClassId == classId);
    }
}