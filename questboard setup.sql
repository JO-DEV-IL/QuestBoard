CREATE TABLE users(
	userID int IDENTITY(1,1) PRIMARY KEY,
	username varchar(20),
	firstName varchar(20),
	lastName varchar(20),
	age int,
	class varchar(20),
	level int,
	role varchar(10),
	password varchar(20),
	is_loggedin bit
)
select * from users
DROP TABLE users

CREATE TABLE user_quests(
	username varchar(20),
	todoTitle varchar(20),
	todoDescription varchar(50)
)
select * from user_quests
drop table user_quests

CREATE TABLE user_equipment(
	userID int,
	head varchar(20),
	chest varchar(20),
	arms varchar(20),
	legs varchar(20),
	feet varchar(20),
	mainhand varchar(20),
	offhand varchar(20),
	accessory1 varchar(20),
	accessory2 varchar(20)
)
select * from user_equipment
drop table user_equipment

CREATE TABLE user_inventory(
	userID int,
	itemName varchar(20),
	itemType varchar(20),
	itemQuantity int,
	description varchar(max),
	image_path varchar(20)
)

INSERT INTO user_inventory(userID,itemName,itemType,itemQuantity,description,image_path) values(1,'Gold','Resource',1,'A pile of gold.','/images/01.png')
INSERT INTO user_inventory(userID,itemName,itemType,itemQuantity,description,image_path) values(1,'Battered Chest','Miscellaneous',1,'An old, weathered chest.','/images/02.png')
INSERT INTO user_inventory(userID,itemName,itemType,itemQuantity,description,image_path) values(1,'Rusty Key','Miscellaneous',1,'It looks like it could still open something...','/images/03.png')
INSERT INTO user_inventory(userID,itemName,itemType,itemQuantity,description,image_path) VALUES(1,'Prototype Helmet','Armor',1,'A helmet for testing.','/images/proto01.png')
INSERT INTO user_inventory(userID,itemName,itemType,itemQuantity,description,image_path) VALUES(1,'Prototype Chest','Armor',1,'A chestpiece for testing.','/images/proto02.png')
INSERT INTO user_inventory(userID,itemName,itemType,itemQuantity,description,image_path) VALUES(1,'Prototype Arms','Armor',1,'Arm guards for testing.','/images/proto03.png')
INSERT INTO user_inventory(userID,itemName,itemType,itemQuantity,description,image_path) VALUES(1,'Prototype Legs','Armor',1,'Leg guards for testing.','/images/proto04.png')
INSERT INTO user_inventory(userID,itemName,itemType,itemQuantity,description,image_path) VALUES(1,'Prototype Boots','Armor',1,'Boots for testing.','/images/proto05.png')
INSERT INTO user_inventory(userID,itemName,itemType,itemQuantity,description,image_path) VALUES(1,'Prototype Mainhand','Weapon',1,'A mainhand weapon for testing.','/images/proto06.png')
INSERT INTO user_inventory(userID,itemName,itemType,itemQuantity,description,image_path) VALUES(1,'Prototype Offhand','Weapon',1,'An offhand for testing.','/images/proto07.png')
INSERT INTO user_inventory(userID,itemName,itemType,itemQuantity,description,image_path) VALUES(1,'Prototype Accessory','Armor',1,'A helmet for testing.','/images/proto08.png')

select * from user_inventory
drop table user_inventory

CREATE TABLE Master_Armory(
	id int IDENTITY(1,1) PRIMARY KEY,
	name varchar(30),
	slot varchar(20),
	type varchar(20),
	description varchar(max),
	image varchar(20),
)

select * from Master_Armory
drop table Master_Armory

INSERT INTO Master_Armory(name,slot,type,description,image) VALUES('Prototype Helmet','Head','Armor','A helmet for testing.','/images/proto01.png')
INSERT INTO Master_Armory(name,slot,type,description,image) VALUES('Prototype Chest','Chest','Armor','A chestpiece for testing.','/images/proto02.png')
INSERT INTO Master_Armory(name,slot,type,description,image) VALUES('Prototype Arms','Arms','Armor','Arm guards for testing.','/images/proto03.png')
INSERT INTO Master_Armory(name,slot,type,description,image) VALUES('Prototype Legs','Legs','Armor','Leg guards for testing.','/images/proto04.png')
INSERT INTO Master_Armory(name,slot,type,description,image) VALUES('Prototype Boots','Feet','Armor','Boots for testing.','/images/proto05.png')
INSERT INTO Master_Armory(name,slot,type,description,image) VALUES('Prototype Mainhand','Mainhand','Weapon','A mainhand weapon for testing.','/images/proto06.png')
INSERT INTO Master_Armory(name,slot,type,description,image) VALUES('Prototype Offhand','Offhand','Weapon','An offhand for testing.','/images/proto07.png')
INSERT INTO Master_Armory(name,slot,type,description,image) VALUES('Prototype Accessory','Accessory1','Armor','A helmet for testing.','/images/proto08.png')

-- Will join inventory items with userID of user list
select i.itemName, i.itemQuantity, i.description, i.image_path
from user_inventory i
join users u
on u.userID = i.userID and i.userID = 2

-- Join users with their quests
select q.username,q.todoTitle,q.todoDescription
from [questboard_app].[dbo].[user_quests] q
join [questboard_app].[dbo].[users] u
on u.username = q.username
where q.username = @username

CREATE TABLE user_stats(
	userID int,
	level int,
	class varchar(20),
	hp int,
	power int,
	defense int,
	luck int,
	avatar_path varchar(20)
)

select * from user_stats
drop table user_stats

INSERT INTO user_stats(userID,level,class,hp,power,defense,luck,avatar_path) VALUES(1,1,'Warrior',50,30,25,30,'/images/avatar01.png')
INSERT INTO user_stats(userID,level,class,hp,power,defense,luck,avatar_path) VALUES(2,1,'Rogue',30,50,15,30,'/images/avatar02.png')

-- match userID to user stats
select s.userID,u.username,s.level,s.class,s.hp,s.power,s.defense,s.luck,s.avatar_path from user_stats s join users u on u.userID = s.userID where u.username = 'jo-dev-il'

-- Randomized chest loot
CREATE TABLE chest_loot(
	lootID int IDENTITY(1,1) PRIMARY KEY,
	lootName varchar(20),
	lootType varchar(20),
	lootQuantity int,
	lootDescription varchar(max),
	image_path varchar(20)
)

insert into chest_loot(lootName,lootType,lootQuantity,lootDescription,image_path) values ('Gold','Resource',1,'A pile of gold.','/images/01.png')
insert into chest_loot(lootName,lootType,lootQuantity,lootDescription,image_path) values ('Rusty Key','Miscellaneous',1,'It looks like it could still open something...','/images/03.png')
insert into chest_loot(lootName,lootType,lootQuantity,lootDescription,image_path) values ('Battered Chest','Miscellaneous',1,'An old, weathered chest. The lock is intact.','/images/02.png')
INSERT INTO chest_loot(lootName,lootType,lootQuantity,lootDescription,image_path) VALUES ('Lost Helmet','Armor',1,'A strong, albeit eroded, helmet.','/images/04.png')
INSERT INTO chest_loot(lootName,lootType,lootQuantity,lootDescription,image_path) VALUES ('Cheese','Resource',1,'Its just a piece of cheese.','/images/05.png')

select * from chest_loot
drop table chest_loot