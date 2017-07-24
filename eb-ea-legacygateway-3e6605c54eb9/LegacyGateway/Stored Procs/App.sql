IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetBidPackageDetails') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetBidPackageDetails];
GO

CREATE PROCEDURE [dbo].[Gateway_GetBidPackageDetails] @bidPackageID uniqueidentifier, @folderID uniqueidentifier
AS
	SELECT
	  [description] = bp.Description,
	  [user_id] = [owner].UserID,
	  [first_name] = [owner].givenName,
	  [last_name] = [owner].sn,
	  [email] = [owner].Email,
	  [phone] = [owner].OfficePhone,
	  [folder_id] = bp.BidDocsFolderID,
	  [folder_name] = f.FolderName,
	  [folder_parent_id] = o.Parent,
	  [response_folder_id] = bp.ResponseFolderID,
	  [response_folder_name] = rf.FolderName,
	  [response_folder_parent_id] = ro.Parent,
	  [portal_id] = p.PortalID,
	  [portal_name] = p.Name,
	  [file_store_path] = p.FileStorePath,
	  [account_id] = a.AccountID,
	  [account_name] = a.Name,
	  [portal_type_id] = a.PortalType,
	  [portal_type_app_name] = pt.DBName_App,
	  [custom_field_1] = bp.BidItemCustomField1,
	  [custom_field_2] = bp.BidItemCustomField2,
	  [custom_field_3] = bp.BidItemCustomField3
	FROM dbo.daBidPackages bp (NOLOCK)
	INNER JOIN ebCore.dbo.daUsers [owner] (NOLOCK) ON [owner].UserID = bp.OwnerID
	INNER JOIN dbo.daFolders f (NOLOCK) ON f.FolderID = bp.BidDocsFolderID
	LEFT JOIN dbo.daFolders rf (NOLOCK) ON rf.FolderID = bp.ResponseFolderID
	INNER JOIN dbo.daObjects o (NOLOCK) ON o.ObjectID = f.FolderID
	LEFT JOIN dbo.daObjects ro (NOLOCK) ON ro.ObjectID = rf.FolderID
	INNER JOIN ebCore.dbo.daPortals p (NOLOCK) ON p.PortalID = bp.PortalID
	INNER JOIN ebCore.dbo.daAccountPortals ap (NOLOCK) ON ap.PortalID = p.PortalID
	INNER JOIN ebCore.dbo.daAccounts a (NOLOCK) ON a.AccountID = ap.AccountID
	INNER JOIN ebCore.dbo.daPortalTypes pt (NOLOCK) ON pt.PortalTypeID = a.PortalType
	WHERE bp.BidPackageID = @bidPackageID;

	--Documents 
	DECLARE @tempSubFolders Table
		(
			folderID uniqueidentifier,
			folderName VARCHAR(128),
			parentID uniqueidentifier,
			bidDocPermissionRoleID uniqueidentifier
		)
	
	;WITH tree (id, foldername, parent, bidDocPermissionRoleID) AS 
	(
		SELECT 
			fo.folderID,
			fo.folderName,
			o.parent,
			cast(null as uniqueidentifier)
		FROM dbo.daFolders fo (NOLOCK)
		INNER JOIN dbo.daBidPackages bp (NOLOCK) ON bp.BidDocsFolderID = fo.FolderID
		INNER JOIN daObjects o (NOLOCK) ON fo.folderID = o.objectID
		INNER JOIN ebCore.dbo.daAccountPortals ap WITH (NOLOCK) on ap.PortalID = bp.PortalID
		LEFT JOIN ebCore.dbo.daPreferences p WITH (NOLOCK) ON ap.AccountID = p.AccountID AND p.Type = 19 -- BidDocPermissionDateCreated
		LEFT JOIN ebCore.dbo.daPreferences p2 WITH (NOLOCK) ON ap.AccountID = p2.AccountID AND p2.Type = 18 -- BidDocPermissionRole
		WHERE o.isDeleted = 0 AND (p.PreferenceID IS NULL OR p2.PreferenceID IS NULL OR bp.DateCreated < Cast(p.Value as DateTime)) AND bp.BidPackageID = @bidPackageID
		UNION ALL
		SELECT 
			fo.folderID,
			fo.folderName,
			o.parent,
			null
		FROM dbo.daFolders fo (NOLOCK)
		INNER JOIN daObjects o (NOLOCK) ON fo.folderID=o.objectID AND o.objectType=1 
		INNER JOIN tree t ON t.id = o.Parent
		where o.isDeleted = 0 AND o.Parent <> o.ObjectID
	)
	insert into @tempSubFolders
	select id, foldername, parent, bidDocPermissionRoleID
	FROM tree


	;WITH tree (id, foldername, parent, bidDocPermissionRoleID) AS 
	(
		SELECT 
			fo.folderID,
			fo.folderName,
			o.parent,
			cast(p2.Value as uniqueidentifier)
		FROM dbo.daFolders fo (NOLOCK)
		INNER JOIN dbo.daBidPackages bp (NOLOCK) ON bp.BidDocsFolderID = fo.FolderID
		INNER JOIN daObjects o (NOLOCK) ON fo.folderID = o.objectID
		INNER JOIN ebCore.dbo.daAccountPortals ap WITH (NOLOCK) on ap.PortalID = bp.PortalID
		INNER JOIN ebCore.dbo.daPreferences p WITH (NOLOCK) ON ap.AccountID = p.AccountID AND Type = 19 -- BidDocPermissionDateCreated
		INNER JOIN ebCore.dbo.daPreferences p2 WITH (NOLOCK) ON ap.AccountID = p2.AccountID AND p2.Type = 18 -- BidDocPermissionRole
		INNER JOIN dbo.daACL acl (NOLOCK) ON acl.ObjectID = fo.FolderID and acl.TrusteeID = Cast(p2.Value as uniqueidentifier) AND acl.daRead = 1 AND acl.daNoAccess = 0
		WHERE o.isDeleted = 0 AND bp.DateCreated >= Cast(p.Value as DATETIME) AND bp.BidPackageID = @bidPackageID
		UNION ALL
		SELECT 
			fo.folderID,
			fo.folderName,
			o.parent,
			t.bidDocPermissionRoleID
		FROM dbo.daFolders fo (NOLOCK)
		INNER JOIN daObjects o (NOLOCK) ON fo.folderID=o.objectID AND o.objectType=1 
		INNER JOIN tree t ON t.id = o.Parent
		INNER JOIN dbo.daACL acl (NOLOCK) ON acl.ObjectID = fo.FolderID and acl.TrusteeID = t.bidDocPermissionRoleID AND acl.daRead = 1 AND acl.daNoAccess = 0
		where o.isDeleted = 0 AND o.Parent <> o.ObjectID
	)
	insert into @tempSubFolders
	select id, foldername, parent, bidDocPermissionRoleID
	FROM tree


	SELECT 
	  [ID] = FolderID
	, [ObjectID] = NULL
	, [ParentID] = ParentID
	, [Name] = FolderName
	, [Version] = NULL
	, [DocumentType] ='Folder'
	FROM @tempSubFolders
	UNION
	SELECT 
	  [ID] = fl.FileID
	, [ObjectID] = fl.ObjectID
	, [ParentID] = o.Parent
	, [Name] = fl.FileName
	, [Version] = fl.Version
	, [DocumentType] = 'File'
	FROM dbo.daFiles fl (NOLOCK) 
	INNER JOIN dbo.daObjects o (NOLOCK) ON fl.ObjectID = o.ObjectID 
	INNER JOIN @tempSubFolders t ON t.FolderID = o.Parent
	LEFT JOIN dbo.daACL acl (NOLOCK) ON acl.ObjectID = fl.ObjectID and acl.TrusteeID = t.bidDocPermissionRoleID AND acl.daRead = 1 AND acl.daNoAccess = 0
	WHERE fl.IsMostRecent = 1 AND o.ObjectType = 2 AND o.IsDeleted = 0 and (t.bidDocPermissionRoleID is null OR acl.ObjectID is not null)
	UNION
	SELECT --Bid Package Instructions File
	  [ID] = fl.FileID
	, [ObjectID] = fl.ObjectID
	, [ParentID] = o.Parent
	, [Name] = fl.FileName
	, [Version] = fl.Version
	, [DocumentType] = 'InstructionFile'
	FROM dbo.daBidPackages bp (NOLOCK)
	INNER JOIN dbo.daFiles fl (NOLOCK) ON fl.FileID = bp.BidInstructionsFileID
	INNER JOIN dbo.daObjects o (NOLOCK) ON fl.ObjectID = o.ObjectID
	WHERE o.IsDeleted = 0 AND bp.BidPackageID = @bidPackageID

	SELECT -- File Paths for Bid Documents
	  fp.FileID
	, PathTypeID
	, PathName
	, FileSize 
	FROM dbo.daFilePaths fp (NOLOCK)
	INNER JOIN dbo.daFiles f (NOLOCK)  ON f.FileID = fp.FileID
	INNER JOIN dbo.daObjects o (NOLOCK) ON f.ObjectID = o.ObjectID 
	INNER JOIN @tempSubFolders t ON T.FolderID = o.Parent
	LEFT JOIN dbo.daACL acl (NOLOCK) ON acl.ObjectID = f.ObjectID and acl.TrusteeID = t.bidDocPermissionRoleID AND acl.daRead = 1 AND acl.daNoAccess = 0
	WHERE f.IsMostRecent = 1 AND o.ObjectType = 2 AND o.IsDeleted = 0 and (t.bidDocPermissionRoleID is null OR acl.ObjectID is not null)
	UNION
	SELECT -- Bid Package Instructions File
	  fp.FileID
	, PathTypeID
	, PathName
	, FileSize 
	FROM dbo.daBidPackages bp (NOLOCK)
	INNER JOIN dbo.daFiles f (NOLOCK) ON f.FileID = bp.BidInstructionsFileID
	INNER JOIN dbo.daObjects o (NOLOCK) ON o.ObjectID = f.ObjectID
	INNER JOIN dbo.daFilePaths fp (NOLOCK) ON fp.FileID = f.FileID
	WHERE f.IsMostRecent = 1 AND o.IsDeleted = 0 AND bp.BidPackageID = @bidPackageID

	SELECT
	  [CustomFieldID] = cfem.CustomFieldID 
	, [Name] = cf.Name
	, [FieldType] = cf.FieldType
	, [Description] = cf.Description
	, [DisplayOrder] = cf.DisplayOrder
	, [DefaultValue] = cf.DefaultValue
	, [Required] = cf.Required
	, [MinLength] = cf.MinLength
	, [MaxLength] = cf.MaxLength
	, [MustBeUnique] = cf.MustBeUnique
	, [NumVisibleRows] = cf.NumVisibleRows
	, [DecimalPlaces] = cf.DecimalPlaces
	, [Options] = cf.Options
	FROM dbo.daBidPackages bp (NOLOCK)
	INNER JOIN dbo.daCustomFieldEntityMapping cfem (NOLOCK) ON cfem.EntityID = bp.BidPackageID
	INNER JOIN dbo.daCustomFields cf (NOLOCK) ON cf.CustomFieldID = cfem.CustomFieldID
	WHERE bp.BidPackageID = @bidPackageID;


	SELECT 
	  p.AccountID
	, p.Value 
	FROM ebCore.dbo.daPreferences p (NOLOCK)
	INNER JOIN ebCore.dbo.daAccountPortals ap (NOLOCK) ON ap.AccountID = p.AccountID
	INNER JOIN dbo.daBidPackages bp (NOLOCK) on bp.PortalID = ap.PortalID
	WHERE p.type = 67 AND bp.BidPackageID = @bidPackageID;

	SELECT 
	  EnableBidResubmission = ISNULL(aav.i_Value,0)
	FROM ebCore.dbo.daAccountAttributeValues aav (NOLOCK)
	INNER JOIN ebCore.dbo.daAccountAttributes aa (NOLOCK) ON aa.AttributeID = aav.AttributeID
	INNER JOIN ebCore.dbo.daAccountPortals ap (NOLOCK) ON ap.AccountID = aav.AccountID
	INNER JOIN dbo.daBidPackages bp (NOLOCK) ON bp.PortalID = ap.PortalID
	WHERE aa.Name = 'EnableBidResubmission' AND bp.BidpackageID = @bidPackageID;

	SELECT 
	  PastDueMessage = aav.vc_Value
	FROM ebCore.dbo.daAccountAttributeValues aav (NOLOCK)
	INNER JOIN ebCore.dbo.daAccountAttributes aa (NOLOCK) ON aa.AttributeID = aav.AttributeID
	INNER JOIN ebCore.dbo.daAccountPortals ap (NOLOCK) ON ap.AccountID = aav.AccountID
	INNER JOIN dbo.daBidPackages bp (NOLOCK) ON bp.PortalID = ap.PortalID
	WHERE aa.Name = 'BidPastDueDateMessage' AND bp.BidpackageID = @bidPackageID;
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetBidPackageQuestionIDForResponseID') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetBidPackageQuestionIDForResponseID];
GO

