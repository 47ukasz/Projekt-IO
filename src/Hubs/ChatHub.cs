using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using projekt_io.Data;
using projekt_io.Entities;
using projekt_io.Services;

namespace projekt_io.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly AppDbContext _context;
    private readonly IChatService _chatService;

    public ChatHub(AppDbContext context, IChatService chatService) {
        _context = context;
        _chatService = chatService;
    }

    private static string GroupName(string chatId) => $"chat:{chatId}";

    public async Task JoinChat(string chatId) {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            throw new HubException("Unauthorized");

        var isParticipant = await _context.Chats.AnyAsync(c =>
            c.Id == chatId && (c.CreatorId == userId || c.OwnerId == userId));

        if (!isParticipant)
            throw new HubException("Forbidden");

        await Groups.AddToGroupAsync(Context.ConnectionId, GroupName(chatId));
    }

    public async Task LeaveChat(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName(chatId));
    }

    public async Task SendMessage(string chatId, string content) {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId)) {
            throw new HubException("Unauthorized");
        }
        
        var dto = await _chatService.SendMessageAsync(chatId, userId, content);

        if (dto == null) {
            throw new HubException("Forbidden");
        }
        
        await Clients.Group(GroupName(chatId)).SendAsync("ReceiveMessage", new {
            chatId,
            senderId = userId,
            content = dto.Content,
            sentAt = dto.SentAt
        });
    }
}
