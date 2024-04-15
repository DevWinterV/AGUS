using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AGUS.Data;
using AGUS.Models;
using Microsoft.Identity.Client;

namespace AGUS.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile file, string UserId)
        {
            if (file == null || file.Length == 0)
            {
                TempData["StatusMessage"] = "Chưa chọn ảnh!";
                return Redirect("/Identity/Account/Manage");
            }

            try
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avt");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var userupdate = await _context.Users.FirstOrDefaultAsync(x => x.AccountId == UserId);
                userupdate.Avt = uniqueFileName;
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = "Tải ảnh thành công !";
                return Redirect("/Identity/Account/Manage");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
