using CodeExample.Infrastructure.Requests;

namespace CodeExample.Infrastructure.Validation;

public class InputRequestValidationStrategy : IValidationStrategy<InputRequest>
{
    private readonly IEnumerable<IValidatorWithExistingData<InputRequest>> _validatorsWithExistingData;
    private readonly IEnumerable<IValidatorWithoutExistingData<InputRequest>> _validatorsWithoutExistingData;
    private readonly IDataExistenceChecker _dataExistenceChecker; 

    public InputRequestValidationStrategy(
        IEnumerable<IValidatorWithExistingData<InputRequest>> validatorsWithExistingData,
        IEnumerable<IValidatorWithoutExistingData<InputRequest>> validatorsWithoutExistingData,
        IDataExistenceChecker dataExistenceChecker)
    {
        _validatorsWithExistingData = validatorsWithExistingData;
        _validatorsWithoutExistingData = validatorsWithoutExistingData;
        _dataExistenceChecker = dataExistenceChecker;
    }

    public string? Validate(InputRequest request)
    {
        var validators = GetValidatorsToApply();

        foreach (var validator in validators)
        {
            var result = validator.Validate(request);
            if (!string.IsNullOrWhiteSpace(result))
            {
                return result;
            }
        }

        return null;
    }

    private IEnumerable<IValidator<InputRequest>> GetValidatorsToApply()
    {
        IEnumerable<IValidator<InputRequest>> orderedValidators =
            _validatorsWithoutExistingData.OrderBy(x => x.Order);

        var validatorsToApply = orderedValidators.ToList();
        if (_dataExistenceChecker.Check())
        {
            validatorsToApply.AddRange(_validatorsWithExistingData);
        }

        return validatorsToApply;
    }
}