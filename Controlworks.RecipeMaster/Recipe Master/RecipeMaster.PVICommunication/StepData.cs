using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using BendSheets.Services;

namespace BendSheets.PVICommunication
{
    public class StepData : IStepData
    {
        #region Private Members

        private int _stepNumber;
        private double _bendAngle;
        private double _bendRotation;
        private double _straightLength;
        private double _bendRadius;
        private double _bendOffset;

        #endregion 

        #region IStepData Members

        public int StepNumber
        {
            get { return _stepNumber;}
            set { _stepNumber = value; }
        }

        public double BendAngle
        {
            get { return _bendAngle; }
            set { _bendAngle = value; }
        }

        public double BendRotation
        {
            get { return _bendRotation; }
            set { _bendRotation = value;}
        }

        public double StraightLength
        {
            get { return _straightLength; }
            set { _straightLength = value; }
        }

        public double BendRadius
        {
            get { return _bendRadius; }
            set { _bendRadius = value; }
        }

        public double BendOffset
        {
            get { return _bendOffset; }
            set { _bendOffset = value; }
        }

        #endregion

        #region Constructors
        public StepData(object[] data)
        {
            MapData(data);
        }
        #endregion

        public void MapData(object[] data)
        {
            if (data == null)
            {
                throw new ArgumentException("Invalid or null Step data");
            }

            if (data.Length < 8)
            {
                throw new ArgumentException("Invalid or null Step data");
            }
            _stepNumber = MapInt(data[0]);
            _bendAngle = MapDouble(data[2]);
            _bendRotation =MapAutoRotation(data[4],data[5]);
            _straightLength = MapDouble(data[1]);
            _bendRadius = MapDouble(data[6]);
            _bendOffset = MapDouble(data[7]);
        }

        private static int MapInt(object o)
        {
            if (o == null || String.IsNullOrEmpty(o.ToString()))
            {
                return 0;
            }
            else
            {
                return Utils.FindNumber(o);
            }
        }

        private static double MapDouble(object o)
        {
            if (o == null || String.IsNullOrEmpty(o.ToString()))
            {
                return 0.00;
            }
            else
            {
                return Convert.ToDouble(o, BendSheetsServices.Format);
            }
        }

        private static double MapAutoRotation(object rotation, object direction)
        {
            double d = 0.0;
            if (!double.TryParse(rotation.ToString(), out d))
            {
                return 0.0;
            }
            if (rotation == null || String.IsNullOrEmpty(rotation.ToString()))
            {
                return 0.0;
            }
            if (direction == null)
            {
                return d;
            }
            if (direction.ToString().ToUpper(CultureInfo.CurrentCulture).Trim().Equals("CW"))
            {
                return Math.Abs(Convert.ToDouble(rotation, BendSheetsServices.Format));
            }
            if (direction.ToString().ToUpper(CultureInfo.CurrentCulture).Trim().Equals("CCW"))
            {
                return Math.Abs(Convert.ToDouble(rotation, BendSheetsServices.Format)) * -1;
            }
            return 0.00;
        }
    }
}
