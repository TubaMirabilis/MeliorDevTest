using FluentAssertions;
using Melior.InterviewQuestion.Data;
using Melior.InterviewQuestion.Services;
using Melior.InterviewQuestion.Types;
using NSubstitute;
using Xunit;

namespace Melior.InterviewQuestion.UnitTests;

public class PaymentServiceTests
{
    private readonly IAccountDataStore _store;
    private readonly PaymentService _ps;
    public PaymentServiceTests()
    {
        _store = Substitute.For<IAccountDataStore>();
        _ps = new PaymentService(_store);
    }

    [Fact]
    public void MakePayment_WhenPaymentSchemeIsBacsAndAccountHasBacs_ShouldReturnTrue()
    {
        // Arrange
        var account = new Account
        {
            AccountNumber = "123",
            Balance = 100,
            Status = AccountStatus.Live,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
        };
        _store.GetAccount("123").Returns(account);
        var request = new MakePaymentRequest("456", "123", 50, DateTime.Now, PaymentScheme.Bacs);

        // Act
        var result = _ps.MakePayment(request);

        // Assert
        result.Success.Should().BeTrue();
    }

    [Fact]
    public void MakePayment_WhenPaymentSchemeIsBacsAndAccountDoesNotHaveBacs_ShouldReturnFalse()
    {
        // Arrange
        var account = new Account
        {
            AccountNumber = "123",
            Balance = 100,
            Status = AccountStatus.Live,
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
        };
        _store.GetAccount("123").Returns(account);
        var request = new MakePaymentRequest("456", "123", 50, DateTime.Now, PaymentScheme.Bacs);

        // Act
        var result = _ps.MakePayment(request);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public void MakePayment_WhenPaymentSchemeIsFasterPaymentsAndAccountHasFasterPayments_ShouldReturnTrue()
    {
        // Arrange
        var account = new Account
        {
            AccountNumber = "123",
            Balance = 100,
            Status = AccountStatus.Live,
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments
        };
        _store.GetAccount("123").Returns(account);
        var request = new MakePaymentRequest("456", "123", 50, DateTime.Now, PaymentScheme.FasterPayments);

        // Act
        var result = _ps.MakePayment(request);

        // Assert
        result.Success.Should().BeTrue();
    }

    [Fact]
    public void MakePayment_WhenPaymentSchemeIsFasterPaymentsAndAccountDoesNotHaveFasterPayments_ShouldReturnFalse()
    {
        // Arrange
        var account = new Account
        {
            AccountNumber = "123",
            Balance = 100,
            Status = AccountStatus.Live,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
        };
        _store.GetAccount("123").Returns(account);
        var request = new MakePaymentRequest("456", "123", 50, DateTime.Now, PaymentScheme.FasterPayments);

        // Act
        var result = _ps.MakePayment(request);

        // Assert
        result.Success.Should().BeFalse();
    }

    [Fact]
    public void MakePayment_WhenPaymentSchemeIsChapsAndAccountHasChaps_ShouldReturnTrue()
    {
        // Arrange
        var account = new Account
        {
            AccountNumber = "123",
            Balance = 100,
            Status = AccountStatus.Live,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps
        };
        _store.GetAccount("123").Returns(account);
        var request = new MakePaymentRequest("456", "123", 50, DateTime.Now, PaymentScheme.Chaps);

        // Act
        var result = _ps.MakePayment(request);

        // Assert
        result.Success.Should().BeTrue();
    }

    [Fact]
    public void MakePayment_WhenPaymentSchemeIsChapsAndAccountDoesNotHaveChaps_ShouldReturnFalse()
    {
        // Arrange
        var account = new Account
        {
            AccountNumber = "123",
            Balance = 100,
            Status = AccountStatus.Live,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
        };
        _store.GetAccount("123").Returns(account);
        var request = new MakePaymentRequest("456", "123", 50, DateTime.Now, PaymentScheme.Chaps);

        // Act
        var result = _ps.MakePayment(request);

        // Assert
        result.Success.Should().BeFalse();
    }
}
