using System;
using System.IO;
using CookComputing.XmlRpc;
using System.Text.RegularExpressions;
using System.Net;
using BitFactory.Logging;

namespace Project7
{
	public class PingBackNotificationProxy : XmlRpcClientProtocol
	{
		public PingBackNotificationProxy()
		{
			
		}

		private string errormessage = "No Error";
		public string ErrorMessage
		{
			get{return errormessage;}
		}

		public bool Ping(string pageText,string sourceURI, string targetURI)
		{
			string pingbackURL = GetPingBackURL(pageText,targetURI,sourceURI);
			if(pingbackURL != null)
			{
				this.Url = pingbackURL;
				try
				{
					Notify(sourceURI,targetURI);
					return true;
				}
				catch(Exception ex)
				{
					errormessage = "Error: " + ex.Message;
				}
			}
			return false;

		}

		private  string GetPingBackURL(string pageText, string url, string PostUrl)
		{
			if(!Regex.IsMatch(pageText,PostUrl,RegexOptions.IgnoreCase|RegexOptions.Singleline))
			{
				if(pageText != null)
				{
					string pat = "<link rel=\"pingback\" href=\"([^\"]+)\" ?/?>";
					Regex reg = new Regex(pat, RegexOptions.IgnoreCase | RegexOptions.Singleline  ) ;
					Match m = reg.Match(pageText) ;
					if ( m.Success )
					{
						return m.Result("$1") ;
					}
				}		
			}
			return null;
		}

		[XmlRpcMethod("pingback.ping")]
		public void Notify(string sourceURI , string targetURI )
		{
			Invoke("Notify",new object[] {sourceURI,targetURI});
		}

	}

	/// <summary>
	/// Summary description for Class.
	/// </summary>
	class Class
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//
			// TODO: Add code to start application here
			//

                        PingBackNotificationProxy pbnp = new PingBackNotificationProxy();
                        pbnp.Proxy = new WebProxy("proxy.singnet.com.sg", 8080);
                        pbnp.Ping("<link rel=\"pingback\" href=\"http://chuacw.ath.cx/chewy/Services/Pingback.aspx\" />",
                        "http://chuacw.ath.cx/chewy/archive/2005/05/06/1283.aspx",  // target
                        "http://chuacw.ath.cx/chuacw/archive/2005/05/05/1282.aspx"  // source
                        );
		}
	}
}
