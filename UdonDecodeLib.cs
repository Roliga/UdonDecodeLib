using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System;

public class UdonDecodeLib : UdonSharpBehaviour
{
    public float[] DecodeHexColor(string hex)
    {
        if (hex == null)
            return null;

        if (hex.StartsWith("#"))
            hex = hex.Substring(1);

        if (hex.Length < 6 || !IsHexString(hex))
            return null;

        float r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
        float g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
        float b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 255f;

        float a = 1;
        if(hex.Length >= 8)
            a = int.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) / 255f;

        return new float[] { r, g, b, a };
    }

    public string DecodeByteArray(byte[] bytes)
    {
        char[] c = new char[bytes.Length];
        for (int i = 0; i < bytes.Length; i++)
            c[i] = Convert.ToChar(bytes[i]);
        return (new string(c)).Trim('\0');
    }

    public string DecodeBase64String(string data)
    {
        if (IsBase64String(data))
            return DecodeByteArray(Convert.FromBase64String(data));
        else
            return null;
    }

    public bool IsHexString(string str)
    {
        foreach (char c in str)
        {
            if(!((c >= '0' && c <= '9') ||
                     (c >= 'a' && c <= 'f') ||
                     (c >= 'A' && c <= 'F')))
                return false;
        }
        return true;
    }

    public bool IsBase64String(string str)
    {
        if (str == null || str.Length == 0 || str.Length % 4 != 0
            || str.Contains(" ") || str.Contains("\t") || str.Contains("\r") || str.Contains("\n"))
            return false;
        var index = str.Length - 1;
        if (str[index] == '=')
            index--;
        if (str[index] == '=')
            index--;
        for (var i = 0; i <= index; i++)
            if (IsBase64Char(str[i]))
                return false;
        return true;
    }

    private bool IsBase64Char(char value)
    {
        var intValue = (int)value;
        if (intValue >= 48 && intValue <= 57)
            return false;
        if (intValue >= 65 && intValue <= 90)
            return false;
        if (intValue >= 97 && intValue <= 122)
            return false;
        return intValue != 43 && intValue != 47;
    }
}
