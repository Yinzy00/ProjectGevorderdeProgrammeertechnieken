USE [r0849413]
GO
/****** Object:  Table [bib].[Boekenbeurs]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[Boekenbeurs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Naam] [nvarchar](max) NOT NULL,
	[DatumVan] [datetime] NOT NULL,
	[DatumTot] [datetime] NULL,
	[Locatie] [nvarchar](max) NULL,
	[InkomPrijs] [float] NOT NULL,
 CONSTRAINT [PK_bib.Boekenbeurs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [bib].[Categorie]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[Categorie](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Omschrijving] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_bib.Categorie] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [bib].[Extra]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[Extra](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SoortId] [int] NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_bib.Extra] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [bib].[Functie]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[Functie](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Naam] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_bib.Functie] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [bib].[Gebruiker]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[Gebruiker](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Naam] [nvarchar](max) NULL,
	[Voornaam] [nvarchar](max) NULL,
	[Adres] [nvarchar](max) NULL,
	[Postcode] [nvarchar](max) NULL,
	[Woonplaats] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Wachtwoord] [nvarchar](max) NOT NULL,
	[Key] [nvarchar](max) NOT NULL,
	[Admin] [int] NOT NULL,
	[LidmaatschapAanvraag] [datetime] NULL,
 CONSTRAINT [PK_bib.Gebruiker] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [bib].[GebruikerBoekenbeurs]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[GebruikerBoekenbeurs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GebruikerId] [int] NOT NULL,
	[BoekenbeursId] [int] NOT NULL,
	[IngeschrevenOp] [datetime] NOT NULL,
 CONSTRAINT [PK_bib.GebruikerBoekenbeurs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [bib].[LeeftijdsKlasse]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[LeeftijdsKlasse](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Omschrijving] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_bib.LeeftijdsKlasse] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [bib].[Lidgeld]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[Lidgeld](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LidgeldBetaaldOp] [datetime] NOT NULL,
	[DuurLidmaatschap] [datetime] NOT NULL,
	[GebruikerId] [int] NOT NULL,
	[LastEmail] [datetime] NULL,
 CONSTRAINT [PK_bib.Lidgeld] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [bib].[Medewerker]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[Medewerker](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Naam] [nvarchar](max) NOT NULL,
	[FunctieId] [int] NOT NULL,
 CONSTRAINT [PK_bib.Medewerker] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [bib].[Medium]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[Medium](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MediumDetailId] [int] NOT NULL,
	[Registratie] [datetime] NOT NULL,
	[EindeLevensduur] [datetime] NOT NULL,
	[Verkoopprijs] [float] NOT NULL,
	[Verkocht] [bit] NULL,
 CONSTRAINT [PK_bib.Medium] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [bib].[MediumCategorie]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[MediumCategorie](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MediumDetailId] [int] NOT NULL,
	[CategorieId] [int] NOT NULL,
 CONSTRAINT [PK_bib.MediumCategorie] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [bib].[MediumDetail]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[MediumDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[LeeftijdsKlasseId] [int] NOT NULL,
	[Ean] [nvarchar](max) NOT NULL,
	[SoortId] [int] NOT NULL,
	[KorteInhoud] [nvarchar](max) NULL,
 CONSTRAINT [PK_bib.MediumDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [bib].[MediumDetailExtra]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[MediumDetailExtra](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MediumDetailId] [int] NOT NULL,
	[ExtraId] [int] NOT NULL,
	[Beschrijving] [nvarchar](max) NULL,
 CONSTRAINT [PK_bib.MediumDetailExtra] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [bib].[MediumDetailMedewerker]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[MediumDetailMedewerker](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MedewerkerId] [int] NOT NULL,
	[MediumDetailId] [int] NOT NULL,
 CONSTRAINT [PK_bib.MediumDetailMedewerker] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [bib].[MediumVerkoop]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[MediumVerkoop](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IntresseDatum] [datetime] NOT NULL,
	[GebruikerId] [int] NOT NULL,
	[MediumId] [int] NOT NULL,
 CONSTRAINT [PK_bib.MediumVerkoop] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [bib].[Soort]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[Soort](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Naam] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_bib.Soort] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [bib].[Uitlening]    Script Date: 29-12-2021 17:27:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [bib].[Uitlening](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MediumId] [int] NOT NULL,
	[GebruikerId] [int] NULL,
	[OntleenIntresse] [datetime] NOT NULL,
	[UitgeleendOp] [datetime] NULL,
	[Binnengebracht] [datetime] NULL,
	[BoeteBetaald] [datetime] NULL,
	[LaatsteEmail] [datetime] NULL,
 CONSTRAINT [PK_bib.Uitlening] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [bib].[Extra]  WITH CHECK ADD  CONSTRAINT [FK_bib.Extra_bib.Soort_SoortId] FOREIGN KEY([SoortId])
REFERENCES [bib].[Soort] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [bib].[Extra] CHECK CONSTRAINT [FK_bib.Extra_bib.Soort_SoortId]
GO
ALTER TABLE [bib].[GebruikerBoekenbeurs]  WITH CHECK ADD  CONSTRAINT [FK_bib.GebruikerBoekenbeurs_bib.Boekenbeurs_BoekenbeursId] FOREIGN KEY([BoekenbeursId])
REFERENCES [bib].[Boekenbeurs] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [bib].[GebruikerBoekenbeurs] CHECK CONSTRAINT [FK_bib.GebruikerBoekenbeurs_bib.Boekenbeurs_BoekenbeursId]
GO
ALTER TABLE [bib].[GebruikerBoekenbeurs]  WITH CHECK ADD  CONSTRAINT [FK_bib.GebruikerBoekenbeurs_bib.Gebruiker_GebruikerId] FOREIGN KEY([GebruikerId])
REFERENCES [bib].[Gebruiker] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [bib].[GebruikerBoekenbeurs] CHECK CONSTRAINT [FK_bib.GebruikerBoekenbeurs_bib.Gebruiker_GebruikerId]
GO
ALTER TABLE [bib].[Lidgeld]  WITH CHECK ADD  CONSTRAINT [FK_bib.Lidgeld_bib.Gebruiker_GebruikerId] FOREIGN KEY([GebruikerId])
REFERENCES [bib].[Gebruiker] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [bib].[Lidgeld] CHECK CONSTRAINT [FK_bib.Lidgeld_bib.Gebruiker_GebruikerId]
GO
ALTER TABLE [bib].[Medewerker]  WITH CHECK ADD  CONSTRAINT [FK_bib.Medewerker_bib.Functie_FunctieId] FOREIGN KEY([FunctieId])
REFERENCES [bib].[Functie] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [bib].[Medewerker] CHECK CONSTRAINT [FK_bib.Medewerker_bib.Functie_FunctieId]
GO
ALTER TABLE [bib].[Medium]  WITH CHECK ADD  CONSTRAINT [FK_bib.Medium_bib.MediumDetail_MediumDetailId] FOREIGN KEY([MediumDetailId])
REFERENCES [bib].[MediumDetail] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [bib].[Medium] CHECK CONSTRAINT [FK_bib.Medium_bib.MediumDetail_MediumDetailId]
GO
ALTER TABLE [bib].[MediumCategorie]  WITH CHECK ADD  CONSTRAINT [FK_bib.MediumCategorie_bib.Categorie_CategorieId] FOREIGN KEY([CategorieId])
REFERENCES [bib].[Categorie] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [bib].[MediumCategorie] CHECK CONSTRAINT [FK_bib.MediumCategorie_bib.Categorie_CategorieId]
GO
ALTER TABLE [bib].[MediumCategorie]  WITH CHECK ADD  CONSTRAINT [FK_bib.MediumCategorie_bib.MediumDetail_MediumDetailId] FOREIGN KEY([MediumDetailId])
REFERENCES [bib].[MediumDetail] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [bib].[MediumCategorie] CHECK CONSTRAINT [FK_bib.MediumCategorie_bib.MediumDetail_MediumDetailId]
GO
ALTER TABLE [bib].[MediumDetail]  WITH CHECK ADD  CONSTRAINT [FK_bib.MediumDetail_bib.LeeftijdsKlasse_LeeftijdsKlasseId] FOREIGN KEY([LeeftijdsKlasseId])
REFERENCES [bib].[LeeftijdsKlasse] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [bib].[MediumDetail] CHECK CONSTRAINT [FK_bib.MediumDetail_bib.LeeftijdsKlasse_LeeftijdsKlasseId]
GO
ALTER TABLE [bib].[MediumDetail]  WITH CHECK ADD  CONSTRAINT [FK_bib.MediumDetail_bib.Soort_SoortId] FOREIGN KEY([SoortId])
REFERENCES [bib].[Soort] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [bib].[MediumDetail] CHECK CONSTRAINT [FK_bib.MediumDetail_bib.Soort_SoortId]
GO
ALTER TABLE [bib].[MediumDetailExtra]  WITH CHECK ADD  CONSTRAINT [FK_bib.MediumDetailExtra_bib.Extra_ExtraId] FOREIGN KEY([ExtraId])
REFERENCES [bib].[Extra] ([Id])
GO
ALTER TABLE [bib].[MediumDetailExtra] CHECK CONSTRAINT [FK_bib.MediumDetailExtra_bib.Extra_ExtraId]
GO
ALTER TABLE [bib].[MediumDetailExtra]  WITH CHECK ADD  CONSTRAINT [FK_bib.MediumDetailExtra_bib.MediumDetail_MediumDetailId] FOREIGN KEY([MediumDetailId])
REFERENCES [bib].[MediumDetail] ([Id])
GO
ALTER TABLE [bib].[MediumDetailExtra] CHECK CONSTRAINT [FK_bib.MediumDetailExtra_bib.MediumDetail_MediumDetailId]
GO
ALTER TABLE [bib].[MediumDetailMedewerker]  WITH CHECK ADD  CONSTRAINT [FK_bib.MediumDetailMedewerker_bib.Medewerker_MedewerkerId] FOREIGN KEY([MedewerkerId])
REFERENCES [bib].[Medewerker] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [bib].[MediumDetailMedewerker] CHECK CONSTRAINT [FK_bib.MediumDetailMedewerker_bib.Medewerker_MedewerkerId]
GO
ALTER TABLE [bib].[MediumDetailMedewerker]  WITH CHECK ADD  CONSTRAINT [FK_bib.MediumDetailMedewerker_bib.MediumDetail_MediumDetailId] FOREIGN KEY([MediumDetailId])
REFERENCES [bib].[MediumDetail] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [bib].[MediumDetailMedewerker] CHECK CONSTRAINT [FK_bib.MediumDetailMedewerker_bib.MediumDetail_MediumDetailId]
GO
ALTER TABLE [bib].[MediumVerkoop]  WITH CHECK ADD  CONSTRAINT [FK_bib.MediumVerkoop_bib.Gebruiker_GebruikerId] FOREIGN KEY([GebruikerId])
REFERENCES [bib].[Gebruiker] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [bib].[MediumVerkoop] CHECK CONSTRAINT [FK_bib.MediumVerkoop_bib.Gebruiker_GebruikerId]
GO
ALTER TABLE [bib].[MediumVerkoop]  WITH CHECK ADD  CONSTRAINT [FK_bib.MediumVerkoop_bib.Medium_MediumId] FOREIGN KEY([MediumId])
REFERENCES [bib].[Medium] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [bib].[MediumVerkoop] CHECK CONSTRAINT [FK_bib.MediumVerkoop_bib.Medium_MediumId]
GO
ALTER TABLE [bib].[Uitlening]  WITH CHECK ADD  CONSTRAINT [FK_bib.Uitlening_bib.Gebruiker_GebruikerId] FOREIGN KEY([GebruikerId])
REFERENCES [bib].[Gebruiker] ([Id])
GO
ALTER TABLE [bib].[Uitlening] CHECK CONSTRAINT [FK_bib.Uitlening_bib.Gebruiker_GebruikerId]
GO
ALTER TABLE [bib].[Uitlening]  WITH CHECK ADD  CONSTRAINT [FK_bib.Uitlening_bib.Medium_MediumId] FOREIGN KEY([MediumId])
REFERENCES [bib].[Medium] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [bib].[Uitlening] CHECK CONSTRAINT [FK_bib.Uitlening_bib.Medium_MediumId]
GO
