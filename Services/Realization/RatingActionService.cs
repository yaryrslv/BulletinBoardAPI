using BulletinBoardAPI.EF;

namespace BulletinBoardAPI.Services.Realization
{
    public class RatingActionService
    {
        private BulletinBoardContext _context;
        public RatingActionService(BulletinBoardContext context)
        {
            _context = context;
        }

    }
}
