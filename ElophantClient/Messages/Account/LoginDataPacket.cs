using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluorineFx;
using FluorineFx.AMF3;
using ElophantClient.Flash;
using NotMissing;

namespace ElophantClient.Messages.Account
{
	[Message(".LoginDataPacket")]
	public class LoginDataPacket : BaseObject, ICloneable
	{
		public LoginDataPacket()
			: base(null)
		{
		}


		public LoginDataPacket(ASObject obj)
			: base(obj)
		{
			BaseObject.SetFields(this, obj);
		}

		[InternalName("allSummonerData")]
		public AllSummonerData AllSummonerData { get; set; }


		public object Clone()
		{
			return new LoginDataPacket
			{
				AllSummonerData = AllSummonerData.CloneT(),
			};
		}
	}
}
