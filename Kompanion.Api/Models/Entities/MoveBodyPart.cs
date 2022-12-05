using System;

namespace Kompanion.Api.Models.Entities
{
    public class MoveBodyPart : BaseModel
    {
        public int Id { get; set; }
        public int MoveId { get; set; }
        public int BodyPartId { get; set; }
    }
}