using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PhoneGap
{
    // Need to invoke unmanaged mobile native code from .NET to play sounds. Woot!
    internal class Sound
    {
        private byte[] m_soundBytes;
        private string m_fileName;

        private enum Flags {
            SND_SYNC = 0x0000,  /* play synchronously (default) */
            SND_ASYNC = 0x0001,  /* play asynchronously */
            SND_NODEFAULT = 0x0002,  /* silence (!default) if sound not found */
            SND_MEMORY = 0x0004,  /* pszSound points to a memory file */
            SND_LOOP = 0x0008,  /* loop the sound until next sndPlaySound */
            SND_NOSTOP = 0x0010,  /* don't stop any currently playing sound */
            SND_NOWAIT = 0x00002000, /* don't wait if the driver is busy */
            SND_ALIAS = 0x00010000, /* name is a registry alias */
            SND_ALIAS_ID = 0x00110000, /* alias is a predefined ID */
            SND_FILENAME = 0x00020000, /* name is file name */
            SND_RESOURCE = 0x00040004  /* name is resource name or atom */
        }

        [DllImport("CoreDll.DLL", EntryPoint="PlaySound", SetLastError=true)]
        private extern static int WCE_PlaySound(string szSound, IntPtr hMod, int flags);

        [DllImport("CoreDll.DLL", EntryPoint="PlaySound", SetLastError=true)]
        private extern static int WCE_PlaySoundBytes (byte[] szSound, IntPtr hMod, int flags);

        /// <summary>
        /// Construct the Sound object to play sound data from the specified file.
        /// </summary>
        public Sound (string fileName) {
            m_fileName = fileName;
        }

        /// <summary>
        /// Construct the Sound object to play sound data from the specified stream.
        /// </summary>
        public Sound(Stream stream)    {
            // read the data from the stream
            m_soundBytes = new byte [stream.Length];
            stream.Read(m_soundBytes, 0,(int)stream.Length);
        }

        /// <summary>
        /// Play the sound
        /// </summary>
        public void Play () {
            // if a file name has been registered, call WCE_PlaySound,
            //  otherwise call WCE_PlaySoundBytes
            if (m_fileName != null)
                WCE_PlaySound(m_fileName, IntPtr.Zero, (int) (Flags.SND_ASYNC | Flags.SND_FILENAME));
            else
                WCE_PlaySoundBytes (m_soundBytes, IntPtr.Zero, (int) (Flags.SND_ASYNC | Flags.SND_MEMORY));
        }
    }

    class MediaCommand : Command
    {
        private string soundFileName = "";
        private string soundExtension = "";
        Boolean Command.accept(String instruction)
        {
            Boolean retVal = false;
            if (instruction.StartsWith("/media"))
            {
                int firstSlash = instruction.IndexOf('/',5);
                soundFileName = instruction.Substring(firstSlash);
                soundExtension = soundFileName.Substring(soundFileName.LastIndexOf('.'));
                // TODO: Test what other sound file types work.
                switch (soundExtension)
                {
                    case ".wav":
                    case ".mp3":
                        retVal = true;
                        break;
                }
            }
            return retVal;
        }

        String Command.execute(String instruction)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string path = assembly.GetName().Name + "." + soundFileName.Substring( 1 ).Replace( "/", "." );
            try
            {
                Stream s = assembly.GetManifestResourceStream( path );
                Sound sound = new Sound( s );
                sound.Play();
                return "";
            }
            catch
            {
                return ";alert(\"[PhoneGap Error] Could not find sound file with path '" + path + "'.\");";
            }
        }
    }
}