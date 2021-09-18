﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class HandlerMethodAttribute : Attribute
    {
        //public RoleDef role { get; private set; }
        public HandlerMethodAttribute(uint rpcId)
        {
            A.Ensure(!GlobalParam.RpcIdRange.In(rpcId), Message.Code.Error, $"rpc id{rpcId} range error");
        }
    }
}