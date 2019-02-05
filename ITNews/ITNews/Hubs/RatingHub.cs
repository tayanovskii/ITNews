using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ITNews.Hubs
{
    public class RatingHub : Hub
    {
        //public override async Task OnConnectedAsync()
        //{
        //    var httpContext = Context.GetHttpContext();
        //    var groupNameByNewsId = httpContext.Request.Query["newsId"].ToString();
        //    await Groups.AddToGroupAsync(Context.ConnectionId, groupNameByNewsId);
        //    await base.OnConnectedAsync();
        //}
    }
}
