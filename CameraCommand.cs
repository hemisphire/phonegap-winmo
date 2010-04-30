using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsMobile.Forms;

namespace PhoneGap
{
    class CameraCommand : Command
    {
        private CameraCaptureDialog capture;
        Boolean Command.accept( String instruction )
        {
            Boolean retVal = false;
            if( !Microsoft.WindowsMobile.Status.SystemState.CameraPresent )
                return retVal;
            if( instruction.StartsWith( "/capture" ) )
            {
                capture = new CameraCaptureDialog();
                retVal = true;
            }
            return retVal;
        }
        String Command.execute( String instruction )
        {
            // Take a picture
            capture.ShowDialog();
            
            return ";";
        }
    }
}
