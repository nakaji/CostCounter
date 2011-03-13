using System.Windows;
using CostCounter.ViewModel;

namespace CostCounter
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _model;

        public MainWindow()
        {
            InitializeComponent();
            _model = new MainWindowViewModel();
            DataContext = _model;

            _model.Interval = 1;
        }
    }
}
