using Microsoft.AspNetCore.Mvc;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKShoeAndSports.Web.ViewComponents
{
    public class CategoryMenu : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryMenu(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = _unitOfWork.Category.GetAll().ToList();
            return View(categories);
        }
    }
}
