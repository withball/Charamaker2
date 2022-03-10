using Microsoft.MixedReality.WebRTC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C2WebRTCP2P
{
	
    public partial class tuusinform : Form
    {

        bool sv;
		PeerConnection wow;
		/// <summary>
		/// Peerconectionを操作して通信の接続を行うクラス
		/// </summary>
		/// <param name="ppp"></param>
		/// <param name="server"></param>
		public tuusinform(PeerConnection ppp,bool server )
        {
			sv = server;
			wow = ppp;
			InitializeComponent();
			
			wow.LocalSdpReadytoSend += sdpready;
			wow.IceCandidateReadytoSend += icecandget;
			wow.DataChannelAdded += DataChannelAdded;
			wow.DataChannelRemoved += DataChannelRemoved;
			if (!sv)
			{
				wow.CreateOffer();
				this.Text = supertusin.formmess[0];
			}
			else 
			{
				this.Text = supertusin.formmess[1];
			}
		}
		private void sdpready(SdpMessage s) 
		{
		
			tex1 = s.Content + "(^v^)";
		}
		private void icecandget(IceCandidate s) 
		{
			if (tex2 == "")
			{


				tex2 = s.SdpMid + "(^v^)"

				+ s.SdpMlineIndex + "(^v^)"
		+ s.Content;
				if (s.Content.Count((a)=>a==':')>1) { }
				else
				{
					if (s.Content.Contains("192.168."))
					{
						Console.WriteLine("多分通信は無理だ");
						this.Text = supertusin.formmess[2];
					}
					else
					{
						Console.WriteLine("通信は厳しそうだが");

						this.Text = supertusin.formmess[3];
					}
				}
			}
			
		}
		private void DataChannelAdded(DataChannel channel)
		{
			Console.WriteLine($"Event: DataChannel Added {channel.Label}");
			channel.StateChanged += () => { Console.WriteLine($"DataChannel '{channel.Label}':  StateChanged '{channel.State}'"); };
		}

		private void DataChannelRemoved(DataChannel channel)
		{
			Console.WriteLine($"Event: DataChannel Removed {channel.Label}");
		}
		private void tuusinform_Load(object sender, EventArgs e)
        {
				
        }
		


        private void close(object sender, FormClosedEventArgs e)
        {
			wow.LocalSdpReadytoSend -= sdpready;
			wow.IceCandidateReadytoSend -= icecandget;
			wow.DataChannelAdded -= DataChannelAdded;
			wow.DataChannelRemoved -= DataChannelRemoved;
		}

		private void clicked(object sender, EventArgs e)
		{
			string[] ttt;

			var sss = tucon.getted(textBox3.Text);
			if (sss == null)
			{
				this.Text = supertusin.formmess[7];
				return;
			}
			ttt = sss.Split(new string[] { "(^v^)" }, StringSplitOptions.RemoveEmptyEntries);


			SdpMessage mess = new SdpMessage();
			mess.Content = ttt[0];
			if (sv) mess.Type = SdpMessageType.Offer;
			else mess.Type = SdpMessageType.Answer;
			Console.WriteLine("<" + mess.Content + "> " + ttt[1] + " :wow: " + ttt[2] + " aslmf;a " + ttt[3]);
			try
			{
				var b = wow.SetRemoteDescriptionAsync(mess);
				b.Wait();
				if (sv)
				{
					this.Text = supertusin.formmess[4];

				}
				else
				{
					this.Text = supertusin.formmess[5];

				}

			}
			catch (Exception ee) { Console.WriteLine("^^:" + ee.ToString()); this.Text = supertusin.formmess[6]; }
			if (mess.Type == SdpMessageType.Offer)
			{

				wow.CreateAnswer();
				//wow.CreateOffer();
			}

			try
			{

				var cand = new IceCandidate
				{
					SdpMid = ttt[1],
					SdpMlineIndex = Convert.ToInt32(ttt[2]),
					Content = ttt[3]
				};
				wow.AddIceCandidate(cand);
				if (ttt[3].Count((a) => a == ':') > 1) { }
				else
				{
					if (ttt[3].Contains("192.168."))
					{
						this.Text = supertusin.formmess[2];
					}
					else
					{

						this.Text = supertusin.formmess[3];
					}
				}
			}
			catch { }
		}

		string tex1 = "", tex2 = "";

        

        private void ticked(object sender, EventArgs e)
        {
			textBox1.Text = tucon.tosend(tex1+tex2);

			
			
		}
    }
	static class tucon 
	{
		static public string tosend(string hyo) 
		{
		//return hyo;
			string res = gaticon.conv(serverman3.asyukku(andbyte(hyo)));
		
		
			return res;
		}
		static public string getted(string hyo)
		{
			//return hyo;
	
			byte[] b = gaticon.conv(hyo);
			
			return andbyte(serverman3.kaitou(b));
		}
		static private byte[] andbyte(string hyo)
		{
			try
			{
				if (hyo != null)
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					using (MemoryStream ms = new MemoryStream())
					{
						if (ms != null && binaryFormatter != null)
						{
							binaryFormatter.Serialize(ms, hyo);

							return ms.ToArray();
						}
					}
				}
			}
			catch (Exception eee) { Console.WriteLine("tytytyttytytyubusuze" + eee.ToString()); }
			return null;
		}
		static private string andbyte(byte[] hyo)
		{
			try
			{
				if (hyo != null)
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					using (MemoryStream ms = new MemoryStream(hyo))
					{
						if (ms != null && binaryFormatter != null)
						{
							var a = binaryFormatter.Deserialize(ms);
							if (a != null) return (string)a;
						}
					}
				}
			}
			catch (Exception eee) { Console.WriteLine("tubusuze" + eee.ToString()); }
			return null;

		}
	}

	static class gaticon
	{
        public static readonly char[] wow = { '1', 'q', 'a', 'z', '2', 'w', 's', 'x', '3', 'e', 'd', 'c'
				, '4', 'r', 'f', 'v', '5', 't', 'g', 'b', '6', 'y', 'h', 'n',
			'7', 'u', 'j', 'm', '8', 'i', 'k', ',', '9', 'o', 'l', '.'
				, '0', 'p', ';', '/', '?', '+', 'P', '-', '>', 'L', 'O', ')'
				, '<', 'K', 'I', '(', 'M', 'J', 'U', '\'', 'N', 'H', 'Y', '&', 'B', 'G','T','%' };
		static public string conv(byte[] b) 
		{

			
		/*	for (int i = 0; i < b.Length; i++)
			{
				Console.Write(b[i] + "-");
			}
			Console.WriteLine("  ppl  "+b.Length);
			*/
			string res = "";
			List<int> aas = new List<int> { };
			for (int i = 0; i < b.Length; i++)
			{
				switch (i % 3) 
				{
					case 0:
						aas.Add(b[i]*16);
						break;
					case 1:
						aas[aas.Count - 1] += b[i] / 16;
						aas.Add((b[i] % 16)*16*16);
						break;
					case 2:
						aas[aas.Count - 1] += b[i] ;
						
						break;
					
				}
			
            }
		
			foreach (var a in aas) 
			{
		//		Console.Write(a +" \\ ");
				res += wow[a / 64];
				res += wow[a % 64];
			}
		//	Console.WriteLine(res.Length+" uncho " + aas.Count );
		//	Console.WriteLine(res);
			return res;

		}
		static public byte[] conv(string s)
		{
			//Console.WriteLine(s);
			List<int> aas = new List<int> { };
			List<int> aas2 = new List<int> { };
			for (int i = 0; i < s.Length; i++)
			{
			
				switch (i % 2)
				{
					case 0:
						aas.Add(Array.IndexOf(wow, s[i]) * (wow.Length ));
						break;
					case 1:
						aas[aas.Count-1]+=Array.IndexOf(wow, s[i] );
						break;
				
				}

			}
			for (int i = 0; i < aas.Count; i++)
			{
		//		Console.Write(aas[i] + " \\ ");
				switch (i % 2)
				{
					case 0:
						aas2.Add(aas[i]/16);
						aas2.Add(aas[i] % 16 * 16);
						break;
					case 1:
						aas2[aas2.Count-1]+=(aas[i] / 16/16);
						aas2.Add(aas[i] % (16 * 16));
						break;

				}

			}
			//Console.WriteLine(s.Count()+" poui " +aas.Count);
			var res = new byte[aas2.Count];
			for (int i = 0; i < aas2.Count; i++) 
			{
				res[i] = (byte)aas2[i];
			//	Console.Write(res[i] + "-");
			}
		//	Console.WriteLine("  om "+res.Length);
			return res;
			

		}
	}
}