CREATE PROCEDURE [dbo].[Gateway_GetBidPackageQuestionIDForResponseID] @messageID uniqueidentifier
AS
SELECT
	MessageID
FROM dbo.daBidderMessages (NOLOCK)
WHERE ResponseMessageID = @messageID;
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetContactDetailsFromInvitation') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetContactDetailsFromInvitation];
GO

CREATE PROCEDURE [dbo].[Gateway_GetContactDetailsFromInvitation] @invitationID uniqueidentifier
AS
SELECT
  [ContactID] = c.ContactID,
  [FirstName] = c.FirstName,
  [LastName] = c.LastName,
  [Email] = c.Email,
  [CompanyName] = company.CompanyName,
  [UserID] = c.UserID,
  [Password] = u.EncryptedPassword
FROM ebCore.dbo.daContacts c (NOLOCK)
INNER JOIN dbo.daInvitations i (NOLOCK) ON i.ContactID = c.ContactID
INNER JOIN ebCore.dbo.daCompanies company (nolock) on company.CompanyID = c.CompanyID
LEFT JOIN ebCore.dbo.daUsers u WITH (NOLOCK) ON u.UserID = c.USerID
WHERE i.InvitationID = @invitationID;
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetCustomField') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetCustomField];
GO

CREATE PROCEDURE [dbo].[Gateway_GetCustomField] @CustomFieldID uniqueidentifier, @CustomFieldValueID uniqueidentifier
AS
	SELECT
	  cf.CustomFieldID
	, cf.ObjectType
	, cf.FieldType
	, cf.Description
	, cf.DefaultValue
	, cf.Options
	, IsCompany = CASE WHEN co.CompanyID is null THEN 0 ELSE 1 END 
	, IsContact = CASE WHEN c.ContactID is null THEN 0 ELSE 1 END
	FROM dbo.daCustomFields cf (NOLOCK)
	INNER JOIN dbo.daCustomFieldValues cfv (NOLOCK) ON cfv.CustomFieldID = cf.CustomFieldID
	LEFT JOIN ebCore.dbo.daContacts c (NOLOCK) on c.ContactID = cfv.EntityID
	LEFT JOIN ebCore.dbo.daCompanies co (NOLOCK) ON co.CompanyID = cfv.EntityID
	WHERE cfv.CustomFieldValueID = @CustomFieldValueID;

	SELECT
	  AppliesTo 
	FROM dbo.daCustomFieldAppliesTo (NOLOCK)
	WHERE CustomFieldID = @CustomFieldID;
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetFileNameFromCustomFieldValue') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetFileNameFromCustomFieldValue];
GO

