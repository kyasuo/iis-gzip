
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace iis_gzip
{
    /**
     * IISGzip
     * 
     * @require "C:\\Windows\\System32\\inetsrv\\gzip.dll"
     * @see https://msdn.microsoft.com/en-us/library/dd692872(v=vs.90).aspx#Anchor_1
     */
    class IISGzip
    {

        // default buffer size
        static int BUFFER_SIZE = 4096;

        /**
         * compress
         * 
         * @param inps Byte[]
         * @param outs Stream
         * @param compressionLevel int(between 0 to 10)
         */
        public void compress (Byte[] inps, Stream outs, int compressionLevel)
        {
            // Check arguments
            if (inps == null)
            {
                throw new ArgumentNullException("inps");
            }
            else if (outs == null)
            {
                throw new ArgumentNullException("outs");
            }
            else if (compressionLevel < 0 || 10 < compressionLevel)
            {
                throw new ArgumentOutOfRangeException("compressionLevel", "compressionLevel must be between 0 and 10.");
            }

            // Call InitCompression
            InitCompression();

            // Call CreateCompression
            IntPtr contextHandle = IntPtr.Zero;
            CreateCompression(out contextHandle, 1);

            // Convert byte array to list
            List<Byte> inpList = new List<Byte>(inps);

            // Call Compress
            int hResult = 0;
            Byte[] buffer = new Byte[BUFFER_SIZE];
            do
            {
                int outUsed = 0;
                int inUsed = 0;
                hResult = Compress(contextHandle,
                        inpList.ToArray(),
                        inpList.Count,
                        buffer,
                        BUFFER_SIZE,
                        ref inUsed,
                        ref outUsed,
                        compressionLevel);
                if (0 < outUsed)
                {
                    outs.Write(buffer, 0, outUsed);
                }
                inpList.RemoveRange(0, inUsed);
            }
            while (hResult == 0);
            outs.Flush();

            // DestroyCompression
            DestroyCompression(contextHandle);

            // DeInitCompression
            DeInitCompression();
        }

        /**
         * compress
         * 
         * @param inps Byte[]
         * @param outs Stream
         * @param compressionLevel int(between 0 to 10)
         */
        public void compress(Stream inps, Stream outs, int compressionLevel)
        {
            // Check arguments
            if (inps == null)
            {
                throw new ArgumentNullException("inps");
            }
            else if (outs == null)
            {
                throw new ArgumentNullException("outs");
            }
            else if (compressionLevel < 0 || 10 < compressionLevel)
            {
                throw new ArgumentOutOfRangeException("compressionLevel", "compressionLevel must be between 0 and 10.");
            }

            // Read input stream and copy to MemoryStream
            MemoryStream ms = new MemoryStream();
            inps.CopyTo(ms);

            // Call compres
            this.compress(ms.ToArray(), outs, compressionLevel);
        }

        [DllImport("C:\\Windows\\System32\\inetsrv\\gzip.dll", CharSet = CharSet.Auto)]
        private static extern int InitCompression();

        [DllImport("C:\\Windows\\System32\\inetsrv\\gzip.dll", CharSet = CharSet.Auto)]
        private static extern int CreateCompression(out IntPtr context, int flags);

        [DllImport("C:\\Windows\\System32\\inetsrv\\gzip.dll", CharSet = CharSet.Auto)]
        private static extern int Compress(IntPtr context,
             byte[] input,
             int input_size,
             byte[] output,
             int output_size,
             ref int input_used,
             ref int output_used,
             int compressionLevel);

        [DllImport("C:\\Windows\\System32\\inetsrv\\gzip.dll", CharSet = CharSet.Auto)]
        private static extern int DestroyCompression(IntPtr context);

        [DllImport("C:\\Windows\\System32\\inetsrv\\gzip.dll", CharSet = CharSet.Auto)]
        private static extern int DeInitCompression();

    }
}
