-- =============================================================================
--  Konwencja nazewnicza:
--    PK_Tabela
--    FK_TabelaPodrzedna_TabelaNadrzedna_Kolumna
--    UQ_Tabela_Kolumna
--    CK_Tabela_Kolumna_Opis
--    DF_Tabela_Kolumna
--    IX_Tabela_Kolumny
-- =============================================================================

drop table if exists ExpenseShare;
drop table if exists Settlement;
drop table if exists Expense;
drop table if exists InvitationLink;
drop table if exists Invitation;
drop table if exists MemberBalance;
drop table if exists GroupMember;
drop table if exists ExpenseCategory;
drop table if exists CommunityGroup;
drop table if exists Currency;
drop table if exists AppUser;
go

create table AppUser (
  Id            int             identity(1, 1) not null,
  FirstName     nvarchar(50)    not null,
  LastName      nvarchar(50)    not null,
  Email         nvarchar(256)   not null,

  Login         nvarchar(30)    not null,
  PasswordHash  varbinary(64)   not null,
  PasswordSalt  varbinary(32)   not null,

  AddDate       datetime2(3)    not null constraint DF_AppUser_AddDate   default sysutcdatetime(),
  IsDeleted     bit             not null constraint DF_AppUser_IsDeleted default 0,
  Version       rowversion,

  constraint PK_AppUser        primary key (Id),
  constraint UQ_AppUser_Email  unique (Email),
  constraint UQ_AppUser_Login  unique (Login)
);
go

create table Currency (
  CurrencyKey  nchar(3)  not null,
  Version      rowversion,

  constraint PK_Currency primary key (CurrencyKey)
);
go

insert into Currency (CurrencyKey) values ('PLN'), ('EUR'), ('USD'), ('GBP');
go

create table CommunityGroup (
  Id           int            identity(1, 1) not null,
  Name         nvarchar(50)   not null,
  Description  nvarchar(max)  null,
  CurrencyKey  nchar(3)       not null,

  CreatedById  int            not null,
  AddDate      datetime2(3)   not null constraint DF_CommunityGroup_AddDate default sysutcdatetime(),
  Version      rowversion,

  constraint PK_CommunityGroup                      primary key (Id),
  constraint FK_CommunityGroup_AppUser_CreatedById  foreign key (CreatedById) references AppUser (Id),
  constraint FK_CommunityGroup_Currency_CurrencyKey foreign key (CurrencyKey) references Currency (CurrencyKey)
);
go

create index IX_CommunityGroup_CreatedById on CommunityGroup (CreatedById);
create index IX_CommunityGroup_CurrencyKey on CommunityGroup (CurrencyKey);
go

create table GroupMember (
  Id       int  identity(1, 1) not null,
  UserId   int  not null,
  GroupId  int  not null,
  Version  rowversion,

  constraint PK_GroupMember                        primary key (Id),
  constraint FK_GroupMember_AppUser_UserId         foreign key (UserId)  references AppUser (Id),
  constraint FK_GroupMember_CommunityGroup_GroupId foreign key (GroupId) references CommunityGroup (Id),
  constraint UQ_GroupMember_GroupId_UserId         unique (GroupId, UserId)
);
go

create index IX_GroupMember_UserId on GroupMember (UserId);
create index IX_GroupMember_GroupId on GroupMember (GroupId);
go

create table Settlement (
  Id            int             identity(1, 1) not null,
  GroupId       int             not null,
  Amount        decimal(10, 2)  not null,

  FromMemberId  int             not null,
  ToMemberId    int             not null,

  SettledAt     datetime2(3)    not null constraint DF_Settlement_SettledAt default sysutcdatetime(),
  Version       rowversion,

  constraint PK_Settlement                          primary key (Id),
  constraint FK_Settlement_CommunityGroup_GroupId   foreign key (GroupId)      references CommunityGroup (Id),
  constraint FK_Settlement_GroupMember_FromMemberId foreign key (FromMemberId) references GroupMember (Id),
  constraint FK_Settlement_GroupMember_ToMemberId   foreign key (ToMemberId)   references GroupMember (Id),
  constraint CK_Settlement_Amount_Positive          check (Amount > 0),
  constraint CK_Settlement_Members_Different        check (FromMemberId <> ToMemberId)
);
go

create index IX_Settlement_GroupId_FromMemberId on Settlement (GroupId, FromMemberId) include (Amount);
create index IX_Settlement_GroupId_ToMemberId   on Settlement (GroupId, ToMemberId)   include (Amount);
go

create table Invitation (
  Id                int           identity(1, 1) not null,
  GroupId           int           not null,
  SenderId          int           not null,
  UserId            int           not null,

  InvitationStatus  varchar(32)   not null constraint DF_Invitation_InvitationStatus default 'PENDING',
  AddDate           datetime2(3)  not null constraint DF_Invitation_AddDate          default sysutcdatetime(),
  Version           rowversion,

  constraint PK_Invitation                            primary key (Id),
  constraint FK_Invitation_CommunityGroup_GroupId     foreign key (GroupId)  references CommunityGroup (Id),
  constraint FK_Invitation_AppUser_SenderId           foreign key (SenderId) references AppUser (Id),
  constraint FK_Invitation_AppUser_UserId             foreign key (UserId)   references AppUser (Id),
  constraint CK_Invitation_InvitationStatus_Allowed   check (InvitationStatus in ('PENDING', 'ACCEPTED', 'REJECTED', 'EXPIRED'))
);
go

