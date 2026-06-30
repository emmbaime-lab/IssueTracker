using IssueTracker.Api.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

app.MapGet("/api/issues", () =>
{
    return Results.Ok(issues);
})
.WithName("GetIssues");


app.MapGet("/api/issues/{id}", (int id) =>
{   
    var issue = issues.FirstOrDefault(issue => issue.Id == id);

    if(issue == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(issue);
})
.WithName("GetIssueByID");

app.MapPost("/api/issues", (Issue issue) =>
{   
   var nexID = issues.Max(issue => issue.Id) + 1;

   issue.Id = nexID;
   issue.CreatedAt = DateTime.UtcNow;
   issues.Add(issue);

   return Results.Created($"/api/issues/{issue.Id}", issue);return Results.Created();

})
.WithName("PostNextID");

app.MapPut("/api/issues/{id}", (int id, Issue updatedIssue) =>
{   
    var issue = issues.FirstOrDefault(issue => issue.Id == id);

    if(issue == null)
    {
        return Results.NotFound();
    }
    else
    {
        issue.Title = updatedIssue.Title;
        issue.Description = updatedIssue.Description;
        issue.Status = updatedIssue.Status;
        issue.Priority = updatedIssue.Priority;
    }
    return Results.Ok(issue);
})
.WithName("UpdateIssue");

app.MapDelete("/api/issues/{id}", (int id) =>
{   
    var issue = issues.FirstOrDefault(issue => issue.Id == id);

    if(issue == null)
    {
        return Results.NotFound();
    }
    else
    {
        issues.Remove(issue);
    }
    return Results.NoContent();
})
.WithName("DeleteIssueByID");

app.Run();

