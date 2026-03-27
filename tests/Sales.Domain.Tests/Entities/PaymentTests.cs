using Sales.Domain.Common.Enums;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Entities;
using Sales.Domain.Events;

namespace Sales.Domain.Tests.Entities;

public class PaymentTests
{
    [Fact(DisplayName = "Create should succeed when parameters are valid")]
    public void Create_ShouldSucceed_WhenParametersAreValid()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        decimal value = 100.0m;
        PaymentMethod paymentMethod = PaymentMethod.CreditCard;

        // Act
        var payment = new Payment(orderId, paymentMethod, value);

        // Assert
        payment.OrderId.Should().Be(orderId);
        payment.Value.Should().Be(value);
        payment.PaymentMethod.Should().Be(paymentMethod);
        payment.PaymentStatus.Should().Be(PaymentStatus.Pending);
        payment.PaidAt.Should().BeNull();
        payment.TransactionCode.Should().BeNull();
    }

    [Fact(DisplayName = "Create should throw DomainException when value is zero or negative")]
    public void Create_ShouldThrowDomainException_WhenValueIsZeroOrNegative()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        decimal value = 0.0m;
        PaymentMethod paymentMethod = PaymentMethod.CreditCard;

        // Act
        Action act = () => new Payment(orderId, paymentMethod, value);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Value must be greater than zero.");
    }

    [Fact(DisplayName = "SetTransactionCode should succeed when payment is pending and transaction code is null")]
    public void SetTransactionCode_ShouldSucceed_WhenPaymentIsPendingAndTransactionCodeIsNull()
    {
        // Arrange
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.CreditCard, 100.0m);

        // Act
        payment.SetTransactionCode("TXN12345");

        // Assert
        payment.TransactionCode.Should().Be("TXN12345");
    }

    [Fact(DisplayName = "SetTransactionCode should throw DomainException when transaction code is already set")]
    public void SetTransactionCode_ShouldThrowDomainException_WhenTransactionCodeIsAlreadySet()
    {
        // Arrange
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.CreditCard, 100.0m);
        payment.SetTransactionCode("TXN12345");

        // Act
        Action act = () => payment.SetTransactionCode("TXN67890");

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Transaction code has already been set.");
    }

    [Theory(DisplayName = "SetTransactionCode should throw DomainException when the code is null or whitespace")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void SetTransactionCode_ShouldThrowDomainException_WhenTheCodeIsNullOrWhitespace(string? invalidCode)
    {
        // Arrange
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.CreditCard, 100.0m);

        // Act
        Action act = () => payment.SetTransactionCode(invalidCode);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("'transactionCode' cannot be null or whitespace.");
    }

    [Fact(DisplayName = "GenerateLocalTransactionCode should succeed when transaction code is null")]
    public void GenerateLocalTransactionCode_ShouldSucceed_WhenTransactionCodeIsNull()
    {
        // Arrange
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.CreditCard, 100.0m);

        // Act
        payment.GenerateLocalTransactionCode();

        // Assert
        payment.TransactionCode.Should().NotBeNull();
        payment.TransactionCode.Should().StartWith("LOCAL-");
    }

    [Fact(DisplayName = "ConfirmPayment should succeed when payment is pending and transaction code is set")]
    public void ConfirmPayment_ShouldSucceed_WhenPaymentIsPendingAndTransactionCodeIsSet()
    {
        // Arrange
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.CreditCard, 100.0m);
        payment.GenerateLocalTransactionCode();

        // Act
        payment.ConfirmPayment();

        // Assert
        payment.PaymentStatus.Should().Be(PaymentStatus.Confirmed);
        payment.PaidAt.Should().NotBeNull();
        payment.UpdatedAt.Should().NotBeNull();
        PaymentConfirmedEvent? domainEvent = payment.DomainEvents.OfType<PaymentConfirmedEvent>().SingleOrDefault();
        domainEvent.Should().NotBeNull();
        domainEvent.PaymentId.Should().Be(payment.Id);
        domainEvent.OrderId.Should().Be(payment.OrderId);
        domainEvent.Value.Should().Be(payment.Value);
        domainEvent.PaymentDate.Should().Be(payment.PaidAt.Value);
        domainEvent.TransactionCode.Should().Be(payment.TransactionCode);
    }

    [Fact(DisplayName = "ConfirmPayment should throw DomainException when transaction code is not set")]
    public void ConfirmPayment_ShouldThrowDomainException_WhenTransactionCodeIsNotSet()
    {
        // Arrange
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.CreditCard, 100.0m);

        // Act
        Action act = () => payment.ConfirmPayment();

        // Assert
        act.Should().Throw<DomainException>().WithMessage("'TransactionCode' cannot be null or whitespace.");
    }

    [Fact(DisplayName = "ConfirmPayment should throw DomainException when payment is not pending")]
    public void ConfirmPayment_ShouldThrowDomainException_WhenPaymentIsNotPending()
    {
        // Arrange
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.CreditCard, 100.0m);
        payment.GenerateLocalTransactionCode();
        payment.ConfirmPayment();

        // Act
        Action act = () => payment.ConfirmPayment();

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Only pending payments can be confirmed.");
    }

    [Fact(DisplayName = "CancelPayment should succeed when payment is pending")]
    public void CancelPayment_ShouldSucceed_WhenPaymentIsPending()
    {
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.CreditCard, 100.0m);

        payment.DenyPayment();

        payment.PaymentStatus.Should().Be(PaymentStatus.Rejected);
        payment.PaidAt.Should().NotBeNull();
        payment.UpdatedAt.Should().NotBeNull();
        PaymentRejectedEvent? domainEvent = payment.DomainEvents.OfType<PaymentRejectedEvent>().SingleOrDefault();
        domainEvent.Should().NotBeNull();
        domainEvent.PaymentId.Should().Be(payment.Id);
        domainEvent.OrderId.Should().Be(payment.OrderId);
        domainEvent.Value.Should().Be(payment.Value);
        domainEvent.PaymentDate.Should().Be(payment.PaidAt.Value);
        domainEvent.TransactionCode.Should().Be(payment.TransactionCode);
    }

    [Fact(DisplayName = "CancelPayment should throw DomainException when payment is not pending")]
    public void CancelPayment_ShouldThrowDomainException_WhenPaymentIsNotPending()
    {
        // Arrange
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.CreditCard, 100.0m);
        payment.GenerateLocalTransactionCode();
        payment.ConfirmPayment();

        // Act
        Action act = () => payment.DenyPayment();

        // Assert
        act.Should().Throw<DomainException>().WithMessage("Only pending payments can be denied.");
    }
}
