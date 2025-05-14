namespace CleanKit.Net.Domain.Primitives;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public static bool operator ==(ValueObject left, ValueObject right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject left, ValueObject right) => !(left == right);

    public bool Equals(ValueObject? other) =>
        other is not null && GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());

    public override bool Equals(object? obj) => Equals(obj as ValueObject);

    public override int GetHashCode()
    {
        var hashCode = default(HashCode);
        foreach (var obj in GetEqualityComponents())
            hashCode.Add(obj);
        return hashCode.ToHashCode();
    }

    protected abstract IEnumerable<object?> GetEqualityComponents();
}

public abstract class ValueObject<TValue> : ValueObject
{
    public TValue Value { get; private set; }

    public ValueObject()
    {
        Value = default!;
    }

    public ValueObject(TValue value)
    {
        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
