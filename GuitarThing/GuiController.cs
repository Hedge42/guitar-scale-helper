using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private static StackPanel spIntervals = MainWindow.instance.spIntervals;
        private static StackPanel spDisplay = MainWindow.instance.spDisplay;

        private static TextBox tblGuitar = MainWindow.instance.tblGuitar;
        private static TextBlock tbScale = MainWindow.instance.tbScale;

        public static void Initialize()
        {
            InitializeComboBox(cmbMode);
            InitializeComboBox(cmbKey);
            InitializeComboBox(cmbSign);
            InitializeComboBox(cmbIndicator);

            InitializeCheckBoxes(spDisplay);
            InitializeCheckBoxes(spIntervals);

            CreateFretItems();

            Update();
        }

        private static void UpdateHandler(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private static void Update()
        {
            int _mode = GetSelectedInt(cmbMode);
            int _key = GetSelectedInt(cmbKey);
            int[] _intervals = GetSelectedIntArr(spIntervals);
            int[] _displayPreference = GetSelectedIntArr(spDisplay);
            bool _isFlats = GetSelectedBool(cmbSign);
            bool _isDots = GetSelectedBool(cmbIndicator);
            int _startFret = GetSelectedInt(cmbStartFret);
            int _endFret = GetSelectedInt(cmbEndFret);

            UpdateFretItems(_startFret, _endFret);

            tblGuitar.Text = Guitar.WriteGuitar(_startFret, _endFret, _mode, _key, _intervals, _isFlats, _displayPreference, _isDots);
            tbScale.Text = Music.GetScaleLine(_mode, _key, _isFlats);
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

        // Initialize controls
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

        private static void UpdateFretItems(int startFret, int endFret)
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

        private static void UpdateComboBox(ComboBox box)
        {
            if (box == cmbKey)
            {

            }
        }

        private static void CreateFretItems()
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
    }
}
