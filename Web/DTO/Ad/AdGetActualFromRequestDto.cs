using System.Collections.Generic;

namespace Web.DTO.Ad
{
    public class AdGetActualFromRequestDto
    {
        public IEnumerable<Data.Models.Realizations.Ad> RequestedAds { get; set; }
    }
}
