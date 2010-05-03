using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.WindowsMobile.PocketOutlook;

namespace PhoneGap
{
    class FileCommand : Command
    {
        private string command = "";
        private string filename = "";
        private string data = "";
        Boolean Command.accept( String instruction )
        {
            Boolean retVal = false;
            if( instruction.StartsWith( "/readFile" ) )
            {
                command = instruction;
                int firstSlash = instruction.IndexOf( '/', 2 );
                filename = instruction.Substring( firstSlash + 1, instruction.Length - firstSlash - 1 );
                retVal = true;
            }
            else if( instruction.StartsWith( "/writeFile" ) )
            {
                command = instruction;
                int firstSlash = instruction.IndexOf( '/', 2 );
                string[] payload = instruction.Substring( firstSlash + 1, instruction.Length - firstSlash - 1 ).Split(',');
                if( payload.Length == 2 )
                {
                    filename = payload[ 0 ];
                    data = payload[ 1 ];
                    retVal = true;
                }
            }
            return retVal;
        }
        String Command.execute( String instruction )
        {
            String path = System.IO.Path.GetDirectoryName( 
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase );
            String filepath = path + "\\" + filename;

            if( command.StartsWith( "/readFile" ) )
            {
                if( File.Exists( filepath ) )
                {
                    StreamReader sr = File.OpenText( filepath );
                    data = sr.ReadToEnd();
                    sr.Close();
                    String returnString = ";navigator.fileMgr.reader_onload('" + filename + "','" + data + "');";
                    return returnString;
                }
            }
            else if( command.StartsWith( "/writeFile" ) )
            {
                StreamWriter sw =  File.CreateText( filepath );
                sw.WriteLine( data );
                sw.Close();
                String returnString = ";navigator.fileMgr.writer_oncomplete('" + filename + "','" + data.Length + "');";
                return returnString;
            }
            
            return ";";
        }
    }
}
