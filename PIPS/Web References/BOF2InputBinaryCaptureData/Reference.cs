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

namespace PIPS.BOF2InputBinaryCaptureData {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="InputBinaryCaptureDataWebServiceSoapBinding", Namespace="http://80.177.196.221:8081/bof2/services/InputBinaryCaptureDataWebService")]
    public partial class InputBinaryDataWebServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback addBinaryCaptureDataOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public InputBinaryDataWebServiceService() {
            this.Url = "http://80.177.196.221:8081/bof2/services/InputBinaryCaptureDataWebService";
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
        public event addBinaryCaptureDataCompletedEventHandler addBinaryCaptureDataCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://input.webservices.bof2.anite.com", ResponseNamespace="http://80.177.196.221:8081/bof2/services/InputBinaryCaptureDataWebService")]
        [return: System.Xml.Serialization.SoapElementAttribute("addBinaryCaptureDataReturn")]
        public string addBinaryCaptureData(string signature, string username, string vrm, short feedIdentifier, short sourceIdentifier, short cameraIdentifier, System.DateTime captureTime, [System.Xml.Serialization.SoapElementAttribute(DataType="base64Binary")] byte[] binaryImage, string binaryDataType) {
            object[] results = this.Invoke("addBinaryCaptureData", new object[] {
                        signature,
                        username,
                        vrm,
                        feedIdentifier,
                        sourceIdentifier,
                        cameraIdentifier,
                        captureTime,
                        binaryImage,
                        binaryDataType});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginaddBinaryCaptureData(string signature, string username, string vrm, short feedIdentifier, short sourceIdentifier, short cameraIdentifier, System.DateTime captureTime, byte[] binaryImage, string binaryDataType, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("addBinaryCaptureData", new object[] {
                        signature,
                        username,
                        vrm,
                        feedIdentifier,
                        sourceIdentifier,
                        cameraIdentifier,
                        captureTime,
                        binaryImage,
                        binaryDataType}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndaddBinaryCaptureData(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void addBinaryCaptureDataAsync(string signature, string username, string vrm, short feedIdentifier, short sourceIdentifier, short cameraIdentifier, System.DateTime captureTime, byte[] binaryImage, string binaryDataType) {
            this.addBinaryCaptureDataAsync(signature, username, vrm, feedIdentifier, sourceIdentifier, cameraIdentifier, captureTime, binaryImage, binaryDataType, null);
        }
        
        /// <remarks/>
        public void addBinaryCaptureDataAsync(string signature, string username, string vrm, short feedIdentifier, short sourceIdentifier, short cameraIdentifier, System.DateTime captureTime, byte[] binaryImage, string binaryDataType, object userState) {
            if ((this.addBinaryCaptureDataOperationCompleted == null)) {
                this.addBinaryCaptureDataOperationCompleted = new System.Threading.SendOrPostCallback(this.OnaddBinaryCaptureDataOperationCompleted);
            }
            this.InvokeAsync("addBinaryCaptureData", new object[] {
                        signature,
                        username,
                        vrm,
                        feedIdentifier,
                        sourceIdentifier,
                        cameraIdentifier,
                        captureTime,
                        binaryImage,
                        binaryDataType}, this.addBinaryCaptureDataOperationCompleted, userState);
        }
        
        private void OnaddBinaryCaptureDataOperationCompleted(object arg) {
            if ((this.addBinaryCaptureDataCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.addBinaryCaptureDataCompleted(this, new addBinaryCaptureDataCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    public delegate void addBinaryCaptureDataCompletedEventHandler(object sender, addBinaryCaptureDataCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3190.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class addBinaryCaptureDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal addBinaryCaptureDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591