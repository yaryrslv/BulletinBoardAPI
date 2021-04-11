using System.Collections.Generic;

namespace BulletinBoardAPI.DTO.Ad
{
    public class AdGetActualFromRequestDto
    {
        public IEnumerable<Models.Realizations.Ad> requestedAds { get; set; }
    }
}
