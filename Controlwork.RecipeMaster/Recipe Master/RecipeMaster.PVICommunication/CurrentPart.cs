using System;
using System.Collections.Generic;
using System.Text;
using RecipeMaster.DataMapping;
using BR.AN.PviServices;

namespace BendSheets.PVICommunication
{
    /// <summary>
    /// Represents the data from the PC to the PVI Control
    /// </summary>
    public class CurrentPart : BendSheetPart
    {
        private readonly string NA = "N/A";
        private readonly string FILE_NOT_FOUND = "File Not Found";
        #region Constructors

        public CurrentPart() { }

        #endregion

        /// <summary>
        /// Maps the Data from BendsheetData to the Variable
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="bendData"></param>
        /// <returns></returns>
        //public void MapVariableData(VariableManager vm, BendsheetData bendData)
        public void MapVariableData(Cpu cpu, BendSheetData bendData)
        {
            cpu.Variables[ControllerVariables.REFRESH_CURRENT_STEPS].Value.Assign(false);
            cpu.Variables[ControllerVariables.REFRESH_CURRENT_STEPS].WriteValue();

            CreateStepData(bendData);
            foreach (KeyValuePair<int, IStepData> pair in BendInfo)
            {
                MapStepData(cpu.Variables[ControllerVariables.CURRENT_PART], pair.Value);
            }

            cpu.Variables[ControllerVariables.CURRENT_PART].WriteValue();

            cpu.Variables[CurrentPartVariables.CUSTOMER_NAME].Value.Assign(bendData.Customer);
            cpu.Variables[CurrentPartVariables.CUSTOMER_NAME].WriteValue();

            cpu.Variables[CurrentPartVariables.CABLE_PART_NUMBER].Value.Assign(bendData.CablePN);
            cpu.Variables[CurrentPartVariables.CABLE_PART_NUMBER].WriteValue();

            cpu.Variables[CurrentPartVariables.CABLE_DESCRIPTION].Value.Assign(bendData.CableDescription);
            cpu.Variables[CurrentPartVariables.CABLE_DESCRIPTION].WriteValue();

            cpu.Variables[CurrentPartVariables.CABLE_DIAMETER].Value.Assign(bendData.CableDiameter);
            cpu.Variables[CurrentPartVariables.CABLE_DIAMETER].WriteValue();

            cpu.Variables[CurrentPartVariables.CUSTOMER_PN].Value.Assign(bendData.CustomerPartNumber);
            cpu.Variables[CurrentPartVariables.CUSTOMER_PN].WriteValue();

            cpu.Variables[CurrentPartVariables.CUSTOMER_REVISION].Value.Assign(bendData.CustomerRevisionLevel);
            cpu.Variables[CurrentPartVariables.CUSTOMER_REVISION].WriteValue();

            cpu.Variables[ControllerVariables.REFRESH_CURRENT_STEPS].Value.Assign(true);
            cpu.Variables[ControllerVariables.REFRESH_CURRENT_STEPS].WriteValue();
        }

        private static void MapStepData(Variable v, IStepData stepData)
        {
            v.Value[StepNumber(stepData.StepNumber - 1)].Assign(stepData.StepNumber);
            v.Value[BendAngle(stepData.StepNumber - 1)].Assign(stepData.BendAngle);
            v.Value[BendRotation(stepData.StepNumber - 1)].Assign(stepData.BendRotation);
            v.Value[StraightLength(stepData.StepNumber - 1)].Assign(stepData.StraightLength);
            v.Value[BendRaduis(stepData.StepNumber - 1)].Assign(stepData.BendRadius);
            v.Value[BendOffset(stepData.StepNumber - 1)].Assign(stepData.BendOffset);
        }

        private void CreateStepData(BendSheetData bendData)
        {
            int validRows = BendSheetDataUtils.CountRows(bendData.AutoBendSheet);
            _bendInfo = new Dictionary<int, IStepData>(bendData.AutoBendSheet.GetUpperBound(0));
            int step = 0;
            bendData.AutoBendSheet.GetUpperBound(0);
            for (int i = 1; i <= validRows; i++)
            {
                object[] o = new object[8] {
                    bendData.AutoBendSheet[i,1],
                    bendData.AutoBendSheet[i,2],
                    bendData.AutoBendSheet[i,3],
                    bendData.AutoBendSheet[i,4],
                    bendData.AutoBendSheet[i,5],
                    bendData.AutoBendSheet[i,6],
                    bendData.AutoBendSheet[i,7],
                    bendData.AutoBendSheet[i,8]
                };

                step++;
                BendInfo.Add(step, new StepData(o));
            }
        }

        public void FileNotFound(Cpu cpu)
        {
            if (cpu == null)
            {
                throw new ArgumentException("Parameter Cpu is null");
            }
            cpu.Variables[CurrentPartVariables.CUSTOMER_NAME].Value.Assign(NA);
            cpu.Variables[CurrentPartVariables.CUSTOMER_NAME].WriteValue();

            cpu.Variables[CurrentPartVariables.CABLE_PART_NUMBER].Value.Assign(NA);
            cpu.Variables[CurrentPartVariables.CABLE_PART_NUMBER].WriteValue();

            cpu.Variables[CurrentPartVariables.CABLE_DESCRIPTION].Value.Assign(FILE_NOT_FOUND);
            cpu.Variables[CurrentPartVariables.CABLE_DESCRIPTION].WriteValue();

            cpu.Variables[CurrentPartVariables.CABLE_DIAMETER].Value.Assign(0.00);
            cpu.Variables[CurrentPartVariables.CABLE_DIAMETER].WriteValue();

            cpu.Variables[CurrentPartVariables.CONVERSION_FACTOR].Value.Assign(0.00);
            cpu.Variables[CurrentPartVariables.CONVERSION_FACTOR].WriteValue();

            cpu.Variables[ControllerVariables.REFRESH_CURRENT_STEPS].Value.Assign(true);
            cpu.Variables[ControllerVariables.REFRESH_CURRENT_STEPS].WriteValue();
        }
    }
}
