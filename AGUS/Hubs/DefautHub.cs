using AGUS.Services;
using Microsoft.AspNetCore.SignalR;

namespace AGUS.Hubs
{
    public class DefautHub : Hub
    {
        private int count;

        private readonly MatchmakingService _matchmakingService;
        public DefautHub(MatchmakingService matchmakingService)
        {
            _matchmakingService = matchmakingService;
        }
        public async Task JoinRoom(string roomId, string userId)
        {

   
                    
             if (!Rooms.list.ContainsKey(roomId))
            {
                // If the room doesn't exist, initialize it with 0 users
                Rooms.list[roomId] = 0;
            }

            // Check if the room has more than 2 users
            if (Rooms.list[roomId] >= 2)
            {
                await Clients.Caller.SendAsync("cannot-join-room", Context.ConnectionId, "Phòng đã đủ số lượng hoặc đã hủy");
                // Send a message indicating that the user can't join
            }
            else
            {
                User.list.Add(Context.ConnectionId, new UserInfo
                {
                    ConnectionId = Context.ConnectionId,
                    UserId = userId,
                    Name = Context.ConnectionId,
                    RoomId = roomId
                });
                // Increment the user count for the room
                Rooms.list[roomId]++;

                // Add the user to the room group
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

                // Send a message to all users in the room that a new user has connected
                await Clients.Group(roomId).SendAsync("user-connected", userId);
            }
        }
        public async Task ToggleVideo(string roomId, string userId, bool isVideoOn)
        {
            // Lưu trạng thái camera của người dùng vào một danh sách hoặc dictionary
            // ...

            // Gửi lại thông điệp đến tất cả người dùng khác trong phòng
            await Clients.Group(roomId).SendAsync("user-toggled-video", userId, isVideoOn);
        }

        public async Task ShareScreen(string roomId, bool isshare)
        {
            await Clients.Group(roomId).SendAsync("user-sharecreen", Context.ConnectionId,isshare);
        }

        public async Task SendMessageToRoom(string roomName, string user, string message)
        {
            await Clients.Group(roomName).SendAsync("ReceiveMessage", user, message);
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if(User.list.Count > 0 && User.list.ContainsKey(Context.ConnectionId))
            {
                _matchmakingService.RemoveUser(Context.ConnectionId);
                UserInfo userinfo = User.list.Values.FirstOrDefault(u => u.ConnectionId.Equals(Context.ConnectionId));
                // Kiểm tra xem phòng có tồn tại trong từ điển không trước khi cố gắng xóa nó
                if (Rooms.list.ContainsKey(userinfo.RoomId) )
                {
                    Clients.All.SendAsync("user-disconneted", User.list[Context.ConnectionId]);
                    Rooms.list.Remove(userinfo.RoomId);
                }
                User.list.Remove(Context.ConnectionId);
            }
            count -= 1;
            Clients.All.SendAsync("online-live", count);
            return base.OnDisconnectedAsync(exception);
        }
        public async Task LeaveRoom(string roomId)
        {
            if (User.list.Count > 0 && User.list.ContainsKey(Context.ConnectionId))
            {
                await Clients.Group(roomId).SendAsync("user-leaveroom", Context.ConnectionId);
                // Kiểm tra xem phòng có tồn tại trong từ điển không trước khi cố gắng xóa nó
                if (Rooms.list.ContainsKey(roomId))
                {
                    Rooms.list.Remove(roomId);
                }
                User.list.Remove(Context.ConnectionId);
            }
        }
        public override Task OnConnectedAsync()
        {
            count += 1;
            Clients.All.SendAsync("online-live", count);
            return base.OnConnectedAsync();
        }

        // Thêm user vào hàng đợi.
        public  void JoinQueue(string gender, string name)
        {
            var user = new Services.User
            {
                Name = name,
                ConnectionId = Context.ConnectionId,
                Gender = gender,
                IsSearching = true
            };
            _matchmakingService.AddUser(user);
        }
        // Rời hàng đợi
        public void LeaveQueue()
        {
            _matchmakingService.RemoveUser(Context.ConnectionId);
        }
    }
}
