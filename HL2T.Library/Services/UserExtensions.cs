using System.Security.Claims;

namespace HL2T.Library.Services;

public static class UserExtensions
{
    public static int LayMaNguoiDung(this ClaimsPrincipal user) =>
        int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : 0;

    public static string LayHoTen(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.GivenName) ?? user.Identity?.Name ?? "";
}
