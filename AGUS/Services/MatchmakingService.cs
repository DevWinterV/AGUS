using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGUS.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace AGUS.Services
{
    public class MatchmakingService 
    {
        private readonly List<User> _users = new List<User>();
        private readonly IHubContext<DefautHub> _hubContext;

        public MatchmakingService(IHubContext<DefautHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void AddUser(User user)
        {
            _users.Add(user);
            TryMatchUsers();
        }

        public void RemoveUser(string connectionId)
        {
            var user = _users.FirstOrDefault(u => u.ConnectionId == connectionId);
            if (user != null)
            {
                _users.Remove(user);
            }
        }

        private async void TryMatchUsers()
        {
            var random = new Random();

            var matchingUsers = _users
                .Where(u => u.IsSearching && (u.Gender == "nam" || u.Gender == "nu"))
                .GroupBy(u => u.Gender)  // Nhóm theo giới tính
                .SelectMany(group => group.OrderBy(x => random.Next()).Take(1))  // Chọn ngẫu nhiên 1 người dùng từ mỗi nhóm
                .Take(2);


            if (matchingUsers.Count() == 2)
            {
                var roomID = Guid.NewGuid();
                // Create a room or perform other steps to connect video between two users
                // Send notifications to both users about finding a partner and open the video connection
                foreach (var user in matchingUsers)
                {
                    _users.Remove(user);
                    // Send a notification or perform other steps to open the video connection
                    // Use SignalR to send a notification
                    await _hubContext.Clients.Client(user.ConnectionId).SendAsync("MatchFound", roomID);
                }
            }
        }
    }
    public class User
    {
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public string Gender { get; set; }
        public bool IsSearching { get; set; }
    }
}
