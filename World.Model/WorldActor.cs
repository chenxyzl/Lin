﻿using System;
using Akka.Actor;
using Base;
using Base.Helper;
using Message;

namespace World.Model
{
    public class WorldActor : BaseActor
    {

        public ulong WorldId { get; private set; }
        private ILog _log;
        public override ILog Logger { get { if (_log == null) { _log = new NLogAdapter($"world:{WorldId}"); } return _log; } }


        public WorldActor() : base()
        {
            GameAttrManager.Instance.Hotfix.AddComponent(this);
        }
        protected override async void PreStart()
        {
            base.PreStart();
            await GameAttrManager.Instance.Hotfix.Load(this);
            await GameAttrManager.Instance.Hotfix.Start(this, false);   
            EnterUpState();
        }


        protected override async void PostStop()
        {
            await GameAttrManager.Instance.Hotfix.PreStop(this);
            await GameAttrManager.Instance.Hotfix.Stop(this);
            base.PostStop();
        }
        async void Tick(long now)
        {
            await GameAttrManager.Instance.Hotfix.Tick(this, now);
        }
        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case TickT tik:
                    {
                        var now = TimeHelper.Now();
                        Tick(now);
                        break;
                    }
                case ReceiveTimeout m:
                    {
                        ElegantStop();
                        break;
                    }
                case InnerRequest request:
                    {
                        RpcManager.Instance.InnerHandlerDispatcher.Dispatcher(this, request);
                        break;
                    }
            }
        }
    }
}
