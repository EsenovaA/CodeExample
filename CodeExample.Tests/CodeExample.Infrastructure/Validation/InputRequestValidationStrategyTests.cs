using CodeExample.Infrastructure.Requests;
using CodeExample.Infrastructure.Validation;
using NSubstitute;
using NSubstitute.Extensions;
using Xunit;

namespace CodeExample.Tests.CodeExample.Infrastructure.Validation;

public class InputRequestValidationStrategyTests
{
    private IValidatorWithoutExistingData<InputRequest> _firstValidatorWithoutDataMock;
    private IValidatorWithoutExistingData<InputRequest> _secondValidatorWithoutDataMock;
    private IValidatorWithExistingData<InputRequest> _validatorWithDataMock;
    private IDataExistenceChecker _dataExistenceCheckerMock = Substitute.For<IDataExistenceChecker>();
    
    public InputRequestValidationStrategyTests()
    {
        _firstValidatorWithoutDataMock =
            Substitute.For<IValidatorWithoutExistingData<InputRequest>>();
        _secondValidatorWithoutDataMock =
            Substitute.For<IValidatorWithoutExistingData<InputRequest>>();
        _validatorWithDataMock =
            Substitute.For<IValidatorWithExistingData<InputRequest>>();
    }

    public InputRequestValidationStrategy Sut()
    {
        var validatorsWithoutDataMock = new[] { _secondValidatorWithoutDataMock, _firstValidatorWithoutDataMock };
        var validatorsWithDataMock = new[] { _validatorWithDataMock };

        return new InputRequestValidationStrategy(
            validatorsWithDataMock,
            validatorsWithoutDataMock,
            _dataExistenceCheckerMock);
    }
    
    [Fact]
    public void CheckValidatorsCallOrder_WhenDataExists()
    {
        //Arrange
        _firstValidatorWithoutDataMock.Configure().Order.Returns(1);
        _secondValidatorWithoutDataMock.Configure().Order.Returns(2);

        _firstValidatorWithoutDataMock.Validate(Arg.Any<InputRequest>())
            .Returns((string?) null);
        _secondValidatorWithoutDataMock.Validate(Arg.Any<InputRequest>())
            .Returns((string?) null);
        _validatorWithDataMock.Validate(Arg.Any<InputRequest>())
            .Returns((string?) null);

        _dataExistenceCheckerMock.Check().Returns(true);
        //Act
        Sut().Validate(new InputRequest());
        
        //Assert
        Received.InOrder(() => {
            _firstValidatorWithoutDataMock.Validate(Arg.Any<InputRequest>());
            _secondValidatorWithoutDataMock.Validate(Arg.Any<InputRequest>());
            _validatorWithDataMock.Validate(Arg.Any<InputRequest>());
        });
    }
    
    [Fact]
    public void CheckValidatorsCallOrder_WhenDataNotExists()
    {
        //Arrange
        _firstValidatorWithoutDataMock.Configure().Order.Returns(1);
        _secondValidatorWithoutDataMock.Configure().Order.Returns(2);

        _firstValidatorWithoutDataMock.Validate(Arg.Any<InputRequest>())
            .Returns((string?) null);
        _secondValidatorWithoutDataMock.Validate(Arg.Any<InputRequest>())
            .Returns((string?) null);
        _validatorWithDataMock.Validate(Arg.Any<InputRequest>())
            .Returns((string?) null);

        
        _dataExistenceCheckerMock.Check().Returns(true);

        //Act
        Sut().Validate(new InputRequest());
        
        //Assert
        Received.InOrder(() => {
            _firstValidatorWithoutDataMock.Validate(Arg.Any<InputRequest>());
            _secondValidatorWithoutDataMock.Validate(Arg.Any<InputRequest>());
        });
        _validatorWithDataMock.DidNotReceive();
    }

}