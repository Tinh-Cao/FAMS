using Application.ViewModels.ResponseModels;

namespace FAMS_GROUP2.Repositories;

public class ResponseDataModel<T> : ResponseModel where T : class
{
    public T? Data {get; set;}
}
