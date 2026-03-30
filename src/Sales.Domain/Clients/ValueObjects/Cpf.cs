using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Clients.ValueObjects;

public sealed class Cpf : ValueObject
{
    public string Number { get; }

    public Cpf(string number)
    {
        Guard.AgainstNullOrWhitespace(number, nameof(number));

        var digits = new string([.. number.Where(char.IsDigit)]);

        Guard.Against<DomainException>(digits.Length != 11, "CPF must contain 11 digits.");
        Guard.Against<DomainException>(!ValidCpf(digits), "Invalid Cpf.");
        Number = digits;
    }

    private static bool ValidCpf(string digits)
    {
        if (new string(digits[0], digits.Length) == digits)
            return false;

        int Sum(int length, int weight)
        {
            int sum = 0;
            for (int i = 0; i < length; i++)
            {
                sum += (digits[i] - '0') * (weight - i);
            }
            return sum;
        }

        int dv1 = Sum(9, 10) % 11;
        dv1 = dv1 < 2 ? 0 : 11 - dv1;
        int dv2 = Sum(10, 11) % 11;
        dv2 = dv2 < 2 ? 0 : 11 - dv2;

        return digits[9] - '0' == dv1 && digits[10] - '0' == dv2;
    }

    public override string ToString() => Convert.ToUInt64(Number).ToString(@"000\.000\.000\-00");

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }
}