CREATE PROCEDURE [dbo].[Gateway_GetFileNameFromCustomFieldValue] @customFieldValueID uniqueidentifier
AS
SELECT
	f.FileName
FROM dbo.daCustomFieldValues cfv (NOLOCK)
INNER JOIN dbo.daFiles f (NOLOCK) ON f.FileID = cfv.u_value
WHERE cfv.CustomFieldValueID = @customFieldValueID;
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetFileParentID') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetFileParentID];
GO

CREATE PROCEDURE [dbo].[Gateway_GetFileParentID] @objectID uniqueidentifier
AS
SELECT
	Parent
FROM dbo.daObjects (NOLOCK)
WHERE ObjectID = @objectID;
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetFilePaths') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetFilePaths];
GO

CREATE PROCEDURE [dbo].[Gateway_GetFilePaths] @fileID uniqueidentifier
AS
SELECT
   FileID
 , PathTypeID
 , PathName
 , FileSize
FROM dbo.daFilePaths(NOLOCK)
WHERE FileID = @fileID;
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetFileVersions') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetFileVersions];
GO

CREATE PROCEDURE [dbo].[Gateway_GetFileVersions] @objectID uniqueidentifier
AS
SELECT
    FileID
  , FileName
  , Version
FROM dbo.daFiles (NOLOCK)
WHERE ObjectID = @objectID;
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetBidDocuments') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetBidDocuments];
GO

