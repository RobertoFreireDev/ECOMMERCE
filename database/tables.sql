IF NOT EXISTS (
    SELECT 1
    FROM sys.databases
    WHERE name = 'EventsDb'
)
BEGIN
    CREATE DATABASE EventsDb;
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.tables
    WHERE name = 'Events'
)
BEGIN
    CREATE TABLE dbo.Events (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        EventType INT NOT NULL,
        Payload NVARCHAR(MAX) NOT NULL,
        OccurredOnUtc DATETIME2 NOT NULL
    );
END;


SELECT *
FROM dbo.Events;