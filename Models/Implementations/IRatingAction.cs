namespace BulletinBoardAPI.Models.Realizations
{
    public interface IRatingAction
    {
        string Id { get; set; }
        string Time { get; set; }
        string AdId { get; set; }
        string UserName { get; set; }
    }
}