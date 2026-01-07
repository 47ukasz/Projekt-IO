using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projekt_io.Models;
using projekt_io.Services;

namespace projekt_io.Controllers;

[Authorize]
[Route("profile")]
public class ProfileController : Controller
{
    private readonly ILogger<ProfileController> _logger;
    private readonly IChatService _chatService;

    public ProfileController(ILogger<ProfileController> logger, IChatService chatService)
    {
        _logger = logger;
        _chatService = chatService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(string tab = "reports")
    {
        var vm = new ProfileViewModel
        {
            CurrentTab = tab
        };

        if (tab == "messages")
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            vm.Chats = await _chatService.GetUserChatsAsync(userId);
        }

        return View(vm);
    }
}
