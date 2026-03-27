using Sales.Domain.Base;
using Sales.Domain.Common.Enums;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Events;
using Sales.Domain.Validation;

namespace Sales.Domain.Entities;

public sealed class Payment : Entity
{
    public Guid OrderId { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public decimal Value { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public string? TransactionCode { get; private set; }

    public Payment(Guid orderId, PaymentMethod paymentMethod, decimal value)
    {
        Guard.AgainstEmptyGuid(orderId, nameof(orderId));
        Guard.Against<DomainException>(value <= 0, "Value must be greater than zero.");
        Guard.Against<DomainException>(
            !Enum.IsDefined(typeof(PaymentMethod), paymentMethod),
            $"Invalid payment method: {paymentMethod}."
        );
        OrderId = orderId;
        PaymentMethod = paymentMethod;
        Value = value;
        PaymentStatus = PaymentStatus.Pending;
    }

    public void GenerateLocalTransactionCode()
    {
        if (TransactionCode is not null) return;
        string transactionCode = $"LOCAL-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        SetTransactionCode(transactionCode);
    }
    public void SetTransactionCode(string? transactionCode = null)
    {
        Guard.AgainstNullOrWhitespace(transactionCode, nameof(transactionCode));
        Guard.Against<DomainException>(TransactionCode is not null, "Transaction code has already been set.");
        Guard.Against<DomainException>(PaymentStatus != PaymentStatus.Pending, "Payment must be pending to set transaction code.");

        TransactionCode = transactionCode;
        UpdateDate();
    }

    public void ConfirmPayment()
    {
        Guard.Against<DomainException>(PaymentStatus != PaymentStatus.Pending, "Only pending payments can be confirmed.");
        Guard.AgainstNullOrWhitespace(TransactionCode, nameof(TransactionCode));

        PaymentStatus = PaymentStatus.Confirmed;
        PaidAt = DateTime.UtcNow;
        UpdateDate();

        AddDomainEvent(new PaymentConfirmedEvent(Id, OrderId, Value, PaidAt.Value, TransactionCode));
    }

    public void DenyPayment()
    {
        Guard.Against<DomainException>(PaymentStatus != PaymentStatus.Pending, "Only pending payments can be denied.");

        PaymentStatus = PaymentStatus.Rejected;
        PaidAt = DateTime.UtcNow;
        UpdateDate();

        AddDomainEvent(new PaymentRejectedEvent(Id, OrderId, Value, PaidAt.Value, TransactionCode));
    }
}
