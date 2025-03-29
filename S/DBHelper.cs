using LiteDB;

namespace DnDPartyManager.S;

public static class DBHelper
{
    public static LiteDatabase DB = new LiteDatabase(@"data.db");
}