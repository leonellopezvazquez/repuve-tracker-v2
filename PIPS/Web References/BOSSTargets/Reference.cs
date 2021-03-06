﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace PIPS.BOSSTargets {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="TargetsServiceSoap", Namespace="http://tempuri.org/")]
    public partial class TargetsService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetTargetsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public TargetsService() {
            this.Url = "http://localhost:8088/BOSS/Services/TargetsService.asmx";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetTargetsCompletedEventHandler GetTargetsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetTargets", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public Target[] GetTargets(string clientName) {
            object[] results = this.Invoke("GetTargets", new object[] {
                        clientName});
            return ((Target[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetTargets(string clientName, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetTargets", new object[] {
                        clientName}, callback, asyncState);
        }
        
        /// <remarks/>
        public Target[] EndGetTargets(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((Target[])(results[0]));
        }
        
        /// <remarks/>
        public void GetTargetsAsync(string clientName) {
            this.GetTargetsAsync(clientName, null);
        }
        
        /// <remarks/>
        public void GetTargetsAsync(string clientName, object userState) {
            if ((this.GetTargetsOperationCompleted == null)) {
                this.GetTargetsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetTargetsOperationCompleted);
            }
            this.InvokeAsync("GetTargets", new object[] {
                        clientName}, this.GetTargetsOperationCompleted, userState);
        }
        
        private void OnGetTargetsOperationCompleted(object arg) {
            if ((this.GetTargetsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetTargetsCompleted(this, new GetTargetsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.3190.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    public partial class Target {
        
        private string addressField;
        
        private string fullNameField;
        
        private string birthplaceField;
        
        private string categoryField;
        
        private string ethnicityField;
        
        private string informationField;
        
        private System.DateTime dateOfBirthField;
        
        private string pNCIDField;
        
        private string loginNameField;
        
        private byte[] imageField;
        
        private string warningField;
        
        /// <remarks/>
        public string Address {
            get {
                return this.addressField;
            }
            set {
                this.addressField = value;
            }
        }
        
        /// <remarks/>
        public string FullName {
            get {
                return this.fullNameField;
            }
            set {
                this.fullNameField = value;
            }
        }
        
        /// <remarks/>
        public string Birthplace {
            get {
                return this.birthplaceField;
            }
            set {
                this.birthplaceField = value;
            }
        }
        
        /// <remarks/>
        public string Category {
            get {
                return this.categoryField;
            }
            set {
                this.categoryField = value;
            }
        }
        
        /// <remarks/>
        public string Ethnicity {
            get {
                return this.ethnicityField;
            }
            set {
                this.ethnicityField = value;
            }
        }
        
        /// <remarks/>
        public string Information {
            get {
                return this.informationField;
            }
            set {
                this.informationField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime DateOfBirth {
            get {
                return this.dateOfBirthField;
            }
            set {
                this.dateOfBirthField = value;
            }
        }
        
        /// <remarks/>
        public string PNCID {
            get {
                return this.pNCIDField;
            }
            set {
                this.pNCIDField = value;
            }
        }
        
        /// <remarks/>
        public string LoginName {
            get {
                return this.loginNameField;
            }
            set {
                this.loginNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] Image {
            get {
                return this.imageField;
            }
            set {
                this.imageField = value;
            }
        }
        
        /// <remarks/>
        public string Warning {
            get {
                return this.warningField;
            }
            set {
                this.warningField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void GetTargetsCompletedEventHandler(object sender, GetTargetsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetTargetsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetTargetsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Target[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Target[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591