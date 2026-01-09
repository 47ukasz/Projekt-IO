using projekt_io.Models;

namespace projekt_io.Services;

public interface IChatService
{
    Task<List<ChatListItemViewModel>> GetUserChatsAsync(string userId);
    Task<ChatOpenViewModel?> GetChatAsync(string chatId, string userId);
}
