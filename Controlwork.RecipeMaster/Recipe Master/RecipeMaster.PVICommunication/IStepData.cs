using System;
using System.Collections.Generic;
using System.Text;

namespace BendSheets.PVICommunication
{
    public interface IStepData
    {
        int StepNumber { get; set;}
        double BendAngle { get; set;}
        double BendRotation { get; set;}
        double StraightLength { get; set;}
        double BendRadius { get; set;}
        double BendOffset { get; set;}

        void MapData(object[] data);

    }
}
