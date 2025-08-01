﻿#pragma checksum "..\..\..\GameDetailView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6EA72D4E371BDEBF358091CB707D3BD7182C9A91"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using HYDRA.GUI;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace HYDRA.GUI {
    
    
    /// <summary>
    /// GameDetailView
    /// </summary>
    public partial class GameDetailView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 29 "..\..\..\GameDetailView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image CoverArtImage;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\GameDetailView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock PriceTextBlock;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\GameDetailView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddToCartButton;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\..\GameDetailView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TitleTextBlock;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\..\GameDetailView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock DescriptionTextBlock;
        
        #line default
        #line hidden
        
        
        #line 144 "..\..\..\GameDetailView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ReleaseDateTextBlock;
        
        #line default
        #line hidden
        
        
        #line 158 "..\..\..\GameDetailView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock PublisherTextBlock;
        
        #line default
        #line hidden
        
        
        #line 176 "..\..\..\GameDetailView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ItemsControl ScreenshotsItemsControl;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.7.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/HYDRA.GUI;component/gamedetailview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\GameDetailView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.7.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.CoverArtImage = ((System.Windows.Controls.Image)(target));
            return;
            case 2:
            this.PriceTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.AddToCartButton = ((System.Windows.Controls.Button)(target));
            
            #line 85 "..\..\..\GameDetailView.xaml"
            this.AddToCartButton.Click += new System.Windows.RoutedEventHandler(this.AddToCartButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.TitleTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.DescriptionTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.ReleaseDateTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.PublisherTextBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.ScreenshotsItemsControl = ((System.Windows.Controls.ItemsControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

