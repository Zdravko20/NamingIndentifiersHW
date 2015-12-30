using System;

namespace Application2
{
    public class LeaderBoard
    {
        private string playerName;
        private int points;

        public LeaderBoard(string name, int points)
        {
            this.playerName = name;
            this.points = points;
        }

        public string PlayerName
        {
            get
            {
                return playerName;
            }

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("Name cannot be empty");
                }
                this.playerName = value;
            }
        }

        public int Points
        {
            get
            {
                return points;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Points cannot be negative");
                }
                this.points = value;
            }
        }
    }
}
