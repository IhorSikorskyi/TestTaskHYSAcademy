using TestTask.Models;

namespace TestTask.Services.Users;

public class UserService : IUserService
{
    private readonly List<User> _users = new();
    private int _nextId = 1;

    public User CreateUser(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name), "Name must not be empty or whitespace.");
        }

        var user = new User
        {
            Id = _nextId++,
            Name = name
        };

        _users.Add(user);
        return user;
    }

    public User? GetUserById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }
}