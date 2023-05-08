using System.Net;

namespace GeometricFigures.Exceptions;

public class FigureNotFoundException : FigureModelException
{
    private const int NotFoundCode = (int)HttpStatusCode.NotFound;
    public FigureNotFoundException(int id) : base(NotFoundCode, $"Figure with id {id} is not found.")
    { }
}