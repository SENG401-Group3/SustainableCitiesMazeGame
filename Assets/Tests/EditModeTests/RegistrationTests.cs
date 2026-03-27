using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

/// Verifies that input validation correctly accepts and rejects user input
public class RegistrationTests
{
    private Registration registration;

    [SetUp]
    public void SetUp()
    {
        var obj = new GameObject();
        registration = obj.AddComponent<Registration>();

        // Manually create UI fields (since UIDocument doesn't exist in tests)
        registration.GetType().GetField("firstnameInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(registration, new TextField());

        registration.GetType().GetField("lastnameInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(registration, new TextField());

        registration.GetType().GetField("usernameInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(registration, new TextField());

        registration.GetType().GetField("passwordInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(registration, new TextField());
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(registration.gameObject);
    }

    private void SetInputs(string first, string last, string user, string pass)
    {
        ((TextField)registration.GetType().GetField("firstnameInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(registration)).value = first;
        ((TextField)registration.GetType().GetField("lastnameInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(registration)).value = last;
        ((TextField)registration.GetType().GetField("usernameInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(registration)).value = user;
        ((TextField)registration.GetType().GetField("passwordInput", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(registration)).value = pass;
    }

    [Test]
    public void VerifyInputs_ValidInputs_ReturnsTrue()
    {
        SetInputs("John", "Doe", "johndoe1", "Password1!");
        Assert.IsTrue(registration.VerifyInputs());
    }

    [Test]
    public void VerifyInputs_EmptyFields_ReturnsFalse()
    {
        SetInputs("", "", "", "");
        Assert.IsFalse(registration.VerifyInputs());
    }

    [Test]
    public void VerifyInputs_ShortUsername_ReturnsFalse()
    {
        SetInputs("John", "Doe", "abc", "Password1!");
        Assert.IsFalse(registration.VerifyInputs());
    }

    [Test]
    public void VerifyInputs_ShortPassword_ReturnsFalse()
    {
        SetInputs("John", "Doe", "johndoe1", "Pass1!");
        Assert.IsFalse(registration.VerifyInputs());
    }

    [Test]
    public void VerifyInputs_NoUppercase_ReturnsFalse()
    {
        SetInputs("John", "Doe", "johndoe1", "password1!");
        Assert.IsFalse(registration.VerifyInputs());
    }

    [Test]
    public void VerifyInputs_NoSpecialChar_ReturnsFalse()
    {
        SetInputs("John", "Doe", "johndoe1", "Password1");
        Assert.IsFalse(registration.VerifyInputs());
    }
}
