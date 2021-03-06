using System.Collections.Generic;
using Base;
using Message;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace Home.Model.State;

[BsonCollectionName("Bag")]
public class BagState : IPlayerState
{
    [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
    public Dictionary<ulong, Item> Bag = new();
}