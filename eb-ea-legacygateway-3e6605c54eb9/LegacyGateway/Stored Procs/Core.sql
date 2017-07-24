IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetConstructionCodeDetails') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetConstructionCodeDetails];
GO

CREATE PROCEDURE [dbo].[Gateway_GetConstructionCodeDetails] @constructionCodeID uniqueidentifier
AS
	SELECT
	  [ConstructionCodeID] = cc.ConstructionCodeID,
	  [Code] = cc.ConstructionCode,
	  [Description] = cc.Description,
	  [ParentConstructionCodeID] = pcc.ConstructionCodeID,
	  [ParentCode] = pcc.ConstructionCode,
	  [ParentDescription] = pcc.Description
	FROM dbo.daConstructionCodes cc WITH (NOLOCK)
	INNER JOIN dbo.daConstructionCodes pcc WITH (NOLOCK) ON pcc.ConstructionCodeID = cc.ParentID
	WHERE cc.ConstructionCodeID = @constructionCodeID;
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetContactDetails') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetContactDetails];
GO

CREATE PROCEDURE [dbo].[Gateway_GetContactDetails] @contactID uniqueidentifier
AS
	SELECT
	  c.ContactID,
	  u.EncryptedPassword,
	  c.Email,
	  c.USerID,
	  c.FirstName,
	  c.LastName,
	  c.CompanyID,
	  co.CompanyName
	FROM dbo.daContacts c WITH (NOLOCK)
	INNER JOIN dbo.daCompanies co WITH (NOLOCK) on c.CompanyID = co.CompanyID
	LEFT JOIN dbo.daUsers u WITH (NOLOCK) ON u.UserID = c.USerID
	WHERE c.ContactID = @contactID;
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetUserDetails') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetUserDetails];
GO

CREATE PROCEDURE [dbo].[Gateway_GetUserDetails] @userID uniqueidentifier
AS
	SELECT
	  [UserID] = u.UserID,
	  [FirstName] = u.givenName,
	  [LastName] = u.sn,
	  [Email] = u.Email,
	  [Company] = u.companyName
	FROM dbo.daUsers u WITH (NOLOCK)
	WHERE u.UserID = @userID;
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetCompanyDetailsForContact') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetCompanyDetailsForContact];
GO

CREATE PROCEDURE [dbo].[Gateway_GetCompanyDetailsForContact] @contactID uniqueidentifier
AS
	SELECT
	  company.CompanyID,
	  company.CompanyName
	FROM dbo.daContacts c (NOLOCK)
	INNER JOIN dbo.daCompanies company (NOLOCK) ON company.CompanyID = c.CompanyID
	WHERE c.ContactID = @contactID;
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetAccountAppDatabase') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetAccountAppDatabase];
GO

CREATE PROCEDURE [dbo].[Gateway_GetAccountAppDatabase] @accountID uniqueidentifier
AS
	SELECT 
		DBName_App 
	FROM dbo.daAccounts a  (NOLOCK)
	INNER JOIN dbo.daPortalTypes pt (NOLOCK) ON pt.PortalTypeID = a.PortalType
	WHERE a.AccountID = @accountID
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetAccountDetails') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetAccountDetails];
GO

