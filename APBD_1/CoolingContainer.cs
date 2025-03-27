namespace APBD_1;

public class CoolingContainer : Container<Product>
{
   
    private double _temperature;
    public double Temperature
    {
        get => _temperature;
        set
        {
            if (value is < -30 or > 30)
                throw new ArgumentException("Temperature must be between -30 and 30.");
            _temperature = value;
        }
    }
    public Product? Product { get; set; }
    
    public CoolingContainer(
        int height, 
        int depth, 
        double containerMass, 
        double maxCapacity, 
        double temperature
        ) : base(ContainerType.Cooling, height, depth, containerMass, maxCapacity)
    {
        Temperature = temperature;
    }
    
    public override void Empty()
    {
        Product = null;
        PayloadMass = 0;
    }

    public override ContainerType GetContainerType()
    {
        return ContainerType.Cooling;
    }

    public override void LoadContainer(Product product)
    {
        if(product.MinTemp > Temperature)
            throw new ArgumentException($"Container temp = {Temperature} < min temp for {product.Name} = {product.MinTemp}");
        PayloadMass = Load(product);
        Product = product;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, Temperature: {Temperature}";
    }
    
}
    
