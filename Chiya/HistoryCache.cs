using Chiya.Bancho;
using System.Runtime.Caching;

namespace Chiya
{
	public static class HistoryCache
	{
		public static MemoryCache cache = new MemoryCache("HistoryCache");
		public static BanchoUserHistory Get(string name) => cache[name] as BanchoUserHistory;
		public static void Init(string name) => cache[name] = new BanchoUserHistory();
	}
}
