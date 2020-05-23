#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

#region
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Library40.ExceptionHandlingPattern;
using Library40.Exceptions;
#endregion

namespace Library40.Helpers
{
	/// <summary>
	///     A utility to do some common tasks about methods
	/// </summary>
	public static class MethodHelper
	{
		/// <summary>
		/// </summary>
		public static string CallerMethodName
		{
			get { return GetCallerMethod().Name; }
		}

		/// <summary>
		///     Gets the active try clauses.
		/// </summary>
		/// <param name="type"> The type. </param>
		/// <returns> </returns>
		public static List<ExceptionHandlingClause> GetActiveTryClauses(Type type)
		{
			var activeTryClauses = new List<ExceptionHandlingClause>();

			var stackTrace = new StackTrace();
			var frames = stackTrace.GetFrames();

			// Start at 1 to skip the GetActiveTryClauses frame.
			if (frames != null)
				for (var i = 1; i < frames.Length; i++)
				{
					var frame = frames[i];
					var method = frame.GetMethod();
					var body = method.GetMethodBody();

					// Only consider methods that have an IL body.
					if (body == null)
						continue;
					IEnumerable<ExceptionHandlingClause> ehClauseList = body.ExceptionHandlingClauses;
					activeTryClauses.AddRange(from ehClause in ehClauseList
						where ehClause.Flags == ExceptionHandlingClauseOptions.Clause
						let offsetInFrame = frame.GetILOffset()
						let tryStartOffset = ehClause.TryOffset
						let tryEndOffset = tryStartOffset + ehClause.TryLength
						where (offsetInFrame >= tryStartOffset) && (offsetInFrame < tryEndOffset)
						where ehClause.CatchType != null
						where type == null || ehClause.CatchType.IsAssignableFrom(type)
						select ehClause);
				}

			return activeTryClauses;
		}

		/// <summary>
		/// </summary>
		/// <param name="method"> </param>
		/// <param name="defaultValue"> </param>
		/// <typeparam name="TAttribute"> </typeparam>
		/// <returns> </returns>
		public static TAttribute GetAttribute<TAttribute>(MethodBase method, TAttribute defaultValue) where TAttribute : Attribute
		{
			var attributes = method.GetCustomAttributes(typeof (TAttribute), false);
			return attributes.Length > 0 ? (TAttribute)attributes[0] : defaultValue;
		}

		/// <summary>
		/// </summary>
		/// <returns> </returns>
		public static MethodBase GetCallerMethod()
		{
			return GetCallerMethod(2);
		}

		/// <summary>
		/// </summary>
		/// <param name="index"> </param>
		/// <returns> </returns>
		public static MethodBase GetCallerMethod(int index)
		{
			var stackTrace = new StackTrace(true);
			return stackTrace.GetFrame(index) == null ? null : stackTrace.GetFrame(index).GetMethod();
		}

		/// <summary>
		/// </summary>
		/// <param name="index"> </param>
		/// <param name="parsePrevIfNull"> </param>
		/// <returns> </returns>
		public static MethodBase GetCallerMethod(int index, bool parsePrevIfNull)
		{
			var stackTrace = new StackTrace(true);
			if (stackTrace.GetFrame(index) == null && parsePrevIfNull)
				while (stackTrace.GetFrame(--index) == null)
				{
				}
			return stackTrace.GetFrame(index) == null ? null : stackTrace.GetFrame(index).GetMethod();
		}

		/// <summary>
		///     Gets the current method.
		/// </summary>
		/// <returns> </returns>
		public static MethodBase GetCurrentMethod()
		{
			return GetCallerMethod(2);
		}

		/// <summary>
		///     Gets a delegate from the given method name.
		/// </summary>
		/// <typeparam name="TType"> The type of the type. </typeparam>
		/// <typeparam name="TDelegate"> The type of the delegate. </typeparam>
		/// <param name="methodName"> Name of the method. </param>
		/// <returns> </returns>
		public static TDelegate GetDelegate<TType, TDelegate>(string methodName)
		{
			return (TDelegate)(ISerializable)Delegate.CreateDelegate(typeof (TDelegate), typeof (TType).GetMethod(methodName));
		}

