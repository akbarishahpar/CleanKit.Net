namespace Framework.Outbox.Enumerations;

public enum OutboxMessageState
{
    Pending,
    Succeeded,
    Failed
}