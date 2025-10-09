== Mô tả ==

GuardingPearSoftwares Obfuscator được phát triển để tăng cường bảo mật phần mềm
và trò chơi của bạn, đặc biệt là cho các ứng dụng được xây dựng bằng Unity.

Mục tiêu chính của nó là che giấu mã nguồn độc quyền của riêng bạn, cũng như
các assembly dotNet được biên dịch bởi bên thứ ba. Chúng tôi hỗ trợ tất cả các nền tảng đã biết,
dù là độc lập hay nhúng.

== Cách thực hiện ==

Khi được kích hoạt (mặc định), Obfuscator sẽ tự động chạy tại
thời điểm biên dịch. Ngay sau khi Unity tạo các assembly cụ thể cho mục tiêu biên dịch (*.dll), được tạo tại Library\ScriptAssemblies, Obfuscator
sẽ được áp dụng cho chúng.

== Cài đặt ==

Obfuscator đi kèm với cửa sổ "Cài đặt" cho phép chỉ định chính xác và
thân thiện với người dùng những assembly nào cần được obfuscator và những tính năng nào nên áp dụng
cho chúng. Bạn có thể tìm thấy nó trong Menu Unity Editor OPS->Obfuscator->Settings.

== Error Stack Trace ==

Để vẫn có thể gỡ lỗi hoặc hiểu được các nhật ký lỗi đã được mã hóa, Obfuscator
đi kèm với cửa sổ "Error Stack Trace". Tại đây, bạn có thể tải tệp ánh xạ (bạn
phải kích hoạt nó trong phần cài đặt) và nhập một stack trace đã được mã hóa.
Nhấn "Deobfuscate" (Giải mã), obfuscator sẽ cố gắng giải mã stack trace.
Bạn có thể tìm thấy nó trong Menu Unity Editor: OPS->Obfuscator->Error Stack Trace.