namespace EduHub.Domain;

public static class CheckInMethod
{
    public const string Manual = "Manual";
    public const string QR = "QR";
}

public class CheckIn
{
    public Guid Id { get; private set; }
    public Guid RegistrationId { get; private set; }   // khóa ngoại trỏ về Registration
    public DateTime CheckedInAt { get; private set; }
    public string Method { get; private set; }

    private CheckIn(Guid registrationId, string method)
    {
        Id = Guid.NewGuid();
        RegistrationId = registrationId;
        CheckedInAt = DateTime.Now;
        Method = method;
    }

    public static CheckIn Create(Registration registration, string method = CheckInMethod.Manual)
    {
        ArgumentNullException.ThrowIfNull(registration);
        if (string.IsNullOrWhiteSpace(method))
            throw new ArgumentException("Phương thức check-in trống");
        if (registration.Status != RegistrationStatus.Confirmed)
            throw new InvalidOperationException("Chỉ phiếu Confirmed mới check-in");
        return new CheckIn(registration.Id, method.Trim());
    }

    public string Summary() =>
        $"Phiếu {RegistrationId.ToString()[..8]} check-in lúc {CheckedInAt:HH:mm dd/MM/yyyy} ({Method})";
}