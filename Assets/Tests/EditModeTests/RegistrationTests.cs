using NUnit.Framework;
using UnityEngine;

/// Verifies that input validation correctly accepts and rejects user input
public class RegistrationTests
{
    private Registration registration;

    [SetUp]
    public void SetUp()
    {
        var obj = new GameObject();
        obj.SetActive(false);
        registration = obj.AddComponent<Registration>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(registration.gameObject);
    }

    /// Verifies that valid inputs pass validation 
    [Test]
    public void VerifyInputs_ValidInputs_ReturnsTrue()
    {
        Assert.IsTrue(registration.VerifyInputs("John", "Doe", "johndoe1", "Password1!"));
    }

    /// Verifies that empty fields fail validation
    [Test]
    public void VerifyInputs_EmptyFields_ReturnsFalse()
    {
        Assert.IsFalse(registration.VerifyInputs("", "", "", ""));
    }
    
    /// Verifies that a username shorter than 6 characters fails validation
    [Test]
    public void VerifyInputs_ShortUsername_ReturnsFalse()
    {
        Assert.IsFalse(registration.VerifyInputs("John", "Doe", "abc", "Password1!"));
    }

    /// Verifies that a password shorter than 8 characters fails validation
    [Test]
    public void VerifyInputs_ShortPassword_ReturnsFalse()
    {
        Assert.IsFalse(registration.VerifyInputs("John", "Doe", "johndoe1", "Pass1!"));
    }

    /// Verifies that a password without an uppercase letter fails validation
    [Test]
    public void VerifyInputs_NoUppercase_ReturnsFalse()
    {
        Assert.IsFalse(registration.VerifyInputs("John", "Doe", "johndoe1", "password1!"));
    }

    /// Verifies that a password without a special character fails validation
    [Test]
    public void VerifyInputs_NoSpecialChar_ReturnsFalse()
    {
        Assert.IsFalse(registration.VerifyInputs("John", "Doe", "johndoe1", "Password1"));
    }
}