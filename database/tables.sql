IF NOT EXISTS (
    SELECT 1
    FROM sys.databases
    WHERE name = 'EventsDb'
)
BEGIN
    CREATE DATABASE EventsDb;
END;
GO

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
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.tables
    WHERE name = 'FailedEvents'
)
BEGIN
    CREATE TABLE dbo.FailedEvents (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        EventId UNIQUEIDENTIFIER NOT NULL,
        LastAttemptUtc DATETIME2 NULL,
        CONSTRAINT FK_FailedEvents_Events FOREIGN KEY (EventId) REFERENCES dbo.Events(Id)
    );
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.tables
    WHERE name = 'DeadLetterEvents'
)
BEGIN
    CREATE TABLE dbo.DeadLetterEvents (
        Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        EventId UNIQUEIDENTIFIER NOT NULL,
        FailedOnUtc DATETIME2 NOT NULL,
        CONSTRAINT FK_DeadLetterEvents_Events FOREIGN KEY (EventId) REFERENCES dbo.Events(Id)
    );
END;
GO

use EventsDb;

SELECT * FROM dbo.Events;
SELECT * FROM dbo.FailedEvents;
SELECT * FROM dbo.DeadLetterEvents;

DELETE FROM dbo.FailedEvents;
DELETE FROM dbo.DeadLetterEvents;
DELETE FROM dbo.Events;

drop table DeadLetterEvents;
drop table FailedEvents;
drop table Events;
drop database EventsDb;