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
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:SqlSaucey.CustomControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:SqlSaucey.CustomControls;assembly=SqlSaucey.CustomControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:FolderBrowserDialog/>
    ///
    /// </summary>
    public class SaveFileDialogButton : Control
    {
        public static readonly DependencyProperty DirectoryProperty = DependencyProperty.Register(nameof(Directory),
                typeof(string),
                typeof(FolderBrowserDialog),
               new FrameworkPropertyMetadata(null,
                          FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
                typeof(string),
                typeof(FolderBrowserDialog)
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
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            if (string.IsNullOrEmpty(folderBrowserDialog.SelectedPath)) return;
            Directory = folderBrowserDialog.SelectedPath;
        }

        public string Text { get { return (string)this.GetValue(TextProperty); } set { this.SetValue(TextProperty, value); } }
        static FolderBrowserDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FolderBrowserDialog), new FrameworkPropertyMetadata(typeof(FolderBrowserDialog),
                      FrameworkPropertyMetadataOptions.Journal |
                          FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                      new PropertyChangedCallback(OnPropertyChanged)));
        }
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FolderBrowserDialog)d).Directory = (string)e.NewValue;
        }
        public void Click_Show(object o, EventArgs e)
        {

        }
    }

}
