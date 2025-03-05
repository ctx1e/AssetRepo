using AssetRepo.Models;
using AssetRepo.Pattern.Repository;
using AssetRepo.Pattern.Service;
using Microsoft.EntityFrameworkCore;

namespace AssetRepo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            //dki NorthWinContext
            //builder.Services.AddDbContext<AssetRepoContext>();
            builder.Services.AddDbContext<AssetRepoContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

            // dki repo and service
            builder.Services.AddScoped<IAssetRepository, AssetRepository>();
            builder.Services.AddScoped<IAssetService, AssetService>();
            builder.Services.AddScoped<IAssetSoldRepository, AssetSoldRepository>();
            builder.Services.AddScoped<IAssetSoldService, AssetSoldService>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IFolderRepository, FolderRepository>();
            builder.Services.AddScoped<IFolderService, FolderService>();
            builder.Services.AddScoped<IFIleRepository, FileRepository>();
            builder.Services.AddScoped<IFileService, FileService>();


            var app = builder.Build();
           
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                
                app.UseHsts();
            }
           

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            //app.MapGet("/", async context =>
            //{
            //    context.Response.Redirect("/Assets/AssetRepository");
            //});

            app.Run();
        }
    }
}