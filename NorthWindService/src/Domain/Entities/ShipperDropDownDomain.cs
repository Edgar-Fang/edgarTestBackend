namespace NorthWindService.Domain.Entities;

public class ShipperDropDownDomain
{
    public int Id { get; private set; }
    public string CompanyName { get; private set; }

    public ShipperDropDownDomain(int id, string companyName)
    {
        Id = id;
        CompanyName = companyName;
    }
}