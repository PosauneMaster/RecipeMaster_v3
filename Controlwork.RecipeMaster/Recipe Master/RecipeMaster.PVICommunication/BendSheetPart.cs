using System;
using System.Collections.Generic;
using System.Text;
using RecipeMaster.DataMapping;
using BendSheets.Services;
using BR.AN.PviServices;

namespace BendSheets.PVICommunication
{
    public abstract class BendSheetPart
    {
        protected BendSheetPart() { }

        #region Private Variables

        protected Dictionary<int, IStepData> _bendInfo;
        protected string _customerName;
        protected string _customerPN;
        protected string _customerRevision;
        protected string _cablePartNumber;
        protected string _cableDescription;
        protected double _cableDiameter;
        protected double _conversionFactor;
        protected double _developeLengthMM;
        protected double _developeLengthFt;
        protected double _developeLengthIn;

        #endregion

        public Dictionary<int, IStepData> BendInfo
        {
            get { return _bendInfo; }
        }

        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value;}
        }

        public string CustomerPN
        {
            get { return _customerPN; }
            set {  _customerPN = value; }
        }

        public string CustomerRevision
        {
            get { return _customerRevision; }
            set { _customerRevision = value;}
        }

        public string CablePartNumber
        {
            get { return _cablePartNumber; }
            set {_cablePartNumber = value; }
        }

        public string CableDescription
        {
            get { return _cableDescription; }
            set { _cableDescription = value;}
        }

        public double CableDiameter
        {
            get { return _cableDiameter; }
            set { _cableDiameter = value; }
        }

        public double ConversionFactor
        {
            get { return _conversionFactor; }
            set { _conversionFactor = value; }
        }

        public double DevelopeLengthMM
        {
            get { return _developeLengthMM; }
            set { _developeLengthMM = value; }
        }

        public double DevelopeLengthFt
        {
            get { return _developeLengthFt; }
            set { _developeLengthFt = value; }
        }

        public double DevelopeLengthIn
        {
            get { return _developeLengthIn; }
            set { _developeLengthIn = value; }
        }

        public static string StepNumber(int index)
        {
            return String.Concat("currentPart.bendInfo[", index.ToString("N0", BendSheetsServices.Format), "].stepNumber");
        }

        public static string BendAngle(int index)
        {
            return String.Concat("currentPart.bendInfo[", index.ToString("N0", BendSheetsServices.Format), "].bendAngle");
        }

        public static string BendRotation(int index)
        {
            return String.Concat("currentPart.bendInfo[", index.ToString("N0", BendSheetsServices.Format), "].bendRotation");
        }

        public static string StraightLength(int index)
        {
            return String.Concat("currentPart.bendInfo[", index.ToString("N0", BendSheetsServices.Format), "].straightLength");
        }

        public static string BendRaduis(int index)
        {
            return String.Concat("currentPart.bendInfo[", index.ToString("N0", BendSheetsServices.Format), "].bendRadius");
        }

        public static string BendOffset(int index)
        {
            return String.Concat("currentPart.bendInfo[", index.ToString("N0", BendSheetsServices.Format), "].bendOffset");
        }
    }
}
