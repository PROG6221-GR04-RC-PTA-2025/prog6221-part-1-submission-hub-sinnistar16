using System;
using System.Media;
using System.Threading;

class CyberChat
{
    static void Main()
    {
        // Set the console title and text color
        Console.Title = "Cyber Chat: Your Security Assistant";
        Console.ForegroundColor = ConsoleColor.Cyan; // Vibrant Cyan for the welcome message
        Console.WriteLine("Welcome to Cyber Chat! Your friendly assistant for online safety.\n");

        ShowAsciiArt();
        PlayWelcomeSound();
        Console.ResetColor();

        // Asking for the user's name
        Console.ForegroundColor = ConsoleColor.Magenta; // Magenta for user interaction
        Console.Write("\nWhat is your name? ");
        string userName = Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.Green; // Green for the response
        Console.WriteLine($"\nHey {userName}, I'm Cyber Chat! Let's make sure you're safe online.\n");

        // Displaying the menu and handling user input
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow; // Yellow for the menu
            Console.WriteLine("\nChoose a topic or ask your question:");
            Console.WriteLine("[1] Phishing and Scams");
            Console.WriteLine("[2] Creating Strong Passwords");
            Console.WriteLine("[3] Internet Security Tips");
            Console.WriteLine("[4] Social Media Safety");
            Console.WriteLine("[5] Recognizing Malware");
            Console.WriteLine("[6] Exit");
            Console.ForegroundColor = ConsoleColor.White; // White for the prompt
            Console.Write("Your choice: ");
            Console.ResetColor();

            string userInput = Console.ReadLine().ToLower().Trim();

            if (userInput == "6" || userInput == "exit") break;

            HandleUserChoice(userInput);
        }

        // Farewell message
        Console.ForegroundColor = ConsoleColor.Red; // Red for the goodbye message
        Console.WriteLine("\nGoodbye! Stay safe online!");
        PlayGoodbyeSound();
        Console.ResetColor();
    }

    // New ASCII art design
    static void ShowAsciiArt()
    {
        Console.ForegroundColor = ConsoleColor.Blue; // Blue for the ASCII art
        Console.WriteLine(@"
  
 ░▒▓██████▓▒░░▒▓█▓▒░░▒▓█▓▒░▒▓███████▓▒░░▒▓████████▓▒░▒▓███████▓▒░        ░▒▓██████▓▒░░▒▓█▓▒░░▒▓█▓▒░░▒▓██████▓▒░▒▓████████▓▒░ 
░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░     
░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░      ░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░     
░▒▓█▓▒░       ░▒▓██████▓▒░░▒▓███████▓▒░░▒▓██████▓▒░ ░▒▓███████▓▒░       ░▒▓█▓▒░      ░▒▓████████▓▒░▒▓████████▓▒░ ░▒▓█▓▒░     
░▒▓█▓▒░         ░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░      ░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░     
░▒▓█▓▒░░▒▓█▓▒░  ░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░      ░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░     
 ░▒▓██████▓▒░   ░▒▓█▓▒░   ░▒▓███████▓▒░░▒▓████████▓▒░▒▓█▓▒░░▒▓█▓▒░       ░▒▓██████▓▒░░▒▓█▓▒░░▒▓█▓▒░▒▓█▓▒░░▒▓█▓▒░ ░▒▓█▓▒░     
                                                                                                                             
                                                                                                                             

                                          
  Cyber Chat: Your Security Assistant
        ");
        Console.ResetColor();
    }

    // Playing the welcome sound (wav file)
    static void PlayWelcomeSound()
    {
        try
        {
            // Full file path for the sound
            string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");

            // Check if the file exists before attempting to play it
            if (System.IO.File.Exists(soundPath))
            {
                SoundPlayer player = new SoundPlayer(soundPath);
                player.PlaySync();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed; // Dark Red for errors
                Console.WriteLine("[Audio Error] Cannot find the greeting.wav at the path: " + soundPath);
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed; // Dark Red for errors
            Console.WriteLine("[Audio Error] Unable to play the greeting sound: " + ex.Message);
            Console.ResetColor();
        }
    }

    // Playing the goodbye sound (wav file)
    static void PlayGoodbyeSound()
    {
        try
        {
            // Full file path for the goodbye sound
            string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "goodbye.wav");

            // Check if the file exists before attempting to play it
            if (System.IO.File.Exists(soundPath))
            {
                SoundPlayer player = new SoundPlayer(soundPath);
                player.PlaySync();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed; // Dark Red for errors
                Console.WriteLine("[Audio Error] Cannot find the goodbye.wav at the path: " + soundPath);
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed; // Dark Red for errors
            Console.WriteLine("[Audio Error] Unable to play the goodbye sound: " + ex.Message);
            Console.ResetColor();
        }
    }

    // Handling user input and providing corresponding information
    static void HandleUserChoice(string choice)
    {
        Console.ForegroundColor = ConsoleColor.Green; // Green for responses
        switch (choice)
        {
            case "1":
            case "phishing":
            case "what is phishing":
                SimulateTyping("Phishing is a deceptive attempt to steal sensitive data like passwords or credit card info through fake emails or websites.");
                break;
            case "2":
            case "strong passwords":
            case "how to create a strong password":
                SimulateTyping("A strong password should have at least 12 characters and include a combination of upper and lowercase letters, numbers, and special characters.");
                break;
            case "3":
            case "internet security":
            case "how to stay secure online":
                SimulateTyping("Use unique passwords for each account, enable two-factor authentication, and always browse HTTPS websites.");
                break;
            case "4":
            case "social media safety":
            case "how to stay safe on social media":
                SimulateTyping("Avoid sharing personal information, use privacy settings, and be cautious of friend requests from strangers.");
                break;
            case "5":
            case "recognizing malware":
            case "how to recognize malware":
                SimulateTyping("Look for unusual system behavior, unexpected pop-ups, or slow performance. Use antivirus software to scan your system.");
                break;
            case "how are you?":
            case "how are you":
                SimulateTyping("I'm just a program, but I'm here and ready to help you stay safe online!");
                break;
            case "what’s your purpose?":
            case "what is your purpose":
                SimulateTyping("My purpose is to assist you with online safety tips and answer your questions about cybersecurity.");
                break;
            case "what can i ask you about?":
            case "what can i ask":
                SimulateTyping("You can ask me about phishing, strong passwords, internet security, social media safety, recognizing malware, and more!");
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red; // Red for invalid input
                SimulateTyping("Sorry, I didn't catch that. Please select a valid option or rephrase your question.");
                break;
        }
        Console.ResetColor();
    }

    // Simulating the typing effect for messages
    static void SimulateTyping(string message)
    {
        foreach (char letter in message)
        {
            Console.Write(letter);
            Thread.Sleep(40); // Simulated typing speed
        }
        Console.WriteLine("\n");
    }
}
