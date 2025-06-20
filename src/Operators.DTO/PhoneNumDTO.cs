namespace Operators.DTO;

public class PhoneNumDTO
{
    public int OperatorId { get; set; }
    public int ClientId { get; set; }
    public required string Number { get; set; }
}