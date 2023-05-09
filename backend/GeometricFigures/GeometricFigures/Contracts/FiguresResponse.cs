namespace GeometricFigures.Contracts;

public class FiguresResponse
{
    public List<FigureContract> Figures { get; set; }
    public int TotalFigures { get; set; }
}