using System.Globalization;
using GeometricFigures.Contracts;
using GeometricFigures.Entities;
using GeometricFigures.Exceptions;
using GeometricFigures.Storages;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Expressions;

namespace GeometricFigures.Services;

public class FiguresService : IFiguresService
{
    private readonly FiguresStorage _storage;
    private readonly IFiguresContractReconstructionFactory _contractReconstructionFactory;
    private readonly IFiguresMappingFactory _mappingFactory;

    public FiguresService(FiguresStorage storage, IFiguresContractReconstructionFactory contractReconstructionFactory, IFiguresMappingFactory mappingFactory)
    {
        _storage = storage;
        _contractReconstructionFactory = contractReconstructionFactory;
        _mappingFactory = mappingFactory;
    }

    public async Task Create(FigureContract contract)
    {
        var figure = _mappingFactory.Map(contract);
        await _storage.AddAsync(figure);
        await _storage.SaveChangesAsync();
    }

    public async Task Update(int figureId, FigureContract contract)
    {
        var storageFigure = await _storage.Figures.FirstOrDefaultAsync(f => f.Id == figureId);
        if (storageFigure == null)
        {
            throw new FigureNotFoundException(figureId);
        }
        _mappingFactory.Map(storageFigure, contract);
        await _storage.SaveChangesAsync();
    }

    public async Task<FiguresResponse> Get(string sortField = nameof(Figure.Id), bool isAscending = true, int pageSize = 5, int pageNumber = 1, string? searchText = null)
    {
        var query = _storage.Figures.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            searchText = searchText.ToLower();
            query = query.Where(x =>
                    x.Name.ToLower().Contains(searchText) ||
                    x.Area.ToString().Contains(searchText) ||
                    x.Perimeter.ToString().Contains(searchText)
            );
        }

        if (!string.IsNullOrEmpty(sortField))
        {
            var property = typeof(Figure).GetProperty(sortField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
            {
                var parameter = Expression.Parameter(typeof(Figure), "x");
                var propertyField = Expression.Property(parameter, property);
                var convert = Expression.Convert(propertyField, typeof(object));
                var lambda = Expression.Lambda<Func<Figure, object>>(convert, parameter);
                query = isAscending ? query.OrderBy(lambda) : query.OrderByDescending(lambda);
            }
        }

        var numberOfItems = await query.CountAsync();
        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        var items = (await query.ToListAsync()).Select(_contractReconstructionFactory.Create).ToList();
        return new FiguresResponse { Figures = items, TotalFigures = numberOfItems };
    }

    public async Task<FigureContract> Get(int figureId)
    {
        var figure = await _storage.Figures.FirstOrDefaultAsync(f => f.Id == figureId);
        if (figure == null)
        {
            throw new FigureNotFoundException(figureId);
        }

        return _contractReconstructionFactory.Create(figure);
    }

    public async Task Delete(int figureId)
    {
        var storageFigure = await _storage.Figures.FirstOrDefaultAsync(f => f.Id == figureId);
        if (storageFigure == null)
        {
            throw new FigureNotFoundException(figureId);
        }

        _storage.Figures.Remove(storageFigure);
        await _storage.SaveChangesAsync();
    }
}