using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Reflection.Emit;
using System.Data;
using System;
using System.Collections.Generic;


namespace FRONT_BJ.Pages
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
        public int Money { get; set; } = 1000;
        public int Bet { get; set; } = 0;

        public void OnGet()
        {
            InitializeGame();
            GameStart();
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
            string card;
            DealerScore = 0;
            PlayerScore = 0;

            BetPlaced = false;

            while (!BetPlaced)
            {
                if (Bet > Money)
                {
                    continue;
                }
                else if (Bet <= 0)
                {
                    continue;
                }
                else
                {
                    Money = Money - Bet;
                    BetPlaced = true;
                }
            }

            card = Hit();
            PlayerHand.Add(card);
            card = Hit();
            PlayerHand.Add(card);

            card = Hit();
            DealerHand.Add(card);
        }

        public void Blackjack()
        {
            while (true)
            {
                string cardNumber;
                string cardSuit;

                PlayerScore = 0;
                DealerScore = 0;

                string playerCards = "";
                foreach (string item in PlayerHand)
                {
                    string[] cardSplit = item.Split(' ');
                    cardNumber = cardSplit[1];
                    cardSuit = cardSplit[0];
                    playerCards = playerCards + cardNumber + " of " + cardSuit + ", ";
                    PlayerScore = PlayerScore + CardValues[cardNumber];
                }
                playerCards = playerCards.Remove(playerCards.Length - 2);

                string dealerCards = "";
                foreach (string item in DealerHand)
                {
                    string[] cardSplit = item.Split(' ');
                    cardNumber = cardSplit[1];
                    cardSuit = cardSplit[0];
                    dealerCards = dealerCards + cardNumber + " of " + cardSuit + ", ";
                }
                dealerCards = dealerCards.Remove(dealerCards.Length - 2);

                CheckBust();
                if (PlayerBust == true)
                {
                    foreach (string item in PlayerHand)
                    {
                        string[] cardSplit = item.Split(' ');
                        cardNumber = cardSplit[1];
                        cardSuit = cardSplit[0];
                        playerCards = playerCards + cardNumber + " of " + cardSuit + ", ";
                        PlayerScore = PlayerScore + CardValues[cardNumber];
                    }
                    playerCards = playerCards.Remove(playerCards.Length - 2);
                    break;
                }
                else
                {
                    string input = ""; // Replace this with your own input method

                    if (input == "1")
                    {
                        string card = OnPostHit();
                        PlayerHand.Add(card);
                    }
                    else if (input == "2")
                    {
                        OnPostStand();
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            PlayerHand.Clear();
            DealerHand.Clear();
            PlayerBust = false;
            DealerBust = false;
            PlayerScore = 0;
            DealerScore = 0;
            PlayedCards.Clear();
        }

        public string OnPostHit()
        {
            Random rnd = new Random();
            int card;
            int suit = 1;

            do
            {
                card = rnd.Next(Cards.Count);

                while (card > 13)
                {
                    card -= 13;
                    suit++;
                }

            } while (PlayedCards.Contains(card));

            PlayedCards.Add(card);

            if (card == 0)
            {
                card = 13;
            }

            string returnableCard = CardSuits[suit] + card.ToString();

            return returnableCard;
        }


        public string OnPostStand()
        {
            string card;
            string cardNumber;
            string cardSuit;
            string playerCards = "";
            string dealerCards = "";

            while (!(DealerScore > 17 && !DealerBust) == true)
            {
                DealerScore = 0;
                PlayerScore = 0;

                card = OnPostHit();
                DealerHand.Add(card);
                foreach (string item in DealerHand)
                {
                    string[] cardSplit = item.Split(' ');
                    cardNumber = cardSplit[1];
                    cardSuit = cardSplit[0];
                    DealerScore = DealerScore + CardValues[cardNumber];
                    dealerCards = dealerCards + cardNumber + " of " + cardSuit + ", ";
                }

                foreach (string item in PlayerHand)
                {
                    string[] cardSplit = item.Split(' ');
                    cardNumber = cardSplit[1];
                    cardSuit = cardSplit[0];
                    PlayerScore = PlayerScore + CardValues[cardNumber];
                    playerCards = playerCards + cardNumber + " of " + cardSuit + ", ";
                }
                playerCards = playerCards.Remove(playerCards.Length - 2);

                dealerCards = dealerCards.Remove(dealerCards.Length - 2);

                CheckBust();
                if (DealerScore > 17 || DealerBust == true)
                {
                    break;
                }
            }

            string result = "";
            if (DealerBust == true)
            {
                result = "The dealer busted!";
                Money = Money + Bet * 2;
            }
            else if (PlayerScore > DealerScore)
            {
                result = "You won!";
                Money = Money + Bet * 2;
            }
            else if (PlayerScore == DealerScore)
            {
                result = "It's a tie!";
                Money = Money + Bet;
            }
            else
            {
                result = "You lost!";
            }

            PlayerHand.Clear();
            DealerHand.Clear();
            PlayerBust = false;
            DealerBust = false;
            PlayerScore = 0;
            DealerScore = 0;
            PlayedCards.Clear();

        }


        public void CheckBust()
        {
            if (this.PlayerScore > 21) // Check if the player has busted
            {
                this.PlayerBust = true; // Set player bust to true
            }

            if (this.DealerScore > 21) // Check if the dealer has busted
            {
                this.DealerBust = true; // Set dealer bust to true
            }
        }
    }
}