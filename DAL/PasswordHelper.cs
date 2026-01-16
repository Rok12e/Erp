using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace YamyProject
{

    public static class PasswordHelper
    {
        public static string HashPassword(string password, out string salt)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[16];
                rng.GetBytes(saltBytes);
                salt = Convert.ToBase64String(saltBytes);
            }
            string saltedPassword = salt + password;
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            string saltedPassword = storedSalt + enteredPassword;
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                string enteredHash = Convert.ToBase64String(hashBytes);
                return enteredHash == storedHash;
            }
        }
    }

    public static class CryptoHelper
    {
        // Simple fixed key and IV. Ensure these are 16 bytes for AES-128.
        private static readonly string key = "1234567890123456";  // 16 bytes key for AES-128
        private static readonly string iv = "1234567890123456";   // 16 bytes IV

        // Encrypt method
        public static string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                using (MemoryStream msEncrypt = new MemoryStream())
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                    swEncrypt.Flush();           
                    csEncrypt.FlushFinalBlock(); 
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        // Decrypt method
        public static string Decrypt(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);  // Convert Base64 to byte array

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);  // Ensure key is 16 bytes
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);    // Ensure IV is 16 bytes

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();  // Return the decrypted string
                }
            }
        }
    }

}
public class PasswordPromptForm : Form
{
    private TextBox txtPassword;
    private Label lblPrompt;
    private Button btnOK;
    private Button btnCancel;

    public string EnteredPassword { get; private set; }

    public PasswordPromptForm(string prompt)
    {
        this.Text = "Enter Password";
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.Size = new System.Drawing.Size(350, 150);
        this.MinimizeBox = false;
        this.MaximizeBox = false;
        this.ShowInTaskbar = false;

        lblPrompt = new Label()
        {
            Text = prompt,
            Location = new System.Drawing.Point(10, 10),
            AutoSize = true
        };

        txtPassword = new TextBox()
        {
            Location = new System.Drawing.Point(10, 35),
            Width = 310,
            PasswordChar = '*'
        };

        btnOK = new Button()
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Location = new System.Drawing.Point(165, 70),
            Width = 75
        };
        btnOK.Click += (sender, e) => {
            EnteredPassword = txtPassword.Text;
            this.Close();
        };

        btnCancel = new Button()
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Location = new System.Drawing.Point(245, 70),
            Width = 75
        };
        btnCancel.Click += (sender, e) => {
            this.Close();
        };

        this.Controls.Add(lblPrompt);
        this.Controls.Add(txtPassword);
        this.Controls.Add(btnOK);
        this.Controls.Add(btnCancel);

        this.AcceptButton = btnOK;
        this.CancelButton = btnCancel;
    }
}
