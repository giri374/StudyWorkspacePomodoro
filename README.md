# Pomodoro Web App

Ứng dụng quản lý thời gian Pomodoro viết bằng ASP.NET Razor Pages.

## ⚙️ Cài đặt

1. Clone repo:
   ```bash
   git clone https://github.com/giri374/StudyWorkspacePomodoro.git
2. Tạo CSDL
Mở giao diện SQL của bạn, chạy SQLServerDB.sql để tạo database

## Cấu hình cơ sở dữ liệu (hạn chế tình trạng xung đột CSDL)
1. Copy file `appsettings.example.json` thành `appsettings.json`
2. Chỉnh sửa `ConnectionStrings` phù hợp với máy của bạn. Cần sửa User ID, Password
3. KHÔNG commit `appsettings.json` vào Git (đã được ignore) 