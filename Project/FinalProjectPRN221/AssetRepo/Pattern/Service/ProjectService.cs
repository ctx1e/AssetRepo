// ProjectService.cs
using AssetRepo.Models;
using AssetRepo.Pattern.Repository;

namespace AssetRepo.Pattern.Service
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync(int pageNumber, int pageSize)
        {
            return await _projectRepository.GetAllProjectsAsync(pageNumber, pageSize);
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _projectRepository.GetProjectByIdAsync(id);
        }

        public async Task AddProjectAsync(Project project)
        {
            await _projectRepository.AddProjectAsync(project);
        }

        public async Task UpdateProjectAsync(Project project)
        {
            await _projectRepository.UpdateProjectAsync(project);
        }

        public async Task DeleteProjectAsync(int id)
        {
            await _projectRepository.DeleteProjectAsync(id);
        }

        public async Task<int> GetProjectCountAsync()
        {
            return await _projectRepository.GetProjectCountAsync();
        }
    }
}
