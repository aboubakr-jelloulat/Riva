using System;
using System.Collections.Generic;
using System.Text;

namespace Riva.DTO;

public class UserDTO
{
    public string Id { get; set; }

    public string Email { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Role { get; set; } = default!;

}
