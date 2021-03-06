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

namespace PIPS.BOF2HotlistsUpdates {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="GetHotlistUpdatesServiceSoapBinding", Namespace="http://80.177.196.221:8081/bof2/services/GetHotlistUpdatesService")]
    public partial class UpdateHotlistsServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback getHotlistUpdatesOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public UpdateHotlistsServiceService() {
            this.Url = "http://80.177.196.221:8081/bof2/services/GetHotlistUpdatesService";
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
        public event getHotlistUpdatesCompletedEventHandler getHotlistUpdatesCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="urn:GetHotlistUpdatesService", ResponseNamespace="http://80.177.196.221:8081/bof2/services/GetHotlistUpdatesService")]
        [return: System.Xml.Serialization.SoapElementAttribute("getHotlistUpdatesReturn")]
        public BofHotlistData getHotlistUpdates(string signature, string userName, string sourceId, string hotlistName) {
            object[] results = this.Invoke("getHotlistUpdates", new object[] {
                        signature,
                        userName,
                        sourceId,
                        hotlistName});
            return ((BofHotlistData)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegingetHotlistUpdates(string signature, string userName, string sourceId, string hotlistName, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("getHotlistUpdates", new object[] {
                        signature,
                        userName,
                        sourceId,
                        hotlistName}, callback, asyncState);
        }
        
        /// <remarks/>
        public BofHotlistData EndgetHotlistUpdates(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((BofHotlistData)(results[0]));
        }
        
        /// <remarks/>
        public void getHotlistUpdatesAsync(string signature, string userName, string sourceId, string hotlistName) {
            this.getHotlistUpdatesAsync(signature, userName, sourceId, hotlistName, null);
        }
        
        /// <remarks/>
        public void getHotlistUpdatesAsync(string signature, string userName, string sourceId, string hotlistName, object userState) {
            if ((this.getHotlistUpdatesOperationCompleted == null)) {
                this.getHotlistUpdatesOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetHotlistUpdatesOperationCompleted);
            }
            this.InvokeAsync("getHotlistUpdates", new object[] {
                        signature,
                        userName,
                        sourceId,
                        hotlistName}, this.getHotlistUpdatesOperationCompleted, userState);
        }
        
        private void OngetHotlistUpdatesOperationCompleted(object arg) {
            if ((this.getHotlistUpdatesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getHotlistUpdatesCompleted(this, new getHotlistUpdatesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://getHotlistUpdates")]
    public partial class BofHotlistData {
        
        private bool fileTooBigField;
        
        private byte[] hotlistDeltasField;
        
        private string hotlistNameField;
        
        private string latestRevisionField;
        
        /// <remarks/>
        public bool fileTooBig {
            get {
                return this.fileTooBigField;
            }
            set {
                this.fileTooBigField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(DataType="base64Binary", IsNullable=true)]
        public byte[] hotlistDeltas {
            get {
                return this.hotlistDeltasField;
            }
            set {
                this.hotlistDeltasField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string hotlistName {
            get {
                return this.hotlistNameField;
            }
            set {
                this.hotlistNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string latestRevision {
            get {
                return this.latestRevisionField;
            }
            set {
                this.latestRevisionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void getHotlistUpdatesCompletedEventHandler(object sender, getHotlistUpdatesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getHotlistUpdatesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getHotlistUpdatesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public BofHotlistData Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((BofHotlistData)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591