CREATE PROCEDURE [dbo].[Gateway_GetAccountDetails] @accountID uniqueidentifier
AS
	;WITH AttributeData AS (
		SELECT
			AccountID,
			AdvancedReportsToggle,
			bActivateRequires128BitSSL,
			bAllowActivityReports,
			bEnableWorkflowOffline,
			BidPastDueDateMessage,
			ChangeExportImportDescription,
			cmAllowChangeOnBudgetControl,
			cmAllowChangeOnCommitmentControl,
			cmDefaultBudgetControl,
			cmDefaultCommitmentControl,
			cmEnableBidDetailSets,
			cmEnableCashFlowLeftOverMoneySpread,
			cmEnableDynamicColumns,
			cmEnableDynamicLineItems,
			cmUseConfigurableDocumentManagement,
			CostEnableFundingForecast,
			CostImportEnabled,
			CostImportEnableUpdateInvToVoid,
			CostIntegrationPageURL,
			CostIntegrationSetupURL,
			CSVStripQuotesFromBIReportHeader,
			CustomSOVTemplateForProcessPrefix,
			CustomSOVTemplateOnlyOnStartStep,
			CustomSOVTemplatePageLink,
			DisableEditDocumentsInPlace,
			DisablePublicProcessCaptcha,
			DocuSignAccountPassword,
			DocuSignAccountUserName,
			DocuSignBrandId,
			DocuSignTraceOn,
			ebDefaultInterface,
			ebDisableMobile,
			ebEnterpriseTabVector,
			ebFailedLoginLockoutDuration,
			ebInactiveLogoutTime,
			ebMaxFailedLoginAttempts,
			ebPasswordComplexity,
			ebPasswordExpireDays,
			ebPasswordHistoryLength,
			ebPasswordLength,
			EnableAccountLevelCost,
			EnableAccountLevelFunding,
			EnableAdvancedFundingDistribution,
			EnableAdvancedSSO,
			EnableBidResubmission,
			EnableBravaUpgrade,
			EnableCustomSOV,
			EnableEquipmentManagement,
			EnableEVM,
			EnableEVMImport,
			EnableFundingCategoriesForProcessesLookups,
			EnableFundingForBudgetChange,
			EnableIndividualLicenses,
			EnableInvoiceItemCustomFields,
			EnableMobileSSO,
			EnableModelViewer,
			EnableNegativeFundingAmounts,
			EnableNewAddendumBidDeletion,
			EnablePlanRoom,
			EnableProductManagementReports,
			EnablePublicProcesses,
			EnableScheduleImportUpdates,
			EnableSOVAmountOverrideImport,
			EnableSOVImport,
			EnableSOVImportWithFullEdit,
			EnableSubmittals,
			EnableUseOfAFP,
			ExternalAccountIdentifier,
			FaxCoverSheet,
			FaxServiceEmail,
			FormImportEnabled,
			GPODisplayName,
			GPOPluginEnabled,
			GPOSourceAccount,
			HideInProductMarketing,
			IsActiveAttribute = IsActive,
			IsReprographer,
			MobileInactivityTimeout,
			MobileSSOAuthenticationURL,
			MobileSSOHeader,
			NewTabVector,
			NumberOfUserLicenses,
			oDataAPIUsageMetricsToggle,
			oDataIntervalForRequests,
			oDataRequestsPerInterval,
			oDataToggle,
			PartnerIdpId,
			ProcessImportEnabled,
			ProjectImportEnabled,
			SalesForceID,
			ScheduleRYG,
			ShowCustomCashflowCurves,
			SSOLogoutURL,
			WalkMeEnabled,
			WorkflowAutomatedStepEnabled,
			YearlyUserLicenseCost
		FROM (
			SELECT 
				aav.AccountID, 
				AttributeName = aa.Name, 
				[Value] = Case aa.Datatype 
					WHEN 0 THEN aav.vc_value 
					WHEN 1 THEN Cast(aav.i_value AS VARCHAR(MAX)) 
					WHEN 3 THEN cast(aav.uid_value AS VARCHAR(MAX))
				END
			FROM dbo.daAccountAttributeValues aav (NOLOCK)
			INNER JOIN dbo.daAccountAttributes aa (NOLOCK) ON aa.AttributeID = aav.AttributeID
			WHERE aav.AccountID = @AccountID
		) f
		PIVOT(
			MAX(Value) FOR AttributeName IN (
				[AdvancedReportsToggle],
				[bActivateRequires128BitSSL],
				[bAllowActivityReports],
				[bEnableWorkflowOffline],
				[BidPastDueDateMessage],
				[ChangeExportImportDescription],
				[cmAllowChangeOnBudgetControl],
				[cmAllowChangeOnCommitmentControl],
				[cmDefaultBudgetControl],
				[cmDefaultCommitmentControl],
				[cmEnableBidDetailSets],
				[cmEnableCashFlowLeftOverMoneySpread],
				[cmEnableDynamicColumns],
				[cmEnableDynamicLineItems],
				[cmUseConfigurableDocumentManagement],
				[CostEnableFundingForecast],
				[CostImportEnabled],
				[CostImportEnableUpdateInvToVoid],
				[CostIntegrationPageURL],
				[CostIntegrationSetupURL],
				[CSVStripQuotesFromBIReportHeader],
				[CustomSOVTemplateForProcessPrefix],
				[CustomSOVTemplateOnlyOnStartStep],
				[CustomSOVTemplatePageLink],
				[DisableEditDocumentsInPlace],
				[DisablePublicProcessCaptcha],
				[DocuSignAccountPassword],
				[DocuSignAccountUserName],
				[DocuSignBrandId],
				[DocuSignTraceOn],
				[ebDefaultInterface],
				[ebDisableMobile],
				[ebEnterpriseTabVector],
				[ebFailedLoginLockoutDuration],
				[ebInactiveLogoutTime],
				[ebMaxFailedLoginAttempts],
				[ebPasswordComplexity],
				[ebPasswordExpireDays],
				[ebPasswordHistoryLength],
				[ebPasswordLength],
				[EnableAccountLevelCost],
				[EnableAccountLevelFunding],
				[EnableAdvancedFundingDistribution],
				[EnableAdvancedSSO],
				[EnableBidResubmission],
				[EnableBravaUpgrade],
				[EnableCustomSOV],
				[EnableEquipmentManagement],
				[EnableEVM],
				[EnableEVMImport],
				[EnableFundingCategoriesForProcessesLookups],
				[EnableFundingForBudgetChange],
				[EnableIndividualLicenses],
				[EnableInvoiceItemCustomFields],
				[EnableMobileSSO],
				[EnableModelViewer],
				[EnableNegativeFundingAmounts],
				[EnableNewAddendumBidDeletion],
				[EnablePlanRoom],
				[EnableProductManagementReports],
				[EnablePublicProcesses],
				[EnableScheduleImportUpdates],
				[EnableSOVAmountOverrideImport],
				[EnableSOVImport],
				[EnableSOVImportWithFullEdit],
				[EnableSubmittals],
				[EnableUseOfAFP],
				[ExternalAccountIdentifier],
				[FaxCoverSheet],
				[FaxServiceEmail],
				[FormImportEnabled],
				[GPODisplayName],
				[GPOPluginEnabled],
				[GPOSourceAccount],
				[HideInProductMarketing],
				[IsActive],
				[IsReprographer],
				[MobileInactivityTimeout],
				[MobileSSOAuthenticationURL],
				[MobileSSOHeader],
				[NewTabVector],
				[NumberOfUserLicenses],
				[oDataAPIUsageMetricsToggle],
				[oDataIntervalForRequests],
				[oDataRequestsPerInterval],
				[oDataToggle],
				[PartnerIdpId],
				[ProcessImportEnabled],
				[ProjectImportEnabled],
				[SalesForceID],
				[ScheduleRYG],
				[ShowCustomCashflowCurves],
				[SSOLogoutURL],
				[WalkMeEnabled],
				[WorkflowAutomatedStepEnabled],
				[YearlyUserLicenseCost]
			)
		) AS pvt
	)
	SELECT 
		a.AccountID, 
		a.Name,
		a.Description,
		a.IsActive,
		a.UrlSafeName,
		a.AccountLogoHeight,
		a.PermissionLastUpdated,
		ad.AdvancedReportsToggle,
		ad.bActivateRequires128BitSSL,
		ad.bAllowActivityReports,
		ad.bEnableWorkflowOffline,
		ad.BidPastDueDateMessage,
		ad.ChangeExportImportDescription,
		ad.cmAllowChangeOnBudgetControl,
		ad.cmAllowChangeOnCommitmentControl,
		ad.cmDefaultBudgetControl,
		ad.cmDefaultCommitmentControl,
		ad.cmEnableBidDetailSets,
		ad.cmEnableCashFlowLeftOverMoneySpread,
		ad.cmEnableDynamicColumns,
		ad.cmEnableDynamicLineItems,
		ad.cmUseConfigurableDocumentManagement,
		ad.CostEnableFundingForecast,
		ad.CostImportEnabled,
		ad.CostImportEnableUpdateInvToVoid,
		ad.CostIntegrationPageURL,
		ad.CostIntegrationSetupURL,
		ad.CSVStripQuotesFromBIReportHeader,
		ad.CustomSOVTemplateForProcessPrefix,
		ad.CustomSOVTemplateOnlyOnStartStep,
		ad.CustomSOVTemplatePageLink,
		ad.DisableEditDocumentsInPlace,
		ad.DisablePublicProcessCaptcha,
		ad.DocuSignAccountPassword,
		ad.DocuSignAccountUserName,
		ad.DocuSignBrandId,
		ad.DocuSignTraceOn,
		ad.ebDefaultInterface,
		ad.ebDisableMobile,
		ad.ebEnterpriseTabVector,
		ad.ebFailedLoginLockoutDuration,
		ad.ebInactiveLogoutTime,
		ad.ebMaxFailedLoginAttempts,
		ad.ebPasswordComplexity,
		ad.ebPasswordExpireDays,
		ad.ebPasswordHistoryLength,
		ad.ebPasswordLength,
		ad.EnableAccountLevelCost,
		ad.EnableAccountLevelFunding,
		ad.EnableAdvancedFundingDistribution,
		ad.EnableAdvancedSSO,
		ad.EnableBidResubmission,
		ad.EnableBravaUpgrade,
		ad.EnableCustomSOV,
		ad.EnableEquipmentManagement,
		ad.EnableEVM,
		ad.EnableEVMImport,
		ad.EnableFundingCategoriesForProcessesLookups,
		ad.EnableFundingForBudgetChange,
		ad.EnableIndividualLicenses,
		ad.EnableInvoiceItemCustomFields,
		ad.EnableMobileSSO,
		ad.EnableModelViewer,
		ad.EnableNegativeFundingAmounts,
		ad.EnableNewAddendumBidDeletion,
		ad.EnablePlanRoom,
		ad.EnableProductManagementReports,
		ad.EnablePublicProcesses,
		ad.EnableScheduleImportUpdates,
		ad.EnableSOVAmountOverrideImport,
		ad.EnableSOVImport,
		ad.EnableSOVImportWithFullEdit,
		ad.EnableSubmittals,
		ad.EnableUseOfAFP,
		ad.ExternalAccountIdentifier,
		ad.FaxCoverSheet,
		ad.FaxServiceEmail,
		ad.FormImportEnabled,
		ad.GPODisplayName,
		ad.GPOPluginEnabled,
		ad.GPOSourceAccount,
		ad.HideInProductMarketing,
		ad.IsActiveAttribute,
		ad.IsReprographer,
		ad.MobileInactivityTimeout,
		ad.MobileSSOAuthenticationURL,
		ad.MobileSSOHeader,
		ad.NewTabVector,
		ad.NumberOfUserLicenses,
		ad.oDataAPIUsageMetricsToggle,
		ad.oDataIntervalForRequests,
		ad.oDataRequestsPerInterval,
		ad.oDataToggle,
		ad.PartnerIdpId,
		ad.ProcessImportEnabled,
		ad.ProjectImportEnabled,
		ad.SalesForceID,
		ad.ScheduleRYG,
		ad.ShowCustomCashflowCurves,
		ad.SSOLogoutURL,
		ad.WalkMeEnabled,
		ad.WorkflowAutomatedStepEnabled,
		ad.YearlyUserLicenseCost, 
		[id] = pt.PortalTypeID, 
		pt.DBName_App
	FROM dbo.daAccounts (NOLOCK) a
	INNER JOIN dbo.daPortalTypes pt (NOLOCK) ON pt.PortalTypeID = a.PortalType
	LEFT JOIN AttributeData ad ON ad.AccountID = a.AccountID
	WHERE a.AccountID = @accountID;
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetAccountAttributeDetails') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetAccountAttributeDetails];
GO

CREATE PROCEDURE [dbo].[Gateway_GetAccountAttributeDetails] @attributeID UNIQUEIDENTIFIER
AS
	SELECT 
		[AttributeID],
		[Name],
		[Description],
		[DataType],
		[MultiValued]
	FROM [dbo].[daAccountAttributes] (NOLOCK)
	WHERE AttributeID = @attributeID;
GO


ALTER TABLE dbo.daPreferences
ALTER COLUMN Value NVARCHAR(MAX) NOT NULL
GO

ALTER TABLE dbo.daCompanies
ALTER COLUMN ConstructionCodeDisplay NVARCHAR(MAX) NULL
GO

ALTER TABLE dbo.daCompanies
ALTER COLUMN CSICodes NVARCHAR(MAX) NULL
GO