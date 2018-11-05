using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GuitarThing
{

    public static class Music
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

        // Notes
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

        static readonly int[] MajorRelativeKey = new int[]
        {
            0, 2, 4, 5, 7, 9, 11
        };
        static readonly int[] MajorSteps = new int[]
        {
            2, 2, 1, 2, 2, 2, 1
        };

        public static int[] GetScale(int mode, int key)
        {
            int[] steps = GetModeSteps(mode);
            int[] scaleFromZero = StepsToRelativeKey(steps);
            return RelativeKeyToScale(scaleFromZero, key);
        }
        public static int[] GetIntervals(int[] scale, int[] intervals)
        {
            int[] notes = new int[intervals.Length];
            for (int i = 0; i < intervals.Length; i++)
            {
                notes[i] = scale[intervals[i]];
            }
            return notes;
        }

        public static string GetScaleLine(int mode, int key, bool isFlats)
        {
            int[] scale = GetScale(mode, key);
            return IntArrToLine(scale, isFlats);
        }
        public static string IntArrToLine(int[] ints, bool preferFlats)
        {
            string s = "";
            foreach (int i in ints)
            {
                string note = IndexToNote(i, preferFlats);
                s += note + ", ";
            }
            return s;
        }
        public static string IndexToNote(int index, bool preferFlats)
        {
            switch (index)
            {
                case 0:
                    return "C ";
                case 1:
                    return preferFlats ? "Db" : "C#";
                case 2:
                    return "D ";
                case 3:
                    return preferFlats ? "Eb" : "D#";
                case 4:
                    return "E ";
                case 5:
                    return "F ";
                case 6:
                    return preferFlats ? "Gb" : "F#";
                case 7:
                    return "G ";
                case 8:
                    return preferFlats ? "Ab" : "G#";
                case 9:
                    return "A ";
                case 10:
                    return preferFlats ? "Bb" : "A#";
                case 11:
                    return "B ";
                default:
                    return "?";
            }
        }

        private static int[] GetModeSteps(int mode)
        {
            // reorganize the MajorRelative array starting from index [mode]
            int[] modeRelative = new int[MajorSteps.Length];

            for (int i = 0; i < modeRelative.Length; i++)
            {
                modeRelative[i] = MajorSteps[i.Step(mode, MajorSteps.Length)];
            }

            return modeRelative;
        }
        private static int[] StepsToRelativeKey(int[] steps)
        {
            // should start at 0
            // [2, 2, 1, 2...] -> [0, 2, 4, 5...]

            int[] absolute = new int[steps.Length];

            int temp = 0;
            for (int i = 0; i < absolute.Length; i++)
            {
                absolute[i] = temp;
                temp += steps[i];
            }

            return absolute;
        }
        private static int[] RelativeKeyToScale(int[] relativeKey, int key)
        {
            int[] scale = new int[relativeKey.Length];
            for (int i = 0; i < scale.Length; i++)
            {
                scale[i] = relativeKey[i].Step(key, 12); // 12 semitones
            }

            return scale;
        }
        private static int Step(this int start, int amount, int indexes)
        {
            int raw = start + amount;
            int net = raw;
            while (net >= indexes)
            {
                net -= indexes;
            }
            while (net < 0)
            {
                net += indexes;
            }

            return net;
        }
        public static int Step_(int start, int amount, int indexes)
        {
            int raw = start + amount;
            int net = raw;
            while (net >= indexes)
            {
                net -= indexes;
            }
            while (net < 0)
            {
                net += indexes;
            }

            return net;
        }
    }
}

