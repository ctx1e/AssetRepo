using AssetRepo.Models;

namespace AssetRepo.Pattern.Repository
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync(int pageNumber, int pageSize);
        Task<Project> GetProjectByIdAsync(int id);
        Task AddProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(int id);
        Task<int> GetProjectCountAsync();
       

    }
}
