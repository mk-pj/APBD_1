namespace APBD_1;

public abstract class Container<T> : IContainer, IEquatable<Container<T>>
{
    public int Height { get; init;  }
    public int Depth { get; init;  }
    public double ContainerMass { get; init; }
    public SerialNumber SerialNumber { get; init; }
    public double MaxCapacity { get; init; }
    
    private double _payloadMass;
    public double PayloadMass
    {
        get => _payloadMass;
        set
        {
            if (DataValidator.ValidateDoubleArgument(value)) 
                _payloadMass = value;
            else 
                throw new ArgumentException($"Invalid payload mass value = {value}");
        }
    }
    
    protected Container(
        ContainerType type, 
        int height, 
        int depth, 
        double containerMass, 
        double maxCapacity
    ) {
        if (DataValidator.ValidateIntArgument(height)) 
            Height = height;
        else 
            throw new ArgumentException($"Invalid container height value = {height}");
        
        if (DataValidator.ValidateDoubleArgument(containerMass)) 
            ContainerMass = containerMass;
        else 
            throw new ArgumentException($"Invalid container mass value = {containerMass}");
        
        if (DataValidator.ValidateIntArgument(depth)) 
            Depth = depth;
        else 
            throw new ArgumentException($"Invalid container depth value = {depth}");
        
        if (DataValidator.ValidateDoubleArgument(maxCapacity)) 
            MaxCapacity = maxCapacity;
        else 
            throw new ArgumentException($"Invalid container max capacity value = {maxCapacity}");
        
        SerialNumber = new SerialNumber(type);
    }

    public abstract void Empty();

    public abstract ContainerType GetContainerType();
    public abstract void LoadContainer(T cargo);
    
    public SerialNumber GetSerialNumber() => SerialNumber;
    
    public double GetMass()
    {
        return ContainerMass + PayloadMass;
    }
    
    protected double Load(
        List<Product> products, 
        string? communicate = null,
        double? adjustedMaxCapacity = null
    ) {
        var productMass = products.Sum(product => product.Mass);
        var capacity = adjustedMaxCapacity ?? MaxCapacity;
        if (productMass > capacity)
            throw new OverfillException(communicate ?? "" + " Product mass exceeds max capacity");
        return productMass;
    }
    
    protected double Load(
        Product product,
        string? communicate = null,
        double? adjustedMaxCapacity = null
    ) {
        var capacity = adjustedMaxCapacity ?? MaxCapacity;
        if(product.Mass > capacity)
            throw new OverfillException(communicate ?? "" + " Product mass exceeds max capacity");
        return product.Mass;
    }

    public bool Equals(Container<T>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return SerialNumber.Equals(other.SerialNumber);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Container<T>)obj);
    }

    public override int GetHashCode()
    {
        return SerialNumber.GetHashCode();
    }
    
    public override string ToString()
    {
        return $"{GetContainerType()} Container - Serial Number: {SerialNumber.Code}, " +
               $"Height: {Height}, Depth: {Depth}, Mass: {ContainerMass}, " +
               $"Max Capacity: {MaxCapacity}, Payload Mass: {PayloadMass}";
    }
}
