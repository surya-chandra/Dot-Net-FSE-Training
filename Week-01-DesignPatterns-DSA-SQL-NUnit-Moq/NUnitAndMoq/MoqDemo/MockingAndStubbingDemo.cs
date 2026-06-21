public interface IUserRepository
{
    string GetUserName(int id);
}

public class UserService
{
    private readonly IUserRepository repository;

    public UserService(IUserRepository repository)
    {
        this.repository = repository;
    }

    public string GetUser(int id)
    {
        return repository.GetUserName(id);
    }
}