CREATE PROCEDURE [dbo].[Gateway_GetBidDocuments] @accountID uniqueidentifier
AS
DECLARE @tempSubFolders Table
	(
		folderID uniqueidentifier,
		BidPackageID uniqueidentifier,
		folderName VARCHAR(128),
		parentID uniqueidentifier
	)
	 
;WITH tree (id, bidpackageid, foldername, parent) AS 
(
	SELECT 
		fo.folderID,
		bp.BidPackageID,
		fo.folderName,
		o.parent 
	FROM dbo.daFolders fo (NOLOCK)
	INNER JOIN dbo.daBidPackages bp (NOLOCK) ON bp.BidDocsFolderID = fo.FolderID
	INNER JOIN ebCore.dbo.daAccountPortals ap ON ap.PortalID = bp.PortalID AND AP.AccountID = @accountID
	INNER JOIN dbo.daObjects o (NOLOCK) ON fo.folderID = o.objectID
	LEFT JOIN ebCore.dbo.daPreferences p WITH (NOLOCK) ON ap.AccountID = p.AccountID AND p.Type = 19 -- BidDocPermissionDateCreated
	LEFT JOIN ebCore.dbo.daPreferences p2 WITH (NOLOCK) ON ap.AccountID = p2.AccountID AND p2.Type = 18 -- BidDocPermissionRole
	WHERE o.isDeleted = 0 AND (p.PreferenceID IS NULL OR p2.PreferenceID IS NULL OR bp.DateCreated < Cast(p.Value as DateTime))
	UNION ALL
	SELECT 
		fo.folderID,
		t.BidPackageID,
		fo.folderName,
		o.parent 
	FROM dbo.daFolders fo (NOLOCK)
	INNER JOIN daObjects o (NOLOCK) ON fo.folderID=o.objectID AND o.objectType=1 
	INNER JOIN tree t ON t.id = o.Parent
	where o.isDeleted = 0 AND o.Parent <> o.ObjectID
)
insert into @tempSubFolders
select id, bidpackageid, foldername, parent
FROM tree

