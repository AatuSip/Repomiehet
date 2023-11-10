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

            // Initialize game
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

            CardSuits.Add(1, "Hearts");
            CardSuits.Add(2, "Diamonds");
            CardSuits.Add(3, "Clubs");
            CardSuits.Add(4, "Spades");


        }

        public void OnPostHit()
        {
            // Handle hit action


        }

        public void OnPostStand()
        {
            // Handle stand action
        }

        // Add other methods and properties as needed
    }
}