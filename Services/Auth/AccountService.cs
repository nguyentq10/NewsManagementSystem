using Microsoft.Extensions.Configuration; // Cần cho IConfiguration
using Repositories; // Cần cho UnitOfWork
using Repositories.DTO;
using Repositories.Models; // Cần cho SystemAccount
using Repositories.UnitOfWork;
using Services.Auth;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AccountService
{
   
    private readonly TokenProvider _tokenProvider;
    private readonly IConfiguration _configuration; 

    public AccountService(TokenProvider tokenProvider, IConfiguration configuration)
    {
        _tokenProvider = tokenProvider;
        _configuration = configuration;
    }
    public async Task<LoginResponse> Authenticate(LoginRequest request)
    {
        string roleName;
        string token;
        string accountId;
        string email;

       
        var adminEmail = _configuration["DefaultAdmin:Email"];
        var adminPassword = _configuration["DefaultAdmin:Password"];

        if (request.Email == adminEmail && request.Password == adminPassword)
        {
            roleName = "Admin"; 
            accountId = "0";    
            email = adminEmail;
        }
        else
        {
            using (var uow = new UnitOfWork())
            {
                var account = await uow.AccountRepository.GetOne(request.Email,request.Password);    

                roleName = account.AccountRole switch
                {
                    1 => "Staff",
                    2 => "Lecturer",
                    _ => "Unknown" 
                };

                accountId = account.AccountId.ToString();
                email = account.AccountEmail;
            }
        }

        token = _tokenProvider.GenerateToken(
            accountId,
            email,
            new List<string> { roleName }
        );

      
        return new LoginResponse
        {
            Token = token,
            Role = roleName
        };
    }
}