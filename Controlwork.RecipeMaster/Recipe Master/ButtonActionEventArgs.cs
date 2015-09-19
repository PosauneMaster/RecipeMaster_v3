using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace BendSheets
{
    public class ButtonActionEventArgs : EventArgs
    {
        [DebuggerNonUserCode]
        public Machine MachineData
        { get; set; }

        [DebuggerNonUserCode]
        public ButtonAction Action
        { get; set; }

        public ButtonActionEventArgs(ButtonAction action, Machine machineData)
        {
            Action = action;
            MachineData = machineData;
        }
    }
}
