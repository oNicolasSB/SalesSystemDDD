using Sales.Domain.Base;
using Sales.Domain.Common.Enums;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Events;
using Sales.Domain.Validation;
using Sales.Domain.ValueObjects;

namespace Sales.Domain.Entities;

public sealed class Order : AggregateRoot
{
    public Guid ClientId { get; private set; }
    public DeliveryAddress DeliveryAddress { get; private set; }
    public decimal TotalValue { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public string OrderNumber { get; private set; } = string.Empty;

    private readonly List<OrderItem> _orderItems = [];
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private readonly List<Payment> _payments = [];
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

    private Order(Guid clientId, DeliveryAddress deliveryAddress)
    {
        Guard.AgainstEmptyGuid(clientId, nameof(clientId));
        Guard.AgainstNull(deliveryAddress, nameof(deliveryAddress));

        ClientId = clientId;
        DeliveryAddress = deliveryAddress;
        OrderStatus = OrderStatus.Pending;
        TotalValue = 0;
        GenerateOrderNumber();
    }

    public static Order Create(Guid clientId, DeliveryAddress deliveryAddress) =>
        new(clientId, deliveryAddress);

    public void AddOrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        Guard.Against<DomainException>(
            OrderStatus != OrderStatus.Pending,
            "Cannot add items to an order that is not pending."
        );

        OrderItem? existingItem = _orderItems.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.AddUnits(quantity);
        }
        else
        {
            OrderItem orderItem = new(productId, productName, unitPrice, quantity);
            _orderItems.Add(orderItem);
        }

        RecalculateTotalValue();
        UpdateDate();
    }

    public void RemoveOrderItem(Guid productId, int quantity)
    {
        Guard.AgainstEmptyGuid(productId, nameof(productId));
        Guard.Against<DomainException>(
            OrderStatus != OrderStatus.Pending,
            "Cannot remove items from an order that is not pending."
        );

        OrderItem? existingItem = _orderItems.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem == null)
            throw new DomainException("Product not found in the order.");

        Guard.Against<DomainException>(quantity <= 0, $"{nameof(quantity)} must be greater than zero.");
        existingItem.RemoveUnits(quantity);

        RecalculateTotalValue();
        UpdateDate();
    }

    public void UpdateDeliveryAddress(DeliveryAddress newAddress)
    {
        Guard.AgainstNull(newAddress, nameof(newAddress));
        Guard.Against<DomainException>(
            OrderStatus != OrderStatus.Pending,
            "Cannot update delivery address for an order that is not pending."
        );

        DeliveryAddress = newAddress;
        UpdateDate();
    }

    public Payment StartPayment(PaymentMethod paymentMethod)
    {
        Guard.Against<DomainException>(
            !_orderItems.Any(),
            "Cannot start payment for an order with no items."
        );
        Guard.Against<DomainException>(
            OrderStatus != OrderStatus.Pending,
            "Cannot start payment for an order that is not pending."
        );

        if (_payments.Any(p => p.PaymentStatus == PaymentStatus.Pending))
        {
            throw new DomainException($"A payment with method {paymentMethod} is already in progress.");
        }

        Payment payment = new(Id, paymentMethod, TotalValue);
        _payments.Add(payment);
        UpdateDate();
        return payment;
    }

    public void HandlePaymentApproved(Guid paymentId)
    {
        Payment? payment = _payments.FirstOrDefault(p => p.Id == paymentId);
        if (payment is null) return;

        Guard.Against<DomainException>(
            OrderStatus != OrderStatus.Pending,
            "Only pending orders can be approved."
        );

        OrderStatus = OrderStatus.Confirmed;
        UpdateDate();
    }

    public void HandlePaymentRejected(Guid paymentId)
    {
        Payment? payment = _payments.FirstOrDefault(p => p.Id == paymentId);
        if (payment is null) return;

        Guard.Against<DomainException>(
            OrderStatus != OrderStatus.Pending,
            "Only pending orders can be rejected."
        );

        OrderStatus = OrderStatus.Canceled;
        UpdateDate();

        AddDomainEvent(new OrderCanceledEvent(Id, ClientId, OrderStatus, CancelReason.PaymentIssue, payment.Id));
    }

    public void MarkAsInPreparation()
    {
        Guard.Against<DomainException>(
            OrderStatus != OrderStatus.Confirmed,
            "Only confirmed orders can be marked as in preparation."
        );

        OrderStatus = OrderStatus.InPreparation;
        UpdateDate();
    }

    public void MarkAsShipped()
    {
        Guard.Against<DomainException>(
            OrderStatus != OrderStatus.InPreparation,
            "Only orders in preparation can be marked as sent."
        );

        OrderStatus = OrderStatus.Shipped;
        UpdateDate();

        AddDomainEvent(new OrderSentEvent(Id, ClientId, DeliveryAddress));
    }

    public void MarkAsDelivered()
    {
        Guard.Against<DomainException>(
            OrderStatus != OrderStatus.Shipped,
            "Only shipped orders can be marked as delivered."
        );

        OrderStatus = OrderStatus.Delivered;
        UpdateDate();

        AddDomainEvent(new OrderDeliveredEvent(Id, ClientId));
    }

    public void CancelOrder(CancelReason? reason = null)
    {
        Guard.Against<DomainException>(
            OrderStatus >= OrderStatus.InPreparation,
            "Cannot cancel an order that is under preparation or already processed."
        );

        OrderStatus = OrderStatus.Canceled;
        UpdateDate();

        AddDomainEvent(new OrderCanceledEvent(
            Id,
            ClientId,
            OrderStatus,
            reason ?? CancelReason.Other,
            _payments.LastOrDefault()?.Id
        ));
    }

    private void RecalculateTotalValue() => TotalValue = _orderItems.Sum(i => i.TotalPrice);

    private void GenerateOrderNumber() => OrderNumber = $"ORD-{Id.ToString()[..8].ToUpper()}";
}
