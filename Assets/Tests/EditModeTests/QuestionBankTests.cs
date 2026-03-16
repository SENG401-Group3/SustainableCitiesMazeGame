using System.Collections.Generic;
using NUnit.Framework;

/// Verifies question data and shuffle logic
public class QuestionsBankTests
{
    /// Verifies that the question bank is not empty
    [Test]
    public void AllQuestions_IsNotEmpty()
    {
        Assert.Greater(QuestionsBank.AllQuestions.Count, 0);
    }

    /// Verifies that every question has exactly three answer choices
    [Test]
    public void AllQuestions_EachHasThreeChoices()
    {
        foreach (var q in QuestionsBank.AllQuestions)
            Assert.AreEqual(3, q.Choices.Count, $"Question {q.Id} does not have 3 choices");
    }

    /// Verifies that every question has a non-empty prompt
    [Test]
    public void AllQuestions_EachHasNonEmptyPrompt()
    {
        foreach (var q in QuestionsBank.AllQuestions)
            Assert.IsFalse(string.IsNullOrEmpty(q.Prompt), $"Question {q.Id} has empty prompt");
    }

    /// Verifies that GetShuffledChoices returns exactly three choices
    [Test]
    public void GetShuffledChoices_ReturnsThreeChoices()
    {
        var q = QuestionsBank.AllQuestions[0];
        var shuffled = QuestionsBank.GetShuffledChoices(q, out int correctIndex);

        Assert.AreEqual(3, shuffled.Count);
    }
}