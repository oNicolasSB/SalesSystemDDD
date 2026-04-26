namespace Sales.Application.Commands.OrdersCommands.MarkAsInPreparation;

public sealed class MarkAsInPreparationResultDto
{
    public Guid OrderId { get; init; }

    public string OrderStatus { get; init; } = string.Empty;
}
