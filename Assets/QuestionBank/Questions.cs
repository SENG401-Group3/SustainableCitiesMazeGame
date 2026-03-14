using System;
using System.Collections.Generic;
using System.Linq;

public class Questions
{
    public string Id { get; set; }
    public string Category { get; set; } // Transport, Housing, Environment, Safety, and Tech
    public string Prompt { get; set; }
    public List<string> Choices { get; set; }
    public string CorrectAnswer { get; set; }
}

public static class QuestionsBank
{
    public const string PromptPrefix = "The city is changing. What would you do in the following case?";

    public static List<Questions> AllQuestions = new List<Questions>
    {
        new Questions{ Id="Q01", Category="Transport", Prompt="City has extra budget. Fund?", Choices=new List<string>{"A) Highways","B) Public Transit","C) Parking"}, CorrectAnswer="B"},
        new Questions{ Id="Q02", Category="Transport", Prompt="Traffic rising. First action?", Choices=new List<string>{"A) Add lanes","B) Improve buses","C) Ignore"}, CorrectAnswer="B"},
        new Questions{ Id="Q03", Category="Transport", Prompt="Unsafe school roads?", Choices=new List<string>{"A) Crosswalks","B) Parking","C) Nothing"}, CorrectAnswer="A"},
        new Questions{ Id="Q04", Category="Housing", Prompt="Housing demand grows?", Choices=new List<string>{"A) Affordable units","B) Luxury only","C) Stop builds"}, CorrectAnswer="A"},
        new Questions{ Id="Q05", Category="Housing", Prompt="Old unsafe buildings?", Choices=new List<string>{"A) Renovate","B) Demolish all","C) Leave"}, CorrectAnswer="A"},
        new Questions{ Id="Q06", Category="Environment", Prompt="Few parks exist?", Choices=new List<string>{"A) Build parks","B) Malls","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q07", Category="Environment", Prompt="Waste overflow?", Choices=new List<string>{"A) Recycling","B) Bigger landfill","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q08", Category="Environment", Prompt="Plastic trash high?", Choices=new List<string>{"A) Ban plastics","B) Promote plastic","C) No change"}, CorrectAnswer="A"},
        new Questions{ Id="Q09", Category="Safety", Prompt="Night safety issues?", Choices=new List<string>{"A) Lighting","B) Close parks","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q10", Category="Tech", Prompt="Disaster prep weak?", Choices=new List<string>{"A) Warning systems","B) No plan","C) Wait"}, CorrectAnswer="A"},
        new Questions{ Id="Q11", Category="Transport", Prompt="Bike accidents rise?", Choices=new List<string>{"A) Bike lanes","B) Ban bikes","C) Signs only"}, CorrectAnswer="A"},
        new Questions{ Id="Q12", Category="Housing", Prompt="Limited downtown space?", Choices=new List<string>{"A) Mixed use","B) Parking towers","C) Empty"}, CorrectAnswer="A"},
        new Questions{ Id="Q13", Category="Safety", Prompt="Disabled access poor?", Choices=new List<string>{"A) Ramps","B) No change","C) Stairs"}, CorrectAnswer="A"},
        new Questions{ Id="Q14", Category="Tech", Prompt="Citizen participation low?", Choices=new List<string>{"A) Town halls","B) Close meetings","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q15", Category="Environment", Prompt="Citizens want greenery?", Choices=new List<string>{"A) Urban forest","B) Parking","C) Billboards"}, CorrectAnswer="A"},
        new Questions{ Id="Q16", Category="Health", Prompt="Public health declining?", Choices=new List<string>{"A) Free clinics","B) Close hospitals","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q17", Category="Transport", Prompt="Air pollution rising?", Choices=new List<string>{"A) Car-free days","B) Cheap gas","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q18", Category="Transport", Prompt="New suburb transport?", Choices=new List<string>{"A) Bike lanes","B) Car roads","C) Helipads"}, CorrectAnswer="A"},
        new Questions{ Id="Q19", Category="Transport", Prompt="Transit too costly?", Choices=new List<string>{"A) Subsidize","B) Raise fares","C) Cancel"}, CorrectAnswer="A"},
        new Questions{ Id="Q20", Category="Transport", Prompt="High carbon emissions?", Choices=new List<string>{"A) EV incentives","B) Diesel promo","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q21", Category="Transport", Prompt="Commute times rising?", Choices=new List<string>{"A) Remote work","B) Longer hours","C) Close offices"}, CorrectAnswer="A"},
        new Questions{ Id="Q22", Category="Housing", Prompt="City sprawl?", Choices=new List<string>{"A) Dense housing","B) Endless suburbs","C) Stop permits"}, CorrectAnswer="A"},
        new Questions{ Id="Q23", Category="Housing", Prompt="Rent skyrockets?", Choices=new List<string>{"A) Rent control","B) Remove rules","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q24", Category="Housing", Prompt="Abandoned land?", Choices=new List<string>{"A) Community garden","B) Factory","C) Fence"}, CorrectAnswer="A"},
        new Questions{ Id="Q25", Category="Environment", Prompt="Heat waves increase?", Choices=new List<string>{"A) Plant trees","B) Remove green","C) Asphalt"}, CorrectAnswer="A"},
        new Questions{ Id="Q26", Category="Environment", Prompt="River pollution?", Choices=new List<string>{"A) Enforce fines","B) Allow dumping","C) Close river"}, CorrectAnswer="A"},
        new Questions{ Id="Q27", Category="Environment", Prompt="Air quality drops?", Choices=new List<string>{"A) Green roofs","B) Coal plants","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q28", Category="Environment", Prompt="Flood risk rising?", Choices=new List<string>{"A) Drainage","B) Build wetlands","C) Wait"}, CorrectAnswer="A"},
        new Questions{ Id="Q29", Category="Safety", Prompt="Crime increases?", Choices=new List<string>{"A) Community policing","B) Curfew only","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q30", Category="Safety", Prompt="Youth unemployment?", Choices=new List<string>{"A) Skills centers","B) Raise limits","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q31", Category="Safety", Prompt="Elderly isolation?", Choices=new List<string>{"A) Community hubs","B) Reduce services","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q32", Category="Tech", Prompt="Energy demand rising?", Choices=new List<string>{"A) Solar panels","B) Coal","C) Power cuts"}, CorrectAnswer="A"},
        new Questions{ Id="Q33", Category="Tech", Prompt="Smart city proposal?", Choices=new List<string>{"A) Digital traffic","B) Paper only","C) No change"}, CorrectAnswer="A"},
        new Questions{ Id="Q34", Category="Housing", Prompt="Homelessness increases?", Choices=new List<string>{"A) Expand shelters","B) Push out","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q35", Category="Housing", Prompt="Slums growing?", Choices=new List<string>{"A) Upgrade infra","B) Evict","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q36", Category="Housing", Prompt="Noise complaints?", Choices=new List<string>{"A) Zoning laws","B) Nightlife everywhere","C) Remove limits"}, CorrectAnswer="A"},
        new Questions{ Id="Q37", Category="Environment", Prompt="Wildlife disappearing?", Choices=new List<string>{"A) Protected zones","B) Highways","C) Hunting"}, CorrectAnswer="A"},
        new Questions{ Id="Q38", Category="Environment", Prompt="Factory offers jobs but pollutes?", Choices=new List<string>{"A) Strict rules","B) Approve","C) Hide data"}, CorrectAnswer="A"},
        new Questions{ Id="Q39", Category="Environment", Prompt="Drought threatens water?", Choices=new List<string>{"A) Conserve","B) Wait","C) Waste more"}, CorrectAnswer="A"},
        new Questions{ Id="Q40", Category="Safety", Prompt="Transit harassment reports?", Choices=new List<string>{"A) Safety staff","B) Dismiss","C) Reduce service"}, CorrectAnswer="A"},
        new Questions{ Id="Q41", Category="Safety", Prompt="Inequality rising?", Choices=new List<string>{"A) Social housing","B) Tax breaks rich","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q42", Category="Tech", Prompt="Camera system proposal?", Choices=new List<string>{"A) Privacy rules","B) No oversight","C) Sell footage"}, CorrectAnswer="A"},
        new Questions{ Id="Q43", Category="Tech", Prompt="Data privacy concerns?", Choices=new List<string>{"A) Strong laws","B) Sell data","C) Ignore"}, CorrectAnswer="A"},
        new Questions{ Id="Q44", Category="Tech", Prompt="Budget transparency demand?", Choices=new List<string>{"A) Public dashboards","B) Hidden budgets","C) Partial data"}, CorrectAnswer="A"},
        new Questions{ Id="Q45", Category="Tech", Prompt="Tech inequality?", Choices=new List<string>{"A) Free Wi-Fi","B) Paid only","C) Remove internet"}, CorrectAnswer="A"},
        new Questions{ Id="Q46", Category="Tech", Prompt="Urban data available?", Choices=new List<string>{"A) Open portal","B) Restrict","C) Delete"}, CorrectAnswer="A"},
        new Questions{ Id="Q47", Category="Tech", Prompt="Long-term planning?", Choices=new List<string>{"A) 20-year plan","B) Short profits","C) No plan"}, CorrectAnswer="A"},
        new Questions{ Id="Q48", Category="Transport", Prompt="Transit ridership low?", Choices=new List<string>{"A) Improve reliability","B) Cancel","C) Raise fares"}, CorrectAnswer="A"},
        new Questions{ Id="Q49", Category="Transport", Prompt="Businesses demand parking?", Choices=new List<string>{"A) Better transit","B) Replace parks","C) Parking only"}, CorrectAnswer="A"},
        new Questions{ Id="Q50", Category="Environment", Prompt="Severe heat island effect?", Choices=new List<string>{"A) Green roofs","B) More asphalt","C) Ignore"}, CorrectAnswer="A"},
    };

    public static List<string> GetShuffledChoices(Questions question, out int correctIndex)
    {
        // Strip the "A) ", "B) ", "C) " prefixes
        var stripped = question.Choices.Select(c => c.Substring(3)).ToList();

        // Remember the correct answer text before shuffling
        string correctText = stripped[question.Choices.FindIndex(c => c.StartsWith(question.CorrectAnswer + ")"))];

        // Shuffle
        var shuffled = stripped.OrderBy(_ => Guid.NewGuid()).ToList();

        // Find where the correct answer landed
        correctIndex = shuffled.FindIndex(c => c == correctText);

        return shuffled;
    }
}