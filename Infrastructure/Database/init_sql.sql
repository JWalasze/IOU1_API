drop table if exists GroupTransaction;

drop table if exists GroupExpense;

drop table if exists TransactionStatus;

drop table if exists Currency;

drop table if exists GroupMember;

drop table if exists GroupTransaction;

drop table if exists InvitationLink;

drop table if exists Invitation;

drop table if exists CommunityGroup;

drop table if exists AppUser;

create table AppUser (
  Id bigint primary key identity(1, 1),
  FirstName varchar(20) not null,
  LastName varchar(20) not null,
  Email varchar(30) not null,
  AddDate datetime not null constraint df_add_date_app_user default getdate(),
  Login varchar(30) not null,
  PasswordHash varchar(100) not null,
  PasswordSalt varchar(100) not null,
  IsDeleted bit not null default (0),
  Version rowversion
);

create table CommunityGroup (
  Id bigint primary key identity(1, 1),
  CreatedById bigint not null,
  Name varchar(50) not null,
  Description text null,
  Version rowversion,

  constraint fk_created_by_id_community_group foreign key (CreatedById) references AppUser (Id)
);

create table GroupMember (
  Id bigint primary key identity(1, 1),
  UserId bigint not null,
  GroupId bigint not null,

  constraint fk_member_id_group_member foreign key (MemberId) references AppUser (Id),
  constraint fk_group_id_group_member foreign key (GroupId) references CommunityGroup (Id)
);

create table GroupMemberDebt (
  Id bigint primary key identity(1, 1),
  MemberId bigint not null,
  DebtorId bigint not null,
  GroupId bigint not null,
  Balance decimal not null,

  constraint fk_member_id_gm_debt foreign key (MemberId) references GroupMember (Id),
  constraint fk_debtor_id_gm_debt foreign key (DebtorId) references GroupMember (Id),
  constraint fk_group_id_gm_debt foreign key (GroupId) references CommunityGroup (Id)
);

create table Invitation(
  Id bigint primary key identity(1, 1),
  GroupId bigint not null,
  SenderId bigint not null,
  UserId bigint not null,
  InvitationStatus varchar(32),

  constraint fk_group_id_invitation foreign key (GroupId) references CommunityGroup (Id),
  constraint fk_user_id_invitation foreign key (UserId) references CommunityGroup (Id)
);

create table Currency (
  Id bigint identity(1, 1) not null,
  CurrencyKey nvarchar(3) primary key not null,
);

create table GroupExpense (
  Id bigint primary key identity(1, 1),
  GroupId bigint not null,
  BuyerId bigint not null,
  TotalAmount decimal(10, 2) not null,
  Title nvarchar(50) not null,
  Description nvarchar(255) null,
  CurrencyKey nvarchar(3) not null,
  CreatedAt date not null,
  Version rowversion,
  constraint fk_group_id_group_expense foreign key (GroupId) references CommunityGroup (Id),
  constraint fk_buyer_id_group_expense foreign key (BuyerId) references AppUser (Id),
  constraint fk_currency_key_group_expense foreign key (CurrencyKey) references Currency (CurrencyKey)
);

create table GroupTransaction (
  Id bigint primary key identity(1, 1),
  ExpenseId bigint not null,
  GroupId bigint not null,
  BuyerId bigint not null,
  BorrowerId bigint not null,
  AddDate datetime not null constraint df_add_date_group_transaction default getdate(),
  Amount decimal(10, 2) not null,
  CurrencyKey nvarchar(3) not null,
  BuyerMember bigint not null,
  BorrowerMember bigint not null,
  Version rowversion,
  constraint fk_expense_id_group_transaction foreign key (ExpenseId) references GroupExpense (Id),
  constraint fk_group_id_group_transaction foreign key (GroupId) references CommunityGroup (Id),
  constraint fk_buyer_id_group_transaction foreign key (BuyerId) references AppUser (Id),
  constraint fk_borrower_id_group_transaction foreign key (BorrowerId) references AppUser (Id),
  constraint fk_currency_key_group_transaction foreign key (CurrencyKey) references Currency (CurrencyKey),
  constraint fk_borrower_mem_group_transaction foreign key (BorrowerMember) references GroupMember (Id),
  constraint fk_buyer_mem_group_transaction foreign key (BuyerMember) references GroupMember (Id)
);

create table InvitationLink (
  Id bigint primary key identity(1, 1),
  GroupId bigint not null,
  AddDate datetime not null constraint df_add_date_invitation_link default getdate(),
  ExpirationDate datetime not null,
  InvitationKey nvarchar(30) not null,
  Version rowversion,

  constraint fk_group_id_invitation_link foreign key (GroupId) references CommunityGroup (Id)
);


insert into
  Currency (CurrencyKey)
values
  ('PLN');

-- Id = 1
insert into
  Currency (CurrencyKey)
values
  ('EUR');

-- Id = 2
insert into
  Currency (CurrencyKey)
values
  ('USD');

-- Id = 3
insert into
  Currency (CurrencyKey)
values
  ('GBP');