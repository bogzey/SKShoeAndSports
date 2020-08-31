using Microsoft.AspNetCore.Mvc;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKShoeAndSports.Web.ViewComponents
{
    public class BrandMenu : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrandMenu(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var brands = _unitOfWork.Brand.GetAll().ToList();
            return View(brands);
        }
    }
}
