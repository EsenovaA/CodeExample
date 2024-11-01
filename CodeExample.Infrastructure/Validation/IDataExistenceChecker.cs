namespace CodeExample.Infrastructure.Validation;

public interface IDataExistenceChecker
{
    bool Check();
}

public class InputRequestDataExistenceChecker : IDataExistenceChecker
{
    
    //let's imagine that here we do neccessary actions to check if data exists where we assume it should exists :)
    public bool Check()
    {
        return true;
    }
}