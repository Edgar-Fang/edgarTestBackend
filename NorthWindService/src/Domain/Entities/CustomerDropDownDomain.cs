namespace NorthWindService.Domain.Entities;

public class CustomerDropDownDomain
{
    public CustomerDropDownDomain(string id, string companyName)
    {
        Id = id;
        CompanyName = companyName;
    }

    public string Id { get; private set; }
    public string CompanyName { get; private set; }
}