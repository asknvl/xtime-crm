﻿using crm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crm.WS
{
    public interface IWindowService
    {
        void ShowWindow(ViewModelBase vm);
        void ShowDialog(ViewModelBase vm);
        void ShowDialog(ViewModelBase vm, ViewModelBase parent);
        Task<string> ShowFileDialog(string title, ViewModelBase parent);
    }
}
