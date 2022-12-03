string[] games = File.ReadAllLines("input.txt");
int totalScore = 0;

foreach (var game in games)
{
    var otherHand = char.Parse(game.Split( )[0]);
    var yourHand = char.Parse(game.Split( )[1]);
    switch (otherHand)
    {
        case 'A':
            totalScore += playGame2(otherHand, yourHand);
            break;
        case 'B':
            totalScore += playGame2(otherHand, yourHand);
            break;
        case 'C':
            totalScore += playGame2(otherHand, yourHand);
            break;
    }
}

Console.WriteLine(totalScore);

int playGame1(char otherHand, char yourHand)
{
    Dictionary<char, int> pointsBonus = new()
    {
        {'X', 1},
        {'Y', 2},
        {'Z', 3}
    };

    Dictionary<char, char> convertHand = new()
    {
        {'A', 'X'},
        {'B', 'Y'},
        {'C', 'Z'}
    };
    
    var points = 0;
    if (convertHand[otherHand] == yourHand)
    {
        points += 3;
    }
    else if ((char) (convertHand[otherHand] + 1) == yourHand || (char) (convertHand[otherHand] - 2) == yourHand) 
    {
        points += 6;
    }

    points += pointsBonus[yourHand];
    return points;
}

int playGame2(char otherHand, char yourHand)
{
    Dictionary<char, int> pointsBonus = new()
    {
        {'X', 1},
        {'Y', 2},
        {'Z', 3}
    };

    Dictionary<char, char> findSameCard = new()
    {
        {'A', 'X'},
        {'B', 'Y'},
        {'C', 'Z'}
    };

    Dictionary<char, char> findLosingCard = new()
    {
        {'A', 'Z'},
        {'B', 'X'},
        {'C', 'Y'}
    };

    Dictionary<char, char> findWinningCard = new()
    {
        {'A', 'Y'},
        {'B', 'Z'},
        {'C', 'X'}
    };

    var points = 0;
    if (yourHand == 'X')
    {
        // We have to lose
        points += pointsBonus[findLosingCard[otherHand]];
    }
    else if (yourHand == 'Y') 
    {
        // We have to play a draw
        points += 3 + pointsBonus[findSameCard[otherHand]];
    }
    else 
    {
        // We have to win
        points += 6 + pointsBonus[findWinningCard[otherHand]];

    }
    return points;
}