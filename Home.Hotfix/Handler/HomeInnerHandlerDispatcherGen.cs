﻿using System;
using System.Threading.Tasks;
using Base;
using Base.Serialize;
using Home.Model;
using Message;

namespace Home.Hotfix.Handler;

public partial class HomeInnerHandlerDispatcher
{
    public async Task<IResponse> DispatcherWithResult(BaseActor actor, RequestPlayer message)
    {
        var player = A.NotNull(actor as PlayerActor);
        switch (message.Opcode)
        {
            case 10000:
                return await HomeLoginHandler.LoginKeyHandler(player,
                    SerializeHelper.FromBinary<AHPlayerLoginKeyAsk>(message.Content));
        }

        A.Abort(Code.Error, $"opcode:{message.Opcode} not found", true);
        //只是为了编译过
        throw new Exception();
    }

    public Task DispatcherNoResult(BaseActor actor, RequestPlayer message)
    {
        var player = A.NotNull(actor as PlayerActor);

        A.Abort(Code.Error, $"opcode:{message.Opcode} not found", true);
        return Task.CompletedTask;
    }
}