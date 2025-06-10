using System;
using System.Collections.Generic;

namespace AquaparkApp.Server;

public partial class Employee
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Role { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
