using Operators.DAL.Models;
using Operators.DTO;

namespace Operators.Application;

public interface IOperatorService
{
     IEnumerable<GetPhoneNumbersDTO> GetPhoneNumbers();
     PhoneNumber CreatePhoneNumber(CreatePhoneNumberDTO dto);
}