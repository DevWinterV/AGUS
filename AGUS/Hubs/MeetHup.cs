using AGUS.Services;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace AGUS.Hubs
{
    public class MeetHup : Hub
    {
        public async Task JoinMeetting(string roomId, string userId, string name)
        {

            User.list_meet.Add(Context.ConnectionId, new UserInfo
            {
                ConnectionId = Context.ConnectionId,
                UserId = userId,
                Name = name,
                RoomId = roomId
            });
            // Check if the room exists in the dictionary
            if (!Rooms.list_meet.ContainsKey(roomId))
            {
                // If the room doesn't exist, initialize it with 0 users
                Rooms.list_meet[roomId] = 1;
            }

            // Check if the room has more than 2 users
            if (Rooms.list_meet[roomId] > 10)
            {
                await Clients.Caller.SendAsync("cannot-join-meet", Context.ConnectionId, "Phòng đã đủ số lượng");
                // Send a message indicating that the user can't join
            }
            else
            {
                // Increment the user count for the room
                Rooms.list_meet[roomId]++;

                // Add the user to the room group
                await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

                // Send a message to all users in the room that a new user has connected
                await Clients.Group(roomId).SendAsync("user-connected-meet", userId, name);
            }
        }
        public async Task SendMessageToRoom(string roomName, string user, string message)
        {
            UserInfo userinfo = User.list_meet.Values.FirstOrDefault(u => u.UserId.Equals(user));
            await Clients.Group(roomName).SendAsync("ReceiveMessage", user, message, userinfo.Name);
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
            UserInfo userinfo = User.list_meet.Values.FirstOrDefault(u => u.ConnectionId.Equals(Context.ConnectionId));
            await Clients.Group(roomId).SendAsync("user-sharecreen", userinfo.UserId, userinfo.Name, isshare);
        }

        
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (User.list_meet.Count > 0 && User.list_meet.ContainsKey(Context.ConnectionId))
            {
                UserInfo userinfo = User.list_meet.Values.FirstOrDefault(u => u.ConnectionId.Equals(Context.ConnectionId));
                Clients.All.SendAsync("user-disconneted-meet", userinfo.UserId, userinfo.Name);
                if (Rooms.list_meet.ContainsKey(userinfo.RoomId))
                {
                    Rooms.list_meet[userinfo.RoomId]--;
                }
                User.list_meet.Remove(Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(exception);
        }
        public async Task LeaveMeet(string roomId)
        {
            if (User.list_meet.Count > 0 && User.list_meet.ContainsKey(Context.ConnectionId))
            {
                UserInfo userinfo = User.list_meet.Values.FirstOrDefault(u => u.ConnectionId.Equals(Context.ConnectionId));
                await Clients.Group(roomId).SendAsync("user-leavemeet", Context.ConnectionId, userinfo.Name);
                if (Rooms.list_meet.ContainsKey(userinfo.RoomId))
                {
                    Rooms.list_meet[userinfo.RoomId]--;
                }
                // Kiểm tra xem phòng có tồn tại trong từ điển không trước khi cố gắng xóa nó
                if (Rooms.list_meet.Count == 1)
                {
                    Rooms.list_meet.Remove(roomId);
                }
                User.list_meet.Remove(Context.ConnectionId);
            }
        }
    }
}
