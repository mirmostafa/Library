#region File Notice
// Created at: 2013/12/24 3:42 PM
// Last Update time: 2013/12/24 3:59 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

namespace Library35.Helpers.Internals
{
	public class MemberInfo<TEnum>
		where TEnum : struct
	{
		#region Properties

		#region DisplayMember
		public object DisplayMember { get; private set; }
		#endregion

		#region ValueMember
		public TEnum ValueMember { get; private set; }
		#endregion

		#endregion

		#region Methods

		#region MemberInfo
		public MemberInfo(object displayMember, TEnum valueMember)
		{
			this.DisplayMember = displayMember;
			this.ValueMember = valueMember;
		}
		#endregion

		#region ToString
		public override string ToString()
		{
			return string.Format("Display: {0}  Value: {1}", this.DisplayMember, this.ValueMember.ToInt());
		}
		#endregion

		#endregion
	}
}