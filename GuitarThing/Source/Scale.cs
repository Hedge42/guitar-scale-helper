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
    public int mode { get; private set; }

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

    // 0 = Major
    // 1 = Harmonic Minor
    // 2 = Hungarian Minor???
    public int scaleType { get; private set; }

    // [0, 2, 4, 5, 7, 9, 11]...
    public int[] notes { get; private set; }

    // C, D, Eb, F...
    public string stringScale { get; private set; }
    private bool preferFlats = true;

    private readonly int[][] Scales = new int[][]
    {
        new int[]
        {
            // Major
            0, 2, 4, 5, 7, 9, 11
        },

        new int[]
        {
            // Harmonic Minor
            // Aeolian #7
            0, 2, 3, 5, 7, 8, 11
        },
    };

    // Adapted from Python script...
    public Scale (int scaleType=0, int key=0, int mode=0, bool clamp=true)
    {
        this.scaleType = scaleType;
        this.notes = new int[7];
        int[] baseScale = Scales[scaleType];
        for (int i = 0; i < notes.Length; i++)
        {
            int interval = (mode + i) % 7;
            int note = (baseScale[interval] - baseScale[mode] + key) % 12;
            if (!clamp)
                while (note < notes[i - 1])
                    note += 12;
            notes[i] = note;
        }
    }

    // ie, "A#m "
    public string GetChordName(int interval)
    {
        interval %= 7;
        return GetNoteName(notes[interval], preferFlats, false) + (IsMinor(interval) ? "m" : "");
    }

    // whitelisting intervals
    public int[] GetIntervals(int[] intervals)
    {
        int[] filtered = new int[intervals.Length];
        for (int i = 0; i < filtered.Length; i++)
            filtered[i] = notes[intervals[i]];
        return filtered;
    }
    public bool IsMinor(int interval)
    {
        int chordRootNote = notes[interval];
        //int chordThirdNote = notes[Clamp(interval + 2, 7)];
        int chordThirdNote = notes[(interval + 2) % 7];
        if (chordThirdNote < chordRootNote)
            chordThirdNote += 12;
        return chordThirdNote - chordRootNote == 3;
    }

    // C, C#, Eb...
    public static string GetNoteName(int note, bool preferFlats, bool pad)
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

    // 0, 1, 2...11
    public static int GetNoteValue(string note)
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

    // Major, Melodic Minor, Hungarian Minor...
    public static string GetScaleType(int type)
    {
        if (type == 0)
            return "Major";
        else if (type == 1)
            return "Harmonic Minor";
        else
            return "Not recognized";
    }

    // C# Harmonic Minor Ionian
    public string GetScaleName()
    {
        return GetNoteName(key, preferFlats, false) + " " + GetScaleType(scaleType) + " " + GetModeName();
    }

    // Ionian...Locrian
    private string GetModeName()
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

    // C#, D#...C
    public string ToString(bool preferFlats)
    {
        this.preferFlats = preferFlats;

        string line = "";
        for (int i = 0; i < notes.Length; i++)
            line += GetNoteName(notes[i], this.preferFlats, false) + (IsMinor(i) ? "m" : "") + " ";
        return line;
    }

    public override string ToString()
    {
        return ToString(preferFlats);
    }
}
