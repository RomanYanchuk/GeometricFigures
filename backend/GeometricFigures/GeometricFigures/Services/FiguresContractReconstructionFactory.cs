using GeometricFigures.Contracts;
using GeometricFigures.Entities;

namespace GeometricFigures.Services;

public class FiguresContractReconstructionFactory : IFiguresContractReconstructionFactory
{
    public FigureContract Create(Figure figure) =>
        new() {Name = figure.Name, Area = figure.Area, Perimeter = figure.Perimeter};
}