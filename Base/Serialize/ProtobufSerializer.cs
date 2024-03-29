﻿using System.IO;
using Akka.Actor;
using Akka.Serialization;

namespace Base.Serialize;

/// <summary>
///     Generic Google Protocol Buffers serializer for Akka.NET using Marc Gravell protobuf-net.
/// </summary>
public sealed class ProtobufSerializer : Serializer
{
    public ProtobufSerializer(ExtendedActorSystem system) : base(system)
    {
    }

    public override bool IncludeManifest => false;

    public override int Identifier => 127;

    public override byte[] ToBinary(object obj)
    {
        using (var stream = new MemoryStream())
        {
            ProtoBuf.Serializer.Serialize(stream, obj);
            return stream.ToArray();
        }
    }

    public override object FromBinary(byte[] bytes, Type type)
    {
        using (var stream = new MemoryStream(bytes))
        {
            return ProtoBuf.Serializer.Deserialize(type, stream);
        }
    }
}