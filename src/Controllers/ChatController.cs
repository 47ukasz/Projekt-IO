using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projekt_io.Entities;
using projekt_io.Services;

namespace projekt_io.Controllers;

[Authorize]
[Route("chat")]
public class ChatController : Controller
{
    private readonly IChatService _chatService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ChatController(IChatService chatService, UserManager<ApplicationUser> userManager) {
        _chatService = chatService;
        _userManager = userManager;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Open(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var vm = await _chatService.GetChatDetailsAsync(id, userId);

        if (vm == null) {
            return Forbid();
        }

        return View("Open", vm); 
    }
    
    [HttpPost("create/{reportId}")]
    public async Task<IActionResult> Create(string reportId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var chatId = await _chatService.CreateAsync(reportId, userId);

        if (string.IsNullOrEmpty(chatId)) {
            return BadRequest(); 
        }

        return RedirectToAction(nameof(Open), new { id = chatId });
    }
}
