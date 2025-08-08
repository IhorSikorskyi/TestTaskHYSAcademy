using TestTask.Models;

namespace TestTask.Services.Users;

public interface IUserService
{
    User CreateUser(string name);
    User? GetUserById(int id);
}