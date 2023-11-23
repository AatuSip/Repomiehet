using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text;


namespace FRONTBJ.Pages
{
    public class PlayBlackjackModel : PageModel
    {
        public Dictionary<string, int> CardValues { get; set; } = new Dictionary<string, int>();
        public Dictionary<int, string> CardSuits { get; set; } = new Dictionary<int, string>();
        public List<int> Cards { get; set; } = new List<int>();
        public List<int> PlayedCards { get; set; } = new List<int>();
        public List<string> PlayerHand { get; set; } = new List<string>();
        public List<string> DealerHand { get; set; } = new List<string>();

        public int PlayerScore { get; set; } = 0;
        public bool PlayerBust { get; set; } = false;
        public int DealerScore { get; set; } = 0;
        public bool DealerBust { get; set; } = false;
        private const int MaxCardValue = 13;
        private const int MaxScore = 21;
        public string GameResult { get; set; }
        private static Random rnd = new Random();

        public void OnGet()
        {
            InitializeGame();
            GameStart();
           // Blackjack();
            GameResult = DetermineWinner();
        }

        public void InitializeGame()
        {
            //initialize game

            CardValues.Add("A", 1);
            CardValues.Add("2", 2);
            CardValues.Add("3", 3);
            CardValues.Add("4", 4);
            CardValues.Add("5", 5);
            CardValues.Add("6", 6);
            CardValues.Add("7", 7);
            CardValues.Add("8", 8);
            CardValues.Add("9", 9);
            CardValues.Add("10", 10);
            CardValues.Add("J", 10);
            CardValues.Add("Q", 10);
            CardValues.Add("K", 10);

            CardSuits.Add(1, "♥");
            CardSuits.Add(2, "♦");
            CardSuits.Add(3, "♣");
            CardSuits.Add(4, "♠");

            for (int i = 1; i <= 13; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    Cards.Add(i);
                }
            }

        }

        public void GameStart()
        {
            DealerScore = 0;
            PlayerScore = 0;

            for (int i = 0; i < 3; i++)
            {
                IActionResult result = DrawCard();
                if (result is ContentResult contentResult)
                {
                    if (PlayerHand.Count < 2)
                    {
                        PlayerHand.Add(contentResult.Content);
                    }
                    else
                    {
                        DealerHand.Add(contentResult.Content);
                    }
                }
            }

        }

        public void Blackjack()
        {
            while (!(PlayerBust || PlayerScore >= MaxScore || DealerScore >= MaxScore || (PlayerScore > DealerScore && DealerScore >= 17)))
            {
                PlayerTurn();
                DealerTurn();
            }

            DetermineWinner();
            ResetGame();
        }

        private void PlayerTurn()
        {
            if (PlayerScore < MaxScore)
            {
                IActionResult hitResult = OnPostHit();
                if (hitResult is ContentResult contentResult)
                {
                    PlayerHand.Add(contentResult.Content);
                    CalculatePlayerScore();
                }
            }
        }

        private void DealerTurn()
        {
            while (DealerScore < 17 && !DealerBust)
            {
                IActionResult hitResult = OnPostHit();
                if (hitResult is ContentResult contentResult)
                {
                    DealerHand.Add(contentResult.Content);
                    CalculateDealerScore();
                }
            }
        }

        private string DetermineWinner()
        {
            string result = "";

            if (PlayerBust)
            {
                result = "You busted! Dealer wins.";
            }
            else if (DealerBust)
            {
                result = "The dealer busted! You win.";
            }
            else if (PlayerScore > DealerScore)
            {
                result = "You won!";
            }
            else if (PlayerScore == DealerScore)
            {
                result = "It's a tie!";
            }
            else
            {
                result = "You lost!";
            }

            return result;
        }


        private void ResetGame()
        {
            PlayerHand.Clear();
            DealerHand.Clear();
            PlayerBust = false;
            DealerBust = false;
            PlayerScore = 0;
            DealerScore = 0;
            PlayedCards.Clear();
        }

