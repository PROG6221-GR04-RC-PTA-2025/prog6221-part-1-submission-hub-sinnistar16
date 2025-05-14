// Cyber Chat Bot - Enhanced with Memory Game and Natural Conversation
using System;
using System.Collections.Generic;
using System.Media;
using System.Threading;
using System.IO;

class CyberChat
{
    static string userName = "User";
    static string userInterest = null;
    static string lastTopic = null;

    static Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>()
    {
        { "password", new List<string>{
            "Use a mix of letters, numbers, and special characters.",
            "Avoid using birthdays or names in passwords.",
            "Use a password manager to keep track of your credentials." }},
        { "scam", new List<string>{
            "Watch out for emails with urgent language asking for personal info.",
            "Never click on links from unknown senders.",
            "Report suspicious emails to your service provider." }},
        { "privacy", new List<string>{
            "Adjust your privacy settings on all social platforms.",
            "Limit the amount of personal info you share online.",
            "Regularly review app permissions on your devices." }},
        { "phishing", new List<string>{
            "Be cautious of emails asking for personal information.",
            "Check the sender's email address before clicking any links.",
            "Legitimate companies don't request sensitive information via email." }}
    };

    static Dictionary<string, string> sentimentResponses = new Dictionary<string, string>
    {
        { "worried", "It's okay to feel worried — cybersecurity can be scary, but I'm here to help." },
        { "curious", "Curiosity is the first step to becoming cyber smart. Ask me anything!" },
        { "frustrated", "Take a deep breath. I'll try to make things clearer for you." }
    };

    static List<string> funFacts = new List<string>
    {
        "Did you know? The first computer virus was created in 1986.",
        "Cybercrime damages are predicted to hit $10.5 trillion annually by 2025.",
        "Most hacking is done through phishing, not technical skill!",
        "Public Wi-Fi can be dangerous. Always use a VPN when possible."
    };

    static void Main()
    {
        Console.Title = "Cyber Chat: Your Security Assistant";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Welcome to Cyber Chat! Your friendly assistant for online safety.\n");
        ShowAsciiArt();
        PlaySound("greeting.wav");

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("\nWhat is your name? ");
        userName = Console.ReadLine();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nHey {userName}, I'm Cyber Chat! Let's make sure you're safe online.\n");

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nYou can ask a question or choose an option:");
            Console.WriteLine("[1] Phishing and Scams\n[2] Creating Strong Passwords\n[3] Internet Security Tips\n[4] Social Media Safety\n[5] Recognizing Malware\n[6] Cyber Memory Game\n[7] Random Cyber Fact\n[8] Exit");
            Console.Write("Your choice: ");
            Console.ResetColor();

            string input = Console.ReadLine().ToLower().Trim();
            if (input == "8" || input == "exit") break;

            if (!HandlePredefinedChoices(input))
                HandleDynamicConversation(input);
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nGoodbye! Stay safe online!");
        PlaySound("goodbye.wav");
        Console.ResetColor();
    }

    static bool HandlePredefinedChoices(string input)
    {
        switch (input)
        {
            case "1": lastTopic = "scam"; SimulateTyping(GetRandomResponse("scam")); return true;
            case "2": lastTopic = "password"; SimulateTyping(GetRandomResponse("password")); return true;
            case "3": lastTopic = "security"; SimulateTyping("Use strong passwords, update software, and avoid suspicious links."); return true;
            case "4": lastTopic = "social media"; SimulateTyping("Limit what you share, check privacy settings, and report fake accounts."); return true;
            case "5": lastTopic = "malware"; SimulateTyping("Watch for sudden pop-ups, slow performance, or unauthorized changes."); return true;
            case "6": PlayMemoryGame(); return true;
            case "7": SimulateTyping(GetRandomFact()); return true;
            default: return false;
        }
    }

