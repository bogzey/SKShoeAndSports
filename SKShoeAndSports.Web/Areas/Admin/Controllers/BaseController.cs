using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SKShoeAndSports.Web.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        public enum AlertType { success, danger, warning, info }
        public void Alert(string message, AlertType type = AlertType.info)
        {
            TempData["Alert.Message"] = message;
            TempData["Alert.Type"] = type.ToString();
        }
    }
}
