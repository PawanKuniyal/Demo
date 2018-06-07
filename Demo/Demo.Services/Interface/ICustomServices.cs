using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpa.Entities;

namespace Helpa.Services.Interface
{
    public interface ICustomServices
    {
        AspNetUser LoginUserByEmail(string Email, string Password);
    }
}
