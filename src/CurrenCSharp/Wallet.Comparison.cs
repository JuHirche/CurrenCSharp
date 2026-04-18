namespace CurrenCSharp;

public sealed partial class Wallet : IComparable<ContextedMoney>, IComparable<ContextedWallet>, IEquatable<Wallet>
{
    /// <summary>
    /// Determines whether a wallet total is equal to a context-aware wallet total.
    /// </summary>
    /// <param name="left">The left wallet.</param>
    /// <param name="right">The right context-aware wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Wallet left, ContextedWallet right) =>
        left is null ? right is null : right is not null && left.CompareTo(right) == 0;

    /// <summary>
    /// Determines whether a context-aware wallet total is equal to a wallet total.
    /// </summary>
    /// <param name="left">The left context-aware wallet.</param>
    /// <param name="right">The right wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(ContextedWallet left, Wallet right) => right == left;

    /// <summary>
    /// Determines whether a wallet total is equal to a context-aware monetary value.
    /// </summary>
    /// <param name="left">The left wallet.</param>
    /// <param name="right">The right context-aware monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Wallet left, ContextedMoney right) =>
        left is null ? right is null : right is not null && left.CompareTo(right) == 0;

    /// <summary>
    /// Determines whether a context-aware monetary value is equal to a wallet total.
    /// </summary>
    /// <param name="left">The left context-aware monetary value.</param>
    /// <param name="right">The right wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(ContextedMoney left, Wallet right) => right == left;

    /// <summary>
    /// Determines whether a wallet total is not equal to a context-aware wallet total.
    /// </summary>
    /// <param name="left">The left wallet.</param>
    /// <param name="right">The right context-aware wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Wallet left, ContextedWallet right) => !(left == right);

    /// <summary>
    /// Determines whether a context-aware wallet total is not equal to a wallet total.
    /// </summary>
    /// <param name="left">The left context-aware wallet.</param>
    /// <param name="right">The right wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(ContextedWallet left, Wallet right) => !(right == left);

    /// <summary>
    /// Determines whether a wallet total is not equal to a context-aware monetary value.
    /// </summary>
    /// <param name="left">The left wallet.</param>
    /// <param name="right">The right context-aware monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Wallet left, ContextedMoney right) => !(left == right);

    /// <summary>
    /// Determines whether a context-aware monetary value is not equal to a wallet total.
    /// </summary>
    /// <param name="left">The left context-aware monetary value.</param>
    /// <param name="right">The right wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(ContextedMoney left, Wallet right) => !(right == left);

    /// <summary>
    /// Determines whether a wallet total is less than a context-aware monetary value.
    /// </summary>
    /// <param name="left">The left wallet.</param>
    /// <param name="right">The right context-aware monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Wallet left, ContextedMoney right) =>
        left is not null && right is not null && left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether a context-aware monetary value is less than a wallet total.
    /// </summary>
    /// <param name="left">The left context-aware monetary value.</param>
    /// <param name="right">The right wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(ContextedMoney left, Wallet right) =>
        left is not null && right is not null && right.CompareTo(left) > 0;

    /// <summary>
    /// Determines whether one wallet total is less than another context-aware wallet total.
    /// </summary>
    /// <param name="left">The left wallet.</param>
    /// <param name="right">The right context-aware wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(Wallet left, ContextedWallet right) =>
        left is not null && right is not null && left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether a context-aware wallet total is less than a wallet total.
    /// </summary>
    /// <param name="left">The left context-aware wallet.</param>
    /// <param name="right">The right wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(ContextedWallet left, Wallet right) =>
        left is not null && right is not null && right.CompareTo(left) > 0;

    /// <summary>
    /// Determines whether a wallet total is less than or equal to a context-aware monetary value.
    /// </summary>
    /// <param name="left">The left wallet.</param>
    /// <param name="right">The right context-aware monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Wallet left, ContextedMoney right) =>
        left is not null && right is not null && left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether a context-aware monetary value is less than or equal to a wallet total.
    /// </summary>
    /// <param name="left">The left context-aware monetary value.</param>
    /// <param name="right">The right wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(ContextedMoney left, Wallet right) =>
        left is not null && right is not null && right.CompareTo(left) >= 0;

    /// <summary>
    /// Determines whether one wallet total is less than or equal to another context-aware wallet total.
    /// </summary>
    /// <param name="left">The left wallet.</param>
    /// <param name="right">The right context-aware wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(Wallet left, ContextedWallet right) =>
        left is not null && right is not null && left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether a context-aware wallet total is less than or equal to a wallet total.
    /// </summary>
    /// <param name="left">The left context-aware wallet.</param>
    /// <param name="right">The right wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(ContextedWallet left, Wallet right) =>
        left is not null && right is not null && right.CompareTo(left) >= 0;

