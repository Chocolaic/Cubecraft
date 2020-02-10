using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cubecraft.Utilities
{
    /// <summary>
    /// This class parses JSON chat data from MC 1.6+ and returns the appropriate string to be printed.
    /// </summary>

    static class ChatParser
    {
        /// <summary>
        /// The main function to convert text from MC 1.6+ JSON to MC 1.5.2 formatted text
        /// </summary>
        /// <param name="json">JSON serialized text</param>
        /// <param name="links">Optional container for links from JSON serialized text</param>
        /// <returns>Returns the translated text</returns>
        public static string ParseText(string json, List<string> links = null)
        {
            return JSONData2String(Json.ParseJson(json), "", links);
        }

        /// <summary>
        /// Get the classic color tag corresponding to a color name
        /// </summary>
        /// <param name="colorname">Color Name</param>
        /// <returns>Color code</returns>
        private static string Color2tag(string colorname)
        {
            switch (colorname.ToLower())
            {
                /* MC 1.7+ Name           MC 1.6 Name           Classic tag */
                case "black":        /*  Blank if same  */      return ColorUtility.Black;
                case "dark_blue":                               return ColorUtility.DarkBlue;
                case "dark_green":                              return ColorUtility.DarkGreen;
                case "dark_aqua":       case "dark_cyan":       return ColorUtility.DarkAqua;
                case "dark_red":                                return ColorUtility.DarkRed;
                case "dark_purple":     case "dark_magenta":    return ColorUtility.DarkPurple;
                case "gold":            case "dark_yellow":     return ColorUtility.Gold;
                case "gray":                                    return ColorUtility.Gray;
                case "dark_gray":                               return ColorUtility.DarkGray;
                case "blue":                                    return ColorUtility.Blue;
                case "green":                                   return ColorUtility.Green;
                case "aqua":            case "cyan":            return ColorUtility.Aqua;
                case "red":                                     return ColorUtility.Red;
                case "light_purple":    case "magenta":         return ColorUtility.LightPurple;
                case "yellow":                                  return ColorUtility.Yellow;
                case "white":                                   return ColorUtility.White;
                default: return "";
            }
        }

        /// <summary>
        /// Use a JSON Object to build the corresponding string
        /// </summary>
        /// <param name="data">JSON object to convert</param>
        /// <param name="colorcode">Allow parent color code to affect child elements (set to "" for function init)</param>
        /// <param name="links">Container for links from JSON serialized text</param>
        /// <returns>returns the Minecraft-formatted string</returns>
        private static string JSONData2String(Json.JSONData data, string colorcode, List<string> links)
        {
            string extra_result = "";
            switch (data.Type)
            {
                case Json.JSONData.DataType.Object:
                    if (data.Properties.ContainsKey("color"))
                    {
                        colorcode = Color2tag(JSONData2String(data.Properties["color"], "", links));
                    }
                    if (data.Properties.ContainsKey("clickEvent") && links != null)
                    {
                        Json.JSONData clickEvent = data.Properties["clickEvent"];
                        if (clickEvent.Properties.ContainsKey("action")
                            && clickEvent.Properties.ContainsKey("value")
                            && clickEvent.Properties["action"].StringValue == "open_url"
                            && !String.IsNullOrEmpty(clickEvent.Properties["value"].StringValue))
                        {
                            links.Add(clickEvent.Properties["value"].StringValue);
                        }
                     }
                    if (data.Properties.ContainsKey("extra"))
                    {
                        Json.JSONData[] extras = data.Properties["extra"].DataArray.ToArray();
                        foreach (Json.JSONData item in extras)
                            extra_result = extra_result + JSONData2String(item, colorcode, links);// + "§r";
                    }
                    if (data.Properties.ContainsKey("text"))
                    {
                        return ColorUtility.Set(colorcode, JSONData2String(data.Properties["text"], colorcode, links) + extra_result);
                    }
                    else if (data.Properties.ContainsKey("translate"))
                    {
                        return data.Properties["translate"].StringValue;
                    }
                    else return extra_result;

                case Json.JSONData.DataType.Array:
                    string result = "";
                    foreach (Json.JSONData item in data.DataArray)
                    {
                        result += JSONData2String(item, colorcode, links);
                    }
                    return result;

                case Json.JSONData.DataType.String:
                    return ColorUtility.Set(colorcode, data.StringValue);
            }

            return "";
        }
    }
}
