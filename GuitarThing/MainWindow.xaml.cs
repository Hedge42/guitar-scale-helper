using System.Windows;

namespace GuitarThing
{
    public partial class MainWindow : Window
    {
        public static MainWindow instance;

        public MainWindow()
        {
            InitializeComponent();

            instance = this;

            GuiController.Initialize();
        }
    }
}
