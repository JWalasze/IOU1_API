create table IOU1User (
	Id bigint primary key identity(1,1),
	FirstName varchar(20) not null,
	LastName varchar(20) not null,
	Email varchar(30) not null,
	AddDate datetime not null constraint df_add_date default getdate(),
	Login varchar(30) not null,
	Password varchar(30) not null,
	Version rowversion
);

create table IOU1Group (
	Id bigint primary key identity(1,1),
	CreationUser bigint not null,
	Description text,
	Version rowversion

	constraint fk_creation_user_iou1_user foreign key (CreationUser)
	references IOU1User(Id)
);

create table IOU1GroupMembers (
	Id bigint primary key identity(1,1),
	MemberUser bigint not null,
	IOU1Group bigint not null,

	constraint fk_member_user_iou1_user foreign key (MemberUser)
	references IOU1User(Id),

	constraint fk_iou1_group_iou1_group foreign key (IOU1Group)
	references IOU1Group(Id)
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

