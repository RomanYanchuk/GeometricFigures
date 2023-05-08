using GeometricFigures.Contracts;
using GeometricFigures.Entities;

namespace GeometricFigures.Services
{
    public interface IFiguresMappingFactory
    {
        Figure Map(FigureContract contract);
        void Map(Figure storageFigure, FigureContract contract);
    }
}
