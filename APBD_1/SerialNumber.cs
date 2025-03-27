namespace APBD_1;

public class SerialNumber : IEquatable<SerialNumber>
{
    
    private static int _id;
    public int Id { get; }
    public string Code { get; }
    private const string Prefix = "KON";
    
    private static int IncrementId() => ++_id;
    
    public SerialNumber(ContainerType type)
    {
        Id = IncrementId();
        var typeCode = type switch
        {
            ContainerType.Cooling => "C",
            ContainerType.Gas => "G",
            ContainerType.Liquid => "L",
            _ => throw new UnknownContainerTypeException("Unknown container type.")
        };
        Code = $"{ Prefix }-{ typeCode }-{ Id }";
    }

    public bool Equals(SerialNumber? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id && Code == other.Code;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((SerialNumber)obj);
    }
    
    public static bool operator ==(SerialNumber? left, SerialNumber? right) => Equals(left, right);

    public static bool operator !=(SerialNumber? left, SerialNumber? right) => !(left == right);

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }
}