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


        public static string WriteGuitar(int startFret, int endFret, int mode, int key, int[] intervals, bool preferFlatsToSharps, int[] displayPreference, bool isDots)
        {
            string guitar = "";

            // TODO: don't generate scale twice              
            Scale s = new Scale(key, mode, preferFlatsToSharps);
            int[] scale = s.intScale;
            int[] filteredScale = s.GetIntervals(intervals);

            string indicatorLine = GetIndicatorLine(startFret, endFret, isDots);

            // write indicator on both sides
            guitar += indicatorLine;
            foreach (int openString in StandardTuning)
            {
                string guitarString = WriteGuitarString(startFret, endFret, preferFlatsToSharps, displayPreference, scale, filteredScale, openString);
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

                int intAtFret = Scale.Clamp12(openString + i);
                string noteAtFret = Scale.IntToString(intAtFret, isFlats, true);

                if (IsContained(intAtFret, filteredScale))
                {
                    // print *something* here


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
                if (i == 3 || i == 5 || i == 7 || i == 9 || i == 12 || i == 15 || i == 17 || i == 19 || i == 21 || i == 24)
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
    }
}
