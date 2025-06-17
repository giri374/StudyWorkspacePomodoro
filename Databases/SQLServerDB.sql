CREATE DATABASE PomodoroDB;
GO

USE PomodoroDB;
GO

--bảng không gian làm việc
CREATE TABLE Workspaces (
    WorkspaceID INT IDENTITY(1,1) PRIMARY KEY,
    WorkspaceName NVARCHAR(100) NOT NULL,
    BackgroundImage NVARCHAR(255),
);
GO

CREATE TABLE Music (
    MusicID INT IDENTITY(1,1) PRIMARY KEY,
    SongName NVARCHAR(100) NOT NULL,
    Genre NVARCHAR(50),
    MusicFile NVARCHAR(255),
);
GO

-- set time of pomodoro
CREATE TABLE PomodoroSettings (
    SettingID INT IDENTITY(1,1) PRIMARY KEY,
    PomodoroDuration INT NOT NULL DEFAULT 25,   -- minute
    ShortBreakDuration INT NOT NULL DEFAULT 5,  -- minute
    LongBreakDuration INT NOT NULL DEFAULT 15,  -- minute
);
GO



