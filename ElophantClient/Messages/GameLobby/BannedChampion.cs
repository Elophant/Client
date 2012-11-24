using System;
using System.Diagnostics;
using FluorineFx;
using ElophantClient.Flash;
using NotMissing;

namespace ElophantClient.Messages.GameLobby
{   
    public class BannedChampion : BaseObject, ICloneable
    {
        public BannedChampion()
            : base(null)
        {
        }
        public BannedChampion(ASObject body)
            : base(body)
        {
            BaseObject.SetFields(this, body);
        }

        [InternalName("pickTurn")]
        public int PickTurn { get; set; }

        [InternalName("dataVersion")]
        public object DataVersion { get; set; }

        [InternalName("championId")]
        public int ChampionId { get; set; }

        [InternalName("teamId")]
        public int TeamId { get; set; }

        [InternalName("futureData")]
        public object FutureData { get; set; }

        public object Clone()
        {
            return new BannedChampion
            {
            };
        }
    }
}
