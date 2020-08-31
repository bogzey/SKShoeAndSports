using Microsoft.AspNetCore.Mvc;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKShoeAndSports.Web.ViewComponents
{
    public class SubcategoryMenu : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubcategoryMenu(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var subcategories = _unitOfWork.Subcategory.GetAll().ToList();
            return View(subcategories);
        }
    }
}
