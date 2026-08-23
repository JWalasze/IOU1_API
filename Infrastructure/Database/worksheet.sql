select * from Expense;
select * from memberbalance;

alter table ExpenseShare drop constraint CK_ExpenseShare_Amount_NonNegative;


select * from Groupmember where groupid = 1;
select * from CommunityGroup;
select * from AppUser;

alter table memberbalance add GroupId int;