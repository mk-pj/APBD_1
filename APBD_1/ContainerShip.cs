namespace APBD_1;

public class ContainerShip
{
    public double MaxSpeed { get; }
    public int MaxContainerCount { get; }
    public double MaxMass { get; }

    private double _currentMass;
    public HashSet<IContainer> Containers { get; }

    public ContainerShip(double maxSpeed, int maxContainerCount, double maxMass)
    {
        if(DataValidator.ValidateIntArgument(maxContainerCount))
            MaxContainerCount = maxContainerCount;
        else
            throw new ArgumentException("Invalid container count");
        if(DataValidator.ValidateDoubleArgument(maxMass))
            MaxMass = maxMass;
        else 
            throw new ArgumentException("Invalid max mass value");
        if(DataValidator.ValidateDoubleArgument(maxSpeed))
            MaxSpeed = maxSpeed;
        else 
            throw new ArgumentException("Invalid max speed value");
        Containers = new HashSet<IContainer>();
    }

    public bool LoadContainer(IContainer container)
    {
        if (Containers.Count >= MaxContainerCount)
            return false;
        var containerMass = container.GetMass();
        var totalMass = _currentMass + containerMass;
        if (totalMass > MaxMass)
            return false;
        if (!Containers.Add(container)) return false;
        _currentMass = totalMass;
        return true;
    }

    public bool LoadContainers(HashSet<IContainer> containers)
    {
        if (Containers.Count + containers.Count > MaxContainerCount)
            return false;
        var totalMass = _currentMass;
        foreach (var c in containers)
        {
            if(Containers.Contains(c))
                return false;
            totalMass += c.GetMass();
            if (totalMass > MaxMass)
                return false;
        }
        _currentMass = totalMass;
        Containers.UnionWith(containers);
        return true;
    }

    public bool RemoveContainer(IContainer container) => Containers.Remove(container);

    public bool RemoveContainer(int id)
    {
        IContainer? toRemove = FindContainer(id);
        if(toRemove != null)
            return RemoveContainer(toRemove);
        return false;
    }

    public IContainer? FindContainer(int id)
    { 
        return Containers.FirstOrDefault(c => c.GetSerialNumber().Id.Equals(id));
    }
    
    public IContainer? FindContainer(SerialNumber serialNumber)
    { 
        return Containers.FirstOrDefault(c => c.GetSerialNumber() == serialNumber);
    }

    public bool ReplaceContainer(int id, IContainer newContainer)
    {
        var oldContainer = FindContainer(id);
        if(oldContainer == null)
            return false;
        return ReplaceContainer(oldContainer, newContainer);
    }

    public bool ReplaceContainer(IContainer oldContainer, IContainer newContainer)
    {
        if (!RemoveContainer(oldContainer)) return false;
        if(LoadContainer(newContainer))
            return true;
        LoadContainer(oldContainer);
        return false;
    }

    public bool TransferContainer(IContainer container, ContainerShip targetShip)
    {
        if (!RemoveContainer(container)) return false;
        if(targetShip.LoadContainer(container))
            return true;
        LoadContainer(container);
        return false;
    }
    
    public bool TransferContainer(int id, ContainerShip targetShip)
    {
        var container = FindContainer(id);
        if (container == null)
            return false;
        return TransferContainer(container, targetShip);
    }
    
    
    public override string ToString()
    {
        var containerDetails = string.Join("\n", Containers.Select(c => c.ToString()));
        return $"Container Ship - Max Speed: {MaxSpeed} knots, Max Container Count: {MaxContainerCount}, Max Mass: {MaxMass} kg, Current Mass: {_currentMass} kg\n" +
               $"Number of Containers: {Containers.Count}\n" +
               $"Containers on Board:\n{containerDetails}";
    }
    
}