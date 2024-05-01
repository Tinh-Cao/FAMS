using FAMS_GROUP2.Repositories.Entities;

namespace FAMS_GROUP2.Repositories.Interfaces;

public interface IClassAccountRepository: IGenericRepository<ClassAccount>
{
    public Task<ClassAccount> GetClassAccountByClassId(int classId);
}