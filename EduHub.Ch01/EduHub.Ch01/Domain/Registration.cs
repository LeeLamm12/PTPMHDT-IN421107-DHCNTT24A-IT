namespace EduHub.Domain;

public enum RegistrationStatus { Pending, Confirmed, Cancelled }

public class Registration
{
    public Guid Id { get; private set; }
    public Guid MemberId { get; private set; }        // khóa ngoại trỏ về Member
    public Guid EventId { get; private set; }         // khóa ngoại trỏ về Event
    public DateTime RegisteredAt { get; private set; }
    public RegistrationStatus Status { get; private set; }

    private Registration(Guid memberId, Guid eventId)
    {
        Id = Guid.NewGuid();
        MemberId = memberId;
        EventId = eventId;
        RegisteredAt = DateTime.Now;
        Status = RegistrationStatus.Pending;
    }

    public static Registration Create(Member member, Event ev)
    {
        ArgumentNullException.ThrowIfNull(member);
        ArgumentNullException.ThrowIfNull(ev);
        if (!member.IsActive())
            throw new InvalidOperationException($"{member} đã ngừng hoạt động, không thể đăng ký");
        return new Registration(member.Id, ev.Id);
    }

    // Chỉ phiếu Pending mới được xác nhận
    public void Confirm()
    {
        if (Status != RegistrationStatus.Pending)
            throw new InvalidOperationException($"Không thể xác nhận phiếu đang ở trạng thái {Status}.");
        Status = RegistrationStatus.Confirmed;
    }

    // Pending hoặc Confirmed đều hủy được; Cancelled là trạng thái cuối
    public void Cancel()
    {
        if (Status == RegistrationStatus.Cancelled)
            throw new InvalidOperationException("Phiếu đã bị hủy trước đó.");
        Status = RegistrationStatus.Cancelled;
    }

    public override string ToString() => $"Phiếu {Id.ToString()[..8]} ({Status})";
}