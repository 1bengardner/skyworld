using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// A utility
public static class StringTagConverter
{
    // Return the string representation of a dialogue tag
    public static string ConvertTag(string tag)
    {
        switch (tag.ToLower())
        {
            case "[name]":
                return GameManager.Instance.fileName;
            case "[nickname]":
                return GameManager.Instance.fileName == GameManager.defaultName ? GameManager.defaultName.Substring(0, 3) : GameManager.Instance.fileName;
            case "[space parts left]":
                return (GameManager.totalParts - GameManager.Instance.partsFound).ToString();
            default:
                Debug.LogWarning("[NPCDialogue] Dialogue tag does not exist.");
                return "[Unknown Tag]";
        }
    }

    public static string ConvertString(string s)
    {
        MatchCollection matches = Regex.Matches(s, "\\[.+\\]");

        foreach (Match match in matches)
        {
            s = s.Replace(match.Value, ConvertTag(match.Value));
        }

        return s;
    }
}
