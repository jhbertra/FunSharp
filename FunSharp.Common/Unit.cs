using System;

namespace FunSharp.Common
{

    [Serializable]
    public struct Unit : IEquatable<Unit>, IComparable<Unit>, IAppendable<Unit>
    {

        public bool Equals(Unit other) => true;

        public int CompareTo(Unit other) => 0;
        
        public override bool Equals(object obj) => obj is Unit;

        public override int GetHashCode() => this.GetType().GetHashCode();

        public Unit Append(Unit t) => default;

        public override string ToString() => "()";

        public static bool operator ==(Unit left, Unit right) => true;

        public static bool operator !=(Unit left, Unit right) => false;

        public static bool operator <(Unit left, Unit right) => false;

        public static bool operator >(Unit left, Unit right) => false;

        public static bool operator <=(Unit left, Unit right) => true;

        public static bool operator >=(Unit left, Unit right) => true;

        public static Unit operator +(Unit left, Unit right) => left.Append(right);

    }

}