namespace CurrenCSharp;

public sealed partial class Wallet
{
    /// <summary>
    /// Returns the same wallet.
    /// </summary>
    /// <param name="wallet">The wallet value.</param>
    /// <returns>The unchanged wallet.</returns>
    public static Wallet operator +(Wallet wallet)
    {
        ArgumentNullException.ThrowIfNull(wallet);
        return wallet;
    }

    /// <summary>
    /// Negates all monetary values in a wallet.
    /// </summary>
    /// <param name="wallet">The wallet to negate.</param>
    /// <returns>A wallet with negated monetary values.</returns>
    public static Wallet operator -(Wallet wallet)
    {
        ArgumentNullException.ThrowIfNull(wallet);
        return new(wallet._moneyCollection.Negate());
    }

    /// <summary>
    /// Adds two wallets.
    /// </summary>
    /// <param name="left">The left wallet.</param>
    /// <param name="right">The right wallet.</param>
    /// <returns>A wallet containing the sum of both wallets.</returns>
    public static Wallet operator +(Wallet left, Wallet right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return new(left._moneyCollection.Union(right._moneyCollection));
    }

    /// <summary>
    /// Adds a monetary value to a wallet.
    /// </summary>
    /// <param name="left">The wallet.</param>
    /// <param name="right">The monetary value to add.</param>
    /// <returns>A wallet containing the updated totals.</returns>
    public static Wallet operator +(Wallet left, Money right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return new(left._moneyCollection.Add(right));
    }

    /// <summary>
    /// Subtracts one wallet from another.
    /// </summary>
    /// <param name="left">The left wallet.</param>
    /// <param name="right">The right wallet.</param>
    /// <returns>A wallet containing the subtraction result.</returns>
    public static Wallet operator -(Wallet left, Wallet right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return new(left._moneyCollection.Union((-right)._moneyCollection));
    }

    /// <summary>
    /// Subtracts a monetary value from a wallet.
    /// </summary>
    /// <param name="left">The wallet.</param>
    /// <param name="right">The monetary value to subtract.</param>
    /// <returns>A wallet containing the updated totals.</returns>
    public static Wallet operator -(Wallet left, Money right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return new(left._moneyCollection.Add(-right));
    }

    /// <summary>
    /// Multiplies all values in a wallet by a scalar.
    /// </summary>
    /// <param name="left">The wallet.</param>
    /// <param name="right">The multiplication factor.</param>
    /// <returns>A scaled wallet.</returns>
    public static Wallet operator *(Wallet left, decimal right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return new(left._moneyCollection.MultiplyBy(right));
    }

    /// <summary>
    /// Multiplies all values in a wallet by a scalar.
    /// </summary>
    /// <param name="left">The multiplication factor.</param>
    /// <param name="right">The wallet.</param>
    /// <returns>A scaled wallet.</returns>
    public static Wallet operator *(decimal left, Wallet right)
    {
        ArgumentNullException.ThrowIfNull(right);
        return right * left;
    }

    /// <summary>
    /// Divides all values in a wallet by a scalar.
    /// </summary>
    /// <param name="left">The wallet.</param>
    /// <param name="right">The divisor.</param>
    /// <returns>A scaled wallet.</returns>
    public static Wallet operator /(Wallet left, decimal right)
    {
        ArgumentNullException.ThrowIfNull(left);
        return right != 0m
            ? new(left._moneyCollection.DivideBy(right))
            : throw new DivideByZeroException("Cannot divide by zero.");
    }
}
