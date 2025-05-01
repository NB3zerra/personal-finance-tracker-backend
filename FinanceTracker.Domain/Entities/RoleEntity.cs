namespace FinanceTracker.Domain.Entities
{
    public class RoleEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Permissions { get; set; }

        public RoleEntity()
        {
            Permissions = [];
        }
    }
}