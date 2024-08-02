using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SmartCommerceAPI.Models
{
    public class Buyer
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("personType")]
        public string PersonType { get; set; }

        [BsonElement("cpfCnpj")]
        public string CpfCnpj { get; set; }

        [BsonElement("stateRegistration")]
        public string StateRegistration { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("birthDate")]
        public DateTime? BirthDate { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("confirmPassword")]
        public string ConfirmPassword { get; set; }
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("blocked")]
        public bool Blocked { get; set; }
    }
}
