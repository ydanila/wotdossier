using System;

namespace WotDossier.Applications.Parser
{
    public class Packet
    {
        public PacketType Type { get; set; }
        public ulong StreamPacketType { get; set; }
        public ulong StreamSubType { get; set; }
        public ulong PlayerId { get; set; }
        public TimeSpan Time { get; set; }

        public long Offset { get; set; }
        public ulong PacketLength { get; set; }
        public ulong SubTypePayloadLength { get; set; }
        public byte[] Payload { get; set; }
        public float Clock { get; set; }
        public object Data { get; set; }
    }

	public enum PacketType
	{
		Unknown = 0,
		ChatMessage = 1,
		BattleLevel = 2,
		DamageReceived = 3,
		ArenaUpdate = 4,
		SlotUpdate = 5,
		Version = 6,
		PlayerPos = 7,
		MinimapClick = 8,
		Health = 9
	}
}