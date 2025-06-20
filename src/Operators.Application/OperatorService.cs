using Operators.Application.Exceptions;
using Operators.DAL.Models;
using Operators.DTO;
using Operators.Infrastructure;

namespace Operators.Application;

public class OperatorService : IOperatorService
{
    
    private readonly IOperatorRepository _repository;

    public OperatorService(IOperatorRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<GetPhoneNumbersDTO> GetPhoneNumbers()
    {
        var numbers = _repository.GetPhoneNumbers();
        List<GetPhoneNumbersDTO> phoneNumbers = [];

        foreach (var number in numbers)
        {
            var ope = _repository.GetOperatorById(number.OperatorId);
            var client = _repository.GetClientById(number.ClientId);
                
            phoneNumbers.Add(new GetPhoneNumbersDTO()
            {
                Id = number.Id,
                Number = number.Number,
                Operator = ope!.Name,
                Client = client!
            });
        }
        
        return phoneNumbers;
    }

    public PhoneNumber CreatePhoneNumber(CreatePhoneNumberDTO dto)
    {
        if (dto.MobileNumber.Length > 20 || !dto.MobileNumber.StartsWith("+48"))
        {
            throw new ClientErrorException("Invalid mobile number");
        }
        
        if (!string.IsNullOrEmpty(dto.Client.Email) && (string.IsNullOrEmpty(dto.Client.Fullname) || string.IsNullOrEmpty(dto.Client.City)))
        {
            var client = _repository.GetClientByEmail(dto.Client.Email);
            if (client is null) throw new KeyNotFoundException($"Invalid with {dto.Client.Email} not found");
        }

        if (!string.IsNullOrEmpty(dto.Client.Email) && !string.IsNullOrEmpty(dto.Client.Fullname))
        {
            var client = _repository.GetClientByEmail(dto.Client.Email);
            
            if (client is null)
            {
                var newClient = new ClientDTO()
                {
                    Fullname = dto.Client.Fullname,
                    Email = dto.Client.Email,
                    City = dto.Client.City,
                };
            
                _repository.CreateClient(newClient);
            }

            var updateClient = new Client()
            {
                Id = client!.Id,
                Email = dto.Client.Email,
                Fullname = dto.Client.Fullname,
                City = dto.Client.City,
            };
            
            _repository.UpdateClient(updateClient);

        }
        
        var opera = _repository.GetOperatorByName(dto.Operator);
        var findClient = _repository.GetClientByEmail(dto.Client.Email);

        var phoneNumber = new PhoneNumDTO()
        {
            Operator_Id = opera.Id,
            Client_Id = findClient.Id,
            Number = dto.MobileNumber,
        };
        
        return _repository.CreatePhoneNumber(phoneNumber);
    }
}