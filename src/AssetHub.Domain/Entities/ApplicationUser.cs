namespace AssetHub.Domain.Entities;
public class ApplicationUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; } = string.Empty;
    //Upper-invariant form used for unique index and for lookups
    public string NormalizedUserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NormalizedEmail { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public string? PasswordHash { get; set; }
    //Rotated on credential change; lets us invliadate outstanding tokens.
    public string SecurityStamp { get; set; } = Guid.NewGuid().ToString("N");
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString("N");
    public string FullName { get; set; } = string.Empty;
    public string? Department { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; } = true;
    public int AccessFailedCount { get; set; } = 0;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginUtc { get; set; }
}

//A named role. Authroization policies are built from these
public class ApplicationRole
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString("N");
    public string? Description { get; set; }
}

//A hashed, single-use refresh token, so a database leak does not hand out usable tokens; useful for revoking access to a single device without affecting others.
public class RefreshToken
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresUtc { get; set; }
    public DateTime CreatedUtc { get; set; }
    public string? CreatedbyIp { get; set; }
    public DateTime? RevokedUtc { get; set; }
    public string? ReplacedByTo { get; set; }
    public bool IsActive(DateTime nowUtc) => RevokedUtc is null && ExpiresUtc > nowUtc; 
}
