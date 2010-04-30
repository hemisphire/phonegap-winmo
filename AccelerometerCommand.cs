using System;
using System.Runtime.InteropServices;
using GSensorSDK;

namespace PhoneGap
{
    /// <summary>
    /// Native accelerometer code - 
    /// HTC only! - will throw exception on unsupported devices, uncomment at your peril!
    /// </summary>
    public class Accelerometer
    {
        //private HTCGSensor mySensor = new HTCGSensor();
        //private GVector vector;
        //private string successCallback = "";

        public Accelerometer()
        {
            //mySensor = new HTCGSensor();
        }

        //void mySensor_OrientationChanged( IGSensor sender )
        //{
        //    vector = sender.GetGVector();
        //    String uriString = "javascript:" + successCallback + "(new Acceleration("+ vector.X + "," + vector.Y + "," + vector.Z + "));";
        //    WebForm.Instance.webBrowser.Navigate( new Uri( uriString ) );
        //}

        public void startWatch(string callback)
        {
            //mySensor.OrientationChanged += new OrientationChangedHandler( mySensor_OrientationChanged );
            //successCallback = callback;
        }

        public void stopWatch()
        {
            //mySensor.OrientationChanged -= new OrientationChangedHandler( mySensor_OrientationChanged );
        }
    }
    class AccelerometerCommand : Command
    {
        Boolean Command.accept( String instruction )
        {
            Boolean retVal = false;
            if( instruction.StartsWith( "/startwatch" ) )
            {
                int firstSlash = instruction.IndexOf( '/', 2 );
                string successCallback = instruction.Substring( firstSlash + 1, instruction.Length - firstSlash - 1 );
                WebForm.Instance.accelerometer.startWatch( successCallback );
                retVal = true;
            }
            if( instruction.StartsWith( "/stopwatch" ) )
            {
                WebForm.Instance.accelerometer.stopWatch();
                retVal = true;
            }
            return retVal;
        }

        String Command.execute( String instruction )
        {
            return ";";
        }
    }
}
