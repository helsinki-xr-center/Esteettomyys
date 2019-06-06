using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;


namespace SaveSystem
{
	public static class StringCompress
	{
		public static string Compress(string uncompressed)
		{
			var bytes = Encoding.UTF8.GetBytes(uncompressed);

			using (var msi = new MemoryStream(bytes))
			using (var mso = new MemoryStream())
			{
				using (var gs = new GZipStream(mso, CompressionMode.Compress))
				{
					msi.CopyTo(gs);
				}

				return Convert.ToBase64String(mso.ToArray());
			}
		}

		public static string Decompress(string compressed)
		{
			using (var msi = new MemoryStream(Convert.FromBase64String(compressed)))
			using (var mso = new MemoryStream())
			{
				using (var gs = new GZipStream(msi, CompressionMode.Decompress))
				{
					gs.CopyTo(mso);
				}

				return Encoding.UTF8.GetString(mso.ToArray());
			}
		}

		public static Stream GetCompressionStream(Stream source)
		{
			return new GZipStream(source, CompressionMode.Compress);
		}

		public static Stream GetDecompressionStream(Stream dest)
		{
			return new GZipStream(dest, CompressionMode.Decompress);
		}
	}
}