    static void HandleDynamicConversation(string input)
    {
        // Handle thank you phrases
        if (input.Contains("thank you") || input.Contains("thanks"))
        {
            SimulateTyping("You're very welcome! I'm here whenever you need cyber help.");
            return;
        }

        // Handle name recall
        if (input.Contains("what was my name") || input.Contains("my name"))
        {
            SimulateTyping($"Your name is {userName}. I never forget!");
            return;
        }

        // Handle topic recall
        if (input.Contains("our last topic") || input.Contains("last topic"))
        {
            if (lastTopic != null)
            {
                SimulateTyping($"We last talked about {lastTopic}, {userName}. Want to continue?");
            }
            else
            {
                SimulateTyping("We haven't discussed anything specific yet.");
            }
            return;
        }

        // Handle random facts request
        if (input.Contains("random fact") || input.Contains("fun fact") || 
            (input.Contains("fact") && input.Contains("cyber")))
        {
            SimulateTyping(GetRandomFact());
            return;
        }

        // Check for sentiments
        bool sentimentFound = false;
        foreach (var sentiment in sentimentResponses.Keys)
        {
            if (input.Contains(sentiment))
            {
                SimulateTyping(sentimentResponses[sentiment]);
                sentimentFound = true;
            }
        }
        if (sentimentFound) return;

        // Check for interest expressions
        if (input.Contains("interested in") || input.Contains("interest is in") || 
            input.Contains("my interest is"))
        {
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    userInterest = keyword;
                    lastTopic = keyword;
                    SimulateTyping($"Thanks for sharing that you're interested in {keyword}, {userName}! Here's a tip:");
                    SimulateTyping(GetRandomResponse(keyword));
                    return;
                }
            }
        }

        // Check for direct keyword mentions
        foreach (var keyword in keywordResponses.Keys)
        {
            if (input.Contains(keyword))
            {
                userInterest = keyword;
                lastTopic = keyword;
                SimulateTyping($"Thanks for asking about {keyword}, {userName}! Here's a tip:");
                SimulateTyping(GetRandomResponse(keyword));
                return;
            }
        }

        // Handle existing interest
        if (input.Contains("my interest") && userInterest != null)
        {
            SimulateTyping($"As someone interested in {userInterest}, you might want to dig deeper into your account settings and security options.");
            return;
        }

        // Handle specific worry about passwords
        if (input.Contains("i'm worried about my password") || input.Contains("worried about password"))
        {
            SimulateTyping("It's normal to worry! Here's a strong password tip:");
            SimulateTyping(GetRandomResponse("password"));
            return;
        }

        SimulateTyping("I'm not sure I understand. Could you try rephrasing your question? You can also choose an option from the menu.");
    }

    static string GetRandomResponse(string key)
    {
        var responses = keywordResponses[key];
        Random rand = new Random();
        return responses[rand.Next(responses.Count)];
    }

    static string GetRandomFact()
    {
        Random rand = new Random();
        return funFacts[rand.Next(funFacts.Count)];
    }

    static void SimulateTyping(string message)
    {
        foreach (char letter in message)
        {
            Console.Write(letter);
            Thread.Sleep(30);
        }
        Console.WriteLine("\n");
    }

    static void ShowAsciiArt()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(@"
  
 ░▒▓██████▓▒░░▒▓█▓▒░░▒▓█▓▒░▒▓███████▓▒░░▒▓████████▓▒░▒▓███████▓▒░        ░▒▓██████▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓██████▓▒░▒▓████████▓▒░ 
░▒▓█▓▒░░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░     
░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░      ░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░     
░▒▓█▓▒░       ░▒▓██████▓▒░░▒▓███████▓▒░░▒▓████████▓▒░ ░▒▓███████▓▒░       ░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░     
░▒▓█▓▒░         ░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░     
░▒▓█▓▒░░▒▓█▓▒░  ░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░     
 ░▒▓██████▓▒░   ░▒▓█▓▒░   ░▒▓███████▓▒░░▒▓████████▓▒░▒▓█▓▒░░▒▓█▓▒░       ░▒▓██████▓▒░░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░     
                                 
                                                             ");
        Console.WriteLine("\nCyber Chat: Your Security Assistant\n");
        Console.ResetColor();
    }

    static void PlaySound(string fileName)
    {
        try
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(path))
            {
                new SoundPlayer(path).PlaySync();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"[Audio Error] {fileName} not found.");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[Audio Error] " + ex.Message);
            Console.ResetColor();
        }
    }

    static void PlayMemoryGame()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n🧠 Cyber Memory Game - Match the Tips!\n");

        Dictionary<string, string> gamePairs = new Dictionary<string, string>
        {
            { "VPN", "A tool to protect your identity online" },
            { "Phishing", "Fake messages to steal info" },
            { "2FA", "Extra login protection" },
            { "Firewall", "Blocks unauthorized access" }
        };

        foreach (var pair in gamePairs)
        {
            Console.WriteLine($"Match: What does '{pair.Key}' mean?");
            Thread.Sleep(1000);
            Console.Write("Your answer: ");
            string answer = Console.ReadLine().ToLower();

            if (pair.Value.ToLower().Contains(answer))
            {
                SimulateTyping($"Correct! '{pair.Key}' means {pair.Value}");
            }
            else
            {
                SimulateTyping($"Oops! '{pair.Key}' means {pair.Value}");
            }
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nGreat job! Stay cyber-smart!");
        Console.ResetColor();
    }
}