using Microsoft.AspNetCore.Mvc;
using projekt_io.Models;

namespace projekt_io.ViewComponents;

public class ModalViewComponent : ViewComponent{

    public IViewComponentResult Invoke(ModalViewModel viewModel) {
        return View(viewModel);
    }
    
}