using System;
using DnDPartyManager.S;
using LiteDB;

namespace DnDPartyManager.M;

public class Log
{
    public string Tag { get; set; }
    public string Message { get; set; }
    public DateTime Date { get; set; }

    public Log(string tag, string message, DateTime date)
    {
        Tag = tag;
        Message = message;
        Date = date;
    }
}