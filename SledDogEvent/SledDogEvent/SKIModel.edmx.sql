-- --------------------------------------------------
-- Entity Designer DDL Script for Oracle database
-- --------------------------------------------------
-- Date Created: 3/7/2016 2:33:33 PM
-- Generated from EDMX file: C:\Workspaces\OracleAndEntityFramework\SledDogEvent\SledDogEvent\SKIModel.edmx
-- --------------------------------------------------

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

-- ALTER TABLE "SKI"."Races" DROP CONSTRAINT "FK_EventRace" CASCADE;

-- ALTER TABLE "SKI"."Teams" DROP CONSTRAINT "FK_RaceTeam" CASCADE;

-- ALTER TABLE "SKI"."Teams_Mushing" DROP CONSTRAINT "FK_Mushing_inherits_Team" CASCADE;

-- ALTER TABLE "SKI"."Teams_Skijoring" DROP CONSTRAINT "FK_Skijoring_inherits_Team" CASCADE;
-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

-- DROP TABLE "SKI"."Events";

-- DROP TABLE "SKI"."Races";

-- DROP TABLE "SKI"."Teams";

-- DROP TABLE "SKI"."Teams_Mushing";

-- DROP TABLE "SKI"."Teams_Skijoring";

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Events'
CREATE TABLE "SKI"."Events" (
   "EventId" NUMBER(10) NOT NULL,
   "Name" NVARCHAR2(75) NOT NULL,
   "EventDate" DATE NOT NULL,
   "Sponsor" NVARCHAR2(50) ,
   "Location" NVARCHAR2(50) 
);

-- Creating table 'Races'
CREATE TABLE "SKI"."Races" (
   "RaceId" NUMBER(10) NOT NULL,
   "Distance" NUMBER(38) NOT NULL,
   "DogCount" NUMBER(10) NOT NULL,
   "Trail" NVARCHAR2(200) NOT NULL,
   "EventEventId" NUMBER(10) NOT NULL
);

-- Creating table 'Teams'
CREATE TABLE "SKI"."Teams" (
   "TeamId" NUMBER(10) NOT NULL,
   "FirstName" NVARCHAR2(50) NOT NULL,
   "LastName" NVARCHAR2(50) NOT NULL,
   "City" NCHAR(30) NOT NULL,
   "Country" NCHAR(30) NOT NULL,
   "DougCount" NUMBER(10) NOT NULL,
   "RaceRaceId" NUMBER(10) NOT NULL
);

-- Creating table 'Teams_Mushing'
CREATE TABLE "SKI"."Teams_Mushing" (
   "Leader1" NVARCHAR2(20) NOT NULL,
   "Leader2" NVARCHAR2(20) NOT NULL,
   "TeamId" NUMBER(10) NOT NULL
);

-- Creating table 'Teams_Skijoring'
CREATE TABLE "SKI"."Teams_Skijoring" (
   "Dog1" NVARCHAR2(20) NOT NULL,
   "Dog2" NVARCHAR2(20) NOT NULL,
   "Dog3" NVARCHAR2(20) NOT NULL,
   "NumberOfLeaders" NUMBER(10) NOT NULL,
   "TeamId" NUMBER(10) NOT NULL
);


-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on "EventId"in table 'Events'
ALTER TABLE "SKI"."Events"
ADD CONSTRAINT "PK_Events"
   PRIMARY KEY ("EventId" )
   ENABLE
   VALIDATE;


-- Creating primary key on "RaceId"in table 'Races'
ALTER TABLE "SKI"."Races"
ADD CONSTRAINT "PK_Races"
   PRIMARY KEY ("RaceId" )
   ENABLE
   VALIDATE;


-- Creating primary key on "TeamId"in table 'Teams'
ALTER TABLE "SKI"."Teams"
ADD CONSTRAINT "PK_Teams"
   PRIMARY KEY ("TeamId" )
   ENABLE
   VALIDATE;


-- Creating primary key on "TeamId"in table 'Teams_Mushing'
ALTER TABLE "SKI"."Teams_Mushing"
ADD CONSTRAINT "PK_Teams_Mushing"
   PRIMARY KEY ("TeamId" )
   ENABLE
   VALIDATE;


-- Creating primary key on "TeamId"in table 'Teams_Skijoring'
ALTER TABLE "SKI"."Teams_Skijoring"
ADD CONSTRAINT "PK_Teams_Skijoring"
   PRIMARY KEY ("TeamId" )
   ENABLE
   VALIDATE;


-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on "EventEventId" in table 'Races'
ALTER TABLE "SKI"."Races"
ADD CONSTRAINT "FK_EventRace"
   FOREIGN KEY ("EventEventId")
   REFERENCES "SKI"."Events"
       ("EventId")
   ENABLE
   VALIDATE;

-- Creating index for FOREIGN KEY 'FK_EventRace'
CREATE INDEX "IX_FK_EventRace"
ON "SKI"."Races"
   ("EventEventId");

-- Creating foreign key on "RaceRaceId" in table 'Teams'
ALTER TABLE "SKI"."Teams"
ADD CONSTRAINT "FK_RaceTeam"
   FOREIGN KEY ("RaceRaceId")
   REFERENCES "SKI"."Races"
       ("RaceId")
   ENABLE
   VALIDATE;

-- Creating index for FOREIGN KEY 'FK_RaceTeam'
CREATE INDEX "IX_FK_RaceTeam"
ON "SKI"."Teams"
   ("RaceRaceId");

-- Creating foreign key on "TeamId" in table 'Teams_Mushing'
ALTER TABLE "SKI"."Teams_Mushing"
ADD CONSTRAINT "FK_Mushing_inherits_Team"
   FOREIGN KEY ("TeamId")
   REFERENCES "SKI"."Teams"
       ("TeamId")
   ENABLE
   VALIDATE;

-- Creating foreign key on "TeamId" in table 'Teams_Skijoring'
ALTER TABLE "SKI"."Teams_Skijoring"
ADD CONSTRAINT "FK_Skijoring_inherits_Team"
   FOREIGN KEY ("TeamId")
   REFERENCES "SKI"."Teams"
       ("TeamId")
   ENABLE
   VALIDATE;

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
