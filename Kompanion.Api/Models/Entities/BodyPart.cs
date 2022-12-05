using System;

namespace Kompanion.Api.Models.Entities
{
    public class BodyPart : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}