using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GuitarThing
{
    public static class GuiController
    {
        // Controls
        private static ComboBox cmbMode = MainWindow.instance.cmbMode;
        private static ComboBox cmbKey = MainWindow.instance.cmbKey;
        private static ComboBox cmbSign = MainWindow.instance.cmbSign;
        private static ComboBox cmbStartFret = MainWindow.instance.cmbStartFret;
        private static ComboBox cmbEndFret = MainWindow.instance.cmbEndFret;
        private static ComboBox cmbIndicator = MainWindow.instance.cmbIndicator;
        private static ComboBox cmbScale = MainWindow.instance.cmbScale;
        private static ComboBox cmbTuning = MainWindow.instance.cmbTuning;

        private static StackPanel spIntervals = MainWindow.instance.spIntervals;
        private static StackPanel spDisplay = MainWindow.instance.spDisplay;

        private static TextBox tbGuitar = MainWindow.instance.tbGuitar;
        private static TextBlock tblScale = MainWindow.instance.tblScale;
        private static TextBlock tblProgression = MainWindow.instance.tblProgression;
        private static Button btnProgression = MainWindow.instance.btnProgression;

        public static void Initialize()
        {
            InitializeComboBox(cmbMode);
            InitializeComboBox(cmbKey);
            InitializeComboBox(cmbSign);
            InitializeComboBox(cmbIndicator);

            // TODO
            InitializeComboBox(cmbScale);
            InitializeComboBox(cmbTuning);


            InitializeCheckBoxes(spDisplay);
            InitializeCheckBoxes(spIntervals);

            btnProgression.Click += UpdateHandler;

            CreateFretComboBoxItems();
            Update();
        }

        private static void UpdateHandler(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private static void Update()
        {
            int mode = GetSelectedInt(cmbMode);
            int key = GetSelectedInt(cmbKey);
            int[] intervals = GetSelectedIntArr(spIntervals);
            int[] displayPreference = GetSelectedIntArr(spDisplay);
            bool isFlats = GetSelectedBool(cmbSign);
            bool isDots = GetSelectedBool(cmbIndicator);
            int startFret = GetSelectedInt(cmbStartFret);
            int endFret = GetSelectedInt(cmbEndFret);

            // TODO
            int scaleType = GetSelectedInt(cmbScale);
            int tuning = GetSelectedInt(cmbTuning);

            UpdateFretComboBoxItems(startFret, endFret);
            UpdateKeyComboBoxItems(isFlats);

            Scale scale = new Scale(scaleType, key, mode);
            tbGuitar.Text = Guitar.WriteGuitar(scale, intervals, tuning:tuning, startFret:startFret, endFret:endFret, preferFlatsToSharps:isFlats, displayPreference: displayPreference, isDots: isDots);

            //tbGuitar.Text = Guitar.WriteGuitar(scaleType, startFret, endFret, mode, key, intervals, isFlats, displayPreference, isDots);
            tblScale.Text = scale.GetScaleName() + " - " + scale.ToString(isFlats);
            tblProgression.Text = new Progression(scale, 4).ToString();
        }

        // Get values
        private static int GetSelectedInt(ComboBox box)
        {
            var items = box.Items;
            for (int i = 0; i < items.Count; i++)
            {
                ComboBoxItem item = (ComboBoxItem)items[i];
                if (item.IsSelected)
                {
                    return i;
                }
            }

            // Shouldn't get here
            MessageBox.Show(box.Name + ": oops");
            return 0;
        }
        private static bool GetSelectedBool(ComboBox box)
        {
            int i = GetSelectedInt(box);
            return i == 0 ? true : false;
        }
        private static int[] GetSelectedIntArr(StackPanel sp)
        {
            List<int> intervals = new List<int>();
            CheckBox[] cbs = GetCheckBoxes(sp);

            for (int i = 0; i < cbs.Length; i++)
            {
                if ((bool)cbs[i].IsChecked)
                {
                    intervals.Add(i);
                }
            }
            return intervals.ToArray();
        }

        private static CheckBox[] GetCheckBoxes(StackPanel sp)
        {
            List<CheckBox> cbs = new List<CheckBox>();
            var children = sp.Children;
            foreach (var c in children)
            {
                if (c.GetType() == typeof(CheckBox))
                {
                    cbs.Add((CheckBox)c);
                }
            }
            return cbs.ToArray();
        }

        private static void InitializeComboBox(ComboBox box)
        {
            // only sets the update handler

            // Get each comboBoxItem
            var items = box.Items;
            foreach (object o in items)
            {
                if (o.GetType() == typeof(ComboBoxItem))
                {
                    ComboBoxItem item = (ComboBoxItem)o;
                    item.Selected += UpdateHandler;
                }
            }
        }
        private static void InitializeCheckBoxes(StackPanel sp)
        {
            CheckBox[] cbs = GetCheckBoxes(sp);
            foreach (CheckBox cb in cbs)
            {
                cb.Checked += UpdateHandler;
                cb.Unchecked += UpdateHandler;
            }
        }

        private static void CreateFretComboBoxItems()
        {
            for (int i = 0; i <= 24; i++)
            {
                ComboBoxItem startItem = new ComboBoxItem();
                cmbStartFret.Items.Add(startItem);
                startItem.Content = i.ToString();
                if (i == 0) cmbStartFret.SelectedIndex = i;
                startItem.Selected += UpdateHandler;

                ComboBoxItem endItem = new ComboBoxItem();
                cmbEndFret.Items.Add(endItem);
                endItem.Content = i.ToString();
                if (i == 15) cmbEndFret.SelectedIndex = i;
                endItem.Selected += UpdateHandler;
            }
        }
        private static void UpdateKeyComboBoxItems(bool preferFlats)
        {
            // Notes / Keys
            // 1 = C# / Db
            // 3 = D# / Eb
            // 6 = F# / Gb
            // 8 = G# / Ab
            // 10 = A# / Bb

            for (int i = 0; i < 12; i++)
            {
                if (i == 1 || i == 3 || i == 6 || i == 8 || i == 10)
                {
                    ComboBoxItem item = (ComboBoxItem)cmbKey.Items[i];
                    item.Content = Scale.GetNoteName(i, preferFlats, false);
                }
            }
        }
        private static void UpdateFretComboBoxItems(int startFret, int endFret)
        {
            // make each comboBoxItem in cmbStartFret after endFret invisibile
            // make each comboBoxItem in cmbEndFret before startFret invisible
            // make everything else visible

            var startItems = cmbStartFret.Items;
            var endItems = cmbEndFret.Items;
            for (int i = 0; i <= 24; i++)
            {
                ComboBoxItem startItem = (ComboBoxItem)startItems[i];
                startItem.Visibility = i > endFret ? Visibility.Collapsed : Visibility.Visible;

                ComboBoxItem endItem = (ComboBoxItem)endItems[i];
                endItem.Visibility = i < startFret ? Visibility.Collapsed : Visibility.Visible;
            }
        }
    }
}