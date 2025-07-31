# Pomodoro Web App

Ứng dụng quản lý thời gian Pomodoro viết bằng ASP.NET Razor Pages.
Thành viên:
- HOÀNG LINH CHI – 2255020010
- NGUYỄN ANH ĐỨC – 2255020017
- ĐINH THỊ KHÁNH LINH – 2255020033
- NGUYỄN GIA HIẾU – 2255020024
- TRẦN KHÁNH LINH – 2255020037

## Hình ảnh
<img width="1919" height="1079" alt="image" src="https://github.com/user-attachments/assets/6bdca38b-83f6-4c32-938e-ace17a9ea98a" />
<img width="1897" height="927" alt="image" src="https://github.com/user-attachments/assets/816a432b-a519-4c13-b162-fa7a9cc135ed" />
<img width="1920" height="922" alt="image" src="https://github.com/user-attachments/assets/95810901-cd02-402f-b687-bd4fc664c6b4" />
<img width="1920" height="861" alt="image" src="https://github.com/user-attachments/assets/4fd5fb43-0bd6-45fc-867f-03632a139a14" />


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

 # Structure

![image](https://github.com/user-attachments/assets/f573a9de-a1e9-4b02-92e2-4848d30cef90)
