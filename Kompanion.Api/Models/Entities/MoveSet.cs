using System;

namespace Kompanion.Api.Models.Entities
{
    public class MoveSet : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Difficulty { get; set; }
        public int Duration { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int CreatorUserId { get; set; }
        public int? UpdaterUserId { get; set; }
        public int Status { get; set; }
    }
}