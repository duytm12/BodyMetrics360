namespace Domain.Interfaces;

public interface IGetWtHR
{
    Task<double> CalculateWtHRAsync(double waist, double height);
}
