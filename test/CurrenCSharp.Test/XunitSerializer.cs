using System.Diagnostics.CodeAnalysis;
using CurrenCSharp;
using CurrenCSharp.Test;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(
    typeof(XUnitSerializer),
    typeof(Currency),
    typeof(Money),
    typeof(Wallet),
    typeof(ConversionOptions),
    typeof(ExchangeRateContext))
]

namespace CurrenCSharp.Test;

internal class XUnitSerializer : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
        throw new NotImplementedException();
    }

    public bool IsSerializable(Type type, object? value, [NotNullWhen(false)] out string? failureReason)
    {
        if (value is Currency || value is Money || value is ConversionOptions || value is Wallet)
        {
            failureReason = null;
            return true;
        }

        failureReason = $"Type {type} is not supported by this serializer.";
        return false;
    }

    public string Serialize(object value)
        => value?.ToString() ?? "null";
}
