using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Library45.EventsArgs;
using Library45.Helpers;
using Calendar = System.Globalization.Calendar;

namespace Library45.Wpf.Windows.Controls
{
    public partial class LibDatePickerTextBox : INotifyPropertyChanged
    {
        public static readonly DependencyProperty ErrorStyleProperty = DependencyProperty.Register("ErrorStyle",
            typeof (Style),
            typeof (LibDatePickerTextBox),
            new PropertyMetadata(null));
        public static readonly DependencyProperty ThrowExceptionsProperty = DependencyProperty.Register("ThrowExceptions",
            typeof (bool),
            typeof (LibDatePickerTextBox),
            new PropertyMetadata(default(bool)));
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate",
            typeof (DateTime?),
            typeof (LibDatePickerTextBox),
            new PropertyMetadata(default(DateTime),
                (sender, e) =>
                {
                    var me = sender as LibDatePickerTextBox;
                    if (me == null)
                        return;
                    me.OnSelectedDateTimeChanged();
                    me.OnPropertyChanged("SelectedDateTime");
                }));
        public static readonly DependencyProperty HijriAdjustmentProperty = DependencyProperty.Register("HijriAdjustment",
            typeof (int),
            typeof (LibDatePickerTextBox),
            new PropertyMetadata(default(int)));
        public static readonly DependencyProperty SelectedCalendarTypeProperty = DependencyProperty.Register("SelectedCalendarType",
            typeof (CalendarType),
            typeof (LibDatePickerTextBox),
            new PropertyMetadata(default(CalendarType),
                (sender, e) =>
                {
                    var me = sender as LibDatePickerTextBox;
                    if (me == null)
                        return;
                    if ((CalendarType)e.NewValue == CalendarType.CurrentCulture)
                    {
                        me.SelectedCalendarType = GetCorrectCalendarTypeDependingOnCurrentCulture();
                        return;
                    }
                    me.OnSelectedCalendarTypeChanged();
                    me.OnPropertyChanged("SelectedCalendarType");
                }));
        public static readonly DependencyProperty GotoDefaultDateButtonVisibilityProperty = DependencyProperty.Register("GotoDefaultDateButtonVisibility",
            typeof (Visibility),
            typeof (LibDatePickerTextBox),
            new PropertyMetadata(Visibility.Collapsed));
        public static readonly DependencyProperty CalendarTypeVisibilityProperty = DependencyProperty.Register("CalendarTypeVisibility",
            typeof (Visibility),
            typeof (LibDatePickerTextBox),
            new PropertyMetadata(Visibility.Collapsed));
        public static readonly DependencyProperty IsNullableProperty = DependencyProperty.Register("IsNullable",
            typeof (bool),
            typeof (LibDatePickerTextBox),
            new PropertyMetadata(true));
        public static readonly DependencyProperty DropDownButtonVisibilityProperty = DependencyProperty.Register("DropDownButtonVisibility",
            typeof (Visibility),
            typeof (LibDatePickerTextBox),
            new PropertyMetadata(Visibility.Collapsed));
        private Calendar _Calendar;
        private string _Day;
        private string _Era;
        private bool _InternalChanging;
        private string _Month;
        private bool _MoveNextOnEnter = true;
        private string _Year;

        public LibDatePickerTextBox()
        {
            this.InitializeComponent();

            EventManager.RegisterClassHandler(typeof (TextBox), KeyDownEvent, new KeyEventHandler(this.TextBox_OnKeyDown));
        }

