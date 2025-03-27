namespace APBD_1;

public class GasContainer(
    int height, 
    int depth, 
    double containerMass, 
    double maxCapacity,
    PressureCalculator pressureCalculator
    ) : 
    Container<Product>(
        ContainerType.Gas, 
        height, depth, 
        containerMass, maxCapacity
    ), IHazardNotifier
{
    public Product? Gas { get; set; }
    public PressureCalculator PressureCalculator { get; set; } = pressureCalculator;
    
    public override void Empty()
    {
        if (Gas is null) {
            PayloadMass = 0; 
        } else {
            Gas.Mass *= 0.05;
            PayloadMass = Gas.Mass;
        }
    }

    public override ContainerType GetContainerType()
    {
        return ContainerType.Gas;
    }

    public override void LoadContainer(Product gas)
    {
        PayloadMass = Load(gas, SendWarning());
        Gas = gas;
    }

    public double GetPressure()
    {
        if(Gas != null)
            return PressureCalculator.CalculatePressure(Gas);
        return 0;
    }
    
    public string SendWarning()
    { 
        return $"Dangerous situation for gas container: { SerialNumber.Code }: ";
    }
    
    public override string ToString()
    {
        return $"{base.ToString()}, Pressure: {GetPressure()} Pa";
    }
}
    