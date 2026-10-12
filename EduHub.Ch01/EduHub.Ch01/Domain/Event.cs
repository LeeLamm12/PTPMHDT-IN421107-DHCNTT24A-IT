namespace EduHub.Domain;

public class Event
{
    public Guid Id { get; private set; }
    public Guid ClubId { get; private set; }          // khóa ngoại trỏ về Club
    public string Title { get; private set; }
    public DateTime Date { get; private set; }
    public int Capacity { get; private set; }
    public bool IsOnline { get; private set; }

    private Event(Guid clubId, string title, DateTime date, int capacity, bool isOnline)
    {
        Id = Guid.NewGuid();
        ClubId = clubId;
        Title = title;
        Date = date;
        Capacity = capacity;
        IsOnline = isOnline;
    }

    public static Event Create(Guid clubId, string title, DateTime date, int capacity, bool isOnline = false)
    {
        if (clubId == Guid.Empty)
            throw new ArgumentException("ClubId không hợp lệ");
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tiêu đề trống");
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Sức chứa phải lớn hơn 0");
        return new Event(clubId, title.Trim(), date, capacity, isOnline);
    }

    // Còn chỗ nếu số phiếu đang tính (chưa Cancelled) còn nhỏ hơn sức chứa
    public bool HasCapacity(int registeredCount)
    {
        if (registeredCount < 0)
            throw new ArgumentOutOfRangeException(nameof(registeredCount));
        return registeredCount < Capacity;
    }

    public override string ToString() => Title;
}