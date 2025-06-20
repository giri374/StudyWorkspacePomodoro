# Pomodoro Web App

Ứng dụng quản lý thời gian Pomodoro viết bằng ASP.NET Razor Pages.

## ⚙️ Cài đặt

1. Clone repo:
   ```bash
   git clone https://github.com/giri374/StudyWorkspacePomodoro.git
2. Tạo CSDL
 Mở giao diện SQL (VD: SSMS) của bạn, chạy SQLServerDB.sql trong new query để tạo database.
- Chạy query này để có Server Name:SELECT @@SERVERNAME;
- copy server name vửa rồi

## Cấu hình cơ sở dữ liệu (hạn chế tình trạng xung đột CSDL)
1. Copy file `appsettings.example.json` thành `appsettings.json`
2. Chỉnh sửa `ConnectionStrings` phù hợp với máy của bạn. Cần sửa User ID, Password
3. KHÔNG commit `appsettings.json` vào Git (đã được ignore) 
+ VD Connectionstring của Đinh Linh: "Data Source=localhost\\sqlexpress;Initial Catalog=music;Integrated Security=True;Pooling=False;TrustServerCertificate=True"
+ VD2: "Server=myServerAddress\sqlexpress;Database=PomodoroDB;Integrated Security=True;Pooling=False;TrustServerCertificate=True"
+ thay myServerAddress\sqlexpress bằng server name của bạn
  
## Connect database
làm theo hướng dẫn của thầy:
Trong cửa sổ Server Explorer chọn Connect to Database:
Nhập server name và chọn database:
![image](https://github.com/user-attachments/assets/784c38f9-fad6-4687-b113-18ea4747f7ed)


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
 Xem và sử dụng các không gian, nhạc có sẵn trong trang web.
Giao diện
(A) 2 nút chọn nhạc và chọn workspace, nhấn vào sẽ xổ ra thông tin: Sau khi lựa chọn nhạc, nhạc đang phát sẽ thay đổi (nhạc chỉ phát khi đang trong thời gian Pomodoro). Sau khi lựa chọn không gian, ảnh nền ở (B) sẽ thay đổi.

(B)tên workspace và ảnh nền ở chính giữa màn hình (hiển thị workspace được chọn).

(C) Khu vực Pomodoro (hiển thị đè lên ảnh nền): StartPomodoro: bắt đầu thời gian Pomodoro (nút Start), nhận chuông báo sau 25 phút, xem thời gian pomodoro còn lại (ô số 25:00 đếm ngược); ShortBreak: tương tự StartPomodoro, nhận chuông báo sau 5 phút (hiển thị thời gian nghỉ còn lại) (giao diện ShortBreak sẽ thay thế giao diện StartPomodoro); LongBreak: tương tự ShortBreak, nhận chuông báo sau 15 phút."

 # Structure

![image](https://github.com/user-attachments/assets/f573a9de-a1e9-4b02-92e2-4848d30cef90)
