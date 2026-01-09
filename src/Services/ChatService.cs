using Microsoft.EntityFrameworkCore;
using projekt_io.Data;
using projekt_io.Models;

namespace projekt_io.Services;

public class ChatService : IChatService
{
    private readonly AppDbContext _context;

    public ChatService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ChatListItemViewModel>> GetUserChatsAsync(string userId)
    {
        // bierzemy czaty, gdzie user jest uczestnikiem
        var chats = await _context.Chats
            .Where(c => c.Participants.Any(p => p.UserId == userId))
            .Select(c => new {
                c.Id,
                LastMessage = c.Messages
                    .OrderByDescending(m => m.SentAt)
                    .Select(m => new { m.Content, m.SentAt })
                    .FirstOrDefault(),
                OtherUser = c.Participants
                    .Where(p => p.UserId != userId)
                    .Select(p => new { p.User.FirstName, p.User.LastName, p.User.Email })
                    .FirstOrDefault()
            })
            .OrderByDescending(x => x.LastMessage != null ? x.LastMessage.SentAt : DateTime.MinValue)
            .ToListAsync();

        return chats.Select(x => new ChatListItemViewModel
        {
            ChatId = x.Id,
            OtherUserDisplay = x.OtherUser == null
                ? "(brak drugiego uczestnika)"
                : $"{x.OtherUser.FirstName} {x.OtherUser.LastName}".Trim() + $" - {x.OtherUser.Email}",
            LastMessagePreview = x.LastMessage?.Content ?? "(brak wiadomości)",
            LastMessageAt = x.LastMessage?.SentAt
        }).ToList();
    }

    public async Task<ChatOpenViewModel?> GetChatAsync(string chatId, string userId)
    {
        // sprawdzamy czy user jest uczestnikiem
        var isParticipant = await _context.ChatParticipants
            .AnyAsync(p => p.ChatId == chatId && p.UserId == userId);

        if (!isParticipant)
        {
            return null;
        }

        var otherUser = await _context.ChatParticipants
            .Where(p => p.ChatId == chatId && p.UserId != userId)
            .Select(p => new { p.User.FirstName, p.User.LastName, p.User.Email })
            .FirstOrDefaultAsync();

        var messages = await _context.Messages
            .Where(m => m.ChatId == chatId)
            .OrderBy(m => m.SentAt)
            .Select(m => new ChatMessageViewModel
            {
                SenderId = m.SenderId,
                Content = m.Content,
                SentAt = m.SentAt,
                IsMine = m.SenderId == userId
            })
            .ToListAsync();

        return new ChatOpenViewModel
        {
            ChatId = chatId,
            OtherUserDisplay = otherUser == null
                ? "(brak drugiego uczestnika)"
                : $"{otherUser.FirstName} {otherUser.LastName}".Trim() + $" - {otherUser.Email}",
            Messages = messages
        };
    }
}
