using System;

namespace Kompanion.Api.Models.Entities
{
    public class MoveSetMove : BaseModel
    {
        public int Id { get; set; }
        public int MoveSetId { get; set; }
        public int MoveId { get; set; }
        public int Order { get; set;}
        public int Status { get; set; }
    }
}