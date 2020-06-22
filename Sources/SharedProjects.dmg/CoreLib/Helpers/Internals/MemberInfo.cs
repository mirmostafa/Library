namespace Mohammad.Helpers.Internals
{
    public class MemberInfo<TEnum>
        where TEnum : struct
    {
        public object DisplayMember { get; }
        public TEnum ValueMember { get; }
        public object Value { get; private set; }

        public MemberInfo(object displayMember, TEnum valueMember, object value)
        {
            this.Value = value;
            this.DisplayMember = displayMember;
            this.ValueMember = valueMember;
        }

        public override string ToString() { return $"Display: {this.DisplayMember}  Value: {this.ValueMember.ToInt()}"; }
        public static implicit operator TEnum(MemberInfo<TEnum> member) { return member.ValueMember; }
    }
}