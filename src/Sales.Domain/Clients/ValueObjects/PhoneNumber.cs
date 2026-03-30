using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Clients.ValueObjects;

public class PhoneNumber : ValueObject
{
    public string Number { get; }
    public PhoneNumber(string number)
    {
        Guard.AgainstNullOrWhitespace(number, nameof(number));
        var digits = new string([.. number.Where(char.IsDigit)]);

        Guard.Against<DomainException>(
            digits.Length is < 10 or > 11,
            "Phone number must be have 10 or 11 digits."
        );

        Number = digits;
    }

    public override string ToString()
    {
        if( Number.Length == 11)
            return Convert.ToUInt64(Number).ToString(@"\(00\) 00000\-0000");
        return Convert.ToUInt64(Number).ToString(@"\(00\) 0000\-0000");
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }
}
