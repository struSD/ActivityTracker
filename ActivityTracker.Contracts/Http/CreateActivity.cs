namespace ActivityTracker.Contracts.Http
{

    public class CreateActivityRequest
    {
        public string ActivityType { get; set; }
        public int ActivityDuration { get; set; }
        public int UserId { get; set; }
    }

    public class CreateActivityResponce
    {
        public int Id { get; init; }
    }
}