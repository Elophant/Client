using System.Collections.Generic;
using FluorineFx;
using FluorineFx.AMF3;
using ElophantClient.Flash;

namespace ElophantClient.Messages.GameLobby
{
    public class BannedChampionList : List<BannedChampion>
    {
        protected readonly ArrayCollection Base;
        public BannedChampionList()
        {
        }
        public BannedChampionList(IEnumerable<BannedChampion> collection)
            : base(collection)
        {
        }

        public BannedChampionList(ArrayCollection thebase)
        {
            Base = thebase;
            if (Base == null)
                return;

            foreach (var item in Base)
            {
                Add(new BannedChampion(item as ASObject));
            }
        }
    }
}
