using IssueTracker.Api.Models;
using IssueTracker.Api.Services;
using IssueTracker.Api.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IssueService>();

var app = builder.Build();


List<Issue> issues = new List<Issue>();

issues.Add(new Issue
{
    Id =1,
    Title = " Login button does not work",
    Description = "User cannot log in when clicking the login button.",
    Status = "Open",
    Priority = "High",
    CreatedAt = DateTime.UtcNow
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/health", () =>
{
    return Results.Ok("Api is running");
})
.WithName("GetHeath");

app.MapGet("/api/issues", (IssueService issueService) =>
{   
    return Results.Ok(issueService.GetAllIssues());
})
.WithName("GetIssues");


app.MapGet("/api/issues/{id}", (int id, IssueService issueService) =>
{   
    var issue = issueService.GetIssueById(id);

    if(issue == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(issue);
})
.WithName("GetIssueByID");

app.MapPost("/api/issues", (CreateIssueRequest request, IssueService issueService) =>
{   
    var allowPriorities = new[] {"Low", "Medium", "High"};

    if (string.IsNullOrWhiteSpace(request.Title))
    {
       return Results.BadRequest("Title is required");
    }

    var priorityIsValid = allowPriorities.Any(priority => priority.Equals(request.Priority, StringComparison.OrdinalIgnoreCase));
    
    if ( !priorityIsValid )
    {
        return Results.BadRequest("Priority must be Low, Medium or High.");
    }

    var issue = new Issue
    { 
        Title = request.Title,
        Description = request.Description,
        Status = request.Status,
        Priority = request.Priority 
    };
    var createdIssue = issueService.CreateIssue(issue);

   return Results.Created($"/api/issues/{createdIssue.Id}", createdIssue);
})  
.WithName("CreatedIssue");


app.MapPut("/api/issues/{id}", (int id, IssueService issueService, UpdateIssueRequest updateIssueRequest) =>
{   
    var issue = new Issue
    
    { Title = updateIssueRequest.Title,
      Description = updateIssueRequest.Description,
      Status = updateIssueRequest.Status,
      Priority = updateIssueRequest.Priority   
    };
   
    var updatedIssue = issueService.UpdateIssue(id, issue);
    if( updatedIssue == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(updatedIssue);  
})
.WithName("UpdateIssue");

app.MapDelete("/api/issues/{id}", (int id, IssueService issueService) =>
{   
    var deleted = issueService.DeleteIssue(id);
    
    if(deleted == false)
    {
        return Results.NotFound();
    
    }
        return Results.NoContent();
})
.WithName("DeleteIssueByID");

app.Run();

