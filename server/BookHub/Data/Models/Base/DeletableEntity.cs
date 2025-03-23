namespace BookHub.Data.Models.Base
{
    public abstract class DeletableEntity<TKey> 
        : Entity<TKey>, IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string? DeletedBy { get; set; }
    }
}
