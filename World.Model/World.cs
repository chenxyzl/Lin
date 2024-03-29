﻿using Base;
using Common;
using Share.Model.Component;

namespace World.Model;

public class World : GameServer
{
    public World(ushort nodeId) : base(RoleType.World, nodeId)
    {
    }

    public new static World Instance => A.NotNull(_ins as World);

    protected override void RegisterComponent()
    {
        GameServer.Instance.AddComponent<DBComponent>("mongodb://admin:Qwert123!@10.7.69.214:27017");
    }
}