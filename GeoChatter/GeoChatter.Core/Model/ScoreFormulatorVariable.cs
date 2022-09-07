using GeoChatter.Core.Common.Extensions;

namespace GeoChatter.Core.Model
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    /// <summary>
    /// Score formulator variable model. All values evaluate to <see cref="ScoreFormulatorVariable"/> in expressions.
    /// </summary>
    public sealed class ScoreFormulatorVariable : System.IComparable
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        /// <summary>
        /// Represents boolean <see langword="true"/>
        /// </summary>
        public static ScoreFormulatorVariable True => new(1D);
        /// <summary>
        /// Represents boolean <see langword="false"/>
        /// </summary>
        public static ScoreFormulatorVariable False => new(0D);

        /// <summary>
        /// Numerical representation of the variable
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ScoreFormulatorVariable() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public ScoreFormulatorVariable(double value)
        {
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public ScoreFormulatorVariable(object value)
        {
            if (value is null)
            {
                Value = False;
            }
            else
            {
                bool _ = GetAsDouble(value, out double val);
                Value = val;
            }
        }

        /// <summary>
        /// Cast <paramref name="variable"/> as <see langword="double"/>. <c>0</c> when null
        /// </summary>
        /// <param name="variable"></param>
        public static implicit operator double(ScoreFormulatorVariable variable)
        {
            return variable is null ? 0D : variable.Value;
        }

        /// <summary>
        /// Cast <paramref name="variable"/> as <see langword="float"/>. <c>0</c> when null
        /// </summary>
        /// <param name="variable"></param>
        public static implicit operator float(ScoreFormulatorVariable variable)
        {
            return variable is null ? 0F : variable.Value.CastAsFloat();
        }

        /// <summary>
        /// Cast <paramref name="variable"/> as <see langword="long"/>. <c>0</c> when null
        /// </summary>
        /// <param name="variable"></param>
        public static implicit operator long(ScoreFormulatorVariable variable)
        {
            return variable is null ? 0L : variable.Value.CastAsLong();
        }

        /// <summary>
        /// Cast <paramref name="variable"/> as <see langword="int"/>. <c>0</c> when null
        /// </summary>
        /// <param name="variable"></param>
        public static implicit operator int(ScoreFormulatorVariable variable)
        {
            return variable is null ? 0 : variable.Value.CastAsInt();
        }

        /// <summary>
        /// Cast <paramref name="variable"/> as <see langword="bool"/>. <see langword="false"/> when null
        /// </summary>
        /// <param name="variable"></param>
        public static implicit operator bool(ScoreFormulatorVariable variable)
        {
            return variable is not null && variable.Equals(True);
        }

        /// <summary>
        /// Create a new variable
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ScoreFormulatorVariable(double value)
        {
            return new(value);
        }

        /// <summary>
        /// Create a new variable
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ScoreFormulatorVariable(float value)
        {
            return new(value);
        }

        /// <summary>
        /// Create a new variable
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ScoreFormulatorVariable(long value)
        {
            return new(value);
        }

        /// <summary>
        /// Create a new variable
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ScoreFormulatorVariable(int value)
        {
            return new(value);
        }

        /// <summary>
        /// Create a new variable
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ScoreFormulatorVariable(bool value)
        {
            return new(value ? True : False);
        }

        /// <summary>
        /// Get <paramref name="obj"/> as a <see langword="double"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool GetAsDouble(object obj, out double value)
        {
            value = False;
            if (obj is double d)
            {
                value = d;
            }
            else if (obj is ScoreFormulatorVariable sf)
            {
                value = sf.Value;
            }
            else if (obj is null)
            {
                return false;
            }
            else if (obj is float f)
            {
                value = f;
            }
            else if (obj is decimal dc)
            {
                value = decimal.ToDouble(dc);
            }
            else if (obj is long l)
            {
                value = l;
            }
            else if (obj is int i)
            {
                value = i;
            }
            else if (obj is bool b)
            {
                value = b ? True : False;
            }
            else if (obj is byte by)
            {
                value = by;
            }
            else if (obj is char c)
            {
                value = c;
            }
            else if (obj is string s)
            {
                if (s.TryParseDoubleDefault(out double sd))
                {
                    value = sd;
                }
                else
                {
                    value = !string.IsNullOrWhiteSpace(s) ? True : False;
                    return false;
                }
            }
            else
            {
                value = True;
            }
            return true;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Value.ToStringDefault();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return GetAsDouble(obj, out double num) && Value == num;
        }

        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            return GetAsDouble(obj, out double val)
                ? val == Value
                        ? 0
                        : val < Value
                            ? 1
                            : -1
                : 1;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator ==(ScoreFormulatorVariable v, object d)
        {
            return (v is null && d is null) || (v is not null && v.Equals(d)) ? True : False;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator ==(object d, ScoreFormulatorVariable v)
        {
            return v == d;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator ==(ScoreFormulatorVariable d, ScoreFormulatorVariable v)
        {
            return (v is null && d is null) || (v is not null && v.Equals(d)) ? True : False;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator !=(ScoreFormulatorVariable v, object d)
        {
            return new(!(v == d));
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator !=(object d, ScoreFormulatorVariable v)
        {
            return v != d;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator !=(ScoreFormulatorVariable d, ScoreFormulatorVariable v)
        {
            return new(!(v == d));
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator <=(ScoreFormulatorVariable v, object d)
        {
            return v is not null && v.CompareTo(d) <= 0 ? True : False;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator <=(object d, ScoreFormulatorVariable v)
        {
            return v <= d;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator <=(ScoreFormulatorVariable d, ScoreFormulatorVariable v)
        {
            return v is not null && v.CompareTo(d) <= 0 ? True : False;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator <(ScoreFormulatorVariable v, object d)
        {
            return v is not null && v.CompareTo(d) < 0 ? True : False;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator <(object d, ScoreFormulatorVariable v)
        {
            return v < d;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator <(ScoreFormulatorVariable d, ScoreFormulatorVariable v)
        {
            return v is not null && v.CompareTo(d) < 0 ? True : False;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator >=(ScoreFormulatorVariable v, object d)
        {
            return v < d ? False : True;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator >=(object d, ScoreFormulatorVariable v)
        {
            return v < d ? False : True;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator >=(ScoreFormulatorVariable d, ScoreFormulatorVariable v)
        {
            return v < d ? False : True;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator >(ScoreFormulatorVariable v, object d)
        {
            return v <= d ? False : True;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator >(object d, ScoreFormulatorVariable v)
        {
            return v <= d ? False : True;
        }

        /// <summary>
        /// Compares <see cref="Value"/>
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator >(ScoreFormulatorVariable d, ScoreFormulatorVariable v)
        {
            return v <= d ? False : True;
        }

        /// <summary>
        /// Operates with <see cref="Value"/>.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator +(ScoreFormulatorVariable v, object d)
        {
            return v is null ? new ScoreFormulatorVariable(d) : GetAsDouble(d, out double val) ? new(v.Value + val) : v;
        }

        /// <summary>
        /// Operates with <see cref="Value"/>.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator +(object d, ScoreFormulatorVariable v)
        {
            return v is null ? new ScoreFormulatorVariable(d) : GetAsDouble(d, out double val) ? new(val + v.Value) : new(val);
        }

        /// <summary>
        /// Operates with <see cref="Value"/>.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator +(ScoreFormulatorVariable d, ScoreFormulatorVariable v)
        {
            if (d is null)
            {
                return new(v);
            }
            else if (v is null)
            {
                return new(d);
            }
            return new(d.Value + v.Value);
        }

        /// <summary>
        /// Operates with <see cref="Value"/>.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator -(ScoreFormulatorVariable v, object d)
        {
            return v is null ? new ScoreFormulatorVariable(d) * -1 : GetAsDouble(d, out double val) ? new(v.Value - val) : v;
        }

        /// <summary>
        /// Operates with <see cref="Value"/>.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator -(object d, ScoreFormulatorVariable v)
        {
            return v is null ? (new(d)) : GetAsDouble(d, out double val) ? new(val - v.Value) : new(val);
        }

        /// <summary>
        /// Operates with <see cref="Value"/>.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator -(ScoreFormulatorVariable d, ScoreFormulatorVariable v)
        {
            if (v is null)
            {
                return new(d);
            }
            else if (d is null)
            {
                return new ScoreFormulatorVariable(v) * -1;
            }
            return new(d.Value - v.Value);
        }

        /// <summary>
        /// Operates with <see cref="Value"/>.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator *(ScoreFormulatorVariable v, object d)
        {
            return v is null ? False : GetAsDouble(d, out double val) ? new(v.Value * val) : v;
        }

        /// <summary>
        /// Operates with <see cref="Value"/>.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator *(object d, ScoreFormulatorVariable v)
        {
            return v is null ? False : GetAsDouble(d, out double val) ? new(val * v.Value) : new(val);
        }

        /// <summary>
        /// Operates with <see cref="Value"/>.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator *(ScoreFormulatorVariable d, ScoreFormulatorVariable v)
        {
            return v is null || d is null ? False : (new(d.Value * v.Value));
        }

        /// <summary>
        /// Operates with <see cref="Value"/>.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator /(ScoreFormulatorVariable v, object d)
        {
            return v is null ? False : GetAsDouble(d, out double val) ? new(v.Value / val) : v;
        }

        /// <summary>
        /// Operates with <see cref="Value"/>.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator /(object d, ScoreFormulatorVariable v)
        {
            return v is null ? False : GetAsDouble(d, out double val) ? new(val / v.Value) : new(val);
        }

        /// <summary>
        /// Operates with <see cref="Value"/>.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator /(ScoreFormulatorVariable d, ScoreFormulatorVariable v)
        {
            return v is null || d is null ? False : (new(d.Value / v.Value));
        }

        /// <summary>
        /// Operates with <see cref="Value"/>.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator %(ScoreFormulatorVariable v, object d)
        {
            return v is null ? False : GetAsDouble(d, out double val) ? new(v.Value % val) : v;
        }

        /// <summary>
        /// Operates with <see cref="Value"/>.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator %(object d, ScoreFormulatorVariable v)
        {
            return v is null ? False : GetAsDouble(d, out double val) ? new(val % v.Value) : new(val);
        }

        /// <summary>
        /// Operates with <see cref="Value"/>.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static ScoreFormulatorVariable operator %(ScoreFormulatorVariable d, ScoreFormulatorVariable v)
        {
            return v is null || d is null ? False : (new(d.Value % v.Value));
        }
    }
}
