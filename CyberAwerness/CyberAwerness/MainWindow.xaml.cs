using System.IO;
using System.Media;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CyberChatBotWPF
{
    public partial class MainWindow : Window
    {
        // User and Game State
        private string userName = "User";
        private int quizScore = 0, quizLevel = 1;
        private int gameScore = 0, gameLevel = 1;
        private bool inQuiz = false, inGame = false;
        private int quizQuestionIndex = 0;
        private string currentGameChallenge = null;
        private System.Timers.Timer reminderTimer;

        // Data Storage
        private readonly List<string> activityLog = new();
        private readonly List<CyberTask> userTasks = new();
        private readonly Dictionary<string, DateTime> reminders = new();
        private readonly Dictionary<int, List<QuizQuestion>> quizLevels = new();
        private readonly Dictionary<int, List<GameChallenge>> gameLevels = new();

        // NLP Keywords
        private readonly Dictionary<string, string[]> intentKeywords = new()
        {
            ["task"] = new[] { "add task", "create task", "new task", "task", "todo", "remember to", "remind me to" },
            ["quiz"] = new[] { "quiz", "test", "start quiz", "cybersecurity quiz", "security test" },
            ["game"] = new[] { "game", "play", "mini game", "cyber game", "challenge" },
            ["help"] = new[] { "help", "what can you do", "commands", "how to use", "instructions" },
            ["log"] = new[] { "activity log", "log", "history", "what have you done", "recent actions" },
            ["tasks"] = new[] { "show tasks", "view tasks", "my tasks", "task list", "todo list" },
            ["reminder"] = new[] { "remind me", "set reminder", "reminder", "alert me" }
        };

        // Auto-responses for natural conversation
        private readonly Dictionary<string, string[]> autoResponses = new()
        {
            ["greeting"] = new[] { "hello", "hi", "hey", "good morning", "good afternoon", "good evening" },
            ["thanks"] = new[] { "thank you", "thanks", "appreciate", "grateful" },
            ["bye"] = new[] { "bye", "goodbye", "see you", "exit", "quit" },
            ["password"] = new[] { "password", "passwords", "strong password", "secure password" },
            ["phishing"] = new[] { "phishing", "fake email", "suspicious email", "scam email" },
            ["malware"] = new[] { "malware", "virus", "trojan", "ransomware", "antivirus" },
            ["vpn"] = new[] { "vpn", "virtual private network", "secure connection" },
            ["2fa"] = new[] { "2fa", "two factor", "two-factor authentication", "multi factor" }
        };

        public MainWindow()
        {
            InitializeComponent();
            InitializeQuizzes();
            InitializeGames();
            InitializeReminderSystem();
            ShowTypingIndicator();
            PlayGreeting();
            BotSay("👋 Hey there! I'm your cybersecurity assistant. I can help you with security tasks, quizzes, games and more!");
            Task.Delay(1500).ContinueWith(_ => Dispatcher.Invoke(() =>
            {
                BotSay("Try saying 'help' or click the 📎 button for quick actions! 🚀");
            }));
        }
        private void PlayGreeting()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");
            if (File.Exists(filePath))
            {
                SoundPlayer player = new(filePath);
                player.Play();
            }
            else
            {
                MessageBox.Show("Greeting sound file not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #region Initialization Methods
        private void InitializeReminderSystem()
        {
            reminderTimer = new System.Timers.Timer(30000); // Check every 30 seconds
            reminderTimer.Elapsed += CheckReminders;
            reminderTimer.AutoReset = true;
            reminderTimer.Start();
        }

        private void InitializeQuizzes()
        {
            quizLevels[1] = new List<QuizQuestion>
            {
                new("What does 2FA stand for?",
                    new[] {"Two-Factor Authentication", "Fast Access", "File Authentication", "Fake Authentication"},
                    0, "2FA adds an extra layer of security to your accounts!"),

                new("What's the main purpose of phishing?",
                    new[] {"To catch fish", "To steal personal information", "To send newsletters", "To block websites"},
                    1, "Phishing attacks try to trick you into giving away sensitive information."),

                new("Which makes a password stronger?",
                    new[] {"abc123", "Pa$$w0rd123!", "password", "12345"},
                    1, "Strong passwords use a mix of letters, numbers, and symbols!"),

                new("What should you do with suspicious emails?",
                    new[] {"Open all attachments", "Delete or report them", "Forward to friends", "Reply immediately"},
                    1, "Never open suspicious attachments - always delete or report phishing attempts!"),

                new("What does HTTPS mean for website security?",
                    new[] {"It's slower", "It's encrypted and secure", "It's free", "It has more ads"},
                    1, "HTTPS encrypts data between your browser and the website!")
            };

            quizLevels[2] = new List<QuizQuestion>
            {
                new("What's a VPN used for?",
                    new[] {"Playing games", "Hiding your IP address", "Sending emails", "Making calls"},
                    1, "VPNs create secure, private connections over the internet!"),

                new("How often should you update software?",
                    new[] {"Never", "Once a year", "Regularly when updates are available", "Only when it breaks"},
                    2, "Regular updates patch security vulnerabilities!"),

                new("What's social engineering?",
                    new[] {"Building websites", "Manipulating people to reveal information", "Programming", "Network setup"},
                    1, "Social engineering exploits human psychology rather than technical vulnerabilities."),

                new("What's the best backup strategy?",
                    new[] {"No backups needed", "One backup on same device", "Multiple backups in different locations", "Email yourself files"},
                    2, "Follow the 3-2-1 rule: 3 copies, 2 different media, 1 offsite!"),

                new("What should you do on public WiFi?",
                    new[] {"Use banking apps freely", "Avoid sensitive activities", "Share the password", "Download everything"},
                    1, "Public WiFi is not secure - avoid sensitive activities or use a VPN!")
            };
        }

        private void InitializeGames()
        {
            gameLevels[1] = new List<GameChallenge>
            {
                new("I protect your privacy online by hiding your IP address. What am I?", "vpn"),
                new("I'm a security feature that requires two forms of identification. What am I?", "2fa"),
                new("I'm a fake email designed to steal your information. What am I?", "phishing"),
                new("I'm software that protects against malicious programs. What am I?", "antivirus"),
                new("I'm a secure protocol that encrypts web traffic. What am I?", "https")
            };

            gameLevels[2] = new List<GameChallenge>
            {
                new("I'm a type of malware that locks your files and demands payment. What am I?", "ransomware"),
                new("I'm a security barrier that monitors network traffic. What am I?", "firewall"),
                new("I'm a technique where attackers manipulate people psychologically. What am I?", "social engineering"),
                new("I'm a copy of your data stored separately for safety. What am I?", "backup"),
                new("I'm a secret code that should be unique and complex. What am I?", "password")
            };
        }
        #endregion

        #region UI Event Handlers
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                e.Handled = true;
                ProcessUserInput();
            }
        }

        private void QuickActions_Click(object sender, RoutedEventArgs e)
        {
            QuickActionsPopup.IsOpen = !QuickActionsPopup.IsOpen;
        }

        private void StartQuizAction_Click(object sender, RoutedEventArgs e)
        {
            QuickActionsPopup.IsOpen = false;
            StartQuiz();
        }

        private void StartGameAction_Click(object sender, RoutedEventArgs e)
        {
            QuickActionsPopup.IsOpen = false;
            StartGame();
        }

        private void ViewTasksAction_Click(object sender, RoutedEventArgs e)
        {
            QuickActionsPopup.IsOpen = false;
            ShowTasks();
        }

        private void ViewLogAction_Click(object sender, RoutedEventArgs e)
        {
            QuickActionsPopup.IsOpen = false;
            ShowActivityLog();
        }

        private void ShowHelpAction_Click(object sender, RoutedEventArgs e)
        {
            QuickActionsPopup.IsOpen = false;
            ShowHelp();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Input Processing and NLP
        private void ProcessUserInput()
        {
            string input = UserInputBox.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            AddUserMessage(input);
            UserInputBox.Clear();

            ShowTypingIndicator();

            // Simulate processing delay for natural feel
            Task.Delay(800).ContinueWith(_ => Dispatcher.Invoke(() =>
            {
                RemoveTypingIndicator();
                ProcessInput(input);
                ScrollToBottom();
            }));
        }

        private void ProcessInput(string input)
        {
            string lowerInput = input.ToLower();

            // Handle active quiz or game states first
            if (inQuiz)
            {
                HandleQuiz(input);
                return;
            }
            if (inGame)
            {
                HandleGame(input);
                return;
            }

            // Auto-responses for natural conversation
            if (HandleAutoResponses(lowerInput)) return;

            // Process intents using NLP simulation
            string intent = DetectIntent(lowerInput);

            switch (intent)
            {
                case "task":
                    HandleTaskIntent(input);
                    break;
                case "quiz":
                    StartQuiz();
                    break;
                case "game":
                    StartGame();
                    break;
                case "help":
                    ShowHelp();
                    break;
                case "log":
                    ShowActivityLog();
                    break;
                case "tasks":
                    ShowTasks();
                    break;
                case "reminder":
                    HandleReminderIntent(input);
                    break;
                default:
                    if (!HandleCybersecurityQuery(input))
                    {
                        HandleFallback();
                    }
                    break;
            }
        }

        private string DetectIntent(string input)
        {
            foreach (var intent in intentKeywords)
            {
                if (intent.Value.Any(keyword => input.Contains(keyword)))
                {
                    return intent.Key;
                }
            }
            return "unknown";
        }

        private bool HandleAutoResponses(string input)
        {
            if (autoResponses["greeting"].Any(g => input.Contains(g)))
            {
                BotSay($"Hello! 👋 How can I help you stay secure today?");
                return true;
            }
            if (autoResponses["thanks"].Any(t => input.Contains(t)))
            {
                BotSay("You're welcome! 😊 Stay safe out there!");
                return true;
            }
            if (autoResponses["bye"].Any(b => input.Contains(b)))
            {
                BotSay("Goodbye! Remember to stay cyber-safe! 🛡️");
                return true;
            }
            return false;
        }

        private bool HandleCybersecurityQuery(string input)
        {
            string lowerInput = input.ToLower();

            if (autoResponses["password"].Any(p => lowerInput.Contains(p)))
            {
                BotSay("🔐 Strong passwords should be at least 12 characters with uppercase, lowercase, numbers, and symbols!");
                BotSay("💡 Consider using a password manager to generate and store unique passwords.");
                AddLog("Provided password security advice");
                return true;
            }

            if (autoResponses["phishing"].Any(p => lowerInput.Contains(p)))
            {
                BotSay("🎣 Phishing emails try to steal your information! Watch for:");
                BotSay("• Urgent requests for personal info\n• Suspicious links or attachments\n• Poor grammar/spelling");
                BotSay("Always verify the sender before clicking anything!");
                AddLog("Provided phishing awareness info");
                return true;
            }

            if (autoResponses["malware"].Any(m => lowerInput.Contains(m)))
            {
                BotSay("🦠 Malware includes viruses, trojans, ransomware, and spyware.");
                BotSay("Protect yourself with:\n• Updated antivirus software\n• Regular system updates\n• Careful downloading");
                AddLog("Provided malware protection info");
                return true;
            }

            if (autoResponses["vpn"].Any(v => lowerInput.Contains(v)))
            {
                BotSay("🔒 VPNs encrypt your internet connection and hide your IP address.");
                BotSay("Great for:\n• Public WiFi security\n• Privacy protection\n• Accessing geo-blocked content");
                AddLog("Provided VPN information");
                return true;
            }

            if (autoResponses["2fa"].Any(t => lowerInput.Contains(t)))
            {
                BotSay("🔐 Two-Factor Authentication adds extra security to your accounts!");
                BotSay("Enable 2FA using:\n• SMS codes\n• Authenticator apps\n• Hardware keys");
                AddLog("Provided 2FA information");
                return true;
            }

            return false;
        }

        private bool HandleFallback()
        {
            string[] fallbacks = {
                "I'm here to help with cybersecurity! Try asking about passwords, phishing, or type 'help' for options.",
                "Not sure about that, but I can help you with security tasks, quizzes, or cyber advice! 🛡️",
                "Let me know how I can help keep you secure online! Type 'help' to see what I can do.",
                "I specialize in cybersecurity assistance. Ask me about online safety or try the quiz! 🧠"
            };

            BotSay(fallbacks[new Random().Next(fallbacks.Length)]);
            return true;
        }
        #endregion

        #region Task Management
        private void HandleTaskIntent(string input)
        {
            // Extract task from input using simple NLP
            string taskDescription = ExtractTaskFromInput(input);

            if (string.IsNullOrEmpty(taskDescription))
            {
                BotSay("What cybersecurity task would you like me to help you remember?");
                BotSay("💡 Examples: 'Enable 2FA on email', 'Update browser', 'Check privacy settings'");
                return;
            }

            var task = new CyberTask
            {
                Id = Guid.NewGuid(),
                Title = taskDescription,
                Description = GenerateTaskDescription(taskDescription),
                CreatedDate = DateTime.Now,
                IsCompleted = false
            };

            userTasks.Add(task);
            AddLog($"Task added: {task.Title}");

            BotSay($"✅ Task added: '{task.Title}'");
            BotSay("Would you like to set a reminder for this task? (e.g., 'remind me in 2 days')");
        }

        private string ExtractTaskFromInput(string input)
        {
            // Remove common task keywords and extract the actual task
            string[] taskKeywords = { "add task", "create task", "new task", "task", "remember to", "remind me to" };

            string cleaned = input;
            foreach (string keyword in taskKeywords)
            {
                cleaned = Regex.Replace(cleaned, keyword, "", RegexOptions.IgnoreCase).Trim();
            }

            // Remove common prepositions and articles
            cleaned = Regex.Replace(cleaned, @"^(to|the|a|an|my)\s+", "", RegexOptions.IgnoreCase);

            return cleaned.Trim();
        }

        private string GenerateTaskDescription(string title)
        {
            var descriptions = new Dictionary<string, string>
            {
                ["2fa"] = "Set up two-factor authentication to add an extra layer of security to your accounts.",
                ["password"] = "Create strong, unique passwords and consider using a password manager.",
                ["update"] = "Keep your software updated to patch security vulnerabilities.",
                ["backup"] = "Regularly backup your important data to protect against loss.",
                ["privacy"] = "Review and adjust your privacy settings on social media and online accounts.",
                ["antivirus"] = "Install and maintain up-to-date antivirus software.",
                ["firewall"] = "Enable and configure your firewall for network protection."
            };

            string lowerTitle = title.ToLower();
            foreach (var desc in descriptions)
            {
                if (lowerTitle.Contains(desc.Key))
                    return desc.Value;
            }

            return $"Complete the cybersecurity task: {title}";
        }

        private void HandleReminderIntent(string input)
        {
            var match = Regex.Match(input, @"remind me (?:to\s+)?(.+?)\s+in\s+(\d+)\s+(minute|minutes|hour|hours|day|days)", RegexOptions.IgnoreCase);

            if (match.Success)
            {
                string task = match.Groups[1].Value.Trim();
                int amount = int.Parse(match.Groups[2].Value);
                string unit = match.Groups[3].Value.ToLower();

                DateTime reminderTime = DateTime.Now;
                if (unit.StartsWith("minute")) reminderTime = reminderTime.AddMinutes(amount);
                else if (unit.StartsWith("hour")) reminderTime = reminderTime.AddHours(amount);
                else if (unit.StartsWith("day")) reminderTime = reminderTime.AddDays(amount);

                reminders[task] = reminderTime;
                AddLog($"Reminder set: {task} at {reminderTime:MMM dd, HH:mm}");

                BotSay($"⏰ Reminder set for '{task}' in {amount} {unit}!");
                BotSay($"I'll notify you on {reminderTime:MMM dd} at {reminderTime:HH:mm}");
            }
            else
            {
                BotSay("I can set reminders! Try saying:");
                BotSay("'Remind me to update passwords in 2 days'");
                BotSay("'Remind me to check privacy settings in 1 hour'");
            }
        }

        private void ShowTasks()
        {
            if (!userTasks.Any())
            {
                BotSay("📋 You don't have any tasks yet!");
                BotSay("Try adding one: 'Add task to enable 2FA on email'");
                return;
            }

            BotSay("📋 Your Cybersecurity Tasks:");

            foreach (var task in userTasks.Where(t => !t.IsCompleted).Take(5))
            {
                string status = task.IsCompleted ? "✅" : "⏳";
                BotSay($"{status} {task.Title}");
                if (!string.IsNullOrEmpty(task.Description))
                    BotSay($"   💡 {task.Description}");
            }

            AddLog("Viewed task list");
        }
        #endregion

        #region Quiz System
        private void StartQuiz()
        {
            quizLevel = 1;
            quizScore = 0;
            quizQuestionIndex = 0;
            inQuiz = true;

            BotSay("🧠 Starting Cybersecurity Quiz - Level 1!");
            BotSay("Test your knowledge and learn important security concepts!");
            AddLog("Started cybersecurity quiz");

            Task.Delay(1000).ContinueWith(_ => Dispatcher.Invoke(AskQuizQuestion));
        }

        private void AskQuizQuestion()
        {
            if (quizQuestionIndex >= quizLevels[quizLevel].Count)
            {
                CompleteQuizLevel();
                return;
            }

            var question = quizLevels[quizLevel][quizQuestionIndex];
            BotSay($"❓ Question {quizQuestionIndex + 1}: {question.Question}");

            for (int i = 0; i < question.Options.Length; i++)
            {
                BotSay($"{i + 1}. {question.Options[i]}");
            }

            BotSay("💬 Type the number of your answer (1, 2, 3, or 4)");
        }

        private void HandleQuiz(string input)
        {
            if (!int.TryParse(input.Trim(), out int answer) || answer < 1 || answer > 4)
            {
                BotSay("❌ Please enter a valid number (1, 2, 3, or 4)");
                return;
            }

            var question = quizLevels[quizLevel][quizQuestionIndex];
            bool isCorrect = (answer - 1) == question.CorrectAnswer;

            if (isCorrect)
            {
                quizScore++;
                BotSay("✅ Correct! Well done!");
                BotSay($"💡 {question.Explanation}");
            }
            else
            {
                BotSay($"❌ Incorrect. The correct answer was: {question.Options[question.CorrectAnswer]}");
                BotSay($"💡 {question.Explanation}");
            }

            quizQuestionIndex++;
            AddLog($"Quiz question answered - Score: {quizScore}/{quizQuestionIndex}");

            Task.Delay(2000).ContinueWith(_ => Dispatcher.Invoke(AskQuizQuestion));
        }

        private void CompleteQuizLevel()
        {
            int totalQuestions = quizLevels[quizLevel].Count;
            double percentage = (double)quizScore / totalQuestions * 100;

            BotSay($"🎯 Level {quizLevel} Complete!");
            BotSay($"📊 Your Score: {quizScore}/{totalQuestions} ({percentage:F0}%)");

            if (percentage >= 80)
                BotSay("🌟 Excellent! You're a cybersecurity champion!");
            else if (percentage >= 60)
                BotSay("👍 Good job! Keep learning to stay secure!");
            else
                BotSay("📚 Keep studying! Cybersecurity knowledge is crucial!");

            if (quizLevels.ContainsKey(quizLevel + 1) && percentage >= 60)
            {
                BotSay($"🚀 Ready for Level {quizLevel + 1}? Type 'yes' to continue or 'no' to finish.");
                // Don't increment level yet, wait for user response
            }
            else
            {
                inQuiz = false;
                BotSay("🏁 Quiz finished! Thanks for testing your cybersecurity knowledge!");
                AddLog($"Completed quiz - Final score: {quizScore}");
            }
        }
        #endregion

        #region Game System
        private void StartGame()
        {
            gameLevel = 1;
            gameScore = 0;
            inGame = true;

            BotSay("🎮 Welcome to the Cybersecurity Challenge Game!");
            BotSay("I'll give you riddles about cybersecurity concepts. Try to guess the answer!");
            AddLog("Started cybersecurity game");

            Task.Delay(1000).ContinueWith(_ => Dispatcher.Invoke(NextGameChallenge));
        }

        private void NextGameChallenge()
        {
            if (gameScore >= gameLevels[gameLevel].Count)
            {
                CompleteGameLevel();
                return;
            }

            var random = new Random();
            var challenge = gameLevels[gameLevel][random.Next(gameLevels[gameLevel].Count)];
            currentGameChallenge = challenge.Answer.ToLower();

            BotSay($"🔍 Challenge {gameScore + 1}: {challenge.Challenge}");
            BotSay("💭 Type your answer...");
        }

        private void HandleGame(string input)
        {
            string userAnswer = input.ToLower().Trim();

            if (userAnswer.Contains(currentGameChallenge) ||
                LevenshteinDistance(userAnswer, currentGameChallenge) <= 2)
            {
                gameScore++;
                BotSay("🎯 Correct! Great job!");

                // Provide educational context
                string context = GetGameAnswerContext(currentGameChallenge);
                if (!string.IsNullOrEmpty(context))
                    BotSay($"💡 {context}");
            }
            else
            {
                BotSay($"❌ Not quite right. The answer was: {currentGameChallenge}");
                string context = GetGameAnswerContext(currentGameChallenge);
                if (!string.IsNullOrEmpty(context))
                    BotSay($"💡 {context}");
            }

            AddLog($"Game challenge completed - Score: {gameScore}");
            Task.Delay(2000).ContinueWith(_ => Dispatcher.Invoke(NextGameChallenge));
        }

        private string GetGameAnswerContext(string answer)
        {
            var contexts = new Dictionary<string, string>
            {
                ["vpn"] = "VPNs create encrypted tunnels for secure internet browsing.",
                ["2fa"] = "Two-factor authentication significantly reduces account breaches.",
                ["phishing"] = "Always verify sender identity before clicking links or attachments.",
                ["antivirus"] = "Keep antivirus software updated for maximum protection.",
                ["https"] = "Look for HTTPS and the lock icon when entering sensitive information.",
                ["ransomware"] = "Regular backups are your best defense against ransomware.",
                ["firewall"] = "Firewalls act as barriers between trusted and untrusted networks.",
                ["social engineering"] = "Attackers often exploit human psychology rather than technical flaws.",
                ["backup"] = "Follow the 3-2-1 backup rule: 3 copies, 2 different media, 1 offsite.",
                ["password"] = "Use unique, complex passwords for each account."
            };

            return contexts.ContainsKey(answer) ? contexts[answer] : "";
        }

        private void CompleteGameLevel()
        {
            BotSay($"🏆 Level {gameLevel} Complete!");
            BotSay($"⭐ You solved {gameScore} challenges correctly!");

            if (gameLevels.ContainsKey(gameLevel + 1))
            {
                gameLevel++;
                gameScore = 0;
                BotSay($"🚀 Ready for Level {gameLevel}? The challenges get trickier!");
                Task.Delay(2000).ContinueWith(_ => Dispatcher.Invoke(NextGameChallenge));
            }
            else
            {
                inGame = false;
                BotSay("🎉 Congratulations! You've completed all game levels!");
                BotSay("🛡️ You're now a certified cybersecurity champion!");
                AddLog("Completed all game levels");
            }
        }
        #endregion

        #region Helper Methods
        private void CheckReminders(object sender, ElapsedEventArgs e)
        {
            var dueReminders = reminders.Where(r => DateTime.Now >= r.Value).ToList();

            foreach (var reminder in dueReminders)
            {
                Dispatcher.Invoke(() =>
                {
                    BotSay($"⏰ Reminder: {reminder.Key}");
                    BotSay("Don't forget to stay cyber-safe! 🛡️");
                });

                reminders.Remove(reminder.Key);
                AddLog($"Reminder triggered: {reminder.Key}");
            }
        }

        private void ShowActivityLog()
        {
            if (!activityLog.Any())
            {
                BotSay("📊 No recent activity to show.");
                return;
            }

            BotSay("📊 Recent Activity Log:");
            foreach (var entry in activityLog.Take(8))
            {
                BotSay($"• {entry}");
            }

            if (activityLog.Count > 8)
                BotSay($"... and {activityLog.Count - 8} more activities");
        }

        private void ShowHelp()
        {
            BotSay("🆘 Here's what I can help you with:");
            BotSay("🧠 **Quiz** - Test your cybersecurity knowledge");
            BotSay("🎮 **Game** - Play cybersecurity riddle challenges");
            BotSay("📋 **Tasks** - Manage your security to-do list");
            BotSay("⏰ **Reminders** - Set alerts for security tasks");
            BotSay("📊 **Log** - View your recent activity");
            BotSay("💬 **Chat** - Ask about passwords, phishing, VPNs, etc.");
            BotSay("");
            BotSay("💡 Try saying: 'Start quiz', 'Add task to enable 2FA', or 'Remind me to update passwords in 2 days'");
        }

        private void AddLog(string entry)
        {
            activityLog.Insert(0, $"{DateTime.Now:HH:mm} - {entry}");
            if (activityLog.Count > 50) // Keep log manageable
                activityLog.RemoveAt(activityLog.Count - 1);
        }

        private int LevenshteinDistance(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1)) return string.IsNullOrEmpty(s2) ? 0 : s2.Length;
            if (string.IsNullOrEmpty(s2)) return s1.Length;

            int[,] d = new int[s1.Length + 1, s2.Length + 1];

            for (int i = 0; i <= s1.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= s2.Length; j++) d[0, j] = j;

            for (int i = 1; i <= s1.Length; i++)
            {
                for (int j = 1; j <= s2.Length; j++)
                {
                    int cost = s1[i - 1] == s2[j - 1] ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[s1.Length, s2.Length];
        }
        #endregion

        #region UI Methods
        private void ShowTypingIndicator()
        {
            var typingBorder = new Border
            {
                Style = (Style)FindResource("BotMessage"),
                Name = "TypingIndicator"
            };

            var typingText = new TextBlock
            {
                Text = "🤖 CyberBot is typing...",
                Foreground = Brushes.Gray,
                FontStyle = FontStyles.Italic,
                FontSize = 12
            };

            typingBorder.Child = typingText;
            ChatPanel.Children.Add(typingBorder);
            ScrollToBottom();
        }

        private void RemoveTypingIndicator()
        {
            var typingIndicator = ChatPanel.Children.OfType<Border>()
                .FirstOrDefault(b => b.Name == "TypingIndicator");
            if (typingIndicator != null)
                ChatPanel.Children.Remove(typingIndicator);
        }

        private void UserInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddUserMessage(string message)
        {
            var border = new Border { Style = (Style)FindResource("UserMessage") };
            var textBlock = new TextBlock
            {
                Text = message,
                Foreground = Brushes.White,
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap
            };

            border.Child = textBlock;
            ChatPanel.Children.Add(border);
            ScrollToBottom();
        }

        private void BotSay(string message)
        {
            var border = new Border { Style = (Style)FindResource("BotMessage") };
            var textBlock = new TextBlock
            {
                Text = message,
                Foreground = Brushes.Black,
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap
            };

            border.Child = textBlock;
            ChatPanel.Children.Add(border);
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            ChatScrollViewer.ScrollToEnd();
        }
        #endregion
    }

    #region Data Classes
    public class CyberTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class QuizQuestion
    {
        public string Question { get; set; }
        public string[] Options { get; set; }
        public int CorrectAnswer { get; set; }
        public string Explanation { get; set; }

        public QuizQuestion(string question, string[] options, int correctAnswer, string explanation)
        {
            Question = question;
            Options = options;
            CorrectAnswer = correctAnswer;
            Explanation = explanation;
        }
    }

    public class GameChallenge
    {
        public string Challenge { get; set; }
        public string Answer { get; set; }

        public GameChallenge(string challenge, string answer)
        {
            Challenge = challenge;
            Answer = answer;
        }
    }
    #endregion
}