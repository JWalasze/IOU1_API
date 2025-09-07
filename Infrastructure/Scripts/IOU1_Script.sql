create table IOU1User (
	Id bigint primary key identity(1,1),
	FirstName varchar not null,
	LastName varchar not null,
	AddDate datetime not null constraint df_add_date default getdate(),
	Login varchar not null,
	Password varchar not null,
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