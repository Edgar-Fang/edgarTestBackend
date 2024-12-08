namespace NorthWindService.Domain.Entities;

public class EmployeeDropDownDomain
{
    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    
    
    public EmployeeDropDownDomain(int id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public string FullName => $"{FirstName} {LastName}";
}