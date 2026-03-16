using NUnit.Framework;

/// Verifies login and logout behaviour
public class DBManagerTests
{
    [SetUp]
    public void SetUp()
    {
        DBManager.firstname = null;
        DBManager.lastname = null;
        DBManager.username = null;
        DBManager.score = 0;
    }

    /// Verifies that LoggedIn returns false when no username is set
    [Test]
    public void LoggedIn_ReturnsFalse_WhenUsernameIsNull()
    {
        Assert.IsFalse(DBManager.LoggedIn);
    }

    /// Verifies that LoggedIn returns true when a username is set
    [Test]
    public void LoggedIn_ReturnsTrue_WhenUsernameIsSet()
    {
        DBManager.username = "testuser";
        Assert.IsTrue(DBManager.LoggedIn);
    }

    /// Verifies that LogOut clears the username field
    [Test]
    public void LogOut_SetsUsernameToNull()
    {
        DBManager.username = "testuser";
        DBManager.LogOut();
        Assert.IsNull(DBManager.username);
    }

    /// Verifies that LoggedIn returns false when LogOut is called
    [Test]
    public void LogOut_SetsLoggedInToFalse()
    {
        DBManager.username = "testuser";
        DBManager.LogOut();
        Assert.IsFalse(DBManager.LoggedIn);
    }
}