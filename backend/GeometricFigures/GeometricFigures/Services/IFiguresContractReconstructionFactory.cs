using GeometricFigures.Contracts;
using GeometricFigures.Entities;

namespace GeometricFigures.Services;

public interface IFiguresContractReconstructionFactory
{
    FigureContract Create(Figure figure);
}