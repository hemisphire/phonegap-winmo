// based on http://blogs.msdn.com/jehance/archive/2004/07/12/181067.aspx

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PhoneGap
{
    public sealed class DeviceIDException : Exception
    {
        public DeviceIDException() : base() {}
        public DeviceIDException(string message) : base(message) {}
        public DeviceIDException(string message, Exception innerException) : base(message, innerException) {}
    }

    /// <summary>
    /// Summary description for DeviceID.
    /// </summary>
    public sealed class DeviceID
    {
        private const int GuidLength = 16;
        private const Int32 ERROR_NOT_SUPPORTED = 0x32;
        private const Int32 ERROR_INSUFFICIENT_BUFFER = 0x7A;    
        private const Int32 IOCTL_HAL_GET_DEVICEID = 0x01010054;

        private byte[] idBytes  = null;
        private Guid   idGuid   = Guid.Empty;
        private string idString = String.Empty;

        public delegate byte[] ComputeHash( byte[] hash );

        public DeviceID()
        {
            byte[] buffer = new byte[20];
            bool idLoaded = false;
            uint bytesReturned;

            while(!idLoaded)
            {
                Array.Copy(BitConverter.GetBytes(buffer.Length), 0, buffer, 0, 4);

                try
                {
                    idLoaded = KernelIoControl(IOCTL_HAL_GET_DEVICEID, 0, 0, buffer, (uint)buffer.Length, out bytesReturned);
                }
                catch (Exception e)
                {
                    throw new DeviceIDException("This platform may not support DeviceIDs", e);
                }

                if(!idLoaded)
                {
                    int error = Marshal.GetLastWin32Error();

                    if(error == ERROR_INSUFFICIENT_BUFFER)
                    {
                        buffer = new byte[BitConverter.ToUInt32(buffer, 0)];
                    }
                    else
                    {
                        // Some older PPC devices only return the ID if the buffer
                        // is exactly the size of a GUID, so attempt to retrieve
                        // the ID this way before throwing an exception
                        buffer = new byte[GuidLength];
                        idLoaded = KernelIoControl(IOCTL_HAL_GET_DEVICEID, 0, 0, buffer, GuidLength, out bytesReturned);

                        if(idLoaded)
                        {
                            InitializeFromBytes(buffer);
                            return;
                        }
                        else
                        {
                            if(error == ERROR_NOT_SUPPORTED)
                            {
                                throw new DeviceIDException("This platform does not support DeviceIDs");
                            }
                            else
                            {
                                throw new DeviceIDException(String.Format(
                                "Error Encountered Retrieving ID (0x{0})", error.ToString("X8")));
                            }
                        }
                    }
                }
            }

            int dwPresetIDOffset = BitConverter.ToInt32(buffer, 4);
            int dwPresetIDBytes = BitConverter.ToInt32(buffer, 8);
            int dwPlatformIDOffset = BitConverter.ToInt32(buffer, 12);
            int dwPlatformIDBytes = BitConverter.ToInt32(buffer, 16);

            idBytes = new byte[dwPresetIDBytes + dwPlatformIDBytes];

            Array.Copy(buffer, dwPresetIDOffset, idBytes, 0, dwPresetIDBytes);
            Array.Copy(buffer, dwPlatformIDOffset, idBytes, dwPresetIDBytes, dwPlatformIDBytes);
			buildIdString();
        }

        public DeviceID(Guid g)
        {
            idBytes = g.ToByteArray();
            idGuid = g;
			buildIdString();
        }

        public DeviceID(byte[] bytes)
        {
            InitializeFromBytes(bytes);
			buildIdString();
        }

        private void InitializeFromBytes(byte[] bytes)
        {
            idBytes = new byte[bytes.Length];
            Array.Copy(bytes, 0, idBytes, 0, bytes.Length);
        }

        /// <summary>
        /// DeviceIDs are only guaranteed to be Guids on PPC.  On generic Windows CE, they can be
        /// any unspecified length
        /// </summary>
        public bool IsGuid
        {
            get
            {
                return (idBytes.Length == GuidLength);
            }
        }

        public Guid Guid
        {
            get
            {
                if(IsGuid)
                {
                    if(idGuid == Guid.Empty)
                    {
                        idGuid = new Guid(idBytes);
                    }

                    return idGuid;
                }
                else
                {
                    throw new DeviceIDException(String.Format("The DeviceID {0} is not a Guid", ToString()));
                }
            }
        }

        public override bool Equals(object obj)
        {
            if(obj is DeviceID)
            {
                DeviceID rhs = (DeviceID)obj;

                if(idBytes.Length == rhs.idBytes.Length)
                {
                    for(int i = 0; i < idBytes.Length; i++)
                    {
                        if(idBytes[i] != rhs.idBytes[i])
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            if(IsGuid)
            {
                return this.Guid.GetHashCode();
            }
            else
            {
                // The default GetHashCode for object is guaranteed to
                // always be the same for a given object, but not for
                // multiple objects with the same value.  We want a HashCode
                // that will always be the same for a given ID
                byte[] tempbytes = new byte[16];
                if(idBytes.Length > 16)
                {
                    Array.Copy(idBytes, 0, tempbytes, 0, 16);
                }
                else
                {
                    Array.Clear(tempbytes, 0, 16); // Should be unneccessary
                    Array.Copy(idBytes, 0, tempbytes, 0, idBytes.Length);
                }

                return (new Guid(tempbytes)).GetHashCode();
            }
        }

		private void buildIdString()
		{
			if( IsGuid )
			{
				idString = this.Guid.ToString();
			}
			else
			{
				idString = "";

				for( int i = 0; i < idBytes.Length; i++ )
				{
					if( i == 4 || i == 6 || i == 8 || i == 10 )
					{
						idString = String.Format( "{0}-{1}", idString, idBytes[ i ].ToString( "x2" ) );
					}
					else
					{
						idString = String.Format( "{0}{1}", idString, idBytes[ i ].ToString( "x2" ) );
					}
				}
			}
		}

        public override string ToString()
        {
            return idString;
        }

        public byte[] ToByteArray()
        {
            byte[] cpyBytes = new byte[idBytes.Length];
            Array.Copy(idBytes, 0, cpyBytes, 0, idBytes.Length);
            return cpyBytes;
        }

        /// <summary>
        /// Check to see if current device is on list of approved devices
        /// </summary>
        /// <returns></returns>
        public static bool checkDeviceID( string resourceName, ComputeHash ch )
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            string deviceID;
            byte[] deviceIDbytes;
            byte[] hashedIDbytes;
            byte[] hashedIDbytes2 = new byte[ 20 ];

            Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream( resourceName );
            BinaryReader br = new BinaryReader( s );

            deviceID = new DeviceID().ToString();
            deviceIDbytes = enc.GetBytes( deviceID );
            hashedIDbytes = ch( deviceIDbytes );

            hashedIDbytes2 = br.ReadBytes( 20 );
            while( hashedIDbytes2 != null && hashedIDbytes2.Length > 0 )
            {
                bool bEqual = false;
                // byte array comparison
                if( hashedIDbytes.Length == hashedIDbytes2.Length )
                {
                    int i = 0;
                    while( ( i < hashedIDbytes2.Length ) && ( hashedIDbytes2[ i ] == hashedIDbytes[ i ] ) )
                    {
                        i += 1;
                    }
                    if( i == hashedIDbytes2.Length )
                    {
                        bEqual = true;
                    }
                }

                if( bEqual )
                {
                    br.Close();
                    return true;
                }
                if( br.PeekChar() == -1 )
                {
                    br.Close();
                    return false;
                }
                hashedIDbytes2 = br.ReadBytes( 20 );
            }

            br.Close();
            return false;
        }

        [DllImport( "coredll.dll", SetLastError = true )]
        private static extern bool KernelIoControl(
            uint dwIoControlCode,
            uint lpInBuf /* set to 0 */,
            uint nInBufSize /* set to 0 */,
            [In, Out] byte[] lpOutBuf,
            uint nOutBufSize,
            out     uint lpBytesReturned);
    }
}
