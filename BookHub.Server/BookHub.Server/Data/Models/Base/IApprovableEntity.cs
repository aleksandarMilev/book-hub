namespace BookHub.Server.Data.Models.Base
{
    public interface IApprovableEntity
    {
        bool IsApproved { get; }
    }
}
