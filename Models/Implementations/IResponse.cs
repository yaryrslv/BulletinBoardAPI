namespace BulletinBoardAPI.Models.Implementations
{
    interface IResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
