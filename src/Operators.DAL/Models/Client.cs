namespace Operators.DAL.Models;

public class Client
{
    public int Id { get; set; }
    public required string Fullname { get; set; }
    public required string Email { get; set; }
    public string? City { get; set; }
}