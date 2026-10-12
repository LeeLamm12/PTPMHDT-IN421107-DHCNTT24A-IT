namespace EduHub.Domain;

public enum MemberStatus { Active, Inactive }

public class Member
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string StudentCode { get; private set; }
    public DateTime JoinedAt { get; private set; }
    public MemberStatus Status { get; private set; }

    private Member(string name, string email, string studentCode)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        StudentCode = studentCode;
        JoinedAt = DateTime.Now;
        Status = MemberStatus.Active;
    }

    public static Member Create(string name, string email, string studentCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên trống");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Email sai");
        if (string.IsNullOrWhiteSpace(studentCode))
            throw new ArgumentException("Mã sinh viên trống");
        return new Member(name.Trim(), email.Trim().ToLowerInvariant(), studentCode.Trim());
    }

    public bool IsActive() => Status == MemberStatus.Active;

    public void Deactivate()
    {
        if (!IsActive())
            throw new InvalidOperationException("Thành viên đã ngừng hoạt động");
        Status = MemberStatus.Inactive;
    }

    public void Reactivate()
    {
        if (IsActive())
            throw new InvalidOperationException("Thành viên đã hoạt động");
        Status = MemberStatus.Active;
    }

    public override string ToString() => $"{Name} ({StudentCode})";
}