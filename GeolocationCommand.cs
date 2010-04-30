using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsMobile.Samples.Location;

namespace PhoneGap
{
    /// <summary>
    /// Native gps code using Microsoft provided classes
    /// </summary>
    public class Geolocator
    {
        private Gps gps;

        public Geolocator()
        {
            gps = new Gps();
        }

        public void start()
        {
            if( !gps.Opened )
            {
                gps.Open();
            }
        }

        public void stop()
        {
            if( gps.Opened )
            {
                gps.Close();
            }
        }

        public GpsPosition getCurrentPosition()
        {
            return gps.GetPosition();
        }
    }
    class GeolocationCommand : Command
    {
        private string command = "";
        private string successCallback = "";

        Boolean Command.accept(String instruction)
        {
            Boolean retVal = false;
            if( instruction.StartsWith( "/start_gps" ) )
            {
                command = instruction;
                WebForm.Instance.geolocator.start();
                retVal = true;
            }
            else if( instruction.StartsWith( "/stop_gps" ) )
            {
                command = instruction;
                WebForm.Instance.geolocator.stop(); 
                retVal = true;
            }
            else if( instruction.StartsWith( "/getCurrentPosition" ) )
            {
                command = instruction;
                int firstSlash = instruction.IndexOf( '/', 2 );
                successCallback = instruction.Substring( firstSlash + 1, instruction.Length - firstSlash - 2 );
                retVal = true;
            }
            return retVal;            
        }
        String Command.execute(String instruction)
        {
            if( command.StartsWith( "/start" ) )
            {
                return ";";
            }
            else if( command.StartsWith( "/stop" ) )
            {
                return ";";
            }
            else if( command.StartsWith( "/getCurrentPosition" ) )
            {
                GpsPosition position = WebForm.Instance.geolocator.getCurrentPosition();

                if( position == null )
                    return ";";

                String returnString = ";" + successCallback + "(new Position(new Coordinates(";
                returnString += position.Latitude + ",";
                returnString += position.Longitude + ",";
                returnString += position.SeaLevelAltitude + ",";
                // based on http://www.developerfusion.co.uk/show/4652/3/
                returnString += position.HorizontalDilutionOfPrecision * 6 + ",";
                returnString += position.Heading + ",";
                returnString += position.Speed + "),";
                if( position.TimeValid )
                    returnString += position.Time.Ticks + "));";
                else
                    returnString += DateTime.Now.Ticks + "));";

                return returnString;
            }
            return ";alert(\"[PhoneGap Error] invalid instruction\");";
        }
    }
}
