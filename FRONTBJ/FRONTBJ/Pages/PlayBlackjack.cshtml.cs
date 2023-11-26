using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;


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

        public PlayBlackjackModel()
        {

        }
        
        public void OnGet()
        {
            GetTempData();

            if (CardValues.Count == 0 || CardSuits.Count == 0 || Cards.Count == 0)
            {
                ResetGame();
                InitializeGame();
                SetTempData();
            }

            if (PlayerHand.Count == 0 && DealerHand.Count == 0)
            {
                GameStart();
            }

            SetTempData();
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

            CalculatePlayerScore();
            CalculateDealerScore();

        }

        private void GetTempData()
        {
            if (TempData.ContainsKey("PlayerHand"))
            {
                string serializedPlayerHand = TempData["PlayerHand"] as string;
                if (serializedPlayerHand != null)
                {
                    PlayerHand = JsonConvert.DeserializeObject<List<string>>(serializedPlayerHand);
                }
            }
            if (TempData.ContainsKey("DealerHand"))
            {
                string serializedDealerHand = TempData["DealerHand"] as string;
                if (serializedDealerHand != null)
                {
                    DealerHand = JsonConvert.DeserializeObject<List<string>>(serializedDealerHand);
                }
            }
            if (TempData.ContainsKey("PlayerScore"))
            {
                PlayerScore = Convert.ToInt32(TempData["PlayerScore"]);
            }
            if (TempData.ContainsKey("DealerScore"))
            {
                DealerScore = Convert.ToInt32(TempData["DealerScore"]);
            }
            if (TempData.ContainsKey("CardValues"))
            {
                string serializedCardValues = TempData["CardValues"] as string;
                if (serializedCardValues != null)
                {
                    CardValues = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializedCardValues);
                }
            }
            if (TempData.ContainsKey("CardSuits"))
            {
                string serializedCardSuits = TempData["CardSuits"] as string;
                if (serializedCardSuits != null)
                {
                    CardSuits = JsonConvert.DeserializeObject<Dictionary<int, string>>(serializedCardSuits);
                }
            }
            if (TempData.ContainsKey("Cards"))
            {
                string serializedCards = TempData["Cards"] as string;
                if (serializedCards != null)
                {
                    Cards = JsonConvert.DeserializeObject<List<int>>(serializedCards);
                }
            }
            if (TempData.ContainsKey("PlayedCards"))
            {
                string serializedPlayedCards = TempData["PlayedCards"] as string;
                if (serializedPlayedCards != null)
                {
                    PlayedCards = JsonConvert.DeserializeObject<List<int>>(serializedPlayedCards);
                }
            }
            if (TempData.ContainsKey("GameResult"))
            {
                GameResult = TempData["GameResult"] as string;
            }
        }
        private void SetTempData()
        {
            TempData["PlayerHand"] = JsonConvert.SerializeObject(PlayerHand);
            TempData["DealerHand"] = JsonConvert.SerializeObject(DealerHand);
            TempData["PlayerScore"] = PlayerScore;
            TempData["DealerScore"] = DealerScore;
            TempData["CardValues"] = JsonConvert.SerializeObject(CardValues);
            TempData["CardSuits"] = JsonConvert.SerializeObject(CardSuits);
            TempData["Cards"] = JsonConvert.SerializeObject(Cards);
            TempData["PlayedCards"] = JsonConvert.SerializeObject(PlayedCards);
            TempData["GameResult"] = GameResult;
        }

        private void PlayerTurn()
        {

            if (PlayerScore < MaxScore)
            {
                OnPostHit();

            }
        }

        private void DealerTurn()
        {
            while (DealerScore < 17 && !DealerBust)
            {
                OnPostStand();
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
            if (TempData != null)
            {
                TempData.Clear();
                ClearProperties();
            }
        }

        private void ClearProperties()
        {
            PlayerHand.Clear();
            DealerHand.Clear();
            Cards.Clear();
            PlayedCards.Clear();
            CardValues.Clear();
            CardSuits.Clear();
            GameResult = "";
            PlayerScore = 0;
            DealerScore = 0;
            PlayerBust = false;
            DealerBust = false;
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

            if (PlayerHand.Count < 5) // Assuming a maximum of 5 cards per hand
            {
                IActionResult hitResult = DrawCard(); // Custom method to draw a card
                if (hitResult is ContentResult contentResult)
                {
                    PlayerHand.Add(contentResult.Content);
                    CalculatePlayerScore();
                }
            }

            if (PlayerScore > MaxScore)
            {
                // Player is already bust or has reached maximum score
                // Handle this case as needed, for instance, display a message
                PlayerBust = true;
                GameResult = DetermineWinner();
            }

            SetTempData();

            return RedirectToPage();

        }


        public IActionResult OnPostStand()
        {

            while (!(DealerScore > 17) && !DealerBust)
            {
                IActionResult hitResult = DrawCardForDealer(); // Custom method to draw a card for the dealer
                if (hitResult is ContentResult contentResult)
                {
                    DealerHand.Add(contentResult.Content);
                    CalculateDealerScore();
                }
            }

            if (DealerScore > MaxScore)
            {
                // Player is already bust or has reached maximum score
                // Handle this case as needed, for instance, display a message
                DealerBust = true;
            }


            GameResult = DetermineWinner(); // Method to determine the winner
            SetTempData();
            // Reset the game state, clear hands, scores, etc.

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
                    string cardNumber = cardSplit[0];
                    if (CardValues.ContainsKey(cardNumber))
                    {
                        DealerScore += CardValues[cardNumber];
                    }
                }
            }

            if (DealerHand.Any(card => card.Contains("A")) && DealerScore + 11 <= MaxScore)
            {
                DealerScore += 11;
            }

            SetTempData();


        }

        private void CalculatePlayerScore()
        {
            PlayerScore = 0;

            foreach (string item in PlayerHand)
            {
                string[] cardSplit = item.Split(' ');
                if (cardSplit.Length >= 2)
                {
                    string cardNumber = cardSplit[0];
                    if (CardValues.ContainsKey(cardNumber))
                    {
                        PlayerScore += CardValues[cardNumber];
                    }
                }
            }

            if (PlayerHand.Any(card => card.Contains("A")) && PlayerScore + 11 <= MaxScore)
            {
                PlayerScore += 11;
            }

            SetTempData();


        }

        public IActionResult OnPost(string action)
        {

            GetTempData();
            if (action == "Hit")
            {
                PlayerTurn();

            }
            else if (action == "Stand")
            {
                DealerTurn();
            }
            else if (action == "Reset")
            {
                ResetGame();

            }


            SetTempData();

            return RedirectToPage();
        }
    }
}