create table [dbo].[ITN_Users]
(
 [Id] int not null primary key identity,
 [Jmeno] nvarchar(50),
 [UrlObrazek] nvarchar(50),
 [Urlwww] nvarchar(50),
 [IdNaITN] INT,
 [Vek] INT,
 [Zkusenost] INT,
 [Aura] INT
)
