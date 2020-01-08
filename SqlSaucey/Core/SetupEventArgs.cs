using System;
using SqlSaucey.Models;
namespace SqlSaucey.Core
{
    public class SetupEventArgs : EventArgs
    {
        private Setup _Setup;
        public Setup Setup { get { return _Setup; } }
        public SetupEventArgs(Setup setup) {
            _Setup = setup;
        }
    }
}
