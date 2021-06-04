using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class LMConvert
{
    public static byte[] ToByte(string name, string desc, float r, float g, float b)
    {
        byte[] bname = Encoding.Unicode.GetBytes(name);
        byte[] bdesc = Encoding.Unicode.GetBytes(desc);
        byte[] br = BitConverter.GetBytes((double)r);
        byte[] bg = BitConverter.GetBytes((double)g);
        byte[] bb = BitConverter.GetBytes((double)b);
        List<byte> data = new List<byte>();
        data.Add(Convert.ToByte(bname.Length));
        data.Add(Convert.ToByte(bdesc.Length));
        foreach (byte by in bname) data.Add(by);
        foreach (byte by in bdesc) data.Add(by);
        foreach (byte by in br) data.Add(by);
        foreach (byte by in bg) data.Add(by);
        foreach (byte by in bb) data.Add(by);
        return data.ToArray();
    }

    public static Player ToPlayer(byte[] data, ulong id)
    {
        int nameLen = Convert.ToInt32(data[0]) + 1;
        int descLen = Convert.ToInt32(data[1]) + 1;
        string name = "";
        for (int i = 0; i < nameLen; i += 2) name += Encoding.Unicode.GetChars(new byte[] { data[i + 2], data[i + 3] })[0];
        string desc = "";
        for (int i = 0; i < descLen; i += 2) desc += Encoding.Unicode.GetChars(new byte[] { data[i + 1 + nameLen], data[i + 2 + nameLen] })[0];
        float r = Convert.ToSingle(BitConverter.ToDouble(data, nameLen + descLen + 1));
        float g = Convert.ToSingle(BitConverter.ToDouble(data, nameLen + descLen + 8));
        float b = Convert.ToSingle(BitConverter.ToDouble(data, nameLen + descLen + 16));
        name = name.Remove(name.Length - 1);
        return new Player(id, name, desc, new Color(r, g, b));
    }
}
