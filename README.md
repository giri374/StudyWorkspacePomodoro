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

## Miêu tả web:
4 màn hình:pomodoro, quản lý workspace, quản lý nhạc, cài đặt pomodoro
Layout: Footer/Side bar dẫn link tới 4 màn hình
## Người dùng có thể:
+ Thêm/sửa/xóa không gian làm việc (màn hình quản lý workspace)
+ Thêm/sửa/xóa nhạc nền (màn hình quản lý nhạc)
+ Xem và sử dụng các không gian, nhạc có sẵn trong trang web (màn hình pomodoro)
+ Bắt đầu thời gian Pomodoro, nhận chuông báo sau 25 phút (màn hình pomodoro)
+ Cài đặt thời gian pomodoro (màn hình cài đặt pomodoro) (sẽ phát triển sau)
### Với màn hình MainScreen, người dùng có thể: 

+ Xem và sử dụng các không gian, nhạc có sẵn trong trang web (2 nút chọn nhạc  và chọn workspace, nhấn vào sẽ xổ ra thông tin)
+ tên workspace và ảnh nền ở chính giữa màn hình (hiển thị workspace được chọn)
+ Khu vực Pomodoro (hiển thị đè lên ảnh nền):
  StartPomodoro: bắt đầu thời gian Pomodoro (nút Start), nhận chuông báo sau 25 phút, xem thời gian pomodoro còn lại (ô số 25:00 đếm ngược) 
  ShortBreak: tương tự StartPomodoro, nhận chuông báo sau 5 phút (hiển thị thời gian nghỉ còn lại) (giao diện ShortBreak sẽ thay thế giao diện StartPomodoro)

 # Structure

 StudyWorkspace/

│

├── Pages/                   # Contains all Razor Pages

│   ├── Workspaces/          # Pages for managing workspaces

│   │   ├── Index.cshtml     # List all workspaces

│   │   ├── Index.cshtml.cs  # Logic for listing workspaces

│   │   ├── Create.cshtml    # Form to create a workspace

│   │   ├── Create.cshtml.cs # Logic for creating a workspace

│   │   ├── Edit.cshtml      # Form to edit a workspace

│   │   ├── Edit.cshtml.cs   # Logic for editing a workspace

│   │   └── Delete.cshtml    # Confirmation page for deletion

│   │       └── Delete.cshtml.cs # Logic for deleting a workspace

│   ├── Music/               # Pages for managing background music

│   │   ├── Index.cshtml     # List all music tracks

│   │   ├── Index.cshtml.cs  # Logic for listing music

│   │   ├── Create.cshtml    # Form to upload new music

│   │   ├── Create.cshtml.cs # Logic for uploading music

│   │   ├── Edit.cshtml      # Form to edit music details

│   │   ├── Edit.cshtml.cs   # Logic for editing music

│   │   └── Delete.cshtml    # Confirmation page for deletion

│   │       └── Delete.cshtml.cs # Logic for deleting music

│   ├── MainScreen/            # Main Pomodoro timer page

│   │   ├── Index.cshtml     # Pomodoro interface

│   │   └── Index.cshtml.cs  # Logic for timer and selections

│   ├── Shared/              # Shared components like layout

│   │   └── _Layout.cshtml   # Common layout for all pages

│   └── _ViewStart.cshtml    # Sets default layout for pages

│

├── uploads/

│   ├── images/

│   ├── sound/

│   └── music/

│

├── Models/                  # Data model classes

│   ├── Workspace.cs         # Represents a workspace in the database

│   └── Music.cs             # Represents a music track in the database

│

├── wwwroot/                 # Static files (CSS, JS, images, audio)

│   ├── css/                 # CSS files

│   │   └── site.css         # Main stylesheet

│   ├── js/                  # JavaScript files

│   │   └── site.js          # Main client-side script

│   ├── images/              # Image files

│   │   └── workspaces/      # Background images for workspaces

│   └── audio/               # Audio files

│       └── music/           # Background music tracks

│

├── appsettings.json         # Application configuration (e.g., database connection string)

│

├── Program.cs               # Application entry point

│

└── StudyWorkspace.csproj    # Project file with NuGet packages and settings
