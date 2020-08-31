using Microsoft.AspNetCore.Mvc;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKShoeAndSports.Web.ViewComponents
{
    public class ProductTypeMenu : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductTypeMenu(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var productTypes = _unitOfWork.ProductType.GetAll().ToList();
            return View(productTypes);
        }
    }
}
