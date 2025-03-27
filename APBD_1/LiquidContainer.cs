namespace APBD_1;

public class LiquidContainer(
    int height, 
    int depth, 
    double containerMass, 
    double maxCapacity,
    bool containsDangerousCargo
    ) : 
    Container<List<Product>>(
        ContainerType.Liquid, 
        height, depth, 
        containerMass, maxCapacity
    ), IHazardNotifier
{
    
    public List<Product> Liquids { get; set; } = new List<Product>();
    public bool ContainsDangerousCargo { get; set; } = containsDangerousCargo;
    
    public override void Empty()
    {
        PayloadMass = 0;
        Liquids.Clear();
    }

    public override ContainerType GetContainerType()
    {
        return ContainerType.Liquid;
    }

    public override void LoadContainer(List<Product> products) {
        var adjustedMaxCapacity = ContainsDangerousCargo switch
        {
            true => 0.5 * MaxCapacity,
            false => 0.9 * MaxCapacity,
        };
        PayloadMass = Load(products, SendWarning(), adjustedMaxCapacity);
        Liquids = products;
    }
    
    public string SendWarning()
    { 
        return $"Dangerous situation for liquid container: { SerialNumber.Code }: ";
    }

    public override string ToString()
    {
        var liquidsList = string.Join(",", Liquids.Select(liquid => liquid.Name));
        return $"{base.ToString()}\n Cargo: {liquidsList}";
    }
}