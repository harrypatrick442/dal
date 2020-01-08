using System.ComponentModel;
using SqlSaucey.Models;
using SqlSaucey.Core;
namespace SqlSaucey.ViewModels
{
    public class ComboBoxPair
    {
        public string _Key { get; }
        public string _Value { get;  }
        public ComboBoxPair(string key, string value)
        {
            _Key = key;
            _Value = value;
        }
    }
}