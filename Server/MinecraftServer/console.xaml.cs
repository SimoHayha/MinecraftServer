using MinecraftServer.GUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinecraftServer
{
    /// <summary>
    /// Interaction logic for console.xaml
    /// </summary>
    public partial class console : UserControl
    {
        #region Variables

        private static Object thisLock = new Object();
        public static RichTextBox cmdStatic;
        public static ObservableCollection<ColorfulLogs> UCLogs;

        public readonly DependencyProperty LogsProperty = DependencyProperty.Register("Logs",
                                                        typeof(ObservableCollection<ColorfulLogs>),
                                                        typeof(console),
                                                        new FrameworkPropertyMetadata(OnLogsPropertyChanged));

        public ObservableCollection<ColorfulLogs> Logs
        {
            get { return (ObservableCollection<ColorfulLogs>)GetValue(LogsProperty); }
            set { SetValue(LogsProperty, value); }
        }

        #endregion

        #region Constructors

        public console()
        {
            InitializeComponent();
            cmdStatic = cmd;
        }

        #endregion

        #region (Awful) Delegates

        private static void OnLogsPropertyChanged(DependencyObject source,
        DependencyPropertyChangedEventArgs e)
        {
            ObservableCollection<ColorfulLogs> text = (ObservableCollection<ColorfulLogs>)e.NewValue;
            if (console.UCLogs != null)
                console.UCLogs.CollectionChanged -= logsCollectionChanged;
            console.UCLogs = text;
            console.UCLogs.CollectionChanged += logsCollectionChanged;
        }

        static void logsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<ColorfulLogs> logs = sender as ObservableCollection<ColorfulLogs>;
            if (logs.Count != 0)
            {
                Paragraph para = console.cmdStatic.Document.Blocks.FirstBlock as Paragraph;
                if (para == null)
                {
                    para = new Paragraph();
                    FlowDocument doc = console.cmdStatic.Document;
                    doc.Blocks.Add(para);
                }

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    lock (console.thisLock)
                    {
                        try
                        {
                            if (logs.Count == 0)
                                return;
                            Run currentLog = new Run(logs.First().text);
                            currentLog.Foreground = logs.First().color;
                            logs.RemoveAt(0);

                            para.Inlines.Add(currentLog);
                            if ((console.cmdStatic.VerticalOffset + console.cmdStatic.ViewportHeight) == console.cmdStatic.ExtentHeight || console.cmdStatic.ExtentHeight < console.cmdStatic.ViewportHeight)
                                console.cmdStatic.ScrollToEnd();
                        }
                        catch
                        {

                        }
                    }
                }));
            }
        }

        #endregion
    }
}
