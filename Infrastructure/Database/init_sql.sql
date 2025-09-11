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

drop table IOU1GroupMembers;
drop table IOU1Group;
drop table IOU1User;

insert into IOU1User (FirstName, LastName, Email, Login, Password)
values ('Alice',   'Johnson', 'alice.johnson@example.com', 'alicej', 'P@ssword1');

insert into IOU1User (FirstName, LastName, Email, Login, Password)
values ('Bob',     'Smith',   'bob.smith@example.com',     'bobsmith', 'Secr3tPwd');

insert into IOU1User (FirstName, LastName, Email, Login, Password)
values ('Charlie', 'Brown',   'charlie.brown@example.com', 'cbrown', 'Ch@rlie123');

insert into IOU1User (FirstName, LastName, Email, Login, Password)
values ('Diana',   'Evans',   'diana.evans@example.com',   'dianae', 'D!anaPW2024');

insert into IOU1Group (CreationUserId, Description)
values (1, 'Group for project Alpha');

insert into IOU1Group (CreationUserId, Description)
values (2, 'Testers group for QA');

insert into IOU1Group (CreationUserId, Description)
values (3, 'Finance department collaboration');

insert into IOU1Group (CreationUserId, Description)
values (4, 'Casual chat group');

insert into IOU1GroupMembers (MemberUserId, IOU1Group) values (1, 1);
insert into IOU1GroupMembers (MemberUserId, IOU1Group) values (2, 1);
insert into IOU1GroupMembers (MemberUserId, IOU1Group) values (3, 1);
insert into IOU1GroupMembers (MemberUserId, IOU1Group) values (4, 1);

insert into IOU1GroupMembers (MemberUserId, IOU1Group) values (2, 2);
insert into IOU1GroupMembers (MemberUserId, IOU1Group) values (3, 2);

insert into IOU1GroupMembers (MemberUserId, IOU1Group) values (3, 3);
insert into IOU1GroupMembers (MemberUserId, IOU1Group) values (1, 3);

insert into IOU1GroupMembers (MemberUserId, IOU1Group) values (4, 4);
insert into IOU1GroupMembers (MemberUserId, IOU1Group) values (2, 4);

