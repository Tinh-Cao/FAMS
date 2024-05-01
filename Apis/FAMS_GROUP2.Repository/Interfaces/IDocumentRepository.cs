using FAMS_GROUP2.Repositories.Commons;
using FAMS_GROUP2.Repositories.Entities;
using FAMS_GROUP2.Repositories.Helper;
using FAMS_GROUP2.Repositories.ViewModels.DocumentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_GROUP2.Repositories.Interfaces
{
    public interface IDocumentRepository : IGenericRepository<Document>
    {
        public Task<Pagination<Document>> GetDocumentFilterAsync(PaginationParameter paginationParameter, DocumentFilterModel documentFilterModel);
    }
}
