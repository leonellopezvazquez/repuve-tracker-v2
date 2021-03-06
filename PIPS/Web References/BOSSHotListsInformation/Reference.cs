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

namespace PIPS.BOSSHotListsInformation {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="HotListsInformationServiceSoap", Namespace="http://tempuri.org/")]
    public partial class HotListsInformationService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetHotListsInformationOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public HotListsInformationService() {
            this.Url = "http://localhost:8088/BOSS/Services/HotListsInformationService.asmx";
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
        public event GetHotListsInformationCompletedEventHandler GetHotListsInformationCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetHotListsInformation", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public HotList[] GetHotListsInformation(string clientName) {
            object[] results = this.Invoke("GetHotListsInformation", new object[] {
                        clientName});
            return ((HotList[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetHotListsInformation(string clientName, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetHotListsInformation", new object[] {
                        clientName}, callback, asyncState);
        }
        
        /// <remarks/>
        public HotList[] EndGetHotListsInformation(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((HotList[])(results[0]));
        }
        
        /// <remarks/>
        public void GetHotListsInformationAsync(string clientName) {
            this.GetHotListsInformationAsync(clientName, null);
        }
        
        /// <remarks/>
        public void GetHotListsInformationAsync(string clientName, object userState) {
            if ((this.GetHotListsInformationOperationCompleted == null)) {
                this.GetHotListsInformationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetHotListsInformationOperationCompleted);
            }
            this.InvokeAsync("GetHotListsInformation", new object[] {
                        clientName}, this.GetHotListsInformationOperationCompleted, userState);
        }
        
        private void OnGetHotListsInformationOperationCompleted(object arg) {
            if ((this.GetHotListsInformationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetHotListsInformationCompleted(this, new GetHotListsInformationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public partial class HotList {
        
        private bool isCovertField;
        
        private string colorField;
        
        private string nameField;
        
        private long idField;
        
        private string alarmField;
        
        private int priorityField;
        
        private System.DateTime modificationTimeField;
        
        /// <remarks/>
        public bool IsCovert {
            get {
                return this.isCovertField;
            }
            set {
                this.isCovertField = value;
            }
        }
        
        /// <remarks/>
        public string Color {
            get {
                return this.colorField;
            }
            set {
                this.colorField = value;
            }
        }
        
        /// <remarks/>
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        public long ID {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        public string Alarm {
            get {
                return this.alarmField;
            }
            set {
                this.alarmField = value;
            }
        }
        
        /// <remarks/>
        public int Priority {
            get {
                return this.priorityField;
            }
            set {
                this.priorityField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime ModificationTime {
            get {
                return this.modificationTimeField;
            }
            set {
                this.modificationTimeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void GetHotListsInformationCompletedEventHandler(object sender, GetHotListsInformationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetHotListsInformationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetHotListsInformationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public HotList[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((HotList[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591