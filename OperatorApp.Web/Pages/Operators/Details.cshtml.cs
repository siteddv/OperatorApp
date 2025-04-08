using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OperatorApp.Web.Models;

namespace OperatorApp.Web.Pages.Operators
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        // Constructor injection of HttpClient
        public DetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("APIClient");
        }

        public OperatorModel Operator { get; private set; }

        // OnGet async request to retrieve the operator details.
        public async Task<IActionResult> OnGetAsync(string id)
        {
            Operator = await _httpClient.GetFromJsonAsync<OperatorModel>(
                $"api/operators/{id}");

            if (Operator == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}