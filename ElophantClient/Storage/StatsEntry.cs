///*
//copyright (C) 2011-2012 by high828@gmail.com

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//*/
//using System;
//using ElophantClient.Messages.GameStats;
//using ElophantClient.Messages.GameStats.PlayerStats;
//using NotMissing;

//namespace ElophantClient.Storage
//{
//    public class StatsEntry : ICloneable
//    {
//        public StatsEntry()
//        {
//        }
//        public StatsEntry(EndOfGameStats game, PlayerStatsSummary stats)
//        {
//            GameMode = game.GameMode;
//            GameType = game.GameType;
//            Summary = stats;
//            TimeStamp = game.TimeStamp;
//        }
//        public string GameMode { get; set; }
//        public string GameType { get; set; }
//        public long TimeStamp { get; set; }
//        public PlayerStatsSummary Summary { get; set; }

//        public object Clone()
//        {
//            return new StatsEntry
//            {
//               GameMode = GameMode,
//               GameType = GameType,
//               TimeStamp = TimeStamp,
//               Summary = Summary.CloneT(),
//            };
//        }
//    }
//}
