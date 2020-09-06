using Microsoft.AspNetCore.Mvc.RazorPages;
using PickupAnnouncerLegacy.Interfaces;
using System.Threading.Tasks;

namespace PickupAnnouncerLegacy.Pages
{
    public class AnnounceModel : PageModel
    {
        private readonly IDbHelper _dbHelper;

        public AnnounceModel(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public int NumberOfCones { get; set; }
        public async Task OnGet()
        {
            NumberOfCones = await _dbHelper.GetNumberOfCones();
        }
    }
}