using SubmissionsProcessor.API.Models.MongoDB;

namespace SubmissionsProcessor.API.Services.MongoDB
{
    public interface IFormsService
    {
        Task CreateAsync(Form newForm);
        Task<List<Form>> GetAsync();
        Task<Form?> GetAsync(string id);
        Task UpdateAsync(string id, Form updatedForm);
    }
}