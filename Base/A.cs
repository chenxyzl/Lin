﻿using Message;

namespace Base
{
    public static class A
    {
        //可预料的错误 会把错误码返回客户端
        public static void Ensure(bool a, Code code, string des = null, bool serious = false)
        {
            if (a != true)
            {
                throw new CodeException(code, des ?? code.ToString(), serious);
            }
        }
        //可预料的错误 会把错误码返回客户端
        public static void Abort(Code code, string des = null, bool serious = false)
        {
            throw new CodeException(code, des ?? code.ToString(), serious);
        }

        //可预料的错误 会把错误码返回客户端
        public static T RequireNotNull<T>(T t, Code code, string des = null, bool serious = false)
        {
            if (t == null)
            {
                throw new CodeException(code, des ?? code.ToString(), serious);
            }
            return t;
        }
    }
}
