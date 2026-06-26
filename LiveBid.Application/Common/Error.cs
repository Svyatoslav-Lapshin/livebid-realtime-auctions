using System;
using System.Collections.Generic;
using System.Text;

namespace LiveBid.Application.Common
{
    public sealed record class Error(string Code, string Message)
    {
       public static readonly Error None = new(string.Empty,string.Empty);
    }
}
