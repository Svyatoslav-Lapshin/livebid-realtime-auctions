using LiveBid.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Domain.Users
{
    public sealed class User:BaseEntity
    {
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; }=string.Empty;
        public string PasswordHash { get; set; }=string.Empty;

    }
}
