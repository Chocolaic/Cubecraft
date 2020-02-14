class ColorUtility
{
    public const string Black = "000000"; //黑色
    public const string DarkBlue = "0000AA";
    public const string DarkGreen = "00AA00";
    public const string DarkAqua = "00AAAA";
    public const string DarkRed = "AA0000";
    public const string DarkPurple = "AA00AA";
    public const string Gold = "FFAA00";
    public const string Gray = "AAAAAA";
    public const string DarkGray = "555555";
    public const string Blue = "5555FF";
    public const string Green = "55FF55";
    public const string Aqua = "55FFFF";
    public const string Red = "FF5555";
    public const string LightPurple = "FF55FF";
    public const string Yellow = "FFFF55";
    public const string White = "FFFFFF";

    public static string Set(string code, object text)
    {
        return string.IsNullOrEmpty(code) ? text.ToString() : string.Format("<color=#{0}>{1}</color>", code, text.ToString());
    }
}
