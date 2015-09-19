using System;

namespace BendSheets
{
    [Serializable]
    public class RecipeVariable
    {
        public object Value
        { get; set; }

        public object[,] ValueArray
        { get; set; }

        public RecipeType RecipeType
        { get; private set; }

        public string Name
        { get; private set; }

        public string CellMap
        { get; set; }

        public RecipeVariable() { }

        public RecipeVariable(string name)
        {
            Name = name;
            RecipeType = RecipeType.RecipeImplicit;
            Value = new object();
        }

        public RecipeVariable(string name, object variable)
        {
            Name = name;
            RecipeType = RecipeType.RecipeImplicit;
            Value = variable;
        }

        public RecipeVariable(string name, object[,] variable)
        {
            Name = name;
            RecipeType = RecipeType.RecipeArray;
            ValueArray = (object[,])variable.Clone();
        }

        public static implicit operator int(RecipeVariable recipeVariable)
        {
            if (recipeVariable.RecipeType == RecipeType.RecipeImplicit)
            {
                return recipeVariable.Value is int ? (int)recipeVariable.Value : 0;
            }
            return 0;
        }

        public static implicit operator double(RecipeVariable recipeVariable)
        {
            if (recipeVariable.RecipeType == RecipeType.RecipeImplicit)
            {
                return recipeVariable.Value is double ? (double)recipeVariable.Value : 0.00d;
            }
            return 0.00d;
        }

        public static implicit operator string(RecipeVariable recipeVariable)
        {
            if (recipeVariable.RecipeType == RecipeType.RecipeImplicit)
            {
                return recipeVariable.Value is string ? (string)recipeVariable.Value : String.Empty;
            }
            return String.Empty;
        }

        public static implicit operator bool(RecipeVariable recipeVariable)
        {
            if (recipeVariable.RecipeType == RecipeType.RecipeImplicit)
            {
                return recipeVariable.Value is bool ? (bool)recipeVariable.Value : false;
            }
            return false;
        }
    }

    public enum RecipeType
    {
        RecipeArray,
        RecipeImplicit
    }
}
