using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.Core.WebApi.Types;
using Microsoft.TeamFoundation.Dashboards.WebApi;
using Microsoft.TeamFoundation.Work.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.OAuth;
using Microsoft.VisualStudio.Services.WebApi;

namespace MicroManagement.TodoChatBot.Poc;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var connection = new VssConnection(new Uri("https://dev.azure.com/kimoj/"),
            new VssBasicCredential(string.Empty, ""));
        var workItemTrackingClient = connection.GetClient<WorkItemTrackingHttpClient>();
        var workClient = connection.GetClient<WorkHttpClient>();

        var teamContext = new TeamContext("PepinoTech", "Productivity Team");
        var iterations = await workClient.GetTeamIterationsAsync(teamContext);
        var currentIteration = iterations.FirstOrDefault(i => i.Attributes.TimeFrame == TimeFrame.Current);

        if (currentIteration == null)
        {
            throw new Exception("Current iteration not found");
        }

        var chartImage = await workClient.GetIterationChartImageAsync(teamContext, currentIteration.Id, "Burndown");

        var iterationWorkItems = await workClient.GetIterationWorkItemsAsync(teamContext, currentIteration.Id);

        var workItemsIds = iterationWorkItems.WorkItemRelations
            .Select(link => link.Target.Id);

        var workitems = await workItemTrackingClient.GetWorkItemsAsync(workItemsIds);

        var userStories = workitems
            .Where(wi => wi.Fields.GetValueOrDefault("System.WorkItemType") as string == "User Story")
        .ToList();

        var tasks = workitems
            .Where(wi => wi.Fields.GetValueOrDefault("System.WorkItemType") as string == "Task")
            .ToList();

        var relations = iterationWorkItems.WorkItemRelations.Where(r => r.Rel == "System.LinkTypes.Hierarchy-Forward").ToList();

        var grouped = relations
            .Where(r => userStories.Any(us => us.Id == r.Source.Id))
            .GroupBy(r => r.Source.Id)
            .Select(g => new
            {
                Key = userStories.Single(u => u.Id == g.Key),
                Value = g.SelectMany(r => tasks.Where(task => task.Id == r.Target.Id)).ToList()
            })
            .ToDictionary(g => g.Key, g => g.Value);

        // Query
        var queryResults = await workItemTrackingClient.QueryByIdAsync(new Guid("ef7ab7e8-50a6-4c8d-85b0-4e5fa0e51dc8"));

        workitems = await workItemTrackingClient.GetWorkItemsAsync(queryResults.WorkItems.Select(wr => wr.Id).ToList());

        teamContext = new TeamContext("PepinoTech", "Integration Team");

        var workItemFirst = workitems.First();

        var history = await workItemTrackingClient.GetUpdatesAsync("Pepinotech", workItemFirst.Id.Value);

        var dashboardClient = connection.GetClient<DashboardHttpClient>();

        Dashboard dashboard = await dashboardClient.GetDashboardAsync(teamContext, new Guid("1642bb2c-f082-4a06-a73d-0357c82d727f"));
    }
}