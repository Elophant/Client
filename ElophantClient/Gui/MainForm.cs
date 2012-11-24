/*
copyright (C) 2011-2012 by high828@gmail.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Db4objects.Db4o;
//using Db4objects.Db4o.Config;
//using Db4objects.Db4o.TA;
using FluorineFx;
using FluorineFx.AMF3;
using FluorineFx.IO;
using FluorineFx.Messaging.Messages;
using FluorineFx.Messaging.Rtmp.Event;
using ElophantClient.Messages.Account;
using ElophantClient.Messages.Champion;
using ElophantClient.Messages.Commands;
using ElophantClient.Messages.GameLobby;
using ElophantClient.Messages.GameLobby.Participants;
using ElophantClient.Messages.GameStats;
using ElophantClient.Messages.Readers;
using ElophantClient.Messages.Statistics;
using ElophantClient.Messages.Summoner;
using ElophantClient.Properties;
using ElophantClient.Proxy;
//using ElophantClient.Storage;
using ElophantClient.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NotMissing.Logging;
using System.Collections.Specialized;
using Microsoft.Win32;
using System.Globalization;

namespace ElophantClient.Gui
{
	public partial class MainForm : Form
	{
		public static readonly string Version = AssemblyAttributes.FileVersion + AssemblyAttributes.Configuration;
		const string SettingsFile = "settings.json";

		readonly Dictionary<LeagueRegion, CertificateHolder> Certificates;
		readonly Dictionary<ProcessInjector.GetModuleFrom, RadioButton> ModuleResolvers;
		readonly List<PlayerCache> PlayersCache = new List<PlayerCache>();
		readonly ProcessQueue<string> TrackingQueue = new ProcessQueue<string>();
		readonly ProcessMonitor launcher = new ProcessMonitor(new[] { "LoLLauncher" });

		RtmpsProxyHost Connection;
		MessageReader Reader;
		//IObjectContainer Database;
		//GameStorage Recorder;
		CertificateInstaller Installer;
		ProcessInjector Injector;
		GameDTO CurrentGame;
		List<ChampionDTO> Champions;
		SummonerData SelfSummoner;

        Dictionary<LeagueRegion, string> RegionsFullText;

		MainSettings Settings { get { return MainSettings.Instance; } }

        List<string> teammates = null;
        List<int> bans = null;
        bool APIRequestMade = false;
        string TeamIdJson = null;

		public MainForm()
		{
			InitializeComponent();

			Logger.Instance.Register(new DefaultListener(Levels.All, OnLog));
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			Application.ThreadException += Application_ThreadException;
			StaticLogger.Info(string.Format("Version {0}", Version));

			Settings.Load(SettingsFile);

			Certificates = new Dictionary<LeagueRegion, CertificateHolder>
			{
				{LeagueRegion.NA, new CertificateHolder("prod.na1.lol.riotgames.com", Resources.prod_na1_lol_riotgames_com)},
				{LeagueRegion.EUW, new CertificateHolder("prod.eu.lol.riotgames.com", Resources.prod_eu_lol_riotgames_com)},
				{LeagueRegion.EUNE, new CertificateHolder("prod.eun1.lol.riotgames.com", Resources.prod_eun1_lol_riotgames_com)},
				{LeagueRegion.BR, new CertificateHolder("prod.br.lol.riotgame.com", Resources.prod_br_lol_riotgames_com)}
 			};
			ModuleResolvers = new Dictionary<ProcessInjector.GetModuleFrom, RadioButton>
			{	 
				{ProcessInjector.GetModuleFrom.Toolhelp32Snapshot, ToolHelpRadio},
				{ProcessInjector.GetModuleFrom.ProcessClass, ProcessRadio},
				{ProcessInjector.GetModuleFrom.Mirroring, MirrorRadio}
			};
			foreach (var kv in ModuleResolvers)
			{
				kv.Value.Click += moduleresolvers_Click;
			}

            RegionsFullText = new Dictionary<LeagueRegion, string>
            {
                { LeagueRegion.NA, "North America" },
                { LeagueRegion.EUW, "Europe West" },
                { LeagueRegion.EUNE, "Europe Nordic & East" }, 
                { LeagueRegion.BR, "Brazil" }
            };

			//Database = Db4oEmbedded.OpenFile(CreateConfig(), "db.yap");

			var cert = Certificates.FirstOrDefault(kv => kv.Key == Settings.Region).Value;
			if (cert == null)
				cert = Certificates.First().Value;

			Injector = new ProcessInjector("lolclient");
			Connection = new RtmpsProxyHost(2099, cert.Domain, 2099, cert.Certificate);
			Reader = new MessageReader(Connection);

			Connection.Connected += Connection_Connected;
			Injector.Injected += Injector_Injected;
			Reader.ObjectRead += Reader_ObjectRead;

			//Recorder must be initiated after Reader.ObjectRead as
			//the last event handler is called first
			//Recorder = new GameStorage(Database, Connection);

			Connection.CallResult += Connection_Call;
			Connection.Notify += Connection_Notify;


			foreach (var kv in Certificates)
				RegionList.Items.Add(RegionsFullText[kv.Key]);
			int idx = RegionList.Items.IndexOf(RegionsFullText[Settings.Region]);
            //StaticLogger.Info("Index: " + idx);
			RegionList.SelectedIndex = idx != -1 ? idx : 0;	 //This ends up calling UpdateRegion so no reason to initialize the connection here.

			Installer = new CertificateInstaller(Certificates.Select(c => c.Value.Certificate).ToArray());

			TrackingQueue.Process += TrackingQueue_Process;
			launcher.ProcessFound += launcher_ProcessFound;

#if DEBUG
			button1.Visible = true;
#endif

			StaticLogger.Info("Startup Completed");
		}

		void moduleresolvers_Click(object sender, EventArgs e)
		{
			var check = ModuleResolvers.FirstOrDefault(kv => kv.Value.Checked);
			Settings.ModuleResolver = check.Key.ToString();
			Injector.Clear();
		}

		void launcher_ProcessFound(object sender, ProcessMonitor.ProcessEventArgs e)
		{

		}

		void TrackingQueue_Process(object sender, ProcessQueueEventArgs<string> e)
		{
		}

		void Injector_Injected(object sender, EventArgs e)
		{
			if (Created)
				BeginInvoke(new Action(UpdateStatus));
		}

		void Settings_Loaded(object sender, EventArgs e)
		{
            //TraceCheck.Checked = Settings.TraceLog;
            //DebugCheck.Checked = Settings.DebugLog;
            //DevCheck.Checked = Settings.DevMode;
            //LeaveCheck.Checked = Settings.DeleteLeaveBuster;
            IncludeBans.Checked = Settings.IncludeBans;
			var mod = ModuleResolvers.FirstOrDefault(kv => kv.Key.ToString() == Settings.ModuleResolver);
			if (mod.Value == null)
				mod = ModuleResolvers.First();
			mod.Value.Checked = true;
		}

		readonly object settingslock = new object();
		void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
            //MessageBox.Show("Property changed");
			lock (settingslock)
			{
				StaticLogger.Trace("Settings saved");
				Settings.Save(SettingsFile);
			}
		}

        //static IEmbeddedConfiguration CreateConfig()
        //{
        //    var config = Db4oEmbedded.NewConfiguration();
        //    config.Common.ObjectClass(typeof(PlayerEntry)).ObjectField("Id").Indexed(true);
        //    config.Common.ObjectClass(typeof(PlayerEntry)).ObjectField("TimeStamp").Indexed(true);
        //    config.Common.ObjectClass(typeof(GameDTO)).ObjectField("Id").Indexed(true);
        //    config.Common.ObjectClass(typeof(GameDTO)).ObjectField("TimeStamp").Indexed(true);
        //    config.Common.ObjectClass(typeof(EndOfGameStats)).ObjectField("GameId").Indexed(true);
        //    config.Common.ObjectClass(typeof(EndOfGameStats)).ObjectField("TimeStamp").Indexed(true);
        //    config.Common.Add(new TransparentPersistenceSupport());
        //    config.Common.Add(new TransparentActivationSupport());
        //    return config;
        //}

		void SetTitle(string title)
		{
			Text = string.Format(
					"Elophant Client v{0}{1}",
					Version,
					!string.IsNullOrEmpty(title) ? " - " + title : "");
		}

		//Allows for FInvoke(delegate {});
		void FInvoke(MethodInvoker inv)
		{
			BeginInvoke(inv);
		}

		void SetRelease(JObject data)
		{
			if (data == null)
				return;
			SetTitle(string.Format("v{0}{1}", data.Value<string>("Version"), data.Value<string>("ReleaseName")));
			//DownloadLink.Text = data.Value<string>("Link");
		}

		void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			LogException(e.Exception, true);
		}

		void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			LogException((Exception)e.ExceptionObject, !e.IsTerminating);
			//Bypass the queue and log it now if we are terminating.
			if (e.IsTerminating)
				TrackingQueue_Process(this, new ProcessQueueEventArgs<string> { Item = string.Format("error/{0}", Parse.ToBase64(e.ExceptionObject.ToString())) });
		}

		void LogException(Exception ex, bool track)
		{
			LogToFile(string.Format(
			   "[{0}] {1} ({2:MM/dd/yyyy HH:mm:ss.fff})",
			   Levels.Fatal.ToString().ToUpper(),
			   string.Format("{0} [{1}]", ex.Message, Parse.ToBase64(ex.ToString())),
			   DateTime.UtcNow
				));

			if (track)
				TrackingQueue.Enqueue(string.Format("error/{0}", Parse.ToBase64(ex.ToString())));
		}

		void Log(Levels level, object obj)
		{
			object log = string.Format(
					"[{0}] {1} ({2:MM/dd/yyyy HH:mm:ss.fff})",
					level.ToString().ToUpper(),
					obj,
					DateTime.UtcNow);
			Task.Factory.StartNew(LogToFile, log);			
		}

		void OnLog(Levels level, object obj)
		{
			if (level == Levels.Trace)
				return;
			if (level == Levels.Debug)
				return;            

			if (level == Levels.Error && obj is Exception)
			{
				TrackingQueue.Enqueue(string.Format("error/{0}", Parse.ToBase64(obj.ToString())));
			}

			if (obj is Exception)
				Log(level, string.Format("{0} [{1}]", ((Exception)obj).Message, Parse.ToBase64(obj.ToString())));
			else
				Log(level, obj);
		}

	

		readonly object LogLock = new object();
		const string LogFile = "Log.txt";
		void LogToFile(object obj)
		{
			try
			{
				lock (LogLock)
				{
					File.AppendAllText(LogFile, obj + Environment.NewLine);
				}
			}
			catch (Exception ex)
			{
				StaticLogger.Error(string.Format("[{0}] {1} ({2:MM/dd/yyyy HH:mm:ss.fff})", Levels.Fatal.ToString().ToUpper(), ex.Message, DateTime.UtcNow));
			}
		}

		void UpdateStatus()
		{
            if (!Installer.IsInstalled)
            {
                StatusLabel.Text = "Elophant Client is not installed.";
                //StatusLabel.ForeColor = Color.Firebrick;
            }
            else if (!Injector.IsInjected)
            {
                StatusLabel.Text = "Searching for the LoL Client...";
                //StatusLabel.ForeColor = Color.Firebrick;

                if (!RegionList.Enabled)
                {
                    RegionList.Enabled = true;
                }
            }
            else if (Connection != null && Connection.IsConnected)
            {
                StatusLabel.Text = "Connected to the LoL Client.";
                //StatusLabel.ForeColor = Color.ForestGreen;

                RegionList.Enabled = false;
            }
            else
            {
                StatusLabel.Text = "LoL Client found.";
                //StatusLabel.ForeColor = Color.DeepSkyBlue;
            }

            //StatusLabel.Left = (int)(this.Width / 2.0 - StatusLabel.Width / 2.0);
            //StatusLabel.Top = (int)((this.Height - StatusLabel.Height) / 2.0);
		}

		void Connection_Connected(object sender, EventArgs e)
		{
			if (Created)
				BeginInvoke(new Action(UpdateStatus));
		}

		void Reader_ObjectRead(object obj)
		{
            if (obj is GameDTO)
            {
                //StaticLogger.Info("obj is GameDTO");
                GameLobbyUpdate((GameDTO)obj);
            }
            else if (obj is EndOfGameStats)
                ClearCache(); //clear the player cache after each match.
            else if (obj is List<ChampionDTO>)
            {
                Champions = (List<ChampionDTO>)obj;
                if (Champions.Count > 0)
                {
                    ViewOwnedSkins.Enabled = true;
                }
            }
            else if (obj is LoginDataPacket)
                SelfSummoner = ((LoginDataPacket)obj).AllSummonerData.Summoner;
		}

		public void ClearCache()
		{
			lock (PlayersCache)
			{
				PlayersCache.Clear();
			}
		}

		public void GameLobbyUpdate(GameDTO lobby)
		{
            //StaticLogger.Info("Game Mode: " + lobby.GameMode);
            //StaticLogger.Info("Game Type: " + lobby.GameType);

            //StaticLogger.Info("Pre InvokeRequired");

            if (InvokeRequired)
            {
                BeginInvoke(new Action<GameDTO>(GameLobbyUpdate), lobby);
                return;
            }
            //StaticLogger.Info("Game State: " + lobby.GameState);
       
            if (CurrentGame == null || CurrentGame.Id != lobby.Id)
            {
                //StaticLogger.Info("CurrentGame is null OR the Ids don't match.");
                teammates = new List<string>();
                bans = new List<int>();
                APIRequestMade = false;
                CurrentGame = lobby;
            } 
           
            //CurrentGame.QueueTypeName.Equals("RANKED_SOLO_5x5")
            //StaticLogger.Info("Pre main IF");
            //StaticLogger.Info("Teammates count: " + teammates.Count);
            //StaticLogger.Info("Current Game State: " + CurrentGame.GameState);
            if (lobby.QueueTypeName.Equals("RANKED_SOLO_5x5") && lobby.GameState.Equals("PRE_CHAMP_SELECT") && teammates.Count == 0)
            {
                //StaticLogger.Info("Inside main if");                
                IncludeBans.Enabled = false;

                int teamNumber = 0;                               
                List<TeamParticipants> teams = new List<TeamParticipants> { lobby.TeamOne, lobby.TeamTwo };
          
                // Determine what team we're on
                for (int i = 0; i < teams.Count; i++)
                {               
                    TeamParticipants team = teams[i];
                    for (int j = 0; j < team.Count; j++)
                    {
                        PlayerParticipant player = team[j] as PlayerParticipant;
                        if (player != null && player.SummonerId != 0)
                        {                       
                            if (player.SummonerId == SelfSummoner.SummonerId)
                            {                           
                                teamNumber = i;
                            }
                        }            
                    }
                }

                TeamParticipants myTeam = teams[teamNumber];
                for (int i = 0; i < myTeam.Count; i++)
                {
                    PlayerParticipant player = myTeam[i] as PlayerParticipant;
                    teammates.Add(player.Name);
                }
                SummonerNamesLabel.Visible = true;
                //StaticLogger.Info("Team: " + string.Join(", ", teammates));
            }
            else if (lobby.QueueTypeName.Equals("RANKED_SOLO_5x5") && lobby.GameState.Equals("TERMINATED") || lobby.GameState.Equals("START_REQUESTED") || lobby.GameState.Equals("POST_CHAMP_SELECT"))
            {
                IncludeBans.Enabled = true;
                SummonerNamesLabel.Visible = false;
                BansLabel.Visible = false;
                GenRecLabel.Visible = false;
                CopyURLButton.Visible = false;
                OpenURLinBrowser.Visible = false;
            }

            //StaticLogger.Info("Pre POST Ifs");
            if (lobby.QueueTypeName.Equals("RANKED_SOLO_5x5") && lobby.GameState.Equals("PRE_CHAMP_SELECT") && !IncludeBans.Checked && !APIRequestMade) 
            {
                GenRecLabel.Visible = true;  
                //StaticLogger.Info("Not including bans...");
                using (WebClient wb = new WebClient())
                {
                    NameValueCollection data = new NameValueCollection();
                    data["call"] = "generateRecommendations";
                    data["region"] = Settings.Region.ToString().ToLower();
                    data["summonerNames"] = string.Join(",", teammates);
                    data["bans"] = "";

                    //StaticLogger.Info("Generating recommendations: " + data.ToString());
                                      
                    Byte[] response = wb.UploadValues(@"http://www.elophant.com/api.aspx", "POST", data);
                    
                    TeamIdJson = System.Text.Encoding.UTF8.GetString(response);

                    //StaticLogger.Info("Response: " + TeamIdJson);
                    APIRequestMade = true;
                }
                CopyURLButton.Visible = true;
                OpenURLinBrowser.Visible = true;
            }
            else if (lobby.QueueTypeName.Equals("RANKED_SOLO_5x5") && lobby.GameState.Equals("CHAMP_SELECT") && IncludeBans.Checked && !APIRequestMade)
            {
                //StaticLogger.Info("Including bans...");                
                foreach (var ban in lobby.BannedChampions)
                {
                    bans.Add(ban.ChampionId);
                }
                BansLabel.Visible = true;
                GenRecLabel.Visible = true;

                using (WebClient wb = new WebClient())
                {
                    NameValueCollection data = new NameValueCollection();
                    data["call"] = "generateRecommendations";
                    data["region"] = Settings.Region.ToString().ToLower();
                    data["summonerNames"] = string.Join(",", teammates);
                    data["bans"] = string.Join(",", bans);

                    //StaticLogger.Info("region: " + Settings.Region.ToString().ToLower() + " summonerNames: " + string.Join(",", teammates) + " bans: " + string.Join(",", bans));

                    Byte[] response = wb.UploadValues(@"http://www.elophant.com/api.aspx", "POST", data);

                    TeamIdJson = System.Text.Encoding.UTF8.GetString(response);

                    //StaticLogger.Info("Response: " + TeamIdJson);

                    APIRequestMade = true;
                }  
                CopyURLButton.Visible = true;
                OpenURLinBrowser.Visible = true;
            }
		}

        private void GenerateRecommendations()
        {
            using (WebClient wb = new WebClient())
            {
                NameValueCollection data = new NameValueCollection();
                data["call"] = "generateRecommendations";
                data["region"] = Settings.Region.ToString().ToLower();
                data["summonerNames"] = string.Join(",", teammates);
                data["bans"] = string.Join(",", bans);

                //StaticLogger.Info("region: " + Settings.Region.ToString().ToLower() + " summonerNames: " + string.Join(",", teammates) + " bans: " + string.Join(",", bans));
                GenRecLabel.Visible = true;
                Byte[] response = wb.UploadValues(@"http://www.elophant.com/api.aspx", "POST", data);

                TeamIdJson = System.Text.Encoding.UTF8.GetString(response);
                CopyURLButton.Visible = true;
                OpenURLinBrowser.Visible = true;
                //StaticLogger.Info("Response: " + TeamIdJson);

                APIRequestMade = true;
            }
        }

        private string SaveOwnedSkins(List<int> skinIds)
        {
            using (WebClient wb = new WebClient())
            {
                NameValueCollection data = new NameValueCollection();
                data["summonerName"] = SelfSummoner.Username;
                data["region"] = Settings.Region.ToString().ToLower();
                data["skinIds"] = string.Join(",", skinIds);  

                Byte[] response = wb.UploadValues(@"http://www.elophant.com/skins-owned.aspx", "POST", data);

                return System.Text.Encoding.UTF8.GetString(response);   
            }
        }

        private int GetChampIdFromSkinId(int skinId)
        {
            return skinId / 1000;
        }

        private int GetSkinIndexFromSkinId(int skinId)
        {
            return skinId % 1000;
        }

        static T GetParent<T>(Control c) where T : Control
		{
			if (c == null)
				return null;
			if (c.GetType() == typeof(T))
			{
				return (T)c;
			}
			return GetParent<T>(c.Parent);
		}
        
		private void MainForm_Shown(object sender, EventArgs e)
		{
			TrackingQueue.Enqueue("startup");

			Settings_Loaded(this, new EventArgs());   
            UpdateStatus();

			//Add this after otherwise it will save immediately due to RegionList.SelectedIndex
			Settings.PropertyChanged += Settings_PropertyChanged;

            Settings.ModuleResolver = "Toolhelp32Snapshot";

            VersionLabel.Text = "v" + Version;

			//Start after the form is shown otherwise Invokes will fail
			Connection.Start();
			Injector.Start();
			launcher.Start();

			//Fixes the team controls size on start as they keep getting messed up in the WYSIWYG
			MainForm_Resize(this, new EventArgs());

            try
            {
                RegistryKey installed_versions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP");
                string[] version_names = installed_versions.GetSubKeyNames();
                //version names start with 'v', eg, 'v3.5' which needs to be trimmed off before conversion
                double framework = Convert.ToDouble(version_names[version_names.Length - 1].Remove(0, 1), CultureInfo.InvariantCulture);

                if (framework < 4.0)
                {

                    if (MessageBox.Show("The Elophant Client requires the .NET Framework 4.0 Full version. Would you like to download it?", ".NET Framework 4.0 Full Not Found", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Process.Start("http://www.microsoft.com/en-us/download/details.aspx?id=17718");
                    }

                    MessageBox.Show("The Elophant Client will now close.");
                    Process.GetCurrentProcess().Kill();
                    return;
                }
            }
            catch (Exception ex)
            {
                StaticLogger.Error(ex.ToString());
                MessageBox.Show("An unknown exception has occurred. Check the log for more information.");
                Process.GetCurrentProcess().Kill();
                return;
            }

			try
			{
				var filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "lolbans", "LoLLoader.dll");
				if (File.Exists(filename))
				{
					StaticLogger.Info("Uninstalling old loader.");

					var shortfilename = AppInit.GetShortPath(filename);

					var dlls = AppInit.AppInitDlls32;
					if (dlls.Contains(shortfilename))
					{
						dlls.Remove(AppInit.GetShortPath(shortfilename));
						AppInit.AppInitDlls32 = dlls;
					}

					if (File.Exists(filename))
						File.Delete(filename);
				}
			}
			catch (SecurityException se)
			{
				StaticLogger.Warning(se);
			}
			catch (Exception ex)
			{
				StaticLogger.Error("Failed to uninstall. Message: " + ex);
			}

            // NOT SURE IF THIS WORKS - TRYING TO AVOID THE USE OF AN INSTALL BUTTON
            try
            {
                if (!Installer.IsInstalled)
                {
                    if (!Wow.IsAdministrator)
                    {
                        MessageBox.Show("Please run the Elophant Client as the Administrator to install it.");
                        Process.GetCurrentProcess().Kill();
                        return;
                    }
                    try
                    {
                        Installer.Install();
                    }
                    catch (UnauthorizedAccessException uaex)
                    {
                        MessageBox.Show("Unable to fully install/uninstall. Make sure LoL is not running.");
                        StaticLogger.Warning(uaex);
                    }
                    //InstallButton.Text = Installer.IsInstalled ? "Uninstall" : "Install";
                    UpdateStatus();
                }
            }
            catch
            {
            }

            TryToCheckForUpdates();
		}

        private void TryToCheckForUpdates()
        {
            WebClient client = new WebClient();
            string mostRecentVersion = client.DownloadString("http://elophant.com/client?q=v");
            if (!mostRecentVersion.Equals(Version))
            {
                if (MessageBox.Show("A newer version of the client is available. Would you like to download it?", "Update Available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Process.Start("http://elophant.com/client");
                }
            }
        }

        private void IncludeBans_CheckStateChanged(object sender, EventArgs e)
        {
            Settings.IncludeBans = IncludeBans.Checked;
        }

		private void RegionList_SelectedIndexChanged(object sender, EventArgs e)
		{
            //MessageBox.Show("Region changed");
			LeagueRegion region;
			if (!LeagueRegion.TryParse(RegionsFullText.FirstOrDefault(x => x.Value == RegionList.SelectedItem.ToString()).Key.ToString(), out region))
			{
                StaticLogger.Warning("Unknown enum " + RegionsFullText.FirstOrDefault(x => x.Value == RegionList.SelectedItem.ToString()).Key.ToString() + ".");
				return;
			}

			Settings.Region = region;

			var cert = Certificates.FirstOrDefault(kv => kv.Key == Settings.Region).Value;
			if (cert == null)
				cert = Certificates.First().Value;

			Connection.ChangeRemote(cert.Domain, cert.Certificate);
		}

		static string CallArgToString(object arg)
		{
			if (arg is RemotingMessage)
			{
				return ((RemotingMessage)arg).operation;
			}
			if (arg is DSK)
			{
				var dsk = (DSK)arg;
				var ao = dsk.Body as ASObject;
				if (ao != null)
					return ao.TypeName;
			}
			if (arg is CommandMessage)
			{
				return CommandMessage.OperationToString(((CommandMessage)arg).operation);
			}
			return arg.ToString();
		}

        void Connection_Call(object sender, Notify call, Notify result)
        {
            var text = string.Format(
                "Call {0} ({1}), Return ({2})",
                call.ServiceCall.ServiceMethodName,
                string.Join(", ", call.ServiceCall.Arguments.Select(CallArgToString)),
                string.Join(", ", result.ServiceCall.Arguments.Select(CallArgToString))
            );

            //StaticLogger.Info(text);
        }
        void Connection_Notify(object sender, Notify notify)
        {
            var text = string.Format(
                "Recv {0}({1})",
                !string.IsNullOrEmpty(notify.ServiceCall.ServiceMethodName) ? notify.ServiceCall.ServiceMethodName + " " : "",
                string.Join(", ", notify.ServiceCall.Arguments.Select(CallArgToString))
            );

            //StaticLogger.Info(text);
        }      

		/// <summary>
		/// Recursively adds a "TypeName" key to the ASObjects as newtonsoft doesn't serialize it.
		/// </summary>
		/// <param name="obj"></param>
		void AddMissingTypeNames(object obj)
		{
			if (obj == null)
				return;

			if (obj is ASObject)
			{
				var ao = (ASObject)obj;
				ao["TypeName"] = ao.TypeName;
				foreach (var kv in ao)
					AddMissingTypeNames(kv.Value);
			}
			else if (obj is IList)
			{
				var list = (IList)obj;
				foreach (var item in list)
					AddMissingTypeNames(item);
			}
		}             

		private void MainForm_Resize(object sender, EventArgs e)
		{

		}

		private void MainForm_ResizeBegin(object sender, EventArgs e)
		{
			SuspendLayout();
		}

		private void MainForm_ResizeEnd(object sender, EventArgs e)
		{
			ResumeLayout();
			MainForm_Resize(sender, e); //Force one last adjustment
		}

        private void ViewOwnedSkins_Click(object sender, EventArgs e)
        {            
            ViewOwnedSkins.Enabled = false;
            ViewOwnedSkins.Text = "Saving...";
            
            List<int> ownedSkinIds = new List<int>();

            foreach (var champ in Champions)
            {
                foreach (var skin in champ.ChampionSkins)
                {
                    if (skin.Owned)
                    {
                        ownedSkinIds.Add(skin.SkinId);
                    }
                }
            }

            string skinSaveId = SaveOwnedSkins(ownedSkinIds);

            Process.Start("http://elophant.com/skins-owned/" + skinSaveId);
            StaticLogger.Info("Process started: http://elophant.com/skins-owned/" + skinSaveId);

            ViewOwnedSkins.Text = "Saved";
        }

        private void CopyURLButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> TeamId = JsonConvert.DeserializeObject<Dictionary<string, string>>(TeamIdJson);
            Clipboard.SetText("http://elophant.com/" + TeamId.First().Value);
        }

        private void OpenInBrowser_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> TeamId = JsonConvert.DeserializeObject<Dictionary<string, string>>(TeamIdJson);
            Process.Start("http://elophant.com/" + TeamId.First().Value);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Connection != null && Connection.IsConnected)
            {
                if (MessageBox.Show("Closing the Elophant Client now will disconnect you from the LoL Client. Are you sure that you want to exit?", "Exit Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
	}
}