        public bool ThrowExceptions
        {
            get { return (bool)this.GetValue(ThrowExceptionsProperty); }
            set { this.SetValue(ThrowExceptionsProperty, value); }
        }
        public Style ErrorStyle
        {
            get { return (Style)this.GetValue(ErrorStyleProperty) ?? (this.ErrorStyle = (Style)this.FindResource("ErrorStyle")); }
            set { this.SetValue(ErrorStyleProperty, value); }
        }
        public bool IsNullable
        {
            get { return (bool)this.GetValue(IsNullableProperty); }
            set { this.SetValue(IsNullableProperty, value); }
        }
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string Era
        {
            get
            {
                if (this._Era == "00" || (this._Era ?? string.Empty).Length == 1)
                    this.Era = this.GetEra(this.OnGettingDefaultDate().Date).ToString();
                return this._Era;
            }
            set
            {
                if (Equals(this._Era, value))
                    return;
                if (this._Era == "00" || (this._Era ?? string.Empty).Length == 1)
                    this._Era = this.GetEra(this.OnGettingDefaultDate().Date).ToString();
                else
                    this._Era = value;
                this.OnDataChanged();
                this.OnPropertyChanged("Era");
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string Year
        {
            get { return this._Year; }
            set
            {
                if (Equals(value, this._Year))
                    return;
                this._Year = value;
                this.OnDataChanged();
                this.OnPropertyChanged("Year");
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string Month
        {
            get { return this._Month; }
            set
            {
                if (Equals(value, this._Month))
                    return;
                this._Month = value;
                this.OnDataChanged();
                this.OnPropertyChanged("Month");
            }
        }
        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public string Day
        {
            get { return this._Day; }
            set
            {
                if (Equals(value, this._Day))
                    return;
                this._Day = value;
                this.OnDataChanged();
                this.OnPropertyChanged("Day");
            }
        }
        public DateTime? SelectedDate
        {
            get { return (DateTime)this.GetValue(SelectedDateProperty); }
            set { this.SetValue(SelectedDateProperty, value); }
        }
        public CalendarType SelectedCalendarType
        {
            get { return (CalendarType)this.GetValue(SelectedCalendarTypeProperty); }
            set { this.SetValue(SelectedCalendarTypeProperty, value); }
        }
        public Calendar Calendar
        {
            get { return this._Calendar ?? (this._Calendar = Thread.CurrentThread.CurrentUICulture.Calendar); }
            private set { this._Calendar = value; }
        }
        public Visibility CalendarTypeVisibility
        {
            get { return (Visibility)this.GetValue(CalendarTypeVisibilityProperty); }
            set { this.SetValue(CalendarTypeVisibilityProperty, value); }
        }
        public int HijriAdjustment
        {
            get { return (int)this.GetValue(HijriAdjustmentProperty); }
            set { this.SetValue(HijriAdjustmentProperty, value); }
        }
        public bool MoveNextOnEnter
        {
            get { return this._MoveNextOnEnter; }
            set
            {
                if (Equals(this._MoveNextOnEnter, value))
                    return;
                this._MoveNextOnEnter = value;
                this.OnPropertyChanged("MoveNextOnEnter");
            }
        }
        public Visibility DropDownButtonVisibility
        {
            get { return (Visibility)this.GetValue(DropDownButtonVisibilityProperty); }
            set
            {
                throw new NotSupportedException("Pending. Not supported yet.");
                this.SetValue(DropDownButtonVisibilityProperty, value);
            }
        }
        public Visibility GotoDefaultDateButtonVisibility
        {
            get { return (Visibility)this.GetValue(GotoDefaultDateButtonVisibilityProperty); }
            set { this.SetValue(GotoDefaultDateButtonVisibilityProperty, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private static CalendarType GetCorrectCalendarTypeDependingOnCurrentCulture()
        {
            CalendarType ct;
            if (Thread.CurrentThread.CurrentUICulture.Name.ToLower().StartsWith("en-"))
                ct = CalendarType.Gregorian;
            else if (Thread.CurrentThread.CurrentUICulture.Name.ToLower().StartsWith("fa-"))
                ct = CalendarType.Persian;
            else if (Thread.CurrentThread.CurrentUICulture.Name.ToLower().StartsWith("ar-"))
                ct = CalendarType.Hijri;
            else
                throw new NotSupportedException("Current Culture not supported.");
            return ct;
        }
        public event EventHandler<ItemActedEventArgs<DateTime>> GettingDefaultDate;
        public event EventHandler<ItemActedEventArgs<Exception>> ErrorOcuurred;

        private int GetEra(DateTime dateTime)
        {
            return Convert.ToInt32(this.Calendar.GetYear(dateTime).ToString().Substring(0, 2));
        }
        private int GetYear(DateTime dateTime)
        {
            return Convert.ToInt32(this.Calendar.GetYear(dateTime).ToString().Substring(2));
        }
        private void OnDataChanged()
        {
            if (this._InternalChanging)
                return;
            this._InternalChanging = true;
            try
            {
                this.Style = null;
                this.SelectedDate = string.IsNullOrEmpty(this.Era) || string.IsNullOrEmpty(this.Year) || string.IsNullOrEmpty(this.Month) || string.IsNullOrEmpty(this.Day)
                    ? (DateTime?)null
                    : this.Calendar.ToDateTime(int.Parse(this.Era + this.Year), int.Parse(this.Month), int.Parse(this.Day), 0, 0, 0, 0);
                this.SetError();
            }
            catch (Exception ex)
            {
                this.SetError(ex);
            }
            finally
            {
                this._InternalChanging = false;
            }
        }
        private void SetError(Exception ex = null)
        {
            if (ex == null)
            {
                this.Style = (Style)this.FindResource("NormalStyle");
                return;
            }
            this.SelectedDate = null;
            this.ToolTip = ex.GetBaseException().Message;
            this.Style = this.ErrorStyle;
            this.OnErrorOcuurred(new ItemActedEventArgs<Exception>(ex));
        }
        protected virtual void OnErrorOcuurred(ItemActedEventArgs<Exception> e)
        {
            var handler = this.ErrorOcuurred;
            if (handler == null)
            {
                if (!this.ThrowExceptions)
                    return;
                throw e.Item;
            }
            handler(this, e);
        }
        private void OnSelectedDateTimeChanged()
        {
            if (this._InternalChanging)
                return;
            try
            {
                this.SetError();
                if (this.SelectedDate == null)
                    return;

                this._InternalChanging = true;
                var selectedDate = this.SelectedDate.Value;
                this.Era = this.GetEra(selectedDate.Date).ToString();
                this.Year = this.GetYear(selectedDate.Date).ToString();
                this.Month = this.Calendar.GetMonth(selectedDate.Date).ToString("00");
                this.Day = this.Calendar.GetDayOfMonth(selectedDate.Date).ToString("00");
            }
            catch (Exception ex)
            {
                this.SetError(ex);
            }
            finally
            {
                this._InternalChanging = false;
            }
        }
        private void OnSelectedCalendarTypeChanged()
        {
            switch (this.SelectedCalendarType)
            {
                case CalendarType.CurrentCulture:
                    var culture = Thread.CurrentThread.CurrentUICulture.Name.Substring(0, 2).ToLower();
                    switch (culture)
                    {
                        case "en":
                            this.Calendar = new GregorianCalendar();
                            break;
                        case "fa":
                            this.Calendar = new PersianCalendar();
                            break;
                        case "ar":
                            this.Calendar = new HijriCalendar();
                            break;
                        default:
                            throw new NotSupportedException("Current UI culture is not supported.");
                    }
                    break;
                case CalendarType.Persian:
                    this.Calendar = new PersianCalendar();
                    break;
                case CalendarType.Gregorian:
                    this.Calendar = new GregorianCalendar();
                    break;
                case CalendarType.Hijri:
                    this.Calendar = new HijriCalendar {HijriAdjustment = this.HijriAdjustment};
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            this.OnSelectedDateTimeChanged();
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        private void TextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!this.MoveNextOnEnter)
                return;
            if (e.Key == Key.Enter && !((TextBox)sender).AcceptsReturn)
                ControlHelper.MoveToNextUIElement(e);
        }
        private void GotoDefaultDateButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.SelectedDate = this.OnGettingDefaultDate();
        }
        private DateTime OnGettingDefaultDate()
        {
            if (this.GettingDefaultDate == null)
                return DateTime.Now;
            var result = DateTime.Now;
            this.GettingDefaultDate(this, new ItemActedEventArgs<DateTime>(result));
            return result;
        }
        private void DatePickerTextBox_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.SelectedCalendarType = GetCorrectCalendarTypeDependingOnCurrentCulture();
            this.SelectedDate = DateTime.Now;
            this.OnSelectedCalendarTypeChanged();
        }
        private void DatePickerTextBox_OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                this.SelectedDate = this.OnGettingDefaultDate();
        }
        private void SelectedCalendarTypeButton_OnClick(object sender, RoutedEventArgs e)
        {
            var cts = Enum.GetValues(typeof (CalendarType)).Cast<int>();
            if ((int)this.SelectedCalendarType == cts.Max())
                this.SelectedCalendarType = (CalendarType)1;
            else
                this.SelectedCalendarType += 1;
        }
    }

    public class CalendarTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ct = value is CalendarType ? (CalendarType)value : CalendarType.CurrentCulture;
            switch (ct)
            {
                case CalendarType.CurrentCulture:
                    return "C";
                case CalendarType.Persian:
                    return "ش";
                case CalendarType.Gregorian:
                    return "G";
                case CalendarType.Hijri:
                    return "ق";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public enum CalendarType
    {
        CurrentCulture,
        Persian,
        Gregorian,
        Hijri,
    }
}