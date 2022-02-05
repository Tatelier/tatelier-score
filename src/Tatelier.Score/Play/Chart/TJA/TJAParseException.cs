using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Tatelier.Score.Play.Chart.TJA
{
    internal class TJAParseException
        : Exception
    {
        public TJAParseException()
        {
        }

        public TJAParseException(string message) : base(message)
        {
        }

        public TJAParseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TJAParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
