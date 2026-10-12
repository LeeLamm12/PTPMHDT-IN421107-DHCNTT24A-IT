using EduHub.Domain;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var clubs = new List<Club>();
var events = new List<Event>();
var regs = new List<Registration>();
var checkIns = new List<CheckIn>();

// ---------- Hàm trợ giúp ----------
static void Thu(string moTa, Action hanhDong)
{
    try
    {
        hanhDong();
        Console.WriteLine($"  [!] {moTa}: KHÔNG bị chặn (sai luật!)");
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine($"  [OK] {moTa} -> bị chặn: {ex.Message}");
    }
}

// Số chỗ đã dùng = số phiếu chưa Cancelled (US-07)
int DemChoDaDung(Event sk) =>
    regs.Count(r => r.EventId == sk.Id && r.Status != RegistrationStatus.Cancelled);

// US-02 (slide) + kiểm tra còn chỗ trước khi đăng ký
Registration? DangKy(Member tv, Event sk)
{
    int dem = DemChoDaDung(sk);
    if (!sk.HasCapacity(dem))
    {
        Console.WriteLine($"  {tv.Name}: sự kiện đã hết chỗ ({dem}/{sk.Capacity})");
        return null;
    }
    var phieu = Registration.Create(tv, sk);
    regs.Add(phieu);
    Console.WriteLine($"  {tv.Name}: {phieu}");
    return phieu;
}

// US-10: xem danh sách thành viên của CLB
void XemThanhVien(Club c)
{
    Console.WriteLine($"  {c.Name}: {c.MemberCount()} thành viên");
    if (c.MemberCount() == 0) Console.WriteLine("    (danh sách rỗng)");
    foreach (var tv in c.Members) Console.WriteLine($"    - {tv}");
}

// US-08: lịch sự kiện sắp tới của các CLB mình tham gia
void XemLich(Member tv)
{
    var clubIds = clubs.Where(c => c.Members.Any(x => x.Id == tv.Id))
                       .Select(c => c.Id).ToList();
    if (clubIds.Count == 0)
    {
        Console.WriteLine($"  {tv.Name}: Bạn chưa tham gia CLB nào");
        return;
    }
    var sapToi = events.Where(sk => clubIds.Contains(sk.ClubId) && sk.Date > DateTime.Now)
                       .OrderBy(sk => sk.Date).ToList();
    Console.WriteLine($"  {tv.Name} thấy {sapToi.Count} sự kiện sắp tới (gần nhất trước):");
    foreach (var sk in sapToi) Console.WriteLine($"    - {sk.Date:dd/MM/yyyy} · {sk.Title}");
}

// US-12: lịch sử đăng ký và điểm danh của mình
void XemLichSu(Member tv)
{
    var cuaToi = regs.Where(r => r.MemberId == tv.Id).ToList();
    if (cuaToi.Count == 0)
    {
        Console.WriteLine($"  {tv.Name}: chưa đăng ký sự kiện nào (danh sách rỗng)");
        return;
    }
    Console.WriteLine($"  Lịch sử của {tv.Name}:");
    foreach (var r in cuaToi)
    {
        var sk = events.First(x => x.Id == r.EventId);
        bool daCheckIn = checkIns.Any(c => c.RegistrationId == r.Id);
        Console.WriteLine($"    - {sk.Title}: {r.Status}, {(daCheckIn ? "đã check-in" : "chưa check-in")}");
    }
}

// ===== 1. Tạo CLB =====
Console.WriteLine("== 1. Tạo CLB ==");
var club = Club.Create("CLB Lập trình", "Sinh hoạt hằng tuần");
var club2 = Club.Create("CLB Âm nhạc");
clubs.AddRange(new[] { club, club2 });
Console.WriteLine($"Đã tạo: {club}, {club2}");

// ===== 2. Thêm 3 thành viên (US-10) =====
Console.WriteLine("\n== 2. Thêm thành viên ==");
var m1 = Member.Create("An", "an@dthu.edu.vn", "SV001");
var m2 = Member.Create("Bình", "binh@dthu.edu.vn", "SV002");
var m3 = Member.Create("Chi", "chi@dthu.edu.vn", "SV003");
var m4 = Member.Create("Dũng", "dung@dthu.edu.vn", "SV004");   // chưa tham gia CLB nào
foreach (var tv in new[] { m1, m2, m3 }) club.AddMember(tv);
var tatCa = new[] { m1, m2, m3, m4 };
Console.WriteLine("US-10 · Ban CLB xem danh sách thành viên:");
XemThanhVien(club);
XemThanhVien(club2);