SELECT 
  [ID] = folderID
, [BidPackageID] = BidPackageID
, [ObjectID] = null
, [ParentID] = parentID
, [Name] = folderName
, [Version] = null
, [DocumentType] = 'Folder'
FROM @tempSubFolders
UNION
SELECT
  [ID] = f.FileID
, [BidPackageID] = t.BidPackageID
, [ObjectID] = f.ObjectID
, [ParentID] = o.Parent
, [Name] = f.FileName
, [Version] = f.Version
, [DocumentType] = 'File'
FROM dbo.daFiles f (NOLOCK)
INNER JOIN dbo.daObjects o (NOLOCK) ON o.ObjectID = f.ObjectID
INNER JOIN @tempSubFolders t ON t.folderID = o.Parent
WHERE f.IsMostRecent = 1 and o.IsDeleted = 0


--FilePaths
SELECT --File Paths for Files in Bid Docs Sub Folders
  [FileID] = f.FileID
, [BidPackageID] = t.BidPackageID
, [PathName] = fp.PathName
, [FileSize] = fp.FileSize
, [PathTypeID] = fp.PathTypeID 
FROM dbo.daFiles f (NOLOCK)
INNER JOIN dbo.daObjects o (NOLOCK) ON o.ObjectID = f.ObjectID
INNER JOIN @tempSubFolders t ON t.folderID = o.Parent
INNER JOIN dbo.daFilePaths fp (NOLOCK) ON fp.FileID = f.FileID
WHERE f.IsMostRecent = 1 and o.IsDeleted = 0
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetBidDocumentsByRole') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetBidDocumentsByRole];
GO

