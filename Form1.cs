using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1 
{
    public partial class Form1 : Form
    {
        // ── Data ──
        private PostLinkedList _feed = new PostLinkedList();

        // ── Controls ──
        private Panel _headerPanel;
        private Label _lblTitle;

        private GroupBox _grpAdd;
        private TextBox _txtId, _txtContent;
        private ComboBox _cmbInsertMode;
        private Button _btnAdd;

        private GroupBox _grpActions;
        private TextBox _txtThaoTac;
        private Button _btnSearch, _btnDeleteId, _btnLike, _btnEdit, _btnFilter;
        private ComboBox _cmbSort;
        private Button _btnSort;

        private DataGridView _dgvPosts;
        private RichTextBox _rtbLog;

        public Form1()
        {
            KhoiTaoGiaoDien();
            TaoDuLieuMau(); // Tính năng 4: Nạp sẵn dữ liệu khi mở phần mềm
            RefreshGrid(_feed.GetAllPosts()); // Load dữ liệu mặc định
        }

        // GIẢ LẬP DỮ LIỆU TỰ ĐỘNG
        private void TaoDuLieuMau()
        {
            Random rnd = new Random();
            string[] authors = { "UEH_Student", "K51_Finance", "Dev_CSharpVN", "GamerVietPro", "Busch", "Admin_UEH", "EconomicsBuff" };
            string[] verbs = { "đang học", "vừa hoàn thành", "đang tìm hiểu sâu", "rất hào hứng với", "mới thử nghiệm" };
            string[] topics = { "C# 14 Extension Members", "Agentic AI", ".NET 10 Performance", "GTA VI", "Tariff 2026", "Đồ án UEH" };
            string[] icons = { "🔥", "🚀", "💡", "🧠", "🎮", "🤖", "⚡", "💻", "✨" };

            // Thiết lập mốc thời gian thực tế cho năm 2026
            DateTime now = DateTime.Now;
            DateTime startDate = now.AddMonths(-6);

            for (int i = 1; i <= 1000; i++)
            {
                string autoId = "P" + i.ToString("D4");
                string author = authors[rnd.Next(authors.Length)];
                string verb = verbs[rnd.Next(verbs.Length)];
                string topic = topics[rnd.Next(topics.Length)];
                string icon = icons[rnd.Next(icons.Length)];

                // Tạo nội dung
                string randomContent = $"{author} {verb} {topic} {icon}";

                // TẠO THỜI GIAN NGẪU NHIÊN
                TimeSpan timeRange = (rnd.Next(100) < 60) ? (now - startDate) : (now - startDate) * 0.4;
                int randomMinutes = rnd.Next(0, (int)timeRange.TotalMinutes);
                DateTime randomPostTime = now.AddMinutes(-randomMinutes);

                // THÊM VÀO LINKEDLIST (Truyền thời gian vào để Sort chuẩn)
                _feed.AddLast(autoId, randomContent, randomPostTime);

                // GÁN LIKE (Phân bổ thực tế: Thường - Phổ biến - Viral)
                Post lastPost = _feed.Find(autoId, PostLinkedList.TieuChiTim.TimTheoId);
                if (lastPost != null)
                {
                    int likeCategory = rnd.Next(100);
                    lastPost.Likes = likeCategory switch
                    {
                        < 65 => rnd.Next(0, 801),
                        < 92 => rnd.Next(800, 3500),
                        _ => rnd.Next(3500, 15000)
                    };
                }
            }
            Log("🚀 Đã khởi tạo 1000 bài đăng 2026 với phân bổ Like và Thời gian thực tế!");
        }

        private void KhoiTaoGiaoDien()
        {
            this.Text = "📱 Quản Lý Bài Đăng Mạng Xã Hội - Linked List";
            this.Size = new Size(1100, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 242, 245);
            this.Font = new Font("Segoe UI", 9f);

            // ── Header ──
            _headerPanel = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.FromArgb(24, 119, 242) };
            _lblTitle = new Label { Text = "🌐 Social Media Post Manager [Singly Linked List]", ForeColor = Color.White, Font = new Font("Segoe UI", 14f, FontStyle.Bold), AutoSize = true, Location = new Point(15, 14) };
            _headerPanel.Controls.Add(_lblTitle);
            this.Controls.Add(_headerPanel);

            // ── Left panel ──
            Panel leftPanel = new Panel { Location = new Point(10, 65), Size = new Size(350, 630), BackColor = Color.Transparent };

            // GroupBox Thêm/Sửa bài đăng
            _grpAdd = new GroupBox { Text = "➕  Nội Dung Bảng Tin", Location = new Point(0, 0), Size = new Size(345, 200), Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            _txtId = MakeTextBox("Nhập ID Bài Đăng...", new Point(10, 30), 320);
            _txtContent = MakeTextBox("Nội dung bài đăng (hoặc Nội dung muốn sửa)...", new Point(10, 65), 320, 60, true);
            _cmbInsertMode = new ComboBox { Location = new Point(10, 135), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbInsertMode.Items.AddRange(new[] { "Thêm vào đầu (Newest)", "Thêm vào cuối (Oldest)" });
            _cmbInsertMode.SelectedIndex = 0;
            _btnAdd = MakeButton("➕ Đăng bài", new Point(170, 133), 160, Color.FromArgb(24, 119, 242));
            _btnAdd.Click += BtnAdd_Click;
            _grpAdd.Controls.AddRange(new Control[] { _txtId, _txtContent, _cmbInsertMode, _btnAdd });

            // GroupBox Thao tác MỚI
            _grpActions = new GroupBox { Text = "⚙️  Thao tác tương tác", Location = new Point(0, 210), Size = new Size(345, 215), Font = new Font("Segoe UI", 9f, FontStyle.Bold) };
            Label lbl1 = new Label { Text = "Nhập ID hoặc Từ khóa:", Location = new Point(10, 25), AutoSize = true };
            _txtThaoTac = MakeTextBox("Ví dụ: P001 hoặc 'Valorant'", new Point(10, 45), 320);

            // Hàng 1: Tìm ID | Xóa | Like | Sửa
            _btnSearch = MakeButton("🔍 Tìm", new Point(10, 80), 75, Color.FromArgb(66, 133, 244));
            _btnSearch.Click += BtnSearch_Click;
            _btnDeleteId = MakeButton("🗑 Xoá", new Point(90, 80), 75, Color.FromArgb(220, 53, 69));
            _btnDeleteId.Click += BtnDeleteId_Click;
            _btnLike = MakeButton("❤️ Like", new Point(170, 80), 75, Color.FromArgb(233, 30, 99)); // TÍNH NĂNG 1
            _btnLike.Click += BtnLike_Click;
            _btnEdit = MakeButton("✏️ Sửa", new Point(250, 80), 80, Color.FromArgb(255, 152, 0)); // TÍNH NĂNG 2
            _btnEdit.Click += BtnEdit_Click;

            // Hàng 2: Tìm tương đối
            _btnFilter = MakeButton("🔎 Lọc Bảng Tin (Theo Từ Khóa)", new Point(10, 115), 320, Color.FromArgb(0, 150, 136)); // TÍNH NĂNG 3
            _btnFilter.Click += BtnFilter_Click;

            // Hàng 3: Sắp Xếp
            Label lbl4 = new Label { Text = "Sắp xếp:", Location = new Point(10, 163), AutoSize = true };
            _cmbSort = new ComboBox { Location = new Point(70, 160), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            _cmbSort.Items.AddRange(new[] { "Ngày Đăng (Giảm)", "Ngày Đăng (Tăng)", "Lượt Like (Giảm)", "ID Bài (A-Z)" });
            _cmbSort.SelectedIndex = 0;
            _btnSort = MakeButton("🔃 Sort", new Point(230, 158), 100, Color.FromArgb(102, 51, 153));
            _btnSort.Click += BtnSort_Click;

            _grpActions.Controls.AddRange(new Control[] { lbl1, _txtThaoTac, _btnSearch, _btnDeleteId, _btnLike, _btnEdit, _btnFilter, lbl4, _cmbSort, _btnSort });
            leftPanel.Controls.AddRange(new Control[] { _grpAdd, _grpActions });

            // ── Right panel ──
            Panel rightPanel = new Panel { Location = new Point(370, 65), Size = new Size(700, 630), BackColor = Color.Transparent };
            Label lblFeed = new Label { Text = "📰  Bảng Tin (News Feed)", Font = new Font("Segoe UI", 11f, FontStyle.Bold), ForeColor = Color.FromArgb(24, 119, 242), Location = new Point(0, 0), AutoSize = true };

            _dgvPosts = new DataGridView
            {
                
                Location = new Point(0, 30),
                Size = new Size(700, 350),
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(224, 224, 224)
            };
            StyleGrid();

            Label lblLog = new Label { Text = "📋  Log hoạt động hệ thống:", Font = new Font("Segoe UI", 9f, FontStyle.Bold), Location = new Point(0, 395), AutoSize = true };
            _rtbLog = new RichTextBox
            {
                Location = new Point(0, 420),
                Size = new Size(700, 200),
                ReadOnly = true,
                BackColor = Color.FromArgb(40, 44, 52),
                ForeColor = Color.FromArgb(97, 218, 251),
                Font = new Font("Consolas", 9.5f),
                BorderStyle = BorderStyle.None
            };

            rightPanel.Controls.AddRange(new Control[] { lblFeed, _dgvPosts, lblLog, _rtbLog });
            this.Controls.AddRange(new Control[] { leftPanel, rightPanel });
            //double
            _dgvPosts.CellDoubleClick += (s, ev) => {
                // Kiểm tra nếu nhấp vào dòng dữ liệu (không phải tiêu đề)
                if (ev.RowIndex >= 0)
                {
                    string id = _dgvPosts.Rows[ev.RowIndex].Cells["colId"].Value.ToString();
                    string content = _dgvPosts.Rows[ev.RowIndex].Cells["colContent"].Value.ToString();

                    // Hiển thị nội dung đầy đủ trong một MessageBox
                    MessageBox.Show(
                        $"--- CHI TIẾT BÀI ĐĂNG [{id}] ---\n\n{content}",
                        "Chi tiết bài đăng",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            };

        }
        // ─── Helper builders (Giữ nguyên) ───
        private TextBox MakeTextBox(string placeholder, Point loc, int w, int h = 25, bool multiline = false)
        {
            var tb = new TextBox { Location = loc, Width = w, Height = h, Multiline = multiline, ForeColor = Color.Gray, Text = placeholder };
            tb.GotFocus += (s, e) => { if (tb.ForeColor == Color.Gray) { tb.Text = ""; tb.ForeColor = Color.Black; } };
            tb.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(tb.Text)) { tb.Text = placeholder; tb.ForeColor = Color.Gray; } };
            return tb;
        }

        private Button MakeButton(string text, Point loc, int w, Color bg)
        {
            return new Button { Text = text, Location = loc, Width = w, Height = 28, BackColor = bg, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Font = new Font("Segoe UI", 8.5f, FontStyle.Bold) };
        }

        private void StyleGrid()
        {
            _dgvPosts.Columns.Clear();
            _dgvPosts.Columns.Add("colId", "Mã ID"); _dgvPosts.Columns[0].FillWeight = 15;
            _dgvPosts.Columns.Add("colContent", "Nội Dung Bài Đăng"); _dgvPosts.Columns[1].FillWeight = 50;
            _dgvPosts.Columns.Add("colLikes", "❤️ Likes"); _dgvPosts.Columns[2].FillWeight = 15;
            _dgvPosts.Columns.Add("colTime", "Thời Gian"); _dgvPosts.Columns[3].FillWeight = 20;

            _dgvPosts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(24, 119, 242);
            _dgvPosts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _dgvPosts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            _dgvPosts.RowsDefaultCellStyle.BackColor = Color.White;
            _dgvPosts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 248, 250);
        }

        // ─── XỬ LÝ SỰ KIỆN ───
        private void RefreshGrid(List<Post> dataToDisplay)
        {
            _dgvPosts.Rows.Clear();
            foreach (Post p in dataToDisplay)
            {
                _dgvPosts.Rows.Add(p.PostId, p.Content, p.Likes, p.Timestamp.ToString("HH:mm:ss dd/MM"));
            }
        }

        private void Log(string msg) { _rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\n"); _rtbLog.ScrollToCaret(); }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string id = _txtId.Text.Trim(); string content = _txtContent.Text.Trim();
            if (_feed.KiemTraTrungID(id))
            {
                MessageBox.Show("Lỗi: ID bài đăng này đã tồn tại! Vui lòng chọn ID khác.",
                                "Trùng mã định danh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Dừng việc thêm bài
            }
            if (id == "Nhập ID Bài Đăng..." || content.StartsWith("Nội dung")) return;

            if (_cmbInsertMode.SelectedIndex == 0) { _feed.AddFirst(id, content); Log($"[Thêm Đầu] Đăng bài: {id}"); }
            else { _feed.AddLast(id, content); Log($"[Thêm Cuối] Đăng bài: {id}"); }

            RefreshGrid(_feed.GetAllPosts());
        }

        private void BtnDeleteId_Click(object sender, EventArgs e)
        {
            string id = _txtThaoTac.Text.Trim();
            if (id.StartsWith("Ví dụ") || string.IsNullOrWhiteSpace(id)) return;
            _feed.Delete(id); Log($"[Xoá] Thực thi lệnh xoá ID: {id}"); RefreshGrid(_feed.GetAllPosts());
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string id = _txtThaoTac.Text.Trim();
            Post res = _feed.Find(id, PostLinkedList.TieuChiTim.TimTheoId);
            if (res != null) { Log($"[Tìm Kiếm] Khớp bài ID: {id}"); RefreshGrid(new List<Post>() { res }); } // Đưa 1 bài lên grid
            else Log($"[Tìm Kiếm] Lỗi: Không tồn tại ID {id}");
        }

        // TÍNH NĂNG 1: THẢ LIKE
        private void BtnLike_Click(object sender, EventArgs e)
        {
            string id = _txtThaoTac.Text.Trim();
            bool ok = _feed.TangLike(id);
            if (ok) { Log($"[Tương tác] Đã thả tim bài viết {id} ❤️"); RefreshGrid(_feed.GetAllPosts()); }
            else Log($"[Lỗi] Không tìm thấy bài viết {id} để Like.");
        }

        // TÍNH NĂNG 2: SỬA NỘI DUNG (Cập nhật)
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            string id = _txtThaoTac.Text.Trim();
            string contentMoi = _txtContent.Text.Trim();
            if (contentMoi.StartsWith("Nội dung")) { MessageBox.Show("Vui lòng nhập nội dung mới vào ô Nội dung!"); return; }

            bool ok = _feed.SuaNoiDung(id, contentMoi);
            if (ok) { Log($"[Cập nhật] Đã thay đổi nội dung bài {id}"); RefreshGrid(_feed.GetAllPosts()); }
            else Log($"[Lỗi] Không tìm thấy bài viết {id} để Sửa.");
        }

        // TÍNH NĂNG 3: LỌC TỪ KHÓA
        private void BtnFilter_Click(object sender, EventArgs e)
        {
            string kw = _txtThaoTac.Text.Trim();
            if (kw.StartsWith("Ví dụ") || string.IsNullOrWhiteSpace(kw)) { RefreshGrid(_feed.GetAllPosts()); return; } // Rỗng thì hiển thị tất cả

            List<Post> ketQua = _feed.TimTheoTuKhoa(kw);
            Log($"[Lọc] Tìm thấy {ketQua.Count} bài viết chứa từ: '{kw}'");
            RefreshGrid(ketQua); // Chỉ đổ dữ liệu đã lọc lên Grid
        }

        private void BtnSort_Click(object sender, EventArgs e)
        {
            int idx = _cmbSort.SelectedIndex;
            if (idx == 0) _feed.Sort(PostLinkedList.Tieuchisort.NgayDangGiam);
            else if (idx == 1) _feed.Sort(PostLinkedList.Tieuchisort.NgayDangTang);
            else if (idx == 2) _feed.Sort(PostLinkedList.Tieuchisort.LikeGiam);
            else _feed.Sort(PostLinkedList.Tieuchisort.TheoId);

            Log($"[Sắp Xếp] Hoàn tất Merge Sort theo tiêu chí: {_cmbSort.Text}");
            RefreshGrid(_feed.GetAllPosts());
        }
    }
}