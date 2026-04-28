using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsApp1
{
    public class PostLinkedList
    {
        // Hàm này giúp WinForms lấy danh sách bài đăng dạng đối tượng để đưa lên bảng (Grid)

        private Post head; // Node đầu tiên của danh sách

        public PostLinkedList()
        {
            head = null;
        }
        
        public List<Post> GetAllPosts()
        {
            List<Post> list = new List<Post>();
            Post current = head;
            while (current != null)
            {
                list.Add(current);
                current = current.Next;
            }
            return list;
        }

        // 1. Thêm bài mới vào đầu (Thường dùng cho Newest Feed) - O(1)
        public void AddFirst(string id, string content)
        {
            Post newPost = new Post(id, content);
            newPost.Next = head;
            head = newPost;
            System.Diagnostics.Debug.WriteLine($"Đã thêm bài {id} vào đầu danh sách.");
        }

        // 2. Thêm bài vào cuối (Thường dùng cho bài đăng cũ hơn) - O(n)
        public void AddLast(string id, string content, DateTime? time = null)
        {
            Post newPost = new Post(id, content,time);
            if (head == null)
            {
                head = newPost;
            }
            else
            {
                Post current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newPost;
            }
            System.Diagnostics.Debug.WriteLine($"Đã thêm bài {id} vào cuối danh sách.");
        }

        // 3. Xóa bài theo ID - O(n)
        public void Delete(string id)
        {
            if (head == null) return;
            if (head.PostId == id)
            {
                head = head.Next;
                return;
            }
            Post current = head;
            while (current.Next != null && current.Next.PostId != id)
            {
                current = current.Next;
            }

            if (current.Next != null)
            {
                current.Next = current.Next.Next;
                System.Diagnostics.Debug.WriteLine($"Đã xóa bài có ID: {id}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Không tìm thấy bài có ID: {id} để xóa.");
            }
        }
        //TĂNG LIKE
        public bool TangLike(string id)
        {
            Post current = head;
            while (current != null)
            {
                if (current.PostId == id)
                {
                    current.Likes++; // Tăng lượt thích lên 1
                    return true; // Trả về true báo hiệu thành công
                }
                current = current.Next;
            }
            return false; // Không tìm thấy ID
        }

        //SỬA NỘI DUNG
        public bool SuaNoiDung(string id, string noiDungMoi)
        {
            Post current = head;
            while (current != null)
            {
                if (current.PostId == id)
                {
                    current.Content = noiDungMoi; // Ghi đè dữ liệu mới
                    return true;
                }
                current = current.Next;
            }
            return false;
        }
        //Duple check
        public bool KiemTraTrungID(string id)
        {
            Post current = head;
            while (current != null)
            {
                if (current.PostId == id) return true; // Tìm thấy ID đã tồn tại
                current = current.Next;
            }
            return false;
        }
        //THUẬT TOÁN LỌC TỪ KHÓA
        public List<Post> TimTheoTuKhoa(string keyword)
        {
            List<Post> ketQua = new List<Post>();
            Post current = head;
            string kw = keyword.ToLower(); // Chuyển về chữ thường để dễ so sánh

            while (current != null)
            {
                if (current.Content.ToLower().Contains(kw))
                {
                    ketQua.Add(current); // Nếu chứa từ khóa thì đưa vào danh sách kết quả
                }
                current = current.Next;
            }
            return ketQua;
        }
        // 4. Duyệt danh sách (Hiển thị tất cả bài đăng)
        public void Traverse()
        {
            if (head == null)
            {
                System.Diagnostics.Debug.WriteLine("Bảng tin trống.");
                return;
            }
            System.Diagnostics.Debug.WriteLine("\n--- DANH SÁCH BÀI ĐĂNG ---");
            Post current = head;
            while (current != null)
            {
                System.Diagnostics.Debug.WriteLine(current.ToString());
                current = current.Next;
            }
            System.Diagnostics.Debug.WriteLine("--------------------------\n");
        }
        // 5. Tìm bài (theo ID hoặc nội dung)
        public enum TieuChiTim
        {
            TimTheoId,
            TimTheoContent
        }
        public Post Find(string a, TieuChiTim Tieuchi)
        {
            Post current = head;
            bool found = false;
            while (current != null)
            {
                switch (Tieuchi)
                {
                    case TieuChiTim.TimTheoId:
                        {
                            if (current.PostId == a)
                            {
                                Console.WriteLine(current);
                                found = true;
                                return current;
                            }
                            current = current.Next;
                            break;
                        }
                    case TieuChiTim.TimTheoContent:
                        {
                            List<Post> result = new List<Post>();
                            string kw = a.ToLower();
                            if (current.Content.ToLower().Contains(kw))
                            {
                                result.Add(current);
                                foreach (Post post in result)
                                {
                                    Console.WriteLine(post);
                                }
                                found = true;
                            }
                            current = current.Next;
                            break;
                        }
                }
            }
            if (found == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                System.Diagnostics.Debug.WriteLine("Không tìm thấy bài đăng");
                Console.ResetColor();
            }
            return null;
        }
        // 6. Đăng bài mới
        public void DangBaiMoi(string id, string content)
        {
            Post newpost = new Post(id,content);
            if (head == null)
            {
                head = newpost;
            }
            else
            {
                Post current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newpost;
            }
            System.Diagnostics.Debug.WriteLine($"Đã thêm bài {id} vào cuối danh sách.");
        }
        // 7. Sắp xếp
        public enum Tieuchisort
        {
            NgayDangGiam,
            NgayDangTang,
            LikeGiam,
            LikeTang,
            ContentAtoiZ,
            TheoId,
        }
        public void Sort(Tieuchisort Tieuchi)
        {
            head = MergeSort(head, Tieuchi);
            System.Diagnostics.Debug.WriteLine("Đã sắp xếp danh sách.");
        }

        private Post MergeSort(Post head, Tieuchisort Tieuchi)
        {
            if (head == null || head.Next == null)
                return head;

            Post mid = GetMid(head);
            Post secondHalf = mid.Next;
            mid.Next = null;

            Post left = MergeSort(head, Tieuchi);
            Post right = MergeSort(secondHalf, Tieuchi);

            return Merge(left, right, Tieuchi);
        }

        private Post GetMid(Post head)
        {
            if (head == null) return head;

            Post current = head, currentx2 = head.Next;
            while (currentx2 != null && currentx2.Next != null)
            {
                current = current.Next;
                currentx2 = currentx2.Next.Next;
            }
            return current;
        }

        private Post Merge(Post left, Post right, Tieuchisort Tieuchi)
        {
            if (left == null) return right;
            if (right == null) return left;

            Post result;
            if (Compare(left, right, Tieuchi))
            {
                result = left;
                result.Next = Merge(left.Next, right, Tieuchi);
            }
            else
            {
                result = right;
                result.Next = Merge(left, right.Next, Tieuchi);
            }
            return result;
        }

        private bool Compare(Post a, Post b, Tieuchisort Tieuchi)
        {
            switch (Tieuchi)
            {
                case Tieuchisort.NgayDangGiam:
                    {
                        return a.Timestamp >= b.Timestamp;
                    }
                case Tieuchisort.NgayDangTang:
                    {
                        return a.Timestamp <= b.Timestamp;
                    }
                case Tieuchisort.LikeGiam:
                    {
                        return a.Likes >= b.Likes;
                    }
                case Tieuchisort.LikeTang:
                    {
                        return a.Likes <= b.Likes;
                    }
                case Tieuchisort.ContentAtoiZ:
                    {
                        return string.Compare(a.Content, b.Content, StringComparison.OrdinalIgnoreCase) <= 0;
                    }
                default:
                    {
                        return a.PostId.CompareTo(b.PostId) <= 0;
                    }
            }
        }
        // 8. Xử lý logic hiển thị danh sách 
        public List<string> GetDisplayList()
        {
            List<string> displayList = new List<string>();
            Post current = head;
            while (current != null)
            {
                displayList.Add(current.ToString());
                current = current.Next;
            }
            return displayList;
        }
        // 9. Kết nối dữ liệu với giao diện
        public void KetNoiDuLieuVoiGiaoDien()
        {
            if (head == null)
            {
                System.Diagnostics.Debug.WriteLine("Bảng tin trống.");
                return;
            }

            System.Diagnostics.Debug.WriteLine("--- DANH SÁCH BÀI ĐĂNG ---");
            Post current = head;
            while (current != null)
            {
                System.Diagnostics.Debug.WriteLine(current.ToString());
                current = current.Next;
            }
            System.Diagnostics.Debug.WriteLine("--------------------------");
        }
    }
}
