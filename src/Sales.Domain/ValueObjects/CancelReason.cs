using Sales.Domain.Base;

namespace Sales.Domain.ValueObjects;

public sealed class CancelReason : ValueObject
{
    public string Code { get; }
    public string Description { get; }
    private static readonly Dictionary<string, string> _defaultReasons = new()
    {
        {"CUST_REQUEST" , "Customer requested cancellation"},
        {"PAYMENT_ISSUE" , "Payment issue"},
        {"OUT_OF_STOCK" , "Item is out of stock"},
        {"INVALID_ADDRESS", "Invalid address"},
        {"OTHER" , "Other reason"},
    };

    private CancelReason(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Cancel reason code cannot be null or empty.", nameof(code));

        if (!_defaultReasons.TryGetValue(code, out string? value))
            throw new ArgumentException($"Invalid cancel reason code: {code}");

        Code = code;
        Description = value;
    }

    public static CancelReason CustomerRequest => new("CUST_REQUEST");
    public static CancelReason PaymentIssue => new("PAYMENT_ISSUE");
    public static CancelReason OutOfStock => new("OUT_OF_STOCK");
    public static CancelReason InvalidAddress => new("INVALID_ADDRESS");
    public static CancelReason Other => new("OTHER");

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
        yield return Description;
    }

    public override string ToString() => $"{Code}: {Description}";
}
