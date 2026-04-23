namespace Sales.Application.Commands.OrdersCommands.MarkAsInPreparation;

public sealed class MarkAsInPreparationCommand
{
    public Guid OrderId { get; }
    public MarkAsInPreparationCommand(Guid orderId)
    {
        OrderId = orderId;
    }
    
}
