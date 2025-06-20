using Operators.DAL.Models;

namespace Operators.DTO;

public class GetPhoneNumbersDTO
{
    public int Id { get; set; }
    public required string Number { get; set; }
    public required string Operator { get; set; }
    public required Client Client { get; set; }
}