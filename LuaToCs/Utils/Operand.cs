using System;

namespace LuaToCs.Utils
{
    public class Operand
    {
        private bool logical;

        public static implicit operator Operand(string value)
        {
            return new StringLiteral(value);
        }

        public static implicit operator Operand(bool value)
        {
            return new IntLiteral(value ? 1 : 0);
        }

        public static implicit operator Operand(byte value)
        {
            return new IntLiteral(value);
        }

        public static implicit operator Operand(sbyte value)
        {
            return new IntLiteral(value);
        }

        public static implicit operator Operand(short value)
        {
            return new IntLiteral(value);
        }

        public static implicit operator Operand(ushort value)
        {
            return new IntLiteral(value);
        }

        public static implicit operator Operand(char value)
        {
            return new IntLiteral(value);
        }

        public static implicit operator Operand(int value)
        {
            return new IntLiteral(value);
        }

        public static implicit operator Operand(uint value)
        {
            return new IntLiteral(unchecked((int) value));
        }

        public static implicit operator Operand(long value)
        {
            return new LongLiteral(value);
        }

        public static implicit operator Operand(ulong value)
        {
            return new LongLiteral(unchecked((long) value));
        }

        public static implicit operator Operand(float value)
        {
            return new FloatLiteral(value);
        }

        public static implicit operator Operand(double value)
        {
            return new DoubleLiteral(value);
        }

        public static implicit operator Operand(decimal value)
        {
            return new DecimalLiteral(value);
        }

        public static implicit operator Operand(Enum value)
        {
            return new EnumLiteral(value);
        }

        public static Operand operator +(Operand left, Operand right)
        {
            return new OverloadableOperation(Operator.Add, left, right);
        }

        public static Operand operator -(Operand left, Operand right)
        {
            return new OverloadableOperation(Operator.Subtract, left, right);
        }

        public static Operand operator *(Operand left, Operand right)
        {
            return new OverloadableOperation(Operator.Multiply, left, right);
        }

        public static Operand operator /(Operand left, Operand right)
        {
            return new OverloadableOperation(Operator.Divide, left, right);
        }

        public static Operand operator %(Operand left, Operand right)
        {
            return new OverloadableOperation(Operator.Modulus, left, right);
        }

        public static Operand operator &(Operand left, Operand right)
        {
            return new OverloadableOperation(Operator.And, left, right);
        }
        
        public static Operand operator <(Operand left, Operand right)
        {
            return new OverloadableOperation(Operator.LessThan, left, right);
        }

        public static Operand operator >(Operand left, Operand right)
        {
            return new OverloadableOperation(Operator.GreaterThan, left, right);
        }

        public static Operand operator >=(Operand left, Operand right)
        {
            return new OverloadableOperation(Operator.GreaterThanOrEqual, left, right);
        }

        public static Operand operator <=(Operand left, Operand right)
        {
            return new OverloadableOperation(Operator.LessThanOrEqual, left, right);
        }

        public static Operand operator |(Operand left, Operand right)
        {
            if (left != null && left.logical)
            {
                left.logical = false;
                return left.LogicalOr(right);
            }

            return new OverloadableOperation(Operator.Or, left, right);
        }
        
        public static Operand operator ^(Operand left, Operand right)
        {
            return new ExpOperator(left, right);
        }

        public static Operand operator <<(Operand left, int right)
        {
            return new OverloadableOperation(Operator.LeftShift, left, right);
        }

        public Operand LeftShift(Operand value)
        {
            return new OverloadableOperation(Operator.LeftShift, this, value);
        }

        public static Operand operator >>(Operand left, int right)
        {
            return new OverloadableOperation(Operator.RightShift, left, right);
        }

        public Operand RightShift(Operand value)
        {
            return new OverloadableOperation(Operator.RightShift, this, value);
        }

        public static bool operator true(Operand op)
        {
            if (op != null)
                op.logical = true;
            return false;
        }

        public static bool operator false(Operand op)
        {
            if (op != null)
                op.logical = true;
            return false;
        }
        
        public static Operand operator +(Operand op)
        {
            return new OverloadableOperation(Operator.Plus, op);
        }

        public static Operand operator -(Operand op)
        {
            return new OverloadableOperation(Operator.Minus, op);
        }
        
        public static Operand operator !(Operand op)
        {
            return new OverloadableOperation(Operator.LogicalNot, op);
        }

        public static Operand operator ~(Operand op)
        {
            return new OverloadableOperation(Operator.Not, op);
        }

        public Operand LogicalOr(Operand other)
        {
            return new OverloadableOperation(Operator.Or, this, other);
        }

        public Assignment Assign(Operand value, bool scoped)
        {
            return Assign(value, false, scoped);
        }

        public Assignment Assign(Operand value, bool local, bool scoped)
        {
            return new Assignment(this, value, local, scoped);
        }

        public Operand EQ(Operand value)
        {
            return new OverloadableOperation(Operator.Equality, this, value);
        }

        public Operand NE(Operand value)
        {
            return new OverloadableOperation(Operator.Inequality, this, value);
        }

        public Operand ArrayLength()
        {
            return new ArrayLength(this);
        }

        public Operand Dot(Operand rval)
        {
            return new DotOperation(this, rval);
        }
    }
}