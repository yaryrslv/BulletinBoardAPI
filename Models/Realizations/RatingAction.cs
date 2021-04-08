namespace BulletinBoardAPI.Models.Realizations
{
    public class RatingAction : IRatingAction
    {
        public string Id { get; set; }
        public string Time { get; set; }
        public string AdId { get; set; }
        public string UserName { get; set; }
    }
}
