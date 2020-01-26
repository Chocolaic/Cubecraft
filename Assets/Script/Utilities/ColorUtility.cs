class ColorUtility
{
    public const string Black = "<color=#000000>{0}</color>"; //黑色
    public const string DarkBlue = "<color=#0000AA>{0}</color>";
    public const string DarkGreen = "<color=#00AA00>{0}</color>";
    public const string DarkAqua = "<color=#00AAAA>{0}</color>";
    public const string DarkRed = "<color=#AA0000>{0}</color>";
    public const string DarkPurple = "<color=#AA00AA>{0}</color>";
    public const string Gold = "<color=#FFAA00>{0}</color>";
    public const string Gray = "<color=#AAAAAA>{0}</color>";
    public const string DarkGray = "<color=#555555>{0}</color>";
    public const string Blue = "<color=#5555FF>{0}</color>";
    public const string Green = "<color=#55FF55>{0}</color>";
    public const string Aqua = "<color=#55FFFF>{0}</color>";
    public const string Red = "<color=#FF5555>{0}</color>";
    public const string LightPurple = "<color=#FF55FF>{0}</color>";
    public const string Yellow = "<color=#FFFF55>{0}</color>";
    public const string White = "<color=#FFFFFF>{0}</color>";

    public static string Set(string code, string text)
    {
        return string.IsNullOrEmpty(code) ? text : string.Format(code, text);
    }
}
