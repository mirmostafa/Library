// Created on     2018/07/29
// Last update on 2018/07/29 by Mohammad Mir mostafa 

using System;
using System.Collections.Generic;

namespace TestConsole45.ProgressiveApproach
{
    public class Step : IStep
    {
        private readonly List<IStep> _Children;

        /// <inheritdoc />
        public Delegate MainAction { get; }

        /// <inheritdoc />
        public IEnumerable<IStep> Children => this._Children;

        /// <inheritdoc />
        public string Description { get; }

        /// <inheritdoc />
        public int Priority { get; }

        public Step(Action mainAction, string description = null, IEnumerable<IStep> children = null, int priority = 0)
        {
            this.MainAction = mainAction ?? throw new ArgumentNullException(nameof(mainAction));
            this._Children = children != null ? new List<IStep>(children) : new List<IStep>();
            this.Description = description;
            this.Priority = priority;
        }

        public Step(Action<IStep> mainAction, string description = null, IEnumerable<IStep> children = null, int priority = 0)
        {
            this.MainAction = mainAction ?? throw new ArgumentNullException(nameof(mainAction));
            this._Children = children != null ? new List<IStep>(children) : new List<IStep>();
            this.Description = description;
            this.Priority = priority;
        }

        public Step(Action<Actor> mainAction, string description = null, IEnumerable<IStep> children = null, int priority = 0)
        {
            this.MainAction = mainAction ?? throw new ArgumentNullException(nameof(mainAction));
            this._Children = children != null ? new List<IStep>(children) : new List<IStep>();
            this.Description = description;
            this.Priority = priority;
        }

        public Step(Action<Actor, IStep> mainAction, string description = null, IEnumerable<IStep> children = null, int priority = 0)
        {
            this.MainAction = mainAction ?? throw new ArgumentNullException(nameof(mainAction));
            this._Children = children != null ? new List<IStep>(children) : new List<IStep>();
            this.Description = description;
            this.Priority = priority;
        }

        public Step AddChildren(params Step[] chilStep)
        {
            this._Children.AddRange(chilStep);
            return this;
        }

        public Step AddChildren(IEnumerable<Step> chilStep)
        {
            this._Children.AddRange(chilStep);
            return this;
        }
    }
}