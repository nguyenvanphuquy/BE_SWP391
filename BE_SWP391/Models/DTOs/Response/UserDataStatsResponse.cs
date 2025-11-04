namespace BE_SWP391.Models.DTOs.Response
{
    public class UserDataStatsResponse
    {
        public int TotalData { get; set; }
        public int ActiveData { get; set; }
        public int ApprovedData { get; set; }
        public int PendingData { get; set; }
    }
}
