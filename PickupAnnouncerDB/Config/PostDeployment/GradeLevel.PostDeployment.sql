IF (SELECT COUNT(*) FROM Config.GradeLevel) < 1 
BEGIN
	INSERT INTO 
		Config.GradeLevel(Name, BackgroundColor, TextColor) 
		VALUES
			('PK', '#4C6472', '#FFFFFF'),
			('KG', '#57A4B1', '#FFFFFF'),
			('1', '#B0D894', '#FFFFFF'),
			('2', '#FADE89', '#FFFFFF'),
			('3', '#F95355', '#FFFFFF'),
			('4', '#23254C', '#FFFFFF')
END;
GO