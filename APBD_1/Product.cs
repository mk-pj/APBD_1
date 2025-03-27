namespace APBD_1;

public class Product
{
    public string Name { get; }
    public double MinTemp { get; }

    private double _mass;
    public double Mass
    {
        get => _mass;
        set
        {
            if (value <= 0)
                throw new ArgumentException($"Product-{Name} mass must be greater than zero.");
            _mass = value;
        }
    }

    public Product(string name, double mass, double minTemp)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Invalid product name.");

        if (minTemp is < -40 or > 40)
            throw new ArgumentException($"Invalid required temperature for: {Name}");

        Name = name;
        MinTemp = minTemp;
        Mass = mass;
    }

}
