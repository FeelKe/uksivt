using System;
using System.Collections.Generic;

namespace WpfApp17.Model;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Token { get; set; } = null!;

    public string Role { get; set; } = null!;
    public string Email { get; set; } = null!;

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
