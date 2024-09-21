using System;
using System.Collections.Generic;

namespace WpfApp17.Model;

public partial class Purchase
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public virtual User? User { get; set; }
}
