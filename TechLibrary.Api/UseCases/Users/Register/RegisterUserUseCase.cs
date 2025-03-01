using FluentValidation.Results;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Api.Infrastructure.Security.Criptography;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;
using TechLibrary.Api.Infrastructure.Security.Token.Access;
namespace TechLibrary.Api.UseCases.Users.Register;

public class RegisterUserUseCase
{
    public ResponseRegisterUserJson Execute(RequestUserJson request)
    {
        var dbContext = new TechLibraryDbContext();

        Validate(request, dbContext);
        
        var cryptography = new BCryptAlgorithm();

        var entity = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = cryptography.HashPassword(request.Password)
        };

        dbContext.Users.Add(entity);
        dbContext.SaveChanges();

        var tokenGenerator = new TokenJwtGenerator();

        return new ResponseRegisterUserJson
        {
            Name = entity.Name,
        }; 
    }

    private void Validate(RequestUserJson request, TechLibraryDbContext dbContext)
    {
        var validator = new RegisterUserValidator();
        var result = validator.Validate(request);

        var existUser = dbContext.Users.Any(user => user.Email.ToLower().Trim() == request.Email.ToLower().Trim());
        result.Errors.Add(new ValidationFailure("Email","Email already registered."));

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }

}
