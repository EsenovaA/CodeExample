namespace CodeExample.Infrastructure.Validation;

public interface IValidator<in T>
{
    string? Validate(T value);//actually here better use return type like Result<Ok, Error>, but I didn't find suitable library fastly :)
    //and also we can make it async, if during validation is neseccary to do http requests or requests to database
}

public interface IValidatorWithExistingData<in T> : IValidator<T>
{
}

public interface IValidatorWithoutExistingData<in T> : IValidator<T>
{
    int Order { get; }
}