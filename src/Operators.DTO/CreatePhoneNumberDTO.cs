using Operators.DAL.Models;

namespace Operators.DTO;

public class CreatePhoneNumberDTO
{
    public required string Operator { get; set; }
    public required string MobileNumber { get; set; }
    public required ClientDTO Client { get; set; }
}