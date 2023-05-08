using GeometricFigures.Contracts;
using GeometricFigures.Entities;

namespace GeometricFigures.Services;

public interface IFiguresService
{
    Task Create(FigureContract contract);
    Task Update(int figureId, FigureContract contract);
    Task<List<FigureContract>> Get(string sortField = nameof(Figure.Id), bool isAscending = true, int pageSize = 5,
        int pageNumber = 1, string? searchText = null);
    Task Delete(int figureId);
}