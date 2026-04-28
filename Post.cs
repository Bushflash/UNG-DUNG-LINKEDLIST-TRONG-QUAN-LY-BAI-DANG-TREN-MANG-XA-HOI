using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp1
{
    public class Post
    {
        // Dữ liệu của bài đăng
        public string PostId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public int Likes { get; set; }

        // Con trỏ tới bài tiếp theo
        public Post Next { get; set; }
        public Post(string id, string content, DateTime? customTime = null)
        {
            PostId = id;
            Content = content;
            Timestamp = customTime ?? DateTime.Now; // Tự động lấy thời gian hiện tại
            Likes = 0;
            Next = null;
        }

        public override string ToString()
        {
            return $"[{PostId}] - {Timestamp:HH:mm:ss}: {Content} ({Likes} likes)";
        }
    }
}
