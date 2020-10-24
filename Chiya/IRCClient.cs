
using Meebey.SmartIrc4net;
using System;
using System.Threading;

namespace Chiya
{
	public partial class IRCClient
	{
		public string name;
		public string password;

		public IrcClient irc = new IrcClient();


		public void Connect()
		{
			irc.Encoding = System.Text.Encoding.UTF8;
			irc.SendDelay = 200;
			irc.ActiveChannelSyncing = true;
			RegisterEvents();

			string[] serverlist = new string[] { "irc.ppy.sh" };
			int port = 6667;
			try
			{
				irc.Connect(serverlist, port);
			}
			catch (ConnectionException e)
			{
				System.Console.WriteLine("couldn't connect! Reason: " + e.Message);
				return;
			}

			try
			{
				irc.Login(name, name, 4, name, password);
				irc.RfcJoin("#korean");
				StartLoop();
			}
			catch (Exception e)
			{
				// this should not happen by just in case we handle it nicely
				System.Console.WriteLine("Error occurred! Message: " + e.Message);
				System.Console.WriteLine("Exception: " + e.StackTrace);
				return;
			}
		}
		private void StartLoop()
		{

			new Thread(() =>
			{
				if (irc.IsConnected)
				{
					try
					{
						irc.Listen();
					} catch (Exception e)
					{
						Console.WriteLine("Connection dead.");
					}
					
					StartLoop(); // if thread dead, just do again uwu..
				}
				else
				{
					Console.WriteLine("The Connection are dead. Reconnecting...");
					try
					{
						irc.Reconnect();
					}
					catch (Exception e)
					{
						Console.WriteLine("Error while reconnecting the irc.\n" + e.Message);
					}
					finally
					{
						irc.Listen();
						StartLoop();
					}

				}


			}).Start();
		}
	}
}
