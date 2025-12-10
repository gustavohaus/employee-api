IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'EmployeeDB')
BEGIN
    CREATE DATABASE EmployeeDB;
END
GO

USE EmployeeDB;
GO

IF OBJECT_ID('dbo.Phones', 'U') IS NOT NULL
    DROP TABLE dbo.Phones;
GO

IF OBJECT_ID('dbo.Employees', 'U') IS NOT NULL
    DROP TABLE dbo.Employees;
GO

CREATE TABLE Employees
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    DocumentNumber NVARCHAR(50) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Role INT NOT NULL,
    Status INT NOT NULL,
	BirthDate DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    ManagerId UNIQUEIDENTIFIER NULL
);
GO

ALTER TABLE Employees
ADD CONSTRAINT FK_Employees_Manager
FOREIGN KEY (ManagerId)
REFERENCES Employees(Id)
ON DELETE NO ACTION;
GO

CREATE TABLE Phones
(
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,

    EmployeeId UNIQUEIDENTIFIER NOT NULL,
    Number NVARCHAR(20) NOT NULL,
    Type INT NOT NULL,
    IsPrimary BIT NOT NULL
);
GO

ALTER TABLE Phones
ADD CONSTRAINT FK_Phones_Employees
FOREIGN KEY (EmployeeId)
REFERENCES Employees(Id)
ON DELETE CASCADE;
GO

INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'8f6f50c6-436a-4bcd-f598-08de36d28449', N'Admin', N'Master', N'admin@employee.com', N'12345678910', N'$2a$11$6etpSVVZcZWTl3PcLPBEM..WK7AUbDoSmYO46lM9pdX2ky8T2gv4u', 4, 1, CAST(N'2000-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-12-10T05:32:34.2200000' AS DateTime2), NULL, NULL)
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'014b6c30-dde5-4766-ba2d-08de37af15e1', N'Lider', N'Teste Tres', N'liderTeste03@gmail.com', N'12345678912', N'$2a$11$M.6NBIWawcWruH1lyDM6i.9iPgQ5dE/b9MixIMy2Hrf8XCZ2kquXG', 3, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T05:43:45.8896074' AS DateTime2), CAST(N'2025-12-10T05:43:45.8902379' AS DateTime2), NULL)
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'a4fbc22b-a3b9-4721-ba2e-08de37af15e1', N'Lider', N'Teste Tres', N'liderTeste04@gmail.com', N'12345678913', N'$2a$11$6YmjcYMPESlYq7aQjwZeOOJ1p4QzMAb8lmbNjGilqmUY.XiUhhwyK', 3, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T05:44:00.3674396' AS DateTime2), CAST(N'2025-12-10T05:44:00.3674413' AS DateTime2), NULL)
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'0901a2bd-bf1f-4388-18b4-08de37b01c49', N'Lider', N'Teste Tres', N'liderTeste07@gmail.com', N'22345678923', N'$2a$11$HPinPKe0a11w6U6btvHc8.oJGEWlH63MPFf.6POejs/2qMf27I7nq', 1, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T05:51:06.1297794' AS DateTime2), CAST(N'2025-12-10T05:51:06.1299643' AS DateTime2), NULL)
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'da77e39b-08ee-4792-18b5-08de37b01c49', N'Funcionario', N'Teste Um', N'liderTeste08@gmail.com', N'22345678924', N'$2a$11$UsiD.GlSmyNYwA9wRZfbL.0tE6x2mpU9/KfbFqx/DkBly5WoDjeke', 1, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T05:51:35.7952300' AS DateTime2), CAST(N'2025-12-10T05:51:35.7952306' AS DateTime2), NULL)
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'59aa29f0-25c5-4b87-18b6-08de37b01c49', N'Funcionario', N'Teste Um', N'funcionarioTeste08@gmail.com', N'22345678925', N'$2a$11$wn3GdjLXc88SrGKGdIXR8.TkvdIhuvdRlQ89VfgStEGYSMCVAzM/q', 1, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T05:51:54.0423503' AS DateTime2), CAST(N'2025-12-10T05:51:54.0423509' AS DateTime2), NULL)
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'd8dfb2e4-af05-445d-18b7-08de37b01c49', N'Funcionario', N'Teste Um', N'funcionarioTeste09@gmail.com', N'22345672925', N'$2a$11$/rmvBo5a6khYZM4Brh.shO71SX6tHIUB4SadgvgTSKrLt9qOpuVgC', 1, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T05:52:23.4231696' AS DateTime2), CAST(N'2025-12-10T05:52:23.4231702' AS DateTime2), NULL)
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'e04d4234-e677-443f-ad74-08de37b07a47', N'Funcionario', N'Teste Um', N'funcionarioTeste10@gmail.com', N'22345672928', N'$2a$11$W1Q9Z/DXCiVM0ypTCgCeIeWBECqFIQfK6/7oyW0B2e3ChzXp7llv6', 1, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T05:53:43.8296980' AS DateTime2), CAST(N'2025-12-10T05:53:43.8298148' AS DateTime2), NULL)
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'8243d6a4-cbd3-488b-ad75-08de37b07a47', N'Funcionario', N'Teste Um', N'funcionarioTeste11@gmail.com', N'22345672929', N'$2a$11$Ie5iiiZ/aWW4XfTRN5uwhum/vz/pLs3UehlHiecawOPpmv9SJ2WWm', 1, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T05:54:37.5036875' AS DateTime2), CAST(N'2025-12-10T05:54:37.5036880' AS DateTime2), NULL)
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'f4cb8719-963c-4247-797a-08de37b0e96d', N'Funcionario', N'Teste dois', N'funcionarioTeste12@gmail.com', N'22345672999', N'$2a$11$4jaDYka8tPvlQC1G19Uz7eHbZRBTTmEWGS5mborHTBXQjDvNSxWnC', 1, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T05:56:50.3066418' AS DateTime2), CAST(N'2025-12-10T05:56:50.3067688' AS DateTime2), NULL)
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'0140de11-01d6-4b62-08d8-08de37b129b3', N'Funcionario', N'Teste dois', N'funcionarioTeste13@gmail.com', N'22345675999', N'$2a$11$t/e7wHXCzr2GmHxxwM96VegzqQF6jPZ/HMTvPgpIASR5MoBy2lqwa', 3, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T05:58:38.1376844' AS DateTime2), CAST(N'2025-12-10T05:58:38.1378552' AS DateTime2), NULL)
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'a3c58ee5-33bb-4df2-08d9-08de37b129b3', N'Lider', N'Teste Falha', N'funcionarioTeste14@gmail.com', N'22342675999', N'$2a$11$EEBamXIr8tOd7WY7WVN0DeDdHxiQu/wRUBef0qLL3F4fXDdhqifgG', 2, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T06:01:22.7208520' AS DateTime2), CAST(N'2025-12-10T06:01:22.7208528' AS DateTime2), N'0140de11-01d6-4b62-08d8-08de37b129b3')
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'e49bbf56-597f-4d0b-08da-08de37b129b3', N'Lider', N'Teste Falha', N'funcionarioTeste15@gmail.com', N'22342675909', N'$2a$11$Tox/pNc6/9qje/hwoXdDj.18RjG1XR3zLf67HwcKAbjg0oJeW7v4q', 2, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T06:02:57.1222296' AS DateTime2), CAST(N'2025-12-10T06:02:57.1222300' AS DateTime2), N'0140de11-01d6-4b62-08d8-08de37b129b3')
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'33e088ed-ecd1-42b4-08db-08de37b129b3', N'Lider', N'Teste Falha', N'funcionarioTeste16@gmail.com', N'22342675902', N'$2a$11$9cI7ljiU/wi8ANCSxmHxveQ9CbxislfFdqr8916R5hDLDg6Yp047q', 2, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T06:03:12.8032046' AS DateTime2), CAST(N'2025-12-10T06:03:12.8032054' AS DateTime2), N'0140de11-01d6-4b62-08d8-08de37b129b3')
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'20fa951e-e390-4af2-08dc-08de37b129b3', N'Lider', N'Teste Falha', N'funcionarioTeste19@gmail.com', N'22342175997', N'$2a$11$siNoTcwAuhSRWLQ7/y3Pf.qPLvE1EOz0YRgbJhW2xdSdLQNy.mvYK', 1, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T06:05:58.0879576' AS DateTime2), CAST(N'2025-12-10T06:05:58.0879584' AS DateTime2), N'a4fbc22b-a3b9-4721-ba2e-08de37af15e1')
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'd0017946-7a3a-427d-08dd-08de37b129b3', N'Lider', N'Teste Sucesso', N'funcionarioTeste20@gmail.com', N'92342175997', N'$2a$11$HC5OxDxNqFum3uTWn.3Dy.Rmrx958h.UxqYXyR5JdX78E5554/1XS', 1, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T06:06:43.9569022' AS DateTime2), CAST(N'2025-12-10T06:06:43.9573637' AS DateTime2), N'a4fbc22b-a3b9-4721-ba2e-08de37af15e1')
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'b5d7ae05-52f1-401c-08de-08de37b129b3', N'Lider', N'Teste Sucesso', N'funcionarioTeste21@gmail.com', N'93342175997', N'$2a$11$ORcE90lh3sdRl00CvBIe6ejyfGFXWwgMmREljU05V0SG0DLkK6V26', 1, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T06:07:05.7479305' AS DateTime2), CAST(N'2025-12-10T06:07:05.7479398' AS DateTime2), N'a4fbc22b-a3b9-4721-ba2e-08de37af15e1')
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'f8dbefae-579b-4680-fd68-08de37b4d6e5', N'Funcionario', N'Base Sucesso', N'funcionarioTeste22@gmail.com', N'96642175997', N'$2a$11$Z2U7Csns2Y.NpgrjuzpOLehLXiN3a.95rc8/xHP19DEJkDgms9sHq', 1, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T06:24:57.2071059' AS DateTime2), CAST(N'2025-12-10T06:24:57.2074881' AS DateTime2), N'a4fbc22b-a3b9-4721-ba2e-08de37af15e1')
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'50604803-9689-4612-fd69-08de37b4d6e5', N'Funcionario', N'Base Sucesso', N'funcionarioTeste23@gmail.com', N'96642175967', N'$2a$11$2k4Z/eTLMzdTvnmDTR7rZONMqewa8gLiJVZcwrQT2CXlhGBgPq33e', 1, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T06:26:18.3132397' AS DateTime2), CAST(N'2025-12-10T06:26:18.3132450' AS DateTime2), N'a4fbc22b-a3b9-4721-ba2e-08de37af15e1')
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'0641eda4-3d8d-4afa-e26c-08de37bcc72c', N'Lider', N'Base', N'funcionarioTeste24@gmail.com', N'96642175987', N'$2a$11$DiJMe3MQGCaqnucbXm09RufgzLDYf8VBcu3Wa43UMZaDER4a6TMq6', 3, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T07:21:46.8020492' AS DateTime2), CAST(N'2025-12-10T07:21:46.8023782' AS DateTime2), NULL)
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'aefc62c4-7bc5-421e-e26d-08de37bcc72c', N'Medio', N'Base', N'funcionarioTeste25@gmail.com', N'12642175987', N'$2a$11$gpsmXvvC6LYfPPqT6XDvXOCF1zKd/iftw4KuZBYP19t5uIBNvJQE2', 3, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T07:22:26.4663817' AS DateTime2), CAST(N'2025-12-10T07:22:26.4663872' AS DateTime2), N'0641eda4-3d8d-4afa-e26c-08de37bcc72c')
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'504859fa-2106-440d-e26e-08de37bcc72c', N'piso', N'Base', N'funcionarioTeste26@gmail.com', N'12642175927', N'$2a$11$2Lxk/993BS/IgQ6KjlHvk.75foozjyVEo/RdTa6Ys.GaMY1gl/n3q', 3, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T07:22:55.2124334' AS DateTime2), CAST(N'2025-12-10T07:22:55.2124391' AS DateTime2), N'aefc62c4-7bc5-421e-e26d-08de37bcc72c')
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'2245b463-3494-4e07-d6ea-08de37ef9cd1', N'Lider Geral', N'Base', N'funcionarioTeste27@gmail.com', N'12442175927', N'$2a$11$aQjGY76mih8Ap0Ru395mW.rliqi19tFahkQrPDjdqfaAyBQub1AQe', 3, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T13:25:40.0748127' AS DateTime2), CAST(N'2025-12-10T13:25:40.0752309' AS DateTime2), NULL)
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'465b21c1-8d9e-4121-d6eb-08de37ef9cd1', N'LiderMedio', N'Base', N'funcionarioTeste28@gmail.com', N'12442175987', N'$2a$11$4JVUL1yTtD.lVy0x.VcIBeDXDcrDHo6EGAekBnTDHKumWRrZBl.e.', 2, 0, CAST(N'2000-12-10T05:17:40.4130000' AS DateTime2), CAST(N'2025-12-10T13:27:16.2160797' AS DateTime2), CAST(N'2025-12-10T13:27:16.2160851' AS DateTime2), N'2245b463-3494-4e07-d6ea-08de37ef9cd1')
GO
INSERT [dbo].[Employees] ([Id], [FirstName], [LastName], [Email], [DocumentNumber], [Password], [Role], [Status], [BirthDate], [CreatedAt], [UpdatedAt], [ManagerId]) VALUES (N'6f2dff9d-51b0-433f-d6ec-08de37ef9cd1', N'Funcionario', N'Atualizado Novamente', N'funcionarioTeste8@gmail.com', N'12462175987', N'$2a$11$VeW5ZifhSrG5wvb4ZYzi9.JG4x1zn.7CB3k5iw4w8ho0cQaojW1xa', 1, 0, CAST(N'2000-12-10T06:52:47.3080000' AS DateTime2), CAST(N'2025-12-10T13:27:39.8973235' AS DateTime2), CAST(N'2025-12-10T13:41:36.6274761' AS DateTime2), N'465b21c1-8d9e-4121-d6eb-08de37ef9cd1')
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'e2a67519-a5a8-4267-58da-08de37af15e4', N'014b6c30-dde5-4766-ba2d-08de37af15e1', N'554199177828', 1, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'fbd136ae-f340-4038-5a0e-08de37b24b43', N'd0017946-7a3a-427d-08dd-08de37b129b3', N'41991777828', 1, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'c5d30c49-c642-4339-5a0f-08de37b24b43', N'b5d7ae05-52f1-401c-08de-08de37b129b3', N'41991777828', 1, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'0390170e-dfe2-4b62-5153-08de37b4d6e7', N'f8dbefae-579b-4680-fd68-08de37b4d6e5', N'41991777828', 1, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'bbc6c2c8-f688-4ec2-5154-08de37b4d6e7', N'50604803-9689-4612-fd69-08de37b4d6e5', N'41991777828', 1, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'e461f96f-de94-4fdc-5155-08de37b4d6e7', N'50604803-9689-4612-fd69-08de37b4d6e5', N'4111111111', 2, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'643faa08-eadf-4fd2-5156-08de37b4d6e7', N'50604803-9689-4612-fd69-08de37b4d6e5', N'4122222222', 3, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'263b8608-4d2a-47aa-84f2-08de37bcc72e', N'0641eda4-3d8d-4afa-e26c-08de37bcc72c', N'41991777828', 1, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'b264b05f-063e-49c7-84f3-08de37bcc72e', N'0641eda4-3d8d-4afa-e26c-08de37bcc72c', N'4111111111', 2, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'0b8e3cff-36c2-47b7-84f4-08de37bcc72e', N'0641eda4-3d8d-4afa-e26c-08de37bcc72c', N'4122222222', 3, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'10eb6cff-1012-4b46-84f5-08de37bcc72e', N'aefc62c4-7bc5-421e-e26d-08de37bcc72c', N'41991777828', 1, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'6c154800-dfb9-4c9e-84f6-08de37bcc72e', N'aefc62c4-7bc5-421e-e26d-08de37bcc72c', N'4111111111', 2, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'444821d9-6a31-41cd-84f7-08de37bcc72e', N'aefc62c4-7bc5-421e-e26d-08de37bcc72c', N'4122222222', 3, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'61fd3f80-f4fe-4dee-84f8-08de37bcc72e', N'504859fa-2106-440d-e26e-08de37bcc72c', N'41991777828', 1, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'4f1cc1fe-548a-4451-84f9-08de37bcc72e', N'504859fa-2106-440d-e26e-08de37bcc72c', N'4111111111', 2, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'845cfa68-9419-4a5b-84fa-08de37bcc72e', N'504859fa-2106-440d-e26e-08de37bcc72c', N'4122222222', 3, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'efb643aa-271b-459a-46e3-08de37ef9cd3', N'2245b463-3494-4e07-d6ea-08de37ef9cd1', N'41991777828', 1, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'3a2a1039-ba36-40c7-46e4-08de37ef9cd3', N'2245b463-3494-4e07-d6ea-08de37ef9cd1', N'4111111111', 2, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'ba3f6870-54e1-4fe2-46e5-08de37ef9cd3', N'2245b463-3494-4e07-d6ea-08de37ef9cd1', N'4122222222', 3, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'c28ccb80-ae90-4012-46e6-08de37ef9cd3', N'465b21c1-8d9e-4121-d6eb-08de37ef9cd1', N'41991777828', 1, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'304e9d35-1b60-4f97-46e7-08de37ef9cd3', N'465b21c1-8d9e-4121-d6eb-08de37ef9cd1', N'4111111111', 2, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'ebfbd3c3-a6ab-4cf1-46e8-08de37ef9cd3', N'465b21c1-8d9e-4121-d6eb-08de37ef9cd1', N'4122222222', 3, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'f61f3cec-02f9-4144-46e9-08de37ef9cd3', N'6f2dff9d-51b0-433f-d6ec-08de37ef9cd1', N'41991777777', 1, 1)
GO
INSERT [dbo].[Phones] ([Id], [EmployeeId], [Number], [Type], [IsPrimary]) VALUES (N'a648bee1-d313-4b5e-41a0-08de37f1cd35', N'6f2dff9d-51b0-433f-d6ec-08de37ef9cd1', N'41996496471', 1, 1)
GO
