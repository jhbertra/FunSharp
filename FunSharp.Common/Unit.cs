using System;

namespace FunSharp.Common
{

    //--------------------------------------------------
    /// <summary>
    /// Trivial datatype to express a lack of significance.
    /// </summary>
    /// <remarks>
    /// Serves as a type-safe alternative to void. 
    /// </remarks>
    [Serializable]
    public struct Unit : IEquatable<Unit>, IComparable<Unit>, IAppendable<Unit>
    {

        //**************************************************
        //* Methods
        //**************************************************

        //--------------------------------------------------
        /// <inheritdoc />
        public bool Equals(Unit other) => true;

        
        //--------------------------------------------------
        /// <inheritdoc />
        public int CompareTo(Unit other) => 0;
        
        
        //--------------------------------------------------
        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Unit;

        
        //--------------------------------------------------
        /// <inheritdoc />
        public override int GetHashCode() => this.GetType().GetHashCode();

        
        //--------------------------------------------------
        /// <inheritdoc />
        public Unit Append(Unit t) => default;

        
        //--------------------------------------------------
        /// <inheritdoc />
        public override string ToString() => "()";


        //**************************************************
        //* Operators
        //**************************************************
        
        public static bool operator ==(Unit left, Unit right) => true;

        public static bool operator !=(Unit left, Unit right) => false;

        public static bool operator <(Unit left, Unit right) => false;

        public static bool operator >(Unit left, Unit right) => false;

        public static bool operator <=(Unit left, Unit right) => true;

        public static bool operator >=(Unit left, Unit right) => true;

        public static Unit operator +(Unit left, Unit right) => left.Append(right);

    }

}