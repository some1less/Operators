namespace Operators.DAL.Models;

public class PhoneNumber
{
    public int Id { get; set; }
    public int OperatorId { get; set; }
    public int ClientId { get; set; }
    public required string Number { get; set; }
}