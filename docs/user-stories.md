# User Stories – EduHub

### US-07 · Sinh viên: hủy đăng ký một sự kiện
Là sinh viên, tôi muốn hủy đăng ký khi bận đột xuất, để nhường chỗ cho bạn khác.

1. Given phiếu đang Pending hoặc Confirmed, When tôi hủy, Then phiếu chuyển sang Cancelled và không còn tính vào số chỗ đã dùng.
2. Given phiếu đã Cancelled, When tôi hủy lại, Then bị từ chối.

### US-08 · Sinh viên: xem lịch sự kiện sắp tới của các CLB mình tham gia
Là sinh viên, tôi muốn xem danh sách sự kiện sắp diễn ra, để không bỏ lỡ hoạt động của CLB.

1. Given tôi là thành viên ít nhất một CLB và CLB có sự kiện chưa diễn ra, When tôi mở lịch, Then thấy các sự kiện đó sắp xếp theo ngày tăng dần (gần nhất trước).
2. Given tôi chưa tham gia CLB nào, When tôi mở lịch, Then danh sách rỗng kèm thông báo "Bạn chưa tham gia CLB nào".

### US-09 · Ban CLB: xác nhận phiếu đăng ký của sinh viên
Là Ban CLB, tôi muốn xác nhận các phiếu đang chờ, để chốt danh sách người tham dự.

1. Given phiếu đang Pending, When tôi xác nhận, Then phiếu chuyển sang Confirmed.
2. Given phiếu đã Cancelled hoặc đã Confirmed, When tôi xác nhận, Then bị từ chối và trạng thái phiếu giữ nguyên.

### US-10 · Ban CLB: xem danh sách thành viên của CLB
Là Ban CLB, tôi muốn xem ai đang là thành viên, để nắm quy mô và liên hệ khi cần.

1. Given CLB có thành viên, When tôi mở danh sách, Then thấy từng thành viên và tổng số thành viên.
2. Given CLB chưa có thành viên, When tôi mở danh sách, Then danh sách rỗng và tổng số là 0.

### US-11 · Admin: khôi phục một thành viên đã bị ngừng
Là Admin, tôi muốn kích hoạt lại thành viên bị ngừng nhầm, để họ tiếp tục dùng hệ thống.

1. Given thành viên đang Inactive, When tôi khôi phục, Then IsActive() = true và lịch sử đăng ký vẫn được giữ.
2. Given thành viên đang Active, When tôi khôi phục, Then báo "đã hoạt động" và không có thay đổi nào.

### US-12 · Sinh viên: xem lại lịch sử đăng ký và điểm danh của mình
Là sinh viên, tôi muốn xem các sự kiện mình đã đăng ký, để biết mình đã được xác nhận và đã điểm danh chưa.

1. Given tôi có phiếu đăng ký, When tôi mở lịch sử, Then thấy từng phiếu cùng trạng thái (Pending, Confirmed, Cancelled) và đã check-in hay chưa.
2. Given tôi chưa đăng ký sự kiện nào, When tôi mở lịch sử, Then danh sách rỗng.