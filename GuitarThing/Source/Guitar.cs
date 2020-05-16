using System;

namespace GuitarThing
{
    public static class Guitar
    {
        public static readonly int[] StandardTuning = new int[]
        {
            4,  // E - 1st string
            11, // B - 2nd string
            7,  // G - 3rd string
            2,  // D - 4th string
            9,  // A - 5th string
            4   // E - 6th string 
        };

        

        public static string WriteGuitar(Scale scale, int[] intervals, int tuning=0, int startFret=0, int endFret=15, bool preferFlatsToSharps=true, int[] displayPreference=null, bool isDots=false)
        {
            string guitar = "";

            int[] openNotes = GetTuning(tuning);
            int[] filteredNotes = scale.GetIntervals(intervals);
            string indicatorLine = GetIndicatorLine(startFret, endFret, isDots);

            // write indicator on both sides
            guitar += indicatorLine;
            foreach (int openString in openNotes)
            {
                string guitarString = WriteGuitarString(startFret, endFret, preferFlatsToSharps, displayPreference, scale.notes, filteredNotes, openString);
                guitar += guitarString;
            }
            guitar += indicatorLine;

            return guitar;
        }

        /// <summary> Fully writes a single string on the guitar </summary>
        private static string WriteGuitarString(int startFret, int endFret, bool isFlats, int[] displayPreference, int[] scale, int[] filteredScale, int openString)
        {
            // foreach string, print the string
            string _string = "";
            for (int i = startFret; i <= endFret; i++)
            {
                string fret = "";

                int intAtFret = (openString + i) % 12;
                string noteAtFret = Scale.GetNoteName(intAtFret, isFlats, true);

                // print note stuff in this fret
                if (IsContained(intAtFret, filteredScale))
                {
                    if (displayPreference.Length == 0)
                    {
                        // dots
                        fret = " <> ";
                    }
                    else if (displayPreference.Length == 2)
                    {
                        // notes AND intervals
                        int interval = Array.IndexOf(scale, intAtFret) + 1;
                        fret = noteAtFret + " " + interval;
                    }
                    else
                    {
                        if (displayPreference[0] == 0)
                        {
                            // notes
                            fret = " " + noteAtFret + " ";
                        }
                        else
                        {
                            // intervals
                            fret = " " + (Array.IndexOf(scale, intAtFret) + 1) + "  ";
                        }
                    }
                }
                else
                {
                    fret = "----";
                }

                _string += fret + "|";
            }
            return _string + "\r\n";
        }

        /// <summary> Determines whether or not int a is contained in int[] b </summary>
        private static bool IsContained(int a, int[] b)
        {
            foreach (int i in b)
                if (a == i)
                    return true;

            return false;
        }

        /// <summary> Prints the dots on frets 3, 5, 7, etc... </summary>
        private static string GetIndicatorLine(int startFret, int endFret, bool isDots)
        {
            string s = "";
            for (int i = startFret; i <= endFret; i++)
            {
                // TODO lol
                if (i==0 || i == 3 || i == 5 || i == 7 || i == 9 || i == 12 || i == 15 || i == 17 || i == 19 || i == 21 || i == 24)
                {
                    if (isDots)
                        s += "[<>] ";
                    else
                    {
                        if (i < 10)
                            s += "[ " + i + "] ";
                        else
                            s += "[" + i + "] ";
                    }
                }
                else
                    s += "     ";
            }
            return s + "\r\n";
        }

        private static int[] GetTuning(int tuning)
        {
            // TODO Lmao

            // 0 = E standard
            if (tuning == 0)
                return GetTuning(6, 0, false);
            // 1 = Drop D
            else if (tuning == 1)
                return GetTuning(6, 0, true);
            // 2 = Eb standard
            else if (tuning == 2)
                return GetTuning(6, 1, false);
            // 3 = Drop Db
            else if (tuning == 3)
                return GetTuning(6, 1, true);
            // 4 = D standard
            else if (tuning == 4)
                return GetTuning(6, 2, false);
            // 5 = Drop C
            else if (tuning == 5)
                return GetTuning(6, 2, true);

            // LOL
            return null;
        }
        private static int[] GetTuning(int numStrings, int downTones, bool dropped)
        {
            int[] strings = new int[numStrings];

            for (int i = 0; i < numStrings; i++)
            {
                // string - downtones
                int note = (StandardTuning[i] + (12 - downTones)) % 12;
                strings[i] = note;
            }

            if (dropped)
                strings[numStrings - 1] = (strings[numStrings - 1] + 10) % 12;

            return strings;
        }
    }
}
