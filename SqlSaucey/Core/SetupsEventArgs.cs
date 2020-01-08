using System;
using SqlSaucey.Models;
namespace SqlSaucey.Core
{
    public class SetupsEventArgs : EventArgs
    {
        private Setups _Setups;
        public Setups Setups { get { return _Setups; } }
        public SetupsEventArgs(Setups setups) {
            _Setups = setups;
        }
    }
}
