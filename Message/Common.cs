using ProtoBuf;

namespace Message;

[ProtoContract]
public class Item : IMessage
{
    [ProtoMember(1)] public ulong Uid { get; set; }

    [ProtoMember(2)] public uint Tid { get; set; }

    [ProtoMember(3)] public ulong Count { get; set; }

    [ProtoMember(4)] public long GetTime { get; set; }
}

[ProtoContract]
public class Hero : IMessage
{
    [ProtoMember(1)] public ulong Tid { get; set; }

    [ProtoMember(2)] public uint Uid { get; set; }

    [ProtoMember(3)] public uint Exp { get; set; }
}

[ProtoContract]
public class Equip : IMessage
{
    [ProtoMember(1)] public ulong Tid { get; set; }

    [ProtoMember(2)] public uint Uid { get; set; }

    [ProtoMember(3)] public uint Exp { get; set; }
}