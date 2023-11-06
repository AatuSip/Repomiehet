using System.Net;
using System.Runtime.Serialization;
using System.Reflection.Emit;
using System.Data;
using System;
using System.Collections.Generic;

class Program
{    
    public static Dictionary<string, int> cardValues = new Dictionary<string, int>(); // Create dictionary for card values
    public static Dictionary<int, string> cardSuits = new Dictionary<int, string>(); // Create dictionary for card suits
    public static List<int> cards = new List<int>(); // Create list for cards
    public static List<int> playedCards = new List<int>(); // Create array for played cards
    public static List<string> playerHand = new List<string>(); // Create dictionary for player hand
    public static List<string> dealerHand = new List<string>(); // Create dictionary for dealer hand 

    public static int playerScore = 0; // Create int for player score
    public static bool playerBust = false; // Create int for player bust
    public static int dealerScore = 0; // Create int for dealer score
    public static bool dealerBust = false; // Create int for dealer bust
    public static int money = 1000; // Create int for money
    public static int bet = 0; // Create int for bet

    static void Main(string[] args)
    {
        cardValues.Add("1", 1); // Add card values to the dictionary
        cardValues.Add("2", 2);
        cardValues.Add("3", 3);
        cardValues.Add("4", 4);
        cardValues.Add("5", 5);
        cardValues.Add("6", 6);
        cardValues.Add("7", 7);
        cardValues.Add("8", 8);
        cardValues.Add("9", 9);
        cardValues.Add("10", 10);
        cardValues.Add("11", 10);
        cardValues.Add("12", 10);
        cardValues.Add("13", 10);

        cardSuits.Add(1, "Hearts"); // Add card suits to the dictionary
        cardSuits.Add(2, "Diamonds");
        cardSuits.Add(3, "Spades");
        cardSuits.Add(4, "Clubs");

        for (int i = 1; i < 53; i++) // Add cards to the list
        {
            cards.Add(i);
        }

        gameStart(); // Start the game
    }

