using System;
using System.Runtime.InteropServices;

namespace PhoneGap
{
    // Need to invoke unmanaged mobile native code from .NET to play sounds. Woot!
    internal partial class PInvoke
    {
        public enum BeepTypes : uint
        {
            SimpleBeep = 0xffffffff,
            IconAsterisk = 0x00000040,
            IconExclamation = 0x00000030,
            IconHand = 0x00000010,
            IconQuestion = 0x00000020,
            Ok = 0x00000000
        };

        private enum SND
        {
            SYNC = 0x0000,
            ASYNC = 0x0001,
            NODEFAULT = 0x0002,
            MEMORY = 0x0004,
            LOOP = 0x0008,
            NOSTOP = 0x0010,
            NOWAIT = 0x00002000,
            ALIAS = 0x00010000,
            ALIAS_ID = 0x00110000,
            FILENAME = 0x00020000,
            RESOURCE = 0x00040004
        }

        private const int NLED_COUNT_INFO_ID = 0;

        private enum LedState : int
        {
            Off = 0,
            On = 1,
            Blink = 2
        }

        private struct NLED_COUNT_INFO
        {
            public uint cLeds;
        }

        private struct NLED_SETTINGS_INFO
        {
            public uint LedNum; // LED number, 0 is first LED
            public int OffOnBlink; // 0 == off, 1 == on, 2 == blink
            public int TotalCycleTime; // total cycle time of a blink in microseconds
            public int OnTime; // on time of a cycle in microseconds
            public int OffTime; // off time of a cycle in microseconds
            public int MetaCycleOn; // number of on blink cycles
            public int MetaCycleOff; // number of off blink cycles
        }

        [DllImport( "coredll.dll" )]
        static extern bool MessageBeep( BeepTypes beepType );

        [DllImport( "coredll.dll" )]
        static extern int PlaySound( string szSound, IntPtr hMod, int flags );

        [DllImport( "coredll.dll", EntryPoint = "NLedGetDeviceInfo" )]
        static extern bool NLedGetDeviceCount( short nID, ref NLED_COUNT_INFO pOutput );

        [DllImport( "coredll.dll", EntryPoint = "NLedSetDevice" )]
        static extern bool NLedSetDevice( short nID, ref NLED_SETTINGS_INFO pOutput );

        // not working on HTC Diamond2
        public static void Beep( BeepTypes msg )
        {
            MessageBeep(msg);
        }

        public static void Beep()
        {
            PlaySound( "\\Windows\\Alarm2.wav", IntPtr.Zero, ( int )( SND.SYNC | SND.FILENAME ) );
        }

        public static void Vibrate( int time )
        {
            NLED_COUNT_INFO CountStruct = new NLED_COUNT_INFO();
            if( !NLedGetDeviceCount( NLED_COUNT_INFO_ID, ref CountStruct ) )
                throw new NotSupportedException( "Not supported in this device." );
            //Error Initialising LEDs

            uint m_count = CountStruct.cLeds;

            NLED_SETTINGS_INFO nsi = new NLED_SETTINGS_INFO();

            // vibrate on
            nsi.LedNum = m_count - 1;
            nsi.OffOnBlink = ( int )LedState.On;
            NLedSetDevice( 2, ref nsi );

            // sleep for specified time
            System.Threading.Thread.Sleep( time );

            // vibrate off
            nsi.OffOnBlink = ( int )LedState.Off;
            NLedSetDevice( 2, ref nsi );
        }
}
    class NotificationCommand : Command
    {
        private string command = "";
        private int millis = 500;

        Boolean Command.accept(String instruction)
        {
            Boolean retVal = false;
            if( instruction.StartsWith( "/beep" ) )
            {
                command = instruction;
                retVal = true;
            }
            else if( instruction.StartsWith( "/vibrate" ) )
            {
                command = instruction;
                int firstSlash = instruction.IndexOf( '/', 2 );
                millis = int.Parse(instruction.Substring( firstSlash + 1, instruction.Length - firstSlash - 1 ));
                retVal = true;
            }
            return retVal;            
        }
        String Command.execute(String instruction)
        {
            if( command.StartsWith( "/beep" ) )
            {
                PInvoke.Beep();
                return ";";
            }
            else if( command.StartsWith( "/vibrate" ) )
            {
                PInvoke.Vibrate( millis );
                return ";";
            }
            return ";alert(\"[PhoneGap Error] invalid instruction\");";
        }
    }
}
