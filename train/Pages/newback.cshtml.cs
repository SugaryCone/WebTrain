using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using train.Model;
using train.Servise;

namespace train.Pages
{
    public class newbackModel : PageModel
    {

        private readonly IBackgrounds service;
        [BindProperty]
        public Background Back { get; set; } = new();
        public newbackModel(IBackgrounds service)
        {
            this.service = service;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            service.SetBack(Back);
            return RedirectToPage("Index");
        }
    }
}
