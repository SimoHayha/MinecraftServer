using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows.Media;
using MinecraftServer.Core.Network;
using MinecraftServer.Core.Network.Clients;
using System.Windows.Threading;
using System.Windows.Input;

namespace MinecraftServer.GUI
{
    public class ColorfulLogs
    {
        public String   text;
        public Brush    color;
    }

    public class ViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Client> Clients { get; private set; }
        public ObservableCollection<ColorfulLogs> logs { get; private set; }
        //private Core.Network.MinecraftServer _server;
        private Core.Network.Server _server;

        public ViewModel()
        {
            logs = new ObservableCollection<ColorfulLogs>();
            Clients = new ObservableCollection<Client>();
            _server = new Core.Network.Server(this);
            //_server = new Core.Network.MinecraftServer(this);
        }

        public void PrintHelp()
        {
            Log("help : display this");
            Log("? : see help");
            Log("start : start the server");
            Log("stop : stop the server (will fail if players connected)");
            Log("forcestop : stop the server");
        }

        public ObservableCollection<ColorfulLogs> Logs
        {
            get
            {
                return logs;
            }
            private set
            {
                this.logs = value;
                this.OnPropertyChanged("Logs");
            }
        }

        public void Log(string log)
        {
            ColorfulLogs tmp = new ColorfulLogs();

            tmp.color = Brushes.Black;
            tmp.text = log + "\n";
            try
            {
                Logs.Add(tmp);
            }
            catch
            {
            }
        }

        public void Error(string error)
        {
            ColorfulLogs tmp = new ColorfulLogs();

            tmp.color = Brushes.Red;
            tmp.text = "[ERROR] " + error + "\n";
            Logs.Add(tmp);
        }

        public void Warning(string warning)
        {
            ColorfulLogs tmp = new ColorfulLogs();

            tmp.color = Brushes.Orange;
            tmp.text = "[WARNING] " + warning + "\n";
            Logs.Add(tmp);
        }

        public void NewClient(Client client)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Clients.Add(client)));
        }

        public void RemoveClient(Client client)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => Clients.Remove(client)));
        }

        #region PropertyChanged
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public void ConsoleKeyDown(object sender, KeyEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box == null)
                return;

            if (box.Text.ToLower() == "help" || box.Text == "?")
            {
                PrintHelp();
                box.Text = "";
                return;
            }

            if (e.Key == Key.Return)
            {
                //_server.OnAdminCommand(box.Text);
                box.Text = "";
            }
        }
    }
}
