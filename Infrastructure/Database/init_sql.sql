drop table if exists GroupMember;
drop table if exists GroupTransaction;
drop table if exists CommunityGroup;
drop table if exists AppUser;
drop table if exists Currency;
drop table if exists TransactionStatus;

create table AppUser (
	Id bigint primary key identity(1,1),
	FirstName varchar(20) not null,
	LastName varchar(20) not null,
	Email varchar(30) not null,
	AddDate datetime not null constraint df_add_date_app_user default getdate(),
	Login varchar(30) not null,
	Password varchar(30) not null,
	Version rowversion
);

create table CommunityGroup (
	Id bigint primary key identity(1,1),
	CreatedById bigint not null,
	Description text,
	Version rowversion

	constraint fk_created_by_id_community_group foreign key (CreatedById)
	references AppUser(Id)
);

create table GroupMember (
	Id bigint primary key identity(1,1),
	MemberId bigint not null,
	GroupId bigint not null,

	constraint fk_member_id_group_member foreign key (MemberId)
	references AppUser(Id),

	constraint fk_group_id_group_member foreign key (GroupId)
	references CommunityGroup(Id)
);

create table Currency (
	Id bigint primary key identity(1,1),
	Name varchar(10) not null,
);

create table TransactionStatus (
	Id bigint primary key identity(1,1),
	Name varchar(10) not null,
);

create table GroupExpense (
	Id bigint primary key identity(1,1),
	GroupId bigint not null,
	BuyerId bigint not null,
	TotalAmount decimal(10,2) not null,
	Title nvarchar(50) not null,
	Description nvarchar(255) null,
	CurrencyId bigint not null,
	Version rowversion,

	constraint fk_group_id_group_expense foreign key (GroupId)
	references CommunityGroup(Id),

	constraint fk_buyer_id_group_expense foreign key (BuyerId)
	references AppUser(Id),

	constraint fk_currency_id_group_expense foreign key (CurrencyId)
	references Currency(Id)
);

create table GroupTransaction (
	Id bigint primary key identity(1,1),
	ExpenseId bigint not null,
	GroupId bigint not null,
	BuyerId bigint not null,
	BorrowerId bigint not null,
	AddDate datetime not null constraint df_add_date_group_transaction default getdate(),
	ModificationDate datetime null,
	Amount decimal(10,2) not null,
	CurrencyId bigint not null,
	StatusId bigint not null,
	Version rowversion,

	constraint fk_expense_id_group_transaction foreign key (ExpenseId)
	references GroupExpense(Id),

	constraint fk_group_id_group_transaction foreign key (GroupId)
	references CommunityGroup(Id),

	constraint fk_buyer_id_group_transaction foreign key (BuyerId)
	references AppUser(Id),

	constraint fk_borrower_id_group_transaction foreign key (BorrowerId)
	references AppUser(Id),

	constraint fk_currency_id_group_transaction foreign key (CurrencyId)
	references Currency(Id),

	constraint fk_status_id_group_transaction foreign key (StatusId)
	references TransactionStatus(Id)
);

insert into AppUser (FirstName, LastName, Email, Login, Password)
values ('Alice',   'Johnson', 'alice.johnson@example.com', 'alicej',  'P@ssword1');

insert into AppUser (FirstName, LastName, Email, Login, Password)
values ('Bob',     'Smith',   'bob.smith@example.com',     'bobsmith','Secr3tPwd');

insert into AppUser (FirstName, LastName, Email, Login, Password)
values ('Charlie', 'Brown',   'charlie.brown@example.com', 'cbrown',  'Ch@rlie123');

insert into AppUser (FirstName, LastName, Email, Login, Password)
values ('Diana',   'Evans',   'diana.evans@example.com',   'dianae',  'D!anaPW2024');

insert into CommunityGroup (CreatedById, Description)
values (1, 'Group for project Alpha');

insert into CommunityGroup (CreatedById, Description)
values (2, 'Testers group for QA');

