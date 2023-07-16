using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Welcome to the Random Sentence Game!");
        Console.WriteLine("Guess the sentence and earn points based on your accuracy.");
        Console.ResetColor();
        Console.WriteLine();

        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wordlist.txt");
        string scoreFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "score.txt");

        int totalScore = LoadScore(scoreFilePath);

        while (true)
        {
            List<string> words = GetRandomWords(filePath, 5);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Generated words:");
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (string word in words)
            {
                Console.Write(word + " ");
            }
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Enter your guess (separated by spaces):");
            Console.ForegroundColor = ConsoleColor.Black;
            string userGuess = Console.ReadLine();
            Console.ResetColor();

            int correctWords = CountCorrectWords(words, userGuess);
            int incorrectWords = CountIncorrectWords(words, userGuess);
            int roundScore = correctWords - incorrectWords;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("----- Results -----");
            Console.WriteLine($"Correct words: {correctWords}");
            Console.WriteLine($"Incorrect words: {incorrectWords}");
            Console.WriteLine($"Round Score: {roundScore}");
            Console.ResetColor();

            totalScore += roundScore;
            SaveScore(totalScore, scoreFilePath);

            Console.WriteLine($"Total Score: {totalScore}");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Press Enter to continue or type 'exit' to quit the game.");
            Console.ResetColor();
            string continueChoice = Console.ReadLine();

            if (continueChoice.Trim().ToLower() == "exit")
                break;

            Console.Clear();
        }

        Console.WriteLine("Thank you for playing the Random Sentence Game!");
    }

    static List<string> GetRandomWords(string filePath, int count)
    {
        List<string> words = new List<string>();
        Random random = new Random();

        // Read all lines from the text file
        string[] lines = File.ReadAllLines(filePath);

        // Shuffle the lines randomly
        for (int i = lines.Length - 1; i >= 0; i--)
        {
            int j = random.Next(i + 1);
            string temp = lines[i];
            lines[i] = lines[j];
            lines[j] = temp;
        }

        // Select the first 'count' lines as random words
        for (int i = 0; i < count; i++)
        {
            words.Add(lines[i]);
        }

        return words;
    }

    static int CountCorrectWords(List<string> words, string guess)
    {
        string[] wordArray = words.ToArray();
        string[] guessWords = guess.Split(' ');

        int correctCount = 0;

        for (int i = 0; i < Math.Min(wordArray.Length, guessWords.Length); i++)
        {
            if (wordArray[i] == guessWords[i])
                correctCount++;
        }

        return correctCount;
    }

    static int CountIncorrectWords(List<string> words, string guess)
    {
        string[] wordArray = words.ToArray();
        string[] guessWords = guess.Split(' ');

        int incorrectCount = 0;

        for (int i = 0; i < Math.Min(wordArray.Length, guessWords.Length); i++)
        {
            if (wordArray[i] != guessWords[i])
                incorrectCount++;
        }

        return incorrectCount + Math.Abs(wordArray.Length - guessWords.Length);
    }

    static int LoadScore(string scoreFilePath)
    {
        if (File.Exists(scoreFilePath))
        {
            string scoreText = File.ReadAllText(scoreFilePath);
            int score;
            if (int.TryParse(scoreText, out score))
                return score;
        }

        return 0;
    }

    static void SaveScore(int score, string scoreFilePath)
    {
        File.WriteAllText(scoreFilePath, score.ToString());
    }
}
