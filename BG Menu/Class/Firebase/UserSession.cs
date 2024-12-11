using Google.Cloud.Firestore;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

public class UserSession
{
    private readonly AuthService authService;
    public bool IsAuthenticated { get; private set; }
    public string Username { get; private set; }
    public string Rank { get; private set; }
    public List<string> Permissions => authService.Permissions;


    public UserSession(AuthService authService)
    {
        this.authService = authService;
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        IsAuthenticated = await authService.LoginAsync(username, password);
        if (IsAuthenticated)
        {
            Username = username;
            await RetrieveUserRank(username);
            ExecuteActionsBasedOnPermissions();
        }
        else
        {
            MessageBox.Show("Invalid username or password.");
            
        }

        return IsAuthenticated;
    }

    private async Task RetrieveUserRank(string username)
    {
        try
        {
            // Access Firestore and get the user's document
            DocumentReference userDoc = authService.FirestoreDb.Collection("BG-Users").Document(username);
            DocumentSnapshot snapshot = await userDoc.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                // Retrieve the "Rank" field from the document and store it in the Rank property
                Rank = snapshot.GetValue<string>("Rank") ?? "No Rank"; // Default to "No Rank" if the field is missing
            }
            else
            {
                Rank = "No Rank";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to retrieve rank for user {username}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Rank = "No Rank"; // Set a default value in case of an error
        }
    }

    public static List<string> AvailablePermissions { get; } = new List<string>
    {
        "admin",
        "salesview",
        "IT Department",
        "networkmonitor",
        "servicetools",
        "fileserver",
        "stocktake",
        "mids/tids",
        "Sales/Budgets"
        
        // Add more permissions here as needed
    };

    private void ExecuteActionsBasedOnPermissions()
    {
        if (authService.HasPermission("admin"))
        {
            //MessageBox.Show("User has admin permission.");
            
        }

        if (authService.HasPermission("read"))
        {
            //MessageBox.Show("User has read permission.");
            
        }

        if (authService.HasPermission("homeview"))
        {
            //MessageBox.Show("User has homeview permission.");
            
        }        
    }    
}
