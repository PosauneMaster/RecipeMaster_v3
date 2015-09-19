using System;
using System.Collections.Generic;
using System.Text;
using RecipeMaster.DataMapping;
using BR.AN.PviServices;

namespace BendSheets.PVICommunication
{
    public interface IPVIValue
    {
        Dictionary<int, IStepData> BendInfo { get; }
        string CustomerName { get; set;}
        string CustomerPN { get; set;}
        string CustomerRevision { get; set;}
        string CablePartNumber { get; set;}
        string CableDescription { get; set;}
        double CableDiameter { get; set;}
        double ConversionFactor { get; set;}
        double DevelopeLengthMM { get; set;}
        double DevelopeLengthFt { get; set;}
        double DevelopeLengthIn { get; set;}

        Variable MapVariableData(Variable variable, BendSheetData bendData);
        BendSheetData MapBendSheetData(Variable pviVariable);        
    }
}
