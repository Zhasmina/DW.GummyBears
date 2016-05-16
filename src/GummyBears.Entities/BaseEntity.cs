using Dapperer;

namespace GummyBears.Entities
{
    public abstract class BaseEntity : IIdentifier<int>
    {
        [Column("Id", IsPrimary = true, AutoIncrement = true)]
        public int Id { get; set; }

        public void SetIdentity(int identity)
        {
            Id = identity;
        }

        public int GetIdentity()
        {
            return Id;
        }
    }
}
