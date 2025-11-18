namespace BookHub.Data.Models.Base
{
    public abstract class Entity<TKey> : IEntity
    {
        public TKey Id { get; set; } = default!;

        public DateTime CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string? ModifiedBy { get; set; }
    }
} 
