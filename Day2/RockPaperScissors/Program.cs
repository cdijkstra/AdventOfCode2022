namespace HelloWorld
{
    class Game
    {
        private Dictionary<char, char> convertHand = new()
        {
            {'A', 'X'},
            {'B', 'Y'},
            {'C', 'Z'}
        };

        private Dictionary<char, int> cardBonus = new()
        {
            {'X', 1},
            {'Y', 2},
            {'Z', 3}
        };

        private Dictionary<char, char> findSameCard = new()
        {
            {'A', 'X'},
            {'B', 'Y'},
            {'C', 'Z'}
        };

        private Dictionary<char, char> findLosingCard = new()
        {
            {'A', 'Z'},
            {'B', 'X'},
            {'C', 'Y'}
        };

        private Dictionary<char, char> findWinningCard = new()
        {
            {'A', 'Y'},
            {'B', 'Z'},
            {'C', 'X'}
        };

        int playGame1(char otherHand, char yourHand)
        {                
            var points = 0;
            if (convertHand[otherHand] == yourHand)
            {
                points += 3;
            }
            else if ((char) (convertHand[otherHand] + 1) == yourHand || (char) (convertHand[otherHand] - 2) == yourHand) 
            {
                points += 6;
            }

            points += cardBonus[yourHand];
            return points;
        }

        int playGame2(char otherHand, char yourHand)
        {
            var points = 0;
            switch (yourHand)
            {
                case 'X':
                    // We have to lose
                    points += cardBonus[findLosingCard[otherHand]];
                    break;
                case 'Y':
                    // We have to play a draw
                    points += 3 + cardBonus[findSameCard[otherHand]];
                    break;
                case 'Z':
                    // We have to win
                    points += 6 + cardBonus[findWinningCard[otherHand]];
                    break;
                default:
                    throw new Exception("Unexpected input");
            }
            return points;
        }


        public void PlayGames()
        {
            string[] games = File.ReadAllLines("input.txt");
            int totalScoreGame1 = 0;
            int totalScoreGame2 = 0;

            foreach (var game in games)
            {
                var otherHand = char.Parse(game.Split( )[0]);
                var yourHand = char.Parse(game.Split( )[1]);
                totalScoreGame1 += playGame1(otherHand, yourHand);
                totalScoreGame2 += playGame2(otherHand, yourHand);
            }

            Console.WriteLine($"Result for game 1 = {totalScoreGame1} and for game 2 = {totalScoreGame2}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            game.PlayGames();
        }
    }
}