using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BendSheets.PVICommunication
{
    public static class ControllerVariables
    {
        public const string CURRENT_PART = "currentPart";
        public const string RUNNING_PART = "runningPart";
        public const string REFRESH_CURRENT_STEPS = "refreshCurrentSteps";
        public const string BTN_GET_PRODUCTION_DATA = "btnGetProductionData";
        public const string BTN_GET_MASTER_DATA = "btnGetMasterData";
        public const string BTN_SEND_PRODUCTION_DATA = "btnSendProductionData";
    }

    public static class CurrentPartVariables
    {
        public const string CUSTOMER_NAME = "currentPart.customerName";
        public const string CUSTOMER_PN = "currentPart.customerPN";
        public const string CUSTOMER_REVISION = "currentPart.customerRevision";
        public const string CABLE_PART_NUMBER = "currentPart.cablePartNumber";
        public const string CABLE_DESCRIPTION = "currentPart.cableDescription";
        public const string CABLE_DIAMETER = "currentPart.cableDiameter";
        public const string CONVERSION_FACTOR = "currentPart.conversionFactor";
        public const string DEVELOP_LENGTH_MM = "currentPart.developeLengthMM";
        public const string DEVELOP_LENGTH_FT = "currentPart.developeLengthFt";
        public const string DEVELOP_LENGTH_IN = "currentPart.developeLengthIn";

        public static ReadOnlyCollection<string> PartList
        {
            get
            {
                List<string> _partList = new List<string>();
                _partList.Add(CUSTOMER_NAME);
                _partList.Add(CUSTOMER_PN);
                _partList.Add(CUSTOMER_REVISION);
                _partList.Add(CABLE_DESCRIPTION);
                _partList.Add(CABLE_PART_NUMBER);
                _partList.Add(CABLE_DIAMETER);
                _partList.Add(CONVERSION_FACTOR);
                _partList.Add(DEVELOP_LENGTH_MM);
                _partList.Add(DEVELOP_LENGTH_FT);
                _partList.Add(DEVELOP_LENGTH_IN);

                return new ReadOnlyCollection<string>(_partList);
            }
        }
    }

    public static class RunningPartVariables
    {
        public const string CUSTOMER_NAME = "runningPart.customerName";
        public const string CUSTOMER_PN = "runningPart.customerPN";
        public const string CUSTOMER_REVISION = "runningPart.customerRevision";
        public const string CABLE_PART_NUMBER = "runningPart.cablePartNumber";
        public const string CABLE_DESCRIPTION = "runningPart.cableDescription";
        public const string CABLE_DIAMETER = "runningPart.cableDiameter";
        public const string CONVERSION_FACTOR = "runningPart.conversionFactor";
        public const string DEVELOP_LENGTH_MM = "runningPart.developeLengthMM";
        public const string DEVELOP_LENGTH_FT = "runningPart.developeLengthFt";
        public const string DEVELOP_LENGTH_IN = "runningPart.developeLengthIn";
    }
}
