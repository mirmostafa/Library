#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Library40.Helpers.Console.Interpret
{
	public class CommandArgument
	{
		private string _Syntax;

		public CommandArgument()
		{
			this.IsOptional = true;
		}

		public string Switch { get; set; }
		public string Description { get; set; }
		public Delegate Action { get; set; }
		public string Syntax
		{
			get { return this._Syntax ?? this.Switch.ToUpper(); }
			set { this._Syntax = value; }
		}
		public bool IsOptional { get; set; }
	}

	public class CommandArgumentCollection : Collection<CommandArgument>
	{
	}

	public class CommandArgumentInterpreter
	{
		private readonly string _AppName;
		private readonly CommandArgumentCollection _Arguments = new CommandArgumentCollection();
		private string _Syntax;

		public CommandArgumentInterpreter()
		{
			this._AppName = this.GetType().Assembly.FullName.Substring(0, this.GetType().Assembly.FullName.IndexOf(","));
		}

		public CommandArgumentCollection Arguments
		{
			get { return this._Arguments; }
		}
		public string Description { get; set; }
		public string Syntax
		{
			get
			{
				if (this._Syntax.IsNullOrEmpty())
				{
					this._Syntax = string.Concat(this._AppName, " ");
					foreach (var argument in this.Arguments)
						this._Syntax = string.Concat(this._Syntax, string.Format("{0} ", argument.IsOptional ? string.Format("[-{0}]", argument.Syntax) : string.Format("-{0}", argument.Syntax)));
				}
				return this._Syntax;
			}
			set { this._Syntax = value; }
		}
		public string Note { get; set; }

		public void ShowHelp()
		{
			if (this.Syntax.IsNullOrEmpty())
				this.InitailizeSyntax();
			var help = string.Format("{1} {0}{0}{2}{0}{0}", Environment.NewLine, this.Description, this.Syntax);

			help = this.Arguments.Aggregate(help, (current, arg) => string.Concat(current, string.Format("    -{0,-10}{1,-25}{2}", arg.Syntax, arg.Description, Environment.NewLine)));
			help = string.Concat(help, string.Format("{0}{0}Note:{1}", Environment.NewLine, this.Note));
			help.WriteLine();
		}

		protected virtual void InitailizeSyntax()
		{
			this.Syntax = this._AppName;
			foreach (var argument in this.Arguments)
				string.Concat(this.Syntax, string.Empty, argument.Switch.ToUpper());
		}

		internal void Interpret(IEnumerable<string> args, Action<string> log = null)
		{
			if (log == null)
				log = delegate { };

			var argsToExec = new List<CommandArgument>();
			foreach (var arg in args)
			{
				var argument = this.Arguments.Where(a => a.Switch.Trim().EqualsTo(arg.Trim())).FirstOrDefault();
				if (argument != null)
					argsToExec.Add(argument);
				else
					log(string.Format("Illegal argument:{0}", arg));
			}
			foreach (var command in argsToExec)
			{
				log(string.Format("Switch: {0}", command.Switch));
				command.Action.Method.Invoke(command.Action.Target, null);
			}
			log("Interpreting ended.");
		}
	}
}