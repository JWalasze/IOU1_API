create table AppUser (
	Id bigint primary key identity(1,1),
	FirstName varchar(20) not null,
	LastName varchar(20) not null,
	Email varchar(30) not null,
	AddDate datetime not null constraint df_add_date default getdate(),
	Login varchar(30) not null,
	Password varchar(30) not null,
	Version rowversion
);

create table CommunityGroup (
	Id bigint primary key identity(1,1),
	CreatedById bigint not null,
	Description text,
	Version rowversion

	constraint fk_created_by_id_app_user foreign key (CreatedById)
	references AppUser(Id)
);

create table GroupMembers (
	Id bigint primary key identity(1,1),
	MemberId bigint not null,
	GroupId bigint not null,

	constraint fk_member_id_app_user foreign key (MemberId)
	references AppUser(Id),

	constraint fk_group_id_community_group foreign key (GroupId)
	references CommunityGroup(Id)
);

create table Transactions (
	Id bigint primary key identity(1,1),
	GroupId bigint not null,
	Version rowversion
);


drop table GroupMembers;
drop table CommunityGroup;
drop table AppUser;

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
insert into GroupMembers (MemberId, GroupId) values (1, 1);
insert into GroupMembers (MemberId, GroupId) values (2, 1);
insert into GroupMembers (MemberId, GroupId) values (3, 1);
insert into GroupMembers (MemberId, GroupId) values (4, 1);

-- Group 2 (QA): Bob + Charlie
insert into GroupMembers (MemberId, GroupId) values (2, 2);
insert into GroupMembers (MemberId, GroupId) values (3, 2);

-- Group 3 (Finance): Charlie + Alice
insert into GroupMembers (MemberId, GroupId) values (3, 3);
insert into GroupMembers (MemberId, GroupId) values (1, 3);

-- Group 4 (Casual chat): Diana + Bob
insert into GroupMembers (MemberId, GroupId) values (4, 4);
insert into GroupMembers (MemberId, GroupId) values (2, 4);


