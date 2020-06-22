using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Mohammad.Win32.Natives.IfacesEnumsStructsClasses
{
    // ReSharper disable InconsistentNaming
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SERVER_INFO_101
    {
        public int dwPlatformID;
        public IntPtr lpszServerName;
        public int dwVersionMajor;
        public int dwVersionMinor;
        public int dwType;
        public IntPtr lpszComment;
    }

    /// <summary>
    ///     Contains information about the file that is found
    ///     by the FindFirstFile or FindNextFile functions.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    [BestFitMapping(false)]
    public class WIN32_FIND_DATA
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string cAlternateFileName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string cFileName;

        public FileAttributes dwFileAttributes;
        public int dwReserved0;
        public int dwReserved1;
        public uint ftCreationTime_dwHighDateTime;
        public uint ftCreationTime_dwLowDateTime;
        public uint ftLastAccessTime_dwHighDateTime;
        public uint ftLastAccessTime_dwLowDateTime;
        public uint ftLastWriteTime_dwHighDateTime;
        public uint ftLastWriteTime_dwLowDateTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
    }

    public sealed class WindowsMessages
    {
        public const int WS_EX_DLGMODALFRAME = 0x0001;
        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_FRAMECHANGED = 0x0020;
        public const long AW_BLEND = 0x00080000;
        public const long AW_CENTER = 0x00000010;
        public const long AW_HIDE = 0x00010000;
        public const int GWL_HINSTANCE = -6;
        public const int HCBT_ACTIVATE = 5;
        public const int WH_CBT = 5;
        public const long WS_EX_RIGHT = 0x00001000L;
        public const long WS_EX_RTLREADING = 0x00002000L;
        public const long WS_EX_LAYERED = 0x00080000;
        public const int GWL_EXSTYLE = -20;
        public const int GWLP_HINSTANCE = -6;
        public const int GWLP_ID = -12;
        public const int GWL_STYLE = -16;
        public const int GWLP_USERDATA = -21;
        public const int GWLP_WNDPROC = -4;
        public const int ACM_OPENA = WM_USER + 100;
        public const int ACM_OPENW = WM_USER + 103;
        public const int ACM_PLAY = WM_USER + 101;
        public const int ACM_STOP = WM_USER + 102;
        public const int BCM_FIRST = 0x1600; // Button control messages
        public const int BCM_GETIDEALSIZE = BCM_FIRST + 0x0001;
        public const int BCM_GETIMAGELIST = BCM_FIRST + 0x0003;
        public const int BCM_GETTEXTMARGIN = BCM_FIRST + 0x0005;
        public const int BCM_SETIMAGELIST = BCM_FIRST + 0x0002;
        public const int BCM_SETTEXTMARGIN = BCM_FIRST + 0x0004;
        public const int BM_CLICK = 0x00F5;
        public const int BM_GETCHECK = 0x00F0;
        public const int BM_GETIMAGE = 0x00F6;
        public const int BM_GETSTATE = 0x00F2;
        public const int BM_SETCHECK = 0x00F1;
        public const int BM_SETIMAGE = 0x00F7;
        public const int BM_SETSTATE = 0x00F3;
        public const int BM_SETSTYLE = 0x00F4;
        public const int CB_ADDSTRING = 0x0143;
        public const int CB_DELETESTRING = 0x0144;
        public const int CB_DIR = 0x0145;
        public const int CB_FINDSTRING = 0x014C;
        public const int CB_FINDSTRINGEXACT = 0x0158;
        public const int CB_GETCOMBOBOXINFO = 0x0164;
        public const int CB_GETCOUNT = 0x0146;
        public const int CB_GETCURSEL = 0x0147;
        public const int CB_GETDROPPEDCONTROLRECT = 0x0152;
        public const int CB_GETDROPPEDSTATE = 0x0157;
        public const int CB_GETDROPPEDWIDTH = 0x015f;
        public const int CB_GETEDITSEL = 0x0140;
        public const int CB_GETEXTENDEDUI = 0x0156;
        public const int CB_GETHORIZONTALEXTENT = 0x015d;
        public const int CB_GETITEMDATA = 0x0150;
        public const int CB_GETITEMHEIGHT = 0x0154;
        public const int CB_GETLBTEXT = 0x0148;
        public const int CB_GETLBTEXTLEN = 0x0149;
        public const int CB_GETLOCALE = 0x015A;
        public const int CB_GETMINVISIBLE = CBM_FIRST + 2;
        public const int CB_GETTOPINDEX = 0x015B;
        public const int CB_INITSTORAGE = 0x0161;
        public const int CB_INSERTSTRING = 0x014A;
        public const int CB_LIMITTEXT = 0x0141;
        public const int CB_MSGMAX_400 = 0x0162;
        public const int CB_MSGMAX_501 = 0x0165;
        public const int CB_MSGMAX_PRE400 = 0x015B;
        public const int CB_MSGMAX_WCE400 = 0x0163;
        public const int CB_MULTIPLEADDSTRING = 0x0163;
        public const int CB_RESETCONTENT = 0x014B;
        public const int CB_SELECTSTRING = 0x014D;
        public const int CB_SETCURSEL = 0x014E;
        public const int CB_SETDROPPEDWIDTH = 0x0160;
        public const int CB_SETEDITSEL = 0x0142;
        public const int CB_SETEXTENDEDUI = 0x0155;
        public const int CB_SETHORIZONTALEXTENT = 0x015e;
        public const int CB_SETITEMDATA = 0x0151;
        public const int CB_SETITEMHEIGHT = 0x0153;
        public const int CB_SETLOCALE = 0x0159;
        public const int CB_SETMINVISIBLE = CBM_FIRST + 1;
        public const int CB_SETTOPINDEX = 0x015C;
        public const int CB_SHOWDROPDOWN = 0x014F;
        public const int CBEM_DELETEITEM = CB_DELETESTRING;
        public const int CBEM_GETCOMBOCONTROL = WM_USER + 6;
        public const int CBEM_GETEDITCONTROL = WM_USER + 7;
        public const int CBEM_GETEXSTYLE = WM_USER + 9;
        public const int CBEM_GETEXTENDEDSTYLE = WM_USER + 9;
        public const int CBEM_GETIMAGELIST = WM_USER + 3;
        public const int CBEM_GETITEMA = WM_USER + 4;
        public const int CBEM_GETITEMW = WM_USER + 13;
        public const int CBEM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int CBEM_HASEDITCHANGED = WM_USER + 10;
        public const int CBEM_INSERTITEMA = WM_USER + 1;
        public const int CBEM_INSERTITEMW = WM_USER + 11;
        public const int CBEM_SETEXSTYLE = WM_USER + 8;
        public const int CBEM_SETEXTENDEDSTYLE = WM_USER + 14;
        public const int CBEM_SETIMAGELIST = WM_USER + 2;
        public const int CBEM_SETITEMA = WM_USER + 5;
        public const int CBEM_SETITEMW = WM_USER + 12;
        public const int CBEM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int CBM_FIRST = 0x1700; // Combobox control messages
        public const int CCM_DPISCALE = CCM_FIRST + 0xc;
        public const int CCM_FIRST = 0x2000; // Common control shared messages
        public const int CCM_GETCOLORSCHEME = CCM_FIRST + 3;
        public const int CCM_GETDROPTARGET = CCM_FIRST + 4;
        public const int CCM_GETUNICODEFORMAT = CCM_FIRST + 6;
        public const int CCM_GETVERSION = CCM_FIRST + 0x8;
        public const int CCM_LAST = CCM_FIRST + 0x200;
        public const int CCM_SETBKCOLOR = CCM_FIRST + 1;
        public const int CCM_SETCOLORSCHEME = CCM_FIRST + 2;
        public const int CCM_SETNOTIFYWINDOW = CCM_FIRST + 0x9;
        public const int CCM_SETUNICODEFORMAT = CCM_FIRST + 5;
        public const int CCM_SETVERSION = CCM_FIRST + 0x7;
        public const int CCM_SETWINDOWTHEME = CCM_FIRST + 0xb;
        public const int DL_BEGINDRAG = WM_USER + 133;
        public const int DL_CANCELDRAG = WM_USER + 136;
        public const int DL_DRAGGING = WM_USER + 134;
        public const int DL_DROPPED = WM_USER + 135;
        public const int DM_GETDEFID = WM_USER + 0;
        public const int DM_REPOSITION = WM_USER + 2;
        public const int DM_SETDEFID = WM_USER + 1;
        public const int DTM_FIRST = 0x1000;
        public const int DTM_GETMCCOLOR = DTM_FIRST + 7;
        public const int DTM_GETMCFONT = DTM_FIRST + 10;
        public const int DTM_GETMONTHCAL = DTM_FIRST + 8;
        public const int DTM_GETRANGE = DTM_FIRST + 3;
        public const int DTM_GETSYSTEMTIME = DTM_FIRST + 1;
        public const int DTM_SETFORMATA = DTM_FIRST + 5;
        public const int DTM_SETFORMATW = DTM_FIRST + 50;
        public const int DTM_SETMCCOLOR = DTM_FIRST + 6;
        public const int DTM_SETMCFONT = DTM_FIRST + 9;
        public const int DTM_SETRANGE = DTM_FIRST + 4;
        public const int DTM_SETSYSTEMTIME = DTM_FIRST + 2;
        public const int ECM_FIRST = 0x1500; // Edit control messages
        public const int EM_CANUNDO = 0x00C6;
        public const int EM_CHARFROMPOS = 0x00D7;
        public const int EM_EMPTYUNDOBUFFER = 0x00CD;
        public const int EM_FMTLINES = 0x00C8;
        public const int EM_GETCUEBANNER = ECM_FIRST + 2;
        public const int EM_GETFIRSTVISIBLELINE = 0x00CE;
        public const int EM_GETHANDLE = 0x00BD;
        public const int EM_GETIMESTATUS = 0x00D9;
        public const int EM_GETLIMITTEXT = 0x00D5;
        public const int EM_GETLINE = 0x00C4;
        public const int EM_GETLINECOUNT = 0x00BA;
        public const int EM_GETMARGINS = 0x00D4;
        public const int EM_GETMODIFY = 0x00B8;
        public const int EM_GETPASSWORDCHAR = 0x00D2;
        public const int EM_GETRECT = 0x00B2;
        public const int EM_GETSEL = 0x00B0;
        public const int EM_GETTHUMB = 0x00BE;
        public const int EM_GETWORDBREAKPROC = 0x00D1;
        public const int EM_HIDEBALLOONTIP = ECM_FIRST + 4;
        public const int EM_LIMITTEXT = 0x00C5;
        public const int EM_LINEFROMCHAR = 0x00C9;
        public const int EM_LINEINDEX = 0x00BB;
        public const int EM_LINELENGTH = 0x00C1;
        public const int EM_LINESCROLL = 0x00B6;
        public const int EM_POSFROMCHAR = 0x00D6;
        public const int EM_REPLACESEL = 0x00C2;
        public const int EM_SCROLL = 0x00B5;
        public const int EM_SCROLLCARET = 0x00B7;
        public const int EM_SETCUEBANNER = ECM_FIRST + 1;
        public const int EM_SETHANDLE = 0x00BC;
        public const int EM_SETIMESTATUS = 0x00D8;
        public const int EM_SETLIMITTEXT = EM_LIMITTEXT;
        public const int EM_SETMARGINS = 0x00D3;
        public const int EM_SETMODIFY = 0x00B9;
        public const int EM_SETPASSWORDCHAR = 0x00CC;
        public const int EM_SETREADONLY = 0x00CF;
        public const int EM_SETRECT = 0x00B3;
        public const int EM_SETRECTNP = 0x00B4;
        public const int EM_SETSEL = 0x00B1;
        public const int EM_SETTABSTOPS = 0x00CB;
        public const int EM_SETWORDBREAKPROC = 0x00D0;
        public const int EM_SHOWBALLOONTIP = ECM_FIRST + 3;
        public const int EM_UNDO = 0x00C7;
        public const int HDM_CLEARFILTER = HDM_FIRST + 24;
        public const int HDM_CREATEDRAGIMAGE = HDM_FIRST + 16;
        public const int HDM_DELETEITEM = HDM_FIRST + 2;
        public const int HDM_EDITFILTER = HDM_FIRST + 23;
        public const int HDM_FIRST = 0x1200; // Header messages
        public const int HDM_GETBITMAPMARGIN = HDM_FIRST + 21;
        public const int HDM_GETIMAGELIST = HDM_FIRST + 9;
        public const int HDM_GETITEMA = HDM_FIRST + 3;
        public const int HDM_GETITEMCOUNT = HDM_FIRST + 0;
        public const int HDM_GETITEMRECT = HDM_FIRST + 7;
        public const int HDM_GETITEMW = HDM_FIRST + 11;
        public const int HDM_GETORDERARRAY = HDM_FIRST + 17;
        public const int HDM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int HDM_HITTEST = HDM_FIRST + 6;
        public const int HDM_INSERTITEMA = HDM_FIRST + 1;
        public const int HDM_INSERTITEMW = HDM_FIRST + 10;
        public const int HDM_LAYOUT = HDM_FIRST + 5;
        public const int HDM_ORDERTOINDEX = HDM_FIRST + 15;
        public const int HDM_SETBITMAPMARGIN = HDM_FIRST + 20;
        public const int HDM_SETFILTERCHANGETIMEOUT = HDM_FIRST + 22;
        public const int HDM_SETHOTDIVIDER = HDM_FIRST + 19;
        public const int HDM_SETIMAGELIST = HDM_FIRST + 8;
        public const int HDM_SETITEMA = HDM_FIRST + 4;
        public const int HDM_SETITEMW = HDM_FIRST + 12;
        public const int HDM_SETORDERARRAY = HDM_FIRST + 18;
        public const int HDM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int HKM_GETHOTKEY = WM_USER + 2;
        public const int HKM_SETHOTKEY = WM_USER + 1;
        public const int HKM_SETRULES = WM_USER + 3;
        public const int LB_ADDFILE = 0x0196;
        public const int LB_ADDSTRING = 0x0180;
        public const int LB_DELETESTRING = 0x0182;
        public const int LB_DIR = 0x018D;
        public const int LB_FINDSTRING = 0x018F;
        public const int LB_FINDSTRINGEXACT = 0x01A2;
        public const int LB_GETANCHORINDEX = 0x019D;
        public const int LB_GETCARETINDEX = 0x019F;
        public const int LB_GETCOUNT = 0x018B;
        public const int LB_GETCURSEL = 0x0188;
        public const int LB_GETHORIZONTALEXTENT = 0x0193;
        public const int LB_GETITEMDATA = 0x0199;
        public const int LB_GETITEMHEIGHT = 0x01A1;
        public const int LB_GETITEMRECT = 0x0198;
        public const int LB_GETLISTBOXINFO = 0x01B2;
        public const int LB_GETLOCALE = 0x01A6;
        public const int LB_GETSEL = 0x0187;
        public const int LB_GETSELCOUNT = 0x0190;
        public const int LB_GETSELITEMS = 0x0191;
        public const int LB_GETTEXT = 0x0189;
        public const int LB_GETTEXTLEN = 0x018A;
        public const int LB_GETTOPINDEX = 0x018E;
        public const int LB_INITSTORAGE = 0x01A8;
        public const int LB_INSERTSTRING = 0x0181;
        public const int LB_ITEMFROMPOINT = 0x01A9;
        public const int LB_MSGMAX_4 = 0x01B0;
        public const int LB_MSGMAX_501 = 0x01B3;
        public const int LB_MSGMAX_PRE4 = 0x01A8;
        public const int LB_MSGMAX_WCE4 = 0x01B1;
        public const int LB_MULTIPLEADDSTRING = 0x01B1;
        public const int LB_RESETCONTENT = 0x0184;
        public const int LB_SELECTSTRING = 0x018C;
        public const int LB_SELITEMRANGE = 0x019B;
        public const int LB_SELITEMRANGEEX = 0x0183;
        public const int LB_SETANCHORINDEX = 0x019C;
        public const int LB_SETCARETINDEX = 0x019E;
        public const int LB_SETCOLUMNWIDTH = 0x0195;
        public const int LB_SETCOUNT = 0x01A7;
        public const int LB_SETCURSEL = 0x0186;
        public const int LB_SETHORIZONTALEXTENT = 0x0194;
        public const int LB_SETITEMDATA = 0x019A;
        public const int LB_SETITEMHEIGHT = 0x01A0;
        public const int LB_SETLOCALE = 0x01A5;
        public const int LB_SETSEL = 0x0185;
        public const int LB_SETTABSTOPS = 0x0192;
        public const int LB_SETTOPINDEX = 0x0197;
        public const int LM_GETIDEALHEIGHT = WM_USER + 0x301;
        public const int LM_GETITEM = WM_USER + 0x303;
        public const int LM_HITTEST = WM_USER + 0x300;
        public const int LM_SETITEM = WM_USER + 0x302;
        public const int LVM_APPROXIMATEVIEWRECT = LVM_FIRST + 64;
        public const int LVM_ARRANGE = LVM_FIRST + 22;
        public const int LVM_CANCELEDITLABEL = LVM_FIRST + 179;
        public const int LVM_CREATEDRAGIMAGE = LVM_FIRST + 33;
        public const int LVM_DELETEALLITEMS = LVM_FIRST + 9;
        public const int LVM_DELETECOLUMN = LVM_FIRST + 28;
        public const int LVM_DELETEITEM = LVM_FIRST + 8;
        public const int LVM_EDITLABELA = LVM_FIRST + 23;
        public const int LVM_EDITLABELW = LVM_FIRST + 118;
        public const int LVM_ENABLEGROUPVIEW = LVM_FIRST + 157;
        public const int LVM_ENSUREVISIBLE = LVM_FIRST + 19;
        public const int LVM_FINDITEMA = LVM_FIRST + 13;
        public const int LVM_FINDITEMW = LVM_FIRST + 83;
        public const int LVM_FIRST = 0x1000; // ListView messages
        public const int LVM_GETBKCOLOR = LVM_FIRST + 0;
        public const int LVM_GETBKIMAGEA = LVM_FIRST + 69;
        public const int LVM_GETBKIMAGEW = LVM_FIRST + 139;
        public const int LVM_GETCALLBACKMASK = LVM_FIRST + 10;
        public const int LVM_GETCOLUMNA = LVM_FIRST + 25;
        public const int LVM_GETCOLUMNORDERARRAY = LVM_FIRST + 59;
        public const int LVM_GETCOLUMNW = LVM_FIRST + 95;
        public const int LVM_GETCOLUMNWIDTH = LVM_FIRST + 29;
        public const int LVM_GETCOUNTPERPAGE = LVM_FIRST + 40;
        public const int LVM_GETEDITCONTROL = LVM_FIRST + 24;
        public const int LVM_GETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 55;
        public const int LVM_GETGROUPINFO = LVM_FIRST + 149;
        public const int LVM_GETGROUPMETRICS = LVM_FIRST + 156;
        public const int LVM_GETHOTCURSOR = LVM_FIRST + 63;
        public const int LVM_GETHOTITEM = LVM_FIRST + 61;
        public const int LVM_GETHOVERTIME = LVM_FIRST + 72;
        public const int LVM_GETIMAGELIST = LVM_FIRST + 2;
        public const int LVM_GETINSERTMARK = LVM_FIRST + 167;
        public const int LVM_GETINSERTMARKCOLOR = LVM_FIRST + 171;
        public const int LVM_GETINSERTMARKRECT = LVM_FIRST + 169;
        public const int LVM_GETISEARCHSTRINGA = LVM_FIRST + 52;
        public const int LVM_GETISEARCHSTRINGW = LVM_FIRST + 117;
        public const int LVM_GETITEMA = LVM_FIRST + 5;
        public const int LVM_GETITEMCOUNT = LVM_FIRST + 4;
        public const int LVM_GETITEMPOSITION = LVM_FIRST + 16;
        public const int LVM_GETITEMRECT = LVM_FIRST + 14;
        public const int LVM_GETITEMSPACING = LVM_FIRST + 51;
        public const int LVM_GETITEMSTATE = LVM_FIRST + 44;
        public const int LVM_GETITEMTEXTA = LVM_FIRST + 45;
        public const int LVM_GETITEMTEXTW = LVM_FIRST + 115;
        public const int LVM_GETITEMW = LVM_FIRST + 75;
        public const int LVM_GETNUMBEROFWORKAREAS = LVM_FIRST + 73;
        public const int LVM_GETORIGIN = LVM_FIRST + 41;
        public const int LVM_GETOUTLINECOLOR = LVM_FIRST + 176;
        public const int LVM_GETSELECTEDCOLUMN = LVM_FIRST + 174;
        public const int LVM_GETSELECTEDCOUNT = LVM_FIRST + 50;
        public const int LVM_GETSELECTIONMARK = LVM_FIRST + 66;
        public const int LVM_GETSTRINGWIDTHA = LVM_FIRST + 17;
        public const int LVM_GETSTRINGWIDTHW = LVM_FIRST + 87;
        public const int LVM_GETSUBITEMRECT = LVM_FIRST + 56;
        public const int LVM_GETTEXTBKCOLOR = LVM_FIRST + 37;
        public const int LVM_GETTEXTCOLOR = LVM_FIRST + 35;
        public const int LVM_GETTILEINFO = LVM_FIRST + 165;
        public const int LVM_GETTILEVIEWINFO = LVM_FIRST + 163;
        public const int LVM_GETTOOLTIPS = LVM_FIRST + 78;
        public const int LVM_GETTOPINDEX = LVM_FIRST + 39;
        public const int LVM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int LVM_GETVIEW = LVM_FIRST + 143;
        public const int LVM_GETVIEWRECT = LVM_FIRST + 34;
        public const int LVM_GETWORKAREAS = LVM_FIRST + 70;
        public const int LVM_HASGROUP = LVM_FIRST + 161;
        public const int LVM_HITTEST = LVM_FIRST + 18;
        public const int LVM_INSERTCOLUMNA = LVM_FIRST + 27;
        public const int LVM_INSERTCOLUMNW = LVM_FIRST + 97;
        public const int LVM_INSERTGROUP = LVM_FIRST + 145;
        public const int LVM_INSERTGROUPSORTED = LVM_FIRST + 159;
        public const int LVM_INSERTITEMA = LVM_FIRST + 7;
        public const int LVM_INSERTITEMW = LVM_FIRST + 77;
        public const int LVM_INSERTMARKHITTEST = LVM_FIRST + 168;
        public const int LVM_ISGROUPVIEWENABLED = LVM_FIRST + 175;
        public const int LVM_MAPIDTOINDEX = LVM_FIRST + 181;
        public const int LVM_MAPINDEXTOID = LVM_FIRST + 180;
        public const int LVM_MOVEGROUP = LVM_FIRST + 151;
        public const int LVM_MOVEITEMTOGROUP = LVM_FIRST + 154;
        public const int LVM_REDRAWITEMS = LVM_FIRST + 21;
        public const int LVM_REMOVEALLGROUPS = LVM_FIRST + 160;
        public const int LVM_REMOVEGROUP = LVM_FIRST + 150;
        public const int LVM_SCROLL = LVM_FIRST + 20;
        public const int LVM_SETBKCOLOR = LVM_FIRST + 1;
        public const int LVM_SETBKIMAGEA = LVM_FIRST + 68;
        public const int LVM_SETBKIMAGEW = LVM_FIRST + 138;
        public const int LVM_SETCALLBACKMASK = LVM_FIRST + 11;
        public const int LVM_SETCOLUMNA = LVM_FIRST + 26;
        public const int LVM_SETCOLUMNORDERARRAY = LVM_FIRST + 58;
        public const int LVM_SETCOLUMNW = LVM_FIRST + 96;
        public const int LVM_SETCOLUMNWIDTH = LVM_FIRST + 30;
        public const int LVM_SETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 54;
        public const int LVM_SETGROUPINFO = LVM_FIRST + 147;
        public const int LVM_SETGROUPMETRICS = LVM_FIRST + 155;
        public const int LVM_SETHOTCURSOR = LVM_FIRST + 62;
        public const int LVM_SETHOTITEM = LVM_FIRST + 60;
        public const int LVM_SETHOVERTIME = LVM_FIRST + 71;
        public const int LVM_SETICONSPACING = LVM_FIRST + 53;
        public const int LVM_SETIMAGELIST = LVM_FIRST + 3;
        public const int LVM_SETINFOTIP = LVM_FIRST + 173;
        public const int LVM_SETINSERTMARK = LVM_FIRST + 166;
        public const int LVM_SETINSERTMARKCOLOR = LVM_FIRST + 170;
        public const int LVM_SETITEMA = LVM_FIRST + 6;
        public const int LVM_SETITEMCOUNT = LVM_FIRST + 47;
        public const int LVM_SETITEMPOSITION = LVM_FIRST + 15;
        public const int LVM_SETITEMPOSITION32 = LVM_FIRST + 49;
        public const int LVM_SETITEMSTATE = LVM_FIRST + 43;
        public const int LVM_SETITEMTEXTA = LVM_FIRST + 46;
        public const int LVM_SETITEMTEXTW = LVM_FIRST + 116;
        public const int LVM_SETITEMW = LVM_FIRST + 76;
        public const int LVM_SETOUTLINECOLOR = LVM_FIRST + 177;
        public const int LVM_SETSELECTEDCOLUMN = LVM_FIRST + 140;
        public const int LVM_SETSELECTIONMARK = LVM_FIRST + 67;
        public const int LVM_SETTEXTBKCOLOR = LVM_FIRST + 38;
        public const int LVM_SETTEXTCOLOR = LVM_FIRST + 36;
        public const int LVM_SETTILEINFO = LVM_FIRST + 164;
        public const int LVM_SETTILEVIEWINFO = LVM_FIRST + 162;
        public const int LVM_SETTILEWIDTH = LVM_FIRST + 141;
        public const int LVM_SETTOOLTIPS = LVM_FIRST + 74;
        public const int LVM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int LVM_SETVIEW = LVM_FIRST + 142;
        public const int LVM_SETWORKAREAS = LVM_FIRST + 65;
        public const int LVM_SORTGROUPS = LVM_FIRST + 158;
        public const int LVM_SORTITEMS = LVM_FIRST + 48;
        public const int LVM_SORTITEMSEX = LVM_FIRST + 81;
        public const int LVM_SUBITEMHITTEST = LVM_FIRST + 57;
        public const int LVM_UPDATE = LVM_FIRST + 42;
        public const int MCM_FIRST = 0x1000;
        public const int MCM_GETCOLOR = MCM_FIRST + 11;
        public const int MCM_GETCURSEL = MCM_FIRST + 1;
        public const int MCM_GETFIRSTDAYOFWEEK = MCM_FIRST + 16;
        public const int MCM_GETMAXSELCOUNT = MCM_FIRST + 3;
        public const int MCM_GETMAXTODAYWIDTH = MCM_FIRST + 21;
        public const int MCM_GETMINREQRECT = MCM_FIRST + 9;
        public const int MCM_GETMONTHDELTA = MCM_FIRST + 19;
        public const int MCM_GETMONTHRANGE = MCM_FIRST + 7;
        public const int MCM_GETRANGE = MCM_FIRST + 17;
        public const int MCM_GETSELRANGE = MCM_FIRST + 5;
        public const int MCM_GETTODAY = MCM_FIRST + 13;
        public const int MCM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int MCM_HITTEST = MCM_FIRST + 14;
        public const int MCM_SETCOLOR = MCM_FIRST + 10;
        public const int MCM_SETCURSEL = MCM_FIRST + 2;
        public const int MCM_SETDAYSTATE = MCM_FIRST + 8;
        public const int MCM_SETFIRSTDAYOFWEEK = MCM_FIRST + 15;
        public const int MCM_SETMAXSELCOUNT = MCM_FIRST + 4;
        public const int MCM_SETMONTHDELTA = MCM_FIRST + 20;
        public const int MCM_SETRANGE = MCM_FIRST + 18;
        public const int MCM_SETSELRANGE = MCM_FIRST + 6;
        public const int MCM_SETTODAY = MCM_FIRST + 12;
        public const int MCM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int PBM_DELTAPOS = WM_USER + 3;
        public const int PBM_GETPOS = WM_USER + 8;
        public const int PBM_GETRANGE = WM_USER + 7;
        public const int PBM_SETBARCOLOR = WM_USER + 9;
        public const int PBM_SETBKCOLOR = CCM_SETBKCOLOR;
        public const int PBM_SETPOS = WM_USER + 2;
        public const int PBM_SETRANGE = WM_USER + 1;
        public const int PBM_SETRANGE32 = WM_USER + 6;
        public const int PBM_SETSTEP = WM_USER + 4;
        public const int PBM_STEPIT = WM_USER + 5;
        public const int PGM_FIRST = 0x1400; // Pager control messages
        public const int PGM_FORWARDMOUSE = PGM_FIRST + 3;
        public const int PGM_GETBKCOLOR = PGM_FIRST + 5;
        public const int PGM_GETBORDER = PGM_FIRST + 7;
        public const int PGM_GETBUTTONSIZE = PGM_FIRST + 11;
        public const int PGM_GETBUTTONSTATE = PGM_FIRST + 12;
        public const int PGM_GETDROPTARGET = CCM_GETDROPTARGET;
        public const int PGM_GETPOS = PGM_FIRST + 9;
        public const int PGM_RECALCSIZE = PGM_FIRST + 2;
        public const int PGM_SETBKCOLOR = PGM_FIRST + 4;
        public const int PGM_SETBORDER = PGM_FIRST + 6;
        public const int PGM_SETBUTTONSIZE = PGM_FIRST + 10;
        public const int PGM_SETCHILD = PGM_FIRST + 1;
        public const int PGM_SETPOS = PGM_FIRST + 8;
        public const int RB_BEGINDRAG = WM_USER + 24;
        public const int RB_DELETEBAND = WM_USER + 2;
        public const int RB_DRAGMOVE = WM_USER + 26;
        public const int RB_ENDDRAG = WM_USER + 25;
        public const int RB_GETBANDBORDERS = WM_USER + 34;
        public const int RB_GETBANDCOUNT = WM_USER + 12;
        public const int RB_GETBANDINFO = WM_USER + 5;
        public const int RB_GETBANDINFOA = WM_USER + 29;
        public const int RB_GETBANDINFOW = WM_USER + 28;
        public const int RB_GETBANDMARGINS = WM_USER + 40;
        public const int RB_GETBARHEIGHT = WM_USER + 27;
        public const int RB_GETBARINFO = WM_USER + 3;
        public const int RB_GETBKCOLOR = WM_USER + 20;
        public const int RB_GETCOLORSCHEME = CCM_GETCOLORSCHEME;
        public const int RB_GETDROPTARGET = CCM_GETDROPTARGET;
        public const int RB_GETPALETTE = WM_USER + 38;
        public const int RB_GETRECT = WM_USER + 9;
        public const int RB_GETROWCOUNT = WM_USER + 13;
        public const int RB_GETROWHEIGHT = WM_USER + 14;
        public const int RB_GETTEXTCOLOR = WM_USER + 22;
        public const int RB_GETTOOLTIPS = WM_USER + 17;
        public const int RB_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int RB_HITTEST = WM_USER + 8;
        public const int RB_IDTOINDEX = WM_USER + 16;
        public const int RB_INSERTBANDA = WM_USER + 1;
        public const int RB_INSERTBANDW = WM_USER + 10;
        public const int RB_MAXIMIZEBAND = WM_USER + 31;
        public const int RB_MINIMIZEBAND = WM_USER + 30;
        public const int RB_MOVEBAND = WM_USER + 39;
        public const int RB_PUSHCHEVRON = WM_USER + 43;
        public const int RB_SETBANDINFOA = WM_USER + 6;
        public const int RB_SETBANDINFOW = WM_USER + 11;
        public const int RB_SETBARINFO = WM_USER + 4;
        public const int RB_SETBKCOLOR = WM_USER + 19;
        public const int RB_SETCOLORSCHEME = CCM_SETCOLORSCHEME;
        public const int RB_SETPALETTE = WM_USER + 37;
        public const int RB_SETPARENT = WM_USER + 7;
        public const int RB_SETTEXTCOLOR = WM_USER + 21;
        public const int RB_SETTOOLTIPS = WM_USER + 18;
        public const int RB_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int RB_SETWINDOWTHEME = CCM_SETWINDOWTHEME;
        public const int RB_SHOWBAND = WM_USER + 35;
        public const int RB_SIZETORECT = WM_USER + 23;
        public const int SB_GETBORDERS = WM_USER + 7;
        public const int SB_GETICON = WM_USER + 20;
        public const int SB_GETPARTS = WM_USER + 6;
        public const int SB_GETRECT = WM_USER + 10;
        public const int SB_GETTEXTA = WM_USER + 2;
        public const int SB_GETTEXTLENGTHA = WM_USER + 3;
        public const int SB_GETTEXTLENGTHW = WM_USER + 12;
        public const int SB_GETTEXTW = WM_USER + 13;
        public const int SB_GETTIPTEXTA = WM_USER + 18;
        public const int SB_GETTIPTEXTW = WM_USER + 19;
        public const int SB_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int SB_ISSIMPLE = WM_USER + 14;
        public const int SB_SETBKCOLOR = CCM_SETBKCOLOR;
        public const int SB_SETICON = WM_USER + 15;
        public const int SB_SETMINHEIGHT = WM_USER + 8;
        public const int SB_SETPARTS = WM_USER + 4;
        public const int SB_SETTEXTA = WM_USER + 1;
        public const int SB_SETTEXTW = WM_USER + 11;
        public const int SB_SETTIPTEXTA = WM_USER + 16;
        public const int SB_SETTIPTEXTW = WM_USER + 17;
        public const int SB_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int SB_SIMPLE = WM_USER + 9;
        public const int SB_SIMPLEID = 0x00ff;
        public const int SBM_ENABLE_ARROWS = 0x00E4;
        public const int SBM_GETPOS = 0x00E1;
        public const int SBM_GETRANGE = 0x00E3;
        public const int SBM_GETSCROLLBARINFO = 0x00EB;
        public const int SBM_GETSCROLLINFO = 0x00EA;
        public const int SBM_SETPOS = 0x00E0;
        public const int SBM_SETRANGE = 0x00E2;
        public const int SBM_SETRANGEREDRAW = 0x00E6;
        public const int SBM_SETSCROLLINFO = 0x00E9;
        public const int STM_GETICON = 0x0171;
        public const int STM_GETIMAGE = 0x0173;
        public const int STM_MSGMAX = 0x0174;
        public const int STM_SETICON = 0x0170;
        public const int STM_SETIMAGE = 0x0172;
        public const int TB_ADDBITMAP = WM_USER + 19;
        public const int TB_ADDBUTTONS = WM_USER + 20;
        public const int TB_ADDBUTTONSA = WM_USER + 20;
        public const int TB_ADDBUTTONSW = WM_USER + 68;
        public const int TB_ADDSTRINGA = WM_USER + 28;
        public const int TB_ADDSTRINGW = WM_USER + 77;
        public const int TB_AUTOSIZE = WM_USER + 33;
        public const int TB_BUTTONCOUNT = WM_USER + 24;
        public const int TB_BUTTONSTRUCTSIZE = WM_USER + 30;
        public const int TB_CHANGEBITMAP = WM_USER + 43;
        public const int TB_CHECKBUTTON = WM_USER + 2;
        public const int TB_COMMANDTOINDEX = WM_USER + 25;
        public const int TB_CUSTOMIZE = WM_USER + 27;
        public const int TB_DELETEBUTTON = WM_USER + 22;
        public const int TB_ENABLEBUTTON = WM_USER + 1;
        public const int TB_GETANCHORHIGHLIGHT = WM_USER + 74;
        public const int TB_GETBITMAP = WM_USER + 44;
        public const int TB_GETBITMAPFLAGS = WM_USER + 41;
        public const int TB_GETBUTTON = WM_USER + 23;
        public const int TB_GETBUTTONINFOA = WM_USER + 65;
        public const int TB_GETBUTTONINFOW = WM_USER + 63;
        public const int TB_GETBUTTONSIZE = WM_USER + 58;
        public const int TB_GETBUTTONTEXTA = WM_USER + 45;
        public const int TB_GETBUTTONTEXTW = WM_USER + 75;
        public const int TB_GETCOLORSCHEME = CCM_GETCOLORSCHEME;
        public const int TB_GETDISABLEDIMAGELIST = WM_USER + 55;
        public const int TB_GETEXTENDEDSTYLE = WM_USER + 85;
        public const int TB_GETHOTIMAGELIST = WM_USER + 53;
        public const int TB_GETHOTITEM = WM_USER + 71;
        public const int TB_GETIMAGELIST = WM_USER + 49;
        public const int TB_GETINSERTMARK = WM_USER + 79;
        public const int TB_GETINSERTMARKCOLOR = WM_USER + 89;
        public const int TB_GETITEMRECT = WM_USER + 29;
        public const int TB_GETMAXSIZE = WM_USER + 83;
        public const int TB_GETMETRICS = WM_USER + 101;
        public const int TB_GETOBJECT = WM_USER + 62;
        public const int TB_GETPADDING = WM_USER + 86;
        public const int TB_GETRECT = WM_USER + 51;
        public const int TB_GETROWS = WM_USER + 40;
        public const int TB_GETSTATE = WM_USER + 18;
        public const int TB_GETSTRINGA = WM_USER + 92;
        public const int TB_GETSTRINGW = WM_USER + 91;
        public const int TB_GETSTYLE = WM_USER + 57;
        public const int TB_GETTEXTROWS = WM_USER + 61;
        public const int TB_GETTOOLTIPS = WM_USER + 35;
        public const int TB_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int TB_HIDEBUTTON = WM_USER + 4;
        public const int TB_HITTEST = WM_USER + 69;
        public const int TB_INDETERMINATE = WM_USER + 5;
        public const int TB_INSERTBUTTON = WM_USER + 21;
        public const int TB_INSERTBUTTONA = WM_USER + 21;
        public const int TB_INSERTBUTTONW = WM_USER + 67;
        public const int TB_INSERTMARKHITTEST = WM_USER + 81;
        public const int TB_ISBUTTONCHECKED = WM_USER + 10;
        public const int TB_ISBUTTONENABLED = WM_USER + 9;
        public const int TB_ISBUTTONHIDDEN = WM_USER + 12;
        public const int TB_ISBUTTONHIGHLIGHTED = WM_USER + 14;
        public const int TB_ISBUTTONINDETERMINATE = WM_USER + 13;
        public const int TB_ISBUTTONPRESSED = WM_USER + 11;
        public const int TB_LOADIMAGES = WM_USER + 50;
        public const int TB_MAPACCELERATORA = WM_USER + 78;
        public const int TB_MAPACCELERATORW = WM_USER + 90;
        public const int TB_MARKBUTTON = WM_USER + 6;
        public const int TB_MOVEBUTTON = WM_USER + 82;
        public const int TB_PRESSBUTTON = WM_USER + 3;
        public const int TB_REPLACEBITMAP = WM_USER + 46;
        public const int TB_SAVERESTOREA = WM_USER + 26;
        public const int TB_SAVERESTOREW = WM_USER + 76;
        public const int TB_SETANCHORHIGHLIGHT = WM_USER + 73;
        public const int TB_SETBITMAPSIZE = WM_USER + 32;
        public const int TB_SETBUTTONINFOA = WM_USER + 66;
        public const int TB_SETBUTTONINFOW = WM_USER + 64;
        public const int TB_SETBUTTONSIZE = WM_USER + 31;
        public const int TB_SETBUTTONWIDTH = WM_USER + 59;
        public const int TB_SETCMDID = WM_USER + 42;
        public const int TB_SETCOLORSCHEME = CCM_SETCOLORSCHEME;
        public const int TB_SETDISABLEDIMAGELIST = WM_USER + 54;
        public const int TB_SETDRAWTEXTFLAGS = WM_USER + 70;
        public const int TB_SETEXTENDEDSTYLE = WM_USER + 84;
        public const int TB_SETHOTIMAGELIST = WM_USER + 52;
        public const int TB_SETHOTITEM = WM_USER + 72;
        public const int TB_SETIMAGELIST = WM_USER + 48;
        public const int TB_SETINDENT = WM_USER + 47;
        public const int TB_SETINSERTMARK = WM_USER + 80;
        public const int TB_SETINSERTMARKCOLOR = WM_USER + 88;
        public const int TB_SETMAXTEXTROWS = WM_USER + 60;
        public const int TB_SETMETRICS = WM_USER + 102;
        public const int TB_SETPADDING = WM_USER + 87;
        public const int TB_SETPARENT = WM_USER + 37;
        public const int TB_SETROWS = WM_USER + 39;
        public const int TB_SETSTATE = WM_USER + 17;
        public const int TB_SETSTYLE = WM_USER + 56;
        public const int TB_SETTOOLTIPS = WM_USER + 36;
        public const int TB_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int TB_SETWINDOWTHEME = CCM_SETWINDOWTHEME;
        public const int TBM_CLEARSEL = WM_USER + 19;
        public const int TBM_CLEARTICS = WM_USER + 9;
        public const int TBM_GETBUDDY = WM_USER + 33;
        public const int TBM_GETCHANNELRECT = WM_USER + 26;
        public const int TBM_GETLINESIZE = WM_USER + 24;
        public const int TBM_GETNUMTICS = WM_USER + 16;
        public const int TBM_GETPAGESIZE = WM_USER + 22;
        public const int TBM_GETPOS = WM_USER;
        public const int TBM_GETPTICS = WM_USER + 14;
        public const int TBM_GETRANGEMAX = WM_USER + 2;
        public const int TBM_GETRANGEMIN = WM_USER + 1;
        public const int TBM_GETSELEND = WM_USER + 18;
        public const int TBM_GETSELSTART = WM_USER + 17;
        public const int TBM_GETTHUMBLENGTH = WM_USER + 28;
        public const int TBM_GETTHUMBRECT = WM_USER + 25;
        public const int TBM_GETTIC = WM_USER + 3;
        public const int TBM_GETTICPOS = WM_USER + 15;
        public const int TBM_GETTOOLTIPS = WM_USER + 30;
        public const int TBM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int TBM_SETBUDDY = WM_USER + 32;
        public const int TBM_SETLINESIZE = WM_USER + 23;
        public const int TBM_SETPAGESIZE = WM_USER + 21;
        public const int TBM_SETPOS = WM_USER + 5;
        public const int TBM_SETRANGE = WM_USER + 6;
        public const int TBM_SETRANGEMAX = WM_USER + 8;
        public const int TBM_SETRANGEMIN = WM_USER + 7;
        public const int TBM_SETSEL = WM_USER + 10;
        public const int TBM_SETSELEND = WM_USER + 12;
        public const int TBM_SETSELSTART = WM_USER + 11;
        public const int TBM_SETTHUMBLENGTH = WM_USER + 27;
        public const int TBM_SETTIC = WM_USER + 4;
        public const int TBM_SETTICFREQ = WM_USER + 20;
        public const int TBM_SETTIPSIDE = WM_USER + 31;
        public const int TBM_SETTOOLTIPS = WM_USER + 29;
        public const int TBM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int TCM_ADJUSTRECT = TCM_FIRST + 40;
        public const int TCM_DELETEALLITEMS = TCM_FIRST + 9;
        public const int TCM_DELETEITEM = TCM_FIRST + 8;
        public const int TCM_DESELECTALL = TCM_FIRST + 50;
        public const int TCM_FIRST = 0x1300; // Tab control messages
        public const int TCM_GETCURFOCUS = TCM_FIRST + 47;
        public const int TCM_GETCURSEL = TCM_FIRST + 11;
        public const int TCM_GETEXTENDEDSTYLE = TCM_FIRST + 53;
        public const int TCM_GETIMAGELIST = TCM_FIRST + 2;
        public const int TCM_GETITEMA = TCM_FIRST + 5;
        public const int TCM_GETITEMCOUNT = TCM_FIRST + 4;
        public const int TCM_GETITEMRECT = TCM_FIRST + 10;
        public const int TCM_GETITEMW = TCM_FIRST + 60;
        public const int TCM_GETROWCOUNT = TCM_FIRST + 44;
        public const int TCM_GETTOOLTIPS = TCM_FIRST + 45;
        public const int TCM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int TCM_HIGHLIGHTITEM = TCM_FIRST + 51;
        public const int TCM_HITTEST = TCM_FIRST + 13;
        public const int TCM_INSERTITEMA = TCM_FIRST + 7;
        public const int TCM_INSERTITEMW = TCM_FIRST + 62;
        public const int TCM_REMOVEIMAGE = TCM_FIRST + 42;
        public const int TCM_SETCURFOCUS = TCM_FIRST + 48;
        public const int TCM_SETCURSEL = TCM_FIRST + 12;
        public const int TCM_SETEXTENDEDSTYLE = TCM_FIRST + 52;
        public const int TCM_SETIMAGELIST = TCM_FIRST + 3;
        public const int TCM_SETITEMA = TCM_FIRST + 6;
        public const int TCM_SETITEMEXTRA = TCM_FIRST + 14;
        public const int TCM_SETITEMSIZE = TCM_FIRST + 41;
        public const int TCM_SETITEMW = TCM_FIRST + 61;
        public const int TCM_SETMINTABWIDTH = TCM_FIRST + 49;
        public const int TCM_SETPADDING = TCM_FIRST + 43;
        public const int TCM_SETTOOLTIPS = TCM_FIRST + 46;
        public const int TCM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int TTM_ACTIVATE = WM_USER + 1;
        public const int TTM_ADDTOOLA = WM_USER + 4;
        public const int TTM_ADDTOOLW = WM_USER + 50;
        public const int TTM_ADJUSTRECT = WM_USER + 31;
        public const int TTM_DELTOOLA = WM_USER + 5;
        public const int TTM_DELTOOLW = WM_USER + 51;
        public const int TTM_ENUMTOOLSA = WM_USER + 14;
        public const int TTM_ENUMTOOLSW = WM_USER + 58;
        public const int TTM_GETBUBBLESIZE = WM_USER + 30;
        public const int TTM_GETCURRENTTOOLA = WM_USER + 15;
        public const int TTM_GETCURRENTTOOLW = WM_USER + 59;
        public const int TTM_GETDELAYTIME = WM_USER + 21;
        public const int TTM_GETMARGIN = WM_USER + 27;
        public const int TTM_GETMAXTIPWIDTH = WM_USER + 25;
        public const int TTM_GETTEXTA = WM_USER + 11;
        public const int TTM_GETTEXTW = WM_USER + 56;
        public const int TTM_GETTIPBKCOLOR = WM_USER + 22;
        public const int TTM_GETTIPTEXTCOLOR = WM_USER + 23;
        public const int TTM_GETTITLE = WM_USER + 35;
        public const int TTM_GETTOOLCOUNT = WM_USER + 13;
        public const int TTM_GETTOOLINFOA = WM_USER + 8;
        public const int TTM_GETTOOLINFOW = WM_USER + 53;
        public const int TTM_HITTESTA = WM_USER + 10;
        public const int TTM_HITTESTW = WM_USER + 55;
        public const int TTM_NEWTOOLRECTA = WM_USER + 6;
        public const int TTM_NEWTOOLRECTW = WM_USER + 52;
        public const int TTM_POP = WM_USER + 28;
        public const int TTM_POPUP = WM_USER + 34;
        public const int TTM_RELAYEVENT = WM_USER + 7;
        public const int TTM_SETDELAYTIME = WM_USER + 3;
        public const int TTM_SETMARGIN = WM_USER + 26;
        public const int TTM_SETMAXTIPWIDTH = WM_USER + 24;
        public const int TTM_SETTIPBKCOLOR = WM_USER + 19;
        public const int TTM_SETTIPTEXTCOLOR = WM_USER + 20;
        public const int TTM_SETTITLEA = WM_USER + 32;
        public const int TTM_SETTITLEW = WM_USER + 33;
        public const int TTM_SETTOOLINFOA = WM_USER + 9;
        public const int TTM_SETTOOLINFOW = WM_USER + 54;
        public const int TTM_SETWINDOWTHEME = CCM_SETWINDOWTHEME;
        public const int TTM_TRACKACTIVATE = WM_USER + 17;
        public const int TTM_TRACKPOSITION = WM_USER + 18;
        public const int TTM_UPDATE = WM_USER + 29;
        public const int TTM_UPDATETIPTEXTA = WM_USER + 12;
        public const int TTM_UPDATETIPTEXTW = WM_USER + 57;
        public const int TTM_WINDOWFROMPOINT = WM_USER + 16;
        public const int TV_FIRST = 0x1100; // TreeView messages
        public const int TVM_CREATEDRAGIMAGE = TV_FIRST + 18;
        public const int TVM_DELETEITEM = TV_FIRST + 1;
        public const int TVM_EDITLABELA = TV_FIRST + 14;
        public const int TVM_EDITLABELW = TV_FIRST + 65;
        public const int TVM_ENDEDITLABELNOW = TV_FIRST + 22;
        public const int TVM_ENSUREVISIBLE = TV_FIRST + 20;
        public const int TVM_EXPAND = TV_FIRST + 2;
        public const int TVM_GETBKCOLOR = TV_FIRST + 31;
        public const int TVM_GETCOUNT = TV_FIRST + 5;
        public const int TVM_GETEDITCONTROL = TV_FIRST + 15;
        public const int TVM_GETIMAGELIST = TV_FIRST + 8;
        public const int TVM_GETINDENT = TV_FIRST + 6;
        public const int TVM_GETINSERTMARKCOLOR = TV_FIRST + 38;
        public const int TVM_GETISEARCHSTRINGA = TV_FIRST + 23;
        public const int TVM_GETISEARCHSTRINGW = TV_FIRST + 64;
        public const int TVM_GETITEMA = TV_FIRST + 12;
        public const int TVM_GETITEMHEIGHT = TV_FIRST + 28;
        public const int TVM_GETITEMRECT = TV_FIRST + 4;
        public const int TVM_GETITEMSTATE = TV_FIRST + 39;
        public const int TVM_GETITEMW = TV_FIRST + 62;
        public const int TVM_GETLINECOLOR = TV_FIRST + 41;
        public const int TVM_GETNEXTITEM = TV_FIRST + 10;
        public const int TVM_GETSCROLLTIME = TV_FIRST + 34;
        public const int TVM_GETTEXTCOLOR = TV_FIRST + 32;
        public const int TVM_GETTOOLTIPS = TV_FIRST + 25;
        public const int TVM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int TVM_GETVISIBLECOUNT = TV_FIRST + 16;
        public const int TVM_HITTEST = TV_FIRST + 17;
        public const int TVM_INSERTITEMA = TV_FIRST + 0;
        public const int TVM_INSERTITEMW = TV_FIRST + 50;
        public const int TVM_MAPACCIDTOHTREEITEM = TV_FIRST + 42;
        public const int TVM_MAPHTREEITEMTOACCID = TV_FIRST + 43;
        public const int TVM_SELECTITEM = TV_FIRST + 11;
        public const int TVM_SETBKCOLOR = TV_FIRST + 29;
        public const int TVM_SETIMAGELIST = TV_FIRST + 9;
        public const int TVM_SETINDENT = TV_FIRST + 7;
        public const int TVM_SETINSERTMARK = TV_FIRST + 26;
        public const int TVM_SETINSERTMARKCOLOR = TV_FIRST + 37;
        public const int TVM_SETITEMA = TV_FIRST + 13;
        public const int TVM_SETITEMHEIGHT = TV_FIRST + 27;
        public const int TVM_SETITEMW = TV_FIRST + 63;
        public const int TVM_SETLINECOLOR = TV_FIRST + 40;
        public const int TVM_SETSCROLLTIME = TV_FIRST + 33;
        public const int TVM_SETTEXTCOLOR = TV_FIRST + 30;
        public const int TVM_SETTOOLTIPS = TV_FIRST + 24;
        public const int TVM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int TVM_SORTCHILDREN = TV_FIRST + 19;
        public const int TVM_SORTCHILDRENCB = TV_FIRST + 21;
        public const int UDM_GETACCEL = WM_USER + 108;
        public const int UDM_GETBASE = WM_USER + 110;
        public const int UDM_GETBUDDY = WM_USER + 106;
        public const int UDM_GETPOS = WM_USER + 104;
        public const int UDM_GETPOS32 = WM_USER + 114;
        public const int UDM_GETRANGE = WM_USER + 102;
        public const int UDM_GETRANGE32 = WM_USER + 112;
        public const int UDM_GETUNICODEFORMAT = CCM_GETUNICODEFORMAT;
        public const int UDM_SETACCEL = WM_USER + 107;
        public const int UDM_SETBASE = WM_USER + 109;
        public const int UDM_SETBUDDY = WM_USER + 105;
        public const int UDM_SETPOS = WM_USER + 103;
        public const int UDM_SETPOS32 = WM_USER + 113;
        public const int UDM_SETRANGE = WM_USER + 101;
        public const int UDM_SETRANGE32 = WM_USER + 111;
        public const int UDM_SETUNICODEFORMAT = CCM_SETUNICODEFORMAT;
        public const int UNICODE_NOCHAR = 0xFFFF;
        public const int WM_ACTIVATE = 0x0006;
        public const int WM_ACTIVATEAPP = 0x001C;
        public const int WM_AFXFIRST = 0x0360;
        public const int WM_AFXLAST = 0x037F;
        public const int WM_APP = 0x8000;
        public const int WM_APPCOMMAND = 0x0319;
        public const int WM_ASKCBFORMATNAME = 0x030C;
        public const int WM_CANCELJOURNAL = 0x004B;
        public const int WM_CANCELMODE = 0x001F;
        public const int WM_CAPTURECHANGED = 0x0215;
        public const int WM_CHANGECBCHAIN = 0x030D;
        public const int WM_CHANGEUISTATE = 0x0127;
        public const int WM_CHAR = 0x0102;
        public const int WM_CHARTOITEM = 0x002F;
        public const int WM_CHILDACTIVATE = 0x0022;
        public const int WM_CLEAR = 0x0303;
        public const int WM_CLOSE = 0x0010;
        public const int WM_COMMAND = 0x0111;
        public const int WM_COMMNOTIFY = 0x0044;
        public const int WM_COMPACTING = 0x0041;
        public const int WM_COMPAREITEM = 0x0039;
        public const int WM_CONTEXTMENU = 0x007B;
        public const int WM_COPY = 0x0301;
        public const int WM_COPYDATA = 0x004A;
        public const int WM_CREATE = 0x0001;
        public const int WM_CTLCOLORBTN = 0x0135;
        public const int WM_CTLCOLORDLG = 0x0136;
        public const int WM_CTLCOLOREDIT = 0x0133;
        public const int WM_CTLCOLORLISTBOX = 0x0134;
        public const int WM_CTLCOLORMSGBOX = 0x0132;
        public const int WM_CTLCOLORSCROLLBAR = 0x0137;
        public const int WM_CTLCOLORSTATIC = 0x0138;
        public const int WM_CUT = 0x0300;
        public const int WM_DEADCHAR = 0x0103;
        public const int WM_DELETEITEM = 0x002D;
        public const int WM_DESTROY = 0x0002;
        public const int WM_DESTROYCLIPBOARD = 0x0307;
        public const int WM_DEVICECHANGE = 0x0219;
        public const int WM_DEVMODECHANGE = 0x001B;
        public const int WM_DISPLAYCHANGE = 0x007E;
        public const int WM_DRAWCLIPBOARD = 0x0308;
        public const int WM_DRAWITEM = 0x002B;
        public const int WM_DROPFILES = 0x0233;
        public const int WM_ENABLE = 0x000A;
        public const int WM_ENDSESSION = 0x0016;
        public const int WM_ENTERIDLE = 0x0121;
        public const int WM_ENTERMENULOOP = 0x0211;
        public const int WM_ENTERSIZEMOVE = 0x0231;
        public const int WM_ERASEBKGND = 0x0014;
        public const int WM_EXITMENULOOP = 0x0212;
        public const int WM_EXITSIZEMOVE = 0x0232;
        public const int WM_FONTCHANGE = 0x001D;
        public const int WM_GETDLGCODE = 0x0087;
        public const int WM_GETFONT = 0x0031;
        public const int WM_GETHOTKEY = 0x0033;
        public const int WM_GETICON = 0x007F;
        public const int WM_GETMINMAXINFO = 0x0024;
        public const int WM_GETOBJECT = 0x003D;
        public const int WM_GETTEXT = 0x000D;
        public const int WM_GETTEXTLENGTH = 0x000E;
        public const int WM_HANDHELDFIRST = 0x0358;
        public const int WM_HANDHELDLAST = 0x035F;
        public const int WM_HELP = 0x0053;
        public const int WM_HOTKEY = 0x0312;
        public const int WM_HSCROLL = 0x0114;
        public const int WM_HSCROLLCLIPBOARD = 0x030E;
        public const int WM_ICONERASEBKGND = 0x0027;
        public const int WM_IME_CHAR = 0x0286;
        public const int WM_IME_COMPOSITION = 0x010F;
        public const int WM_IME_COMPOSITIONFULL = 0x0284;
        public const int WM_IME_CONTROL = 0x0283;
        public const int WM_IME_ENDCOMPOSITION = 0x010E;
        public const int WM_IME_KEYDOWN = 0x0290;
        public const int WM_IME_KEYLAST = 0x010F;
        public const int WM_IME_KEYUP = 0x0291;
        public const int WM_IME_NOTIFY = 0x0282;
        public const int WM_IME_REQUEST = 0x0288;
        public const int WM_IME_SELECT = 0x0285;
        public const int WM_IME_SETCONTEXT = 0x0281;
        public const int WM_IME_STARTCOMPOSITION = 0x010D;
        public const int WM_INITDIALOG = 0x0110;
        public const int WM_INITMENU = 0x0116;
        public const int WM_INITMENUPOPUP = 0x0117;
        public const int WM_INPUT = 0x00FF;
        public const int WM_INPUTLANGCHANGE = 0x0051;
        public const int WM_INPUTLANGCHANGEREQUEST = 0x0050;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYFIRST = 0x0100;
        public const int WM_KEYLAST_NT501 = 0x0109;
        public const int WM_KEYLAST_PRE501 = 0x0108;
        public const int WM_KEYUP = 0x0101;
        public const int WM_KILLFOCUS = 0x0008;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_MBUTTONDBLCLK = 0x0209;
        public const int WM_MBUTTONDOWN = 0x0207;
        public const int WM_MBUTTONUP = 0x0208;
        public const int WM_MDIACTIVATE = 0x0222;
        public const int WM_MDICASCADE = 0x0227;
        public const int WM_MDICREATE = 0x0220;
        public const int WM_MDIDESTROY = 0x0221;
        public const int WM_MDIGETACTIVE = 0x0229;
        public const int WM_MDIICONARRANGE = 0x0228;
        public const int WM_MDIMAXIMIZE = 0x0225;
        public const int WM_MDINEXT = 0x0224;
        public const int WM_MDIREFRESHMENU = 0x0234;
        public const int WM_MDIRESTORE = 0x0223;
        public const int WM_MDISETMENU = 0x0230;
        public const int WM_MDITILE = 0x0226;
        public const int WM_MEASUREITEM = 0x002C;
        public const int WM_MENUCHAR = 0x0120;
        public const int WM_MENUCOMMAND = 0x0126;
        public const int WM_MENUDRAG = 0x0123;
        public const int WM_MENUGETOBJECT = 0x0124;
        public const int WM_MENURBUTTONUP = 0x0122;
        public const int WM_MENUSELECT = 0x011F;
        public const int WM_MOUSEACTIVATE = 0x0021;
        public const int WM_MOUSEFIRST = 0x0200;
        public const int WM_MOUSEHOVER = 0x02A1;
        public const int WM_MOUSELAST_4 = 0x020A;
        public const int WM_MOUSELAST_5 = 0x020D;
        public const int WM_MOUSELAST_PRE_4 = 0x0209;
        public const int WM_MOUSELEAVE = 0x02A3;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_MOUSEWHEEL = 0x020A;
        public const int WM_MOVE = 0x0003;
        public const int WM_MOVING = 0x0216;
        public const int WM_NCACTIVATE = 0x0086;
        public const int WM_NCCALCSIZE = 0x0083;
        public const int WM_NCCREATE = 0x0081;
        public const int WM_NCDESTROY = 0x0082;
        public const int WM_NCHITTEST = 0x0084;
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCLBUTTONUP = 0x00A2;
        public const int WM_NCMBUTTONDBLCLK = 0x00A9;
        public const int WM_NCMBUTTONDOWN = 0x00A7;
        public const int WM_NCMBUTTONUP = 0x00A8;
        public const int WM_NCMOUSEHOVER = 0x02A0;
        public const int WM_NCMOUSELEAVE = 0x02A2;
        public const int WM_NCMOUSEMOVE = 0x00A0;
        public const int WM_NCPAINT = 0x0085;
        public const int WM_NCRBUTTONDBLCLK = 0x00A6;
        public const int WM_NCRBUTTONDOWN = 0x00A4;
        public const int WM_NCRBUTTONUP = 0x00A5;
        public const int WM_NCXBUTTONDBLCLK = 0x00AD;
        public const int WM_NCXBUTTONDOWN = 0x00AB;
        public const int WM_NCXBUTTONUP = 0x00AC;
        public const int WM_NEXTDLGCTL = 0x0028;
        public const int WM_NEXTMENU = 0x0213;
        public const int WM_NOTIFY = 0x004E;
        public const int WM_NOTIFYFORMAT = 0x0055;
        public const int WM_NULL = 0x0000;
        public const int WM_PAINT = 0x000F;
        public const int WM_PAINTCLIPBOARD = 0x0309;
        public const int WM_PAINTICON = 0x0026;
        public const int WM_PALETTECHANGED = 0x0311;
        public const int WM_PALETTEISCHANGING = 0x0310;
        public const int WM_PARENTNOTIFY = 0x0210;
        public const int WM_PASTE = 0x0302;
        public const int WM_PENWINFIRST = 0x0380;
        public const int WM_PENWINLAST = 0x038F;
        public const int WM_POWER = 0x0048;
        public const int WM_POWERBROADCAST = 0x0218;
        public const int WM_PRINT = 0x0317;
        public const int WM_PRINTCLIENT = 0x0318;
        public const int WM_QUERYDRAGICON = 0x0037;
        public const int WM_QUERYENDSESSION = 0x0011;
        public const int WM_QUERYNEWPALETTE = 0x030F;
        public const int WM_QUERYOPEN = 0x0013;
        public const int WM_QUERYUISTATE = 0x0129;
        public const int WM_QUEUESYNC = 0x0023;
        public const int WM_QUIT = 0x0012;
        public const int WM_RBUTTONDBLCLK = 0x0206;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;
        public const int WM_RENDERALLFORMATS = 0x0306;
        public const int WM_RENDERFORMAT = 0x0305;
        public const int WM_SETCURSOR = 0x0020;
        public const int WM_SETFOCUS = 0x0007;
        public const int WM_SETFONT = 0x0030;
        public const int WM_SETHOTKEY = 0x0032;
        public const int WM_SETICON = 0x0080;
        public const int WM_SETREDRAW = 0x000B;
        public const int WM_SETTEXT = 0x000C;
        public const int WM_SETTINGCHANGE = 0x001A;
        public const int WM_SHOWWINDOW = 0x0018;
        public const int WM_SIZE = 0x0005;
        public const int WM_SIZECLIPBOARD = 0x030B;
        public const int WM_SIZING = 0x0214;
        public const int WM_SPOOLERSTATUS = 0x002A;
        public const int WM_STYLECHANGED = 0x007D;
        public const int WM_STYLECHANGING = 0x007C;
        public const int WM_SYNCPAINT = 0x0088;
        public const int WM_SYSCHAR = 0x0106;
        public const int WM_SYSCOLORCHANGE = 0x0015;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int WM_SYSDEADCHAR = 0x0107;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        public const int WM_TABLET_FIRST = 0x02c0;
        public const int WM_TABLET_LAST = 0x02df;
        public const int WM_TCARD = 0x0052;
        public const int WM_THEMECHANGED = 0x031A;
        public const int WM_TIMECHANGE = 0x001E;
        public const int WM_TIMER = 0x0113;
        public const int WM_UNDO = 0x0304;
        public const int WM_UNICHAR = 0x0109;
        public const int WM_UNINITMENUPOPUP = 0x0125;
        public const int WM_UPDATEUISTATE = 0x0128;
        public const int WM_USER = 0x0400;
        public const int WM_USERCHANGED = 0x0054;
        public const int WM_VKEYTOITEM = 0x002E;
        public const int WM_VSCROLL = 0x0115;
        public const int WM_VSCROLLCLIPBOARD = 0x030A;
        public const int WM_WINDOWPOSCHANGED = 0x0047;
        public const int WM_WINDOWPOSCHANGING = 0x0046;
        public const int WM_WININICHANGE = 0x001A;
        public const int WM_WTSSESSION_CHANGE = 0x02B1;
        public const int WM_XBUTTONDBLCLK = 0x020D;
        public const int WM_XBUTTONDOWN = 0x020B;
        public const int WM_XBUTTONUP = 0x020C;
        public const int WM_DWMCOMPOSITIONCHANGED = 0x031E;
        public const int SC_MONITORPOWER = 0xF170;
    }

    public sealed class Hresults
    {
        public const int DV_E_LINDEX = unchecked((int) 0x80040068);
        public const int E_ABORT = unchecked((int) 0x80004004);
        public const int E_ACCESSDENIED = unchecked((int) 0x80070005);
        public const int E_FAIL = unchecked((int) 0x80004005);
        public const int E_FLAGS = 0x1000;
        public const int E_HANDLE = unchecked((int) 0x80070006);
        public const int E_INVALIDARG = unchecked((int) 0x80070057);
        public const int E_NOINTERFACE = unchecked((int) 0x80004002);
        public const int E_NOTIMPL = unchecked((int) 0x80004001);
        public const int E_OUTOFMEMORY = unchecked((int) 0x8007000E);
        public const int E_PENDING = unchecked((int) 0x8000000A);
        public const int E_POINTER = unchecked((int) 0x80004003);
        public const int E_UNEXPECTED = unchecked((int) 0x8000FFFF);
        //Wininet
        public const int ERROR_ACCESS_DENIED = 5;
        public const int ERROR_FILE_NOT_FOUND = 2;
        public const int ERROR_INSUFFICIENT_BUFFER = 122;
        public const int ERROR_INVALID_PARAMETER = 87;
        public const int ERROR_NO_MORE_ITEMS = 259;
        public const int ERROR_NO_TOKEN = 1008;
        public const int ERROR_SUCCESS = 0;
        public const int NOERROR = 0;
        public const int OLE_E_ADVF = unchecked((int) 0x80040001);
        public const int OLE_E_ADVISENOTSUPPORTED = unchecked((int) 0x80040003);
        public const int OLE_E_BLANK = unchecked((int) 0x80040007);
        public const int OLE_E_CANT_BINDTOSOURCE = unchecked((int) 0x8004000A);
        public const int OLE_E_CANT_GETMONIKER = unchecked((int) 0x80040009);
        public const int OLE_E_CANTCONVERT = unchecked((int) 0x80040011);
        public const int OLE_E_CLASSDIFF = unchecked((int) 0x80040008);
        public const int OLE_E_ENUM_NOMORE = unchecked((int) 0x80040002);
        //Ole Errors
        public const int OLE_E_FIRST = unchecked((int) 0x80040000);
        public const int OLE_E_INVALIDHWND = unchecked((int) 0x8004000F);
        public const int OLE_E_INVALIDRECT = unchecked((int) 0x8004000D);
        public const int OLE_E_LAST = unchecked((int) 0x800400FF);
        public const int OLE_E_NOCACHE = unchecked((int) 0x80040006);
        public const int OLE_E_NOCONNECTION = unchecked((int) 0x80040004);
        public const int OLE_E_NOSTORAGE = unchecked((int) 0x80040012);
        public const int OLE_E_NOT_INPLACEACTIVE = unchecked((int) 0x80040010);
        public const int OLE_E_NOTRUNNING = unchecked((int) 0x80040005);
        public const int OLE_E_OLEVERB = unchecked((int) 0x80040000);
        public const int OLE_E_PROMPTSAVECANCELLED = unchecked((int) 0x8004000C);
        public const int OLE_E_STATIC = unchecked((int) 0x8004000B);
        public const int OLE_E_WRONGCOMPOBJ = unchecked((int) 0x8004000E);
        public const int OLE_S_FIRST = 0x00040000;
        public const int OLE_S_LAST = 0x000400FF;
        public const int OLECMDERR_E_CANCELED = unchecked(OLECMDERR_E_FIRST + 3);
        //OLECMDERR_E_FIRST = 0x80040100
        public const int OLECMDERR_E_DISABLED = unchecked(OLECMDERR_E_FIRST + 1);
        public const int OLECMDERR_E_FIRST = unchecked(OLE_E_LAST + 1);
        public const int OLECMDERR_E_NOHELP = unchecked(OLECMDERR_E_FIRST + 2);
        public const int OLECMDERR_E_NOTSUPPORTED = OLECMDERR_E_FIRST;
        public const int OLECMDERR_E_UNKNOWNGROUP = unchecked(OLECMDERR_E_FIRST + 4);
        public const int OLEOBJ_E_NOVERBS = unchecked((int) 0x80040180);
        public const int OLEOBJ_S_CANNOT_DOVERB_NOW = 0x00040181;
        public const int OLEOBJ_S_INVALIDHWND = 0x00040182;
        public const int OLEOBJ_S_INVALIDVERB = 0x00040180;
        public const int RPC_E_RETRY = unchecked((int) 0x80010109);
        public const int S_FALSE = 1;
        public const int S_OK = 0;
    }

    /// <summary>
    ///     Dispids constants taken from MsHtmdid.h
    /// </summary>
    public sealed class HTMLDispIDs
    {
        //useful DISPIDs

        public const int DISPID_A_ACCELERATOR = DISPID_A_FIRST + 147;
        public const int DISPID_A_ALLOWTRANSPARENCY = DISPID_A_FIRST + 206;
        public const int DISPID_A_BACKGROUNDATTACHMENT = DISPID_A_FIRST + 45;
        public const int DISPID_A_BACKGROUNDIMAGE = DISPID_A_FIRST + 1;
        public const int DISPID_A_BACKGROUNDPOSITION = DISPID_A_FIRST + 46;
        public const int DISPID_A_BACKGROUNDPOSX = DISPID_A_FIRST + 33;
        public const int DISPID_A_BACKGROUNDPOSY = DISPID_A_FIRST + 34;
        public const int DISPID_A_BACKGROUNDREPEAT = DISPID_A_FIRST + 44;
        public const int DISPID_A_BASEFONT = DISPID_A_FIRST + 26;
        public const int DISPID_A_BEHAVIOR = DISPID_A_FIRST + 115; // xtags
        public const int DISPID_A_BORDER = DISPID_A_FIRST + 49;
        public const int DISPID_A_BORDERBOTTOM = DISPID_A_FIRST + 52;
        public const int DISPID_A_BORDERBOTTOMCOLOR = DISPID_A_FIRST + 57;
        public const int DISPID_A_BORDERBOTTOMSTYLE = DISPID_A_FIRST + 67;
        public const int DISPID_A_BORDERBOTTOMWIDTH = DISPID_A_FIRST + 62;
        public const int DISPID_A_BORDERCOLLAPSE = DISPID_A_FIRST + 84;
        public const int DISPID_A_BORDERCOLOR = DISPID_A_FIRST + 54;
        public const int DISPID_A_BORDERLEFT = DISPID_A_FIRST + 53;
        public const int DISPID_A_BORDERLEFTCOLOR = DISPID_A_FIRST + 58;
        public const int DISPID_A_BORDERLEFTSTYLE = DISPID_A_FIRST + 68;
        public const int DISPID_A_BORDERLEFTWIDTH = DISPID_A_FIRST + 63;
        public const int DISPID_A_BORDERRIGHT = DISPID_A_FIRST + 51;
        public const int DISPID_A_BORDERRIGHTCOLOR = DISPID_A_FIRST + 56;
        public const int DISPID_A_BORDERRIGHTSTYLE = DISPID_A_FIRST + 66;
        public const int DISPID_A_BORDERRIGHTWIDTH = DISPID_A_FIRST + 61;
        public const int DISPID_A_BORDERSTYLE = DISPID_A_FIRST + 64;
        public const int DISPID_A_BORDERTOP = DISPID_A_FIRST + 50;
        public const int DISPID_A_BORDERTOPCOLOR = DISPID_A_FIRST + 55;
        public const int DISPID_A_BORDERTOPSTYLE = DISPID_A_FIRST + 65;
        public const int DISPID_A_BORDERTOPWIDTH = DISPID_A_FIRST + 60;
        public const int DISPID_A_BORDERWIDTH = DISPID_A_FIRST + 59;
        public const int DISPID_A_CLEAR = DISPID_A_FIRST + 16;
        public const int DISPID_A_CLIP = DISPID_A_FIRST + 92;
        public const int DISPID_A_CLIPRECTBOTTOM = DISPID_A_FIRST + 95;
        public const int DISPID_A_CLIPRECTLEFT = DISPID_A_FIRST + 96;
        public const int DISPID_A_CLIPRECTRIGHT = DISPID_A_FIRST + 94;
        public const int DISPID_A_CLIPRECTTOP = DISPID_A_FIRST + 93;
        public const int DISPID_A_COLOR = DISPID_A_FIRST + 2;
        public const int DISPID_A_CURSOR = DISPID_A_FIRST + 102;
        public const int DISPID_A_DEFAULTTEXTSELECTION = DISPID_A_FIRST + 188;
        public const int DISPID_A_DIR = DISPID_A_FIRST + 117;
        public const int DISPID_A_DIRECTION = DISPID_A_FIRST + 119; // Complex Text support for CSS2 direction
        public const int DISPID_A_DISPLAY = DISPID_A_FIRST + 71;
        public const int DISPID_A_DOCFRAGMENT = DISPID_A_FIRST + 142;
        public const int DISPID_A_EDITABLE = DISPID_A_FIRST + 162;
        public const int DISPID_A_EXTENDEDTAGDESC = DISPID_A_FIRST + 151;
        public const int DISPID_A_FILTER = DISPID_A_FIRST + 82;
        public const int DISPID_A_FIRST = DISPID_ATTRS;
        public const int DISPID_A_FLOAT = DISPID_A_FIRST + 70;
        public const int DISPID_A_FONTFACE = DISPID_A_FIRST + 18;
        public const int DISPID_A_FONTFACESRC = DISPID_A_FIRST + 97;
        public const int DISPID_A_FONTSIZE = DISPID_A_FIRST + 19;
        public const int DISPID_A_FONTSTYLE = DISPID_A_FIRST + 24;
        public const int DISPID_A_FONTVARIANT = DISPID_A_FIRST + 25;
        public const int DISPID_A_FONTWEIGHT = DISPID_A_FIRST + 27;
        public const int DISPID_A_HASLAYOUT = DISPID_A_FIRST + 160;
        public const int DISPID_A_HIDEFOCUS = DISPID_A_FIRST + 163;
        public const int DISPID_A_HTCDD_CREATEEVENTOBJECT = DISPID_A_FIRST + 144;
        public const int DISPID_A_HTCDD_ELEMENT = DISPID_A_FIRST + 143;
        public const int DISPID_A_HTCDD_ISMARKUPSHARED = DISPID_A_FIRST + 157;
        public const int DISPID_A_HTCDD_PROTECTEDELEMENT = DISPID_A_FIRST + 154;
        public const int DISPID_A_HTCDISPATCHITEM_VALUE = DISPID_A_FIRST + 141;
        public const int DISPID_A_HTCDISPATCHITEM_VALUE_SCRIPTSONLY = DISPID_A_FIRST + 150;
        public const int DISPID_A_IMEMODE = DISPID_A_FIRST + 120;
        public const int DISPID_A_ISBLOCK = DISPID_A_FIRST + 208;
        public const int DISPID_A_LANG = DISPID_A_FIRST + 9;
        public const int DISPID_A_LANGUAGE = DISPID_A_FIRST + 100;
        public const int DISPID_A_LAYOUTFLOW = DISPID_A_FIRST + 155;
        public const int DISPID_A_LAYOUTGRID = DISPID_A_FIRST + 131;
        public const int DISPID_A_LAYOUTGRIDCHAR = DISPID_A_FIRST + 127;
        public const int DISPID_A_LAYOUTGRIDLINE = DISPID_A_FIRST + 128;
        public const int DISPID_A_LAYOUTGRIDMODE = DISPID_A_FIRST + 129;
        public const int DISPID_A_LAYOUTGRIDTYPE = DISPID_A_FIRST + 130;
        public const int DISPID_A_LETTERSPACING = DISPID_A_FIRST + 8;
        public const int DISPID_A_LINEBREAK = DISPID_A_FIRST + 133;
        public const int DISPID_A_LINEHEIGHT = DISPID_A_FIRST + 6;
        public const int DISPID_A_LISTSTYLE = DISPID_A_FIRST + 75;
        public const int DISPID_A_LISTSTYLEIMAGE = DISPID_A_FIRST + 74;
        public const int DISPID_A_LISTSTYLEPOSITION = DISPID_A_FIRST + 73;
        public const int DISPID_A_LISTSTYLETYPE = DISPID_A_FIRST + 72;
        public const int DISPID_A_LISTTYPE = DISPID_A_FIRST + 17;
        public const int DISPID_A_MARGIN = DISPID_A_FIRST + 36;
        public const int DISPID_A_MARGINBOTTOM = DISPID_A_FIRST + 39;
        public const int DISPID_A_MARGINLEFT = DISPID_A_FIRST + 40;
        public const int DISPID_A_MARGINRIGHT = DISPID_A_FIRST + 38;
        public const int DISPID_A_MARGINTOP = DISPID_A_FIRST + 37;
        public const int DISPID_A_MEDIA = DISPID_A_FIRST + 161;
        public const int DISPID_A_MINHEIGHT = DISPID_A_FIRST + 211;
        public const int DISPID_A_NOWRAP = DISPID_A_FIRST + 5;
        public const int DISPID_A_OVERFLOW = DISPID_A_FIRST + 10;
        public const int DISPID_A_OVERFLOWX = DISPID_A_FIRST + 139;
        public const int DISPID_A_OVERFLOWY = DISPID_A_FIRST + 140;
        public const int DISPID_A_PADDING = DISPID_A_FIRST + 11;
        public const int DISPID_A_PADDINGBOTTOM = DISPID_A_FIRST + 14;
        public const int DISPID_A_PADDINGLEFT = DISPID_A_FIRST + 15;
        public const int DISPID_A_PADDINGRIGHT = DISPID_A_FIRST + 13;
        public const int DISPID_A_PADDINGTOP = DISPID_A_FIRST + 12;
        public const int DISPID_A_PAGEBREAKAFTER = DISPID_A_FIRST + 78;
        public const int DISPID_A_PAGEBREAKBEFORE = DISPID_A_FIRST + 77;
        public const int DISPID_A_POSITION = DISPID_A_FIRST + 90;
        public const int DISPID_A_READYSTATE = DISPID_A_FIRST + 116; // ready state
        public const int DISPID_A_RENDERINGPRIORITY = DISPID_A_FIRST + 170;
        public const int DISPID_A_ROTATE = DISPID_A_FIRST + 152;
        public const int DISPID_A_RUBYALIGN = DISPID_A_FIRST + 121;
        public const int DISPID_A_RUBYOVERHANG = DISPID_A_FIRST + 123;
        public const int DISPID_A_RUBYPOSITION = DISPID_A_FIRST + 122;
        public const int DISPID_A_SCROLL = DISPID_A_FIRST + 79;
        public const int DISPID_A_SCROLLBAR3DLIGHTCOLOR = DISPID_A_FIRST + 182;
        public const int DISPID_A_SCROLLBARARROWCOLOR = DISPID_A_FIRST + 186;
        public const int DISPID_A_SCROLLBARBASECOLOR = DISPID_A_FIRST + 180;
        public const int DISPID_A_SCROLLBARDARKSHADOWCOLOR = DISPID_A_FIRST + 185;
        public const int DISPID_A_SCROLLBARFACECOLOR = DISPID_A_FIRST + 181;
        public const int DISPID_A_SCROLLBARHIGHLIGHTCOLOR = DISPID_A_FIRST + 184;
        public const int DISPID_A_SCROLLBARSHADOWCOLOR = DISPID_A_FIRST + 183;
        public const int DISPID_A_SCROLLBARTRACKCOLOR = DISPID_A_FIRST + 196;
        public const int DISPID_A_STYLETEXTDECORATION = DISPID_A_FIRST + 191;
        public const int DISPID_A_TABLEBORDERCOLOR = DISPID_A_FIRST + 28;
        public const int DISPID_A_TABLEBORDERCOLORDARK = DISPID_A_FIRST + 30;
        public const int DISPID_A_TABLEBORDERCOLORLIGHT = DISPID_A_FIRST + 29;
        public const int DISPID_A_TABLELAYOUT = DISPID_A_FIRST + 98;
        public const int DISPID_A_TABLEVALIGN = DISPID_A_FIRST + 31;
        public const int DISPID_A_TEXTALIGNLAST = DISPID_A_FIRST + 203;
        public const int DISPID_A_TEXTAUTOSPACE = DISPID_A_FIRST + 132;
        public const int DISPID_A_TEXTBACKGROUNDCOLOR = DISPID_A_FIRST + 169;
        public const int DISPID_A_TEXTCOLOR = DISPID_A_FIRST + 190;
        public const int DISPID_A_TEXTDECORATION = DISPID_A_FIRST + 35;
        public const int DISPID_A_TEXTDECORATIONCOLOR = DISPID_A_FIRST + 189;
        public const int DISPID_A_TEXTEFFECT = DISPID_A_FIRST + 168;
        public const int DISPID_A_TEXTINDENT = DISPID_A_FIRST + 7;
        public const int DISPID_A_TEXTJUSTIFY = DISPID_A_FIRST + 135;
        public const int DISPID_A_TEXTJUSTIFYTRIM = DISPID_A_FIRST + 136;
        public const int DISPID_A_TEXTKASHIDA = DISPID_A_FIRST + 137;
        public const int DISPID_A_TEXTKASHIDASPACE = DISPID_A_FIRST + 204;
        public const int DISPID_A_TEXTLINETHROUGHSTYLE = DISPID_A_FIRST + 166;
        public const int DISPID_A_TEXTOVERFLOW = DISPID_A_FIRST + 209;
        public const int DISPID_A_TEXTTRANSFORM = DISPID_A_FIRST + 4;
        public const int DISPID_A_TEXTUNDERLINEPOSITION = DISPID_A_FIRST + 159;
        public const int DISPID_A_TEXTUNDERLINESTYLE = DISPID_A_FIRST + 167;
        public const int DISPID_A_UNICODEBIDI = DISPID_A_FIRST + 118; // Complex Text support for CSS2 unicode-bidi
        public const int DISPID_A_UNIQUEPEERNUMBER = DISPID_A_FIRST + 146;
        public const int DISPID_A_URNATOM = DISPID_A_FIRST + 145;
        public const int DISPID_A_VALUE = DISPID_A_FIRST + 101;
        public const int DISPID_A_VERTICALALIGN = DISPID_A_FIRST + 48;
        public const int DISPID_A_VISIBILITY = DISPID_A_FIRST + 80;
        public const int DISPID_A_WHITESPACE = DISPID_A_FIRST + 76;
        public const int DISPID_A_WORDBREAK = DISPID_A_FIRST + 134;
        public const int DISPID_A_WORDSPACING = DISPID_A_FIRST + 47;
        public const int DISPID_A_WORDWRAP = DISPID_A_FIRST + 158;
        public const int DISPID_A_WRITINGMODE = DISPID_A_FIRST + 192;
        public const int DISPID_A_ZINDEX = DISPID_A_FIRST + 91;
        public const int DISPID_A_ZOOM = DISPID_A_FIRST + 153;
        public const int DISPID_ABOUTBOX = -552;
        public const int DISPID_ACCELERATOR = -543;
        public const int DISPID_ADDITEM = -553;
        public const int DISPID_AMBIENT_CHARSET = -727;
        public const int DISPID_AMBIENT_CODEPAGE = -725;
        public const int DISPID_AMBIENT_DLCONTROL = -5512;
        public const int DISPID_ANCHOR = DISPID_NORMAL_FIRST;
        public const int DISPID_APPEARANCE = -520;
        public const int DISPID_AREA = DISPID_NORMAL_FIRST;
        public const int DISPID_ATTRS = DISPID_STYLE + 1000;
        //  Standard dispatch ID constants

        public const int DISPID_AUTOSIZE = -500;
        public const int DISPID_BACKCOLOR = -501;
        public const int DISPID_BACKSTYLE = -502;
        public const int DISPID_BODY = DISPID_TEXTSITE + 1000;
        public const int DISPID_BORDERCOLOR = -503;
        public const int DISPID_BORDERSTYLE = -504;
        public const int DISPID_BORDERVISIBLE = -519;
        public const int DISPID_BORDERWIDTH = -505;
        public const int DISPID_BUTTON = DISPID_RICHTEXT + 1000;
        public const int DISPID_CAPTION = -518;
        public const int DISPID_CLEAR = -554;
        public const int DISPID_CLICK = -600;
        public const int DISPID_CLICK_VALUE = -610;
        public const int DISPID_COLLECTION = DISPID_NORMAL_FIRST + 500;
        public const int DISPID_COLLECTION_MAX = 2999999;
        public const int DISPID_COLLECTION_MIN = 1000000;
        public const int DISPID_COLUMN = -529;
        private const int DISPID_COMMENTPDL = DISPID_NORMAL_FIRST;
        public const int DISPID_DBLCLICK = -601;
        public const int DISPID_DEFAULTVALUE = DISPID_A_FIRST + 83;
        public const int DISPID_DISPLAYSTYLE = -540;
        public const int DISPID_DOCLICK = -551;
        public const int DISPID_DOMATTRIBUTE = DISPID_NORMAL_FIRST;
        public const int DISPID_DOMTEXTNODE = DISPID_NORMAL_FIRST;
        public const int DISPID_DRAWMODE = -507;
        public const int DISPID_DRAWSTYLE = -508;
        public const int DISPID_DRAWWIDTH = -509;
        public const int DISPID_ELEMENT = DISPID_HTMLOBJECT + 500;
        public const int DISPID_ENABLED = -514;
        public const int DISPID_ENTERKEYBEHAVIOR = -544;
        public const int DISPID_ERROREVENT = -608;
        public const int DISPID_EVENTOBJ = DISPID_NORMAL_FIRST;
        public const int DISPID_EVENTS = DISPID_ATTRS + 1000;
        public const int DISPID_EVMETH_ONABORT = DISPID_ONABORT;
        public const int DISPID_EVMETH_ONACTIVATE = DISPID_ONACTIVATE;
        public const int DISPID_EVMETH_ONAFTERPRINT = DISPID_ONAFTERPRINT;
        public const int DISPID_EVMETH_ONAFTERUPDATE = STDDISPID_XOBJ_AFTERUPDATE;
        public const int DISPID_EVMETH_ONBEFOREACTIVATE = DISPID_ONBEFOREACTIVATE;
        public const int DISPID_EVMETH_ONBEFORECOPY = STDDISPID_XOBJ_ONBEFORECOPY;
        public const int DISPID_EVMETH_ONBEFORECUT = STDDISPID_XOBJ_ONBEFORECUT;
        public const int DISPID_EVMETH_ONBEFOREDEACTIVATE = DISPID_ONBEFOREDEACTIVATE;
        public const int DISPID_EVMETH_ONBEFOREEDITFOCUS = DISPID_ONBEFOREEDITFOCUS;
        public const int DISPID_EVMETH_ONBEFOREPASTE = STDDISPID_XOBJ_ONBEFOREPASTE;
        public const int DISPID_EVMETH_ONBEFOREPRINT = DISPID_ONBEFOREPRINT;
        public const int DISPID_EVMETH_ONBEFOREUNLOAD = DISPID_ONBEFOREUNLOAD;
        public const int DISPID_EVMETH_ONBEFOREUPDATE = STDDISPID_XOBJ_BEFOREUPDATE;
        public const int DISPID_EVMETH_ONBLUR = STDDISPID_XOBJ_ONBLUR;
        public const int DISPID_EVMETH_ONBOUNCE = DISPID_ONBOUNCE;
        public const int DISPID_EVMETH_ONCELLCHANGE = STDDISPID_XOBJ_ONCELLCHANGE;
        public const int DISPID_EVMETH_ONCHANGE = DISPID_ONCHANGE;
        public const int DISPID_EVMETH_ONCHANGEBLUR = DISPID_ONCHANGEBLUR;
        public const int DISPID_EVMETH_ONCHANGEFOCUS = DISPID_ONCHANGEFOCUS;
        public const int DISPID_EVMETH_ONCLICK = DISPID_CLICK;
        public const int DISPID_EVMETH_ONCONTENTREADY = DISPID_ONCONTENTREADY;
        public const int DISPID_EVMETH_ONCONTEXTMENU = DISPID_ONCONTEXTMENU;
        public const int DISPID_EVMETH_ONCONTROLSELECT = DISPID_ONCONTROLSELECT;
        public const int DISPID_EVMETH_ONCOPY = STDDISPID_XOBJ_ONCOPY;
        public const int DISPID_EVMETH_ONCUT = STDDISPID_XOBJ_ONCUT;
        public const int DISPID_EVMETH_ONDATAAVAILABLE = STDDISPID_XOBJ_ONDATAAVAILABLE;
        public const int DISPID_EVMETH_ONDATASETCHANGED = STDDISPID_XOBJ_ONDATASETCHANGED;
        public const int DISPID_EVMETH_ONDATASETCOMPLETE = STDDISPID_XOBJ_ONDATASETCOMPLETE;
        public const int DISPID_EVMETH_ONDBLCLICK = DISPID_DBLCLICK;
        public const int DISPID_EVMETH_ONDEACTIVATE = DISPID_ONDEACTIVATE;
        public const int DISPID_EVMETH_ONDRAG = STDDISPID_XOBJ_ONDRAG;
        public const int DISPID_EVMETH_ONDRAGEND = STDDISPID_XOBJ_ONDRAGEND;
        public const int DISPID_EVMETH_ONDRAGENTER = STDDISPID_XOBJ_ONDRAGENTER;
        public const int DISPID_EVMETH_ONDRAGLEAVE = STDDISPID_XOBJ_ONDRAGLEAVE;
        public const int DISPID_EVMETH_ONDRAGOVER = STDDISPID_XOBJ_ONDRAGOVER;
        public const int DISPID_EVMETH_ONDRAGSTART = STDDISPID_XOBJ_ONDRAGSTART;
        public const int DISPID_EVMETH_ONDROP = STDDISPID_XOBJ_ONDROP;
        public const int DISPID_EVMETH_ONERROR = DISPID_ONERROR;
        public const int DISPID_EVMETH_ONERRORUPDATE = STDDISPID_XOBJ_ERRORUPDATE;
        public const int DISPID_EVMETH_ONFILTER = STDDISPID_XOBJ_ONFILTER;
        public const int DISPID_EVMETH_ONFINISH = DISPID_ONFINISH;
        public const int DISPID_EVMETH_ONFOCUS = STDDISPID_XOBJ_ONFOCUS;
        public const int DISPID_EVMETH_ONFOCUSIN = DISPID_ONFOCUSIN;
        public const int DISPID_EVMETH_ONFOCUSOUT = DISPID_ONFOCUSOUT;
        public const int DISPID_EVMETH_ONHELP = STDDISPID_XOBJ_ONHELP;
        public const int DISPID_EVMETH_ONKEYDOWN = DISPID_KEYDOWN;
        public const int DISPID_EVMETH_ONKEYPRESS = DISPID_KEYPRESS;
        public const int DISPID_EVMETH_ONKEYUP = DISPID_KEYUP;
        public const int DISPID_EVMETH_ONLAYOUT = DISPID_ONLAYOUT;
        public const int DISPID_EVMETH_ONLAYOUTCOMPLETE = DISPID_ONLAYOUTCOMPLETE;
        public const int DISPID_EVMETH_ONLINKEDOVERFLOW = DISPID_ONLINKEDOVERFLOW;
        public const int DISPID_EVMETH_ONLOAD = DISPID_ONLOAD;
        public const int DISPID_EVMETH_ONLOSECAPTURE = STDDISPID_XOBJ_ONLOSECAPTURE;
        public const int DISPID_EVMETH_ONMOUSEDOWN = DISPID_MOUSEDOWN;
        public const int DISPID_EVMETH_ONMOUSEENTER = DISPID_ONMOUSEENTER;
        public const int DISPID_EVMETH_ONMOUSEHOVER = DISPID_ONMOUSEHOVER;
        public const int DISPID_EVMETH_ONMOUSELEAVE = DISPID_ONMOUSELEAVE;
        public const int DISPID_EVMETH_ONMOUSEMOVE = DISPID_MOUSEMOVE;
        public const int DISPID_EVMETH_ONMOUSEOUT = STDDISPID_XOBJ_ONMOUSEOUT;
        public const int DISPID_EVMETH_ONMOUSEOVER = STDDISPID_XOBJ_ONMOUSEOVER;
        public const int DISPID_EVMETH_ONMOUSEUP = DISPID_MOUSEUP;
        public const int DISPID_EVMETH_ONMOUSEWHEEL = DISPID_ONMOUSEWHEEL;
        public const int DISPID_EVMETH_ONMOVE = DISPID_ONMOVE;
        public const int DISPID_EVMETH_ONMOVEEND = DISPID_ONMOVEEND;
        public const int DISPID_EVMETH_ONMOVESTART = DISPID_ONMOVESTART;
        public const int DISPID_EVMETH_ONMULTILAYOUTCLEANUP = DISPID_ONMULTILAYOUTCLEANUP;
        public const int DISPID_EVMETH_ONPAGE = DISPID_ONPAGE;
        public const int DISPID_EVMETH_ONPASTE = STDDISPID_XOBJ_ONPASTE;
        public const int DISPID_EVMETH_ONPERSISTLOAD = DISPID_ONPERSISTLOAD;
        public const int DISPID_EVMETH_ONPERSISTSAVE = DISPID_ONPERSISTSAVE;
        public const int DISPID_EVMETH_ONPROPERTYCHANGE = STDDISPID_XOBJ_ONPROPERTYCHANGE;
        public const int DISPID_EVMETH_ONREADYSTATECHANGE = DISPID_READYSTATECHANGE;
        public const int DISPID_EVMETH_ONRESET = DISPID_ONRESET;
        public const int DISPID_EVMETH_ONRESIZE = DISPID_ONRESIZE;
        public const int DISPID_EVMETH_ONRESIZEEND = DISPID_ONRESIZEEND;
        public const int DISPID_EVMETH_ONRESIZESTART = DISPID_ONRESIZESTART;
        public const int DISPID_EVMETH_ONROWENTER = STDDISPID_XOBJ_ONROWENTER;
        public const int DISPID_EVMETH_ONROWEXIT = STDDISPID_XOBJ_ONROWEXIT;
        public const int DISPID_EVMETH_ONROWSDELETE = STDDISPID_XOBJ_ONROWSDELETE;
        public const int DISPID_EVMETH_ONROWSINSERTED = STDDISPID_XOBJ_ONROWSINSERTED;
        public const int DISPID_EVMETH_ONSCROLL = DISPID_ONSCROLL;
        public const int DISPID_EVMETH_ONSELECT = DISPID_ONSELECT;
        public const int DISPID_EVMETH_ONSELECTIONCHANGE = DISPID_ONSELECTIONCHANGE;
        public const int DISPID_EVMETH_ONSELECTSTART = STDDISPID_XOBJ_ONSELECTSTART;
        public const int DISPID_EVMETH_ONSTART = DISPID_ONSTART;
        public const int DISPID_EVMETH_ONSTOP = DISPID_ONSTOP;
        public const int DISPID_EVMETH_ONSUBMIT = DISPID_ONSUBMIT;
        public const int DISPID_EVMETH_ONUNLOAD = DISPID_ONUNLOAD;
        public const int DISPID_EVPROP_ONABORT = DISPID_EVENTS + 28;
        public const int DISPID_EVPROP_ONACTIVATE = DISPID_EVENTS + 87;
        public const int DISPID_EVPROP_ONAFTERPRINT = DISPID_EVENTS + 67;
        public const int DISPID_EVPROP_ONAFTERUPDATE = DISPID_EVENTS + 22;
        public const int DISPID_EVPROP_ONATTACHEVENT = DISPID_EVENTS + 70;
        public const int DISPID_EVPROP_ONBEFOREACTIVATE = DISPID_EVENTS + 90;
        public const int DISPID_EVPROP_ONBEFORECOPY = DISPID_EVENTS + 59;
        public const int DISPID_EVPROP_ONBEFORECUT = DISPID_EVENTS + 58;
        public const int DISPID_EVPROP_ONBEFOREDEACTIVATE = DISPID_EVENTS + 77;
        public const int DISPID_EVPROP_ONBEFOREDRAGOVER = DISPID_EVENTS + 23;
        //public const int  DISPID_EVMETH_ONBEFOREDRAGOVER =  EVENTID_CommonCtrlEvent_BeforeDragOver;
        public const int DISPID_EVPROP_ONBEFOREDROPORPASTE = DISPID_EVENTS + 24;
        public const int DISPID_EVPROP_ONBEFOREEDITFOCUS = DISPID_EVENTS + 69;
        public const int DISPID_EVPROP_ONBEFOREPASTE = DISPID_EVENTS + 60;
        public const int DISPID_EVPROP_ONBEFOREPRINT = DISPID_EVENTS + 66;
        public const int DISPID_EVPROP_ONBEFOREUNLOAD = DISPID_EVENTS + 39;
        public const int DISPID_EVPROP_ONBEFOREUPDATE = DISPID_EVENTS + 21;
        public const int DISPID_EVPROP_ONBLUR = DISPID_EVENTS + 15;
        public const int DISPID_EVPROP_ONBOUNCE = DISPID_EVENTS + 20;
        public const int DISPID_EVPROP_ONCELLCHANGE = DISPID_EVENTS + 64;
        public const int DISPID_EVPROP_ONCHANGE = DISPID_EVENTS + 30;
        public const int DISPID_EVPROP_ONCHANGEBLUR = DISPID_EVENTS + 45;
        public const int DISPID_EVPROP_ONCHANGEFOCUS = DISPID_EVENTS + 44;
        public const int DISPID_EVPROP_ONCLICK = DISPID_EVENTS + 8;
        public const int DISPID_EVPROP_ONCONTENTREADY = DISPID_EVENTS + 72;
        public const int DISPID_EVPROP_ONCONTEXTMENU = DISPID_EVENTS + 65;
        public const int DISPID_EVPROP_ONCONTROLSELECT = DISPID_EVENTS + 79;
        public const int DISPID_EVPROP_ONCOPY = DISPID_EVENTS + 56;
        public const int DISPID_EVPROP_ONCUT = DISPID_EVENTS + 55;
        public const int DISPID_EVPROP_ONDATAAVAILABLE = DISPID_EVENTS + 41;
        //public const int  DISPID_EVMETH_ONBEFOREUNLOAD  = DISPID_ONBEFOREUNLOAD;
        public const int DISPID_EVPROP_ONDATASETCHANGED = DISPID_EVENTS + 40;
        public const int DISPID_EVPROP_ONDATASETCOMPLETE = DISPID_EVENTS + 42;
        public const int DISPID_EVPROP_ONDBLCLICK = DISPID_EVENTS + 9;
        public const int DISPID_EVPROP_ONDEACTIVATE = DISPID_EVENTS + 88;
        public const int DISPID_EVPROP_ONDRAG = DISPID_EVENTS + 49;
        public const int DISPID_EVPROP_ONDRAGEND = DISPID_EVENTS + 50;
        public const int DISPID_EVPROP_ONDRAGENTER = DISPID_EVENTS + 51;
        public const int DISPID_EVPROP_ONDRAGLEAVE = DISPID_EVENTS + 53;
        public const int DISPID_EVPROP_ONDRAGOVER = DISPID_EVENTS + 52;
        public const int DISPID_EVPROP_ONDRAGSTART = DISPID_EVENTS + 35;
        public const int DISPID_EVPROP_ONDROP = DISPID_EVENTS + 54;
        public const int DISPID_EVPROP_ONERROR = DISPID_EVENTS + 29;
        public const int DISPID_EVPROP_ONERRORUPDATE = DISPID_EVENTS + 38;
        public const int DISPID_EVPROP_ONFILTER = DISPID_EVENTS + 43;
        public const int DISPID_EVPROP_ONFINISH = DISPID_EVENTS + 26;
        public const int DISPID_EVPROP_ONFOCUS = DISPID_EVENTS + 14;
        public const int DISPID_EVPROP_ONFOCUSIN = DISPID_EVENTS + 91;
        public const int DISPID_EVPROP_ONFOCUSOUT = DISPID_EVENTS + 92;
        public const int DISPID_EVPROP_ONHELP = DISPID_EVENTS + 13;
        public const int DISPID_EVPROP_ONKEYDOWN = DISPID_EVENTS + 5;
        public const int DISPID_EVPROP_ONKEYPRESS = DISPID_EVENTS + 7;
        public const int DISPID_EVPROP_ONKEYUP = DISPID_EVENTS + 6;
        public const int DISPID_EVPROP_ONLAYOUT = DISPID_EVENTS + 34;
        public const int DISPID_EVPROP_ONLAYOUTCOMPLETE = DISPID_EVENTS + 73;
        public const int DISPID_EVPROP_ONLINKEDOVERFLOW = DISPID_EVENTS + 75;
        public const int DISPID_EVPROP_ONLOAD = DISPID_EVENTS + 32;
        public const int DISPID_EVPROP_ONLOSECAPTURE = DISPID_EVENTS + 46;
        public const int DISPID_EVPROP_ONMOUSEDOWN = DISPID_EVENTS + 2;
        public const int DISPID_EVPROP_ONMOUSEENTER = DISPID_EVENTS + 85;
        public const int DISPID_EVPROP_ONMOUSEHOVER = DISPID_EVENTS + 71;
        public const int DISPID_EVPROP_ONMOUSELEAVE = DISPID_EVENTS + 86;
        public const int DISPID_EVPROP_ONMOUSEMOVE = DISPID_EVENTS + 4;
        public const int DISPID_EVPROP_ONMOUSEOUT = DISPID_EVENTS + 1;
        public const int DISPID_EVPROP_ONMOUSEOVER = DISPID_EVENTS + 0;
        public const int DISPID_EVPROP_ONMOUSEUP = DISPID_EVENTS + 3;
        public const int DISPID_EVPROP_ONMOUSEWHEEL = DISPID_EVENTS + 76;
        public const int DISPID_EVPROP_ONMOVE = DISPID_EVENTS + 78;
        public const int DISPID_EVPROP_ONMOVEEND = DISPID_EVENTS + 82;
        public const int DISPID_EVPROP_ONMOVESTART = DISPID_EVENTS + 81;
        public const int DISPID_EVPROP_ONMULTILAYOUTCLEANUP = DISPID_EVENTS + 89;
        public const int DISPID_EVPROP_ONPAGE = DISPID_EVENTS + 74;
        public const int DISPID_EVPROP_ONPASTE = DISPID_EVENTS + 57;
        public const int DISPID_EVPROP_ONPERSISTLOAD = DISPID_EVENTS + 61;
        public const int DISPID_EVPROP_ONPERSISTSAVE = DISPID_EVENTS + 48;
        public const int DISPID_EVPROP_ONPROPERTYCHANGE = DISPID_EVENTS + 47;
        public const int DISPID_EVPROP_ONREADYSTATECHANGE = DISPID_EVENTS + 25;
        public const int DISPID_EVPROP_ONRESET = DISPID_EVENTS + 12;
        public const int DISPID_EVPROP_ONRESIZE = DISPID_EVENTS + 36;
        public const int DISPID_EVPROP_ONRESIZEEND = DISPID_EVENTS + 84;
        public const int DISPID_EVPROP_ONRESIZESTART = DISPID_EVENTS + 83;
        public const int DISPID_EVPROP_ONROWENTER = DISPID_EVENTS + 19;
        public const int DISPID_EVPROP_ONROWEXIT = DISPID_EVENTS + 18;
        public const int DISPID_EVPROP_ONROWSDELETE = DISPID_EVENTS + 62;
        public const int DISPID_EVPROP_ONROWSINSERTED = DISPID_EVENTS + 63;
        public const int DISPID_EVPROP_ONSCROLL = DISPID_EVENTS + 31;
        public const int DISPID_EVPROP_ONSELECT = DISPID_EVENTS + 10;
        public const int DISPID_EVPROP_ONSELECTIONCHANGE = DISPID_EVENTS + 80;
        public const int DISPID_EVPROP_ONSELECTSTART = DISPID_EVENTS + 37;
        public const int DISPID_EVPROP_ONSTART = DISPID_EVENTS + 27;
        public const int DISPID_EVPROP_ONSTOP = DISPID_EVENTS + 68;
        public const int DISPID_EVPROP_ONSUBMIT = DISPID_EVENTS + 11;
        public const int DISPID_EVPROP_ONUNLOAD = DISPID_EVENTS + 33;
        public const int DISPID_FILLCOLOR = -510;
        public const int DISPID_FILLSTYLE = -511;
        public const int DISPID_FONT = -512;
        public const int DISPID_FORECOLOR = -513;
        public const int DISPID_FORM = DISPID_NORMAL_FIRST;
        public const int DISPID_FRAME = DISPID_FRAMESITE + 1000;
        public const int DISPID_FRAMESET = DISPID_NORMAL_FIRST;
        public const int DISPID_FRAMESITE = DISPID_SITE + 1000;
        public const int DISPID_GROUPNAME = -541;
        public const int DISPID_HEDELEMS = DISPID_NORMAL_FIRST;
        public const int DISPID_HR = DISPID_NORMAL_FIRST;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONACTIVATE = DISPID_EVMETH_ONACTIVATE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONAFTERUPDATE = DISPID_EVMETH_ONAFTERUPDATE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONBEFOREACTIVATE = DISPID_EVMETH_ONBEFOREACTIVATE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONBEFOREDEACTIVATE = DISPID_EVMETH_ONBEFOREDEACTIVATE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONBEFOREEDITFOCUS = DISPID_EVMETH_ONBEFOREEDITFOCUS;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONBEFOREUPDATE = DISPID_EVMETH_ONBEFOREUPDATE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONCELLCHANGE = DISPID_EVMETH_ONCELLCHANGE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONCLICK = DISPID_EVMETH_ONCLICK;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONCONTEXTMENU = DISPID_EVMETH_ONCONTEXTMENU;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONCONTROLSELECT = DISPID_EVMETH_ONCONTROLSELECT;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONDATAAVAILABLE = DISPID_EVMETH_ONDATAAVAILABLE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONDATASETCHANGED = DISPID_EVMETH_ONDATASETCHANGED;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONDATASETCOMPLETE = DISPID_EVMETH_ONDATASETCOMPLETE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONDBLCLICK = DISPID_EVMETH_ONDBLCLICK;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONDEACTIVATE = DISPID_EVMETH_ONDEACTIVATE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONDRAGSTART = DISPID_EVMETH_ONDRAGSTART;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONERRORUPDATE = DISPID_EVMETH_ONERRORUPDATE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONFOCUSIN = DISPID_EVMETH_ONFOCUSIN;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONFOCUSOUT = DISPID_EVMETH_ONFOCUSOUT;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONHELP = DISPID_EVMETH_ONHELP;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONKEYDOWN = DISPID_EVMETH_ONKEYDOWN;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONKEYPRESS = DISPID_EVMETH_ONKEYPRESS;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONKEYUP = DISPID_EVMETH_ONKEYUP;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEDOWN = DISPID_EVMETH_ONMOUSEDOWN;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEMOVE = DISPID_EVMETH_ONMOUSEMOVE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEOUT = DISPID_EVMETH_ONMOUSEOUT;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEOVER = DISPID_EVMETH_ONMOUSEOVER;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEUP = DISPID_EVMETH_ONMOUSEUP;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONMOUSEWHEEL = DISPID_EVMETH_ONMOUSEWHEEL;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONPROPERTYCHANGE = DISPID_EVMETH_ONPROPERTYCHANGE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONREADYSTATECHANGE = DISPID_EVMETH_ONREADYSTATECHANGE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONROWENTER = DISPID_EVMETH_ONROWENTER;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONROWEXIT = DISPID_EVMETH_ONROWEXIT;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONROWSDELETE = DISPID_EVMETH_ONROWSDELETE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONROWSINSERTED = DISPID_EVMETH_ONROWSINSERTED;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONSELECTIONCHANGE = DISPID_EVMETH_ONSELECTIONCHANGE;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONSELECTSTART = DISPID_EVMETH_ONSELECTSTART;
        public const int DISPID_HTMLDOCUMENTEVENTS2_ONSTOP = DISPID_EVMETH_ONSTOP;
        public const int DISPID_HTMLELEMENTEVENTS2_ONACTIVATE = DISPID_EVMETH_ONACTIVATE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONAFTERUPDATE = DISPID_EVMETH_ONAFTERUPDATE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONBEFOREACTIVATE = DISPID_EVMETH_ONBEFOREACTIVATE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONBEFORECOPY = DISPID_EVMETH_ONBEFORECOPY;
        public const int DISPID_HTMLELEMENTEVENTS2_ONBEFORECUT = DISPID_EVMETH_ONBEFORECUT;
        public const int DISPID_HTMLELEMENTEVENTS2_ONBEFOREDEACTIVATE = DISPID_EVMETH_ONBEFOREDEACTIVATE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONBEFOREPASTE = DISPID_EVMETH_ONBEFOREPASTE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONBEFOREUPDATE = DISPID_EVMETH_ONBEFOREUPDATE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONBLUR = DISPID_EVMETH_ONBLUR;
        public const int DISPID_HTMLELEMENTEVENTS2_ONCELLCHANGE = DISPID_EVMETH_ONCELLCHANGE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONCLICK = DISPID_EVMETH_ONCLICK;
        public const int DISPID_HTMLELEMENTEVENTS2_ONCONTEXTMENU = DISPID_EVMETH_ONCONTEXTMENU;
        public const int DISPID_HTMLELEMENTEVENTS2_ONCONTROLSELECT = DISPID_EVMETH_ONCONTROLSELECT;
        public const int DISPID_HTMLELEMENTEVENTS2_ONCOPY = DISPID_EVMETH_ONCOPY;
        public const int DISPID_HTMLELEMENTEVENTS2_ONCUT = DISPID_EVMETH_ONCUT;
        public const int DISPID_HTMLELEMENTEVENTS2_ONDATAAVAILABLE = DISPID_EVMETH_ONDATAAVAILABLE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONDATASETCHANGED = DISPID_EVMETH_ONDATASETCHANGED;
        public const int DISPID_HTMLELEMENTEVENTS2_ONDATASETCOMPLETE = DISPID_EVMETH_ONDATASETCOMPLETE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONDBLCLICK = DISPID_EVMETH_ONDBLCLICK;
        public const int DISPID_HTMLELEMENTEVENTS2_ONDEACTIVATE = DISPID_EVMETH_ONDEACTIVATE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONDRAG = DISPID_EVMETH_ONDRAG;
        public const int DISPID_HTMLELEMENTEVENTS2_ONDRAGEND = DISPID_EVMETH_ONDRAGEND;
        public const int DISPID_HTMLELEMENTEVENTS2_ONDRAGENTER = DISPID_EVMETH_ONDRAGENTER;
        public const int DISPID_HTMLELEMENTEVENTS2_ONDRAGLEAVE = DISPID_EVMETH_ONDRAGLEAVE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONDRAGOVER = DISPID_EVMETH_ONDRAGOVER;
        public const int DISPID_HTMLELEMENTEVENTS2_ONDRAGSTART = DISPID_EVMETH_ONDRAGSTART;
        public const int DISPID_HTMLELEMENTEVENTS2_ONDROP = DISPID_EVMETH_ONDROP;
        public const int DISPID_HTMLELEMENTEVENTS2_ONERRORUPDATE = DISPID_EVMETH_ONERRORUPDATE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONFILTERCHANGE = DISPID_EVMETH_ONFILTER;
        public const int DISPID_HTMLELEMENTEVENTS2_ONFOCUS = DISPID_EVMETH_ONFOCUS;
        public const int DISPID_HTMLELEMENTEVENTS2_ONFOCUSIN = DISPID_EVMETH_ONFOCUSIN;
        public const int DISPID_HTMLELEMENTEVENTS2_ONFOCUSOUT = DISPID_EVMETH_ONFOCUSOUT;
        public const int DISPID_HTMLELEMENTEVENTS2_ONHELP = DISPID_EVMETH_ONHELP;
        public const int DISPID_HTMLELEMENTEVENTS2_ONKEYDOWN = DISPID_EVMETH_ONKEYDOWN;
        public const int DISPID_HTMLELEMENTEVENTS2_ONKEYPRESS = DISPID_EVMETH_ONKEYPRESS;
        public const int DISPID_HTMLELEMENTEVENTS2_ONKEYUP = DISPID_EVMETH_ONKEYUP;
        public const int DISPID_HTMLELEMENTEVENTS2_ONLAYOUTCOMPLETE = DISPID_EVMETH_ONLAYOUTCOMPLETE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONLOSECAPTURE = DISPID_EVMETH_ONLOSECAPTURE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONMOUSEDOWN = DISPID_EVMETH_ONMOUSEDOWN;
        public const int DISPID_HTMLELEMENTEVENTS2_ONMOUSEENTER = DISPID_EVMETH_ONMOUSEENTER;
        public const int DISPID_HTMLELEMENTEVENTS2_ONMOUSELEAVE = DISPID_EVMETH_ONMOUSELEAVE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONMOUSEMOVE = DISPID_EVMETH_ONMOUSEMOVE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONMOUSEOUT = DISPID_EVMETH_ONMOUSEOUT;
        public const int DISPID_HTMLELEMENTEVENTS2_ONMOUSEOVER = DISPID_EVMETH_ONMOUSEOVER;
        public const int DISPID_HTMLELEMENTEVENTS2_ONMOUSEUP = DISPID_EVMETH_ONMOUSEUP;
        public const int DISPID_HTMLELEMENTEVENTS2_ONMOUSEWHEEL = DISPID_EVMETH_ONMOUSEWHEEL;
        public const int DISPID_HTMLELEMENTEVENTS2_ONMOVE = DISPID_EVMETH_ONMOVE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONMOVEEND = DISPID_EVMETH_ONMOVEEND;
        public const int DISPID_HTMLELEMENTEVENTS2_ONMOVESTART = DISPID_EVMETH_ONMOVESTART;
        public const int DISPID_HTMLELEMENTEVENTS2_ONPAGE = DISPID_EVMETH_ONPAGE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONPASTE = DISPID_EVMETH_ONPASTE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONPROPERTYCHANGE = DISPID_EVMETH_ONPROPERTYCHANGE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONREADYSTATECHANGE = DISPID_EVMETH_ONREADYSTATECHANGE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONRESIZE = DISPID_EVMETH_ONRESIZE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONRESIZEEND = DISPID_EVMETH_ONRESIZEEND;
        public const int DISPID_HTMLELEMENTEVENTS2_ONRESIZESTART = DISPID_EVMETH_ONRESIZESTART;
        public const int DISPID_HTMLELEMENTEVENTS2_ONROWENTER = DISPID_EVMETH_ONROWENTER;
        public const int DISPID_HTMLELEMENTEVENTS2_ONROWEXIT = DISPID_EVMETH_ONROWEXIT;
        public const int DISPID_HTMLELEMENTEVENTS2_ONROWSDELETE = DISPID_EVMETH_ONROWSDELETE;
        public const int DISPID_HTMLELEMENTEVENTS2_ONROWSINSERTED = DISPID_EVMETH_ONROWSINSERTED;
        public const int DISPID_HTMLELEMENTEVENTS2_ONSCROLL = DISPID_EVMETH_ONSCROLL;
        public const int DISPID_HTMLELEMENTEVENTS2_ONSELECTSTART = DISPID_EVMETH_ONSELECTSTART;
        public const int DISPID_HTMLOBJECT = DISPID_XOBJ_BASE + 500;
        public const int DISPID_HTMLPOPUP = 27000;
        public const int DISPID_HTMLWINDOWEVENTS2_ONAFTERPRINT = DISPID_EVMETH_ONAFTERPRINT;
        public const int DISPID_HTMLWINDOWEVENTS2_ONBEFOREPRINT = DISPID_EVMETH_ONBEFOREPRINT;
        public const int DISPID_HTMLWINDOWEVENTS2_ONBEFOREUNLOAD = DISPID_EVMETH_ONBEFOREUNLOAD;
        public const int DISPID_HTMLWINDOWEVENTS2_ONBLUR = DISPID_EVMETH_ONBLUR;
        public const int DISPID_HTMLWINDOWEVENTS2_ONERROR = DISPID_EVMETH_ONERROR;
        public const int DISPID_HTMLWINDOWEVENTS2_ONFOCUS = DISPID_EVMETH_ONFOCUS;
        public const int DISPID_HTMLWINDOWEVENTS2_ONHELP = DISPID_EVMETH_ONHELP;
        public const int DISPID_HTMLWINDOWEVENTS2_ONLOAD = DISPID_EVMETH_ONLOAD;
        public const int DISPID_HTMLWINDOWEVENTS2_ONRESIZE = DISPID_EVMETH_ONRESIZE;
        public const int DISPID_HTMLWINDOWEVENTS2_ONSCROLL = DISPID_EVMETH_ONSCROLL;
        public const int DISPID_HTMLWINDOWEVENTS2_ONUNLOAD = DISPID_EVMETH_ONUNLOAD;
        public const int DISPID_HWND = -515;
        public const int DISPID_IFRAME = DISPID_FRAMESITE + 1000;
        public const int DISPID_IHTMLANCHORELEMENT_ACCESSKEY = DISPID_SITE + 5;
        public const int DISPID_IHTMLANCHORELEMENT_BLUR = DISPID_SITE + 2;
        public const int DISPID_IHTMLANCHORELEMENT_FOCUS = DISPID_SITE + 0;
        public const int DISPID_IHTMLANCHORELEMENT_HASH = DISPID_ANCHOR + 18;
        public const int DISPID_IHTMLANCHORELEMENT_HOST = DISPID_ANCHOR + 12;
        public const int DISPID_IHTMLANCHORELEMENT_HOSTNAME = DISPID_ANCHOR + 13;
        public const int DISPID_IHTMLANCHORELEMENT_HREF = DISPID_VALUE;
        public const int DISPID_IHTMLANCHORELEMENT_METHODS = DISPID_ANCHOR + 8;
        public const int DISPID_IHTMLANCHORELEMENT_MIMETYPE = DISPID_ANCHOR + 30;
        public const int DISPID_IHTMLANCHORELEMENT_NAME = STDPROPID_XOBJ_NAME;
        public const int DISPID_IHTMLANCHORELEMENT_NAMEPROP = DISPID_ANCHOR + 32;
        public const int DISPID_IHTMLANCHORELEMENT_ONBLUR = DISPID_EVPROP_ONBLUR;
        public const int DISPID_IHTMLANCHORELEMENT_ONFOCUS = DISPID_EVPROP_ONFOCUS;
        public const int DISPID_IHTMLANCHORELEMENT_PATHNAME = DISPID_ANCHOR + 14;
        public const int DISPID_IHTMLANCHORELEMENT_PORT = DISPID_ANCHOR + 15;
        public const int DISPID_IHTMLANCHORELEMENT_PROTOCOL = DISPID_ANCHOR + 16;
        public const int DISPID_IHTMLANCHORELEMENT_PROTOCOLLONG = DISPID_ANCHOR + 31;
        public const int DISPID_IHTMLANCHORELEMENT_REL = DISPID_ANCHOR + 5;
        public const int DISPID_IHTMLANCHORELEMENT_REV = DISPID_ANCHOR + 6;
        public const int DISPID_IHTMLANCHORELEMENT_SEARCH = DISPID_ANCHOR + 17;
        public const int DISPID_IHTMLANCHORELEMENT_TABINDEX = STDPROPID_XOBJ_TABINDEX;
        public const int DISPID_IHTMLANCHORELEMENT_TARGET = DISPID_ANCHOR + 3;
        public const int DISPID_IHTMLANCHORELEMENT_URN = DISPID_ANCHOR + 7;
        public const int DISPID_IHTMLANCHORELEMENT2_CHARSET = DISPID_ANCHOR + 23;
        public const int DISPID_IHTMLANCHORELEMENT2_COORDS = DISPID_ANCHOR + 24;
        public const int DISPID_IHTMLANCHORELEMENT2_HREFLANG = DISPID_ANCHOR + 25;
        public const int DISPID_IHTMLANCHORELEMENT2_SHAPE = DISPID_ANCHOR + 26;
        public const int DISPID_IHTMLANCHORELEMENT2_TYPE = DISPID_ANCHOR + 27;
        public const int DISPID_IHTMLAREAELEMENT_ALT = DISPID_AREA + 5;
        public const int DISPID_IHTMLAREAELEMENT_BLUR = DISPID_SITE + 2;
        public const int DISPID_IHTMLAREAELEMENT_COORDS = DISPID_AREA + 2;
        public const int DISPID_IHTMLAREAELEMENT_FOCUS = DISPID_SITE + 0;
        public const int DISPID_IHTMLAREAELEMENT_HASH = DISPID_AREA + 13;
        public const int DISPID_IHTMLAREAELEMENT_HOST = DISPID_AREA + 7;
        public const int DISPID_IHTMLAREAELEMENT_HOSTNAME = DISPID_AREA + 8;
        public const int DISPID_IHTMLAREAELEMENT_HREF = DISPID_VALUE;
        public const int DISPID_IHTMLAREAELEMENT_NOHREF = DISPID_AREA + 6;
        public const int DISPID_IHTMLAREAELEMENT_ONBLUR = DISPID_EVPROP_ONBLUR;
        public const int DISPID_IHTMLAREAELEMENT_ONFOCUS = DISPID_EVPROP_ONFOCUS;
        public const int DISPID_IHTMLAREAELEMENT_PATHNAME = DISPID_AREA + 9;
        public const int DISPID_IHTMLAREAELEMENT_PORT = DISPID_AREA + 10;
        public const int DISPID_IHTMLAREAELEMENT_PROTOCOL = DISPID_AREA + 11;
        public const int DISPID_IHTMLAREAELEMENT_SEARCH = DISPID_AREA + 12;
        public const int DISPID_IHTMLAREAELEMENT_SHAPE = DISPID_AREA + 1;
        public const int DISPID_IHTMLAREAELEMENT_TABINDEX = STDPROPID_XOBJ_TABINDEX;
        public const int DISPID_IHTMLAREAELEMENT_TARGET = DISPID_AREA + 4;
        public const int DISPID_IHTMLAREASCOLLECTION__NEWENUM = DISPID_NEWENUM;
        public const int DISPID_IHTMLAREASCOLLECTION_ADD = DISPID_COLLECTION + 3;
        public const int DISPID_IHTMLAREASCOLLECTION_ITEM = DISPID_VALUE;
        public const int DISPID_IHTMLAREASCOLLECTION_LENGTH = DISPID_COLLECTION;
        public const int DISPID_IHTMLAREASCOLLECTION_REMOVE = DISPID_COLLECTION + 4;
        public const int DISPID_IHTMLAREASCOLLECTION_TAGS = DISPID_COLLECTION + 2;
        public const int DISPID_IHTMLATTRIBUTECOLLECTION__NEWENUM = DISPID_NEWENUM;
        public const int DISPID_IHTMLATTRIBUTECOLLECTION_ITEM = DISPID_VALUE;
        public const int DISPID_IHTMLATTRIBUTECOLLECTION_LENGTH = DISPID_COLLECTION;
        public const int DISPID_IHTMLBASEELEMENT_HREF = DISPID_HEDELEMS + 3;
        public const int DISPID_IHTMLBASEELEMENT_TARGET = DISPID_HEDELEMS + 4;
        public const int DISPID_IHTMLBASEFONTELEMENT_COLOR = DISPID_A_COLOR;
        public const int DISPID_IHTMLBASEFONTELEMENT_FACE = DISPID_A_FONTFACE;
        public const int DISPID_IHTMLBASEFONTELEMENT_SIZE = DISPID_A_BASEFONT;
        public const int DISPID_IHTMLBODYELEMENT_CREATETEXTRANGE = DISPID_BODY + 13;
        public const int DISPID_IHTMLBRELEMENT_CLEAR = DISPID_A_CLEAR;
        public const int DISPID_IHTMLBUTTONELEMENT_CREATETEXTRANGE = DISPID_BUTTON + 2;
        public const int DISPID_IHTMLBUTTONELEMENT_DISABLED = STDPROPID_XOBJ_DISABLED;
        public const int DISPID_IHTMLBUTTONELEMENT_FORM = DISPID_SITE + 4;
        public const int DISPID_IHTMLBUTTONELEMENT_NAME = STDPROPID_XOBJ_NAME;
        public const int DISPID_IHTMLBUTTONELEMENT_STATUS = DISPID_BUTTON + 1;
        public const int DISPID_IHTMLBUTTONELEMENT_TYPE = DISPID_INPUT;
        public const int DISPID_IHTMLBUTTONELEMENT_VALUE = DISPID_A_VALUE;
        public const int DISPID_IHTMLCOMMENTELEMENT_ATOMIC = DISPID_COMMENTPDL + 2;
        public const int DISPID_IHTMLCOMMENTELEMENT_TEXT = DISPID_COMMENTPDL + 1;
        public const int DISPID_IHTMLCOMPUTEDSTYLE = DISPID_NORMAL_FIRST;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_BACKGROUNDCOLOR = DISPID_IHTMLCOMPUTEDSTYLE + 14;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_BLOCKDIRECTION = DISPID_IHTMLCOMPUTEDSTYLE + 17;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_BOLD = DISPID_IHTMLCOMPUTEDSTYLE + 1;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_DIRECTION = DISPID_IHTMLCOMPUTEDSTYLE + 16;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_EXPLICITFACE = DISPID_IHTMLCOMPUTEDSTYLE + 8;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_FONTNAME = DISPID_IHTMLCOMPUTEDSTYLE + 11;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_FONTSIZE = DISPID_IHTMLCOMPUTEDSTYLE + 10;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_FONTWEIGHT = DISPID_IHTMLCOMPUTEDSTYLE + 9;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_HASBGCOLOR = DISPID_IHTMLCOMPUTEDSTYLE + 12;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_ITALIC = DISPID_IHTMLCOMPUTEDSTYLE + 2;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_OL = DISPID_IHTMLCOMPUTEDSTYLE + 18;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_OVERLINE = DISPID_IHTMLCOMPUTEDSTYLE + 4;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_PREFORMATTED = DISPID_IHTMLCOMPUTEDSTYLE + 15;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_STRIKEOUT = DISPID_IHTMLCOMPUTEDSTYLE + 5;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_SUBSCRIPT = DISPID_IHTMLCOMPUTEDSTYLE + 6;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_SUPERSCRIPT = DISPID_IHTMLCOMPUTEDSTYLE + 7;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_TEXTCOLOR = DISPID_IHTMLCOMPUTEDSTYLE + 13;
        public const int DISPID_IHTMLCOMPUTEDSTYLE_UNDERLINE = DISPID_IHTMLCOMPUTEDSTYLE + 3;
        public const int DISPID_IHTMLCONTROLELEMENT_ACCESSKEY = DISPID_SITE + 5;
        public const int DISPID_IHTMLCONTROLELEMENT_ADDFILTER = DISPID_SITE + 17;
        public const int DISPID_IHTMLCONTROLELEMENT_BLUR = DISPID_SITE + 2;
        public const int DISPID_IHTMLCONTROLELEMENT_CLIENTHEIGHT = DISPID_SITE + 19;
        public const int DISPID_IHTMLCONTROLELEMENT_CLIENTLEFT = DISPID_SITE + 22;
        public const int DISPID_IHTMLCONTROLELEMENT_CLIENTTOP = DISPID_SITE + 21;
        public const int DISPID_IHTMLCONTROLELEMENT_CLIENTWIDTH = DISPID_SITE + 20;
        public const int DISPID_IHTMLCONTROLELEMENT_FOCUS = DISPID_SITE + 0;
        public const int DISPID_IHTMLCONTROLELEMENT_ONBLUR = DISPID_EVPROP_ONBLUR;
        public const int DISPID_IHTMLCONTROLELEMENT_ONFOCUS = DISPID_EVPROP_ONFOCUS;
        public const int DISPID_IHTMLCONTROLELEMENT_ONRESIZE = DISPID_EVPROP_ONRESIZE;
        public const int DISPID_IHTMLCONTROLELEMENT_REMOVEFILTER = DISPID_SITE + 18;
        public const int DISPID_IHTMLCONTROLELEMENT_TABINDEX = STDPROPID_XOBJ_TABINDEX;
        public const int DISPID_IHTMLCONTROLRANGE_ADD = DISPID_RANGE + 3;
        public const int DISPID_IHTMLCONTROLRANGE_COMMONPARENTELEMENT = DISPID_RANGE + 15;
        public const int DISPID_IHTMLCONTROLRANGE_EXECCOMMAND = DISPID_RANGE + 13;
        public const int DISPID_IHTMLCONTROLRANGE_EXECCOMMANDSHOWHELP = DISPID_RANGE + 14;
        public const int DISPID_IHTMLCONTROLRANGE_ITEM = DISPID_VALUE;
        public const int DISPID_IHTMLCONTROLRANGE_LENGTH = DISPID_RANGE + 5;
        public const int DISPID_IHTMLCONTROLRANGE_QUERYCOMMANDENABLED = DISPID_RANGE + 8;
        public const int DISPID_IHTMLCONTROLRANGE_QUERYCOMMANDINDETERM = DISPID_RANGE + 10;
        public const int DISPID_IHTMLCONTROLRANGE_QUERYCOMMANDSTATE = DISPID_RANGE + 9;
        public const int DISPID_IHTMLCONTROLRANGE_QUERYCOMMANDSUPPORTED = DISPID_RANGE + 7;
        public const int DISPID_IHTMLCONTROLRANGE_QUERYCOMMANDTEXT = DISPID_RANGE + 11;
        public const int DISPID_IHTMLCONTROLRANGE_QUERYCOMMANDVALUE = DISPID_RANGE + 12;
        public const int DISPID_IHTMLCONTROLRANGE_REMOVE = DISPID_RANGE + 4;
        public const int DISPID_IHTMLCONTROLRANGE_SCROLLINTOVIEW = DISPID_RANGE + 6;
        public const int DISPID_IHTMLCONTROLRANGE_SELECT = DISPID_RANGE + 2;
        public const int DISPID_IHTMLCURRENTSTYLE_ACCELERATOR = DISPID_A_ACCELERATOR;
        public const int DISPID_IHTMLCURRENTSTYLE_BACKGROUNDATTACHMENT = DISPID_A_BACKGROUNDATTACHMENT;
        public const int DISPID_IHTMLCURRENTSTYLE_BACKGROUNDCOLOR = DISPID_BACKCOLOR;
        public const int DISPID_IHTMLCURRENTSTYLE_BACKGROUNDIMAGE = DISPID_A_BACKGROUNDIMAGE;
        public const int DISPID_IHTMLCURRENTSTYLE_BACKGROUNDPOSITIONX = DISPID_A_BACKGROUNDPOSX;
        public const int DISPID_IHTMLCURRENTSTYLE_BACKGROUNDPOSITIONY = DISPID_A_BACKGROUNDPOSY;
        public const int DISPID_IHTMLCURRENTSTYLE_BACKGROUNDREPEAT = DISPID_A_BACKGROUNDREPEAT;
        public const int DISPID_IHTMLCURRENTSTYLE_BEHAVIOR = DISPID_A_BEHAVIOR;
        public const int DISPID_IHTMLCURRENTSTYLE_BLOCKDIRECTION = DISPID_A_DIR;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERBOTTOMCOLOR = DISPID_A_BORDERBOTTOMCOLOR;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERBOTTOMSTYLE = DISPID_A_BORDERBOTTOMSTYLE;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERBOTTOMWIDTH = DISPID_A_BORDERBOTTOMWIDTH;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERCOLLAPSE = DISPID_A_BORDERCOLLAPSE;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERCOLOR = DISPID_A_BORDERCOLOR;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERLEFTCOLOR = DISPID_A_BORDERLEFTCOLOR;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERLEFTSTYLE = DISPID_A_BORDERLEFTSTYLE;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERLEFTWIDTH = DISPID_A_BORDERLEFTWIDTH;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERRIGHTCOLOR = DISPID_A_BORDERRIGHTCOLOR;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERRIGHTSTYLE = DISPID_A_BORDERRIGHTSTYLE;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERRIGHTWIDTH = DISPID_A_BORDERRIGHTWIDTH;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERSTYLE = DISPID_A_BORDERSTYLE;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERTOPCOLOR = DISPID_A_BORDERTOPCOLOR;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERTOPSTYLE = DISPID_A_BORDERTOPSTYLE;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERTOPWIDTH = DISPID_A_BORDERTOPWIDTH;
        public const int DISPID_IHTMLCURRENTSTYLE_BORDERWIDTH = DISPID_A_BORDERWIDTH;
        public const int DISPID_IHTMLCURRENTSTYLE_BOTTOM = STDPROPID_XOBJ_BOTTOM;
        public const int DISPID_IHTMLCURRENTSTYLE_CLEAR = DISPID_A_CLEAR;
        public const int DISPID_IHTMLCURRENTSTYLE_CLIPBOTTOM = DISPID_A_CLIPRECTBOTTOM;
        public const int DISPID_IHTMLCURRENTSTYLE_CLIPLEFT = DISPID_A_CLIPRECTLEFT;
        public const int DISPID_IHTMLCURRENTSTYLE_CLIPRIGHT = DISPID_A_CLIPRECTRIGHT;
        public const int DISPID_IHTMLCURRENTSTYLE_CLIPTOP = DISPID_A_CLIPRECTTOP;
        public const int DISPID_IHTMLCURRENTSTYLE_COLOR = DISPID_A_COLOR;
        public const int DISPID_IHTMLCURRENTSTYLE_CURSOR = DISPID_A_CURSOR;
        public const int DISPID_IHTMLCURRENTSTYLE_DIRECTION = DISPID_A_DIRECTION;
        public const int DISPID_IHTMLCURRENTSTYLE_DISPLAY = DISPID_A_DISPLAY;
        public const int DISPID_IHTMLCURRENTSTYLE_FONTFAMILY = DISPID_A_FONTFACE;
        public const int DISPID_IHTMLCURRENTSTYLE_FONTSIZE = DISPID_A_FONTSIZE;
        public const int DISPID_IHTMLCURRENTSTYLE_FONTSTYLE = DISPID_A_FONTSTYLE;
        public const int DISPID_IHTMLCURRENTSTYLE_FONTVARIANT = DISPID_A_FONTVARIANT;
        public const int DISPID_IHTMLCURRENTSTYLE_FONTWEIGHT = DISPID_A_FONTWEIGHT;
        public const int DISPID_IHTMLCURRENTSTYLE_GETATTRIBUTE = DISPID_HTMLOBJECT + 2;
        public const int DISPID_IHTMLCURRENTSTYLE_HEIGHT = STDPROPID_XOBJ_HEIGHT;
        public const int DISPID_IHTMLCURRENTSTYLE_IMEMODE = DISPID_A_IMEMODE;
        public const int DISPID_IHTMLCURRENTSTYLE_LAYOUTGRIDCHAR = DISPID_A_LAYOUTGRIDCHAR;
        public const int DISPID_IHTMLCURRENTSTYLE_LAYOUTGRIDLINE = DISPID_A_LAYOUTGRIDLINE;
        public const int DISPID_IHTMLCURRENTSTYLE_LAYOUTGRIDMODE = DISPID_A_LAYOUTGRIDMODE;
        public const int DISPID_IHTMLCURRENTSTYLE_LAYOUTGRIDTYPE = DISPID_A_LAYOUTGRIDTYPE;
        public const int DISPID_IHTMLCURRENTSTYLE_LEFT = STDPROPID_XOBJ_LEFT;
        public const int DISPID_IHTMLCURRENTSTYLE_LETTERSPACING = DISPID_A_LETTERSPACING;
        public const int DISPID_IHTMLCURRENTSTYLE_LINEBREAK = DISPID_A_LINEBREAK;
        public const int DISPID_IHTMLCURRENTSTYLE_LINEHEIGHT = DISPID_A_LINEHEIGHT;
        public const int DISPID_IHTMLCURRENTSTYLE_LISTSTYLEIMAGE = DISPID_A_LISTSTYLEIMAGE;
        public const int DISPID_IHTMLCURRENTSTYLE_LISTSTYLEPOSITION = DISPID_A_LISTSTYLEPOSITION;
        public const int DISPID_IHTMLCURRENTSTYLE_LISTSTYLETYPE = DISPID_A_LISTSTYLETYPE;
        public const int DISPID_IHTMLCURRENTSTYLE_MARGIN = DISPID_A_MARGIN;
        public const int DISPID_IHTMLCURRENTSTYLE_MARGINBOTTOM = DISPID_A_MARGINBOTTOM;
        public const int DISPID_IHTMLCURRENTSTYLE_MARGINLEFT = DISPID_A_MARGINLEFT;
        public const int DISPID_IHTMLCURRENTSTYLE_MARGINRIGHT = DISPID_A_MARGINRIGHT;
        public const int DISPID_IHTMLCURRENTSTYLE_MARGINTOP = DISPID_A_MARGINTOP;
        public const int DISPID_IHTMLCURRENTSTYLE_OVERFLOW = DISPID_A_OVERFLOW;
        public const int DISPID_IHTMLCURRENTSTYLE_OVERFLOWX = DISPID_A_OVERFLOWX;
        public const int DISPID_IHTMLCURRENTSTYLE_OVERFLOWY = DISPID_A_OVERFLOWY;
        public const int DISPID_IHTMLCURRENTSTYLE_PADDING = DISPID_A_PADDING;
        public const int DISPID_IHTMLCURRENTSTYLE_PADDINGBOTTOM = DISPID_A_PADDINGBOTTOM;
        public const int DISPID_IHTMLCURRENTSTYLE_PADDINGLEFT = DISPID_A_PADDINGLEFT;
        public const int DISPID_IHTMLCURRENTSTYLE_PADDINGRIGHT = DISPID_A_PADDINGRIGHT;
        public const int DISPID_IHTMLCURRENTSTYLE_PADDINGTOP = DISPID_A_PADDINGTOP;
        public const int DISPID_IHTMLCURRENTSTYLE_PAGEBREAKAFTER = DISPID_A_PAGEBREAKAFTER;
        public const int DISPID_IHTMLCURRENTSTYLE_PAGEBREAKBEFORE = DISPID_A_PAGEBREAKBEFORE;
        public const int DISPID_IHTMLCURRENTSTYLE_POSITION = DISPID_A_POSITION;
        public const int DISPID_IHTMLCURRENTSTYLE_RIGHT = STDPROPID_XOBJ_RIGHT;
        public const int DISPID_IHTMLCURRENTSTYLE_RUBYALIGN = DISPID_A_RUBYALIGN;
        public const int DISPID_IHTMLCURRENTSTYLE_RUBYOVERHANG = DISPID_A_RUBYOVERHANG;
        public const int DISPID_IHTMLCURRENTSTYLE_RUBYPOSITION = DISPID_A_RUBYPOSITION;
        public const int DISPID_IHTMLCURRENTSTYLE_STYLEFLOAT = DISPID_A_FLOAT;
        public const int DISPID_IHTMLCURRENTSTYLE_TABLELAYOUT = DISPID_A_TABLELAYOUT;
        public const int DISPID_IHTMLCURRENTSTYLE_TEXTALIGN = STDPROPID_XOBJ_BLOCKALIGN;
        public const int DISPID_IHTMLCURRENTSTYLE_TEXTAUTOSPACE = DISPID_A_TEXTAUTOSPACE;
        public const int DISPID_IHTMLCURRENTSTYLE_TEXTDECORATION = DISPID_A_TEXTDECORATION;
        public const int DISPID_IHTMLCURRENTSTYLE_TEXTINDENT = DISPID_A_TEXTINDENT;
        public const int DISPID_IHTMLCURRENTSTYLE_TEXTJUSTIFY = DISPID_A_TEXTJUSTIFY;
        public const int DISPID_IHTMLCURRENTSTYLE_TEXTJUSTIFYTRIM = DISPID_A_TEXTJUSTIFYTRIM;
        public const int DISPID_IHTMLCURRENTSTYLE_TEXTKASHIDA = DISPID_A_TEXTKASHIDA;
        public const int DISPID_IHTMLCURRENTSTYLE_TEXTTRANSFORM = DISPID_A_TEXTTRANSFORM;
        public const int DISPID_IHTMLCURRENTSTYLE_TOP = STDPROPID_XOBJ_TOP;
        public const int DISPID_IHTMLCURRENTSTYLE_UNICODEBIDI = DISPID_A_UNICODEBIDI;
        public const int DISPID_IHTMLCURRENTSTYLE_VERTICALALIGN = DISPID_A_VERTICALALIGN;
        public const int DISPID_IHTMLCURRENTSTYLE_VISIBILITY = DISPID_A_VISIBILITY;
        public const int DISPID_IHTMLCURRENTSTYLE_WIDTH = STDPROPID_XOBJ_WIDTH;
        public const int DISPID_IHTMLCURRENTSTYLE_WORDBREAK = DISPID_A_WORDBREAK;
        public const int DISPID_IHTMLCURRENTSTYLE_ZINDEX = DISPID_A_ZINDEX;
        public const int DISPID_IHTMLCURRENTSTYLE2_FILTER = DISPID_A_FILTER;
        public const int DISPID_IHTMLCURRENTSTYLE2_HASLAYOUT = DISPID_A_HASLAYOUT;
        public const int DISPID_IHTMLCURRENTSTYLE2_ISBLOCK = DISPID_A_ISBLOCK;
        public const int DISPID_IHTMLCURRENTSTYLE2_LAYOUTFLOW = DISPID_A_LAYOUTFLOW;
        public const int DISPID_IHTMLCURRENTSTYLE2_SCROLLBAR3DLIGHTCOLOR = DISPID_A_SCROLLBAR3DLIGHTCOLOR;
        public const int DISPID_IHTMLCURRENTSTYLE2_SCROLLBARARROWCOLOR = DISPID_A_SCROLLBARARROWCOLOR;
        public const int DISPID_IHTMLCURRENTSTYLE2_SCROLLBARBASECOLOR = DISPID_A_SCROLLBARBASECOLOR;
        public const int DISPID_IHTMLCURRENTSTYLE2_SCROLLBARDARKSHADOWCOLOR = DISPID_A_SCROLLBARDARKSHADOWCOLOR;
        public const int DISPID_IHTMLCURRENTSTYLE2_SCROLLBARFACECOLOR = DISPID_A_SCROLLBARFACECOLOR;
        public const int DISPID_IHTMLCURRENTSTYLE2_SCROLLBARHIGHLIGHTCOLOR = DISPID_A_SCROLLBARHIGHLIGHTCOLOR;
        public const int DISPID_IHTMLCURRENTSTYLE2_SCROLLBARSHADOWCOLOR = DISPID_A_SCROLLBARSHADOWCOLOR;
        public const int DISPID_IHTMLCURRENTSTYLE2_SCROLLBARTRACKCOLOR = DISPID_A_SCROLLBARTRACKCOLOR;
        public const int DISPID_IHTMLCURRENTSTYLE2_TEXTALIGNLAST = DISPID_A_TEXTALIGNLAST;
        public const int DISPID_IHTMLCURRENTSTYLE2_TEXTKASHIDASPACE = DISPID_A_TEXTKASHIDASPACE;
        public const int DISPID_IHTMLCURRENTSTYLE2_TEXTUNDERLINEPOSITION = DISPID_A_TEXTUNDERLINEPOSITION;
        public const int DISPID_IHTMLCURRENTSTYLE2_WORDWRAP = DISPID_A_WORDWRAP;
        public const int DISPID_IHTMLCURRENTSTYLE2_WRITINGMODE = DISPID_A_WRITINGMODE;
        public const int DISPID_IHTMLCURRENTSTYLE2_ZOOM = DISPID_A_ZOOM;
        public const int DISPID_IHTMLCURRENTSTYLE3_MINHEIGHT = DISPID_A_MINHEIGHT;
        public const int DISPID_IHTMLCURRENTSTYLE3_TEXTOVERFLOW = DISPID_A_TEXTOVERFLOW;
        public const int DISPID_IHTMLCURRENTSTYLE3_WHITESPACE = DISPID_A_WHITESPACE;
        public const int DISPID_IHTMLCURRENTSTYLE3_WORDSPACING = DISPID_A_WORDSPACING;
        public const int DISPID_IHTMLDOCUMENT3_ATTACHEVENT = DISPID_HTMLOBJECT + 7;
        public const int DISPID_IHTMLDOCUMENT3_BASEURL = DISPID_OMDOCUMENT + 80;
        public const int DISPID_IHTMLDOCUMENT3_CHILDNODES = DISPID_ELEMENT + 49;
        public const int DISPID_IHTMLDOCUMENT3_CREATEDOCUMENTFRAGMENT = DISPID_OMDOCUMENT + 76;
        public const int DISPID_IHTMLDOCUMENT3_CREATETEXTNODE = DISPID_OMDOCUMENT + 74;
        public const int DISPID_IHTMLDOCUMENT3_DETACHEVENT = DISPID_HTMLOBJECT + 8;
        public const int DISPID_IHTMLDOCUMENT3_DIR = DISPID_A_DIR;
        public const int DISPID_IHTMLDOCUMENT3_DOCUMENTELEMENT = DISPID_OMDOCUMENT + 75;
        public const int DISPID_IHTMLDOCUMENT3_ENABLEDOWNLOAD = DISPID_OMDOCUMENT + 79;
        public const int DISPID_IHTMLDOCUMENT3_GETELEMENTBYID = DISPID_OMDOCUMENT + 88;
        public const int DISPID_IHTMLDOCUMENT3_GETELEMENTSBYNAME = DISPID_OMDOCUMENT + 86;
        public const int DISPID_IHTMLDOCUMENT3_GETELEMENTSBYTAGNAME = DISPID_OMDOCUMENT + 87;
        public const int DISPID_IHTMLDOCUMENT3_INHERITSTYLESHEETS = DISPID_OMDOCUMENT + 82;
        public const int DISPID_IHTMLDOCUMENT3_ONBEFOREEDITFOCUS = DISPID_EVPROP_ONBEFOREEDITFOCUS;
        public const int DISPID_IHTMLDOCUMENT3_ONCELLCHANGE = DISPID_EVPROP_ONCELLCHANGE;
        public const int DISPID_IHTMLDOCUMENT3_ONCONTEXTMENU = DISPID_EVPROP_ONCONTEXTMENU;
        public const int DISPID_IHTMLDOCUMENT3_ONDATAAVAILABLE = DISPID_EVPROP_ONDATAAVAILABLE;
        public const int DISPID_IHTMLDOCUMENT3_ONDATASETCHANGED = DISPID_EVPROP_ONDATASETCHANGED;
        public const int DISPID_IHTMLDOCUMENT3_ONDATASETCOMPLETE = DISPID_EVPROP_ONDATASETCOMPLETE;
        public const int DISPID_IHTMLDOCUMENT3_ONPROPERTYCHANGE = DISPID_EVPROP_ONPROPERTYCHANGE;
        public const int DISPID_IHTMLDOCUMENT3_ONROWSDELETE = DISPID_EVPROP_ONROWSDELETE;
        public const int DISPID_IHTMLDOCUMENT3_ONROWSINSERTED = DISPID_EVPROP_ONROWSINSERTED;
        public const int DISPID_IHTMLDOCUMENT3_ONSTOP = DISPID_EVPROP_ONSTOP;
        public const int DISPID_IHTMLDOCUMENT3_PARENTDOCUMENT = DISPID_OMDOCUMENT + 78;
        public const int DISPID_IHTMLDOCUMENT3_RECALC = DISPID_OMDOCUMENT + 73;
        public const int DISPID_IHTMLDOCUMENT3_RELEASECAPTURE = DISPID_OMDOCUMENT + 72;
        public const int DISPID_IHTMLDOCUMENT3_UNIQUEID = DISPID_OMDOCUMENT + 77;
        public const int DISPID_IHTMLDOCUMENT4_CREATEDOCUMENTFROMURL = DISPID_OMDOCUMENT + 92;
        public const int DISPID_IHTMLDOCUMENT4_CREATEEVENTOBJECT = DISPID_OMDOCUMENT + 94;
        public const int DISPID_IHTMLDOCUMENT4_CREATERENDERSTYLE = DISPID_OMDOCUMENT + 96;
        public const int DISPID_IHTMLDOCUMENT4_FIREEVENT = DISPID_OMDOCUMENT + 95;
        public const int DISPID_IHTMLDOCUMENT4_FOCUS = DISPID_OMDOCUMENT + 89;
        public const int DISPID_IHTMLDOCUMENT4_HASFOCUS = DISPID_OMDOCUMENT + 90;
        public const int DISPID_IHTMLDOCUMENT4_MEDIA = DISPID_OMDOCUMENT + 93;
        public const int DISPID_IHTMLDOCUMENT4_NAMESPACES = DISPID_OMDOCUMENT + 91;
        public const int DISPID_IHTMLDOCUMENT4_ONCONTROLSELECT = DISPID_EVPROP_ONCONTROLSELECT;
        public const int DISPID_IHTMLDOCUMENT4_ONSELECTIONCHANGE = DISPID_EVPROP_ONSELECTIONCHANGE;
        public const int DISPID_IHTMLDOCUMENT4_URLUNENCODED = DISPID_OMDOCUMENT + 97;
        public const int DISPID_IHTMLDOCUMENT5_COMPATMODE = DISPID_OMDOCUMENT + 102;
        public const int DISPID_IHTMLDOCUMENT5_CREATEATTRIBUTE = DISPID_OMDOCUMENT + 100;
        public const int DISPID_IHTMLDOCUMENT5_CREATECOMMENT = DISPID_OMDOCUMENT + 101;
        public const int DISPID_IHTMLDOCUMENT5_DOCTYPE = DISPID_OMDOCUMENT + 98;
        public const int DISPID_IHTMLDOCUMENT5_IMPLEMENTATION = DISPID_OMDOCUMENT + 99;
        public const int DISPID_IHTMLDOCUMENT5_ONACTIVATE = DISPID_EVPROP_ONACTIVATE;
        public const int DISPID_IHTMLDOCUMENT5_ONBEFOREACTIVATE = DISPID_EVPROP_ONBEFOREACTIVATE;
        public const int DISPID_IHTMLDOCUMENT5_ONBEFOREDEACTIVATE = DISPID_EVPROP_ONBEFOREDEACTIVATE;
        public const int DISPID_IHTMLDOCUMENT5_ONDEACTIVATE = DISPID_EVPROP_ONDEACTIVATE;
        public const int DISPID_IHTMLDOCUMENT5_ONFOCUSIN = DISPID_EVPROP_ONFOCUSIN;
        public const int DISPID_IHTMLDOCUMENT5_ONFOCUSOUT = DISPID_EVPROP_ONFOCUSOUT;
        public const int DISPID_IHTMLDOCUMENT5_ONMOUSEWHEEL = DISPID_EVPROP_ONMOUSEWHEEL;
        public const int DISPID_IHTMLDOMATTRIBUTE_NODENAME = DISPID_DOMATTRIBUTE;
        public const int DISPID_IHTMLDOMATTRIBUTE_NODEVALUE = DISPID_DOMATTRIBUTE + 2;
        public const int DISPID_IHTMLDOMATTRIBUTE_SPECIFIED = DISPID_DOMATTRIBUTE + 1;
        public const int DISPID_IHTMLDOMATTRIBUTE2_APPENDCHILD = DISPID_DOMATTRIBUTE + 18;
        public const int DISPID_IHTMLDOMATTRIBUTE2_ATTRIBUTES = DISPID_DOMATTRIBUTE + 13;
        public const int DISPID_IHTMLDOMATTRIBUTE2_CHILDNODES = DISPID_DOMATTRIBUTE + 8;
        public const int DISPID_IHTMLDOMATTRIBUTE2_CLONENODE = DISPID_DOMATTRIBUTE + 20;
        public const int DISPID_IHTMLDOMATTRIBUTE2_EXPANDO = DISPID_DOMATTRIBUTE + 5;
        public const int DISPID_IHTMLDOMATTRIBUTE2_FIRSTCHILD = DISPID_DOMATTRIBUTE + 9;
        public const int DISPID_IHTMLDOMATTRIBUTE2_HASCHILDNODES = DISPID_DOMATTRIBUTE + 19;
        public const int DISPID_IHTMLDOMATTRIBUTE2_INSERTBEFORE = DISPID_DOMATTRIBUTE + 15;
        public const int DISPID_IHTMLDOMATTRIBUTE2_LASTCHILD = DISPID_DOMATTRIBUTE + 10;
        public const int DISPID_IHTMLDOMATTRIBUTE2_NAME = DISPID_DOMATTRIBUTE + 3;
        public const int DISPID_IHTMLDOMATTRIBUTE2_NEXTSIBLING = DISPID_DOMATTRIBUTE + 12;
        public const int DISPID_IHTMLDOMATTRIBUTE2_NODETYPE = DISPID_DOMATTRIBUTE + 6;
        public const int DISPID_IHTMLDOMATTRIBUTE2_OWNERDOCUMENT = DISPID_DOMATTRIBUTE + 14;
        public const int DISPID_IHTMLDOMATTRIBUTE2_PARENTNODE = DISPID_DOMATTRIBUTE + 7;
        public const int DISPID_IHTMLDOMATTRIBUTE2_PREVIOUSSIBLING = DISPID_DOMATTRIBUTE + 11;
        public const int DISPID_IHTMLDOMATTRIBUTE2_REMOVECHILD = DISPID_DOMATTRIBUTE + 17;
        public const int DISPID_IHTMLDOMATTRIBUTE2_REPLACECHILD = DISPID_DOMATTRIBUTE + 16;
        public const int DISPID_IHTMLDOMATTRIBUTE2_VALUE = DISPID_DOMATTRIBUTE + 4;
        public const int DISPID_IHTMLDOMCHILDRENCOLLECTION__NEWENUM = DISPID_NEWENUM;
        public const int DISPID_IHTMLDOMCHILDRENCOLLECTION_ITEM = DISPID_VALUE;
        public const int DISPID_IHTMLDOMCHILDRENCOLLECTION_LENGTH = DISPID_COLLECTION;
        public const int DISPID_IHTMLDOMNODE_APPENDCHILD = DISPID_ELEMENT + 73;
        public const int DISPID_IHTMLDOMNODE_ATTRIBUTES = DISPID_ELEMENT + 50;
        public const int DISPID_IHTMLDOMNODE_CHILDNODES = DISPID_ELEMENT + 49;
        public const int DISPID_IHTMLDOMNODE_CLONENODE = DISPID_ELEMENT + 61;
        public const int DISPID_IHTMLDOMNODE_FIRSTCHILD = DISPID_ELEMENT + 76;
        public const int DISPID_IHTMLDOMNODE_HASCHILDNODES = DISPID_ELEMENT + 48;
        public const int DISPID_IHTMLDOMNODE_INSERTBEFORE = DISPID_ELEMENT + 51;
        public const int DISPID_IHTMLDOMNODE_LASTCHILD = DISPID_ELEMENT + 77;
        public const int DISPID_IHTMLDOMNODE_NEXTSIBLING = DISPID_ELEMENT + 79;
        public const int DISPID_IHTMLDOMNODE_NODENAME = DISPID_ELEMENT + 74;
        public const int DISPID_IHTMLDOMNODE_NODETYPE = DISPID_ELEMENT + 46;
        public const int DISPID_IHTMLDOMNODE_NODEVALUE = DISPID_ELEMENT + 75;
        public const int DISPID_IHTMLDOMNODE_PARENTNODE = DISPID_ELEMENT + 47;
        public const int DISPID_IHTMLDOMNODE_PREVIOUSSIBLING = DISPID_ELEMENT + 78;
        public const int DISPID_IHTMLDOMNODE_REMOVECHILD = DISPID_ELEMENT + 52;
        public const int DISPID_IHTMLDOMNODE_REMOVENODE = DISPID_ELEMENT + 66;
        public const int DISPID_IHTMLDOMNODE_REPLACECHILD = DISPID_ELEMENT + 53;
        public const int DISPID_IHTMLDOMNODE_REPLACENODE = DISPID_ELEMENT + 67;
        public const int DISPID_IHTMLDOMNODE_SWAPNODE = DISPID_ELEMENT + 68;
        public const int DISPID_IHTMLDOMTEXTNODE_DATA = DISPID_DOMTEXTNODE;
        public const int DISPID_IHTMLDOMTEXTNODE_LENGTH = DISPID_DOMTEXTNODE + 2;
        public const int DISPID_IHTMLDOMTEXTNODE_SPLITTEXT = DISPID_DOMTEXTNODE + 3;
        public const int DISPID_IHTMLDOMTEXTNODE_TOSTRING = DISPID_DOMTEXTNODE + 1;
        public const int DISPID_IHTMLDOMTEXTNODE2_APPENDDATA = DISPID_DOMTEXTNODE + 5;
        public const int DISPID_IHTMLDOMTEXTNODE2_DELETEDATA = DISPID_DOMTEXTNODE + 7;
        public const int DISPID_IHTMLDOMTEXTNODE2_INSERTDATA = DISPID_DOMTEXTNODE + 6;
        public const int DISPID_IHTMLDOMTEXTNODE2_REPLACEDATA = DISPID_DOMTEXTNODE + 8;
        public const int DISPID_IHTMLDOMTEXTNODE2_SUBSTRINGDATA = DISPID_DOMTEXTNODE + 4;
        public const int DISPID_IHTMLELEMENT_ALL = DISPID_ELEMENT + 38;
        public const int DISPID_IHTMLELEMENT_CHILDREN = DISPID_ELEMENT + 37;
        public const int DISPID_IHTMLELEMENT_CLASSNAME = DISPID_ELEMENT + 1;
        public const int DISPID_IHTMLELEMENT_CLICK = DISPID_ELEMENT + 33;
        public const int DISPID_IHTMLELEMENT_CONTAINS = DISPID_ELEMENT + 20;
        public const int DISPID_IHTMLELEMENT_DOCUMENT = DISPID_ELEMENT + 18;
        public const int DISPID_IHTMLELEMENT_FILTERS = DISPID_ELEMENT + 35;
        public const int DISPID_IHTMLELEMENT_GETATTRIBUTE = DISPID_HTMLOBJECT + 2;
        public const int DISPID_IHTMLELEMENT_ID = DISPID_ELEMENT + 2;
        public const int DISPID_IHTMLELEMENT_INNERHTML = DISPID_ELEMENT + 26;
        public const int DISPID_IHTMLELEMENT_INNERTEXT = DISPID_ELEMENT + 27;
        public const int DISPID_IHTMLELEMENT_INSERTADJACENTHTML = DISPID_ELEMENT + 30;
        public const int DISPID_IHTMLELEMENT_INSERTADJACENTTEXT = DISPID_ELEMENT + 31;
        public const int DISPID_IHTMLELEMENT_ISTEXTEDIT = DISPID_ELEMENT + 34;
        public const int DISPID_IHTMLELEMENT_LANG = DISPID_A_LANG;
        public const int DISPID_IHTMLELEMENT_LANGUAGE = DISPID_A_LANGUAGE;
        public const int DISPID_IHTMLELEMENT_OFFSETHEIGHT = DISPID_ELEMENT + 11;
        public const int DISPID_IHTMLELEMENT_OFFSETLEFT = DISPID_ELEMENT + 8;
        public const int DISPID_IHTMLELEMENT_OFFSETPARENT = DISPID_ELEMENT + 12;
        public const int DISPID_IHTMLELEMENT_OFFSETTOP = DISPID_ELEMENT + 9;
        public const int DISPID_IHTMLELEMENT_OFFSETWIDTH = DISPID_ELEMENT + 10;
        public const int DISPID_IHTMLELEMENT_ONAFTERUPDATE = DISPID_EVPROP_ONAFTERUPDATE;
        public const int DISPID_IHTMLELEMENT_ONBEFOREUPDATE = DISPID_EVPROP_ONBEFOREUPDATE;
        public const int DISPID_IHTMLELEMENT_ONCLICK = DISPID_EVPROP_ONCLICK; //-2147412103
        public const int DISPID_IHTMLELEMENT_ONDATAAVAILABLE = DISPID_EVPROP_ONDATAAVAILABLE;
        public const int DISPID_IHTMLELEMENT_ONDATASETCHANGED = DISPID_EVPROP_ONDATASETCHANGED;
        public const int DISPID_IHTMLELEMENT_ONDATASETCOMPLETE = DISPID_EVPROP_ONDATASETCOMPLETE;
        public const int DISPID_IHTMLELEMENT_ONDBLCLICK = DISPID_EVPROP_ONDBLCLICK; //-2147412102
        public const int DISPID_IHTMLELEMENT_ONDRAGSTART = DISPID_EVPROP_ONDRAGSTART;
        public const int DISPID_IHTMLELEMENT_ONERRORUPDATE = DISPID_EVPROP_ONERRORUPDATE;
        public const int DISPID_IHTMLELEMENT_ONFILTERCHANGE = DISPID_EVPROP_ONFILTER;
        public const int DISPID_IHTMLELEMENT_ONHELP = DISPID_EVPROP_ONHELP; //-2147412098
        public const int DISPID_IHTMLELEMENT_ONKEYDOWN = DISPID_EVPROP_ONKEYDOWN; //-2147412106
        public const int DISPID_IHTMLELEMENT_ONKEYPRESS = DISPID_EVPROP_ONKEYPRESS; //-2147412104
        public const int DISPID_IHTMLELEMENT_ONKEYUP = DISPID_EVPROP_ONKEYUP;
        public const int DISPID_IHTMLELEMENT_ONMOUSEDOWN = DISPID_EVPROP_ONMOUSEDOWN;
        public const int DISPID_IHTMLELEMENT_ONMOUSEMOVE = DISPID_EVPROP_ONMOUSEMOVE; // -2147412107
        public const int DISPID_IHTMLELEMENT_ONMOUSEOUT = DISPID_EVPROP_ONMOUSEOUT; //-2147412110
        public const int DISPID_IHTMLELEMENT_ONMOUSEOVER = DISPID_EVPROP_ONMOUSEOVER; //-2147412111
        public const int DISPID_IHTMLELEMENT_ONMOUSEUP = DISPID_EVPROP_ONMOUSEUP;
        public const int DISPID_IHTMLELEMENT_ONROWENTER = DISPID_EVPROP_ONROWENTER;
        public const int DISPID_IHTMLELEMENT_ONROWEXIT = DISPID_EVPROP_ONROWEXIT;
        public const int DISPID_IHTMLELEMENT_ONSELECTSTART = DISPID_EVPROP_ONSELECTSTART;
        public const int DISPID_IHTMLELEMENT_OUTERHTML = DISPID_ELEMENT + 28;
        public const int DISPID_IHTMLELEMENT_OUTERTEXT = DISPID_ELEMENT + 29;
        public const int DISPID_IHTMLELEMENT_PARENTELEMENT = STDPROPID_XOBJ_PARENT;
        public const int DISPID_IHTMLELEMENT_PARENTTEXTEDIT = DISPID_ELEMENT + 32;
        public const int DISPID_IHTMLELEMENT_RECORDNUMBER = DISPID_ELEMENT + 25;
        public const int DISPID_IHTMLELEMENT_REMOVEATTRIBUTE = DISPID_HTMLOBJECT + 3;
        public const int DISPID_IHTMLELEMENT_SCROLLINTOVIEW = DISPID_ELEMENT + 19;
        public const int DISPID_IHTMLELEMENT_SETATTRIBUTE = DISPID_HTMLOBJECT + 1;
        public const int DISPID_IHTMLELEMENT_SOURCEINDEX = DISPID_ELEMENT + 24;
        public const int DISPID_IHTMLELEMENT_STYLE = STDPROPID_XOBJ_STYLE;
        public const int DISPID_IHTMLELEMENT_TAGNAME = DISPID_ELEMENT + 4;
        public const int DISPID_IHTMLELEMENT_TITLE = STDPROPID_XOBJ_CONTROLTIPTEXT;
        public const int DISPID_IHTMLELEMENT_TOSTRING = DISPID_ELEMENT + 36;
        public const int DISPID_IHTMLELEMENT2_ACCESSKEY = DISPID_SITE + 5;
        public const int DISPID_IHTMLELEMENT2_ADDBEHAVIOR = DISPID_ELEMENT + 80;
        public const int DISPID_IHTMLELEMENT2_ADDFILTER = DISPID_SITE + 17;
        public const int DISPID_IHTMLELEMENT2_APPLYELEMENT = DISPID_ELEMENT + 65;
        public const int DISPID_IHTMLELEMENT2_ATTACHEVENT = DISPID_HTMLOBJECT + 7;
        public const int DISPID_IHTMLELEMENT2_BEHAVIORURNS = DISPID_ELEMENT + 82;
        public const int DISPID_IHTMLELEMENT2_BLUR = DISPID_SITE + 2;
        public const int DISPID_IHTMLELEMENT2_CANHAVECHILDREN = DISPID_ELEMENT + 72;
        public const int DISPID_IHTMLELEMENT2_CLEARATTRIBUTES = DISPID_ELEMENT + 62;
        public const int DISPID_IHTMLELEMENT2_CLIENTHEIGHT = DISPID_SITE + 19;
        public const int DISPID_IHTMLELEMENT2_CLIENTLEFT = DISPID_SITE + 22;
        public const int DISPID_IHTMLELEMENT2_CLIENTTOP = DISPID_SITE + 21;
        public const int DISPID_IHTMLELEMENT2_CLIENTWIDTH = DISPID_SITE + 20;
        public const int DISPID_IHTMLELEMENT2_COMPONENTFROMPOINT = DISPID_ELEMENT + 42;
        public const int DISPID_IHTMLELEMENT2_CREATECONTROLRANGE = DISPID_ELEMENT + 56;
        public const int DISPID_IHTMLELEMENT2_CURRENTSTYLE = DISPID_ELEMENT + 7;
        public const int DISPID_IHTMLELEMENT2_DETACHEVENT = DISPID_HTMLOBJECT + 8;
        public const int DISPID_IHTMLELEMENT2_DOSCROLL = DISPID_ELEMENT + 43;
        public const int DISPID_IHTMLELEMENT2_FOCUS = DISPID_SITE + 0;
        public const int DISPID_IHTMLELEMENT2_GETADJACENTTEXT = DISPID_ELEMENT + 70;
        public const int DISPID_IHTMLELEMENT2_GETBOUNDINGCLIENTRECT = DISPID_ELEMENT + 45;
        public const int DISPID_IHTMLELEMENT2_GETCLIENTRECTS = DISPID_ELEMENT + 44;
        public const int DISPID_IHTMLELEMENT2_GETELEMENTSBYTAGNAME = DISPID_ELEMENT + 85;
        public const int DISPID_IHTMLELEMENT2_GETEXPRESSION = DISPID_HTMLOBJECT + 5;
        public const int DISPID_IHTMLELEMENT2_INSERTADJACENTELEMENT = DISPID_ELEMENT + 69;
        public const int DISPID_IHTMLELEMENT2_MERGEATTRIBUTES = DISPID_ELEMENT + 63;
        public const int DISPID_IHTMLELEMENT2_ONBEFORECOPY = DISPID_EVPROP_ONBEFORECOPY;
        public const int DISPID_IHTMLELEMENT2_ONBEFORECUT = DISPID_EVPROP_ONBEFORECUT;
        public const int DISPID_IHTMLELEMENT2_ONBEFOREEDITFOCUS = DISPID_EVPROP_ONBEFOREEDITFOCUS;
        public const int DISPID_IHTMLELEMENT2_ONBEFOREPASTE = DISPID_EVPROP_ONBEFOREPASTE;
        public const int DISPID_IHTMLELEMENT2_ONBLUR = DISPID_EVPROP_ONBLUR;
        public const int DISPID_IHTMLELEMENT2_ONCELLCHANGE = DISPID_EVPROP_ONCELLCHANGE;
        public const int DISPID_IHTMLELEMENT2_ONCONTEXTMENU = DISPID_EVPROP_ONCONTEXTMENU;
        public const int DISPID_IHTMLELEMENT2_ONCOPY = DISPID_EVPROP_ONCOPY;
        public const int DISPID_IHTMLELEMENT2_ONCUT = DISPID_EVPROP_ONCUT;
        public const int DISPID_IHTMLELEMENT2_ONDRAG = DISPID_EVPROP_ONDRAG;
        public const int DISPID_IHTMLELEMENT2_ONDRAGEND = DISPID_EVPROP_ONDRAGEND;
        public const int DISPID_IHTMLELEMENT2_ONDRAGENTER = DISPID_EVPROP_ONDRAGENTER;
        public const int DISPID_IHTMLELEMENT2_ONDRAGLEAVE = DISPID_EVPROP_ONDRAGLEAVE;
        public const int DISPID_IHTMLELEMENT2_ONDRAGOVER = DISPID_EVPROP_ONDRAGOVER;
        public const int DISPID_IHTMLELEMENT2_ONDROP = DISPID_EVPROP_ONDROP;
        public const int DISPID_IHTMLELEMENT2_ONFOCUS = DISPID_EVPROP_ONFOCUS;
        public const int DISPID_IHTMLELEMENT2_ONLOSECAPTURE = DISPID_EVPROP_ONLOSECAPTURE;
        public const int DISPID_IHTMLELEMENT2_ONPASTE = DISPID_EVPROP_ONPASTE;
        public const int DISPID_IHTMLELEMENT2_ONPROPERTYCHANGE = DISPID_EVPROP_ONPROPERTYCHANGE;
        //public const int  DISPID_IHTMLELEMENT2_READYSTATE  = DISPID_A_READYSTATE;
        public const int DISPID_IHTMLELEMENT2_ONREADYSTATECHANGE = DISPID_EVPROP_ONREADYSTATECHANGE;
        public const int DISPID_IHTMLELEMENT2_ONRESIZE = DISPID_EVPROP_ONRESIZE;
        public const int DISPID_IHTMLELEMENT2_ONROWSDELETE = DISPID_EVPROP_ONROWSDELETE;
        public const int DISPID_IHTMLELEMENT2_ONROWSINSERTED = DISPID_EVPROP_ONROWSINSERTED;
        public const int DISPID_IHTMLELEMENT2_ONSCROLL = DISPID_EVPROP_ONSCROLL;
        public const int DISPID_IHTMLELEMENT2_READYSTATEVALUE = DISPID_ELEMENT + 84;
        public const int DISPID_IHTMLELEMENT2_RELEASECAPTURE = DISPID_ELEMENT + 41;
        public const int DISPID_IHTMLELEMENT2_REMOVEBEHAVIOR = DISPID_ELEMENT + 81;
        public const int DISPID_IHTMLELEMENT2_REMOVEEXPRESSION = DISPID_HTMLOBJECT + 6;
        public const int DISPID_IHTMLELEMENT2_REMOVEFILTER = DISPID_SITE + 18;
        public const int DISPID_IHTMLELEMENT2_REPLACEADJACENTTEXT = DISPID_ELEMENT + 71;
        public const int DISPID_IHTMLELEMENT2_RUNTIMESTYLE = DISPID_ELEMENT + 64;
        public const int DISPID_IHTMLELEMENT2_SCOPENAME = DISPID_ELEMENT + 39;
        public const int DISPID_IHTMLELEMENT2_SCROLLHEIGHT = DISPID_ELEMENT + 57;
        public const int DISPID_IHTMLELEMENT2_SCROLLLEFT = DISPID_ELEMENT + 60;
        public const int DISPID_IHTMLELEMENT2_SCROLLTOP = DISPID_ELEMENT + 59;
        public const int DISPID_IHTMLELEMENT2_SCROLLWIDTH = DISPID_ELEMENT + 58;
        public const int DISPID_IHTMLELEMENT2_SETCAPTURE = DISPID_ELEMENT + 40;
        public const int DISPID_IHTMLELEMENT2_SETEXPRESSION = DISPID_HTMLOBJECT + 4;
        public const int DISPID_IHTMLELEMENT2_TAGURN = DISPID_ELEMENT + 83;
        public const int DISPID_IHTMLELEMENTCOLLECTION__NEWENUM = DISPID_NEWENUM;
        public const int DISPID_IHTMLELEMENTCOLLECTION_ITEM = DISPID_VALUE;
        public const int DISPID_IHTMLELEMENTCOLLECTION_LENGTH = DISPID_COLLECTION;
        public const int DISPID_IHTMLELEMENTCOLLECTION_TAGS = DISPID_COLLECTION + 2;
        public const int DISPID_IHTMLELEMENTCOLLECTION_TOSTRING = DISPID_COLLECTION + 1;
        //    DISPIDs for interface IHTMLEventObj

        public const int DISPID_IHTMLEVENTOBJ_ALTKEY = DISPID_EVENTOBJ + 2;
        public const int DISPID_IHTMLEVENTOBJ_BUTTON = DISPID_EVENTOBJ + 12;
        public const int DISPID_IHTMLEVENTOBJ_CANCELBUBBLE = DISPID_EVENTOBJ + 8;
        public const int DISPID_IHTMLEVENTOBJ_CLIENTX = DISPID_EVENTOBJ + 20;
        public const int DISPID_IHTMLEVENTOBJ_CLIENTY = DISPID_EVENTOBJ + 21;
        public const int DISPID_IHTMLEVENTOBJ_CTRLKEY = DISPID_EVENTOBJ + 3;
        public const int DISPID_IHTMLEVENTOBJ_FROMELEMENT = DISPID_EVENTOBJ + 9;
        public const int DISPID_IHTMLEVENTOBJ_KEYCODE = DISPID_EVENTOBJ + 11;
        public const int DISPID_IHTMLEVENTOBJ_OFFSETX = DISPID_EVENTOBJ + 22;
        public const int DISPID_IHTMLEVENTOBJ_OFFSETY = DISPID_EVENTOBJ + 23;
        public const int DISPID_IHTMLEVENTOBJ_QUALIFIER = DISPID_EVENTOBJ + 14;
        public const int DISPID_IHTMLEVENTOBJ_REASON = DISPID_EVENTOBJ + 15;
        public const int DISPID_IHTMLEVENTOBJ_RETURNVALUE = DISPID_EVENTOBJ + 7;
        public const int DISPID_IHTMLEVENTOBJ_SCREENX = DISPID_EVENTOBJ + 24;
        public const int DISPID_IHTMLEVENTOBJ_SCREENY = DISPID_EVENTOBJ + 25;
        public const int DISPID_IHTMLEVENTOBJ_SHIFTKEY = DISPID_EVENTOBJ + 4;
        public const int DISPID_IHTMLEVENTOBJ_SRCELEMENT = DISPID_EVENTOBJ + 1;
        public const int DISPID_IHTMLEVENTOBJ_SRCFILTER = DISPID_EVENTOBJ + 26;
        public const int DISPID_IHTMLEVENTOBJ_TOELEMENT = DISPID_EVENTOBJ + 10;
        public const int DISPID_IHTMLEVENTOBJ_TYPE = DISPID_EVENTOBJ + 13;
        public const int DISPID_IHTMLEVENTOBJ_X = DISPID_EVENTOBJ + 5;
        public const int DISPID_IHTMLEVENTOBJ_Y = DISPID_EVENTOBJ + 6;
        public const int DISPID_IHTMLEVENTOBJ2_ALTKEY = DISPID_EVENTOBJ + 2;
        //    DISPIDs for interface IHTMLEventObj2

        public const int DISPID_IHTMLEVENTOBJ2_BOOKMARKS = DISPID_EVENTOBJ + 31;
        public const int DISPID_IHTMLEVENTOBJ2_BOUNDELEMENTS = DISPID_EVENTOBJ + 34;
        public const int DISPID_IHTMLEVENTOBJ2_BUTTON = DISPID_EVENTOBJ + 12;
        public const int DISPID_IHTMLEVENTOBJ2_CLIENTX = DISPID_EVENTOBJ + 20;
        public const int DISPID_IHTMLEVENTOBJ2_CLIENTY = DISPID_EVENTOBJ + 21;
        public const int DISPID_IHTMLEVENTOBJ2_CTRLKEY = DISPID_EVENTOBJ + 3;
        public const int DISPID_IHTMLEVENTOBJ2_DATAFLD = DISPID_EVENTOBJ + 33;
        public const int DISPID_IHTMLEVENTOBJ2_DATATRANSFER = DISPID_EVENTOBJ + 37;
        public const int DISPID_IHTMLEVENTOBJ2_FROMELEMENT = DISPID_EVENTOBJ + 9;
        public const int DISPID_IHTMLEVENTOBJ2_GETATTRIBUTE = DISPID_HTMLOBJECT + 2;
        public const int DISPID_IHTMLEVENTOBJ2_OFFSETX = DISPID_EVENTOBJ + 22;
        public const int DISPID_IHTMLEVENTOBJ2_OFFSETY = DISPID_EVENTOBJ + 23;
        public const int DISPID_IHTMLEVENTOBJ2_PROPERTYNAME = DISPID_EVENTOBJ + 27;
        public const int DISPID_IHTMLEVENTOBJ2_QUALIFIER = DISPID_EVENTOBJ + 14;
        public const int DISPID_IHTMLEVENTOBJ2_REASON = DISPID_EVENTOBJ + 15;
        public const int DISPID_IHTMLEVENTOBJ2_RECORDSET = DISPID_EVENTOBJ + 32;
        public const int DISPID_IHTMLEVENTOBJ2_REMOVEATTRIBUTE = DISPID_HTMLOBJECT + 3;
        public const int DISPID_IHTMLEVENTOBJ2_REPEAT = DISPID_EVENTOBJ + 35;
        public const int DISPID_IHTMLEVENTOBJ2_SCREENX = DISPID_EVENTOBJ + 24;
        public const int DISPID_IHTMLEVENTOBJ2_SCREENY = DISPID_EVENTOBJ + 25;
        public const int DISPID_IHTMLEVENTOBJ2_SETATTRIBUTE = DISPID_HTMLOBJECT + 1;
        public const int DISPID_IHTMLEVENTOBJ2_SHIFTKEY = DISPID_EVENTOBJ + 4;
        public const int DISPID_IHTMLEVENTOBJ2_SRCELEMENT = DISPID_EVENTOBJ + 1;
        public const int DISPID_IHTMLEVENTOBJ2_SRCFILTER = DISPID_EVENTOBJ + 26;
        public const int DISPID_IHTMLEVENTOBJ2_SRCURN = DISPID_EVENTOBJ + 36;
        public const int DISPID_IHTMLEVENTOBJ2_TOELEMENT = DISPID_EVENTOBJ + 10;
        public const int DISPID_IHTMLEVENTOBJ2_TYPE = DISPID_EVENTOBJ + 13;
        public const int DISPID_IHTMLEVENTOBJ2_X = DISPID_EVENTOBJ + 5;
        public const int DISPID_IHTMLEVENTOBJ2_Y = DISPID_EVENTOBJ + 6;
        public const int DISPID_IHTMLEVENTOBJ3_ALTLEFT = DISPID_EVENTOBJ + 40;
        public const int DISPID_IHTMLEVENTOBJ3_BEHAVIORCOOKIE = DISPID_EVENTOBJ + 48;
        public const int DISPID_IHTMLEVENTOBJ3_BEHAVIORPART = DISPID_EVENTOBJ + 49;
        public const int DISPID_IHTMLEVENTOBJ3_CONTENTOVERFLOW = DISPID_EVENTOBJ + 38;
        public const int DISPID_IHTMLEVENTOBJ3_CTRLLEFT = DISPID_EVENTOBJ + 41;
        public const int DISPID_IHTMLEVENTOBJ3_IMECOMPOSITIONCHANGE = DISPID_EVENTOBJ + 42;
        public const int DISPID_IHTMLEVENTOBJ3_IMENOTIFYCOMMAND = DISPID_EVENTOBJ + 43;
        public const int DISPID_IHTMLEVENTOBJ3_IMENOTIFYDATA = DISPID_EVENTOBJ + 44;
        public const int DISPID_IHTMLEVENTOBJ3_IMEREQUEST = DISPID_EVENTOBJ + 46;
        public const int DISPID_IHTMLEVENTOBJ3_IMEREQUESTDATA = DISPID_EVENTOBJ + 47;
        public const int DISPID_IHTMLEVENTOBJ3_KEYBOARDLAYOUT = DISPID_EVENTOBJ + 45;
        public const int DISPID_IHTMLEVENTOBJ3_NEXTPAGE = DISPID_EVENTOBJ + 50;
        public const int DISPID_IHTMLEVENTOBJ3_SHIFTLEFT = DISPID_EVENTOBJ + 39;
        public const int DISPID_IHTMLEVENTOBJ4_WHEELDELTA = DISPID_EVENTOBJ + 51;
        public const int DISPID_IHTMLFORMELEMENT__NEWENUM = DISPID_NEWENUM;
        public const int DISPID_IHTMLFORMELEMENT_ACTION = DISPID_FORM + 1;
        public const int DISPID_IHTMLFORMELEMENT_DIR = DISPID_A_DIR;
        public const int DISPID_IHTMLFORMELEMENT_ELEMENTS = DISPID_FORM + 5;
        public const int DISPID_IHTMLFORMELEMENT_ENCODING = DISPID_FORM + 3;
        public const int DISPID_IHTMLFORMELEMENT_ITEM = DISPID_VALUE;
        public const int DISPID_IHTMLFORMELEMENT_LENGTH = DISPID_COLLECTION;
        public const int DISPID_IHTMLFORMELEMENT_METHOD = DISPID_FORM + 4;
        public const int DISPID_IHTMLFORMELEMENT_NAME = STDPROPID_XOBJ_NAME;
        public const int DISPID_IHTMLFORMELEMENT_ONRESET = DISPID_EVPROP_ONRESET;
        public const int DISPID_IHTMLFORMELEMENT_ONSUBMIT = DISPID_EVPROP_ONSUBMIT;
        public const int DISPID_IHTMLFORMELEMENT_RESET = DISPID_FORM + 10;
        public const int DISPID_IHTMLFORMELEMENT_SUBMIT = DISPID_FORM + 9;
        public const int DISPID_IHTMLFORMELEMENT_TAGS = DISPID_COLLECTION + 2;
        public const int DISPID_IHTMLFORMELEMENT_TARGET = DISPID_FORM + 6;
        public const int DISPID_IHTMLFRAMEBASE_BORDER = DISPID_FRAMESITE + 2;
        public const int DISPID_IHTMLFRAMEBASE_FRAMEBORDER = DISPID_FRAMESITE + 3;
        public const int DISPID_IHTMLFRAMEBASE_FRAMESPACING = DISPID_FRAMESITE + 4;
        public const int DISPID_IHTMLFRAMEBASE_MARGINHEIGHT = DISPID_FRAMESITE + 6;
        public const int DISPID_IHTMLFRAMEBASE_MARGINWIDTH = DISPID_FRAMESITE + 5;
        public const int DISPID_IHTMLFRAMEBASE_NAME = STDPROPID_XOBJ_NAME;
        public const int DISPID_IHTMLFRAMEBASE_NORESIZE = DISPID_FRAMESITE + 7;
        public const int DISPID_IHTMLFRAMEBASE_SCROLLING = DISPID_FRAMESITE + 8;
        public const int DISPID_IHTMLFRAMEBASE_SRC = DISPID_FRAMESITE + 0;
        public const int DISPID_IHTMLFRAMEBASE2_ALLOWTRANSPARENCY = DISPID_A_ALLOWTRANSPARENCY;
        public const int DISPID_IHTMLFRAMEBASE2_CONTENTWINDOW = DISPID_FRAMESITE + 9;
        public const int DISPID_IHTMLFRAMEBASE2_ONLOAD = DISPID_EVPROP_ONLOAD;
        public const int DISPID_IHTMLFRAMEBASE2_ONREADYSTATECHANGE = DISPID_EVPROP_ONREADYSTATECHANGE;
        public const int DISPID_IHTMLFRAMEBASE2_READYSTATE = DISPID_A_READYSTATE;
        //    DISPIDs for interface IHTMLFramesCollection2

        public const int DISPID_IHTMLFRAMESCOLLECTION2_ITEM = 0;
        public const int DISPID_IHTMLFRAMESCOLLECTION2_LENGTH = 1001;
        public const int DISPID_IHTMLFRAMESETELEMENT_BORDER = DISPID_FRAMESET + 2;
        public const int DISPID_IHTMLFRAMESETELEMENT_BORDERCOLOR = DISPID_FRAMESET + 3;
        public const int DISPID_IHTMLFRAMESETELEMENT_COLS = DISPID_FRAMESET + 1;
        public const int DISPID_IHTMLFRAMESETELEMENT_FRAMEBORDER = DISPID_FRAMESET + 4;
        public const int DISPID_IHTMLFRAMESETELEMENT_FRAMESPACING = DISPID_FRAMESET + 5;
        public const int DISPID_IHTMLFRAMESETELEMENT_NAME = STDPROPID_XOBJ_NAME;
        public const int DISPID_IHTMLFRAMESETELEMENT_ONBEFOREUNLOAD = DISPID_EVPROP_ONBEFOREUNLOAD;
        public const int DISPID_IHTMLFRAMESETELEMENT_ONLOAD = DISPID_EVPROP_ONLOAD;
        public const int DISPID_IHTMLFRAMESETELEMENT_ONUNLOAD = DISPID_EVPROP_ONUNLOAD;
        public const int DISPID_IHTMLFRAMESETELEMENT_ROWS = DISPID_FRAMESET;
        public const int DISPID_IHTMLHEADELEMENT_PROFILE = DISPID_HEDELEMS + 1;
        public const int DISPID_IHTMLHEADERELEMENT_ALIGN = STDPROPID_XOBJ_BLOCKALIGN;
        public const int DISPID_IHTMLHRELEMENT_ALIGN = STDPROPID_XOBJ_BLOCKALIGN;
        public const int DISPID_IHTMLHRELEMENT_COLOR = DISPID_A_COLOR;
        public const int DISPID_IHTMLHRELEMENT_NOSHADE = DISPID_HR + 1;
        public const int DISPID_IHTMLHRELEMENT_SIZE = STDPROPID_XOBJ_HEIGHT;
        public const int DISPID_IHTMLHRELEMENT_WIDTH = STDPROPID_XOBJ_WIDTH;
        public const int DISPID_IHTMLIFRAMEELEMENT_ALIGN = STDPROPID_XOBJ_CONTROLALIGN;
        public const int DISPID_IHTMLIFRAMEELEMENT_HSPACE = DISPID_IFRAME + 2;
        public const int DISPID_IHTMLIFRAMEELEMENT_VSPACE = DISPID_IFRAME + 1;
        public const int DISPID_IHTMLIFRAMEELEMENT2_HEIGHT = STDPROPID_XOBJ_HEIGHT;
        public const int DISPID_IHTMLIFRAMEELEMENT2_WIDTH = STDPROPID_XOBJ_WIDTH;
        //    DISPIDs for interface IHTMLImageElementFactory
        public const int DISPID_IHTMLIMAGEELEMENTFACTORY_CREATE = DISPID_VALUE;
        public const int DISPID_IHTMLIMGELEMENT_ALIGN = STDPROPID_XOBJ_CONTROLALIGN;
        public const int DISPID_IHTMLIMGELEMENT_ALT = DISPID_IMGBASE + 2;
        public const int DISPID_IHTMLIMGELEMENT_BORDER = DISPID_IMGBASE + 4;
        public const int DISPID_IHTMLIMGELEMENT_COMPLETE = DISPID_IMGBASE + 10;
        public const int DISPID_IHTMLIMGELEMENT_DYNSRC = DISPID_IMGBASE + 9;
        //    DISPIDs for interface IHTMLWindow2

        public const int DISPID_IHTMLIMGELEMENT_FILECREATEDDATE = DISPID_IMG + 12;
        public const int DISPID_IHTMLIMGELEMENT_FILEMODIFIEDDATE = DISPID_IMG + 13;
        public const int DISPID_IHTMLIMGELEMENT_FILESIZE = DISPID_IMG + 11;
        public const int DISPID_IHTMLIMGELEMENT_FILEUPDATEDDATE = DISPID_IMG + 14;
        public const int DISPID_IHTMLIMGELEMENT_HEIGHT = STDPROPID_XOBJ_HEIGHT;
        public const int DISPID_IHTMLIMGELEMENT_HREF = DISPID_IMG + 16;
        public const int DISPID_IHTMLIMGELEMENT_HSPACE = DISPID_IMGBASE + 6;
        public const int DISPID_IHTMLIMGELEMENT_ISMAP = DISPID_IMG + 2;
        public const int DISPID_IHTMLIMGELEMENT_LOOP = DISPID_IMGBASE + 11;
        public const int DISPID_IHTMLIMGELEMENT_LOWSRC = DISPID_IMGBASE + 7;
        public const int DISPID_IHTMLIMGELEMENT_MIMETYPE = DISPID_IMG + 10;
        public const int DISPID_IHTMLIMGELEMENT_NAME = STDPROPID_XOBJ_NAME;
        public const int DISPID_IHTMLIMGELEMENT_NAMEPROP = DISPID_IMG + 17;
        public const int DISPID_IHTMLIMGELEMENT_ONABORT = DISPID_EVPROP_ONABORT;
        public const int DISPID_IHTMLIMGELEMENT_ONERROR = DISPID_EVPROP_ONERROR;
        public const int DISPID_IHTMLIMGELEMENT_ONLOAD = DISPID_EVPROP_ONLOAD;
        public const int DISPID_IHTMLIMGELEMENT_PROTOCOL = DISPID_IMG + 15;
        public const int DISPID_IHTMLIMGELEMENT_READYSTATE = DISPID_A_READYSTATE;
        public const int DISPID_IHTMLIMGELEMENT_SRC = DISPID_IMGBASE + 3;
        public const int DISPID_IHTMLIMGELEMENT_START = DISPID_IMGBASE + 13;
        public const int DISPID_IHTMLIMGELEMENT_USEMAP = DISPID_IMG + 8;
        public const int DISPID_IHTMLIMGELEMENT_VRML = DISPID_IMGBASE + 8;
        public const int DISPID_IHTMLIMGELEMENT_VSPACE = DISPID_IMGBASE + 5;
        public const int DISPID_IHTMLIMGELEMENT_WIDTH = STDPROPID_XOBJ_WIDTH;
        public const int DISPID_IHTMLINPUTBUTTONELEMENT_CREATETEXTRANGE = DISPID_INPUT + 6;
        public const int DISPID_IHTMLINPUTBUTTONELEMENT_DISABLED = STDPROPID_XOBJ_DISABLED;
        public const int DISPID_IHTMLINPUTBUTTONELEMENT_FORM = DISPID_SITE + 4;
        public const int DISPID_IHTMLINPUTBUTTONELEMENT_NAME = STDPROPID_XOBJ_NAME;
        public const int DISPID_IHTMLINPUTBUTTONELEMENT_STATUS = DISPID_INPUT + 21;
        public const int DISPID_IHTMLINPUTBUTTONELEMENT_TYPE = DISPID_INPUT;
        public const int DISPID_IHTMLINPUTBUTTONELEMENT_VALUE = DISPID_A_VALUE;
        public const int DISPID_IHTMLINPUTELEMENT_ALIGN = STDPROPID_XOBJ_CONTROLALIGN;
        public const int DISPID_IHTMLINPUTELEMENT_ALT = DISPID_INPUT + 10;
        public const int DISPID_IHTMLINPUTELEMENT_BORDER = DISPID_INPUT + 12;
        public const int DISPID_IHTMLINPUTELEMENT_CHECKED = DISPID_INPUT + 9;
        public const int DISPID_IHTMLINPUTELEMENT_COMPLETE = DISPID_INPUT + 18;
        public const int DISPID_IHTMLINPUTELEMENT_CREATETEXTRANGE = DISPID_INPUT + 6;
        public const int DISPID_IHTMLINPUTELEMENT_DEFAULTCHECKED = DISPID_INPUT + 8;
        public const int DISPID_IHTMLINPUTELEMENT_DEFAULTVALUE = DISPID_DEFAULTVALUE;
        public const int DISPID_IHTMLINPUTELEMENT_DISABLED = STDPROPID_XOBJ_DISABLED;
        public const int DISPID_IHTMLINPUTELEMENT_DYNSRC = DISPID_INPUT + 17;
        public const int DISPID_IHTMLINPUTELEMENT_FORM = DISPID_SITE + 4;
        public const int DISPID_IHTMLINPUTELEMENT_HEIGHT = STDPROPID_XOBJ_HEIGHT;
        public const int DISPID_IHTMLINPUTELEMENT_HSPACE = DISPID_INPUT + 14;
        public const int DISPID_IHTMLINPUTELEMENT_INDETERMINATE = DISPID_INPUT + 7;
        public const int DISPID_IHTMLINPUTELEMENT_LOOP = DISPID_INPUT + 19;
        public const int DISPID_IHTMLINPUTELEMENT_LOWSRC = DISPID_INPUT + 15;
        public const int DISPID_IHTMLINPUTELEMENT_MAXLENGTH = DISPID_INPUT + 3;
        public const int DISPID_IHTMLINPUTELEMENT_NAME = STDPROPID_XOBJ_NAME;
        public const int DISPID_IHTMLINPUTELEMENT_ONABORT = DISPID_EVPROP_ONABORT;
        public const int DISPID_IHTMLINPUTELEMENT_ONCHANGE = DISPID_EVPROP_ONCHANGE;
        public const int DISPID_IHTMLINPUTELEMENT_ONERROR = DISPID_EVPROP_ONERROR;
        public const int DISPID_IHTMLINPUTELEMENT_ONLOAD = DISPID_EVPROP_ONLOAD;
        public const int DISPID_IHTMLINPUTELEMENT_ONSELECT = DISPID_EVPROP_ONSELECT;
        public const int DISPID_IHTMLINPUTELEMENT_READONLY = DISPID_INPUT + 5;
        public const int DISPID_IHTMLINPUTELEMENT_READYSTATE = DISPID_A_READYSTATE;
        public const int DISPID_IHTMLINPUTELEMENT_SELECT = DISPID_INPUT + 4;
        public const int DISPID_IHTMLINPUTELEMENT_SIZE = DISPID_INPUT + 2;
        public const int DISPID_IHTMLINPUTELEMENT_SRC = DISPID_INPUT + 11;
        public const int DISPID_IHTMLINPUTELEMENT_START = DISPID_INPUT + 20;
        public const int DISPID_IHTMLINPUTELEMENT_STATUS = DISPID_INPUT + 1;
        public const int DISPID_IHTMLINPUTELEMENT_TYPE = DISPID_INPUT;
        public const int DISPID_IHTMLINPUTELEMENT_VALUE = DISPID_A_VALUE;
        public const int DISPID_IHTMLINPUTELEMENT_VRML = DISPID_INPUT + 16;
        public const int DISPID_IHTMLINPUTELEMENT_VSPACE = DISPID_INPUT + 13;
        public const int DISPID_IHTMLINPUTELEMENT_WIDTH = STDPROPID_XOBJ_WIDTH;
        public const int DISPID_IHTMLINPUTFILEELEMENT_DISABLED = STDPROPID_XOBJ_DISABLED;
        public const int DISPID_IHTMLINPUTFILEELEMENT_FORM = DISPID_SITE + 4;
        public const int DISPID_IHTMLINPUTFILEELEMENT_MAXLENGTH = DISPID_INPUT + 3;
        public const int DISPID_IHTMLINPUTFILEELEMENT_NAME = STDPROPID_XOBJ_NAME;
        public const int DISPID_IHTMLINPUTFILEELEMENT_ONCHANGE = DISPID_EVPROP_ONCHANGE;
        public const int DISPID_IHTMLINPUTFILEELEMENT_ONSELECT = DISPID_EVPROP_ONSELECT;
        public const int DISPID_IHTMLINPUTFILEELEMENT_SELECT = DISPID_INPUT + 4;
        public const int DISPID_IHTMLINPUTFILEELEMENT_SIZE = DISPID_INPUT + 2;
        public const int DISPID_IHTMLINPUTFILEELEMENT_STATUS = DISPID_INPUT + 21;
        public const int DISPID_IHTMLINPUTFILEELEMENT_TYPE = DISPID_INPUT;
        public const int DISPID_IHTMLINPUTFILEELEMENT_VALUE = DISPID_A_VALUE;
        public const int DISPID_IHTMLINPUTHIDDENELEMENT_CREATETEXTRANGE = DISPID_INPUT + 6;
        public const int DISPID_IHTMLINPUTHIDDENELEMENT_DISABLED = STDPROPID_XOBJ_DISABLED;
        public const int DISPID_IHTMLINPUTHIDDENELEMENT_FORM = DISPID_SITE + 4;
        public const int DISPID_IHTMLINPUTHIDDENELEMENT_NAME = STDPROPID_XOBJ_NAME;
        public const int DISPID_IHTMLINPUTHIDDENELEMENT_STATUS = DISPID_INPUT + 21;
        public const int DISPID_IHTMLINPUTHIDDENELEMENT_TYPE = DISPID_INPUT;
        public const int DISPID_IHTMLINPUTHIDDENELEMENT_VALUE = DISPID_A_VALUE;
        public const int DISPID_IHTMLINPUTIMAGE_ALIGN = STDPROPID_XOBJ_CONTROLALIGN;
        public const int DISPID_IHTMLINPUTIMAGE_ALT = DISPID_INPUT + 10;
        public const int DISPID_IHTMLINPUTIMAGE_BORDER = DISPID_INPUT + 12;
        public const int DISPID_IHTMLINPUTIMAGE_COMPLETE = DISPID_INPUT + 18;
        public const int DISPID_IHTMLINPUTIMAGE_DISABLED = STDPROPID_XOBJ_DISABLED;
        public const int DISPID_IHTMLINPUTIMAGE_DYNSRC = DISPID_INPUT + 17;
        public const int DISPID_IHTMLINPUTIMAGE_HEIGHT = STDPROPID_XOBJ_HEIGHT;
        public const int DISPID_IHTMLINPUTIMAGE_HSPACE = DISPID_INPUT + 14;
        public const int DISPID_IHTMLINPUTIMAGE_LOOP = DISPID_INPUT + 19;
        public const int DISPID_IHTMLINPUTIMAGE_LOWSRC = DISPID_INPUT + 15;
        public const int DISPID_IHTMLINPUTIMAGE_NAME = STDPROPID_XOBJ_NAME;
        public const int DISPID_IHTMLINPUTIMAGE_ONABORT = DISPID_EVPROP_ONABORT;
        public const int DISPID_IHTMLINPUTIMAGE_ONERROR = DISPID_EVPROP_ONERROR;
        public const int DISPID_IHTMLINPUTIMAGE_ONLOAD = DISPID_EVPROP_ONLOAD;
        public const int DISPID_IHTMLINPUTIMAGE_READYSTATE = DISPID_A_READYSTATE;
        public const int DISPID_IHTMLINPUTIMAGE_SRC = DISPID_INPUT + 11;
        public const int DISPID_IHTMLINPUTIMAGE_START = DISPID_INPUT + 20;
        public const int DISPID_IHTMLINPUTIMAGE_TYPE = DISPID_INPUT;
        public const int DISPID_IHTMLINPUTIMAGE_VRML = DISPID_INPUT + 16;
        public const int DISPID_IHTMLINPUTIMAGE_VSPACE = DISPID_INPUT + 13;
        public const int DISPID_IHTMLINPUTIMAGE_WIDTH = STDPROPID_XOBJ_WIDTH;
        public const int DISPID_IHTMLINPUTTEXTELEMENT_CREATETEXTRANGE = DISPID_INPUT + 6;
        public const int DISPID_IHTMLINPUTTEXTELEMENT_DEFAULTVALUE = DISPID_DEFAULTVALUE;
        public const int DISPID_IHTMLINPUTTEXTELEMENT_DISABLED = STDPROPID_XOBJ_DISABLED;
        public const int DISPID_IHTMLINPUTTEXTELEMENT_FORM = DISPID_SITE + 4;
        public const int DISPID_IHTMLINPUTTEXTELEMENT_MAXLENGTH = DISPID_INPUT + 3;
        public const int DISPID_IHTMLINPUTTEXTELEMENT_NAME = STDPROPID_XOBJ_NAME;
        public const int DISPID_IHTMLINPUTTEXTELEMENT_ONCHANGE = DISPID_EVPROP_ONCHANGE;
        public const int DISPID_IHTMLINPUTTEXTELEMENT_ONSELECT = DISPID_EVPROP_ONSELECT;
        public const int DISPID_IHTMLINPUTTEXTELEMENT_READONLY = DISPID_INPUT + 5;
        public const int DISPID_IHTMLINPUTTEXTELEMENT_SELECT = DISPID_INPUT + 4;
        public const int DISPID_IHTMLINPUTTEXTELEMENT_SIZE = DISPID_INPUT + 2;
        public const int DISPID_IHTMLINPUTTEXTELEMENT_STATUS = DISPID_INPUT + 21;
        public const int DISPID_IHTMLINPUTTEXTELEMENT_TYPE = DISPID_INPUT;
        public const int DISPID_IHTMLINPUTTEXTELEMENT_VALUE = DISPID_A_VALUE;
        public const int DISPID_IHTMLLIELEMENT_TYPE = DISPID_A_LISTTYPE;
        public const int DISPID_IHTMLLIELEMENT_VALUE = DISPID_LI + 1;
        public const int DISPID_IHTMLMETAELEMENT_CHARSET = DISPID_HEDELEMS + 13;
        //    DISPIDs for interface IHTMLTxtRange
        public const int DISPID_IHTMLMETAELEMENT_CONTENT = DISPID_HEDELEMS + 2;
        public const int DISPID_IHTMLMETAELEMENT_HTTPEQUIV = DISPID_HEDELEMS + 1;
        public const int DISPID_IHTMLMETAELEMENT_NAME = STDPROPID_XOBJ_NAME;
        public const int DISPID_IHTMLMETAELEMENT_URL = DISPID_HEDELEMS + 3;
        //    DISPIDs for interface IHTMLMetaElement2
        public const int DISPID_IHTMLMETAELEMENT2_SCHEME = DISPID_HEDELEMS + 20;
        //    DISPIDs for interface IHTMLBaseElement

        //    DISPIDs for interface IHTMLNextIdElement
        public const int DISPID_IHTMLNEXTIDELEMENT_N = DISPID_HEDELEMS + 12;
        public const int DISPID_IHTMLOPTIONELEMENT_DEFAULTSELECTED = DISPID_OPTION + 3;
        public const int DISPID_IHTMLOPTIONELEMENT_FORM = DISPID_OPTION + 6;
        public const int DISPID_IHTMLOPTIONELEMENT_INDEX = DISPID_OPTION + 5;
        public const int DISPID_IHTMLOPTIONELEMENT_SELECTED = DISPID_OPTION + 1;
        public const int DISPID_IHTMLOPTIONELEMENT_TEXT = DISPID_OPTION + 4;
        public const int DISPID_IHTMLOPTIONELEMENT_VALUE = DISPID_OPTION + 2;
        public const int DISPID_IHTMLPOPUP_DOCUMENT = DISPID_HTMLPOPUP + 3;
        public const int DISPID_IHTMLPOPUP_HIDE = DISPID_HTMLPOPUP + 2;
        public const int DISPID_IHTMLPOPUP_ISOPEN = DISPID_HTMLPOPUP + 4;
        public const int DISPID_IHTMLPOPUP_SHOW = DISPID_HTMLPOPUP + 1;
        public const int DISPID_IHTMLRENDERSTYLE_DEFAULTTEXTSELECTION = DISPID_A_DEFAULTTEXTSELECTION;
        public const int DISPID_IHTMLRENDERSTYLE_RENDERINGPRIORITY = DISPID_A_RENDERINGPRIORITY;
        public const int DISPID_IHTMLRENDERSTYLE_TEXTBACKGROUNDCOLOR = DISPID_A_TEXTBACKGROUNDCOLOR;
        public const int DISPID_IHTMLRENDERSTYLE_TEXTCOLOR = DISPID_A_TEXTCOLOR;
        public const int DISPID_IHTMLRENDERSTYLE_TEXTDECORATION = DISPID_A_STYLETEXTDECORATION;
        public const int DISPID_IHTMLRENDERSTYLE_TEXTDECORATIONCOLOR = DISPID_A_TEXTDECORATIONCOLOR;
        public const int DISPID_IHTMLRENDERSTYLE_TEXTEFFECT = DISPID_A_TEXTEFFECT;
        public const int DISPID_IHTMLRENDERSTYLE_TEXTLINETHROUGHSTYLE = DISPID_A_TEXTLINETHROUGHSTYLE;
        public const int DISPID_IHTMLRENDERSTYLE_TEXTUNDERLINESTYLE = DISPID_A_TEXTUNDERLINESTYLE;
        public const int DISPID_IHTMLSCRIPTELEMENT_DEFER = DISPID_SCRIPT + 7;
        public const int DISPID_IHTMLSCRIPTELEMENT_EVENT = DISPID_SCRIPT + 5;
        public const int DISPID_IHTMLSCRIPTELEMENT_HTMLFOR = DISPID_SCRIPT + 4;
        public const int DISPID_IHTMLSCRIPTELEMENT_ONERROR = DISPID_EVPROP_ONERROR;
        public const int DISPID_IHTMLSCRIPTELEMENT_READYSTATE = DISPID_A_READYSTATE;
        public const int DISPID_IHTMLSCRIPTELEMENT_SRC = DISPID_SCRIPT + 1;
        public const int DISPID_IHTMLSCRIPTELEMENT_TEXT = DISPID_SCRIPT + 6;
        public const int DISPID_IHTMLSCRIPTELEMENT_TYPE = DISPID_SCRIPT + 9;
        public const int DISPID_IHTMLSELECTELEMENT__NEWENUM = DISPID_NEWENUM;
        public const int DISPID_IHTMLSELECTELEMENT_ADD = DISPID_COLLECTION + 3;
        public const int DISPID_IHTMLSELECTELEMENT_DISABLED = STDPROPID_XOBJ_DISABLED;
        public const int DISPID_IHTMLSELECTELEMENT_FORM = DISPID_SITE + 4;
        public const int DISPID_IHTMLSELECTELEMENT_ITEM = DISPID_VALUE;
        public const int DISPID_IHTMLSELECTELEMENT_LENGTH = DISPID_COLLECTION;
        public const int DISPID_IHTMLSELECTELEMENT_MULTIPLE = DISPID_SELECT + 3;
        public const int DISPID_IHTMLSELECTELEMENT_NAME = STDPROPID_XOBJ_NAME;
        public const int DISPID_IHTMLSELECTELEMENT_ONCHANGE = DISPID_EVPROP_ONCHANGE;
        public const int DISPID_IHTMLSELECTELEMENT_OPTIONS = DISPID_SELECT + 5;
        public const int DISPID_IHTMLSELECTELEMENT_REMOVE = DISPID_COLLECTION + 4;
        public const int DISPID_IHTMLSELECTELEMENT_SELECTEDINDEX = DISPID_SELECT + 10;
        public const int DISPID_IHTMLSELECTELEMENT_SIZE = DISPID_SELECT + 2;
        public const int DISPID_IHTMLSELECTELEMENT_TAGS = DISPID_COLLECTION + 2;
        public const int DISPID_IHTMLSELECTELEMENT_TYPE = DISPID_SELECT + 12;
        public const int DISPID_IHTMLSELECTELEMENT_VALUE = DISPID_SELECT + 11;
        public const int DISPID_IHTMLSELECTIONOBJECT_CLEAR = DISPID_SELECTOBJ + 3;
        public const int DISPID_IHTMLSELECTIONOBJECT_CREATERANGE = DISPID_SELECTOBJ + 1;
        public const int DISPID_IHTMLSELECTIONOBJECT_EMPTY = DISPID_SELECTOBJ + 2;
        public const int DISPID_IHTMLSELECTIONOBJECT_TYPE = DISPID_SELECTOBJ + 4;
        public const int DISPID_IHTMLSTYLESHEETSCOLLECTION__NEWENUM = DISPID_NEWENUM;
        public const int DISPID_IHTMLSTYLESHEETSCOLLECTION_ITEM = DISPID_VALUE;
        public const int DISPID_IHTMLSTYLESHEETSCOLLECTION_LENGTH = DISPID_STYLESHEETS_COL + 1;
        public const int DISPID_IHTMLTABLE_ALIGN = STDPROPID_XOBJ_CONTROLALIGN;
        public const int DISPID_IHTMLTABLE_BACKGROUND = DISPID_A_BACKGROUNDIMAGE;
        public const int DISPID_IHTMLTABLE_BGCOLOR = DISPID_BACKCOLOR;
        public const int DISPID_IHTMLTABLE_BORDER = DISPID_TABLE + 2;
        public const int DISPID_IHTMLTABLE_BORDERCOLOR = DISPID_A_TABLEBORDERCOLOR;
        public const int DISPID_IHTMLTABLE_BORDERCOLORDARK = DISPID_A_TABLEBORDERCOLORDARK;
        public const int DISPID_IHTMLTABLE_BORDERCOLORLIGHT = DISPID_A_TABLEBORDERCOLORLIGHT;
        public const int DISPID_IHTMLTABLE_CAPTION = DISPID_TABLE + 25;
        public const int DISPID_IHTMLTABLE_CELLPADDING = DISPID_TABLE + 6;
        public const int DISPID_IHTMLTABLE_CELLSPACING = DISPID_TABLE + 5;
        public const int DISPID_IHTMLTABLE_COLS = DISPID_TABLE + 1;
        public const int DISPID_IHTMLTABLE_CREATECAPTION = DISPID_TABLE + 30;
        public const int DISPID_IHTMLTABLE_CREATETFOOT = DISPID_TABLE + 28;
        public const int DISPID_IHTMLTABLE_CREATETHEAD = DISPID_TABLE + 26;
        public const int DISPID_IHTMLTABLE_DATAPAGESIZE = DISPID_TABLE + 17;
        public const int DISPID_IHTMLTABLE_DELETECAPTION = DISPID_TABLE + 31;
        public const int DISPID_IHTMLTABLE_DELETEROW = DISPID_TABLE + 33;
        public const int DISPID_IHTMLTABLE_DELETETFOOT = DISPID_TABLE + 29;
        public const int DISPID_IHTMLTABLE_DELETETHEAD = DISPID_TABLE + 27;
        public const int DISPID_IHTMLTABLE_FRAME = DISPID_TABLE + 4;
        public const int DISPID_IHTMLTABLE_HEIGHT = STDPROPID_XOBJ_HEIGHT;
        public const int DISPID_IHTMLTABLE_INSERTROW = DISPID_TABLE + 32;
        public const int DISPID_IHTMLTABLE_NEXTPAGE = DISPID_TABLE + 18;
        public const int DISPID_IHTMLTABLE_ONREADYSTATECHANGE = DISPID_EVPROP_ONREADYSTATECHANGE;
        public const int DISPID_IHTMLTABLE_PREVIOUSPAGE = DISPID_TABLE + 19;
        public const int DISPID_IHTMLTABLE_READYSTATE = DISPID_A_READYSTATE;
        public const int DISPID_IHTMLTABLE_REFRESH = DISPID_TABLE + 15;
        public const int DISPID_IHTMLTABLE_ROWS = DISPID_TABLE + 16;
        public const int DISPID_IHTMLTABLE_RULES = DISPID_TABLE + 3;
        public const int DISPID_IHTMLTABLE_TBODIES = DISPID_TABLE + 24;
        public const int DISPID_IHTMLTABLE_TFOOT = DISPID_TABLE + 21;
        public const int DISPID_IHTMLTABLE_THEAD = DISPID_TABLE + 20;
        public const int DISPID_IHTMLTABLE_WIDTH = STDPROPID_XOBJ_WIDTH;
        public const int DISPID_IHTMLTABLE2_CELLS = DISPID_TABLE + 37;
        //    DISPIDs for interface IHTMLTable2
        public const int DISPID_IHTMLTABLE2_FIRSTPAGE = DISPID_TABLE + 35;
        public const int DISPID_IHTMLTABLE2_LASTPAGE = DISPID_TABLE + 36;
        public const int DISPID_IHTMLTABLE2_MOVEROW = DISPID_TABLE + 38;
        //    DISPIDs for interface IHTMLTable3
        public const int DISPID_IHTMLTABLE3_SUMMARY = DISPID_TABLE + 39;
        public const int DISPID_IHTMLTABLECELL_ALIGN = STDPROPID_XOBJ_BLOCKALIGN;
        public const int DISPID_IHTMLTABLECELL_BACKGROUND = DISPID_A_BACKGROUNDIMAGE;
        public const int DISPID_IHTMLTABLECELL_BGCOLOR = DISPID_BACKCOLOR;
        public const int DISPID_IHTMLTABLECELL_BORDERCOLOR = DISPID_A_TABLEBORDERCOLOR;
        public const int DISPID_IHTMLTABLECELL_BORDERCOLORDARK = DISPID_A_TABLEBORDERCOLORDARK;
        public const int DISPID_IHTMLTABLECELL_BORDERCOLORLIGHT = DISPID_A_TABLEBORDERCOLORLIGHT;
        public const int DISPID_IHTMLTABLECELL_CELLINDEX = DISPID_TABLECELL + 3;
        public const int DISPID_IHTMLTABLECELL_COLSPAN = DISPID_TABLECELL + 2;
        public const int DISPID_IHTMLTABLECELL_HEIGHT = STDPROPID_XOBJ_HEIGHT;
        public const int DISPID_IHTMLTABLECELL_NOWRAP = DISPID_A_NOWRAP;
        public const int DISPID_IHTMLTABLECELL_ROWSPAN = DISPID_TABLECELL + 1;
        public const int DISPID_IHTMLTABLECELL_VALIGN = DISPID_A_TABLEVALIGN;
        public const int DISPID_IHTMLTABLECELL_WIDTH = STDPROPID_XOBJ_WIDTH;
        public const int DISPID_IHTMLTABLECELL2_ABBR = DISPID_TABLECELL + 4;
        public const int DISPID_IHTMLTABLECELL2_AXIS = DISPID_TABLECELL + 5;
        public const int DISPID_IHTMLTABLECELL2_CH = DISPID_TABLECELL + 6;
        public const int DISPID_IHTMLTABLECELL2_CHOFF = DISPID_TABLECELL + 7;
        public const int DISPID_IHTMLTABLECELL2_HEADERS = DISPID_TABLECELL + 8;
        public const int DISPID_IHTMLTABLECELL2_SCOPE = DISPID_TABLECELL + 9;
        //    DISPIDs for interface IHTMLTableCol
        public const int DISPID_IHTMLTABLECOL_ALIGN = STDPROPID_XOBJ_BLOCKALIGN;
        public const int DISPID_IHTMLTABLECOL_SPAN = DISPID_TABLECOL + 1;
        public const int DISPID_IHTMLTABLECOL_VALIGN = DISPID_A_TABLEVALIGN;
        public const int DISPID_IHTMLTABLECOL_WIDTH = STDPROPID_XOBJ_WIDTH;
        //    DISPIDs for interface IHTMLTableCol2
        public const int DISPID_IHTMLTABLECOL2_CH = DISPID_TABLECOL + 2;
        public const int DISPID_IHTMLTABLECOL2_CHOFF = DISPID_TABLECOL + 3;
        //    DISPIDs for interface IHTMLTableSection

        //    DISPIDs for interface IHTMLTableRow
        public const int DISPID_IHTMLTABLEROW_ALIGN = STDPROPID_XOBJ_BLOCKALIGN;
        public const int DISPID_IHTMLTABLEROW_BGCOLOR = DISPID_BACKCOLOR;
        public const int DISPID_IHTMLTABLEROW_BORDERCOLOR = DISPID_A_TABLEBORDERCOLOR;
        public const int DISPID_IHTMLTABLEROW_BORDERCOLORDARK = DISPID_A_TABLEBORDERCOLORDARK;
        public const int DISPID_IHTMLTABLEROW_BORDERCOLORLIGHT = DISPID_A_TABLEBORDERCOLORLIGHT;
        public const int DISPID_IHTMLTABLEROW_CELLS = DISPID_TABLEROW + 2;
        public const int DISPID_IHTMLTABLEROW_DELETECELL = DISPID_TABLEROW + 4;
        public const int DISPID_IHTMLTABLEROW_INSERTCELL = DISPID_TABLEROW + 3;
        public const int DISPID_IHTMLTABLEROW_ROWINDEX = DISPID_TABLEROW;
        public const int DISPID_IHTMLTABLEROW_SECTIONROWINDEX = DISPID_TABLEROW + 1;
        public const int DISPID_IHTMLTABLEROW_VALIGN = DISPID_A_TABLEVALIGN;
        //    DISPIDs for interface IHTMLTableRow2
        public const int DISPID_IHTMLTABLEROW2_HEIGHT = STDPROPID_XOBJ_HEIGHT;
        //    DISPIDs for interface IHTMLTableRow3
        public const int DISPID_IHTMLTABLEROW3_CH = DISPID_TABLEROW + 9;
        public const int DISPID_IHTMLTABLEROW3_CHOFF = DISPID_TABLEROW + 10;
        //    DISPIDs for interface IHTMLTableRowMetrics
        public const int DISPID_IHTMLTABLEROWMETRICS_CLIENTHEIGHT = DISPID_SITE + 19;
        public const int DISPID_IHTMLTABLEROWMETRICS_CLIENTLEFT = DISPID_SITE + 22;
        public const int DISPID_IHTMLTABLEROWMETRICS_CLIENTTOP = DISPID_SITE + 21;
        public const int DISPID_IHTMLTABLEROWMETRICS_CLIENTWIDTH = DISPID_SITE + 20;
        public const int DISPID_IHTMLTABLESECTION_ALIGN = STDPROPID_XOBJ_BLOCKALIGN;
        public const int DISPID_IHTMLTABLESECTION_BGCOLOR = DISPID_BACKCOLOR;
        public const int DISPID_IHTMLTABLESECTION_DELETEROW = DISPID_TABLESECTION + 2;
        public const int DISPID_IHTMLTABLESECTION_INSERTROW = DISPID_TABLESECTION + 1;
        public const int DISPID_IHTMLTABLESECTION_ROWS = DISPID_TABLESECTION;
        public const int DISPID_IHTMLTABLESECTION_VALIGN = DISPID_A_TABLEVALIGN;
        public const int DISPID_IHTMLTABLESECTION2_MOVEROW = DISPID_TABLESECTION + 3;
        //    DISPIDs for interface IHTMLTableSection3
        public const int DISPID_IHTMLTABLESECTION3_CH = DISPID_TABLESECTION + 4;
        public const int DISPID_IHTMLTABLESECTION3_CHOFF = DISPID_TABLESECTION + 5;
        public const int DISPID_IHTMLTEXTAREAELEMENT_COLS = DISPID_RICHTEXT + 2;
        public const int DISPID_IHTMLTEXTAREAELEMENT_CREATETEXTRANGE = DISPID_RICHTEXT + 6;
        public const int DISPID_IHTMLTEXTAREAELEMENT_DEFAULTVALUE = DISPID_DEFAULTVALUE;
        //    DISPIDs for interface IHTMLTableCell
        public const int DISPID_IHTMLTEXTAREAELEMENT_DISABLED = STDPROPID_XOBJ_DISABLED;
        public const int DISPID_IHTMLTEXTAREAELEMENT_FORM = DISPID_SITE + 4;
        public const int DISPID_IHTMLTEXTAREAELEMENT_NAME = STDPROPID_XOBJ_NAME;
        public const int DISPID_IHTMLTEXTAREAELEMENT_ONCHANGE = DISPID_EVPROP_ONCHANGE;
        public const int DISPID_IHTMLTEXTAREAELEMENT_ONSELECT = DISPID_EVPROP_ONSELECT;
        public const int DISPID_IHTMLTEXTAREAELEMENT_READONLY = DISPID_RICHTEXT + 4;
        public const int DISPID_IHTMLTEXTAREAELEMENT_ROWS = DISPID_RICHTEXT + 1;
        public const int DISPID_IHTMLTEXTAREAELEMENT_SELECT = DISPID_RICHTEXT + 5;
        public const int DISPID_IHTMLTEXTAREAELEMENT_STATUS = DISPID_INPUT + 1;
        public const int DISPID_IHTMLTEXTAREAELEMENT_TYPE = DISPID_INPUT;
        public const int DISPID_IHTMLTEXTAREAELEMENT_VALUE = DISPID_A_VALUE;
        public const int DISPID_IHTMLTEXTAREAELEMENT_WRAP = DISPID_RICHTEXT + 3;
        public const int DISPID_IHTMLTITLEELEMENT_TEXT = DISPID_A_VALUE;
        public const int DISPID_IHTMLTXTRANGE_COLLAPSE = DISPID_RANGE + 13;
        public const int DISPID_IHTMLTXTRANGE_COMPAREENDPOINTS = DISPID_RANGE + 18;
        public const int DISPID_IHTMLTXTRANGE_DUPLICATE = DISPID_RANGE + 8;
        public const int DISPID_IHTMLTXTRANGE_EXECCOMMAND = DISPID_RANGE + 33;
        public const int DISPID_IHTMLTXTRANGE_EXECCOMMANDSHOWHELP = DISPID_RANGE + 34;
        public const int DISPID_IHTMLTXTRANGE_EXPAND = DISPID_RANGE + 14;
        public const int DISPID_IHTMLTXTRANGE_FINDTEXT = DISPID_RANGE + 19;
        public const int DISPID_IHTMLTXTRANGE_GETBOOKMARK = DISPID_RANGE + 21;
        public const int DISPID_IHTMLTXTRANGE_HTMLTEXT = DISPID_RANGE + 3;
        public const int DISPID_IHTMLTXTRANGE_INRANGE = DISPID_RANGE + 10;
        public const int DISPID_IHTMLTXTRANGE_ISEQUAL = DISPID_RANGE + 11;
        public const int DISPID_IHTMLTXTRANGE_MOVE = DISPID_RANGE + 15;
        public const int DISPID_IHTMLTXTRANGE_MOVEEND = DISPID_RANGE + 17;
        public const int DISPID_IHTMLTXTRANGE_MOVESTART = DISPID_RANGE + 16;
        public const int DISPID_IHTMLTXTRANGE_MOVETOBOOKMARK = DISPID_RANGE + 9;
        public const int DISPID_IHTMLTXTRANGE_MOVETOELEMENTTEXT = DISPID_RANGE + 1;
        public const int DISPID_IHTMLTXTRANGE_MOVETOPOINT = DISPID_RANGE + 20;
        public const int DISPID_IHTMLTXTRANGE_PARENTELEMENT = DISPID_RANGE + 6;
        public const int DISPID_IHTMLTXTRANGE_PASTEHTML = DISPID_RANGE + 26;
        public const int DISPID_IHTMLTXTRANGE_QUERYCOMMANDENABLED = DISPID_RANGE + 28;
        public const int DISPID_IHTMLTXTRANGE_QUERYCOMMANDINDETERM = DISPID_RANGE + 30;
        public const int DISPID_IHTMLTXTRANGE_QUERYCOMMANDSTATE = DISPID_RANGE + 29;
        public const int DISPID_IHTMLTXTRANGE_QUERYCOMMANDSUPPORTED = DISPID_RANGE + 27;
        public const int DISPID_IHTMLTXTRANGE_QUERYCOMMANDTEXT = DISPID_RANGE + 31;
        public const int DISPID_IHTMLTXTRANGE_QUERYCOMMANDVALUE = DISPID_RANGE + 32;
        public const int DISPID_IHTMLTXTRANGE_SCROLLINTOVIEW = DISPID_RANGE + 12;
        public const int DISPID_IHTMLTXTRANGE_SELECT = DISPID_RANGE + 24;
        public const int DISPID_IHTMLTXTRANGE_SETENDPOINT = DISPID_RANGE + 25;
        public const int DISPID_IHTMLTXTRANGE_TEXT = DISPID_RANGE + 4;
        public const int DISPID_IHTMLWINDOW2__NEWENUM = 1153;
        public const int DISPID_IHTMLWINDOW2_ALERT = 1105;
        public const int DISPID_IHTMLWINDOW2_BLUR = 1159;
        public const int DISPID_IHTMLWINDOW2_CLEARINTERVAL = 1163;
        public const int DISPID_IHTMLWINDOW2_CLEARTIMEOUT = 1104;
        public const int DISPID_IHTMLWINDOW2_CLIENTINFORMATION = 1161;
        public const int DISPID_IHTMLWINDOW2_CLOSE = 3;
        public const int DISPID_IHTMLWINDOW2_CLOSED = 23;
        public const int DISPID_IHTMLWINDOW2_CONFIRM = 1110;
        public const int DISPID_IHTMLWINDOW2_DEFAULTSTATUS = 1101;
        public const int DISPID_IHTMLWINDOW2_DOCUMENT = 1151;
        public const int DISPID_IHTMLWINDOW2_EVENT = 1152;
        public const int DISPID_IHTMLWINDOW2_EXECSCRIPT = 1165;
        public const int DISPID_IHTMLWINDOW2_EXTERNAL = 1169;
        public const int DISPID_IHTMLWINDOW2_FOCUS = 1158;
        public const int DISPID_IHTMLWINDOW2_FRAMES = 1100;
        public const int DISPID_IHTMLWINDOW2_HISTORY = 2;
        public const int DISPID_IHTMLWINDOW2_IMAGE = 1125;
        public const int DISPID_IHTMLWINDOW2_LOCATION = 14;
        public const int DISPID_IHTMLWINDOW2_MOVEBY = 7;
        public const int DISPID_IHTMLWINDOW2_MOVETO = 6;
        public const int DISPID_IHTMLWINDOW2_NAME = 11;
        public const int DISPID_IHTMLWINDOW2_NAVIGATE = 25;
        public const int DISPID_IHTMLWINDOW2_NAVIGATOR = 5;
        public const int DISPID_IHTMLWINDOW2_OFFSCREENBUFFERING = 1164;
        public const int DISPID_IHTMLWINDOW2_ONBEFOREUNLOAD = DISPID_EVPROP_ONBEFOREUNLOAD;
        public const int DISPID_IHTMLWINDOW2_ONBLUR = DISPID_EVPROP_ONBLUR;
        public const int DISPID_IHTMLWINDOW2_ONERROR = DISPID_EVPROP_ONERROR;
        public const int DISPID_IHTMLWINDOW2_ONFOCUS = DISPID_EVPROP_ONFOCUS;
        public const int DISPID_IHTMLWINDOW2_ONHELP = DISPID_EVPROP_ONHELP;
        public const int DISPID_IHTMLWINDOW2_ONLOAD = DISPID_EVPROP_ONLOAD;
        public const int DISPID_IHTMLWINDOW2_ONRESIZE = DISPID_EVPROP_ONRESIZE;
        public const int DISPID_IHTMLWINDOW2_ONSCROLL = DISPID_EVPROP_ONSCROLL;
        public const int DISPID_IHTMLWINDOW2_ONUNLOAD = DISPID_EVPROP_ONUNLOAD;
        public const int DISPID_IHTMLWINDOW2_OPEN = 13;
        public const int DISPID_IHTMLWINDOW2_OPENER = 4;
        public const int DISPID_IHTMLWINDOW2_OPTION = 1157;
        public const int DISPID_IHTMLWINDOW2_PARENT = 12;
        public const int DISPID_IHTMLWINDOW2_PROMPT = 1111;
        public const int DISPID_IHTMLWINDOW2_RESIZEBY = 8;
        public const int DISPID_IHTMLWINDOW2_RESIZETO = 9;
        public const int DISPID_IHTMLWINDOW2_SCREEN = 1156;
        public const int DISPID_IHTMLWINDOW2_SCROLL = 1160;
        public const int DISPID_IHTMLWINDOW2_SCROLLBY = 1167;
        public const int DISPID_IHTMLWINDOW2_SCROLLTO = 1168;
        public const int DISPID_IHTMLWINDOW2_SELF = 20;
        public const int DISPID_IHTMLWINDOW2_SETINTERVAL = 1173;
        public const int DISPID_IHTMLWINDOW2_SETTIMEOUT = 1172;
        public const int DISPID_IHTMLWINDOW2_SHOWHELP = 1155;
        public const int DISPID_IHTMLWINDOW2_SHOWMODALDIALOG = 1154;
        public const int DISPID_IHTMLWINDOW2_STATUS = 1102;
        public const int DISPID_IHTMLWINDOW2_TOP = 21;
        public const int DISPID_IHTMLWINDOW2_TOSTRING = 1166;
        public const int DISPID_IHTMLWINDOW2_WINDOW = 22;
        public const int DISPID_IHTMLWINDOW3_ATTACHEVENT = DISPID_HTMLOBJECT + 7;
        public const int DISPID_IHTMLWINDOW3_CLIPBOARDDATA = 1175;
        public const int DISPID_IHTMLWINDOW3_DETACHEVENT = DISPID_HTMLOBJECT + 8;
        public const int DISPID_IHTMLWINDOW3_ONAFTERPRINT = DISPID_EVPROP_ONAFTERPRINT;
        public const int DISPID_IHTMLWINDOW3_ONBEFOREPRINT = DISPID_EVPROP_ONBEFOREPRINT;
        public const int DISPID_IHTMLWINDOW3_PRINT = 1174;
        public const int DISPID_IHTMLWINDOW3_SCREENLEFT = 1170;
        public const int DISPID_IHTMLWINDOW3_SCREENTOP = 1171;
        public const int DISPID_IHTMLWINDOW3_SETINTERVAL = 1162;
        public const int DISPID_IHTMLWINDOW3_SETTIMEOUT = 1103;
        public const int DISPID_IHTMLWINDOW3_SHOWMODELESSDIALOG = 1176;
        //    DISPIDs for interface IHTMLWindow4
        public const int DISPID_IHTMLWINDOW4_CREATEPOPUP = 1180;
        public const int DISPID_IHTMLWINDOW4_FRAMEELEMENT = 1181;
        public const int DISPID_ILINEINFO = DISPID_NORMAL_FIRST;
        //    DISPIDs for interface ILineInfo
        public const int DISPID_ILINEINFO_BASELINE = DISPID_ILINEINFO + 2;
        public const int DISPID_ILINEINFO_LINEDIRECTION = DISPID_ILINEINFO + 5;
        public const int DISPID_ILINEINFO_TEXTDESCENT = DISPID_ILINEINFO + 3;
        public const int DISPID_ILINEINFO_TEXTHEIGHT = DISPID_ILINEINFO + 4;
        public const int DISPID_ILINEINFO_X = DISPID_ILINEINFO + 1;
        public const int DISPID_IMEMODE = -542;
        public const int DISPID_IMG = DISPID_IMGBASE + 1000;
        public const int DISPID_IMGBASE = DISPID_NORMAL_FIRST;
        public const int DISPID_INPUT = DISPID_TEXTSITE + 1000;
        public const int DISPID_INPUTIMAGE = DISPID_IMGBASE + 1000;
        public const int DISPID_INPUTTEXT = DISPID_INPUTTEXTBASE + 1000;
        public const int DISPID_INPUTTEXTBASE = DISPID_INPUT + 1000;
        public const int DISPID_INTERNAL_CATTRIBUTECOLLPTRCACHE = DISPID_A_FIRST + 210;
        public const int DISPID_INTERNAL_CDOMCHILDRENPTRCACHE = DISPID_A_FIRST + 126;
        public const int DISPID_INTERNAL_INVOKECONTEXTDOCUMENT = DISPID_A_FIRST + 212;
        public const int DISPID_INTERNAL_ONBEHAVIOR_APPLYSTYLE = DISPID_A_FIRST + 148;
        public const int DISPID_INTERNAL_ONBEHAVIOR_CONTENTREADY = DISPID_A_FIRST + 124;
        public const int DISPID_INTERNAL_ONBEHAVIOR_DOCUMENTREADY = DISPID_A_FIRST + 125;
        public const int DISPID_INTERNAL_RUNTIMESTYLEAA = DISPID_A_FIRST + 149;
        //    DISPIDs for interface IOmNavigator
        public const int DISPID_IOMNAVIGATOR_APPCODENAME = DISPID_NAVIGATOR;
        public const int DISPID_IOMNAVIGATOR_APPMINORVERSION = DISPID_NAVIGATOR + 16;
        public const int DISPID_IOMNAVIGATOR_APPNAME = DISPID_NAVIGATOR + 1;
        public const int DISPID_IOMNAVIGATOR_APPVERSION = DISPID_NAVIGATOR + 2;
        public const int DISPID_IOMNAVIGATOR_BROWSERLANGUAGE = DISPID_NAVIGATOR + 13;
        public const int DISPID_IOMNAVIGATOR_CONNECTIONSPEED = DISPID_NAVIGATOR + 17;
        public const int DISPID_IOMNAVIGATOR_COOKIEENABLED = DISPID_NAVIGATOR + 8;
        public const int DISPID_IOMNAVIGATOR_CPUCLASS = DISPID_NAVIGATOR + 11;
        public const int DISPID_IOMNAVIGATOR_JAVAENABLED = DISPID_NAVIGATOR + 4;
        public const int DISPID_IOMNAVIGATOR_MIMETYPES = DISPID_NAVIGATOR + 6;
        public const int DISPID_IOMNAVIGATOR_ONLINE = DISPID_NAVIGATOR + 18;
        public const int DISPID_IOMNAVIGATOR_OPSPROFILE = DISPID_NAVIGATOR + 9;
        public const int DISPID_IOMNAVIGATOR_PLATFORM = DISPID_NAVIGATOR + 15;
        public const int DISPID_IOMNAVIGATOR_PLUGINS = DISPID_NAVIGATOR + 7;
        public const int DISPID_IOMNAVIGATOR_SYSTEMLANGUAGE = DISPID_NAVIGATOR + 12;
        public const int DISPID_IOMNAVIGATOR_TAINTENABLED = DISPID_NAVIGATOR + 5;
        public const int DISPID_IOMNAVIGATOR_TOSTRING = DISPID_NAVIGATOR + 10;
        public const int DISPID_IOMNAVIGATOR_USERAGENT = DISPID_NAVIGATOR + 3;
        public const int DISPID_IOMNAVIGATOR_USERLANGUAGE = DISPID_NAVIGATOR + 14;
        public const int DISPID_IOMNAVIGATOR_USERPROFILE = DISPID_NAVIGATOR + 19;
        public const int DISPID_KEYDOWN = -602;
        public const int DISPID_KEYPRESS = -603;
        public const int DISPID_KEYUP = -604;
        public const int DISPID_LI = DISPID_NORMAL_FIRST;
        public const int DISPID_LIST = -528;
        public const int DISPID_LISTCOUNT = -531;
        public const int DISPID_LISTINDEX = -526;
        public const int DISPID_MARQUEE = DISPID_TEXTAREA + 1000;
        public const int DISPID_MAXLENGTH = -533;
        public const int DISPID_MOUSEDOWN = -605;
        public const int DISPID_MOUSEICON = -522;
        public const int DISPID_MOUSEMOVE = -606;
        public const int DISPID_MOUSEPOINTER = -521;
        public const int DISPID_MOUSEUP = -607;
        public const int DISPID_MULTILINE = -537;
        public const int DISPID_MULTISELECT = -532;
        public const int DISPID_NAVIGATOR = 1;
        public const int DISPID_NEWENUM = -4;
        public const int DISPID_NORMAL_FIRST = 1000;
        public const int DISPID_NUMBEROFCOLUMNS = -539;
        public const int DISPID_NUMBEROFROWS = -538;
        public const int DISPID_OBJECT = DISPID_SITE + 1000;
        public const int DISPID_OMDOCUMENT = DISPID_NORMAL_FIRST;
        public const int DISPID_ONABORT = DISPID_NORMAL_FIRST;
        public const int DISPID_ONACTIVATE = DISPID_NORMAL_FIRST + 44;
        public const int DISPID_ONAFTERPRINT = DISPID_NORMAL_FIRST + 25;
        public const int DISPID_ONBEFOREACTIVATE = DISPID_NORMAL_FIRST + 47;
        public const int DISPID_ONBEFOREDEACTIVATE = DISPID_NORMAL_FIRST + 34;
        public const int DISPID_ONBEFOREEDITFOCUS = DISPID_NORMAL_FIRST + 27;
        public const int DISPID_ONBEFOREPRINT = DISPID_NORMAL_FIRST + 24;
        public const int DISPID_ONBEFOREUNLOAD = DISPID_NORMAL_FIRST + 17;
        public const int DISPID_ONBOUNCE = DISPID_NORMAL_FIRST + 9;
        public const int DISPID_ONCHANGE = DISPID_NORMAL_FIRST + 1;
        public const int DISPID_ONCHANGEBLUR = DISPID_NORMAL_FIRST + 19;
        public const int DISPID_ONCHANGEFOCUS = DISPID_NORMAL_FIRST + 18;
        public const int DISPID_ONCONTENTREADY = DISPID_NORMAL_FIRST + 29;
        public const int DISPID_ONCONTEXTMENU = DISPID_NORMAL_FIRST + 23;
        public const int DISPID_ONCONTROLSELECT = DISPID_NORMAL_FIRST + 36;
        public const int DISPID_ONDEACTIVATE = DISPID_NORMAL_FIRST + 45;
        public const int DISPID_ONERROR = DISPID_NORMAL_FIRST + 2;
        public const int DISPID_ONFINISH = DISPID_NORMAL_FIRST + 10;
        public const int DISPID_ONFOCUSIN = DISPID_NORMAL_FIRST + 48;
        public const int DISPID_ONFOCUSOUT = DISPID_NORMAL_FIRST + 49;
        public const int DISPID_ONLAYOUT = DISPID_NORMAL_FIRST + 13;
        public const int DISPID_ONLAYOUTCOMPLETE = DISPID_NORMAL_FIRST + 30;
        public const int DISPID_ONLINKEDOVERFLOW = DISPID_NORMAL_FIRST + 32;
        public const int DISPID_ONLOAD = DISPID_NORMAL_FIRST + 3;
        public const int DISPID_ONMOUSEENTER = DISPID_NORMAL_FIRST + 42;
        public const int DISPID_ONMOUSEHOVER = DISPID_NORMAL_FIRST + 28;
        public const int DISPID_ONMOUSELEAVE = DISPID_NORMAL_FIRST + 43;
        public const int DISPID_ONMOUSEWHEEL = DISPID_NORMAL_FIRST + 33;
        public const int DISPID_ONMOVE = DISPID_NORMAL_FIRST + 35;
        public const int DISPID_ONMOVEEND = DISPID_NORMAL_FIRST + 39;
        public const int DISPID_ONMOVESTART = DISPID_NORMAL_FIRST + 38;
        public const int DISPID_ONMULTILAYOUTCLEANUP = DISPID_NORMAL_FIRST + 46;
        public const int DISPID_ONPAGE = DISPID_NORMAL_FIRST + 31;
        public const int DISPID_ONPERSIST = DISPID_NORMAL_FIRST + 20;
        public const int DISPID_ONPERSISTLOAD = DISPID_NORMAL_FIRST + 22;
        public const int DISPID_ONPERSISTSAVE = DISPID_NORMAL_FIRST + 21;
        public const int DISPID_ONRESET = DISPID_NORMAL_FIRST + 15;
        public const int DISPID_ONRESIZE = DISPID_NORMAL_FIRST + 16;
        public const int DISPID_ONRESIZEEND = DISPID_NORMAL_FIRST + 41;
        public const int DISPID_ONRESIZESTART = DISPID_NORMAL_FIRST + 40;
        public const int DISPID_ONSCROLL = DISPID_NORMAL_FIRST + 14;
        public const int DISPID_ONSELECT = DISPID_NORMAL_FIRST + 6;
        public const int DISPID_ONSELECTIONCHANGE = DISPID_NORMAL_FIRST + 37;
        public const int DISPID_ONSTART = DISPID_NORMAL_FIRST + 11;
        public const int DISPID_ONSTOP = DISPID_NORMAL_FIRST + 26;
        public const int DISPID_ONSUBMIT = DISPID_NORMAL_FIRST + 7;
        public const int DISPID_ONUNLOAD = DISPID_NORMAL_FIRST + 8;
        public const int DISPID_OPTION = DISPID_NORMAL_FIRST;
        public const int DISPID_PASSWORDCHAR = -534;
        public const int DISPID_PICTURE = -523;
        public const int DISPID_PROPERTYPUT = -3;
        public const int DISPID_RANGE = DISPID_NORMAL_FIRST;
        public const int DISPID_READYSTATE = -525;
        public const int DISPID_READYSTATECHANGE = -609;
        public const int DISPID_REFRESH = -550;
        public const int DISPID_REMOVEITEM = -555;
        public const int DISPID_RICHTEXT = DISPID_MARQUEE + 1000;
        public const int DISPID_RIGHTTOLEFT = -611;
        public const int DISPID_SCRIPT = DISPID_NORMAL_FIRST;
        public const int DISPID_SCROLLBARS = -535;
        public const int DISPID_SELECT = DISPID_NORMAL_FIRST;
        public const int DISPID_SELECTED = -527;
        public const int DISPID_SELECTOBJ = DISPID_NORMAL_FIRST;
        public const int DISPID_SELLENGTH = -548;
        public const int DISPID_SELSTART = -547;
        public const int DISPID_SELTEXT = -546;
        public const int DISPID_SITE = DISPID_ELEMENT + 1000;
        public const int DISPID_STYLE = DISPID_OBJECT + 1000;
        public const int DISPID_STYLESHEETS_COL = DISPID_NORMAL_FIRST;
        public const int DISPID_TABKEYBEHAVIOR = -545;
        public const int DISPID_TABLE = DISPID_NORMAL_FIRST;
        public const int DISPID_TABLECELL = DISPID_TEXTSITE + 1000;
        public const int DISPID_TABLECOL = DISPID_NORMAL_FIRST;
        public const int DISPID_TABLEROW = DISPID_NORMAL_FIRST;
        public const int DISPID_TABLESECTION = DISPID_NORMAL_FIRST;
        public const int DISPID_TABSTOP = -516;
        public const int DISPID_TEXT = -517;
        public const int DISPID_TEXTAREA = DISPID_INPUTTEXT + 1000;
        public const int DISPID_TEXTSITE = DISPID_NORMAL_FIRST;
        public const int DISPID_THIS = -613;
        public const int DISPID_TOPTOBOTTOM = -612;
        public const int DISPID_UNKNOWN = -1;
        public const int DISPID_VALID = -524;
        public const int DISPID_VALUE = 0;
        public const int DISPID_WORDWRAP = -536;
        public const int DISPID_XOBJ_BASE = DISPID_XOBJ_MIN;
        public const int DISPID_XOBJ_EXPANDO = DISPID_EVENTS + 1000;
        public const int DISPID_XOBJ_MAX = -2147352577;
        public const int DISPID_XOBJ_MIN = -2147418112;
        public const int DISPID_XOBJ_ORDINAL = DISPID_XOBJ_EXPANDO + 1000;
        public const int STDDISPID_XOBJ_AFTERUPDATE = DISPID_XOBJ_BASE + 5;
        public const int STDDISPID_XOBJ_BEFOREUPDATE = DISPID_XOBJ_BASE + 4;
        public const int STDDISPID_XOBJ_ERRORUPDATE = DISPID_XOBJ_BASE + 13;
        public const int STDDISPID_XOBJ_ONBEFORECOPY = DISPID_XOBJ_BASE + 30;
        public const int STDDISPID_XOBJ_ONBEFORECUT = DISPID_XOBJ_BASE + 29;
        public const int STDDISPID_XOBJ_ONBEFOREPASTE = DISPID_XOBJ_BASE + 31;
        public const int STDDISPID_XOBJ_ONBLUR = DISPID_XOBJ_BASE;
        public const int STDDISPID_XOBJ_ONCELLCHANGE = DISPID_XOBJ_BASE + 34;
        public const int STDDISPID_XOBJ_ONCOPY = DISPID_XOBJ_BASE + 27;
        public const int STDDISPID_XOBJ_ONCUT = DISPID_XOBJ_BASE + 26;
        public const int STDDISPID_XOBJ_ONDATAAVAILABLE = DISPID_XOBJ_BASE + 15;
        public const int STDDISPID_XOBJ_ONDATASETCHANGED = DISPID_XOBJ_BASE + 14;
        public const int STDDISPID_XOBJ_ONDATASETCOMPLETE = DISPID_XOBJ_BASE + 16;
        public const int STDDISPID_XOBJ_ONDRAG = DISPID_XOBJ_BASE + 20;
        public const int STDDISPID_XOBJ_ONDRAGEND = DISPID_XOBJ_BASE + 21;
        public const int STDDISPID_XOBJ_ONDRAGENTER = DISPID_XOBJ_BASE + 22;
        public const int STDDISPID_XOBJ_ONDRAGLEAVE = DISPID_XOBJ_BASE + 24;
        public const int STDDISPID_XOBJ_ONDRAGOVER = DISPID_XOBJ_BASE + 23;
        public const int STDDISPID_XOBJ_ONDRAGSTART = DISPID_XOBJ_BASE + 11;
        public const int STDDISPID_XOBJ_ONDROP = DISPID_XOBJ_BASE + 25;
        public const int STDDISPID_XOBJ_ONFILTER = DISPID_XOBJ_BASE + 17;
        public const int STDDISPID_XOBJ_ONFOCUS = DISPID_XOBJ_BASE + 1;
        public const int STDDISPID_XOBJ_ONHELP = DISPID_XOBJ_BASE + 10;
        public const int STDDISPID_XOBJ_ONLOSECAPTURE = DISPID_XOBJ_BASE + 18;
        public const int STDDISPID_XOBJ_ONMOUSEOUT = DISPID_XOBJ_BASE + 9;
        public const int STDDISPID_XOBJ_ONMOUSEOVER = DISPID_XOBJ_BASE + 8;
        public const int STDDISPID_XOBJ_ONPASTE = DISPID_XOBJ_BASE + 28;
        public const int STDDISPID_XOBJ_ONPROPERTYCHANGE = DISPID_XOBJ_BASE + 19;
        public const int STDDISPID_XOBJ_ONROWENTER = DISPID_XOBJ_BASE + 7;
        public const int STDDISPID_XOBJ_ONROWEXIT = DISPID_XOBJ_BASE + 6;
        public const int STDDISPID_XOBJ_ONROWSDELETE = DISPID_XOBJ_BASE + 32;
        public const int STDDISPID_XOBJ_ONROWSINSERTED = DISPID_XOBJ_BASE + 33;
        public const int STDDISPID_XOBJ_ONSELECTSTART = DISPID_XOBJ_BASE + 12;
        public const int STDPROPID_XOBJ_BLOCKALIGN = DISPID_XOBJ_BASE + 0x48;
        public const int STDPROPID_XOBJ_BOTTOM = DISPID_XOBJ_BASE + 0x4E;
        public const int STDPROPID_XOBJ_CONTROLALIGN = DISPID_XOBJ_BASE + 0x49;
        public const int STDPROPID_XOBJ_CONTROLTIPTEXT = DISPID_XOBJ_BASE + 0x45;
        public const int STDPROPID_XOBJ_DISABLED = DISPID_XOBJ_BASE + 0x4C; //+76
        public const int STDPROPID_XOBJ_HEIGHT = DISPID_XOBJ_BASE + 0x6;
        public const int STDPROPID_XOBJ_LEFT = DISPID_XOBJ_BASE + 0x3;
        public const int STDPROPID_XOBJ_NAME = DISPID_XOBJ_BASE + 0x0;
        public const int STDPROPID_XOBJ_PARENT = DISPID_XOBJ_BASE + 0x8;
        public const int STDPROPID_XOBJ_RIGHT = DISPID_XOBJ_BASE + 0x4D;
        public const int STDPROPID_XOBJ_STYLE = DISPID_XOBJ_BASE + 0x4A;
        public const int STDPROPID_XOBJ_TABINDEX = DISPID_XOBJ_BASE + 0xF;
        public const int STDPROPID_XOBJ_TOP = DISPID_XOBJ_BASE + 0x4;
        public const int STDPROPID_XOBJ_WIDTH = DISPID_XOBJ_BASE + 0x5;
    }

    public sealed class WindowsHookUtil
    {
        //public struct HookInfo
        //{
        //    //This is a Unique MsgID which is registered by the DLL
        //    //and passed to the client when hooking starts, each hook
        //    //has it's own MsgID. Used in WndProc to capture hook notifications
        //    public int UniqueMsgID;
        //    public CSEXWBDLMANLib.WINDOWSHOOK_TYPES HookType;
        //    public bool IsHooked;
        //    public HookInfo(CSEXWBDLMANLib.WINDOWSHOOK_TYPES _HookType)
        //    {
        //        this.HookType = _HookType;
        //        UniqueMsgID = 0;
        //        this.IsHooked = false;
        //    }
        //}

        //
        //LL Keyboard hook
        //
        public const int HCBT_ACTIVATE = 5;
        public const int HCBT_CLICKSKIPPED = 6;
        public const int HCBT_CREATEWND = 3;
        public const int HCBT_DESTROYWND = 4;
        public const int HCBT_KEYSKIPPED = 7;
        public const int HCBT_MINMAX = 1;
        public const int HCBT_MOVESIZE = 0;
        public const int HCBT_QS = 2;
        public const int HCBT_SETFOCUS = 9;
        public const int HCBT_SYSCOMMAND = 8;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class CBT_CREATEWND
        {
            public IntPtr hwndInsertAfter;
            public IntPtr lpcs; //CREATESTRUCT
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class CREATESTRUCT
        {
            public int cx;
            public int cy;
            public int dwExStyle;
            public IntPtr hInstance;
            public IntPtr hMenu;
            public IntPtr hwndParent;
            public IntPtr lpCreateParams;
            public IntPtr lpszClass;
            public IntPtr lpszName;
            public int style;
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            public IntPtr dwExtraInfo = IntPtr.Zero;

            [MarshalAs(UnmanagedType.I4)]
            public int flags;

            [MarshalAs(UnmanagedType.I4)]
            public int scanCode;

            [MarshalAs(UnmanagedType.I4)]
            public int time;

            [MarshalAs(UnmanagedType.I4)]
            public int vkCode;

            public void Reset()
            {
                this.vkCode = 0;
                this.flags = 0;
                this.scanCode = 0;
                this.time = 0;
                this.dwExtraInfo = IntPtr.Zero;
            }
        }
    }

    public struct HTML_PAINT_XFORM
    {
        public float eDx;
        public float eDy;
        public float eM11;
        public float eM12;
        public float eM21;
        public float eM22;
    }

    public struct HTML_PAINT_DRAW_INFO
    {
        public uint hrgnUpdate;
        public tagRECT rcViewport;
        public HTML_PAINT_XFORM xform;
    }

    public struct HTML_PAINTER_INFO
    {
        public int iidDrawObject;
        public int lFlags;
        public int lZOrder;
        public tagRECT rcExpand;
    }

    /// <summary>
    ///     Used to handle Travel log entries
    /// </summary>
    public struct TravelLogStruct
    {
        public string Title;
        public string URL;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct URLINVOKECOMMANDINFOA
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint dwcbSize;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwFlags;

        public IntPtr hwndParent;

        [MarshalAs(UnmanagedType.LPStr)]
        public string pcszVerb;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct URLINVOKECOMMANDINFOW
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint dwcbSize;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwFlags;

        public IntPtr hwndParent;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string pcszVerb;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STATURL
    {
        public uint cbSize;

        /// <summary>
        ///     The specified URL.The calling function must free this parameter.
        ///     Set this parameter to STATURL_QUERYFLAG_NOURL if no URL is specified.
        /// </summary>
        public IntPtr pwcsUrl;

        /// <summary>
        ///     The title of the Web page, as contained in the title tags.
        ///     This calling application must free this parameter.
        ///     Set this parameter to STATURL_QUERYFLAG_NOTITLE if no Web page is specified.
        /// </summary>
        public IntPtr pwcsTitle;

        public FILETIME ftLastVisited;
        public FILETIME ftLastUpdated;
        public FILETIME ftExpires;
        public uint dwFlags;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct WIN32_FIND_DATAA
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint dwFileAttributes;

        public FILETIME ftCreationTime;
        public FILETIME ftLastAccessTime;
        public FILETIME ftLastWriteTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
        public uint dwReserved0;
        public uint dwReserved1;

        [MarshalAs(UnmanagedType.LPStr, SizeConst = 260)]
        public StringBuilder cFileName;

        [MarshalAs(UnmanagedType.LPStr, SizeConst = 14)]
        public StringBuilder cAlternateFileName;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct WIN32_FIND_DATAW
    {
        public uint dwFileAttributes;
        public FILETIME ftCreationTime;
        public FILETIME ftLastAccessTime;
        public FILETIME ftLastWriteTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
        public uint dwReserved0;
        public uint dwReserved1;

        [MarshalAs(UnmanagedType.LPWStr, SizeConst = 260)]
        public StringBuilder cFileName;

        [MarshalAs(UnmanagedType.LPWStr, SizeConst = 14)]
        public StringBuilder cAlternateFileName;
    }

    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct tagDVASPECTINFO
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint cb;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwFlags;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct DVTARGETDEVICE
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint tdSize;

        [MarshalAs(UnmanagedType.U2)]
        public short tdDriverNameOffset;

        [MarshalAs(UnmanagedType.U2)]
        public short tdDeviceNameOffset;

        [MarshalAs(UnmanagedType.U2)]
        public short tdPortNameOffset;

        [MarshalAs(UnmanagedType.U2)]
        public short tdExtDevmodeOffset;

        [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)]
        public byte[] tdData;
    }

    public struct tagPALETTEENTRY
    {
        public byte peBlue;
        public byte peFlags;
        public byte peGreen;
        public byte peRed;
    }

    public struct tagLOGPALETTE
    {
        [MarshalAs(UnmanagedType.U2)]
        public short palNumEntries;

        [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)]
        public tagPALETTEENTRY[] palPalEntry;

        [MarshalAs(UnmanagedType.U2)]
        public short palVersion;
    }

    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct tagPOINT
    {
        [MarshalAs(UnmanagedType.I4)]
        public int X;

        [MarshalAs(UnmanagedType.I4)]
        public int Y;

        //public tagPOINT(int x, int y)
        //{
        //    this.X = x;
        //    this.Y = y;
        //}

        //public static implicit operator System.Drawing.Point(tagPOINT p)
        //{
        //    return new System.Drawing.Point(p.X, p.Y);
        //}

        //public static implicit operator tagPOINT(System.Drawing.Point p)
        //{
        //    return new tagPOINT(p.X, p.Y);
        //}
    }

    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct tagSIZE
    {
        [MarshalAs(UnmanagedType.I4)]
        public int cx;

        [MarshalAs(UnmanagedType.I4)]
        public int cy;

        //public tagSIZE(int cx, int cy)
        //{
        //    this.cx = cx;
        //    this.cy = cy;
        //}
    }

    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct tagSIZEL
    {
        [MarshalAs(UnmanagedType.I4)]
        public int cx;

        [MarshalAs(UnmanagedType.I4)]
        public int cy;
    }

    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct tagRECT
    {
        [MarshalAs(UnmanagedType.I4)]
        public int Left;

        [MarshalAs(UnmanagedType.I4)]
        public int Top;

        [MarshalAs(UnmanagedType.I4)]
        public int Right;

        [MarshalAs(UnmanagedType.I4)]
        public int Bottom;

        public tagRECT(int left_, int top_, int right_, int bottom_)
        {
            this.Left = left_;
            this.Top = top_;
            this.Right = right_;
            this.Bottom = bottom_;
        }

        //public int Height { get { return Bottom - Top + 1; } }
        //public int Width { get { return Right - Left + 1; } }
        //public tagSIZE Size { get { return new tagSIZE(Width, Height); } }
        //public tagPOINT Location { get { return new tagPOINT(Left, Top); } }

        //// Handy method for converting to a System.Drawing.Rectangle
        //public System.Drawing.Rectangle ToRectangle()
        //{ return System.Drawing.Rectangle.FromLTRB(Left, Top, Right, Bottom); }

        //public static tagRECT FromRectangle(System.Drawing.Rectangle rectangle)
        //{
        //    return new tagRECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
        //}

        //public override int GetHashCode()
        //{
        //    return Left ^ ((Top << 13) | (Top >> 0x13))
        //      ^ ((Width << 0x1a) | (Width >> 6))
        //      ^ ((Height << 7) | (Height >> 0x19));
        //}

        //#region Operator overloads

        //public static implicit operator System.Drawing.Rectangle(tagRECT rect)
        //{
        //    return System.Drawing.Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
        //}

        //public static implicit operator tagRECT(System.Drawing.Rectangle rect)
        //{
        //    return new tagRECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
        //}

        //#endregion
    }

    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct tagMSG
    {
        public IntPtr hwnd;

        [MarshalAs(UnmanagedType.I4)]
        public int message;

        public IntPtr wParam;
        public IntPtr lParam;

        [MarshalAs(UnmanagedType.I4)]
        public int time;

        // pt was a by-value POINT structure
        [MarshalAs(UnmanagedType.I4)]
        public int pt_x;

        [MarshalAs(UnmanagedType.I4)]
        public int pt_y;

        //public tagPOINT pt;
    }

    //typedef struct _DOCHOSTUIINFO
    //{
    //ULONG cbSize;
    //DWORD dwFlags;
    //DWORD dwDoubleClick;
    //OLECHAR *pchHostCss;
    //OLECHAR *pchHostNS;
    //} 	DOCHOSTUIINFO;
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct DOCHOSTUIINFO
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint cbSize;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwFlags;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwDoubleClick;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string pchHostCss;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string pchHostNS;
    }

    //typedef struct tagOLEVERB
    //{
    //LONG lVerb;
    //LPOLESTR lpszVerbName;
    //DWORD fuFlags;
    //DWORD grfAttribs;
    //} 	OLEVERB;
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct tagOLEVERB
    {
        [MarshalAs(UnmanagedType.I4)]
        public int lVerb;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszVerbName;

        [MarshalAs(UnmanagedType.U4)]
        public uint fuFlags;

        [MarshalAs(UnmanagedType.U4)]
        public uint grfAttribs;
    }

    //typedef struct _tagOLECMD
    //{
    //ULONG cmdID;
    //DWORD cmdf;
    //} 	OLECMD;
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct tagOLECMD
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint cmdID;

        [MarshalAs(UnmanagedType.U4)]
        public uint cmdf;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OLECMDTEXT
    {
        public uint cmdtextf;
        public uint cwActual;
        public uint cwBuf;
        public char rgwz;
    }

    //typedef struct tagOleMenuGroupWidths
    //    {
    //    LONG width[ 6 ];
    //    } 	OLEMENUGROUPWIDTHS;
    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential)]
    public struct tagOleMenuGroupWidths
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public int[] widths;
    }

    //typedef /* [unique] */ IOleInPlaceFrame *LPOLEINPLACEFRAME;
    //typedef struct tagOIFI
    //    {
    //    UINT cb;
    //    BOOL fMDIApp;
    //    HWND hwndFrame;
    //    HACCEL haccel;
    //    UINT cAccelEntries;
    //    } 	OLEINPLACEFRAMEINFO;
    //typedef struct tagOIFI *LPOLEINPLACEFRAMEINFO;
    [StructLayout(LayoutKind.Sequential)]
    public struct tagOIFI
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint cb;

        [MarshalAs(UnmanagedType.Bool)]
        public bool fMDIApp;

        public IntPtr hwndFrame;
        public IntPtr hAccel;

        [MarshalAs(UnmanagedType.U4)]
        public uint cAccelEntries;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STGMEDIUM
    {
        [MarshalAs(UnmanagedType.U4)]
        public int tymed;

        public IntPtr data;

        [MarshalAs(UnmanagedType.IUnknown)]
        public object pUnkForRelease;
    }

    ////typedef struct _REMSECURITY_ATTRIBUTES
    ////{
    ////DWORD nLength;
    ////DWORD lpSecurityDescriptor;
    ////BOOL bInheritHandle;
    ////} 	REMSECURITY_ATTRIBUTES;
    [StructLayout(LayoutKind.Sequential)]
    public struct SECURITY_ATTRIBUTES
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint nLength;

        public IntPtr lpSecurityDescriptor;

        [MarshalAs(UnmanagedType.Bool)]
        public bool bInheritHandle;
    }

    //typedef struct _tagBINDINFO
    //{
    //ULONG cbSize;
    //LPWSTR szExtraInfo;
    //STGMEDIUM stgmedData;
    //DWORD grfBindInfoF;
    //DWORD dwBindVerb;
    //LPWSTR szCustomVerb;
    //DWORD cbstgmedData;
    //DWORD dwOptions;
    //DWORD dwOptionsFlags;
    //DWORD dwCodePage;
    //SECURITY_ATTRIBUTES securityAttributes;
    //IID iid;
    //IUnknown *pUnk;
    //DWORD dwReserved;
    //} 	BINDINFO;
    [StructLayout(LayoutKind.Sequential)]
    public struct BINDINFO
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint cbSize;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string szExtraInfo;

        [MarshalAs(UnmanagedType.Struct)]
        public STGMEDIUM stgmedData;

        [MarshalAs(UnmanagedType.U4)]
        public uint grfBindInfoF;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwBindVerb;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string szCustomVerb;

        [MarshalAs(UnmanagedType.U4)]
        public uint cbstgmedData;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwOptions;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwOptionsFlags;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwCodePage;

        [MarshalAs(UnmanagedType.Struct)]
        public SECURITY_ATTRIBUTES securityAttributes;

        public Guid iid;

        [MarshalAs(UnmanagedType.IUnknown)]
        public object punk;

        [MarshalAs(UnmanagedType.U4)]
        public uint dwReserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FILETIME
    {
        public uint dwLowDateTime;
        public uint dwHighDateTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME
    {
        public ushort Year;
        public ushort Month;
        public ushort DayOfWeek;
        public ushort Day;
        public ushort Hour;
        public ushort Minute;
        public ushort Second;
        public ushort Milliseconds;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct INTERNET_CACHE_ENTRY_INFO
    {
        public uint dwStructSize;
        public string lpszSourceUrlName;
        public string lpszLocalFileName;
        public uint CacheEntryType;
        public uint dwUseCount;
        public uint dwHitRate;
        public uint dwSizeLow;
        public uint dwSizeHigh;
        public FILETIME LastModifiedTime;
        public FILETIME ExpireTime;
        public FILETIME LastAccessTime;
        public FILETIME LastSyncTime;
        public IntPtr lpHeaderInfo;
        public uint dwHeaderInfoSize;
        public string lpszFileExtension;
        public uint dwExemptDelta;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STARTUPINFO
    {
        public int cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public int dwX;
        public int dwY;
        public int dwXSize;
        public int dwXCountChars;
        public int dwYCountChars;
        public int dwFillAttribute;
        public int dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESS_INFORMATION
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public int dwProcessID;
        public int dwThreadID;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TokenPrivileges
    {
        public int PrivilegeCount;
        public long Luid;
        public int Attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT : IEquatable<RECT>
    {
        public RECT(Rectangle rectangle)
            : this(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom) {}

        public RECT(int left, int top, int right, int bottom)
        {
            this.X = left;
            this.Y = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Left { get { return this.X; } set { this.X = value; } }

        public int Top { get { return this.Y; } set { this.Y = value; } }

        public int Right { get; set; }

        public int Bottom { get; set; }

        public int Height { get { return this.Bottom - this.Y; } set { this.Bottom = value - this.Y; } }

        public int Width { get { return this.Right - this.X; } set { this.Right = value + this.X; } }

        public Point Location
        {
            get { return new Point(this.Left, this.Top); }
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public Size Size
        {
            get { return new Size(this.Width, this.Height); }
            set
            {
                this.Right = value.Height + this.X;
                this.Bottom = value.Height + this.Y;
            }
        }

        public Rectangle ToRectangle() { return new Rectangle(this.Left, this.Top, this.Width, this.Height); }

        public static Rectangle ToRectangle(RECT rectangle) { return rectangle.ToRectangle(); }

        public static RECT FromRectangle(Rectangle rectangle) { return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom); }

        public static implicit operator Rectangle(RECT rectangle) { return rectangle.ToRectangle(); }

        public static implicit operator RECT(Rectangle rectangle) { return new RECT(rectangle); }

        public static bool operator ==(RECT rectangle1, RECT rectangle2) { return rectangle1.Equals(rectangle2); }

        public static bool operator !=(RECT rectangle1, RECT rectangle2) { return !rectangle1.Equals(rectangle2); }

        public override string ToString() { return "{Left: " + this.X + "; " + "Top: " + this.Y + "; Right: " + this.Right + "; Bottom: " + this.Bottom + "}"; }

        public bool Equals(RECT rectangle)
        {
            return rectangle.X == this.X && rectangle.Y == this.Y && rectangle.Right == this.Right && rectangle.Bottom == this.Bottom;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj.GetType() == typeof(RECT) && this.Equals((RECT) obj);
        }

        /// <summary>
        ///     Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                var result = this.X;
                result = (result * 397) ^ this.Y;
                result = (result * 397) ^ this.Right;
                result = (result * 397) ^ this.Bottom;
                return result;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public long x;
        public long y;
    }

    public struct PALTTABINFO
    {
        public ulong cbSize;
        public int cColumns;
        public int cItems;
        public int cRows;
        public int cxItem;
        public int cyItem;
        public int iColFocus;
        public int iRowFocus;
        public POINT ptStart;
    }

    public struct LPRECT
    {
        public long bottom;
        public long left;
        public long right;
        public long top;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SHFILEOPSTRUCT
    {
        public IntPtr hwnd;
        public uint wFunc;
        public IntPtr pFrom;
        public IntPtr pTo;
        public ushort fFlags;

        public int fAnyOperationsAborted;

        public IntPtr hNameMappings;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszProgressTitle;

        // Address of a string to use as the title of 
        // a progress dialog box. This member is used 
        // only if fFlags includes the 
        // FOF_SIMPLEPROGRESS flag.
    }

    // ReSharper restore InconsistentNaming
}