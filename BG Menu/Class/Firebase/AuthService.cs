using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class AuthService
{
    private readonly FirestoreDb firestoreDb;
    public FirestoreDb FirestoreDb => firestoreDb;
    public List<string> Permissions { get; private set; } = new List<string>();
    public static string Username { get; private set; }


    public AuthService(FirestoreDb db)
    {
        firestoreDb = db;
    } 
    

    public async Task SetPasswordAsync(string username, string password)
    {
        try
        {
            DocumentReference docRef = firestoreDb.Collection("BG-Users").Document(username);
            string hashedPassword = HashPassword(password);

            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                { "Password", hashedPassword }
            };

            await docRef.SetAsync(updates, SetOptions.MergeAll);

            Console.WriteLine($"Password for user {username} has been set successfully.");
        }
        catch (Exception ex)
        {
            // Handle exceptions (log them, show message, etc.)
            Console.WriteLine($"Error setting password: {ex.Message}");
        }
    }

    // Method to hash password
    private string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    // Method to login a user
    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            DocumentReference docRef = firestoreDb.Collection("BG-Users").Document(username);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                string storedHash = snapshot.GetValue<string>("Password");

                if (VerifyPassword(password, storedHash))
                {
                    // Store user information and permissions after successful login
                    Username = username;
                    Permissions = snapshot.GetValue<List<string>>("Permissions");
                    return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            // Handle exceptions (log them, show message, etc.)
            Console.WriteLine($"Error during login: {ex.Message}");
            return false;
        }
    }

    // Method to verify password
    private bool VerifyPassword(string password, string storedHash)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            string hash = Convert.ToBase64String(bytes);
            return hash == storedHash;
        }
    }

    // Method to check if the user has a specific permission
    public bool HasPermission(string permission)
    {
        return Permissions.Contains(permission);
    }

    public async Task<List<string>> GetAllowedStoresAsync()
    {
        if (string.IsNullOrEmpty(Username))
        {
            throw new InvalidOperationException("User is not logged in.");
        }

        DocumentReference docRef = firestoreDb.Collection("BG-Users").Document(Username);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            Dictionary<string, object> userData = snapshot.ToDictionary();
            if (userData.ContainsKey("Stores"))
            {
                List<string> allowedStores = new List<string>();
                foreach (var store in (List<object>)userData["Stores"])
                {
                    allowedStores.Add(store.ToString());
                }
                return allowedStores;
            }
        }
        return new List<string>();
    }


}
