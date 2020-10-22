using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Chiya.Bancho
{
	public class BanchoMessage
	{
		public BanchoMessage() { }
		public BanchoMessage(string message) => Message = message;
		public string Message;
	}
	public class ActionMessage : BanchoMessage
	{
		public ActionMessage(string message) => Message = message;
		public override string ToString()
		{
			return $"{Message}";
		}
	}
	public class UrlMessage : BanchoMessage
	{
		public UrlMessage(string message, string url)
		{
			Message = message;
			Url = url;
		}
		public string Url;
		public override string ToString()
		{
			return $"[{Url} {Message}]";
		}
	}
}
