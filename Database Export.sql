USE [LOTO]
GO
/****** Object:  Table [dbo].[Jugada]    Script Date: 28/11/2020 22:51:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jugada](
	[ID] [uniqueidentifier] NOT NULL,
	[Numero] [nchar](10) NOT NULL,
	[Repetido] [int] NOT NULL,
	[LoteriaId] [int] NULL,
 CONSTRAINT [PK_Jugada] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Loteria]    Script Date: 28/11/2020 22:51:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Loteria](
	[ID] [int] NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Loteria] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ticket]    Script Date: 28/11/2020 22:51:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticket](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PIN] [int] NOT NULL,
	[Anulado] [bit] NOT NULL,
	[Creado] [smalldatetime] NULL,
 CONSTRAINT [PK_Ticket] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ticket_Jugada]    Script Date: 28/11/2020 22:51:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ticket_Jugada](
	[TicketID] [int] NOT NULL,
	[JugadaID] [uniqueidentifier] NOT NULL,
	[Puntos] [int] NOT NULL,
 CONSTRAINT [PK_Ticket_Jugada] PRIMARY KEY CLUSTERED 
(
	[TicketID] ASC,
	[JugadaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Jugada]  WITH CHECK ADD  CONSTRAINT [FK_Jugada_Loteria] FOREIGN KEY([LoteriaId])
REFERENCES [dbo].[Loteria] ([ID])
GO
ALTER TABLE [dbo].[Jugada] CHECK CONSTRAINT [FK_Jugada_Loteria]
GO
ALTER TABLE [dbo].[Ticket_Jugada]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_Jugada_Jugada] FOREIGN KEY([JugadaID])
REFERENCES [dbo].[Jugada] ([ID])
GO
ALTER TABLE [dbo].[Ticket_Jugada] CHECK CONSTRAINT [FK_Ticket_Jugada_Jugada]
GO
ALTER TABLE [dbo].[Ticket_Jugada]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_Jugada_Ticket] FOREIGN KEY([TicketID])
REFERENCES [dbo].[Ticket] ([ID])
GO
ALTER TABLE [dbo].[Ticket_Jugada] CHECK CONSTRAINT [FK_Ticket_Jugada_Ticket]
GO