		/// <summary>
		///     Invokes the specified method in target type.
		/// </summary>
		/// <typeparam name="TTargetType"> The type of the target type. </typeparam>
		/// <param name="target"> The target. </param>
		/// <param name="methodName"> Name of the method. </param>
		/// <param name="parameters"> The parameters. </param>
		/// <returns> </returns>
		public static object Invoke<TTargetType>(TTargetType target, string methodName, params object[] parameters)
		{
			if (string.IsNullOrEmpty(methodName))
			{
				if (parameters != null && parameters.Length > 0)
				{
					var types = new Type[parameters.Length];
					for (var counter = 0; counter != parameters.Length; counter++)
						types[counter] = parameters.GetType();
				}
				var contstructors = typeof (TTargetType).GetConstructors();
				return
					(from contstructor in contstructors where parameters != null where contstructor.GetParameters().GetLength(0) == parameters.Length select contstructor.Invoke(parameters))
						.FirstOrDefault();
			}
			var methods = typeof (TTargetType).GetMethods();
			return (from method in methods
				where String.Compare(method.Name, methodName, StringComparison.Ordinal) == 0
				where method.GetParameters().GetLength(0) == parameters.Length
				select method.Invoke(target, parameters)).FirstOrDefault();
		}

		/// <summary>
		///     Determines whether the specified type is caught.
		/// </summary>
		/// <param name="type"> The type. </param>
		/// <returns>
		///     <c>true</c> if the specified type is caught; otherwise, <c>false</c> .
		/// </returns>
		public static bool IsCaught(Type type)
		{
			var handlers = GetActiveTryClauses(type);
			return (handlers.Count > 0);
		}

		/// <summary>
		///     Catches the specified method  in try/catch.
		/// </summary>
		/// <typeparam name="T"> </typeparam>
		/// <param name="tryMethod"> The try method. </param>
		/// <param name="arg"> The arg. </param>
		/// <param name="handling"> The handling. </param>
		/// <param name="throwException">
		///     if set to <c>true</c> [throw exception].
		/// </param>
		/// <returns> </returns>
		public static Exception Catch<T>(this Action<T> tryMethod, T arg, ExceptionHandling handling, bool throwException = false)
		{
			try
			{
				tryMethod(arg);
				return null;
			}
			catch (Exception ex)
			{
				if (handling != null)
					handling.HandleException(ex);
				if (throwException)
					throw;
				return ex;
			}
		}

		public static IEnumerable<Exception> Catch<T>(this IEnumerable<Action<T>> tryMethods, T arg, ExceptionHandling handling, bool throwException = false)
		{
			return tryMethods.Select(tryMethod => tryMethod.Catch(arg, handling, throwException));
		}

		public static IEnumerable<Exception> Catch(this IEnumerable<Action> tryMethods, ExceptionHandling handling)
		{
			return tryMethods.Select(tryMethod => tryMethod.CatchByExceptionHandling(handling));
		}

		public static TResult Catch<TResult>(this Func<TResult> tryFunc, Func<Exception, TResult> catchFunc = null, Action finallyFunc = null)
		{
			try
			{
				return tryFunc();
			}
			catch (Exception ex)
			{
				return catchFunc != null ? catchFunc(ex) : default(TResult);
			}
			finally
			{
				if (finallyFunc != null)
					finallyFunc();
			}
		}

		/// <exception cref="ArgumentNullException">tryFunc</exception>
		public static void Catch(this Action tryFunc, Action<Exception> catchFunc = null, Action finallyFunc = null, bool throwException = false)
		{
			if (tryFunc == null)
				throw new ArgumentNullException("tryFunc");
			try
			{
				tryFunc();
			}
			catch (Exception ex)
			{
				if (catchFunc != null)
					catchFunc(ex);
				if (throwException)
					throw;
			}
			finally
			{
				if (finallyFunc != null)
					finallyFunc();
			}
		}

		/// <summary>
		///     Catches the specified method in try/catch.
		/// </summary>
		/// <param name="tryMethod"> The try method. </param>
		/// <param name="handling"> The handling. </param>
		/// <returns> </returns>
		public static Exception CatchByExceptionHandling(this Action tryMethod, ExceptionHandling handling = null)
		{
			try
			{
				tryMethod();
				return null;
			}
			catch (Exception ex)
			{
				if (handling != null)
					handling.HandleException(ex);
				return ex;
			}
		}

		public static void While(this Action action, Func<bool> predicate, bool runOnce = true)
		{
			if (runOnce)
				action();
			while (predicate())
				action();
		}

		/// <summary>
		///     Executes the specified method.
		/// </summary>
		/// <param name="method"> The method. </param>
		/// <param name="args"> The arguments. </param>
		public static void Execute(this Delegate method, params object[] args)
		{
			method.DynamicInvoke(null, args != null && args.Any() ? args : null);
		}

		/// <summary>
		///     Breaks this instance by throwing a BreakException.
		/// </summary>
		/// <exception cref="BreakException"></exception>
		public static void Break()
		{
			throw new BreakException();
		}

		public static Stopwatch Stopwatch(Action action)
		{
			var result = System.Diagnostics.Stopwatch.StartNew();
			action();
			result.Stop();
			return result;
		}

		public static void ExecOnDebugger(this Action action)
		{
			if (Debugger.IsAttached)
				if (action != null)
					action();
		}
	}
}