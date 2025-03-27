namespace APBD_1;

public interface IContainer
{
    ContainerType GetContainerType();
    SerialNumber GetSerialNumber();
    void Empty();
    
    double GetMass();
    string ToString();
    
    
}