        private IActionResult DrawCard()
        {
            if (Cards.Count == 0)
            {
                return Content("No more cards in the deck.");
            }

            int cardIndex = rnd.Next(Cards.Count);
            int card = Cards[cardIndex];
            Cards.RemoveAt(cardIndex);
            PlayedCards.Add(card);

            string cardValue = CardValues.Keys.ElementAt(card - 1);
            string cardSuit = CardSuits[rnd.Next(1, 5)];

            return Content($"{cardValue} of {cardSuit}");
        }

        private IActionResult DrawCardForDealer()
        {
            if (Cards.Count == 0)
            {
                return Content("No more cards in the deck.");
            }

            int cardIndex = rnd.Next(Cards.Count);
            int card = Cards[cardIndex];
            Cards.RemoveAt(cardIndex);
            PlayedCards.Add(card);

            string cardValue = CardValues.Keys.ElementAt(card - 1);
            string cardSuit = CardSuits[rnd.Next(1, 5)];

            return Content($"{cardValue} of {cardSuit}");
        }


        public IActionResult OnPostHit()
        {
            if (PlayerBust || PlayerScore >= MaxScore)
            {
                // Player is already bust or has reached maximum score
                // Handle this case as needed, for instance, display a message
                return Content("Cannot hit. Game over for player.");
            }

            if (PlayerHand.Count < 5) // Assuming a maximum of 5 cards per hand
            {
                IActionResult hitResult = DrawCard(); // Custom method to draw a card
                if (hitResult is ContentResult contentResult)
                {
                    PlayerHand.Add(contentResult.Content);
                    CalculatePlayerScore();
                }
                return Content("Hit successful"); // Optional: Message indicating successful hit
            }
            else
            {
                // Player already has the maximum number of cards in hand
                // Handle this case, for instance, display a message
                return Content("Maximum cards reached. Cannot hit.");
            }
        }


        public IActionResult OnPostStand()
        {
            if (PlayerBust || PlayerScore >= MaxScore)
            {
                // Player is already bust or has reached maximum score
                // Handle this case as needed, for instance, display a message
                return Content("Player cannot stand. Game over for player.");
            }

            while (!(DealerScore > 17) && !DealerBust)
            {
                IActionResult hitResult = DrawCardForDealer(); // Custom method to draw a card for the dealer
                if (hitResult is ContentResult contentResult)
                {
                    DealerHand.Add(contentResult.Content);
                    CalculateDealerScore();
                }
            }

            DetermineWinner(); // Method to determine the winner

            // Reset the game state, clear hands, scores, etc.
            ResetGame();

            return RedirectToPage(); // Redirect to the game page or another page as needed
        }

        private void CalculateDealerScore()
        {
            DealerScore = 0;

            foreach (string item in DealerHand)
            {
                string[] cardSplit = item.Split(' ');
                if (cardSplit.Length >= 2)
                {
                    string cardNumber = cardSplit[1];
                    if (CardValues.ContainsKey(cardNumber))
                    {
                        DealerScore += CardValues[cardNumber];
                    }
                }
            }

            // Additional logic for handling Ace as 1 or 11 based on the score
            if (DealerHand.Any(card => card.Contains("A")) && DealerScore + 10 <= MaxScore)
            {
                DealerScore += 10;
            }
        }

        private void CalculatePlayerScore()
        {
            PlayerScore = 0;

            foreach (string item in PlayerHand)
            {
                string[] cardSplit = item.Split(' ');
                if (cardSplit.Length >= 2)
                {
                    string cardNumber = cardSplit[1];
                    if (CardValues.ContainsKey(cardNumber))
                    {
                        PlayerScore += CardValues[cardNumber];
                    }
                }
            }

            // Additional logic for handling Ace as 1 or 11 based on the score
            if (PlayerHand.Any(card => card.Contains("A")) && PlayerScore + 10 <= MaxScore)
            {
                PlayerScore += 10;
            }
        }

        public IActionResult OnPost(string action)
        {
            if (action == "Hit")
            {

                IActionResult hitResult = OnPostHit();
                if (hitResult is ContentResult contentResult)
                {
                    PlayerHand.Add(contentResult.Content);
                }
            }
            else if (action == "Stand")
            {

                IActionResult standResult = OnPostStand();

            }


            return RedirectToPage();
        }
    }
}