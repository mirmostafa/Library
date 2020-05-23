#region Code Identifications

// Created on     2018/07/29
// Last update on 2018/07/29 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Mohammad.Diagnostics;
using Mohammad.Helpers;
using Mohammad.ProgressiveOperations;

namespace TestConsole45
{
    public class SumAlgorithmsTest : MultiStepOperation
    {
        private Data _Data;

        /// <inheritdoc />
        protected override void OnInitializingMainOperationSteps(MultiStepOperationStepCollection steps)
        {
            steps.Add(new MultiStepOperationStep(this, this.InitializeActors, "Initializing..."));
            steps.Add(new MultiStepOperationStep(this, this.ExecuteActors, "Executing...."));
            steps.Add(new MultiStepOperationStep(this, this.GatherResults, "Gathering results..."));
            steps.Add(new MultiStepOperationStep(this, this.AnalysisResults, "Analyzing results..."));
        }

        private void AnalysisResults()
        {
            var winner = this._Data.Results.First(res => res.Time == this._Data.Results.Select(r => r.Time).Min());
            var loser = this._Data.Results.First(res => res.Time == this._Data.Results.Select(r => r.Time).Max());
            this.Info($"The winner is {winner.Name} with {winner.Time}");
            this.Info($"The loser is {loser.Name} with {loser.Time}");
        }

        private void GatherResults()
        {
            this._Data.Results = new List<(TimeSpan Time, string Name)>();
            foreach (var (_, name) in this._Data.Actors)
            {
                var average = this._Data.Info.Where(i => i.Name == name).Average(i => i.Calc.Ticks).ToLong();
                var time = TimeSpan.FromTicks(average);
                this.Info($"The average time for {name} is {time}");
                this._Data.Results.Add((time, name));
            }
        }

        private void ExecuteActors()
        {
            const int max = 10;
            this.StartCurrentOperation(max);
            for (var i = 0; i < max; i++)
            {
                foreach (var actor in this._Data.Actors)
                {
                    this.Info($"Running {actor.Name}");
                    this._Data.Info.Add((Run(actor), actor.Name));
                }

                this.CurrentOperationIncreased($"Phase {i} of {max}");
            }
        }

        private void InitializeActors()
        {
            this._Data = new Data
            {
                Actors = new List<(Func<IEnumerable<long>, long> Sun, string Name)>
                {
                    (SumAlgorithms.Sum1, "Sum1"),
                    (SumAlgorithms.Sum2, "Sum2"),
                    (SumAlgorithms.Sum3, "Sum3"),
                    (SumAlgorithms.Sum4, "Sum4")
                },
                Info = new List<(TimeSpan Calc, string Name)>()
            };
        }

        private static TimeSpan Run((Func<IEnumerable<long>, long> Sun, string Name) actor)
        {
            return Diag.Stopwatch(() => actor.Sun(Enumerable.Range(1, 100000000).Select(n => n.ToLong()))).Elapsed;
        }

        private static class SumAlgorithms
        {
            public static long Sum1(IEnumerable<long> nums)
            {
                var result = 0L;
                foreach (var num in nums)
                {
                    result += num;
                }

                return result;
            }

            public static long Sum2(IEnumerable<long> nums)
            {
                var result = 0L;
                foreach (var num in nums.AsParallel())
                {
                    result += num;
                }

                return result;
            }

            public static long Sum3(IEnumerable<long> nums) => nums.Sum();
            public static long Sum4(IEnumerable<long> nums) => nums.AsParallel().Sum();
        }

        private struct Data
        {
            public List<(Func<IEnumerable<long>, long> Sun, string Name)> Actors;
            public List<(TimeSpan Calc, string Name)> Info;
            public List<(TimeSpan Time, string Name)> Results;
        }
    }
}