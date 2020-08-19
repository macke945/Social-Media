using Microsoft.AspNetCore.Http;
using Social_Media.Data;
using Social_Media.Data.DataTables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Social_Media.Services
{
    public class ProfileService
    {
        private readonly ApplicationDbContext context;

        public ProfileService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool IsImage(IFormFile formFile)
        {
            if (!string.Equals(formFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(formFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(formFile.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(formFile.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(formFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }


            var postedFileExtension = Path.GetExtension(formFile.FileName);
            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;

        }
        public string GetUserNameById(string id)
        {
            var user = context.Users
                .Find(id);

            return user.UserName;


        }

        public void EditProfile(ApplicationUser user)
        {
            context.Users.Update(user);
            context.SaveChanges();
        }
        public string GetProfileImagePathByUserId(string id)
        {
            var user = context.Users
                .Find(id);

            return user.ProfileImagePath;

        }
    }
}
