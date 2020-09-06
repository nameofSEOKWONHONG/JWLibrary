namespace JWLibrary.Core.NetFramework.GZip {

    public interface ICompress {

        string Compress(string param);

        string Decompress(string param);
    }
}