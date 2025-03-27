namespace APBD_1;

public class PressureCalculator(
    Func<Product, double> basicModel,
    Dictionary<string, double>? parameters = null,
    Func<Product, Dictionary<string, double>, double>? customModel = null
    )
{
    private Func<Product, double> BasicModel { get; set;  } = basicModel;
    private Dictionary<string, double>? Parameters { get; set; } = parameters;
    private Func<Product, Dictionary<string, double>, double>? CustomModel { get; set; } = customModel;

    
    public double CalculatePressure(Product gas)
    {
        if(CustomModel == null || Parameters == null)
            return BasicModel(gas);
        return CustomModel(gas, Parameters);
    }
}