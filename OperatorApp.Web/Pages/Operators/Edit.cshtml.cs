using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OperatorApp.Web.Models;
using System.Net.Http;
using System.Net.Http.Json;
using OperatorApp.Core.Entities;

namespace OperatorApp.Web.Pages.Operators
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("APIClient");
        }

        [BindProperty] public Operator Operator { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Operator = await _httpClient.GetFromJsonAsync<Operator>($"operators/{id}");

            if (Operator == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var response = await _httpClient.PutAsJsonAsync($"operators/{Operator.Code}", Operator);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("./Index");

            ModelState.AddModelError(string.Empty, "Unable to update Operator.");
            return Page();
        }
    }
}