using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace BendSheets
{
    public class VariableCollections
    {
        public ReadOnlyCollection<RecipeVariable> SendCollection
        {
            get;
            private set;
        }

        public ReadOnlyCollection<RecipeVariable> ReceiveCollection
        {
            get;
            private set;
        }

        public VariableCollections() { }

        public VariableCollections(IList<RecipeVariable> sendList, IList<RecipeVariable> receiveList)
        {
            SendCollection = new ReadOnlyCollection<RecipeVariable>(sendList);
            ReceiveCollection = new ReadOnlyCollection<RecipeVariable>(receiveList);
        }
    }
}
