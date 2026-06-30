using IssueTracker.Api.Models;
namespace IssueTracker.Api.Services;

public class IssueService
{
    private readonly List<Issue> _issues = new();

    public IssueService()
    {
        _issues.Add(new Issue
        {
            Id = 1,
            Title = " Login button does not work",
            Description = "User cannot log in when clicking the login button.",
            Status = "Open",
            Priority = "High",
            CreatedAt = DateTime.UtcNow   
        });
    }

    public List<Issue> GetAllIssues()
    {
        return _issues;
    }

    
    
}