namespace Mohammad.Constants
{
    public class RegexPatterns
    {
        public const string ALPHA = @"^[a-zA-Z]*$";
        public const string ALPHA_UPPER_CASE = @"^[A-Z]*$";
        public const string ALPHA_LOWER_CASE = @"^[a-z]*$";
        public const string ALPHA_NUMERIC = @"^[a-zA-Z0-9]*$";
        public const string ALPHA_NUMERIC_SPACE = @"^[a-zA-Z0-9 ]*$";
        public const string ALPHA_NUMERIC_SPACE_DASH = @"^[a-zA-Z0-9 \-]*$";
        public const string ALPHA_NUMERIC_SPACE_DASH_UNDERSCORE = @"^[a-zA-Z0-9 \-_]*$";
        public const string ALPHA_NUMERIC_SPACE_DASH_UNDERSCORE_PERIOD = @"^[a-zA-Z0-9\. \-_]*$";
        public const string NUMERIC = @"^\-?[0-9]*\.?[0-9]*$";
        public const string SOCIAL_SECURITY = @"\d{3}[-]?\d{2}[-]?\d{4}";
        public const string EMAIL = @"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$";
        public const string URL = @"^^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$";
        public const string ZIP_CODE_US = @"\d{5}";
        public const string ZIP_CODE_US_WITH_FOUR = @"\d{5}[-]\d{4}";
        public const string ZIP_CODE_US_WITH_FOUR_OPTIONAL = @"\d{5}([-]\d{4})?";
        public const string PHONE_US = @"\d{3}[-]?\d{3}[-]?\d{4}";
        public const string CELL_PHONE = @"^(0|\+98)?([ ]|-|[()]){0,2}9[0-9]([ ]|-|[()]){0,2}(?:[0-9]([ ]|-|[()]){0,2}){8}$";
        public const string EMAIL_ADDRESS = "^([a-zA-Z0-9_'+*$%\\^&!\\.\\-])+\\@(([a-zA-Z0-9\\-])+\\.)+([a-zA-Z0-9:]{2,4})+$";
        public const string CURRENCY = "(^\\$?(?!0,?\\d)\\d{1,3}(,?\\d{3})*(\\.\\d\\d)?)$";

        public const string DATE =
            "^(((0?[1-9]|[12]\\d|3[01])[\\.\\-\\/](0?[13578]|1[02])[\\.\\-\\/]((1[6-9]|[2-9]\\d)?\\d{2}|\\d))|((0?[1-9]|[12]\\d|30)[\\.\\-\\/](0?[13456789]|1[012])[\\.\\-\\/]((1[6-9]|[2-9]\\d)?\\d{2}|\\d))|((0?[1-9]|1\\d|2[0-8])[\\.\\-\\/]0?2[\\.\\-\\/]((1[6-9]|[2-9]\\d)?\\d{2}|\\d))|(29[\\.\\-\\/]0?2[\\.\\-\\/]((1[6-9]|[2-9]\\d)?(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)|00|[048])))$";
    }
}