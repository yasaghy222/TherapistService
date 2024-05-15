using TherapistService.Models;

namespace FileService
{
    public interface IFileService
    {
        Task<Result> Add(AddFileDto model);
        Result Delete(string address);
    }
}