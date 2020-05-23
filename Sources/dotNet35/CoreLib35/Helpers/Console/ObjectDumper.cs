#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:58 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

#region
using System;
using System.Collections;
using System.Reflection;
#endregion

namespace Library35.Helpers.Console
{
	public static class ObjectDumper
	{
		#region Methods

		#region WriteDump
		public static void WriteDump(object element)
		{
			WriteDump(element, 0);
		}

		public static void WriteLineDump(object element)
		{
			WriteLineDump(element, 0);
		}
		#endregion

		#region WriteDump
		public static void WriteDump(object element, int depth)
		{
			var dumper = new Dumper(depth);
			dumper.Write(element, null);
		}

		public static void WriteLineDump(object element, int depth)
		{
			var dumper = new Dumper(depth);
			dumper.WriteLine(element, null);
		}
		#endregion

		#endregion

		#region Nested Types

		#region Dumper
		private class Dumper : Dumper<Object>
		{
			internal Dumper(int depth)
				: base(depth)
			{
			}

			internal override void Write(object element, string prefix)
			{
				if (element == null || element is ValueType || element is string)
				{
					this.WriteIndent();
					this.Write(prefix);
					this.WriteValue(element);
					//WriteLine();
				}
				else
				{
					var enumerableElement = element as IEnumerable;
					if (enumerableElement != null)
						foreach (var item in enumerableElement)
							if (item is IEnumerable && !(item is string))
							{
								this.WriteIndent();
								this.Write(prefix);
								this.Write(string.Format("{0}...", item));
								this.WriteLine();
								if (this.Level < this.Depth)
								{
									this.Level++;
									this.Write(item, prefix);
									this.Level--;
								}
							}
							else
								this.Write(item, prefix);
					else
					{
						var members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
						this.WriteIndent();
						this.Write(prefix);
						var propWritten = false;
						foreach (var m in members)
						{
							var f = m as FieldInfo;
							var p = m as PropertyInfo;
							if (f != null || p != null)
							{
								if (propWritten)
									this.WriteTab();
								else
									propWritten = true;
								this.Write(m.Name);
								this.Write("=");
								var t = f != null ? f.FieldType : p.PropertyType;
								if (t.IsValueType || t == typeof (string))
									this.WriteValue(f != null ? f.GetValue(element) : p.GetValue(element, null));
								else if (typeof (IEnumerable).IsAssignableFrom(t))
									this.Write(string.Format("{0}...", f));
								else
									this.Write(string.Concat(f, " { }"));
							}
						}
						if (propWritten)
							this.WriteLine();
						if (this.Level < this.Depth)
							foreach (var m in members)
							{
								var f = m as FieldInfo;
								var p = m as PropertyInfo;
								if (f != null || p != null)
								{
									var t = f != null ? f.FieldType : p.PropertyType;
									if (!(t.IsValueType || t == typeof (string)))
									{
										var value = f != null ? f.GetValue(element) : p.GetValue(element, null);
										if (value != null)
										{
											this.Level++;
											this.Write(value, m.Name + ": ");
											this.Level--;
										}
									}
								}
							}
					}
				}
			}

			internal override void WriteLine(object element, string prefix)
			{
				if (element == null || element is ValueType || element is string)
				{
					this.WriteIndent();
					this.Write(prefix);
					this.WriteValue(element);
					this.WriteLine();
				}
				else
				{
					var enumerableElement = element as IEnumerable;
					if (enumerableElement != null)
						foreach (var item in enumerableElement)
							if (item is IEnumerable && !(item is string))
							{
								this.WriteIndent();
								this.Write(prefix);
								this.Write(string.Format("{0}...", item));
								this.WriteLine();
								if (this.Level < this.Depth)
								{
									this.Level++;
									this.Write(item, prefix);
									this.Level--;
								}
							}
							else
								this.WriteLine(item, prefix);
					else
					{
						var members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
						this.WriteIndent();
						this.Write(prefix);
						var propWritten = false;
						foreach (var m in members)
						{
							var f = m as FieldInfo;
							var p = m as PropertyInfo;
							if (f != null || p != null)
							{
								if (propWritten)
									this.WriteTab();
								else
									propWritten = true;
								this.Write(m.Name);
								this.Write("=");
								var t = f != null ? f.FieldType : p.PropertyType;
								if (t.IsValueType || t == typeof (string))
									this.WriteValue(f != null ? f.GetValue(element) : p.GetValue(element, null));
								else if (typeof (IEnumerable).IsAssignableFrom(t))
									this.Write(string.Format("{0}...", f));
								else
									this.Write(string.Concat(f, " { }"));
								this.WriteLine();
							}
						}
						if (propWritten)
							this.WriteLine();
						if (this.Level < this.Depth)
							foreach (var m in members)
							{
								var f = m as FieldInfo;
								var p = m as PropertyInfo;
								if (f != null || p != null)
								{
									var t = f != null ? f.FieldType : p.PropertyType;
									if (!(t.IsValueType || t == typeof (string)))
									{
										var value = f != null ? f.GetValue(element) : p.GetValue(element, null);
										if (value != null)
										{
											this.Level++;
											this.Write(value, m.Name + ": ");
											this.Level--;
											this.WriteLine();
										}
									}
								}
							}
					}
				}
			}

			private void WriteValue(object o)
			{
				if (o == null)
					this.Write("null");
				else if (o is DateTime)
					this.Write(((DateTime)o).ToShortDateString());
				else if (o is ValueType || o is string)
					this.Write(o.ToString());
				else if (o is IEnumerable)
					this.Write(string.Format("{0}...", o));
				else
					this.Write(string.Concat(o, " { }"));
			}
		}
		#endregion

		#endregion
	}
}