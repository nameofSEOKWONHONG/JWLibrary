using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Core.NetFramework.GZip
{
	public interface ICompress {
		string Compress(string param);
		string Decompress(string param);
	}
}
