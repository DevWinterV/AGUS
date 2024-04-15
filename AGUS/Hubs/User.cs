namespace AGUS.Hubs
{
    public static class User
    {
        public static IDictionary<string, UserInfo> list_meet = new Dictionary<string, UserInfo>();

        public static IDictionary<string, UserInfo> list = new Dictionary<string, UserInfo>();
        public static IDictionary<string, string> list_online = new Dictionary<string, string>();

    }
    public class UserInfo
    {
        public string ConnectionId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string RoomId { get; set; }

    }
}
