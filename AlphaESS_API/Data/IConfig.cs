using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaESS_API.Data
{
    public interface IConfig
    {
        Dictionary<string, string> ToArgs();
    }
}
