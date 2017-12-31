CREATE VIEW [dbo].[vw_CompleteUser]
  AS 
  SELECT 
	COALESCE(au.UserID, oid.UserID) as UserID, 
	COALESCE(au.Username, oid.OpenIdClaimedIdentifier COLLATE DATABASE_DEFAULT) as Username, 
	COALESCE(up.DisplayName, au.Username, 'New User') as DisplayName, 
	up.[Description], 
	up.GuildWebsite, 
	up.BirthDate, 
	up.[Location],
	COALESCE(up.Email, am.Email) as Email,
	CASE WHEN oid.UserID is not null THEN 1 ELSE 0 END as IsOpenIDUser,
	CASE WHEN au.UserID is not null THEN 1 ELSE 0 END as IsAspNetMembershipUser,
	COALESCE(am.LastLoginDate, oid.LastLoginDate) as LastLoginDate,
	COALESCE(am.CreateDate, oid.CreateDate) as CreateDate,
	COALESCE(up.Email, am.Email, oid.OpenIdClaimedIdentifier COLLATE DATABASE_DEFAULT) as GravatarID,	
	up.UserIntID
  from dbo.aspnet_Users au
  left join aspnet_Membership am on am.UserID = au.UserID
  FULL OUTER JOIN dbo.OpenID oid on au.UserID = oid.UserID
  left join dbo.UserProfile up on up.UserID = au.UserID or up.UserID = oid.UserID