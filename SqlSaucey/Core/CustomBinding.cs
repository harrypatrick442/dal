using System;
using SqlSaucey.Models;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using SqlSaucey.Interfaces;
using SqlSaucey.Strings;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;

namespace SqlSaucey.Core
{
    public class CustomBinding<T>
    {
        DependencyProperty _DependencyProperty;
        private Control _Control;
        public CustomBinding(Control control, string name)
        {
            _Control = control;
            _DependencyProperty = DependencyProperty.Register("Text",
                typeof(T),
                control.GetType(),
                new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal)
            );
        }
        public void Set(T value)
        {
            _Control.SetValue(_DependencyProperty, value);
        }
        public T Get() {
            return (T)_Control.GetValue(_DependencyProperty);
        }
    }
}
