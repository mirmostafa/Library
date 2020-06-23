using System.Diagnostics;

namespace Mohammad.Helpers.Internals
{
    public class MemberInfo<TEnum>
        where TEnum : struct
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public object DisplayMember { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public object Value { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public TEnum ValueMember { get; }

        public MemberInfo(object displayMember, TEnum valueMember, object value)
        {
            this.Value = value;
            this.DisplayMember = displayMember;
            this.ValueMember = valueMember;
        }

        public static implicit operator TEnum(MemberInfo<TEnum> member) => member.ValueMember;

        public override string ToString() => $"Display: {this.DisplayMember}  Value: {this.ValueMember.ToInt()}";
    }
}