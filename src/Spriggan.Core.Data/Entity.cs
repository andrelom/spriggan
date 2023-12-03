namespace Spriggan.Core.Data;

public abstract class Entity
{
    public Guid Id { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;
}
