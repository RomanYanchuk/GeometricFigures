using GeometricFigures.Contracts;
using GeometricFigures.Entities;
using GeometricFigures.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeometricFigures.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiguresController : ControllerBase
    {
        private readonly IFiguresService _figuresService;


        public FiguresController(IFiguresService figuresService)
        {
            _figuresService = figuresService;
        }

        [HttpPost]
        public async Task Create(FigureContract contract)
        {
            await _figuresService.Create(contract);
        }

        [HttpPut("{id:int}")]
        public async Task Update(FigureContract contract, int id)
        {
            await _figuresService.Update(id, contract);
        }

        [HttpGet]
        public async Task<List<FigureContract>> Get(string sortField = nameof(Figure.Id), bool isAscending = true,
            int pageSize = 5, int pageNumber = 1, string? searchText = null)
        {
            return await _figuresService.Get(sortField, isAscending, pageSize, pageNumber, searchText);
        }


        [HttpDelete("{id:int}")]
        public async Task Delete(int id)
        {
            await _figuresService.Delete(id);
        }
    }
}
