using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Common.Validation
{
    public interface IUser
    {
        public string Id { get; }
        public string Username { get; }
        public string Role { get; }
    }
}
