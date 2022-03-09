using System.Collections.Generic;
using System.Threading.Tasks;
using Application.CommandQueries.CountryDivision.Model;
using Application.CommandQueries.CountryDivision.Query.GetAllCitiesQuery;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class CountryDivisionController : BaseController
    {

        [HttpGet]
        public async Task<IActionResult> GetAllCities([FromQuery] GetAllCitiesQuery model) => Ok(ServiceResult<List<CountryDivisionModel>>.Set(await Mediator.Send(model)));
    }
}
