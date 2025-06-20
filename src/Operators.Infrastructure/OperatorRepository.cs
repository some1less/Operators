using Microsoft.Data.SqlClient;
using Operators.DAL.Models;
using Operators.DTO;

namespace Operators.Infrastructure;

public class OperatorRepository : IOperatorRepository
{
    
    private readonly string _connectionString;

    public OperatorRepository(string connectionString)
    {
        _connectionString = connectionString;
    }


    public IEnumerable<PhoneNumber> GetPhoneNumbers()
    {
        List<PhoneNumber> numbers = [];
        const string sqlString = "SELECT * FROM PhoneNumber";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand(sqlString, conn);
            
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var row = new PhoneNumber()
                        {
                            Id = reader.GetInt32(0),
                            OperatorId = reader.GetInt32(1),
                            ClientId = reader.GetInt32(2),
                            Number = reader.GetString(3),
                        };

                        numbers.Add(row);
                    }
                }
            }
            finally
            {
                reader.Close();
            }
            return numbers;
            
        }
    }

    public Operator? GetOperatorById(int id)
    {
        const string sqlString = @"SELECT Id, Name
                                    FROM Operator
                                    WHERE Id = @Id";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand(sqlString, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            
            conn.Open();
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Operator()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                    };
                }
            }
            
            return null;
        }
    }

    public Operator? GetOperatorByName(string name)
    {
        const string sqlString = @"SELECT Id, Name
                                    FROM Operator
                                    WHERE Name = @Name";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand(sqlString, conn);
            cmd.Parameters.AddWithValue("@Name", name);
            
            conn.Open();
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Operator()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                    };
                }
            }
            
            return null;
        }
    }

    public Client? GetClientById(int id)
    {
        const string sqlString = @"SELECT Id, Fullname, Email, City
                                    FROM Client
                                    WHERE Id = @Id";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand(sqlString, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            
            conn.Open();
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Client()
                    {
                        Id = reader.GetInt32(0),
                        Fullname = reader.GetString(1),
                        Email = reader.GetString(2),
                        City = reader.IsDBNull(3) ? null : reader.GetString(3),
                    };
                }
            }
            
            return null;
        }
    }

    public Client? GetClientByEmail(string email)
    {
        const string sqlString = @"SELECT Id, Fullname, Email, City
                                    FROM Client
                                    WHERE Email = @Email";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand(sqlString, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            
            conn.Open();
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Client()
                    {
                        Id = reader.GetInt32(0),
                        Fullname = reader.GetString(1),
                        Email = reader.GetString(2),
                        City = reader.IsDBNull(3) ? null : reader.GetString(3),
                    };
                }
            }
            
            return null;
        }
    }

    public PhoneNumber CreatePhoneNumber(PhoneNumDTO dto)
    {
        
        const string insertSql = @"INSERT INTO 
                PhoneNumber (Operator_Id, Client_Id, Number) 
                        OUTPUT INSERTED.Id
                        VALUES (@Operator_Id, @Client_Id, @Number)";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            
            SqlCommand cmdInsert = new SqlCommand(insertSql, conn);
            cmdInsert.Parameters.AddWithValue("@Operator_Id", dto.Operator_Id);
            cmdInsert.Parameters.AddWithValue("@Client_Id", dto.Client_Id);
            cmdInsert.Parameters.AddWithValue("@Number", dto.Number);

            var newId = (int)cmdInsert.ExecuteScalar();
            
            conn.Close();

            return new PhoneNumber()
            {
                Id = newId,
                OperatorId = dto.Operator_Id,
                ClientId = dto.Client_Id,
                Number = dto.Number
            };
            
        }
    }

    public Client CreateClient(ClientDTO client)
    {
        const string insertSql = @"INSERT INTO 
                Client (Fullname, Email, City) 
                        OUTPUT INSERTED.Id
                        VALUES (@Fullname, @Email, @City)";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            
            SqlCommand cmdInsert = new SqlCommand(insertSql, conn);
            cmdInsert.Parameters.AddWithValue("@Fullname", client.Fullname);
            cmdInsert.Parameters.AddWithValue("@Email", client.Email);
            if (client.City != null)
            {               
                cmdInsert.Parameters.AddWithValue("@City", client.City);
            }
            else
            {
                cmdInsert.Parameters.AddWithValue("@City", DBNull.Value);
            }
            
            
            var newId = (int)cmdInsert.ExecuteScalar();
            
            conn.Close();

            return new Client()
            {
                Id = newId,
                Fullname = client.Fullname,
                Email = client.Email,
                City = client.City
            };
            
        }
    }

    public Client UpdateClient(Client client)
    {
        const string updateSql = @"UPDATE Client
                                    SET Fullname = @Fullname, Email = @Email , City = @City
                                    WHERE Id = @Id";
        
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            
            SqlCommand cmdUpdate = new SqlCommand(updateSql, conn);
            
            cmdUpdate.Parameters.AddWithValue("@Id", client.Id);
            cmdUpdate.Parameters.AddWithValue("@Fullname", client.Fullname);
            cmdUpdate.Parameters.AddWithValue("@Email", client.Email);
            if (client.City != null)
            {               
                cmdUpdate.Parameters.AddWithValue("@City", client.City);
            }
            else
            {
                cmdUpdate.Parameters.AddWithValue("@City", DBNull.Value);
            }
            
            
            var newId = (int)cmdUpdate.ExecuteScalar();
            
            conn.Close();

            return new Client()
            {
                Id = newId,
                Fullname = client.Fullname,
                Email = client.Email,
                City = client.City
            };
            
        }
    }
}