    /// <summary>
    /// Determines whether a wallet total is greater than a context-aware monetary value.
    /// </summary>
    /// <param name="left">The left wallet.</param>
    /// <param name="right">The right context-aware monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Wallet left, ContextedMoney right) =>
        left is not null && right is not null && left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether a context-aware monetary value is greater than a wallet total.
    /// </summary>
    /// <param name="left">The left context-aware monetary value.</param>
    /// <param name="right">The right wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(ContextedMoney left, Wallet right) =>
        left is not null && right is not null && right.CompareTo(left) < 0;

    /// <summary>
    /// Determines whether one wallet total is greater than another context-aware wallet total.
    /// </summary>
    /// <param name="left">The left wallet.</param>
    /// <param name="right">The right context-aware wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(Wallet left, ContextedWallet right) =>
        left is not null && right is not null && left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether a context-aware wallet total is greater than a wallet total.
    /// </summary>
    /// <param name="left">The left context-aware wallet.</param>
    /// <param name="right">The right wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(ContextedWallet left, Wallet right) =>
        left is not null && right is not null && right.CompareTo(left) < 0;

    /// <summary>
    /// Determines whether a wallet total is greater than or equal to a context-aware monetary value.
    /// </summary>
    /// <param name="left">The left wallet.</param>
    /// <param name="right">The right context-aware monetary value.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Wallet left, ContextedMoney right) =>
        left is not null && right is not null && left.CompareTo(right) >= 0;

    /// <summary>
    /// Determines whether a context-aware monetary value is greater than or equal to a wallet total.
    /// </summary>
    /// <param name="left">The left context-aware monetary value.</param>
    /// <param name="right">The right wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(ContextedMoney left, Wallet right) =>
        left is not null && right is not null && right.CompareTo(left) <= 0;

    /// <summary>
    /// Determines whether one wallet total is greater than or equal to another context-aware wallet total.
    /// </summary>
    /// <param name="left">The left wallet.</param>
    /// <param name="right">The right context-aware wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(Wallet left, ContextedWallet right) =>
        left is not null && right is not null && left.CompareTo(right) >= 0;

    /// <summary>
    /// Determines whether a context-aware wallet total is greater than or equal to a wallet total.
    /// </summary>
    /// <param name="left">The left context-aware wallet.</param>
    /// <param name="right">The right wallet.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(ContextedWallet left, Wallet right) =>
        left is not null && right is not null && right.CompareTo(left) <= 0;

    /// <summary>
    /// Compares this wallet total with a context-aware monetary value.
    /// </summary>
    /// <param name="other">The context-aware monetary value to compare with this wallet.</param>
    /// <returns>
    /// A value less than zero if this wallet is less than <paramref name="other"/>,
    /// zero if both are equal, or greater than zero if this wallet is greater.
    /// </returns>
    public int CompareTo(ContextedMoney? other)
    {
        if (other is null)
            return 1;

        Money otherMoney = new(other.Amount, other.Currency);
        Money walletTotal = In(other.Context).Total(otherMoney.Currency);
        return walletTotal.CompareTo(otherMoney);
    }

    /// <summary>
    /// Compares this wallet total with another context-aware wallet total.
    /// </summary>
    /// <param name="other">The context-aware wallet to compare with this wallet.</param>
    /// <returns>
    /// A value less than zero if this wallet is less than <paramref name="other"/>,
    /// zero if both are equal, or greater than zero if this wallet is greater.
    /// </returns>
    public int CompareTo(ContextedWallet? other)
    {
        if (other is null)
            return 1;

        var currency = ResolveCurrency();
        var walletTotal = In(other.Context).Total(currency);
        return walletTotal.CompareTo(other.Total(currency));
    }

    /// <summary>
    /// Determines whether this wallet has the same monetary contents as another wallet.
    /// </summary>
    /// <param name="other">The wallet to compare with this instance.</param>
    /// <returns><see langword="true"/> if both wallets contain the same monetary values per currency; otherwise, <see langword="false"/>.</returns>
    public bool Equals(Wallet? other) =>
        other is not null && _moneyCollection.Equals(other._moneyCollection);

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as Wallet);

    /// <inheritdoc />
    public override int GetHashCode() => _moneyCollection.GetHashCode();
}
