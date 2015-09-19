using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RecipeMaster.DataMapping;
using WH.Utils.Logging;
using BR.AN.PviServices;

namespace BendSheets.PVICommunication
{
    /// <summary>
    /// Represents the data from the PVI Control
    /// </summary>
    public class RunningPart : BendSheetPart
    {
        private const int MAX_STEPS = 50;

        public RunningPart() { }

        /// <summary>
        /// Maps the data from the Variable to BendsheetData
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="bendData"></param>
        /// <returns></returns>
        public static void MapBendSheetData(Cpu cpu, BendSheetData bendData)
        {
            try
            {
                if (cpu == null)
                {
                    throw new ArgumentException("Parameter Cpu is null");
                }
                if (bendData == null)
                {
                    throw new ArgumentException("Parameter bendData is null");
                }

                bendData.AutoBendSheet = CreateStepData(cpu.Variables[ControllerVariables.RUNNING_PART]);

                bendData.Customer = cpu.Variables[RunningPartVariables.CUSTOMER_NAME].Value;
                bendData.CablePN = cpu.Variables[RunningPartVariables.CABLE_PART_NUMBER].Value;
                bendData.CableDescription = cpu.Variables[RunningPartVariables.CABLE_DESCRIPTION].Value;
                bendData.CableDiameter = cpu.Variables[RunningPartVariables.CABLE_DIAMETER].Value;
                bendData.ConversionFactor = cpu.Variables[RunningPartVariables.CONVERSION_FACTOR].Value;
                bendData.CustomerPartNumber = cpu.Variables[RunningPartVariables.CUSTOMER_PN].Value;
                bendData.CustomerRevisionLevel = cpu.Variables[RunningPartVariables.CUSTOMER_REVISION].Value;
            }
            catch (System.Exception ex)
            {
                Thread t = new Thread(new ParameterizedThreadStart(Log.Write));
                t.Start(ex);
                t.Join();
            }
        }

        private static object[,] CreateStepData(Variable variable)
        {
            object[,] autoBendSheet = new object[MAX_STEPS, 8];

            for (int i = 0; i < MAX_STEPS; i++)
             //for (int i = 1; i < bendData.AutoBendSheet.GetUpperBound(0); i++)

            {
                autoBendSheet[i, 0] = variable.Value[StepNumber(i)].ToString();
                autoBendSheet[i, 1] = variable.Value[StraightLength(i)].ToString();
                autoBendSheet[i, 2] = variable.Value[BendAngle(i)].ToString();
                autoBendSheet[i, 4] = variable.Value[BendRotation(i)].ToString();
                autoBendSheet[i, 6] = variable.Value[BendRaduis(i)].ToString();
                autoBendSheet[i, 7] = variable.Value[BendOffset(i)].ToString();
            }

            return autoBendSheet;
        }
    }
}
