using System.Reflection;
using Sales.Domain.Common.Enums;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Entities;
using Sales.Domain.Events;
using Sales.Domain.ValueObjects;

namespace Sales.Domain.Tests.Entities;

public class OrderTests
{
    private static DeliveryAddress CreateValidDeliveryAddress() =>
        DeliveryAddress.Create("12345-678", "123 Main St", "complement", "Neighbourhood", "City", "State", "Country");

    private static readonly Guid ValidClientId = Guid.NewGuid();
    private static readonly Guid ValidProductId = Guid.NewGuid();

    private static void SetOrderStatus(Order order, OrderStatus status)
    {
        typeof(Order)
            .GetProperty(nameof(Order.OrderStatus),
            BindingFlags.Public | BindingFlags.Instance)!
                .SetValue(order, status);
    }

    [Fact(DisplayName = "Create Order with valid parameters should create order successfully")]
    public void Create_ValidParameters_ShouldCreateOrder()
    {
        // Arrange
        DeliveryAddress deliveryAddress = CreateValidDeliveryAddress();

        // Act
        Order order = Order.Create(ValidClientId, deliveryAddress);

        // Assert
        order.Should().NotBeNull();
        order.ClientId.Should().Be(ValidClientId);
        order.DeliveryAddress.Should().Be(deliveryAddress);
        order.OrderStatus.Should().Be(OrderStatus.Pending);
        order.TotalValue.Should().Be(0);
        order.OrderItems.Should().BeEmpty();
        order.Payments.Should().BeEmpty();
        order.Id.Should().NotBeEmpty();
    }

    [Fact(DisplayName = "Create Order with empty ClientId should throw DomainException")]
    public void Create_OrderWithEmptyClientId_ShouldThrowDomainException()
    {
        // Arrange
        DeliveryAddress deliveryAddress = CreateValidDeliveryAddress();

        // Act
        Action act = () => Order.Create(Guid.Empty, deliveryAddress);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*clientId*");
    }

    [Fact(DisplayName = "Create Order with null DeliveryAddress should throw DomainException")]
    public void Create_OrderWithNullDeliveryAddress_ShouldThrowDomainException()
    {
        // Act
        Action act = () => Order.Create(ValidClientId, null!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("*deliveryAddress*");
    }

    [Fact]
    public void AddOrderItem_WithValidParameters_ShouldAddOrderItem()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());

