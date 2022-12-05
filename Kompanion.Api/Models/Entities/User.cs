using System;

namespace Kompanion.Api.Models.Entities
{
    public class User : BaseModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int Status { get; set; }
        public int UserType { get; set;}
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        
    }
}