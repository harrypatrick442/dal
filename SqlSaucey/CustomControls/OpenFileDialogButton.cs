using SqlSaucey.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using SqlSaucey.Core;
namespace SqlSaucey.CustomControls
{
    public class OpenFileDialogButton : Control
    {
        public static readonly DependencyProperty DirectoryProperty = DependencyProperty.Register(nameof(Directory),
                typeof(string),
                typeof(OpenFileDialogButton),
               new FrameworkPropertyMetadata(null,
                          FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
                typeof(string),
                typeof(OpenFileDialogButton)
            );
        private ICommand _ButtonCommand;
        public ICommand ButtonCommand
        {
            get
            {
                if (_ButtonCommand == null)
                    _ButtonCommand = new CommandHandler(OnButtonCommand);
                return _ButtonCommand;
            }
        }
        public string Directory { get { return (string)this.GetValue(DirectoryProperty); } set { this.SetValue(DirectoryProperty, value); } }
        private void OnButtonCommand()
        {
            var openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.ShowDialog();
            if (string.IsNullOrEmpty(openFileDialog.FileName)) return;
            FileName = openFileDialog.FileName;
        }

        public string Text { get { return (string)this.GetValue(TextProperty); } set { this.SetValue(TextProperty, value); } }
        static OpenFileDialogButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OpenFileDialogButton), new FrameworkPropertyMetadata(typeof(OpenFileDialogButton),
                      FrameworkPropertyMetadataOptions.Journal |
                          FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                      new PropertyChangedCallback(OnPropertyChanged)));
        }
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((OpenFileDialogButton)d).Directory = (string)e.NewValue;
        }
    }

}
