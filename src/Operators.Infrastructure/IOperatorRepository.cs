using Operators.DAL.Models;
using Operators.DTO;

namespace Operators.Infrastructure;

public interface IOperatorRepository
{
    IEnumerable<PhoneNumber> GetPhoneNumbers();
    Operator? GetOperatorById(int id);
    Operator? GetOperatorByName(string name);
    Client? GetClientById(int id);
    Client? GetClientByEmail(string email);
    
    PhoneNumber CreatePhoneNumber(PhoneNumDTO dto);
    Client CreateClient(ClientDTO client);
    Client UpdateClient(Client client);

}