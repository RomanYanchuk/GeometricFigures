using GeometricFigures.Contracts;
using GeometricFigures.Entities;

namespace GeometricFigures.Services;

public class FiguresMappingFactory : IFiguresMappingFactory
{
    public Figure Map(FigureContract contract)
    {
        var figure = new Figure();
        Map(figure, contract);
        return figure;
    }

    public void Map(Figure storageFigure, FigureContract contract)
    {
        storageFigure.Name = contract.Name;
        storageFigure.Area = contract.Area;
        storageFigure.Perimeter = contract.Perimeter;
    }
}