CREATE PROCEDURE [dbo].[Gateway_GetBidDocumentsByRole] @accountID uniqueidentifier, @bidDocumentRoleID uniqueidentifier
AS
DECLARE @tempSubFolders Table
	(
		folderID uniqueidentifier,
		BidPackageID uniqueidentifier,
		folderName VARCHAR(128),
		parentID uniqueidentifier
	)

;WITH tree (id, bidpackageid, foldername, parent) AS 
(
	SELECT 
		fo.folderID,
		bp.BidPackageID,
		fo.folderName,
		o.parent 
	FROM dbo.daFolders fo (NOLOCK)
	INNER JOIN dbo.daBidPackages bp (NOLOCK) ON bp.BidDocsFolderID = fo.FolderID
	INNER JOIN ebCore.dbo.daAccountPortals ap ON ap.PortalID = bp.PortalID AND AP.AccountID = @accountID
	INNER JOIN dbo.daObjects o (NOLOCK) ON fo.folderID = o.objectID
	INNER JOIN ebCore.dbo.daPreferences p (NOLOCK) ON p.AccountID = ap.AccountID AND p.Type = 19
	INNER JOIN dbo.daACL acl (NOLOCK) ON acl.ObjectID = fo.FolderID and acl.TrusteeID = @bidDocumentRoleID AND acl.daRead = 1 AND acl.daNoAccess = 0
	WHERE o.isDeleted = 0 and bp.DateCreated >= CAST(p.Value as DateTime)
	UNION ALL
	SELECT 
		fo.folderID,
		t.BidPackageID,
		fo.folderName,
		o.parent 
	FROM dbo.daFolders fo (NOLOCK)
	INNER JOIN daObjects o (NOLOCK) ON fo.folderID=o.objectID AND o.objectType=1 
	INNER JOIN tree t ON t.id = o.Parent
	INNER JOIN dbo.daACL acl (NOLOCK) ON acl.ObjectID = fo.FolderID and acl.TrusteeID = @bidDocumentRoleID AND acl.daRead = 1 AND acl.daNoAccess = 0
	where o.isDeleted = 0 AND o.Parent <> o.ObjectID
)
insert into @tempSubFolders
select id, bidpackageid, foldername, parent
FROM tree

SELECT 
  [ID] = folderID
, [BidPackageID] = BidPackageID
, [ObjectID] = null
, [ParentID] = parentID
, [Name] = folderName
, [Version] = null
, [DocumentType] = 'Folder'
FROM @tempSubFolders
UNION
SELECT
  [ID] = f.FileID
, [BidPackageID] = t.BidPackageID
, [ObjectID] = f.ObjectID
, [ParentID] = o.Parent
, [Name] = f.FileName
, [Version] = f.Version
, [DocumentType] = 'File'
FROM dbo.daFiles f (NOLOCK)
INNER JOIN dbo.daObjects o (NOLOCK) ON o.ObjectID = f.ObjectID
INNER JOIN @tempSubFolders t ON t.folderID = o.Parent
INNER JOIN dbo.daACL acl (NOLOCK) ON acl.ObjectID = f.ObjectID and acl.TrusteeID = @bidDocumentRoleID AND acl.daRead = 1 AND acl.daNoAccess = 0
WHERE f.IsMostRecent = 1 AND o.IsDeleted = 0


--FilePaths
SELECT --File Paths for Files in Bid Docs Sub Folders
  [FileID] = f.FileID
