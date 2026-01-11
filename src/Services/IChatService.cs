using projekt_io.DTOs;
using projekt_io.Models;

namespace projekt_io.Services;

public interface IChatService {
    public Task<string> CreateAsync(string reportId, string creatorId);
    public Task<ChatDetailsDto?> GetChatDetailsAsync(string chatId, string userId);
    public Task<MessageDto?> SendMessageAsync(string chatId, string senderId, string content);
    public Task<List<ChatListItemViewModel>> GetUserChatsAsync(string userId);
}
