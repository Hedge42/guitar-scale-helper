using System;
using System.Collections.Generic;
using System.Text;

public class Progression
{
    private Scale scale;

    private int numChords;
    private string[] chords;
    private int[] intervals;
    private Random r;

    public Progression(Scale scale, int numChords)
    {
        this.scale = scale;
        this.numChords = numChords;
        this.r = new Random();

        SetChords();
    }

    private string IntervalToString(int interval)
    {
        interval %= 7;
        if (interval < 0)
            interval *= -1;

        bool isMinor = scale.IsMinor(interval);
        switch (interval)
        {
            case 0:
                return isMinor ? "i" : "I";
            case 1:
                return isMinor ? "ii" : "II";
            case 2:
                return isMinor ? "iii" : "III";
            case 3:
                return isMinor ? "iv" : "IV";
            case 4:
                return isMinor ? "v" : "V";
            case 5:
                return isMinor ? "vi" : "VI";
            case 6:
                return isMinor ? "vii" : "VII";
            default:
                return "wot";
        }
    }

    private int[] SetIntervals()
    {
        if (numChords < 2)
            return null;

        intervals = new int[numChords];

        // set first and last intervals
        intervals[0] = 0;
        intervals[intervals.Length - 1] = r.Next(3, 6); // 4, 5, or 6

        // make everything else completely random
        for (int i = 1; i < intervals.Length - 1; i++)
            intervals[i] = r.Next(7);

        return intervals;
    }
    private string[] SetChords()
    {
        chords = new string[numChords];

        int[] intervals = SetIntervals();

        for (int i = 0; i < numChords; i++)
        {
            string chord = scale.GetChordName(intervals[i]);
            int add = r.Next(3);
            if (add == 1)
                chord += "7";
            else if (add == 2)
                chord += "9";
            chords[i] = chord;
        }

        return chords;
    }

    public override string ToString()
    {
        string s = "    ";
        for (int i = 0; i < numChords - 1; i++)
            s += chords[i] + " - ";
        s += chords[chords.Length - 1] + "\r\n    "; // properly formatted final chord

        for (int i = 0; i < numChords - 1; i++)
            s += IntervalToString(intervals[i]) + " - ";
        s += IntervalToString(intervals[intervals.Length - 1]);

        return s;
    }
}
