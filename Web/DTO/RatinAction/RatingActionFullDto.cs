using System;

namespace Web.DTO.RatinAction
{
    public class RatingActionFullDto
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public Guid AdId { get; set; }
        public string UserName { get; set; }
    }
}
