using System;
using System.Runtime.InteropServices;

namespace PhoneGap
{
	/// <summary>
	/// Summary description for OSInfo
	/// </summary>
	public class OSInfo
	{
		internal const int SPI_GETPLATFORMTYPE = 257;

		[DllImport("coredll.dll")]
		public static extern int SystemParametersInfo (
			int uiAction,
			int uiParam,
			string pvParam,
			int fWinIni );

		public static string GetPlatform()
		{
			string szPlatform = "                                   ";
			string strPlatform = "";

			//Get OSVersion
			System.OperatingSystem osVersion = Environment.OSVersion;

			// Get Platform
			int ret = SystemParametersInfo(SPI_GETPLATFORMTYPE, szPlatform.Length ,
				szPlatform, 0);

			if (ret != 0)
			{
				strPlatform = szPlatform.Substring(0, szPlatform.IndexOf('\0'));
			}

			if (osVersion.Version.Major == 3) //PPC2000 or PPC2002
			{
                if( strPlatform == "PocketPC" )
                {
                    return "Pocket PC 2002";
                }
                else
                {
                    return "Pocket PC";
                }
            }
			else if (osVersion.Version.Major == 4)
			{
                if (strPlatform == "PocketPC")
                {
                    if (osVersion.Version.Build == 1081 || osVersion.Version.Build == 1088) 
                        return "Windows Mobile";
                    else
                        return "Pocket PC";
                }
                else
					return "Unknown";
			}
            else if (osVersion.Version.Major == 5)
            {
                if (strPlatform == "PocketPC")
                    return "Windows Mobile";
                else
                    return "Unknown";
            }
            return "Unknown";
		}

		public static string GetVersion()
		{
			//Get OSVersion
			System.OperatingSystem osVersion = Environment.OSVersion;

			if (osVersion.Version.Major == 4)
			{
                if (osVersion.Version.Build == 1081) 
                    return "2003 for Pocket PC";
                else if (osVersion.Version.Build == 1088) 
                    return "2003 Second Edition";
                else
                    return "2003";
			}
            else if (osVersion.Version.Major == 5)
            {
                if (osVersion.Version.Minor == 0 || osVersion.Version.Minor == 1)
                    return "5.0";
                else if( osVersion.Version.Minor == 2 )
                {
                    if( osVersion.Version.Build < 19202 )
                        return "6";
                    else if( osVersion.Version.Build < 20757 )
                        return "6.1";
                    else if( osVersion.Version.Build >= 20757 )
                        return "6.2";
                    return "Unknown";
                }
            }
            return "Unknown";
		}

        public static string CFVersion()
        {
            string cfVer = System.Environment.Version.ToString();

            if (cfVer == "1.0.2268.0")
                return "1.0";
            else if (cfVer == "1.0.3111.0")
                return "1.0 SP1";
            else if (cfVer == "1.0.3226.0")
                return "1.0 SP2 Recall";
            else if (cfVer == "1.0.3227.0")
                return "1.0 SP2 Beta";
            else if (cfVer == "1.0.3316.0")
                return "1.0 SP2";
            else if (cfVer == "1.0.4177.0")
                return "1.0 SP3 Beta";
            else if (cfVer == "1.0.4292.0")
                return "1.0 SP3";
            else if (cfVer == "2.0.4037.0")
                return "2.0 VS2005 CTP May";
            else if (cfVer == "2.0.4135.0")
                return "2.0 VS2005 Beta 1";
            else if (cfVer == "2.0.4317.0")
                return "2.0 VS2005 CTP November";
            else if (cfVer == "2.0.4278.0")
                return "2.0 VS2005 CTP December";
            else if (cfVer == "2.0.5056.0")
                return "2.0 VS2005 Beta 2";
            else if( cfVer == "2.0.5238.0" )
                return "2.0";
            else if( cfVer == "2.0.6129.0" )
                return "2.0 SP1";
            else if( cfVer == "2.0.7045.0" )
                return "2.0 SP2";
            else if( cfVer == "3.5.7283.0" )
                return "3.5";
            else
                return cfVer + " (no match)";
        }
	}
}
