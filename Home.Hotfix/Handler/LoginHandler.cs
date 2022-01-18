﻿using Base.Helper;
using Home.Model;
using Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home.Hotfix.Handler
{
    public static class LoginHandler
    {
        public static Task<S2CPong> Ping(PlayerActor player, C2SPing ping)
        {
            return Task.FromResult(new S2CPong { Time = TimeHelper.Now() });
        }
        public static Task NotifyTest(PlayerActor player, CNotifyTest msg)
        {
            return Task.CompletedTask;
        }
    }
}
