using System;

namespace Kompanion.Api.Infrastructure.DTO
{
    public class MoveSetDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Difficulty { get; set; }
        public int Duration { get; set; }
        public int Status { get; set; }
    }
}