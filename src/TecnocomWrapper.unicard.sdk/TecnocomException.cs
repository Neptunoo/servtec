using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecnocomWrapper.unicard.sdk
{
    [Serializable]
    public class TecnocomException : Exception
    {
        public TecnocomException(string name)
            : base(name)
        {

        }
    }
}
