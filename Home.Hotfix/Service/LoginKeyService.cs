using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Base;
using Base.Helper;
using Home.Model.Component;

namespace Home.Hotfix.Service;

public static class LoginKeyService
{
    public static Task Load(this LoginKeyComponent self)
    {
        return Task.CompletedTask;
    }

    public static Task Tick(this LoginKeyComponent self)
    {
        var now = TimeHelper.Now();
        while (true)
        {
            if (self.timeKeys.Count == 0) break;

            //因为正在登录中人数一定不多。所以这里lock写在while里。
            lock (self.lockObj)
            {
                var item = self.timeKeys.First();
                var t = IdGenerater.ParseTime(item.Key);
                if (t - now < 15_000) break;
                //开始处理超时
                self.timeKeys.Remove(item.Key);
                self.loginKeys.Remove(item.Value);
            }
        }

        if (now > self._lastConsoleTime + 60_000)
        {
            self._lastConsoleTime = now;
            GlobalLog.Info($"time:{now} active actor size:{self.loginKeys.Count} ");
        }

        return Task.CompletedTask;
    }

    public static string AddPlayerRef(this LoginKeyComponent self, IActorRef actor, string? lastLoginKey)
    {
        lock (self.lockObj)
        {
            //删除老的loginKey
            if (lastLoginKey != null)
                self.loginKeys.Remove(lastLoginKey);
            var playerRef = GameServer.Instance.system.ActorSelection(actor.Path.ToString());
            while (true)
            {
                var key = self.random.RandUInt64().ToString();
                if (self.loginKeys.ContainsKey(key)) continue;
                //不用去除老得key。因为会因为过期自动删除
                self.loginKeys.TryAdd(key, playerRef);
                //用id生成是避免重复，保留来时间信息,自增排序
                self.timeKeys.TryAdd(IdGenerater.GenerateId(), key);
                return key;
            }
        }
    }

    public static ulong GetUid(this ActorSelection self)
    {
        return ulong.Parse(self.PathString.Split("/").Last());
    }

    public static ActorSelection? CheckGetKey(this LoginKeyComponent self, string key)
    {
        lock (self.lockObj)
        {
            self.loginKeys.TryGetValue(key, out var v);
            return v;
        }
    }

    public static void RemoveLoginKey(this LoginKeyComponent self, string key)
    {
        lock (self.lockObj)
        {
            self.loginKeys.Remove(key);
        }
    }
}