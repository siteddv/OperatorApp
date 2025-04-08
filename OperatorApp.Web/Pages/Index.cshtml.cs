using Microsoft.AspNetCore.Mvc.RazorPages;
using OperatorApp.Core.Entities;

namespace OperatorApp.Web.Pages.Operators
{
    public class IndexModel(IHttpClientFactory httpClientFactory) : PageModel
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("APIClient");

        public IList<Operator> Operators { get; set; } = new List<Operator>();

        public async Task OnGetAsync()
        {
            Operators = await _httpClient.GetFromJsonAsync<List<Operator>>("operators")
                        ?? new List<Operator>();
        }
    }
}