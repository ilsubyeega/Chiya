using System;
using System.Collections.Generic;
using System.Text;

namespace Chiya.Bancho
{
	public enum BanchoBotParseType
	{
		JOIN,
		LEAVE,
		SLOT_MOVE,
		BEATMAP_CHANGING,
		BEATMAP_CHANGED,
		ALL_PLAYER_READY,
		MATCH_STARTED,
		MATCH_FINISHED,
		MATCH_FINISHEDSCORE
	}
}
