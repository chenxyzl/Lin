﻿using Base;
using Base.Helper;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BootLogin
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Game.Boot(RoleDef.Login, typeof(Login.Model.Login));
        }
    }
}