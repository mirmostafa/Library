#region Code Identifications

// Created on     2018/07/22
// Last update on 2018/07/23 by Mohammad Mir mostafa 

#endregion

namespace Mohammad.Helpers.Internals
{
    public class MemberInfo<TEnum>
        where TEnum : struct
    {
        public MemberInfo(object displayMember, TEnum valueMember, object value)
        {
            this.Value         = value;
            this.DisplayMember = displayMember;
            this.ValueMember   = valueMember;
        }

        public                          object DisplayMember            { get; }
        public                          object Value                    { get; }
        public                          TEnum  ValueMember              { get; }
        public static implicit operator TEnum(MemberInfo<TEnum> member) => member.ValueMember;

        public override string ToString() => $"Display: {this.DisplayMember}  Value: {this.ValueMember.ToInt()}";
    }
}