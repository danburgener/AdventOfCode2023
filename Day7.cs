namespace AdventOfCode2023
{
    public static class Day7
    {
        public static async Task<long> One()
        {
            var data = await Common.ReadFile("Seven", "One");
            List<Hand> hands = new List<Hand>();
            foreach(var item in data)
            {
                hands.Add(CalculateHand(item));
            }
            CalculateWinnings(hands);
            long total = 0;
            foreach(var hand in hands)
            {
                total += hand.Winnings;
            }
            return total;
        }

        private static void CalculateWinnings(List<Hand> hands)
        {
            int ordering = hands.Count();
            var groupedRanks = hands.OrderByDescending(h => h.Rank).GroupBy(h => h.Rank);
            foreach(var groupOfCards in groupedRanks)
            {
                if (groupOfCards.Count() == 1){
                    groupOfCards.First().Ordering = ordering;
                    ordering--;
                    continue;
                }
                else
                {
                    bool orderChanged = true;
                    var orderedHands = groupOfCards.ToList();
                    while (orderChanged)
                    {
                        orderChanged = false;
                        for (var i = 0; i < orderedHands.Count() - 1; i++)
                        {
                            var orderHands = OrderHands(orderedHands[i], orderedHands[i+1]);
                            if (orderHands.Item3)
                            {
                                orderedHands[i] = orderHands.Item1;
                                orderedHands[i+1] = orderHands.Item2;
                                orderChanged = true;
                                break;
                            }
                        }
                    }
                    foreach(var hand in orderedHands)
                    {
                        hand.Ordering = ordering--;
                    }
                }
            }
            foreach(var hand in hands)
            {
                hand.Winnings = hand.Ordering * hand.Bid;
            }
            
        }

        private static Hand CalculateHand(string item)
        {
            var items = item.Split(" ");
            Hand hand = new Hand();
            hand.Bid = int.Parse(items[1]);
            foreach (var cardChar in items[0])
            {
                var card = cardChar switch
                {
                    '2' => Card.Two,
                    '3' => Card.Three,
                    '4' => Card.Four,
                    '5' => Card.Five,
                    '6' => Card.Six,
                    '7' => Card.Seven,
                    '8' => Card.Eight,
                    '9' => Card.Nine,
                    'T' => Card.T,
                    'J' => Card.J,
                    'Q' => Card.Q,
                    'K' => Card.K,
                    'A' => Card.A,
                    _ => throw new NotImplementedException()
                };
                hand.Cards.Add(card);
            }
            var groups = hand.Cards.GroupBy(h => h);
            int groupsCount = groups.Count();
            if (groupsCount == 1)
            {
                hand.Rank = Rank.FiveOfAKind;
            }else if (groupsCount == 2)
            {
                if (groups.Any(g => g.Count() == 4))
                {
                    hand.Rank = Rank.FourOfAKind;
                }
                else
                {
                    hand.Rank = Rank.FullHouse;
                }
            }
            else if (groupsCount == 3)
            {
                if (groups.Any(g => g.Count() == 3))
                {
                    hand.Rank = Rank.ThreeOfAKind;
                }
                else
                {
                    hand.Rank = Rank.TwoPair;
                }

            }
            else if (groupsCount == 4)
            {
                hand.Rank = Rank.OnePair;
            }
            else if(groupsCount == 5){
                hand.Rank = Rank.HighCard;
            }
            return hand;
        }

        private static (Hand, Hand, bool) OrderHands(Hand left, Hand right)
        {
            for(int i = 0; i<left.Cards.Count(); i++)
            {
                if (left.Cards[i] == right.Cards[i])
                {
                    continue;
                }else if (left.Cards[i] > right.Cards[i])
                {
                    return (left, right, false);
                }
                else
                {
                    return (right, left, true);
                }
            }
            return (left, right, false);
        }

        public static async Task<long> Two()
        {
            var data = await Common.ReadFile("Seven", "Two");
            List<HandTwo> hands = new List<HandTwo>();
            foreach (var item in data)
            {
                hands.Add(CalculateHandTwo(item));
            }
            CalculateWinningsTwo(hands);
            long total = 0;
            foreach (var hand in hands)
            {
                total += hand.Winnings;
            }
            return total;
        }

        private static HandTwo CalculateHandTwo(string item)
        {
            var items = item.Split(" ");
            HandTwo hand = new()
            {
                Bid = int.Parse(items[1])
            };
            foreach (var cardChar in items[0])
            {
                var card = cardChar switch
                {
                    'J' => CardTwo.J,
                    '2' => CardTwo.Two,
                    '3' => CardTwo.Three,
                    '4' => CardTwo.Four,
                    '5' => CardTwo.Five,
                    '6' => CardTwo.Six,
                    '7' => CardTwo.Seven,
                    '8' => CardTwo.Eight,
                    '9' => CardTwo.Nine,
                    'T' => CardTwo.T,
                    'Q' => CardTwo.Q,
                    'K' => CardTwo.K,
                    'A' => CardTwo.A,
                    _ => throw new NotImplementedException()
                }; ;
                hand.Cards.Add(card);
            }
            var jokes = hand.Cards.Where(h => h == CardTwo.J);
            if (!jokes.Any())
            {
                var groups = hand.Cards.GroupBy(h => h);
                int groupsCount = groups.Count();
                if (groupsCount == 1)
                {
                    hand.Rank = Rank.FiveOfAKind;
                }
                else if (groupsCount == 2)
                {
                    if (groups.Any(g => g.Count() == 4))
                    {
                        hand.Rank = Rank.FourOfAKind;
                    }
                    else
                    {
                        hand.Rank = Rank.FullHouse;
                    }
                }
                else if (groupsCount == 3)
                {
                    if (groups.Any(g => g.Count() == 3))
                    {
                        hand.Rank = Rank.ThreeOfAKind;
                    }
                    else
                    {
                        hand.Rank = Rank.TwoPair;
                    }

                }
                else if (groupsCount == 4)
                {
                    hand.Rank = Rank.OnePair;
                }
                else if (groupsCount == 5)
                {
                    hand.Rank = Rank.HighCard;
                }
                else
                {
                    throw new Exception();
                }
            }
            else if (jokes.Count() > 3)
            {
                hand.Rank = Rank.FiveOfAKind;
            }
            else
            {
                var groupsWithoutJokers = hand.Cards.Where(c => c != CardTwo.J).GroupBy(c => c);
                int groupsCount = groupsWithoutJokers.Count();

                if (groupsCount == 1)
                {
                    hand.Rank = Rank.FiveOfAKind;
                }
                else if (groupsCount == 2)
                {
                    if (jokes.Count() == 1)
                    {
                        if (groupsWithoutJokers.Any(c => c.Count() == 3))
                        {
                            hand.Rank = Rank.FourOfAKind;
                        }
                        else if (!groupsWithoutJokers.Any(c => c.Count() == 2))
                        {
                            throw new Exception();
                        }
                        else
                        {
                            hand.Rank = Rank.FullHouse;
                        }
                    }
                    else if (jokes.Count() == 2)
                    {
                        if (!groupsWithoutJokers.Any(c => c.Count() == 2))
                        {
                            throw new Exception();
                        }
                        hand.Rank = Rank.FourOfAKind;
                    }
                    else
                    {
                        if (!groupsWithoutJokers.Any(c => c.Count() == 1))
                        {
                            throw new Exception();
                        }
                        hand.Rank = Rank.FourOfAKind;
                    }
                }

                else if (groupsCount == 3)
                {
                    if (jokes.Count() == 1)
                    {
                        if (!groupsWithoutJokers.Any(c => c.Count() == 2))
                        {
                            throw new Exception();
                        }
                        hand.Rank = Rank.ThreeOfAKind;
                    }
                    else if (jokes.Count() == 2)
                    {
                        if (!groupsWithoutJokers.Any(c => c.Count() == 1))
                        {
                            throw new Exception();
                        }
                        hand.Rank = Rank.ThreeOfAKind;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else if (groupsCount == 4)
                {
                    if (jokes.Count() == 1)
                    {
                        if (!groupsWithoutJokers.Any(c => c.Count() == 1))
                        {
                            throw new Exception();
                        }
                        hand.Rank = Rank.OnePair;
                    }
                    else if (jokes.Count() == 2)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else if (groupsCount == 5 || groupsCount == 0)
                {
                    throw new Exception();
                }
            }
            return hand;
        }

        private static void CalculateWinningsTwo(List<HandTwo> hands)
        {
            int ordering = hands.Count();
            var groupedRanks = hands.OrderByDescending(h => h.Rank).GroupBy(h => h.Rank);
            foreach (var groupOfCards in groupedRanks)
            {
                if (groupOfCards.Count() == 1)
                {
                    groupOfCards.First().Ordering = ordering;
                    ordering--;
                    continue;
                }
                else
                {
                    bool orderChanged = true;
                    var orderedHands = groupOfCards.ToList();
                    while (orderChanged)
                    {
                        orderChanged = false;
                        for (var i = 0; i < orderedHands.Count() - 1; i++)
                        {
                            var orderHands = OrderHandsTwo(orderedHands[i], orderedHands[i + 1]);
                            if (orderHands.Item3)
                            {
                                orderedHands[i] = orderHands.Item1;
                                orderedHands[i + 1] = orderHands.Item2;
                                orderChanged = true;
                                break;
                            }
                        }
                    }
                    foreach (var hand in orderedHands)
                    {
                        hand.Ordering = ordering--;
                    }
                }
            }
            foreach (var hand in hands)
            {
                hand.Winnings = hand.Ordering * hand.Bid;
            }

        }

        private static (HandTwo, HandTwo, bool) OrderHandsTwo(HandTwo left, HandTwo right)
        {
            for (int i = 0; i < left.Cards.Count(); i++)
            {
                if (left.Cards[i] == right.Cards[i])
                {
                    continue;
                }
                else if (left.Cards[i] > right.Cards[i])
                {
                    return (left, right, false);
                }
                else
                {
                    return (right, left, true);
                }
            }
            throw new Exception();
        }

        private class Hand
        {
            public List<Card> Cards { get; set; } = new();
            public Rank Rank { get; set; }
            public long Bid { get; set; }
            public long Winnings { get; set; }
            public int Ordering { get; set; }
        }

        private class HandTwo
        {
            public List<CardTwo> Cards { get; set; } = new();
            public Rank Rank { get; set; }
            public long Bid { get; set; }
            public long Winnings { get; set; }
            public int Ordering { get; set; }
        }

        private enum Card
        {
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            T,
            J,
            Q,
            K,
            A
        }

        private enum CardTwo
        {
            J,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            T,
            Q,
            K,
            A
        }

        private enum Rank
        {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind
        }
    }
}
