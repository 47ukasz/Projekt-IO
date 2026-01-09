using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projekt_io.Services;

namespace projekt_io.Controllers;

[Authorize]
[Route("chat")]
public class ChatController : Controller
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var chats = await _chatService.GetUserChatsAsync(userId);
        return View(chats);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Open(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var vm = await _chatService.GetChatAsync(id, userId);

        if (vm == null)
        {
            return Forbid();
        }

        return View(vm);
    }
}