create index IX_Invitation_GroupId  on Invitation (GroupId);
create index IX_Invitation_UserId   on Invitation (UserId);
create index IX_Invitation_SenderId on Invitation (SenderId);
go

create table InvitationLink (
  Id              int            identity(1, 1) not null,
  GroupId         int            not null,

  AddDate         datetime2(3)   not null constraint DF_InvitationLink_AddDate default sysutcdatetime(),
  ExpirationDate  datetime2(3)   not null,
  InvitationKey   nvarchar(30)   not null,
  Version         rowversion,

  constraint PK_InvitationLink                          primary key (Id),
  constraint FK_InvitationLink_CommunityGroup_GroupId   foreign key (GroupId) references CommunityGroup (Id),
  constraint UQ_InvitationLink_InvitationKey            unique (InvitationKey),
  constraint CK_InvitationLink_ExpirationDate_After_Add check (ExpirationDate > AddDate)
);
go

create index IX_InvitationLink_GroupId on InvitationLink (GroupId);
go

create table ExpenseCategory (
  Id            int             identity(1, 1) not null,
  Title         nvarchar(20)    not null,
  Description   nvarchar(255)   null,
  GroupId       int             null,

  CreatedAt     datetime2(3)    not null constraint DF_ExpenseCategory_CreatedAt default sysutcdatetime(),
  IsDeleted     bit             not null constraint DF_ExpenseCategory_IsDeleted default 0,
  Version       rowversion,

  constraint PK_ExpenseCategory                        primary key (Id),
  constraint FK_ExpenseCategory_CommunityGroup_GroupId foreign key (GroupId) references CommunityGroup (Id)
);
go

create index IX_ExpenseCategory_CommunityGroup_GroupId on ExpenseCategory (GroupId);
go

create table ExpenseSplit (
  Id            int             identity(1, 1) not null,
  Title         nvarchar(20)    not null,
  Description   nvarchar(255)   null,
  
  CreatedAt     datetime2(3)    not null constraint DF_ExpenseSplit_CreatedAt default sysutcdatetime(),
  IsDeleted     bit             not null constraint DF_ExpenseSplit_IsDeleted default 0,
  Version       rowversion,

  constraint PK_ExpenseSplit    primary key (Id)
);
go

create table Expense (
  Id            int             identity(1, 1) not null,
  GroupId       int             not null,
  PayerId       int             not null,

  Amount        decimal(10, 2)  not null,
  Title         nvarchar(50)    not null,
  Description   nvarchar(255)   null,
  CategoryId    int             not null,

  SplitMethodId int             not null,
  IsSettled     bit             not null constraint DF_Expense_IsSettled default 0,

  CreatedAt     datetime2(3)    not null constraint DF_Expense_CreatedAt default sysutcdatetime(),
  IsDeleted     bit             not null constraint DF_Expense_IsDeleted default 0,
  Version       rowversion,

  constraint PK_Expense                            primary key (Id),
  constraint FK_Expense_CommunityGroup_GroupId     foreign key (GroupId) references CommunityGroup (Id),
  constraint FK_Expense_GroupMember_PayerId        foreign key (PayerId) references GroupMember (Id),
  constraint FK_Expense_ExpenseCategory_CategoryId foreign key (CategoryId) references ExpenseCategory (Id),
  constraint FK_Expense_ExpenseSplit_SplitMethodId foreign key (SplitMethodId) references ExpenseSplit (Id),
  constraint CK_Expense_Amount_Positive            check (Amount > 0)
);
go

create index IX_Expense_GroupId_PayerId   on Expense (GroupId, PayerId) include (Amount);
create index IX_Expense_GroupId_CreatedAt on Expense (GroupId, CreatedAt);
go

create table ExpenseShare (
  Id           int             identity(1, 1) not null,
  ExpenseId    int             not null,
  MemberId     int             not null,

  Amount       decimal(10, 2)  not null,
  Version      rowversion,

  constraint PK_ExpenseShare                       primary key (Id),
  constraint FK_ExpenseShare_Expense_ExpenseId     foreign key (ExpenseId) references Expense (Id),
  constraint FK_ExpenseShare_GroupMember_MemberId  foreign key (MemberId)  references GroupMember (Id),
  constraint UQ_ExpenseShare_ExpenseId_MemberId    unique (ExpenseId, MemberId),
  constraint CK_ExpenseShare_Amount_NonNegative    check (Amount >= 0)
);
go

create index IX_ExpenseShare_MemberId on ExpenseShare (MemberId) include (Amount);
go

create table MemberBalance (
  Id                    int           identity(1, 1) not null,

  MemberId              int           not null,
  CounterpartyMemberId  int           not null,
  Amount                decimal(10,2) not null,

  UpdatedAt             datetime2(3)  not null constraint DF_MemberBalance_UpdatedAt default sysutcdatetime(),
  Version               rowversion,

  constraint PK_MemberBalance                                   primary key (Id),
  constraint FK_MemberBalance_GroupMember_MemberId              foreign key (MemberId)                  references GroupMember (Id),
  constraint FK_MemberBalance_GroupMember_CounterpartyMemberId  foreign key (CounterpartyMemberId)      references GroupMember (Id),
  constraint UQ_MemberBalance_MemberId_CounterpartyMemberId     unique (MemberId, CounterpartyMemberId),
  constraint CK_MemberBalance_Members_Different                 check (MemberId <> CounterpartyMemberId)
);
go

create index IX_MemberBalance_MemberId on MemberBalance (MemberId);
create index IX_MemberBalance_CounterpartyMemberId on MemberBalance (CounterpartyMemberId);
go