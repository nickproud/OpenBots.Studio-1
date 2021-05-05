using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Ionic.Zip;

namespace OpenBots.Core.ChromeNativeMessaging.Extension
{
    public class BrowserExtensions
    {
        private const byte IntegerTag = 0x02;
        private const byte BitStringTag = 0x03;
        private const byte SequenceTag = 0x30;
        private const byte NullTag = 0x50;

        public static bool CreateForChrome(String unpackedExtensionPath, String destinationPath)
        {
            try
            {
                // zipping an extension
                String archivePath = String.Format("{0}\\{1}.zip", Path.GetDirectoryName(destinationPath),
                                                   Path.GetFileNameWithoutExtension(destinationPath));
                var zipFile = new ZipFile();
                zipFile.AddDirectory(unpackedExtensionPath);
                zipFile.Save(archivePath);
                zipFile.Dispose();

                // signing the hash
                byte[] zipBytes = File.ReadAllBytes(archivePath);
                var rsa = new RSACryptoServiceProvider();
                var sha1 = new SHA1Managed();
                byte[] hash = sha1.ComputeHash(zipBytes);
                String sha1Oid = CryptoConfig.MapNameToOID("SHA1");
                byte[] signature = rsa.SignHash(hash, sha1Oid);
                byte[] modulus = rsa.ExportParameters(false).Modulus;
                byte[] exponent = rsa.ExportParameters(false).Exponent;
                String rsaOid = CryptoConfig.MapNameToOID("RSA");
                byte[] objectId = CryptoConfig.EncodeOID(rsaOid);

                // constructing DER-encoded public key structure
                List<byte> publicKeyStructure = new List<byte>();
                publicKeyStructure.AddRange(new byte[] { SequenceTag, 0x81, 0x9f });
                // SEQUENCE (9F bytes), 81 - 10000001, bits 6-0 (0000001) indicates that there is one more byte needed to specify the length (9F)
                publicKeyStructure.AddRange(new byte[] { SequenceTag, 0x0d }); // SEQUENCE (D bytes)
                publicKeyStructure.AddRange(objectId);
                publicKeyStructure.AddRange(new byte[] { NullTag, 0x00 }); // parameters - NULL (0 bytes)
                publicKeyStructure.AddRange(new byte[] { BitStringTag, 0x81, 0x8d }); // BIT_STRING (8d bytes)
                publicKeyStructure.Add(0x00); // unused bits in the final byte of content
                publicKeyStructure.AddRange(new byte[] { SequenceTag, 0x81, 0x89 }); // SEQUENCE (89 bytes)
                publicKeyStructure.AddRange(new byte[] { IntegerTag, 0x81, 0x81 }); // INTEGER (81 bytes)
                publicKeyStructure.Add(0x00); // indicates that number is not negative
                publicKeyStructure.AddRange(modulus);
                publicKeyStructure.AddRange(new byte[] { IntegerTag, 0x03 }); // INTEGER (3 bytes)
                publicKeyStructure.AddRange(exponent);

                // building CRX file
                byte[] crxMagicNumber = Encoding.UTF8.GetBytes("Cr24");
                var crxVersion = new byte[4];
                crxVersion = BitConverter.GetBytes(2);
                var keyLength = new byte[4];
                keyLength = BitConverter.GetBytes(publicKeyStructure.Count);
                var signatureLength = new byte[4];
                signatureLength = BitConverter.GetBytes(signature.Length);
                List<byte> crxPackage = new List<byte>();
                crxPackage.AddRange(crxMagicNumber);
                crxPackage.AddRange(crxVersion);
                crxPackage.AddRange(keyLength);
                crxPackage.AddRange(signatureLength);
                crxPackage.AddRange(publicKeyStructure);
                crxPackage.AddRange(signature);
                crxPackage.AddRange(zipBytes);
                File.WriteAllBytes(destinationPath, crxPackage.ToArray());
                File.Delete(archivePath);
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}