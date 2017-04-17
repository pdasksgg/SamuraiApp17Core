﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SA.Model
{
    public class ClientChangeTracker : INotifyPropertyChanged
    {
        private bool _isDirty;
        public bool IsDirty { get { return _isDirty; } set {
                SetWithNotify(value, ref _isDirty);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetWithNotify<T>(T value, ref T field, [CallerMemberName] string propertyName = "")
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
