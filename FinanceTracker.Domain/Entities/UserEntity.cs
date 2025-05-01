using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FinanceTracker.Domain.Entities
{
    public class UserEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public RoleEntity Role  { get; set; } = new RoleEntity();
    }
}