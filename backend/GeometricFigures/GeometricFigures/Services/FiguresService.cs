using System.Globalization;
using GeometricFigures.Contracts;
using GeometricFigures.Entities;
using GeometricFigures.Exceptions;
using GeometricFigures.Storages;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

    public async Task<List<FigureContract>> Get(string sortField = nameof(Figure.Id), bool isAscending = true, int pageSize = 5, int pageNumber = 1, string? searchText = null)
    {
        var query = _storage.Figures.AsQueryable();

        if (!string.IsNullOrEmpty(searchText))
        {
            query = query.Where(x =>
                    x.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    x.Area.ToString(CultureInfo.InvariantCulture).Contains(searchText) ||
                    x.Perimeter.ToString(CultureInfo.InvariantCulture).Contains(searchText)
            );
        }

        if (!string.IsNullOrEmpty(sortField))
        {
            var property = typeof(Figure).GetProperty(sortField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
            {
                query = isAscending ? query.OrderBy(x => property.GetValue(x)) : query.OrderByDescending(x => property.GetValue(x));
            }
        }


        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return (await query.ToListAsync()).Select(_contractReconstructionFactory.Create).ToList();
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