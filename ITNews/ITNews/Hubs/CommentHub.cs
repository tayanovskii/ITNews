using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITNews.DTO;
using ITNews.DTO.NewsDto;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;

namespace ITNews.Hubs
{
    public class CommentHub:Hub
    {

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            string groupNameByNewsId = httpContext.Request.Query["newsId"].ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, groupNameByNewsId);
            await base.OnConnectedAsync();
        }

        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    var httpContext = Context.GetHttpContext();
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, httpContext.Request.Query["newsId"].ToString());
        //    await base.OnDisconnectedAsync(exception);
        //}
    }
}
