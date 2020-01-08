using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
namespace ConsoleControl
{
    class Entry
    {
        private Paragraph _Paragraph;
        private Run _Run;
        private LineBreak _LineBreak;
        public Entry(Paragraph paragraph, string text, Color? color= null) {
            _Paragraph = paragraph;
            _Run = new Run(text);
            _LineBreak = new LineBreak();
            if (color != null)
                _Run.Foreground = new SolidColorBrush((Color)color);
            _Paragraph.Inlines.Add(_Run);
            _Paragraph.Inlines.Add(_LineBreak);
        }
        public void Dispose()
        {
            _Paragraph.Inlines.Remove(_Run);
            _Paragraph.Inlines.Remove(_LineBreak);
        }
    }
}
