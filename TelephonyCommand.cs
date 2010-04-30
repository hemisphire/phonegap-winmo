using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsMobile.Telephony;

namespace PhoneGap
{
	class TelephonyCommand : Command
	{
		private string number = "";

		Boolean Command.accept(String instruction)
		{
			Boolean retVal = false;
			if( instruction.StartsWith( "/call" ) )
			{
				int firstSlash = instruction.IndexOf('/', 4);
				number = instruction.Substring( firstSlash + 1 );
				retVal = true;
			}
			return retVal;
		}

		String Command.execute(String instruction)
		{
			//Call number
            Phone call = new Phone();
            call.Talk( number );

			return ";";
		}
	}
}