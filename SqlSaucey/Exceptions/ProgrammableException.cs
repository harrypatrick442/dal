using System.Xml;
using System.Xml.Linq;
using SqlSaucey.Extensions;
using System.IO;
using SqlSaucey.Strings;
using SqlSaucey.Core;
using System;
namespace SqlSaucey.Exceptions
{
    public class ProgrammableException : Exception
    {
        public ProgrammableException(string message) : base(message)
        {

        }
        public ProgrammableException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