// ===== 3. Tạo sự kiện (US-08) =====
Console.WriteLine("\n== 3. Tạo sự kiện ==");
var ev = Event.Create(club.Id, "Workshop Git", DateTime.Now.AddDays(7), 2);
var ev2 = Event.Create(club.Id, "Seminar .NET", DateTime.Now.AddDays(3), 50, true);
var evCu = Event.Create(club.Id, "Gặp mặt đầu năm", DateTime.Now.AddDays(-10), 20);
events.AddRange(new[] { ev, ev2, evCu });
foreach (var sk in events) Console.WriteLine($"  {sk.Title}: sức chứa {sk.Capacity}");
Console.WriteLine("US-08 · Sinh viên xem lịch sự kiện sắp tới:");
XemLich(m1);
XemLich(m4);

// ===== 4. Đăng ký (Pending) =====
Console.WriteLine("\n== 4. Đăng ký ==");
var r1 = DangKy(m1, ev)!;
var r2 = DangKy(m2, ev)!;
DangKy(m3, ev);   // sự kiện sức chứa 2 nên Chi bị báo hết chỗ

// ===== 5. Xác nhận (US-09) =====
Console.WriteLine("\n== 5. Ban CLB xác nhận phiếu ==");
r1.Confirm();
r2.Confirm();
Console.WriteLine($"  {r1}");
Console.WriteLine($"  {r2}");

// ===== 6. Check-in qua QR =====
Console.WriteLine("\n== 6. Check-in ==");
var ci = CheckIn.Create(r1, CheckInMethod.QR);   // chỉ An check-in, Bình chưa
checkIns.Add(ci);
Console.WriteLine("  " + ci.Summary());

// ===== 7. Báo cáo tham dự (US-12) =====
Console.WriteLine("\n== 7. Báo cáo tham dự ==");
int daDung = DemChoDaDung(ev);
Console.WriteLine($"Đăng ký: {daDung}/{ev.Capacity} | Còn chỗ: {(ev.HasCapacity(daDung) ? "Có" : "Không")}");
Console.WriteLine($"Đã check-in: {checkIns.Count}");
foreach (var p in regs.Where(x => x.EventId == ev.Id && x.Status != RegistrationStatus.Cancelled))
{
    var tv = tatCa.First(x => x.Id == p.MemberId);
    bool daCheckIn = checkIns.Any(c => c.RegistrationId == p.Id);
    Console.WriteLine($"  - {tv}: {(daCheckIn ? "đã check-in" : "chưa check-in")}");
}
Console.WriteLine("US-12 · Sinh viên xem lịch sử của mình:");
XemLichSu(m1);
XemLichSu(m4);

// ===== 8. Hủy, khôi phục và cố tình vi phạm luật =====
Console.WriteLine("\n== 8. Vi phạm luật nghiệp vụ ==");

Console.WriteLine("-- US-07: hủy đăng ký --");
r2.Cancel();
int daDung2 = DemChoDaDung(ev);
Console.WriteLine($"  Bình hủy: {r2} | chỗ đã dùng {daDung2}/{ev.Capacity}, còn chỗ: {(ev.HasCapacity(daDung2) ? "Có" : "Không")}");
var r3 = DangKy(m3, ev)!;   // Chi đăng ký được vào chỗ Bình nhường
Thu("Hủy lại phiếu đã Cancelled", () => r2.Cancel());

Console.WriteLine("-- US-09: xác nhận sai trạng thái --");
Thu("Xác nhận phiếu đã Cancelled", () => r2.Confirm());
Thu("Xác nhận phiếu đã Confirmed", () => r1.Confirm());
Console.WriteLine($"  Trạng thái giữ nguyên: {r1}, {r2}");

Console.WriteLine("-- US-11: ngừng và khôi phục thành viên --");
m2.Deactivate();
Console.WriteLine($"  Đã ngừng {m2}: IsActive = {m2.IsActive()}");
Thu("Thành viên đã ngừng đăng ký sự kiện", () => Registration.Create(m2, ev2));
m2.Reactivate();
Console.WriteLine($"  Đã khôi phục {m2}: IsActive = {m2.IsActive()}");
Thu("Khôi phục thành viên đang Active", () => m2.Reactivate());
XemLichSu(m2);   // lịch sử đăng ký vẫn còn

Console.WriteLine("-- Các luật khác --");
Thu("Thêm thành viên trùng vào CLB", () => club.AddMember(m1));
Thu("Tạo sự kiện capacity = 0", () => Event.Create(club.Id, "Lỗi", DateTime.Now, 0));
Thu("Tạo CLB tên trống", () => Club.Create("   "));
Thu("Check-in phiếu Pending", () => CheckIn.Create(r3, CheckInMethod.QR));