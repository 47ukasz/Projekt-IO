using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using projekt_io.Models;

namespace projekt_io.ViewComponents;

public class RolesModalViewComponent : ViewComponent {
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesModalViewComponent(RoleManager<IdentityRole> roleManager) {
        _roleManager = roleManager;
    }

    public IViewComponentResult Invoke(RolesModalViewModel viewModel) {
        viewModel.AvailableRoles = _roleManager.Roles.Select(role => role.Name).ToList();
        
        return View(viewModel);
    }
}