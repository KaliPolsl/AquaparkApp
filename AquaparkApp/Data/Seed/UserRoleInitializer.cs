// Plik: Data/Seed/UserRoleInitializer.cs
using Microsoft.AspNetCore.Identity;
using AquaparkApp.Data;

public static class UserRoleInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // --- Tworzenie Ról ---
        string[] roleNames = { "Admin", "Pracownik", "Klient" };
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                // Stwórz rolę, jeśli nie istnieje
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // --- Przypisywanie roli Admina pierwszemu użytkownikowi ---
        // Zmień adres email na swój własny, którego użyłeś do rejestracji
        var adminEmail = "admin@gmail.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser != null)
        {
            // Sprawdź, czy użytkownik nie ma już roli Admin
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                // Przypisz rolę Admin
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}