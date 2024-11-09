using BG_Menu.Class.Design;
using Google.Cloud.Firestore;
using System.Security.Cryptography;
using System.Text;


namespace BG_Menu.Forms.Sub_Forms
{
    public partial class AddUserForm : Form
    {
        private RoundedCorners roundedCorners;
        private FirestoreDb firestoreDb;
        private bool isEditMode = false;
        private string originalUsername;

        public AddUserForm(FirestoreDb db, string username = null, bool isEditMode = false)
        {
            InitializeComponent();
            firestoreDb = db;
            this.DoubleBuffered = true;
            var roundedCorners = new RoundedCorners(this, 70, 3, Color.Yellow);
            var LoginDraggable = new Draggable(this, this);
            this.isEditMode = isEditMode;

            if (isEditMode)
            {
                UsernameTextBox.Text = username;
                UsernameTextBox.ReadOnly = true; // Make the username read-only during self-editing
                LoadUserDetails(username); // Load details for the logged-in user
                AddUserButton.Text = "Edit User"; // Change the button text to indicate editing
                RankTextBox.ReadOnly = true;
            }

        }

        private async void LoadUserDetails(string username)
        {
            try
            {
                // Reference to the user document in Firestore
                DocumentReference userDoc = firestoreDb.Collection("BG-Users").Document(username);
                DocumentSnapshot snapshot = await userDoc.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    // Get the "Rank" field from the user's document and populate it in the form
                    string rank = snapshot.GetValue<string>("Rank");
                    RankTextBox.Text = rank;
                }
                else
                {
                    MessageBox.Show("User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void AddUserButton_Click(object sender, EventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordTextBox.Text.Trim();
            string rank = RankTextBox.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(rank) || (!isEditMode && string.IsNullOrEmpty(password)))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                DocumentReference userDoc = firestoreDb.Collection("BG-Users").Document(username);

                if (isEditMode)
                {
                    // In edit mode, update the user's data without checking if the user exists
                    var updates = new Dictionary<string, object>
                    {
                        { "Rank", rank }
                    };

                    // If the password is filled, update it
                    if (!string.IsNullOrEmpty(password))
                    {
                        string hashedPassword = HashPassword(password);
                        updates.Add("Password", hashedPassword);
                    }

                    // Perform a merge operation to update existing fields and leave others intact
                    await userDoc.SetAsync(updates, SetOptions.MergeAll);

                    MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Check if the username already exists when adding a new user
                    DocumentSnapshot snapshot = await userDoc.GetSnapshotAsync();

                    if (snapshot.Exists)
                    {
                        MessageBox.Show("Username already exists. Please choose a different username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string hashedPassword = HashPassword(password);

                    Dictionary<string, object> userData = new Dictionary<string, object>
                    {
                        { "Password", hashedPassword },
                        { "Rank", rank },
                        { "Permissions", new List<string>() }, // Empty array for permissions
                        { "Stores", new List<string>() }        // Empty array for stores
                    };

                    await userDoc.SetAsync(userData);

                    MessageBox.Show("User created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.Close(); // Close the form after operation
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public AddUserForm(FirestoreDb db, string username, string rank) : this(db)
        {
            isEditMode = true;
            originalUsername = username;

            UsernameTextBox.Text = username;
            RankTextBox.Text = rank;

            // Set the Username field to readonly
            UsernameTextBox.ReadOnly = true;

            // Change the button text to "Edit User"
            AddUserButton.Text = "Edit User";

            var roundedCorners = new RoundedCorners(this, 70);
            var LoginDraggable = new Draggable(this, this);
        }

        

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}