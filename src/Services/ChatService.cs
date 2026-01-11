using Microsoft.EntityFrameworkCore;
using projekt_io.Data;
using projekt_io.DTOs;
using projekt_io.Entities;
using projekt_io.Mappers;
using projekt_io.Models;

namespace projekt_io.Services;

public class ChatService : IChatService {
    private readonly AppDbContext _context;
    private readonly ILogger<ChatService> _logger;
    private readonly ILostReportService _lostReportService;

    public ChatService(AppDbContext context, ILogger<ChatService> logger, ILostReportService lostReportService) {
        _context = context;
        _logger = logger;
        _lostReportService = lostReportService;
    }
    
    public async Task<string> CreateAsync(string reportId, string creatorId) {
        var report = await _lostReportService.GetLostReportByIdAsync(reportId);

        if (report == null) {
            return String.Empty;
        }

        if (report.UserId == creatorId) {
            return String.Empty;
        }
        
        var ownerId = report.UserId;
        var existingChatId = await _context.Chats.Where(c => c.LostReportId == reportId && ((c.CreatorId == creatorId && c.OwnerId == ownerId) || (c.CreatorId == ownerId && c.OwnerId == creatorId))).Select(c => c.Id).FirstOrDefaultAsync();
        
        if (!String.IsNullOrEmpty(existingChatId)) {
            return existingChatId;
        }
        
        var newChatId = Guid.NewGuid().ToString();
        
        _context.Chats.Add(new Chat {
            Id = newChatId,
            LostReportId = reportId,
            SightingId = null,
            CreatorId = creatorId,
            OwnerId = ownerId,
            CreatedAt = DateTime.UtcNow
        });
        
        var count = await _context.SaveChangesAsync();

        if (count > 0) {
            return newChatId;
        }
        
        return String.Empty;
    }

    public async Task<ChatDetailsDto?> GetChatDetailsAsync(string chatId, string userId) {
        var chat = await _context.Chats.Include(c => c.Creator).Include(c => c.Owner).Include(c => c.LostReport).Include(c => c.Sighting).ThenInclude(s => s.LostReport).ThenInclude(l => l.Animal).FirstOrDefaultAsync(c => c.Id == chatId);

        if (chat == null) {
            return null;
        }

        if (chat.CreatorId != userId && chat.OwnerId != userId) {
            return null;
        }

        var chatHeaderDto = new ChatHeaderDto() {
            ChatId = chat.Id,
            CreatedAt = chat.CreatedAt
        };

        chatHeaderDto.Creator = new UserShortDto() {
            Id = chat.Creator.Id,
            FirstName = chat.Creator.FirstName,
            LastName = chat.Creator.LastName
        };

        chatHeaderDto.Owner = new UserShortDto() {
            Id = chat.Owner.Id,
            FirstName = chat.Owner.FirstName,
            LastName = chat.Owner.LastName
        };
        
        var animalName = chat.LostReport?.Animal?.Name ?? chat.Sighting?.LostReport?.Animal?.Name ?? "Zwierzę";
        var status = chat.LostReport?.Status ?? chat.Sighting?.LostReport?.Status ?? "";
        
        chatHeaderDto.TargetTitle = string.IsNullOrWhiteSpace(status) ? animalName : $"{animalName} ({status.ToUpper()})";
        
        var messages = await _context.Messages.Where(m => m.ChatId == chat.Id).OrderBy(m => m.SentAt).ToListAsync();
        var messagesDto = new List<MessageDto>();

        foreach (var message in messages) {
            var messageDto = new MessageDto() {
                SenderId = message.SenderId,
                Content = message.Content,
                SentAt = message.SentAt,
                IsMine = message.SenderId == userId
            };
            
            messagesDto.Add(messageDto);
        }

        return new ChatDetailsDto() {
            Chat = chatHeaderDto,
            Messages = messagesDto,
        };
    }

    public async Task<MessageDto?> SendMessageAsync(string chatId, string senderId, string content) {
        if (string.IsNullOrWhiteSpace(content)) {
            return null;
        }
        
        var chat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == chatId);

        if (chat == null) {
            return null;
        }

        if (chat.CreatorId != senderId && chat.OwnerId != senderId) {
            return null;
        }

        var message = new Message {
            Id = Guid.NewGuid().ToString(),
            ChatId = chat.Id,
            SenderId = senderId,
            Content = content.Trim(),
            SentAt = DateTime.UtcNow
        };
        
        _context.Messages.Add(message);
        
        var count = await _context.SaveChangesAsync();

        if (count <= 0) {
            return null;
        }

        return new MessageDto() {
            SenderId = message.SenderId,
            Content = message.Content,
            SentAt = message.SentAt,
            IsMine = true
        };
    }

    public async Task<List<ChatListItemViewModel>> GetUserChatsAsync(string userId) {
        var chats = await _context.Chats.AsNoTracking().Where(c => c.CreatorId == userId || c.OwnerId == userId).Select(
            c => new ChatListItemViewModel { 
                ChatId = c.Id,
                ReceiverId = c.CreatorId == userId ? c.OwnerId : c.CreatorId,
                ReceiverFirstName = c.CreatorId == userId ? c.Owner.FirstName : c.Creator.FirstName,
                ReceiverLastName  = c.CreatorId == userId ? c.Owner.LastName  : c.Creator.LastName,

                LastMessagePreview = _context.Messages.Where(m => m.ChatId == c.Id).OrderByDescending(m => m.SentAt).Select(m => m.Content).FirstOrDefault(),

                LastMessageAt = _context.Messages.Where(m => m.ChatId == c.Id).OrderByDescending(m => m.SentAt).Select(m => (DateTime?)m.SentAt).FirstOrDefault()
            }).OrderByDescending(x => x.LastMessageAt).ToListAsync();

        Console.WriteLine($"Ilosc chatow: {chats.Count}");
        
        return chats;
    }
}
