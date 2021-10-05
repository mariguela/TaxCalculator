CREATE TABLE [dbo].[Tax] (
    [TaxId]      INT             IDENTITY (1, 1) NOT NULL,
    [PostalCode] VARCHAR (10)    NOT NULL,
    [Salary]     NUMERIC (12, 2) NULL,
    [CreateAt]   DATETIME        NULL,
    [TaxValue]   NUMERIC (12, 2) NULL,
    [Type]       SMALLINT        NULL
);