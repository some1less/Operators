namespace Operators.DTO;

public class PhoneNumDTO
{
    public int Operator_Id { get; set; }
    public int Client_Id { get; set; }
    public required string Number { get; set; }
}