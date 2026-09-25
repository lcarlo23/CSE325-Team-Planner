using MongoDB.Driver;
using TeamProjectPlanner.Data;
using TeamProjectPlanner.Models;

namespace TeamProjectPlanner.Services;

public class ProjectService
{
    private readonly IMongoCollection<Project> _projects;

    public ProjectService(MongoDbSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.ConnectionString))
        {
            throw new InvalidOperationException(
                "MongoDB connection string is not configured.");
        }

        if (string.IsNullOrWhiteSpace(settings.DatabaseName))
        {
            throw new InvalidOperationException(
                "MongoDB database name is not configured.");
        }

        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        _projects = database.GetCollection<Project>("Projects");
    }

    public async Task CreateProjectAsync(Project project)
    {
        if (string.IsNullOrWhiteSpace(project.Name))
        {
            throw new ArgumentException(
                "Project name is required.",
                nameof(project));
        }

        if (string.IsNullOrWhiteSpace(project.Description))
        {
            throw new ArgumentException(
                "Project description is required.",
                nameof(project));
        }

        if (project.EndDate < project.StartDate)
        {
            throw new ArgumentException(
                "Project end date cannot be before the start date.");
        }

        await _projects.InsertOneAsync(project);
    }

    public async Task<List<Project>> GetProjectsAsync()
    {
        return await _projects.Find(_ => true).ToListAsync();
    }
}