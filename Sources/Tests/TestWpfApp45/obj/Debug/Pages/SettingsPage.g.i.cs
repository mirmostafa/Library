﻿#pragma checksum "..\..\..\Pages\SettingsPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "73A22304C7883486F9A26233422518D5780058791978CA639A24B5CCCA9DB71A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Win32;
using Mohammad.Wpf.Windows.Controls;
using Mohammad.Wpf.Windows.Input;
using Mohammad.Wpf.Windows.Input.LibCommands;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using TestWpfApp45;


namespace TestWpfApp45.Pages {
    
    
    /// <summary>
    /// SettingsPage
    /// </summary>
    public partial class SettingsPage : Mohammad.Wpf.Windows.Controls.LibraryCommonPage, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\Pages\SettingsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal TestWpfApp45.Pages.SettingsPage AppSettingsPage;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TestWpfApp45;component/pages/settingspage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\SettingsPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.AppSettingsPage = ((TestWpfApp45.Pages.SettingsPage)(target));
            
            #line 13 "..\..\..\Pages\SettingsPage.xaml"
            this.AppSettingsPage.CommonDialogClosing += new System.ComponentModel.CancelEventHandler(this.SettingsPage_OnCommonDialogClosing);
            
            #line default
            #line hidden
            
            #line 16 "..\..\..\Pages\SettingsPage.xaml"
            this.AppSettingsPage.Loaded += new System.Windows.RoutedEventHandler(this.SettingsPage_OnLoaded);
            
            #line default
            #line hidden
            
            #line 17 "..\..\..\Pages\SettingsPage.xaml"
            this.AppSettingsPage.OkButtonClicked += new System.EventHandler<Mohammad.EventsArgs.ActingEventArgs>(this.SettingsPage_OnOkButtonClicked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

