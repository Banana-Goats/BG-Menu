using Google.Cloud.Storage.V1;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;

public class FirebaseStorage
{
    private readonly StorageClient firebaseStorage;

    public FirebaseStorage(StorageClient firebaseStorage)
    {
        this.firebaseStorage = firebaseStorage ?? throw new ArgumentNullException(nameof(firebaseStorage));
    }

    public async Task<string> DownloadBudgetsCalcFileToBudgetsReportFolderAsync()
    {
        try
        {
            // Determine the AppData path
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appDirectory = Path.Combine(appDataPath, "BG-Menu");

            // Ensure the directory exists
            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
            }

            // Define the destination file path in the BudgetsReport folder
            string destinationFilePath = Path.Combine(appDirectory, "Targets.db");

            // Download the file from Firebase Storage to the BudgetsReport folder
            using (var fileStream = File.OpenWrite(destinationFilePath))
            {
                await firebaseStorage.DownloadObjectAsync(
                    "ableworld-ho-menu.appspot.com", // Replace with your Firebase Storage bucket name
                    "Budgets/Targets.db", // Replace with the path to your file in Firebase Storage
                    fileStream
                );
            }

            return destinationFilePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error downloading file from Firebase Storage: " + ex.Message);
            return null;
        }
    }

    public async Task<string> DownloadBudgetsAsync()
    {
        try
        {
            string filePath = await DownloadBudgetsCalcFileToBudgetsReportFolderAsync();

            if (filePath != null)
            {
                return filePath; // Return the file path if successful
            }
            else
            {
                MessageBox.Show("Failed to download the file.");
                return null;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error occurred while downloading the file: " + ex.Message);
            return null;
        }
    }

    public async Task<bool> UploadBudgetsCalcFileFromBudgetsReportFolderAsync(SQLiteConnection sqlConnection)
    {
        try
        {
            // Close the SQLite connection if it's open
            if (sqlConnection != null && sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appDirectory = Path.Combine(appDataPath, "BG-Menu");
            string sourceFilePath = Path.Combine(appDirectory, "Targets.db");

            if (!File.Exists(sourceFilePath))
            {
                MessageBox.Show("Database file not found. Please ensure the file exists before uploading.");
                return false;
            }

            using (var fileStream = File.OpenRead(sourceFilePath))
            {
                await firebaseStorage.UploadObjectAsync(
                    "ableworld-ho-menu.appspot.com",
                    "Budgets/Targets.db",
                    null,
                    fileStream
                );
            }

            MessageBox.Show("File uploaded successfully.");
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error uploading file to Firebase Storage: " + ex.Message);
            return false;
        }
    }
}
