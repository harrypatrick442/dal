using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ConsoleControl
{
    class Entries
    {
        private const int MAX_N_LINES_DEFAULT = 40;
        private int _MaxNLines = MAX_N_LINES_DEFAULT;
        private List<Entry> _List = new List<Entry>();
        private RichTextBox _RichTextBox;
        public Entries(RichTextBox richTextBox) {
            _RichTextBox = richTextBox;
        }
        public void Add(Entry entry) {
            _List.Add(entry);
            Overflow();
        }
        private void Overflow() {
            while (_List.Count > _MaxNLines)
            {
                Entry entry = _List[0];
                entry.Dispose();
                _List.Remove(entry);
            }
        }
    }
}
