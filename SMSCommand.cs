using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsMobile.PocketOutlook;

namespace PhoneGap
{
    class SMSCommand : Command
    {
        private string number = "";
        private string message = "";
        Boolean Command.accept( String instruction )
        {
            Boolean retVal = false;
            if( instruction.StartsWith( "/send" ) )
            {
                int firstSlash = instruction.IndexOf( '/', 5 );
                string[] payload = instruction.Substring( firstSlash ).Split(',');
                if( payload.Length == 2 )
                {
                    number = payload[ 0 ];
                    message = payload[ 1 ];
                    retVal = true;
                }
            }
            return retVal;
        }
        String Command.execute( String instruction )
        {
            // Create the SMS message
            SmsMessage msg = new SmsMessage( number, message );

            // Send the SMS message
            msg.Send();
            
            return ";";
        }
    }
}
