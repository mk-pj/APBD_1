namespace APBD_1;

public static class DataValidator
{
    public static bool ValidateDoubleArgument(double number)
    {
        return number >= 0 && !double.IsNaN(number) && !double.IsInfinity(number);
    }

    public static bool ValidateIntArgument(int number)
    {
        return number >= 0;
    }
    
}