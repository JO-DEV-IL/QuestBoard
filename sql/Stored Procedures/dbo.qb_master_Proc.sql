CREATE procedure [dbo].[qb_master_Proc]
@Option varchar(max) = NULL, --Transaction flag

@user varchar(max) = NULL, --user id

--Register form parameters
@userName varchar(20) = NULL,
@email varchar(20) = NULL,
@password nvarchar(20) = NULL,
@class varchar(20) = NULL,
@stat1 int = NULL,
@stat2 int = NULL,
@stat3 int = NULL,
@stat4 int = NULL,

@itemName varchar(max) = NULL, -- item equipping
@accessory1or2 varchar(1) = NULL --accessory equipping
AS
if @itemName IS NOT NULL
	BEGIN
		DECLARE @item_hp int, @item_power int, @item_defense int, @item_luck int

		SELECT @item_hp = hp_equip,
		       @item_power = power_equip,
		       @item_defense = defense_equip,
		       @item_luck = luck_equip
		FROM [questboard_app].[dbo].[master_Equipment]
		WHERE name = @itemName
	END

if @Option = 'sql_ViewItem'
	BEGIN
		SELECT M.name, M.description, M.rarity, M.image, I.quantity, M.is_equipable, E.type
		FROM [questboard_app].[dbo].[user_Inventory] I
		INNER JOIN [questboard_app].[dbo].[master_Misc_Items] M ON I.itemID = M.id
		INNER JOIN [questboard_app].[dbo].[master_Equipment] E ON I.itemID = E.id
		WHERE M.name = @itemName
		
		UNION ALL
		
		SELECT M.name, M.description, M.rarity, M.image, I.quantity, M.is_equipable, M.type
		FROM [questboard_app].[dbo].[user_Inventory] I
		INNER JOIN [questboard_app].[dbo].[master_Equipment] M ON I.itemID = M.id
		WHERE M.name = @itemName
	END

else if @Option = 'sql_Equip'
	BEGIN
		DECLARE @slotEquip varchar(20) = (SELECT slot FROM master_Equipment WHERE name = @itemName)
		DECLARE @equip_itemID int = (SELECT id FROM master_Equipment WHERE name = @itemName)
		DECLARE @sqlEquip nvarchar(max)

		IF @slotEquip = 'Accessory'
			BEGIN
				SET @sqlEquip = 'UPDATE [questboard_app].[dbo].[user_Equipment] SET accessory' + @accessory1or2 + ' = ' + CONVERT(varchar(10), @equip_itemID) + ' WHERE userID = @user'
				EXEC sp_executesql @sqlEquip, N'@itemName varchar(max), @user varchar(max), @accessory1or2 varchar(1)', @itemName, @user, @accessory1or2
			END
		ELSE
			SET @sqlEquip = 'UPDATE [questboard_app].[dbo].[user_Equipment] SET ' + @slotEquip + ' = ' + CONVERT(varchar(10), @equip_itemID) + ' WHERE userID = @user'
			EXEC sp_executesql @sqlEquip, N'@itemName varchar(max), @user varchar(max)', @itemName, @user

		UPDATE user_Stats 
		SET
			hp = hp + @item_hp,
			power = power + @item_power,
			defense = defense + @item_defense,
			luck = luck + @item_luck
		WHERE userID = @user
	END

else if @Option = 'sql_Unequip'
	BEGIN
		DECLARE @slotUnequip varchar(20) = (SELECT slot FROM master_Equipment WHERE name = @itemName)
		DECLARE @unequip_itemID int = (SELECT id FROM master_Equipment WHERE name = @itemName)
		DECLARE @sqlUnequip nvarchar(max)
		
		IF @slotUnequip = 'Accessory'
			BEGIN
				SET @sqlUnequip = 'UPDATE [questboard_app].[dbo].[user_Equipment] SET accessory' + @accessory1or2 + ' = 0 WHERE userID = @user'
				EXEC sp_executesql @sqlUnequip, N'@user varchar(max), @accessory1or2 varchar(1)', @user, @accessory1or2
			END
		ELSE
			BEGIN
				SET @sqlUnequip = 'UPDATE [questboard_app].[dbo].[user_Equipment] SET ' + @slotUnequip + ' = 0 WHERE userID = @user'
				EXEC sp_executesql @sqlUnequip, N'@user varchar(max)', @user
			END

		UPDATE user_Stats 
		SET
			hp = hp - @item_hp,
			power = power - @item_power,
			defense = defense - @item_defense,
			luck = luck - @item_luck
		WHERE userID = @user
	END

else if @Option = 'sql_ViewEquipment'
	BEGIN
		SELECT
			u.userID, --unnecessary to have but easier to keep it than change all the values in vs
			head.name AS head_name, head.image AS head_image,
			shoulders.name AS shoulders_name, shoulders.image AS shoulders_image,
			chest.name AS chest_name, chest.image AS chest_image,
			hands.name AS hands_name, hands.image AS hands_image,
			legs.name AS legs_name, legs.image AS legs_image,
			feet.name AS feet_name, feet.image AS feet_image,
			main.name AS mainhand_name, main.image AS mainhand_image,
			offh.name AS offhand_name, offh.image AS offhand_image,
			a1.name AS accessory1_name, a1.image AS accessory1_image,
			a2.name AS accessory2_name, a2.image AS accessory2_image
		FROM [questboard_app].[dbo].[user_Equipment] u
			LEFT JOIN
			    [questboard_app].[dbo].[master_Equipment] head ON u.head = head.id
			LEFT JOIN
			    [questboard_app].[dbo].[master_Equipment] shoulders ON u.shoulders = shoulders.id
			LEFT JOIN
			    [questboard_app].[dbo].[master_Equipment] chest ON u.chest = chest.id
			LEFT JOIN
			    [questboard_app].[dbo].[master_Equipment] hands ON u.hands = hands.id
			LEFT JOIN
			    [questboard_app].[dbo].[master_Equipment] legs ON u.legs = legs.id
			LEFT JOIN
			    [questboard_app].[dbo].[master_Equipment] feet ON u.feet = feet.id
			LEFT JOIN
			    [questboard_app].[dbo].[master_Equipment] main ON u.mainhand = main.id
			LEFT JOIN
			    [questboard_app].[dbo].[master_Equipment] offh ON u.offhand = offh.id
			LEFT JOIN
			    [questboard_app].[dbo].[master_Equipment] a1 ON u.accessory1 = a1.id
			LEFT JOIN
				[questboard_app].[dbo].[master_Equipment] a2 ON u.accessory2 = a2.id
		WHERE u.userID = @user;
	END

else if @Option = 'sql_ViewStats'
	BEGIN
		select * from user_Stats where userID = @user
	END

else if @Option = 'sql_ItemShop'
	BEGIN
		select * from test_master_Shop_List
	END

else if @Option = 'sql_Register'
	BEGIN
		-- Enter user into master User list
		INSERT INTO [questboard_app].[dbo].[master_Users] (userName, email, password, class, level, is_admin)
		VALUES (@userName, @email, @password, @class, 1, 0)

		-- Set stats for user's ID
		insert into [questboard_app].[dbo].[user_Stats] (avatar, level, class, hp, power, defense, luck) 
		values ('av_bear.png', 1, @class, @stat1, @stat2, @stat3, @stat4)

		-- Set equipment to empty for user's ID
		 INSERT INTO [questboard_app].[dbo].[user_Equipment] (head, shoulders, chest, hands, legs, feet, mainhand, offhand, accessory1, accessory2)
		 VALUES (0,0,0,0,0,0,0,0,0,0)
	END