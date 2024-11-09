using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using System;
using System.IO;
using System.Windows.Forms;  // Needed for MessageBox

public class FirebaseServiceInitializer
{

    private static readonly Lazy<FirebaseServiceInitializer> lazy = new Lazy<FirebaseServiceInitializer>(() => new FirebaseServiceInitializer());
    public static FirebaseServiceInitializer Instance { get { return lazy.Value; } }

    private string tempFilePath;
    private FirestoreDb firestoreDb;

    public FirebaseServiceInitializer()
    {
        string jsonContent = GenerateFirebaseJson();
        CreateTempJsonFile(jsonContent);
        InitializeFirebase();
        DeleteTempJsonFile();
    }

    private string GenerateFirebaseJson()
    {
        return @"{
        ""type"": ""service_account"",
        ""project_id"": ""ableworld-ho-menu"",
        ""private_key_id"": ""e5f050b39a7e7d0fe80ea6af79fe41d9b26b518d"",
        ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCz2tNXlQcoZQv2\n4kUdmjzigsRJprZEsqvbkhqhVgHRMWLFOEyt0iXWokegqDRrVdznTuj7QHPFcKl3\nIjYKJs6GBnvjMk07PKsuuM6/mvcUkIKx+rFb/sMlT32GTbRMGt4Wj9Ynrd6p74Ju\n0rO4D/5W8f2Gb3dyDlSqhTjOIqVu9IAPDEMIwuwIgaDic9AhgAoj0tWRy/8emYdV\nahi6yGnjvvFWpjn5x2Z1vrv8NA9hBkd5xt/KJowjEj3Kj1TnmeyVVtrloBUohNrP\nV+ZzJ4hRpqpS82E8N2gCFcZD5klYzU6jkf8KT4/rfE9lxT5h9d77+zdFqyceCt+F\njahal737AgMBAAECggEARh1f2LvrX28NHAEX6rNvWuqZtRC968gvwViS8ySBbtku\n5S3OjtDnGWaIP/RzAwklJjOYFiMJPwZtuljm8kwwpQUwFOFORHKhVYSMbRvviN/R\nY6sUq4WE9C2qBMqUKDstK2SFm1BoBp2tnqCbmXz6rVCHgno/+YHmtddbztio37jg\nM4MNruD7tO4OuG1f4Zo7XDRJcI9HmxkYNpbzB6OUBKGkQL0EZMMVMmu1FsE8UoNs\nOQaI/V+bG6dhcyQ24qjag5FEP8XlCvXRpmMkuKXzrzAtHxpVIFZ7284FNcJb72Pv\n7tUV9kDUhj5H8Q3zLlDRFUl9nc5slmVWJUrx6Lm7GQKBgQDdnHc5f1SsJ8G/YSG9\nWgPbR+oaT2GXmQT3ZukScRU6ptbRY14lQBeZxcFWcqstnb2X74GlBjGzD9cEiaGS\niBlvKPkXd2+K21ICYQIMuHI3QJxxMN/N96WpNRvQe9j2Ld+DnTWcDQj0V3feDda3\nb8p8KOmPluRDggjFFEsIave8vwKBgQDPw5S8FU16dyvra4lHvrdr4uBjb55WEDB/\n8WXspBLrLiQGecv+D/lOcOADU7RHDqBnMQZqDPIl008h3J9NfdCeKhD+Btowp4er\nFq7L7xX8ZX/k/iFcRYZuSZNqZ+9UAwpqtxzh2UQikP5HU5Bq08n6XYdwzOgSPn4f\n/LeYg7lBxQKBgD6pdmDQqz1+hF879NFYuYxwejZ0SbEW+HuIItEvHoSWFlngkgdQ\nZxv4+eEazWI5nxluBKeH59es2+yRihkn2KFA4aYMBIMr3rWDfpPgN5N99n5fBnlz\n0+jTdojt6/w4HmJVuonkeaq1bNRh71uxBX00CE6sqOCZzScExO6daG17AoGBAJKi\nKOEt78bVQQwgk30tX2snbtL/PjLjrjc+en8vtaKCqC5h29VuFLiF9bSjaQMVkaQC\n99H1XRnRL3JosY4RlCFs8x02XNwmARyBH9ES5uOCB2fo7EahUyWXjBF3VXRnyPxU\nTHAh1XLH/o78rVqYK18mj6bPF6N4s6+Dv1hG74M5AoGAHGGKrIkDBdPtmN9ZRcAM\njAA/23DOCYoaPh/3VZFQxSRJX/5vQpVpnUSTuJlN7hXdt9UIwlKA2IbxI6svaO04\nuPmzITPOHyHw+6usbW8K7SczVAxQy0SaI2yf0/DpAKMlCjGb2jrWqyI6WcnS6WQr\nM7fKrF69d4MPg1WqEU7qsbM=\n-----END PRIVATE KEY-----\n"",
        ""client_email"": ""firebase-adminsdk-sikz9@ableworld-ho-menu.iam.gserviceaccount.com"",
        ""client_id"": ""104821534731146421345"",
        ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
        ""token_uri"": ""https://oauth2.googleapis.com/token"",
        ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
        ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-sikz9@ableworld-ho-menu.iam.gserviceaccount.com""
    }";
    }

    private void CreateTempJsonFile(string jsonContent)
    {
        // Create a temp file path
        tempFilePath = Path.Combine(Path.GetTempPath(), "firebase_service_account.json");

        // Write the JSON content to the temp file
        File.WriteAllText(tempFilePath, jsonContent);
    }

    private void InitializeFirebase()
    {
        // Check if the FirestoreDb has already been initialized
        if (firestoreDb != null)
        {
            return;
        }

        string jsonContent = GenerateFirebaseJson();
        CreateTempJsonFile(jsonContent);

        // Set the environment variable to the temp file path
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempFilePath);

        // Initialize the Firebase app if not already initialized
        if (FirebaseApp.DefaultInstance == null)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(tempFilePath)
            });
        }

        // Initialize Firestore
        firestoreDb = FirestoreDb.Create("ableworld-ho-menu");

        DeleteTempJsonFile(); // Clean up temp file
    }

    private void DeleteTempJsonFile()
    {
        // Delete the temp file after initialization
        if (File.Exists(tempFilePath))
        {
            File.Delete(tempFilePath);
        }
    }

    public FirestoreDb GetFirestoreDb()
    {
        return firestoreDb;
    }
}
