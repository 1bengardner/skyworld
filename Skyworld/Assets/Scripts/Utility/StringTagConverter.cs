using System.Collections;
using System.Collections.Generic;
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
}
