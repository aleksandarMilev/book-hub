namespace BookHub.Data.Models.Base
{
    public interface IApprovableEntity
    {
        bool IsApproved { get; }
    }
}
