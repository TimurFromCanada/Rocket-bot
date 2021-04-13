using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot
{
    public partial class Bot
    {
        public Rocket GetNextMove(Rocket rocket)
        {
            var moves = new ConcurrentBag<Tuple<Turn, double>>();
            var tasks = new Task[threadsCount];

            for (var i = 0; i < threadsCount; i++)
            {
                tasks[i] = new Task(() =>
                {
                    var random = new Random();
                    var bestMove = SearchBestMove(rocket, this.random, iterationsCount / threadsCount);
                    moves.Add(bestMove);
                });
                tasks[i].Start();
            }

            Task.WaitAll(tasks);
            var result = moves.OrderBy(w => w.Item2).First();
            return rocket.Move(result.Item1, level);
        }
    }
}