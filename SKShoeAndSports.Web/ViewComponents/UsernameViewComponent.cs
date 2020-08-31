using Microsoft.AspNetCore.Mvc;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SKShoeAndSports.Web.ViewComponents
{
    public class UsernameViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsernameViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(i => i.Id == claims.Value);

            return View(user);
        }
    }
}
