namespace BookHub.Server.Features.Chat.Web
{
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        public async Task SendMessageToChat(int chatId, string senderId, string message)
            => await this.Clients
                .Group(chatId.ToString())
                .SendAsync("ReceiveMessage", senderId, message);

        public async Task JoinChat(int id)
            => await this.Groups
                .AddToGroupAsync(this.Context.ConnectionId, id.ToString());

        public async Task LeaveChat(int id)
            => await this.Groups
                .RemoveFromGroupAsync(this.Context.ConnectionId, id.ToString());
    }
}
