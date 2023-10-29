namespace NhlCommands.Extensions;

public static class MiscExtensions
{
    

    public static int GetAge(this DateTime birthDate)
    {
        var currentDate = DateTime.Now;
        var age = currentDate.Year - birthDate.Year;
        if (birthDate.Date > currentDate.AddYears(-age)) age--;
        return age;
    }
}