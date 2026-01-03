using Microsoft.AspNetCore.Mvc;
using projekt_io.Models;

namespace projekt_io.ViewComponents;

public class ConfirmModalViewComponent : ViewComponent{

    public IViewComponentResult Invoke(ConfirmModalViewModel viewModel) {
        return View(viewModel);
    }
    
}