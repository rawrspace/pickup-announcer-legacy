using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using PickupAnnouncerLegacy.Interfaces;
using System;
using System.Threading.Tasks;

namespace PickupAnnouncerLegacy.Pages
{
    public class AdminModel : PageModel
    {
        private readonly IRegistrationFileHelper _registrationFileHelper;
        private readonly IToastNotification _toastNotification;
        private readonly IDbHelper _dbHelper;

        public AdminModel(IRegistrationFileHelper registrationFileHelper, IToastNotification toastNotification, IDbHelper dbHelper)
        {
            _registrationFileHelper = registrationFileHelper;
            _toastNotification = toastNotification;
            _dbHelper = dbHelper;
        }

        public int NumberOfCones { get; set; }

        public async Task OnGet()
        {
            NumberOfCones = await _dbHelper.GetNumberOfCones();
        }
        public async Task OnPostInsert(IFormFile registrationFile)
        {
            try
            {
                await _registrationFileHelper.ProcessFile(registrationFile);
                _toastNotification.AddSuccessToastMessage("Records updated");
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage(e.Message);
            }
        }

        public async Task OnPostDelete()
        {
            try
            {
                await _registrationFileHelper.DeleteAll();
                _toastNotification.AddSuccessToastMessage("Records deleted");
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage(e.Message);
            }
        }

        public async Task<IActionResult> OnPostDownload()
        {
            var outputStream = await _dbHelper.GetRegistrationDetailsStream();
            return new FileStreamResult(outputStream, "application/octet-stream")
            {
                FileDownloadName = $"RegistrationDetails-{DateTime.Now:yyyy-MM-dd-HH-mm}.csv"
            };
        }
    }
}