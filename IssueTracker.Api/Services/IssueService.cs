using System.Reflection.Metadata.Ecma335;
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

    public Issue? GetIssueById(int id)
    {
        return _issues.FirstOrDefault(issue => issue.Id == id);
       
    }
    public Issue CreateIssue(Issue issue)
    {
        var nexId = _issues.Max(issue => issue.Id) + 1;
        issue.Id = nexId;

        issue.CreatedAt = DateTime.UtcNow;
        _issues.Add(issue);

        return issue;
    }

    public Issue? UpdateIssue(int id, Issue updatedIssue)
    {
        var issue = _issues.FirstOrDefault(issue => issue.Id == id);

        if(issue == null)
        {
            return null;
        }
        else{
            issue.Title = updatedIssue.Title;
            issue.Description = updatedIssue.Description;
            issue.Status = updatedIssue.Status;
            issue.Priority = updatedIssue.Priority;
        }
        return issue;
    }

    public bool DeleteIssue(int id)
    {
        var issue = _issues.FirstOrDefault(issue => issue.Id == id);
        if(issue == null)
        {
            return false;
        }
        else
        {
            _issues.Remove(issue);
        }
        return true;
    }

    
    
    
}