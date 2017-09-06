using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using WotDossier.Common;
using WotDossier.Common.Extensions;
using WotDossier.Domain.Replay;

namespace WotDossier.Applications.Parser
{
    public class Parser914 : Parser913
    {
        List<long> Ids = new List<long>();
        protected ulong CurrentVehicleId { get; set; }

        protected override ulong PacketVersion => 0x18;

        protected override ulong UpdateEvent_Slot
        {
            get { return 0x0a; }
        }

        protected override ulong UpdateEvent_Arena
        {
            get { return 0x29; }
        }

	    public override ulong UpdateEvent_Health => 0x04;

	    /// <summary>
		/// Process packet 0x00
		/// Contains Battle level setup and Player Name.
		/// </summary>
		/// <param name="packet">The packet.</param>
		public override void ProcessPacketBattleLevel(Packet packet)
        {
            packet.Type = PacketType.BattleLevel;

            dynamic data = new ExpandoObject();

            packet.Data = data;

            using (var f = new MemoryStream(packet.Payload))
            {
                f.Seek(14, SeekOrigin.Begin);

                int playernamelength = (int)f.Read(1).ConvertLittleEndian();

                data.playername = f.Read(playernamelength).GetUtf8String();

                f.Seek(playernamelength + 29, SeekOrigin.Begin);

                data.more = new BattleInfo();
                int advancedlength = (int)f.Read(1).ConvertLittleEndian();

                if (advancedlength == 255)
                {
                    advancedlength = (int)f.Read(2).ConvertLittleEndian();
                    f.Seek(1, SeekOrigin.Current);
                }

                try
                {
                    byte[] advancedPickles = f.Read(advancedlength);
                    object load = Unpickle.Load(new MemoryStream(advancedPickles));
                    data.more = load.ToObject<BattleInfo>();
                }
                catch (Exception e)
                {
                    _log.Error("Cannot load battle info pickle object. \nPosition: " + f.Position + ", Length: " + advancedlength, e);
                }
            }
        }

        /// <summary>
        /// Processes the player position.
        /// </summary>
        /// <param name="packet">The packet.</param>
        protected override void ProcessPacketPlayerPosition(Packet packet)
        {
            packet.Type = PacketType.PlayerPos;

            dynamic data = new ExpandoObject();

            packet.Data = data;

            using (MemoryStream f = new MemoryStream(packet.Payload))
            {
                var playerId = f.Read(4).ConvertLittleEndian();

				packet.PlayerId = playerId;
                data.PlayerId = playerId;

                f.Seek(8, SeekOrigin.Begin);

                var pos1 = f.Read(4).ToSingle();
                var pos2 = f.Read(4).ToSingle();
                var pos3 = f.Read(4).ToSingle();
                data.position = new[] { pos1, pos2, pos3 };

                f.Seek(32, SeekOrigin.Begin);

                var hull1 = f.Read(4).ToSingle();
                var hull2 = f.Read(4).ToSingle();
                var hull3 = f.Read(4).ToSingle();
                data.hull_orientation = new[] { hull1, hull2, hull3 };
            }
        }
	    /*
			    protected override void ProcessPacketHealth(Packet packet)
			    {
				    packet.Type = PacketType.Health;

				    dynamic data = new ExpandoObject();

				    packet.Data = data;

				    using (MemoryStream stream = new MemoryStream(packet.Payload))
				    {
					    //read 0-4 - player_id
					    var playerId = stream.Read(4).ConvertLittleEndian();
					    if (playerId == CurrentVehicleId && CurrentVehicleId != 0)
						    playerId = ReplayWriterVehicle;

					    packet.PlayerId = playerId;
					    //read 4-8 - subType
					    packet.StreamSubType = stream.Read(4).ConvertLittleEndian();
					    //read 8-12 - update length
					    packet.SubTypePayloadLength = stream.Read(4).ConvertLittleEndian();

					    if (packet.StreamSubType == UpdateEvent_Health)
					    {
						    int value = BitConverter.ToInt16(stream.Read(2), 0);
						    data.health = value < 0 ? 0 : value;
					    }
				    }
			    }

			    /// <summary>
			    /// Process packet 0x08
			    /// Contains Various game state updates
			    /// </summary>
			    /// <param name="packet">The packet.</param>
			    protected override void ProcessPacketStateUpdate(Packet packet)
			    {
				    using (MemoryStream stream = new MemoryStream(packet.Payload))
				    {
					    //read 0-4 - player_id
					    var playerId = stream.Read(4).ConvertLittleEndian();
					    if (playerId != ReplayWriterVehicle)
					    {
						    CurrentVehicleId = playerId;
						    playerId = ReplayWriterVehicle;
					    }
					    packet.PlayerId = playerId;
					    //read 4-8 - subType
					    packet.StreamSubType = stream.Read(4).ConvertLittleEndian();
					    //read 8-12 - update length
					    packet.SubTypePayloadLength = stream.Read(4).ConvertLittleEndian();

					    if (packet.StreamSubType == UpdateEvent_Arena) //onArenaUpdate events
					    {
						    ProcessPacketStateArenaUpdate(packet, stream);
					    }

					    else if (packet.StreamSubType == UpdateEvent_Slot) //onSlotUpdate events
					    {
						    ProcessPacket_0x08_0x09(packet, stream);
					    }

					    else if (packet.StreamSubType == 0x01) //onDamageReceived
					    {
						    ProcessPacket_0x08_0x01(packet, stream);//, data);
					    }
				    }
			    }
		    */
	}
}