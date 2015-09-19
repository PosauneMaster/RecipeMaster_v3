using System;
using System.Collections.Generic;
using System.Text;
using RecipeMaster.DataMapping;
using BR.AN.PviServices;

namespace BendSheets.PVICommunication
{
    public class VariableManager
    {
        private PviEventArgs _eventArgs;

        public event EventHandler<PviEventArgs> VariableChanged;
        public event EventHandler<PviEventArgs> VariablesConnected;
        public event EventHandler<GetDataEventArgs> SendProductionData;
        public event EventHandler<GetDataEventArgs> GetData;

        private void OnSendProductionData(Variable variable, CpuData cpu)
        {
            if (SendProductionData != null)
            {
                SendProductionData(variable, new GetDataEventArgs(cpu));
            }
        }

        private void OnVariableChanged(object sender, PviEventArgs e)
        {
            if (VariableChanged != null)
            {
                VariableChanged(sender, e);
            }
        }

        private void OnGetData(Variable variable, CpuData cpu)
        {
            if (GetData != null)
            {
                GetData(variable, new GetDataEventArgs(cpu));
            }
        }

        private void OnVariablesConnected()
        {
            if (VariablesConnected != null)
            {
                VariablesConnected(this, _eventArgs);
            }
        }

        public VariableManager() { }

        public void LoadVariables(Cpu cpu)
        {
            Initialize(cpu);
            OnVariablesConnected();
        }

        private void Initialize(Cpu cpu)
        {
            // Globals
            CreateGetData(cpu, ControllerVariables.BTN_GET_MASTER_DATA);
            CreateGetData(cpu, ControllerVariables.BTN_GET_PRODUCTION_DATA);
            CreateSendProductionData(cpu);

            CreateVariable(cpu, ControllerVariables.CURRENT_PART);
            CreateVariable(cpu, ControllerVariables.REFRESH_CURRENT_STEPS);
            CreateVariable(cpu, ControllerVariables.RUNNING_PART);

            // Current Part
            CreateVariable(cpu, CurrentPartVariables.CABLE_PART_NUMBER);
            CreateVariable(cpu, CurrentPartVariables.CABLE_DESCRIPTION);
            CreateVariable(cpu, CurrentPartVariables.CABLE_DIAMETER);
            CreateVariable(cpu, CurrentPartVariables.CONVERSION_FACTOR);
            CreateVariable(cpu, CurrentPartVariables.CUSTOMER_NAME);
            CreateCustomerPart(cpu);
            CreateVariable(cpu, CurrentPartVariables.CUSTOMER_REVISION);
            CreateVariable(cpu, CurrentPartVariables.DEVELOP_LENGTH_FT);
            CreateVariable(cpu, CurrentPartVariables.DEVELOP_LENGTH_IN);
            CreateVariable(cpu, CurrentPartVariables.DEVELOP_LENGTH_MM);

            // Running Part
            CreateVariable(cpu, RunningPartVariables.CABLE_PART_NUMBER);
            CreateVariable(cpu, RunningPartVariables.CABLE_DESCRIPTION);
            CreateVariable(cpu, RunningPartVariables.CABLE_DIAMETER);
            CreateVariable(cpu, RunningPartVariables.CONVERSION_FACTOR);
            CreateVariable(cpu, RunningPartVariables.CUSTOMER_NAME);
            CreateVariable(cpu, RunningPartVariables.CUSTOMER_PN);
            CreateVariable(cpu, RunningPartVariables.CUSTOMER_REVISION);
            CreateVariable(cpu, RunningPartVariables.DEVELOP_LENGTH_FT);
            CreateVariable(cpu, RunningPartVariables.DEVELOP_LENGTH_IN);
            CreateVariable(cpu, RunningPartVariables.DEVELOP_LENGTH_MM);
        }

        public Variable CreateSendProductionData(Cpu cpu)
        {
            if (cpu == null)
            {
                throw new ArgumentException("Parameter Cpu is null");
            }

            Variable v = new Variable(cpu, ControllerVariables.BTN_SEND_PRODUCTION_DATA);
            v.UserTag = ControllerVariables.BTN_SEND_PRODUCTION_DATA;
            v.UserData = cpu.UserData;
            v.ValueChanged += new VariableEventHandler(SendProductionData_ValueChanged);
            v.Active = true;
            v.Connect();
            return v;
        }

        private void SendProductionData_ValueChanged(object sender, VariableEventArgs e)
        {
            Variable v = sender as Variable;
            if (v.Value)
            {
                SetFalse(v);
                OnSendProductionData(v, (CpuData)v.UserData);
            }
        }

        private static Variable CreateCustomerPart(Cpu cpu)
        {
            Variable v = new Variable(cpu, CurrentPartVariables.CUSTOMER_PN);
            v.UserTag = CurrentPartVariables.CUSTOMER_PN;
            v.UserData = cpu.UserData;
            v.Active = true;
            v.Connect();
            return v;
        }

        private Variable CreateGetData(Cpu cpu, string name)
        {
            Variable v = new Variable(cpu, name);
            v.UserTag = name;
            v.UserData = cpu.UserData;
            v.ValueChanged += new VariableEventHandler(GetData_ValueChanged);
            v.Connected += new PviEventHandler(v_Connected);
            v.Active = true;
            v.Connect();
            return v;
        }

        private void GetData_ValueChanged(object sender, VariableEventArgs e)
        {
            Variable v = sender as Variable;
            if (v.Value)
            {
                SetFalse(v);
                OnGetData(v, v.UserData as CpuData);
            }
        }

        private Variable CreateVariable(Cpu cpu, string name)
        {
            Variable v = new Variable(cpu, name);
            v.UserTag = name;
            v.UserData = cpu.UserData;
            v.ValueChanged += new VariableEventHandler(v_ValueChanged);
            v.Connected += new PviEventHandler(v_Connected);
            v.Active = true;
            v.Connect();
            return v;
        }

        private static void SetFalse(Variable variable)
        {
            variable.WriteValueAutomatic = false;
            variable.Value.Assign(false);
            variable.WriteValue();
        }

        private static bool IsResetVariable(Variable v)
        {
            return v.UserTag.Equals(ControllerVariables.BTN_GET_MASTER_DATA) ||
                   v.UserTag.Equals(ControllerVariables.BTN_GET_PRODUCTION_DATA);

        }

        private void v_ValueChanged(object sender, VariableEventArgs e)
        {
            Variable v = sender as Variable;
            if (IsResetVariable(v))
            {
                SetFalse(v);
            }

            OnVariableChanged(sender, e);
        }

        private void v_Connected(object sender, PviEventArgs e)
        {
            Variable v = sender as Variable;
            if (IsResetVariable(v))
            {
                SetFalse(v);
            }
            _eventArgs = e;
        }
    }
}
