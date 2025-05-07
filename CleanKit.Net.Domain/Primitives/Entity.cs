using CleanKit.Net.Domain.Abstractions;

namespace CleanKit.Net.Domain.Primitives;

public abstract class Entity : IEquatable<Entity>, IEntity
{
    //'init' qualifier is required to entity CleanKit.Net detect this field as primary key
    public Guid Id { get; init; }

    protected Entity(Guid id) : this()
        => Id = id;

    protected Entity()
    {
    }

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right) => !(left == right);

    public bool Equals(Entity? other) =>
        other is not null && (ReferenceEquals(this, other) || Id == other.Id);

    public override bool Equals(object? obj) => Equals(obj as Entity);

    public override int GetHashCode() => Id.GetHashCode() * 41;
}