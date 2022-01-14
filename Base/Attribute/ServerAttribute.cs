﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = false)]
    public class ServerAttribute : BaseAttribute
    {
        //public RoleDef role { get; private set; }
        public ServerAttribute() { }
    }
}
