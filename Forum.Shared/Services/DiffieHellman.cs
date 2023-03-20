using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Forum.Shared.Services {
  public class DiffieHellman {

    public BigInteger g = 65;
    public BigInteger p = 48;

    public long publicKey;
    public byte[] IV;


    private BigInteger _secret;
    private DES _des = null;

    public DiffieHellman() {
      _des = new DESCryptoServiceProvider();
      IV = _des.IV;

      Random rand = new Random();
      _secret = rand.Next(100)+100;

      publicKey = (long)BigInteger.ModPow(g, _secret, p);
    }

    public byte[] Encrypt(long publicKey, string secretMessage) {
      byte[] encryptedMessage;
      _des.Key = BitConverter.GetBytes(GetCipher(publicKey));

      using (var cipherText = new MemoryStream()) {
        using (var encryptor = _des.CreateEncryptor()) {
          using (var cryptoStream = new CryptoStream(cipherText, encryptor, CryptoStreamMode.Write)) {
            byte[] ciphertextMessage = Encoding.UTF8.GetBytes(secretMessage);
            cryptoStream.Write(ciphertextMessage, 0, ciphertextMessage.Length);
          }
        }

        encryptedMessage = cipherText.ToArray();
      }

      return encryptedMessage;
    }

    public string Decrypt(long publicKey, byte[] encryptedMessage, byte[] iv) {
      string decryptedMessage;

      _des.Key = BitConverter.GetBytes(GetCipher(publicKey));
      _des.IV = iv;

      using (var plainText = new MemoryStream()) {
        using (var decryptor = _des.CreateDecryptor()) {
          using (var cryptoStream = new CryptoStream(plainText, decryptor, CryptoStreamMode.Write)) {
            cryptoStream.Write(encryptedMessage, 0, encryptedMessage.Length);
          }
        }

        decryptedMessage = Encoding.UTF8.GetString(plainText.ToArray());
      }

      return decryptedMessage;
    }
    private long GetCipher(long publicKey) {
      long key = long.Parse(BigInteger.ModPow(publicKey, _secret, p).ToString());
      return key + 100;
    }
  }
}
