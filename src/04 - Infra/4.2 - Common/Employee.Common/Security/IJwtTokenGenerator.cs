using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Common.Validation
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(IUser user);
    }
}
