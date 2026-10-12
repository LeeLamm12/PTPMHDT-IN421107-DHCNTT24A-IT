namespace EduHub.Domain;

public class Club
{
    private readonly List<Member> _members = new();

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyList<Member> Members => _members;   // chỉ xem, muốn thêm phải qua AddMember()

    private Club(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        CreatedAt = DateTime.Now;
    }

    public static Club Create(string name, string description = "")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên CLB trống");
        return new Club(name.Trim(), description?.Trim() ?? "");
    }

    public void AddMember(Member member)
    {
        ArgumentNullException.ThrowIfNull(member);
        if (_members.Any(m => m.Id == member.Id))
            throw new InvalidOperationException($"{member} đã là thành viên");
        _members.Add(member);
    }

    public int MemberCount() => _members.Count;

    public override string ToString() => Name;
}