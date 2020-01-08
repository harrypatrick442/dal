using System.Windows;
using SqlSaucey.Models;
using SqlSaucey.ViewModels;
using SqlSaucey.Strings;
using SqlSaucey.Core;
using SqlSaucey.Views;
using WPFCore;

namespace SqlSaucey
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _ExeDirectory { get { return System.AppDomain.CurrentDomain.BaseDirectory; } }
        private string _ConfigurationFile { get { return System.IO.Path.Combine(_ExeDirectory, SL.CONFIGURATION_FILE_NAME); } }
        private MainViewModel __MainViewModel;
        private MainViewModel _MainViewModel
        {
            get
            {
                if (__MainViewModel == null)
                {
                    __MainViewModel = new MainViewModel(_Configuration);

                }
                return __MainViewModel;
            }
        }
        public MenuViewModel Menu
        {
            get
            {
                return _MainViewModel.Menu;
            }
        }
        private Configuration __Configuration;
        private Configuration _Configuration
        {
            get
            {
                if (__Configuration == null)
                {
                    __Configuration = new Configuration(_ConfigurationFile);
                }
                return __Configuration;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _MainViewModel;
            new MessageReciever(this, OnMessage, true);
        }
        private void OnMessage(string message)
        {
            switch (message) {

            }
        }
    }
}
