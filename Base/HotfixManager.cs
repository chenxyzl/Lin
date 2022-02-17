﻿using System.Collections.Generic;
using Base.Alg;
using Base.Helper;

namespace Base;

public class HotfixManager : Single<HotfixManager>
{
    private UnOrderMultiMapSet<Type, Type> types = new();

    //加载程序集
    public void Reload()
    {
        var t = new UnOrderMultiMapSet<Type, Type>();
        var asm = DllHelper.GetHotfixAssembly(GameServer.Instance);
        foreach (var x in asm)
        foreach (var type in x.GetTypes())
        {
            if (type.IsAbstract) continue;
            var objects = type.GetCustomAttributes(typeof(BaseAttribute), true);
            if (objects.Length == 0) continue;

            foreach (BaseAttribute baseAttribute in objects) t.Add(baseAttribute.AttributeType, type);
        }

        //新旧覆盖 ~~
        types = t;
        //
        GlobalHotfixManager.Instance.ReloadHandler();
        //重新加载Handler
        RpcManager.Instance.ReloadHandler();
        //
        PlayerHotfixManager.Instance.ReloadHanlder();
        //
        GameHotfixManager.Instance.ReloadHandler();
        //
        HttpHotfixManager.Instance.ReloadHanlder();
    }

    public HashSet<Type> GetTypes<T>() where T : BaseAttribute
    {
        return types[typeof(T)];
    }

    public HashSet<Type> GetServers()
    {
        return GetTypes<ServerAttribute>();
    }
}