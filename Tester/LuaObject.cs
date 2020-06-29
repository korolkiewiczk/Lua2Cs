using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Tester
{
    public class LuaObject : DynamicObject
    {
        private readonly Dictionary<object, object> _dictionary;

        public LuaObject()
        {
            _dictionary = new Dictionary<object, object>();
        }

        public LuaObject(Dictionary<object, object> dictionary)
        {
            _dictionary = dictionary;
        }

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            return _dictionary.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name] = value;

            return true;
        }

        public object this[object key]
        {
            get { return _dictionary[key]; }
            set { _dictionary[key] = value; }
        }

        public int Count()
        {
            return _dictionary.Count;
        }
        
        #region Operators
        public static dynamic operator |(LuaObject larg, dynamic rarg)
        {
            if (larg == null) return rarg;
            return larg;
        }
        public static dynamic operator |(dynamic larg, LuaObject rarg)
        {
            if (larg == null) return rarg;
            return larg;
        }
        
        public static bool operator true(LuaObject arg)
        {
            return false;
        }

        public static bool operator false(LuaObject arg)
        {
            return false;
        }
        #endregion

        #region Std methods
        protected dynamic assert(params object[] args)
        {
            return args.Length > 0 ? (dynamic) args[0] : (dynamic) null;
        }

        protected void print(params object[] args)
        {
            Console.Out.WriteLine(args);
        }
        #endregion
    }
}