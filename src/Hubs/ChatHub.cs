using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using projekt_io.Data;
using projekt_io.Entities;

namespace projekt_io.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly AppDbContext _context;

    public ChatHub(AppDbContext context)
    {
        _context = context;
    }

    private static string GroupName(string chatId) => $"chat:{chatId}";

    public async Task JoinChat(string chatId)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new HubException("Unauthorized");
        }

        var isParticipant = await _context.ChatParticipants
            .AnyAsync(p => p.ChatId == chatId && p.UserId == userId);

        if (!isParticipant)
        {
            throw new HubException("Forbidden");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, GroupName(chatId));
    }

    public async Task LeaveChat(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName(chatId));
    }

    public async Task SendMessage(string chatId, string content)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new HubException("Unauthorized");
        }

        content = (content ?? "").Trim();
        if (content.Length == 0)
        {
            throw new HubException("Empty message");
        }

        if (content.Length > 2000)
            throw new HubException("Message too long");

        // czy user ma prawo pisać w tym czacie
        var isParticipant = await _context.ChatParticipants
            .AnyAsync(p => p.ChatId == chatId && p.UserId == userId);

        if (!isParticipant)
        {
            throw new HubException("Forbidden");
        }

        // zapis do bazy
        var message = new Message
        {
            Id = Guid.NewGuid().ToString(),
            ChatId = chatId,
            SenderId = userId,
            Content = content,
            SentAt = DateTime.UtcNow
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        // wyślij do wszystkich w pokoju czatu
        var payload = new ChatMessagePayload
        {
            ChatId = chatId,
            SenderId = userId,
            Content = message.Content,
            SentAt = message.SentAt
        };

        await Clients.Group(GroupName(chatId)).SendAsync("ReceiveMessage", payload);
    }

    public class ChatMessagePayload
    {
        public string ChatId { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