insert into CommunityGroup (CreatedById, Description)
values (3, 'Finance department collaboration');

insert into CommunityGroup (CreatedById, Description)
values (4, 'Casual chat group');

-- Group 1 (Alpha): everyone joins
insert into GroupMember (MemberId, GroupId) values (1, 1);
insert into GroupMember (MemberId, GroupId) values (2, 1);
insert into GroupMember (MemberId, GroupId) values (3, 1);
insert into GroupMember (MemberId, GroupId) values (4, 1);

-- Group 2 (QA): Bob + Charlie
insert into GroupMember (MemberId, GroupId) values (2, 2);
insert into GroupMember (MemberId, GroupId) values (3, 2);

-- Group 3 (Finance): Charlie + Alice
insert into GroupMember (MemberId, GroupId) values (3, 3);
insert into GroupMember (MemberId, GroupId) values (1, 3);

-- Group 4 (Casual chat): Diana + Bob
insert into GroupMember (MemberId, GroupId) values (4, 4);
insert into GroupMember (MemberId, GroupId) values (2, 4);

-- 1) Lookup tables
insert into Currency (Name) values ('PLN');  -- Id = 1
insert into Currency (Name) values ('EUR');  -- Id = 2
insert into Currency (Name) values ('USD');  -- Id = 3
insert into Currency (Name) values ('GBP');  -- Id = 4

insert into TransactionStatus (Name) values ('Pending');   -- Id = 1
insert into TransactionStatus (Name) values ('Paid');      -- Id = 2
insert into TransactionStatus (Name) values ('Cancelled'); -- Id = 3
insert into TransactionStatus (Name) values ('Disputed');  -- Id = 4

-- 2) Sample transactions
-- Group 1 (Alpha: members 1,2,3,4)
declare @ExpenseId bigint;

-- Alice paid for others in Group 1
insert into GroupExpense (GroupId, BuyerId, TotalAmount, Title, CurrencyId)
values (1, 1, 120.50, 'Beer', 1); -- Alice total

set @ExpenseId = SCOPE_IDENTITY();

insert into GroupTransaction (ExpenseId, GroupId, BuyerId, BorrowerId, Amount, CurrencyId, StatusId)
values (@ExpenseId, 1, 1, 2, 65.50, 1, 1); -- Alice bought for Bob, PLN, Pending
insert into GroupTransaction (ExpenseId, GroupId, BuyerId, BorrowerId, Amount, CurrencyId, StatusId)
values (@ExpenseId, 1, 1, 1, 55.00, 1, 1); -- Alice bought for herself, PLN, Pending

-- Bob paid for Charlie in Group 1
insert into GroupExpense (GroupId, BuyerId, TotalAmount, Title, CurrencyId)
values (1, 2, 45.00, 'Beer', 3);

set @ExpenseId = SCOPE_IDENTITY();

insert into GroupTransaction (ExpenseId, GroupId, BuyerId, BorrowerId, Amount, CurrencyId, StatusId)
values (@ExpenseId, 1, 2, 3, 45.00, 3, 2);

-- Diana paid for Alice in Group 1
insert into GroupExpense (GroupId, BuyerId, TotalAmount, Title, CurrencyId)
values (1, 4, 89.99, 'Beer', 2);

set @ExpenseId = SCOPE_IDENTITY();

insert into GroupTransaction (ExpenseId, GroupId, BuyerId, BorrowerId, Amount, CurrencyId, StatusId)
values (@ExpenseId, 1, 4, 1, 89.99, 2, 4);

-- Charlie paid for Diana in Group 1
insert into GroupExpense (GroupId, BuyerId, TotalAmount, Title, CurrencyId)
values (1, 3, 72.10, 'Beer', 2);

set @ExpenseId = SCOPE_IDENTITY();

insert into GroupTransaction (ExpenseId, GroupId, BuyerId, BorrowerId, Amount, CurrencyId, StatusId)
values (@ExpenseId, 1, 3, 4, 72.10, 2, 1);



