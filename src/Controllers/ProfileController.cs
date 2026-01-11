using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projekt_io.DTOs;
using projekt_io.Entities;
using projekt_io.Models;
using projekt_io.Services;

namespace projekt_io.Controllers;

[Route("profile")]
public class ProfileController : Controller {
    private readonly ILogger<ProfileController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILostReportService _lostReportService;
    private readonly IUserService _userService;
    private readonly ISightingService _sightingService;
    private readonly IChatService _chatService;
    
    public ProfileController(ILogger<ProfileController> logger, UserManager<ApplicationUser> userManager, ILostReportService lostReportService, IUserService userService, ISightingService sightingService, IChatService chatService) {
        _logger = logger;
        _userManager = userManager;
        _lostReportService = lostReportService;
        _userService = userService;
        _sightingService = sightingService;
        _chatService = chatService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(string tab = "reports", int reportsPage = 1, int reportsPageSize = 5) {
        var userId = _userManager.GetUserId(User);
        var userSightings = await _sightingService.GetSightingsByIdAsync(userId);
        var userDto = await _userService.GetUserById(userId);
        var userChats = await _chatService.GetUserChatsAsync(userId);
        
        var isAdmin = User.IsInRole("Admin");

        if (tab == "users" && !isAdmin) {
            return Forbid();
        }
        
        var reportsPaginated = await _lostReportService.GetLostReportsByIdAsync(userId, reportsPage, reportsPageSize);
        var reports = reportsPaginated.Value.Items;
        var reportsTotal = reportsPaginated.Value.TotalCount;

        var reportsViewModel = new ListViewModel<LostReportDto>() {
            Items = reports,
            PaginationInfo = new PaginationInfoModel() {
                Page = reportsPage < 1 ? 1 : reportsPage,
                PageSize = reportsPageSize < 1 ? 5 : reportsPageSize,
                TotalCount = reportsTotal
            }
        };
        
        var viewModel = new ProfileViewModel() {
            CurrentTab = tab,
            CurrentUserId = userId,
            Reports = reportsViewModel,
            Sightings = userSightings,
            UserFullName = userDto.FirstName + " " + userDto.LastName,
            UserEmail = userDto.Email,
            Chats = userChats,
        };

        if (tab == "users" && isAdmin) {
            viewModel.Users = await _userService.GetAllUsersAsync();
        }
        
        return View(viewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> UserProfile(string userId, string tab = "reports", int reportsPage = 1, int reportsPageSize = 5) {
        var isAdmin = User.IsInRole("Admin");
        
        if (!isAdmin || string.IsNullOrEmpty(userId)) {
            return Forbid();
        }
        
        var userDto = await _userService.GetUserById(userId);

        if (userDto == null) {
            return NotFound();
        }
        
        var userSightings = await _sightingService.GetSightingsByIdAsync(userId);
        
        var reportsPaginated = await _lostReportService.GetLostReportsByIdAsync(userId, reportsPage, reportsPageSize);
        var reports = reportsPaginated.Value.Items;
        var reportsTotal = reportsPaginated.Value.TotalCount;
        
        var reportsViewModel = new ListViewModel<LostReportDto>() {
            Items = reports,
            PaginationInfo = new PaginationInfoModel() {
                Page = reportsPage < 1 ? 1 : reportsPage,
                PageSize = reportsPageSize < 1 ? 5 : reportsPageSize,
                TotalCount = reportsTotal
            }
        };
        
        var viewModel = new ProfileViewModel() {
            CurrentTab = tab,
            CurrentUserId = userId,
            Reports = reportsViewModel,
            Sightings = userSightings,
            UserFullName = userDto.FirstName + " " + userDto.LastName,
            UserEmail = userDto.Email,
            IsAdminView = isAdmin,
            TargetUserId = userId,
        };

        return View("Index", viewModel);
    }
}