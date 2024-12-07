using System.Security.Cryptography;

namespace BlockTest.Models
{
    public class Wallet
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }

        public Wallet()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                var keys = rsa.ExportParameters(true);
                PublicKey = Convert.ToBase64String(keys.Modulus);
                PrivateKey = Convert.ToBase64String(keys.D);
            }
        }

        // Sign a transaction
        public string SignTransaction(string data)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                var keyParameters = new RSAParameters
                {
                    D = Convert.FromBase64String(PrivateKey)
                };
                rsa.ImportParameters(keyParameters);
                var dataBytes = System.Text.Encoding.UTF8.GetBytes(data);
                var signedData = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                return Convert.ToBase64String(signedData);
            }
        }
    }
}
