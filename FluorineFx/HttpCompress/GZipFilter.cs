using System;
using System.IO;

using System.Text;
using System.Diagnostics;

#if (NET_1_1)
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.GZip;
#else
using System.IO.Compression;
#endif


namespace FluorineFx.HttpCompress
{
#if (NET_1_1)
	/// <summary>
	/// A filter to support HTTP compression using GZip.
	/// </summary>
	class GZipFilter : CompressingFilter 
	{
		/// <summary>
		/// compression stream member
		/// has to be a member as we can only have one instance of the
		/// actual filter class
		/// </summary>
		private GZipOutputStreamEx m_stream = null;

		/// <summary>
		/// Primary constructor.  Need to pass in a stream to wrap up with gzip.
		/// </summary>
		/// <param name="baseStream">The stream to wrap in gzip.  Must have CanWrite.</param>
		public GZipFilter(Stream baseStream) : base(baseStream, CompressionLevels.Normal) 
		{
			m_stream = new GZipOutputStreamEx(baseStream);
		}

		/// <summary>
		/// Write content to the stream and have it compressed using gzip.
		/// </summary>
		/// <param name="buffer">The bytes to write</param>
		/// <param name="offset">The offset into the buffer to start reading bytes</param>
		/// <param name="count">The number of bytes to write</param>
		public override void Write(byte[] buffer, int offset, int count) 
		{
			if(!HasWrittenHeaders) WriteHeaders();
			m_stream.Write(buffer, offset, count);
		}

		/// <summary>
		/// The Http name of this encoding.  Here, gzip.
		/// </summary>
		public override string ContentEncoding 
		{
			get { return "gzip"; }
		}

		/// <summary>
		/// Closes this Filter and calls the base class implementation.
		/// </summary>
		public override void Close() 
		{
			m_stream.Close(); // this will close the gzip stream along with the underlying stream
			// no need for call to base.Close() here.
		}

		/// <summary>
		/// Flushes the stream out to underlying storage
		/// </summary>
		public override void Flush() 
		{
			m_stream.Flush();
		}
        /// <summary>
        /// This method supports the Fluorine infrastructure and is not intended to be used directly from your code.
        /// </summary>
		public override long TotalIn
		{
			get{ return m_stream.TotalIn; }
		}
        /// <summary>
        /// This method supports the Fluorine infrastructure and is not intended to be used directly from your code.
        /// </summary>
		public override long TotalOut
		{
			get{ return m_stream.TotalOut; }
		}
	}
#else
    /// <summary>
    /// A filter to support HTTP compression using GZip.
    /// </summary>
    class GZipFilter : CompressingFilter
    {
        /// <summary>
        /// compression stream member
        /// has to be a member as we can only have one instance of the
        /// actual filter class
        /// </summary>
        private GZipStream m_stream = null;

        /// <summary>
        /// Primary constructor.  Need to pass in a stream to wrap up with gzip.
        /// </summary>
        /// <param name="baseStream">The stream to wrap in gzip.  Must have CanWrite.</param>
        public GZipFilter(Stream baseStream)
            : base(baseStream, CompressionLevels.Normal)
        {
            m_stream = new GZipStream(baseStream, CompressionMode.Compress);
        }

        /// <summary>
        /// Write content to the stream and have it compressed using gzip.
        /// </summary>
        /// <param name="buffer">The bytes to write</param>
        /// <param name="offset">The offset into the buffer to start reading bytes</param>
        /// <param name="count">The number of bytes to write</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!HasWrittenHeaders) WriteHeaders();
            m_stream.Write(buffer, offset, count);
        }

        /// <summary>
        /// The Http name of this encoding.  Here, gzip.
        /// </summary>
        public override string ContentEncoding
        {
            get { return "gzip"; }
        }

        /// <summary>
        /// Closes this Filter and calls the base class implementation.
        /// </summary>
        public override void Close()
        {
            m_stream.Close(); // this will close the gzip stream along with the underlying stream
            // no need for call to base.Close() here.
        }

        /// <summary>
        /// Flushes the stream out to underlying storage
        /// </summary>
        public override void Flush()
        {
            m_stream.Flush();
        }
   }
#endif
}