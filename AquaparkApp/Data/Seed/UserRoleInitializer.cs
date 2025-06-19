// Plik: Data/Seed/UserRoleInitializer.cs
using Microsoft.AspNetCore.Identity;
using AquaparkApp.Data;

public static class UserRoleInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // --- 1. Definicja Ról ---
        string[] roleNames = { "Admin", "Pracownik", "Klient" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // --- 2. Definicja List Użytkowników do Przypisania Ról ---
        // Na tych listach podajesz tylko adresy email.
        // Użytkownicy MUSZĄ już istnieć w systemie (zarejestrowali się wcześniej).

        string[] adminEmails = { "admina@gmail.com" };
        string[] employeeEmails = { "pracownik@gmail.com", "recepcja@aquapark.com" };

        // --- 3. Przetwarzanie i Przypisywanie Ról ---

        // Przypisz rolę "Admin"
        await AssignRoleToUsers(userManager, adminEmails, "Admin");

        // Przypisz rolę "Pracownik"
        await AssignRoleToUsers(userManager, employeeEmails, "Pracownik");
    }

    // Metoda pomocnicza, która tylko przypisuje role do ISTNIEJĄCYCH użytkowników
    private static async Task AssignRoleToUsers(UserManager<ApplicationUser> userManager, string[] userEmails, string roleName)
    {
        foreach (var email in userEmails)
        {
            var user = await userManager.FindByEmailAsync(email);

            // Jeśli użytkownik o tym emailu istnieje i nie ma jeszcze tej roli...
            if (user != null && !await userManager.IsInRoleAsync(user, roleName))
            {
                // ...przypisz mu ją.
                await userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}