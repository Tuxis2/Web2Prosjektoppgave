using Microsoft.AspNetCore.SignalR;
using Web2Prosjektoppgave.shared.ViewModels.Comment;

namespace Web2Prosjektoppgave.api.Hubs;

public interface ISendComment
{
     Task SendBlogPost(string user, CommentItemSignal signal);
}

public class SignalHub: Hub<ISendComment>
{
    public async Task SendBlogPost(string user, CommentItemSignal signal)
    {
        await Clients.All.SendBlogPost(user, signal);
    }
}