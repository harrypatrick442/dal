using JSON;
using System;
using System.Collections.Generic;
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
using WPFCore;

namespace ConsoleControl
{
    /// <summary>
    /// Interaction logic for Console1.xaml
    /// </summary>
    public partial class Console : UserControl
    {
        private Entries __Entries;
        private Entries _Entries { get { if (__Entries == null) __Entries = new Entries(richTextBox); return __Entries; } }
        private Paragraph __Paragraph;
        private Paragraph _Paragraph
        {
            get
            {
                if (__Paragraph == null)
                {
                    __Paragraph = new Paragraph();
                    richTextBox.Document.Blocks.Add(__Paragraph);
                }
                return __Paragraph;
            }
        }
        public Console()
        {
            InitializeComponent();
            _MessageReceiver = new MessageReciever(this, OnMessage, false);
        }
        public void Write(string text, Color? color = null)
        {
            _Entries.Add(new Entry(_Paragraph, text, color));
            ScrollIfNearBottom();
        }
        private void ScrollIfNearBottom() {
            bool shouldScroll = richTextBox.ViewportHeight + richTextBox.VerticalOffset >= 0.96 * richTextBox.ExtentHeight;
            if (!shouldScroll) return;
            richTextBox.ScrollToEnd();
        }




        private MessageReciever _MessageReceiver;
        private void OnMessage(string msg)
        {
            JObject jObject = (JObject)JSONHelper.Deserialize(msg);
            string type = (string)jObject[SL.TYPE];
            string message = (string)jObject[SL.MESSAGE];

            Dispatcher.Invoke((Action)(() =>
            {
                switch (type)
                {
                    case SL.ERROR:
                        Write(message, Colors.Red);
                        break;
                    default:
                        
                        Write(message);
                        break;
                }
            }));
        }
    }
}
