
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/17/2021 16:04:05
-- Generated from EDMX file: C:\Users\Zeynep ARSLAN\source\repos\yazilimYapimi\Models\Entity\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MvcDbProje];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'tableConfirmMoney'
CREATE TABLE [dbo].[tableConfirmMoney] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Money] decimal(19,4)  NULL,
    [UserID] int  NULL,
    [Confirmed] bit  NULL
);
GO

-- Creating table 'tableConfirmProduct'
CREATE TABLE [dbo].[tableConfirmProduct] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductName] varchar(50)  NULL,
    [UserID] int  NULL,
    [Confirmed] bit  NULL,
    [Price] decimal(19,4)  NULL,
    [Quantity] int  NULL
);
GO

-- Creating table 'tableProduct'
CREATE TABLE [dbo].[tableProduct] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductName] varchar(50)  NULL,
    [Price] decimal(19,4)  NULL,
    [Quantity] int  NULL,
    [UserID] int  NULL
);
GO

-- Creating table 'tableUser'
CREATE TABLE [dbo].[tableUser] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [Surname] varchar(50)  NULL,
    [UserName] varchar(50)  NULL,
    [Password] nvarchar(50)  NULL,
    [TC] varchar(11)  NULL,
    [PhoneNumber] varchar(20)  NULL,
    [Email] nvarchar(50)  NULL,
    [Adress] nvarchar(250)  NULL,
    [UserType] bit  NOT NULL
);
GO

-- Creating table 'tableWallet'
CREATE TABLE [dbo].[tableWallet] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Money] decimal(19,4)  NULL,
    [UserID] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [ID] in table 'tableConfirmMoney'
ALTER TABLE [dbo].[tableConfirmMoney]
ADD CONSTRAINT [PK_tableConfirmMoney]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tableConfirmProduct'
ALTER TABLE [dbo].[tableConfirmProduct]
ADD CONSTRAINT [PK_tableConfirmProduct]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tableProduct'
ALTER TABLE [dbo].[tableProduct]
ADD CONSTRAINT [PK_tableProduct]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tableUser'
ALTER TABLE [dbo].[tableUser]
ADD CONSTRAINT [PK_tableUser]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'tableWallet'
ALTER TABLE [dbo].[tableWallet]
ADD CONSTRAINT [PK_tableWallet]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserID] in table 'tableConfirmMoney'
ALTER TABLE [dbo].[tableConfirmMoney]
ADD CONSTRAINT [FK_tableConfirmMoney_tableUser]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[tableUser]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tableConfirmMoney_tableUser'
CREATE INDEX [IX_FK_tableConfirmMoney_tableUser]
ON [dbo].[tableConfirmMoney]
    ([UserID]);
GO

-- Creating foreign key on [UserID] in table 'tableConfirmProduct'
ALTER TABLE [dbo].[tableConfirmProduct]
ADD CONSTRAINT [FK_tableConfirmProduct_tableUser]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[tableUser]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tableConfirmProduct_tableUser'
CREATE INDEX [IX_FK_tableConfirmProduct_tableUser]
ON [dbo].[tableConfirmProduct]
    ([UserID]);
GO

-- Creating foreign key on [UserID] in table 'tableProduct'
ALTER TABLE [dbo].[tableProduct]
ADD CONSTRAINT [FK_tableProduct_tableUser]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[tableUser]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tableProduct_tableUser'
CREATE INDEX [IX_FK_tableProduct_tableUser]
ON [dbo].[tableProduct]
    ([UserID]);
GO

-- Creating foreign key on [UserID] in table 'tableWallet'
ALTER TABLE [dbo].[tableWallet]
ADD CONSTRAINT [FK_tableWallet_tableUser]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[tableUser]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tableWallet_tableUser'
CREATE INDEX [IX_FK_tableWallet_tableUser]
ON [dbo].[tableWallet]
    ([UserID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------