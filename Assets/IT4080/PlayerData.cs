using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace It4080
{
    public struct PlayerData : INetworkSerializable, System.IEquatable<PlayerData> {
        public ulong clientId;
        public Color color;
        public bool isReady;

        public PlayerData(ulong id, Color c, bool ready = true) {
            clientId = id;
            color = c;
            isReady = ready;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref color);
            serializer.SerializeValue(ref isReady);
        }

        public bool Equals(PlayerData other) {
            return other.clientId == clientId;
        }
    }
}
