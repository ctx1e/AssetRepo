// ProjectRepository.cs
using AssetRepo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssetRepo.Pattern.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AssetRepoContext _context;

        public ProjectRepository(AssetRepoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync(int pageNumber, int pageSize)
        {
            return await _context.Projects
                .OrderByDescending(p => p.CreatedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.Asset)
                .ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task AddProjectAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetProjectCountAsync()
        {
            return await _context.Projects.CountAsync();
        }

        public async Task<IEnumerable<Asset>> GetAvailableAssetsForProjectAsync()
        {
            // Lấy tất cả AssetId trong AssetSold
            var soldAssetIds = await _context.AssetSolds
                .Select(s => s.AssetId)
                .ToListAsync();

            // Lấy danh sách tài sản không nằm trong AssetSold
            var availableAssets = await _context.Assets
                .Where(asset => !soldAssetIds.Contains(asset.AssetId))
                .ToListAsync();

            return availableAssets;
        }
    }
}