        // Act
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);

        // Assert
        order.OrderItems.Should().HaveCount(1);
        OrderItem item = order.OrderItems.First();
        item.ProductId.Should().Be(ValidProductId);
        item.ProductName.Should().Be("Product Name");
        item.UnitPrice.Should().Be(10.0m);
        item.Quantity.Should().Be(2);
        item.AppliedDiscount.Should().Be(0);
        item.TotalPrice.Should().Be(20.0m);
    }

    [Fact]
    public void AddOrderItem_WithSameItem_ShouldIncreaseQuantity()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());

        // Act
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 3);

        // Assert
        order.OrderItems.Should().HaveCount(1);
        OrderItem item = order.OrderItems.First();
        item.Quantity.Should().Be(5);
        item.TotalPrice.Should().Be(50.0m);
    }

    [Theory(DisplayName = "Should throw DomainException when adding item to non-pending order")]
    [InlineData(OrderStatus.Confirmed)]
    [InlineData(OrderStatus.InPreparation)]
    [InlineData(OrderStatus.Shipped)]
    [InlineData(OrderStatus.Delivered)]
    [InlineData(OrderStatus.Canceled)]
    public void AddOrderItem_WhenOrderIsNotPending_ShouldThrowDomainException(OrderStatus status)
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        SetOrderStatus(order, status);

        // Act
        Action act = () => order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Cannot add items to an order that is not pending.");
    }

    [Theory(DisplayName = "Should throw DomainException when removing item from non-pending order")]
    [InlineData(OrderStatus.Confirmed)]
    [InlineData(OrderStatus.InPreparation)]
    [InlineData(OrderStatus.Shipped)]
    [InlineData(OrderStatus.Delivered)]
    [InlineData(OrderStatus.Canceled)]
    public void RemoveOrderItem_WhenOrderIsNotPending_ShouldThrowDomainException(OrderStatus status)
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        SetOrderStatus(order, status);

        // Act
        Action act = () => order.RemoveOrderItem(ValidProductId, 1);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Cannot remove items from an order that is not pending.");
    }

    [Fact]
    public void RemoveOrderItem_WithValidParameters_ShouldRemoveOrderItem()

    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 5);

        // Act
        order.RemoveOrderItem(ValidProductId, 2);

        // Assert
        order.OrderItems.Should().HaveCount(1);
        OrderItem item = order.OrderItems.First();
        item.Quantity.Should().Be(3);
        item.TotalPrice.Should().Be(30.0m);
    }

    #region Delivery Address Tests

    [Fact(DisplayName = "Should update delivery address with valid parameters for pending order")]
    public void UpdateDeliveryAddress_WithValidParameters_ShouldUpdateAddress()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        DeliveryAddress newAddress = CreateValidDeliveryAddress();

        // Act
        order.UpdateDeliveryAddress(newAddress);

        // Assert
        order.DeliveryAddress.Should().Be(newAddress);
    }

    [Fact(DisplayName = "Should throw DomainException when updating delivery address for non-pending order")]
    public void UpdateDeliveryAddress_WhenOrderIsNotPending_ShouldThrowDomainException()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        DeliveryAddress newAddress = CreateValidDeliveryAddress();
        SetOrderStatus(order, OrderStatus.Confirmed);

        // Act
        Action act = () => order.UpdateDeliveryAddress(newAddress);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Cannot update delivery address for an order that is not pending.");
    }
    #endregion

    #region Payment Tests

    [Fact(DisplayName = "Should start payment with valid parameters for pending order")]
    public void StartPayment_WithValidParameters_ShouldStartPayment()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        PaymentMethod paymentMethod = PaymentMethod.CreditCard;

        // Act
        Payment payment = order.StartPayment(paymentMethod);

        // Assert
        payment.Should().NotBeNull();
        payment.OrderId.Should().Be(order.Id);
        payment.Value.Should().Be(order.TotalValue);
        payment.PaymentMethod.Should().Be(paymentMethod);
    }

    [Fact(DisplayName = "Should throw DomainException when starting payment for non-pending order")]
    public void StartPayment_WhenOrderIsNotPending_ShouldThrowDomainException()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        PaymentMethod paymentMethod = PaymentMethod.CreditCard;
        SetOrderStatus(order, OrderStatus.Confirmed);

        // Act
        Action act = () => order.StartPayment(paymentMethod);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Cannot start payment for an order that is not pending.");
    }

    [Fact(DisplayName = "Should throw DomainException when starting payment for order with no items")]
    public void StartPayment_WhenOrderHasNoItems_ShouldThrowDomainException()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        PaymentMethod paymentMethod = PaymentMethod.CreditCard;

        // Act
        Action act = () => order.StartPayment(paymentMethod);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Cannot start payment for an order with no items.");
    }

    [Fact(DisplayName = "Should update order status to confirmed when payment is approved")]
    public void HandlePaymentApproved_ShouldUpdateOrderStatusToConfirmed()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        Payment payment = order.StartPayment(PaymentMethod.CreditCard);

        // Act
        order.HandlePaymentApproved(payment.Id);

        // Assert
        order.OrderStatus.Should().Be(OrderStatus.Confirmed);
    }

    [Fact(DisplayName = "Should throw DomainException when handling payment approved for non-pending order")]
    public void HandlePaymentApproved_WhenOrderIsNotPending_ShouldThrowDomainException()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        Payment payment = order.StartPayment(PaymentMethod.CreditCard);
        SetOrderStatus(order, OrderStatus.Confirmed);

        // Act
        Action act = () => order.HandlePaymentApproved(payment.Id);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Only pending orders can be approved.");
    }

    [Fact(DisplayName = "Should update order status to canceled when payment is rejected")]
    public void HandlePaymentRejected_ShouldUpdateOrderStatusToCanceled()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        Payment payment = order.StartPayment(PaymentMethod.CreditCard);

        // Act
        order.HandlePaymentRejected(payment.Id);

        // Assert
        order.OrderStatus.Should().Be(OrderStatus.Canceled);
    }

    [Fact(DisplayName = "HandlePaymentRejected should update order status to canceled when payment is rejected")]
    public void HandlePaymentRejected_ShouldUpdateOrderStatusToCanceled_WhenPaymentIsRejected()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        Payment payment = order.StartPayment(PaymentMethod.CreditCard);

        // Act
        order.HandlePaymentRejected(payment.Id); // Pass an invalid payment ID

        // Assert
        order.OrderStatus.Should().Be(OrderStatus.Canceled);
        order.DomainEvents.Should().ContainSingle(e => e is OrderCanceledEvent)
            .Which.Should().BeOfType<OrderCanceledEvent>()
            .Which.OrderId.Should().Be(order.Id);
    }

    [Fact(DisplayName = "Should throw DomainException when handling payment rejected for non-pending payment")]
    public void HandlePaymentRejected_WhenPaymentIsNotPending_ShouldThrowDomainException()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        Payment payment = order.StartPayment(PaymentMethod.CreditCard);
        SetOrderStatus(order, OrderStatus.InPreparation); // Set order to a non-pending status

        // Act
        Action act = () => order.HandlePaymentRejected(payment.Id);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Only pending orders can be rejected.");
    }
    #endregion

    #region State transition tests

    [Fact(DisplayName = "Should update order status to in preparation")]
    public void MarkAsInPreparation_ShouldUpdateOrderStatusToInPreparation()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        Payment payment = order.StartPayment(PaymentMethod.CreditCard);
        order.HandlePaymentApproved(payment.Id);

        // Act
        order.MarkAsInPreparation();

        // Assert
        order.OrderStatus.Should().Be(OrderStatus.InPreparation);
    }

    [Fact(DisplayName = "Should throw DomainException when marking non-confirmed order as in preparation")]
    public void MarkAsInPreparation_WhenOrderIsNotConfirmed_ShouldThrowDomainException()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        Payment payment = order.StartPayment(PaymentMethod.CreditCard);
        order.HandlePaymentApproved(payment.Id);
        SetOrderStatus(order, OrderStatus.Pending);

        // Act
        Action act = () => order.MarkAsInPreparation();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Only confirmed orders can be marked as in preparation.");
    }

    [Fact(DisplayName = "Should update order status to shipped")]
    public void MarkAsShipped_ShouldUpdateOrderStatusToShipped()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        Payment payment = order.StartPayment(PaymentMethod.CreditCard);
        order.HandlePaymentApproved(payment.Id);
        order.MarkAsInPreparation();

        // Act
        order.MarkAsShipped();

        // Assert
        order.OrderStatus.Should().Be(OrderStatus.Shipped);
    }

    [Fact(DisplayName = "Should throw DomainException when marking non-in preparation order as shipped")]
    public void MarkAsShipped_WhenOrderIsNotInPreparation_ShouldThrowDomainException()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        Payment payment = order.StartPayment(PaymentMethod.CreditCard);
        order.HandlePaymentApproved(payment.Id);
        SetOrderStatus(order, OrderStatus.Confirmed);

        // Act
        Action act = () => order.MarkAsShipped();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Only orders in preparation can be marked as sent.");
    }

    [Fact(DisplayName = "Should update order status to delivered")]
    public void MarkAsDelivered_ShouldUpdateOrderStatusToDelivered()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        Payment payment = order.StartPayment(PaymentMethod.CreditCard);
        order.HandlePaymentApproved(payment.Id);
        order.MarkAsInPreparation();
        order.MarkAsShipped();

        // Act
        order.MarkAsDelivered();

        // Assert
        order.OrderStatus.Should().Be(OrderStatus.Delivered);
    }

    [Fact(DisplayName = "Should throw DomainException when marking non-shipped order as delivered")]
    public void MarkAsDelivered_WhenOrderIsNotShipped_ShouldThrowDomainException()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        Payment payment = order.StartPayment(PaymentMethod.CreditCard);
        order.HandlePaymentApproved(payment.Id);
        order.MarkAsInPreparation();

        // Act
        Action act = () => order.MarkAsDelivered();

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Only shipped orders can be marked as delivered.");
    }

    [Fact(DisplayName = "Should update order status to canceled when canceling order")]
    public void CancelOrder_ShouldUpdateOrderStatusToCanceled()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        Payment payment = order.StartPayment(PaymentMethod.CreditCard);

        // Act
        order.CancelOrder(CancelReason.PaymentIssue);

        // Assert
        order.OrderStatus.Should().Be(OrderStatus.Canceled);
        order.DomainEvents.Should().ContainSingle(e => e is OrderCanceledEvent)
            .Which.Should().BeOfType<OrderCanceledEvent>()
            .Which.OrderId.Should().Be(order.Id);
    }

    [Fact]
    public void CancelOrder_ShouldUpdateOrderStatusToCanceled_WithConfirmedPayment()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        Payment payment = order.StartPayment(PaymentMethod.CreditCard);
        order.HandlePaymentApproved(payment.Id);

        // Act
        order.CancelOrder(CancelReason.PaymentIssue);

        // Assert
        order.OrderStatus.Should().Be(OrderStatus.Canceled);
        order.DomainEvents.Should().ContainSingle(e => e is OrderCanceledEvent)
            .Which.Should().BeOfType<OrderCanceledEvent>()
            .Which.OrderId.Should().Be(order.Id);
    }

    [Fact(DisplayName = "Should throw DomainException when canceling order after preparation")]
    public void CancelOrder_WhenOrderIsAfterPreparation_ShouldThrowDomainException()
    {
        // Arrange
        Order order = Order.Create(ValidClientId, CreateValidDeliveryAddress());
        order.AddOrderItem(ValidProductId, "Product Name", 10.0m, 2);
        Payment payment = order.StartPayment(PaymentMethod.CreditCard);
        order.HandlePaymentApproved(payment.Id);
        order.MarkAsInPreparation();

        // Act
        Action act = () => order.CancelOrder(CancelReason.PaymentIssue);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Cannot cancel an order that is under preparation or already processed.");
    }

    #endregion

}
