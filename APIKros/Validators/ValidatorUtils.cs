
using System.Text.RegularExpressions;

namespace APIKros.Validators
{

    public static class ValidationUtils
    {
        private static readonly string[] AllowedTitles =
        {
        "Bc.", "Mgr.", "Ing.", "Ing. arch.", "MUDr.", "MVDr.",
        "JUDr.", "PhDr.", "RNDr.", "PaedDr.", "PharmDr.",
        "doc.", "prof.", "PhD.", "MBA"
    };

        public static async Task<bool> IsUniqueEmail(string email, CancellationToken cancellationToken, AppDbContext context)
        {
            return !await context.Employees
                .AnyAsync(e => e.Email == email, cancellationToken);
        }

        public static bool IsValidPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            return Regex.IsMatch(phone, @"^\+?[0-9\s\-\/]{7,30}$");
        }

        public static async Task<bool> EntityExists<TEntity>(
        AppDbContext context,
        int? id,
        CancellationToken cancellationToken)
        where TEntity : class
        {
            if (id is null)
                return false;

            return await context.Set<TEntity>()
                .AnyAsync(e => EF.Property<int>(e, "Id") == id.Value, cancellationToken);
        }

        public static bool IsAllowedTitle(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return true;

            return AllowedTitles.Contains(title);

        }
    }

}