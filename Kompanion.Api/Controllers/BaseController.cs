using Kompanion.Api.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace Kompanion.Api.Controllers
{
    public class BaseController: ControllerBase
    {
        public IActionResult ResponseOk<T>(T model)
        {
            var response = new Response<T>();
            response.response = model;

            return new JsonResult(response);
        }

        public IActionResult ResponseCreated<T>(T model)
        {
            var response = new Response<T>();
            response.response = model;

            return new JsonResult(response);
        }
    }
}