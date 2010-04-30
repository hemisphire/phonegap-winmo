using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsMobile.PocketOutlook;

namespace PhoneGap
{
    class ContactCommand : Command
    {
        private OutlookSession outlookSession;
        private string successCallback;
        Boolean Command.accept( String instruction )
        {
            Boolean retVal = false;
            if( instruction.StartsWith( "/contacts" ) )
            {
                int firstSlash = instruction.IndexOf( '/', 2 );
                successCallback = instruction.Substring( firstSlash + 1, instruction.Length - firstSlash - 1 );
                retVal = true;
            }
            return retVal;
        }
        String Command.execute( String instruction )
        {
            // Count number of contacts
            this.outlookSession = new OutlookSession();

            String returnString = ";";
            for(int i = 0; i < outlookSession.Contacts.Items.Count; i++ )
            {
                returnString += "navigator.contacts.foundContact('" + outlookSession.Contacts.Items[i].FirstName + " " +
                    outlookSession.Contacts.Items[i].LastName + "','" + outlookSession.Contacts.Items[i].HomeTelephoneNumber + 
                    "','" + outlookSession.Contacts.Items[i].Email1Address + "');";
            }
            if( outlookSession.Contacts.Items.Count > 0 )
                returnString += successCallback + "();";
            return returnString;
        }
    }
}