, [BidPackageID] = t.BidPackageID
, [PathName] = fp.PathName
, [FileSize] = fp.FileSize
, [PathTypeID] = fp.PathTypeID 
FROM dbo.daFiles f (NOLOCK)
INNER JOIN dbo.daObjects o (NOLOCK) ON o.ObjectID = f.ObjectID
INNER JOIN @tempSubFolders t ON t.folderID = o.Parent
INNER JOIN dbo.daFilePaths fp (NOLOCK) ON fp.FileID = f.FileID
INNER JOIN dbo.daACL acl (NOLOCK) ON acl.ObjectID = f.ObjectID and acl.TrusteeID = @bidDocumentRoleID AND acl.daRead = 1 AND acl.daNoAccess = 0
WHERE f.IsMostRecent = 1 and o.IsDeleted = 0
GO


IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Gateway_GetFileData') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[Gateway_GetFileData];
GO

CREATE PROCEDURE [dbo].[Gateway_GetFileData] @fileID UNIQUEIDENTIFIER
AS
  SELECT
    [ParentID] = o.Parent
  , [InvitationID] = i.InvitationID
  , [BidID] = db.BidID
  , [DateCreated] = ISNULL(ah.[TimeStamp], f.DateModified)
  FROM dbo.daObjects o (NOLOCK)
  INNER JOIN dbo.daFiles f (NOLOCK) ON f.ObjectID = o.ObjectID 
  INNER JOIN (
    SELECT ah.SubObjectID, TimeStamp = MAX(ah.TimeStamp)
    FROM dbo.daAccessHistory ah (NOLOCK) 
    WHERE ah.SubObjectID = @fileID AND AccessType IN (1,9)
    GROUP By ah.SubObjectID
  ) ah ON ah.SubObjectID = f.FileID
  LEFT JOIN dbo.daInvitations i (NOLOCK) ON i.InvitationID = o.Parent
  LEFT JOIN dbo.daDraftBids db (NOLOCK) ON db.BidID = o.Parent
  WHERE f.FileID = @fileID;
GO


ALTER TABLE dbo.daCustomFieldDependencies
ALTER COLUMN [Value] NVARCHAR(MAX) NULL
GO

ALTER TABLE dbo.daBidderMessages
ALTER COLUMN Body NVARCHAR(MAX) NOT NULL
GO

IF FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') = 1
BEGIN 
	ALTER FULLTEXT INDEX ON dbo.daBidPackages DISABLE
	ALTER FULLTEXT INDEX ON dbo.daBidPackages DROP (Description)
	ALTER TABLE dbo.daBidPackages
	ALTER COLUMN [Description] NVARCHAR(MAX) NULL
	ALTER FULLTEXT INDEX ON dbo.daBidPackages ADD (Description)
	ALTER FULLTEXT INDEX ON dbo.daBidPackages ENABLE
END
ELSE
BEGIN
	ALTER TABLE dbo.daBidPackages
	ALTER COLUMN [Description] NVARCHAR(MAX) NULL
END
GO

ALTER TABLE dbo.daBids
ALTER COLUMN BidQualifications NVARCHAR(MAX) NULL
GO

ALTER TABLE dbo.daCustomFields
ALTER COLUMN Options NVARCHAR(MAX) NULL
GO

ALTER TABLE dbo.daCustomFields
ALTER COLUMN DefaultValue NVARCHAR(MAX) NULL
GO

IF FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') = 1
BEGIN 
	ALTER FULLTEXT INDEX ON dbo.daCustomFieldValues DISABLE
	ALTER FULLTEXT INDEX ON dbo.daCustomFieldValues DROP (text_value)
	ALTER TABLE dbo.daCustomFieldValues
	ALTER COLUMN text_value NVARCHAR(MAX) NULL
	ALTER FULLTEXT INDEX ON dbo.daCustomFieldValues ADD (text_value)
	ALTER FULLTEXT INDEX ON dbo.daCustomFieldValues ENABLE
END
ELSE
BEGIN
	ALTER TABLE dbo.daCustomFieldValues
	ALTER COLUMN text_value NVARCHAR(MAX) NULL
END
GO

ALTER TABLE dbo.daDraftBids
ALTER COLUMN BidQualifications NVARCHAR(MAX) NULL
GO

ALTER TABLE dbo.daBidderAccessRequests
ALTER COLUMN [Message] NVARCHAR(MAX) NULL
GO