// Ref: https://github.com/domaindrivendev/Swashbuckle.AspNetCore?tab=readme-ov-file#omit-actions-by-convention

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTS_BE.Helper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OpenApi : Attribute
    {
        
    }
}