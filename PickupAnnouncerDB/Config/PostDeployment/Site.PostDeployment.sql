IF (SELECT COUNT(*) FROM Config.Site) < 1 
BEGIN
	INSERT INTO Config.Site(NumberOfCones, AdminUser, AdminPass) VALUES(8, 'admin', 'password');
END;
GO