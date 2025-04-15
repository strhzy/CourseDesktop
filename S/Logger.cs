using System;
using DnDPartyManager.M;
using LiteDB;

namespace DnDPartyManager.S;

public static class Logger
{
    static ILiteCollection<Log> col = DBHelper.DB.GetCollection<Log>("logs");
    public static void Log(string tag, string message)
    {
        col.Insert(new Log(tag, message, DateTime.Now));
    }
}