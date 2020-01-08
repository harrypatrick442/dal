using System;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
namespace WPFCore
{
    public interface IGetMessageSender
    {
        MessageSender MessageSender { get; }
    }
}
