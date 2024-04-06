using ReactiveUI;
using SAP.Interfaces.State;
using System.Reactive.Disposables;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel,
                    vm => vm.CurrentState,
                    v => v.CurrentStateLabel.Content,
                    state => state?.Name)
                .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.CurrentState,
                    v => v.ProgressBar.IsIndeterminate,
                    state => state is { Name: "CheckingUpdates" }) // todo, fix this
                .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.StartGameCommand,
                    v => v.StartGameButton)
                .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.PatchProgress,
                    v => v.ProgressBar.Value,
                    f => (double)f)
                .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.TotalPatchProgress,
                    v => v.TotalProgressBar.Value,
                    f => (double)f)
                .DisposeWith(disposables);
            });
        }
    }
}