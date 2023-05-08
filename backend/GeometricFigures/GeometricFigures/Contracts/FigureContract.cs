using System.ComponentModel.DataAnnotations;

namespace GeometricFigures.Contracts;

public class FigureContract
{
    [Required] public string Name { get; set; }
    [Required] public double Area { get; set; }
    [Required] public double Perimeter { get; set; }
}