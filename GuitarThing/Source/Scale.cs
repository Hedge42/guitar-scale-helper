using System;
using System.Collections.Generic;
using System.Text;

public class Scale
{
    // https://en.wikipedia.org/wiki/Mode_(music)#Modern_modes

    // Modes
    // 0 = Ionian
    // 1 = Dorian,
    // 2 = Phrygian,
    // 3 = Lydian,
    // 4 = Mixolydian,
    // 5 = Aeolian,
    // 6 = Locrian

    // Notes / Keys
    // 0 = C
    // 1 = C# / Db
    // 2 = D
    // 3 = D# / Eb
    // 4 = E
    // 5 = F
    // 6 = F# / Gb
    // 7 = G
    // 8 = G# / Ab
    // 9 = A
    // 10 = A# / Bb
    // 11 = B

    public int key { get; private set; }
    public int mode { get; private set; }
    public int[] intScale { get; private set; }
    public string stringScale { get; private set; }
    private bool preferFlats = false;

    public Scale(int key, int mode)
    {
        this.key = Clamp(key, 12);
        this.mode = Clamp(mode, 7);
        intScale = GetNotes();
    }
    public Scale(int key, int mode, bool preferFlats)
    {
        this.key = Clamp(key, 12);
        this.mode = Clamp(mode, 7);
        intScale = GetNotes();
        this.preferFlats = preferFlats;
    }

    private readonly int[] CMajor = new int[]
    {
            0, 2, 4, 5, 7, 9, 11
    };

    /// <summary>
    /// Returns array of length 7. The index is the interval (+1) of the scale
    /// </summary>
    /// <returns></returns>
    private int[] GetNotes()
    {
        int[] scale = new int[7];
        for (int i = 0; i < scale.Length; i++)
        {
            int modalOffset = CMajor[mode];

            int index = Clamp(mode + i, 7);
            int note = Clamp(CMajor[index] + key - modalOffset, 12);
            scale[i] = note;
        }
        return scale;
    }

    /// <summary>
    /// ie, "A#m7"
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="add7"></param>
    /// <returns></returns>
    public string GetChordName(int interval)
    {
        interval = Clamp(interval, 7);
        return IntToString(intScale[interval], preferFlats, false) + (IsMinor(interval) ? "m" : "");
    }
    public int[] GetIntervals(int[] intervals)
    {
        int[] filtered = new int[intervals.Length];
        for (int i = 0; i < filtered.Length; i++)
            filtered[i] = intScale[intervals[i]];
        return filtered;
    }

    /// <summary>
    /// Consistent per mode
    /// </summary>
    /// <param name="interval"></param>
    /// <returns></returns>
    public bool IsMinor(int interval)
    {
        int chordRootNote = intScale[interval];
        int chordThirdNote = intScale[Clamp(interval + 2, 7)];
        if (chordThirdNote < chordRootNote)
            chordThirdNote += 12;
        return chordThirdNote - chordRootNote == 3;
    }

    /// <summary>
    /// -1 < returned < exclusiveUpperBound.
    /// </summary>
    /// <param name="num"></param>
    /// <param name="exlusiveUpperBound"></param>
    /// <returns></returns>
    private static int Clamp(int num, int exlusiveUpperBound)
    {
        num %= exlusiveUpperBound;
        if (num < 0)
            num += exlusiveUpperBound;
        return num;
    }
    /// <summary>
    /// Used for chromatic scale
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static int Clamp12(int num)
    {
        return Clamp(num, 12);
    }
    /// <summary>
    /// Used for modes and intervals
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    public static int Clamp7(int num)
    {
        return Clamp(num, 7);
    }

    /// <summary>
    /// Sets future state of preferFlats to the param value
    /// </summary>
    /// <param name="preferFlats"></param>
    /// <returns></returns>
    public string ToString(bool preferFlats)
    {
        this.preferFlats = preferFlats;

        string line = "";
        foreach (int note in intScale)
            line += IntToString(note, this.preferFlats, true) + " ";
        return line;
    }

    /// <summary>
    /// Calls ToString(flats) with internal value
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return ToString(preferFlats);
    }

    /// <summary>
    /// C = 0, C# = 1, D = 2, ... , B = 11
    /// </summary>
    /// <param name="note"></param>
    /// <param name="preferFlats"></param>
    /// <returns></returns>
    public static string IntToString(int note, bool preferFlats, bool pad)
    {
        switch (note)
        {
            case 0:
                return pad ? "C " : "C";
            case 1:
                return preferFlats ? "Db" : "C#";
            case 2:
                return pad ? "D " : "D";
            case 3:
                return preferFlats ? "Eb" : "D#";
            case 4:
                return pad ? "E " : "E";
            case 5:
                return pad ? "F " : "F";
            case 6:
                return preferFlats ? "Gb" : "F#";
            case 7:
                return pad ? "G " : "G";
            case 8:
                return preferFlats ? "Ab" : "G#";
            case 9:
                return pad ? "A " : "A";
            case 10:
                return preferFlats ? "Bb" : "A#";
            case 11:
                return pad ? "B " : "B";
            default:
                return "?";
        }
    }

    /// <summary>
    /// C = 0, C# = 1, D = 2, ... , B = 11
    /// </summary>
    public static int StringToInt(string note)
    {
        note = note.ToLower().Trim();
        switch (note)
        {
            // C
            case "c":
                return 0;

            // C# / Db
            case "c#":
                return 1;
            case "db":
                return 1;

            // D
            case "d":
                return 2;

            // D# / Eb
            case "d#":
                return 3;
            case "eb":
                return 3;

            // F
            case "f":
                return 4;

            // F# / Gb
            case "f#":
                return 5;
            case "gb":
                return 5;

            // G
            case "g":
                return 6;

            // G# / Eb
            case "g#":
                return 7;
            case "ab":
                return 8;

            // A
            case "a":
                return 9;

            // A# / Bb
            case "a#":
                return 10;
            case "bb":
                return 10;

            // B
            case "b":
                return 11;

            default:
                return -1;
        }
    }

    public string GetDescription()
    {
        return IntToString(key, preferFlats, false) + " " + GetMode();
    }
    private string GetMode()
    {
        switch (mode)
        {
            case 0:
                return "Ionian";
            case 1:
                return "Dorian";
            case 2:
                return "Phrygian";
            case 3:
                return "Lydian";
            case 4:
                return "MixoLydian";
            case 5:
                return "Aeolian";
            case 6:
                return "Locrian";
            default:
                return "u wot";
        }
    }
}