    static void gameStart()
    {
        while (true)
        {
            Console.Clear();
            string card; // Create string for the dealt card
            dealerScore = 0;
            playerScore = 0;
    
            Console.WriteLine("Lets play Blackjack!");
            Console.WriteLine("Press any key to start the game");
            Console.ReadKey();

            bool betPlaced = false; // Create bool for bet placed



            while (betPlaced == false)
            {
                Console.Clear();
                Console.WriteLine("You have " + money + " dollars");
                Console.WriteLine("How much do you want to bet?");
                
                while (!int.TryParse(Console.ReadLine(), out bet)) // Check if the bet is an integer
                {
                    Console.WriteLine("Please enter a valid number");
                }

                if (bet > money) // Check if the bet is higher than the money
                {
                    Console.WriteLine("You don't have enough money!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    continue;
                }
                else if (bet <= 0) // Check if the bet is less than or equal to 0
                {
                    Console.WriteLine("You can't bet 0 or less!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    continue;
                }
                else
                {
                    money = money - bet; // Subtract the bet from the money
                    betPlaced = true; // Set bet placed to true
                }
            }
    
            // Deal cards to the player and the dealer
            card = Hit(); // Deal a card
            playerHand.Add(card); // Add the dealt card to the player hand
            card = Hit();
            playerHand.Add(card);
    
            card = Hit();
            dealerHand.Add(card);
            Blackjack();
        }
    }

    public static void Blackjack()
    {
        while (true)
        {
            Console.Clear();
            string cardNumber; // Create string for the card number
            string cardSuit; // Create string for the card suit

            playerScore = 0; // Set player score to 0
            dealerScore = 0; // Set dealer score to 0
        
            string playerCards = ""; // Create string for the players cards
            foreach (string item in playerHand)
            {
                string[] cardSplit = item.Split(' '); // Split the dealt card to card number and suit
                cardNumber = cardSplit[1]; // Set the card number
                cardSuit = cardSplit[0]; // Set the card suit
                playerCards = playerCards + cardNumber + " of " + cardSuit + ", "; // Print the dealt card
                playerScore = playerScore + cardValues[cardNumber]; // Add the value of the first card to the player score
            }
            playerCards = playerCards.Remove(playerCards.Length - 2); // Remove the last comma from the string
            Console.WriteLine("Your hand: ");
            Console.WriteLine(playerCards); // Print the players cards
            playerCards = ""; // Reset the string for the players cards

            string dealerCards = ""; // Create string for the dealers cards
            
            foreach (string item in dealerHand)
            {
                string[] cardSplit = item.Split(' '); // Split the dealt card to card number and suit
                cardNumber = cardSplit[1]; // Set the card number
                cardSuit = cardSplit[0]; // Set the card suit
                dealerCards = dealerCards+cardNumber+" of "+cardSuit+", "; // Print the dealt card
            }
            dealerCards = dealerCards.Remove(dealerCards.Length - 2); // Remove the last comma from the string
            Console.WriteLine("\nDealers hand: ");
            Console.WriteLine(dealerCards); // Print the dealers cards

            CheckBust(); // Check if the player or the dealer has busted
            if (playerBust == true)
            {
                Console.Clear();
                foreach (string item in playerHand)
                {
                    string[] cardSplit = item.Split(' '); // Split the dealt card to card number and suit
                    cardNumber = cardSplit[1]; // Set the card number
                    cardSuit = cardSplit[0]; // Set the card suit
                    playerCards = playerCards + cardNumber + " of " + cardSuit + ", "; // Print the dealt card
                    playerScore = playerScore + cardValues[cardNumber]; // Add the value of the first card to the player score
                }
                playerCards = playerCards.Remove(playerCards.Length - 2); // Remove the last comma from the string
                Console.WriteLine("Your hand: ");
                Console.WriteLine(playerCards); // Print the players cards
                Console.WriteLine("\nYou busted!");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                break;
            }
            else
            {
                Console.WriteLine("\nWhat do you want to do?");
                Console.WriteLine("1. Hit");
                Console.WriteLine("2. Stand");
                string input = ""; // Create string for the user input
                input = Console.ReadLine(); // Read the user input

                if (input == "1")
                {
                    string card = Hit(); // Deal a card
                    playerHand.Add(card); // Add the dealt card to the player hand
                }
                else if (input == "2")
                {
                    Stand();
                    break;
                }
                else
                {
                    break;
                }
            }    
        }

        playerHand.Clear(); // Clear the player hand
        dealerHand.Clear(); // Clear the dealer hand
        playerBust = false; // Reset player bust
        dealerBust = false; // Reset dealer bust
        playerScore = 0; // Reset player score
        dealerScore = 0; // Reset dealer score
        playedCards.Clear(); // Clear the played cards array
    }

    public static void Stand()
    {
        string card; // Create string for the dealt card
        string cardNumber; // Create string for the card number
        string cardSuit; // Create string for the card suit
        string playerCards = ""; // Create string for the players cards
        string dealerCards = ""; // Create string for the dealers cards

        while (!(dealerScore > 17 && !dealerBust) == true) // Check if the dealer score is less than 17 or the players score
        {
            Console.Clear();
            dealerScore = 0; // Set dealer score to 0
            playerScore = 0; // Set player score to 0

            card = Hit(); // Deal a card
            dealerHand.Add(card); // Add the dealt card to the dealer hand
            foreach (string item in dealerHand)
            {
                string[] cardSplit = item.Split(' '); // Split the dealt card to card number and suit
                cardNumber = cardSplit[1]; // Set the card number
                cardSuit = cardSplit[0]; // Set the card suit
                dealerScore = dealerScore + cardValues[cardNumber]; // Add the value of the dealt card to the dealer score
                dealerCards = dealerCards + cardNumber + " of " + cardSuit + ", "; // Print the dealt card
            }

            foreach (string item in playerHand)
            {
                string[] cardSplit = item.Split(' '); // Split the dealt card to card number and suit
                cardNumber = cardSplit[1]; // Set the card number
                cardSuit = cardSplit[0]; // Set the card suit
                playerScore = playerScore + cardValues[cardNumber]; // Add the value of the first card to the player score
                playerCards = playerCards + cardNumber + " of " + cardSuit + ", "; // Print the dealt card
            }
            playerCards = playerCards.Remove(playerCards.Length - 2); // Remove the last comma from the string
            Console.WriteLine("Your hand: ");
            Console.WriteLine(playerCards); // Print the players cards
            playerCards = ""; // Reset the string for the players cards

            dealerCards = dealerCards.Remove(dealerCards.Length - 2); // Remove the last comma from the string
            Console.WriteLine("Dealers hand: ");
            Console.WriteLine(dealerCards); // Print the dealers cards
            dealerCards = ""; // Reset the string for the dealers cards,

            CheckBust();
            if (dealerScore > 17 || dealerBust == true)
            {
                break;
            }
            
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        if (dealerBust == true)
        {
            Console.WriteLine("The dealer busted!");
            money = money + bet*2;
        }
        else if (playerScore > dealerScore)
        {
            Console.WriteLine("You won!");
            money = money + bet*2;
        }
        else if (playerScore == dealerScore)
        {
            Console.WriteLine("It's a tie!");
            money = money + bet;
        }
        else
        {
            Console.WriteLine("You lost!");
        }
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
        
        playerHand.Clear(); // Clear the player hand
        dealerHand.Clear(); // Clear the dealer hand
        playerBust = false; // Reset player bust
        dealerBust = false; // Reset dealer bust
        playerScore = 0; // Reset player score
        dealerScore = 0; // Reset dealer score
        playedCards.Clear(); // Clear the played cards array
    }

    public static string Hit()
    {
        Random rnd = new Random(); // Create random number generator
        int card = rnd.Next(cards.Count); // Generate random number from the cards list
        int suit = 1; // Set suit to 1

        if (playedCards.Contains(card)) // Check if the card has already been played
        {
            Hit(); // If the card has already been played, generate a new card
        }
        else
        {
            playedCards.Add(card); // Add the card to the played cards array
        }
        
        while (card > 13) // Check the suit of the card
        {
            card = card - 13; // If the card is higher than 13, subtract 13 from the card
            suit++; // Change the suit
        }

        if (card == 0) // Check if the card is 0
        {
            card = 13; // If the card is 0, set the card to 13
        }

        string returnableCard = cardSuits[suit]+" "+card; // Create a string for the dealt card

        return returnableCard; // Return the dealt card
    }

    public static void CheckBust()
    {
        if (playerScore > 21) // Check if the player has busted
        {
            playerBust = true; // Set player bust to 1
        }
        
        if (dealerScore > 21) // Check if the dealer has busted
        {
            dealerBust = true; // Set dealer bust to 1
        }
    }
}
