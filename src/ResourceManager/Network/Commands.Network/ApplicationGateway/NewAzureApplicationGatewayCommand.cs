﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using AutoMapper;
using Microsoft.Azure.Commands.Network.Models;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Commands.ResourceManager.Common.Tags;
using Microsoft.Azure.Management.Network;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using MNM = Microsoft.Azure.Management.Network.Models;

namespace Microsoft.Azure.Commands.Network
{
    [Cmdlet("New", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "ApplicationGateway", SupportsShouldProcess = true), OutputType(typeof(PSApplicationGateway))]
    public class NewAzureApplicationGatewayCommand : ApplicationGatewayBaseCmdlet
    {
        [Alias("ResourceName")]
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The resource name.")]
        [ValidateNotNullOrEmpty]
        public virtual string Name { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The resource group name.")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        public virtual string ResourceGroupName { get; set; }

        [Parameter(
         Mandatory = true,
         ValueFromPipelineByPropertyName = true,
         HelpMessage = "location.")]
        [LocationCompleter("Microsoft.Network/applicationGateways")]
        [ValidateNotNullOrEmpty]
        public virtual string Location { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The SKU of application gateway")]
        [ValidateNotNullOrEmpty]
        public virtual PSApplicationGatewaySku Sku { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The SSL policy of application gateway")]
        public virtual PSApplicationGatewaySslPolicy SslPolicy { get; set; }

        [Parameter(
             Mandatory = true,
             ValueFromPipelineByPropertyName = true,
             HelpMessage = "The list of IPConfiguration (subnet)")]
        [ValidateNotNullOrEmpty]
        public PSApplicationGatewayIPConfiguration[] GatewayIPConfigurations { get; set; }

        [Parameter(
             Mandatory = false,
             ValueFromPipelineByPropertyName = true,
             HelpMessage = "The list of ssl certificates")]
        public PSApplicationGatewaySslCertificate[] SslCertificates { get; set; }

        [Parameter(
             Mandatory = false,
             ValueFromPipelineByPropertyName = true,
             HelpMessage = "The list of authentication certificates")]
        public PSApplicationGatewayAuthenticationCertificate[] AuthenticationCertificates { get; set; }

        [Parameter(
             Mandatory = false,
             ValueFromPipelineByPropertyName = true,
             HelpMessage = "The list of trusted root certificates")]
        public PSApplicationGatewayTrustedRootCertificate[] TrustedRootCertificate { get; set; }

        [Parameter(
             Mandatory = false,
             ValueFromPipelineByPropertyName = true,
             HelpMessage = "The list of frontend IP config")]
        public PSApplicationGatewayFrontendIPConfiguration[] FrontendIPConfigurations { get; set; }

        [Parameter(
             Mandatory = true,
             ValueFromPipelineByPropertyName = true,
             HelpMessage = "The list of frontend port")]
        public PSApplicationGatewayFrontendPort[] FrontendPorts { get; set; }

        [Parameter(
             Mandatory = false,
             ValueFromPipelineByPropertyName = true,
             HelpMessage = "The list of probe")]
        public PSApplicationGatewayProbe[] Probes { get; set; }

        [Parameter(
             Mandatory = true,
             ValueFromPipelineByPropertyName = true,
             HelpMessage = "The list of backend address pool")]
        public PSApplicationGatewayBackendAddressPool[] BackendAddressPools { get; set; }

        [Parameter(
             Mandatory = true,
             ValueFromPipelineByPropertyName = true,
             HelpMessage = "The list of backend http settings")]
        public PSApplicationGatewayBackendHttpSettings[] BackendHttpSettingsCollection { get; set; }

        [Parameter(
             Mandatory = true,
             ValueFromPipelineByPropertyName = true,
             HelpMessage = "The list of http listener")]
        public PSApplicationGatewayHttpListener[] HttpListeners { get; set; }

        [Parameter(
             Mandatory = false,
             ValueFromPipelineByPropertyName = true,
             HelpMessage = "The list of UrlPathMap")]
        public PSApplicationGatewayUrlPathMap[] UrlPathMaps { get; set; }

        [Parameter(
             Mandatory = true,
             ValueFromPipelineByPropertyName = true,
             HelpMessage = "The list of request routing rule")]
        public PSApplicationGatewayRequestRoutingRule[] RequestRoutingRules { get; set; }

        [Parameter(
             Mandatory = false,
             ValueFromPipelineByPropertyName = true,
             HelpMessage = "The list of RewriteRuleSet")]
        public PSApplicationGatewayRewriteRuleSet[] RewriteRuleSet { get; set; }

        [Parameter(
             Mandatory = false,
             ValueFromPipelineByPropertyName = true,
             HelpMessage = "The list of redirect configuration")]
        public PSApplicationGatewayRedirectConfiguration[] RedirectConfigurations { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Firewall configuration")]
        public virtual PSApplicationGatewayWebApplicationFirewallConfiguration WebApplicationFirewallConfiguration { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Autoscale Configuration")]
        public virtual PSApplicationGatewayAutoscaleConfiguration AutoscaleConfiguration { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = " Whether HTTP2 is enabled.")]
        public SwitchParameter EnableHttp2 { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = " Whether FIPS is enabled.")]
        public SwitchParameter EnableFIPS { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "A list of availability zones denoting where the application gateway needs to come from.")]
        public string[] Zone { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "A hashtable which represents resource tags.")]
        public Hashtable Tag { get; set; }

        [Parameter(
            Mandatory = false,
            HelpMessage = "Do not ask for confirmation if you want to overrite a resource")]
        public SwitchParameter Force { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Run cmdlet in the background")]
        public SwitchParameter AsJob { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Customer error of an application gateway")]
        [ValidateNotNullOrEmpty]
        public PSApplicationGatewayCustomError[] CustomErrorConfiguration { get; set; }

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            var present = this.IsApplicationGatewayPresent(this.ResourceGroupName, this.Name);
            ConfirmAction(
                Force.IsPresent,
                string.Format(Microsoft.Azure.Commands.Network.Properties.Resources.OverwritingResource, Name),
                Microsoft.Azure.Commands.Network.Properties.Resources.OverwritingResourceMessage,
                Name,
                () =>
                {
                    var applicationGateway = CreateApplicationGateway();
                    WriteObject(applicationGateway);
                },
                () => present);
        }

        private PSApplicationGateway CreateApplicationGateway()
        {
            var applicationGateway = new PSApplicationGateway();
            applicationGateway.Name = this.Name;
            applicationGateway.ResourceGroupName = this.ResourceGroupName;
            applicationGateway.Location = this.Location;
            applicationGateway.Sku = this.Sku;

            if (this.SslPolicy != null)
            {
                applicationGateway.SslPolicy = new PSApplicationGatewaySslPolicy();
                applicationGateway.SslPolicy = this.SslPolicy;
            }

            if (this.GatewayIPConfigurations != null)
            {
                applicationGateway.GatewayIPConfigurations = this.GatewayIPConfigurations?.ToList();
            }

            if (this.SslCertificates != null)
            {
                applicationGateway.SslCertificates = this.SslCertificates?.ToList();
            }

            if (this.AuthenticationCertificates != null)
            {
                applicationGateway.AuthenticationCertificates = this.AuthenticationCertificates?.ToList();
            }

            if (this.TrustedRootCertificate != null)
            {
                applicationGateway.TrustedRootCertificates =this.TrustedRootCertificate?.ToList();
            }

            if (this.FrontendIPConfigurations != null)
            {
                applicationGateway.FrontendIPConfigurations = this.FrontendIPConfigurations?.ToList();
            }

            if (this.FrontendPorts != null)
            {
                applicationGateway.FrontendPorts = this.FrontendPorts?.ToList();
            }

            if (this.Probes != null)
            {
                applicationGateway.Probes = this.Probes?.ToList();
            }

            if (this.BackendAddressPools != null)
            {
                applicationGateway.BackendAddressPools = this.BackendAddressPools?.ToList();
            }

            if (this.BackendHttpSettingsCollection != null)
            {
                applicationGateway.BackendHttpSettingsCollection = this.BackendHttpSettingsCollection?.ToList();
            }

            if (this.HttpListeners != null)
            {
                applicationGateway.HttpListeners = this.HttpListeners?.ToList();
            }

            if (this.UrlPathMaps != null)
            {
                applicationGateway.UrlPathMaps = this.UrlPathMaps?.ToList();
            }

            if (this.RequestRoutingRules != null)
            {
                applicationGateway.RequestRoutingRules = this.RequestRoutingRules?.ToList();
            }

            if (this.RewriteRuleSet != null)
            {
                applicationGateway.RewriteRuleSets = this.RewriteRuleSet?.ToList();
            }

            if (this.RedirectConfigurations != null)
            {
                applicationGateway.RedirectConfigurations = this.RedirectConfigurations?.ToList();
            }

            if (this.WebApplicationFirewallConfiguration != null)
            {
                applicationGateway.WebApplicationFirewallConfiguration = this.WebApplicationFirewallConfiguration;
            }

            if (this.AutoscaleConfiguration != null)
            {
                applicationGateway.AutoscaleConfiguration = this.AutoscaleConfiguration;
            }

            if (this.EnableHttp2.IsPresent)
            {
                applicationGateway.EnableHttp2 = true;
            }

            if (this.EnableFIPS.IsPresent)
            {
                applicationGateway.EnableFips = true;
            }

            if (this.Zone != null)
            {
                applicationGateway.Zones = this.Zone?.ToList();
            }

            if (this.CustomErrorConfiguration != null)
            {
                applicationGateway.CustomErrorConfigurations = this.CustomErrorConfiguration?.ToList();
            }

            // Normalize the IDs
            ApplicationGatewayChildResourceHelper.NormalizeChildResourcesId(applicationGateway);

            // Map to the sdk object
            var appGwModel = NetworkResourceManagerProfile.Mapper.Map<MNM.ApplicationGateway>(applicationGateway);
            appGwModel.Tags = TagsConversionHelper.CreateTagDictionary(this.Tag, validate: true);

            // Execute the Create ApplicationGateway call
            this.ApplicationGatewayClient.CreateOrUpdate(this.ResourceGroupName, this.Name, appGwModel);

            var getApplicationGateway = this.GetApplicationGateway(this.ResourceGroupName, this.Name);

            return getApplicationGateway;
        }
    }
}
