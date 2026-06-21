public interface IEmployeeRepository
{
    string GetEmployeeName();
}

public class EmployeeService
{
    private readonly IEmployeeRepository repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        this.repository = repository;
    }

    public string GetName()
    {
        return repository.GetEmployeeName();
    }
}