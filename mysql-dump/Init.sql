USE messaging_db;

-- 1. Create Tables
CREATE TABLE `users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(255) DEFAULT NULL,
  `password` varchar(255) DEFAULT NULL,
  `lastSignIn` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
ALTER TABLE users AUTO_INCREMENT = 0;

CREATE TABLE `messages` (
  `id` int NOT NULL AUTO_INCREMENT,
  `text` varchar(140) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `userId` int DEFAULT NULL,
  `createDate` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
ALTER TABLE messages AUTO_INCREMENT = 0;

CREATE TABLE `followers` (
  `followerUserId` int NOT NULL,
  `followingUserId` int NOT NULL,
  `followingSince` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`followerUserId`,`followingUserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- 2. Create Stored Procedures
DELIMITER $$
CREATE DEFINER=`root`@`%` PROCEDURE `sp_getUser`(
   IN username varchar(255)
)
BEGIN 
   SELECT id, username FROM users 
   WHERE users.username =  username;
END$$
DELIMITER ;
DELIMITER $$
CREATE DEFINER=`root`@`%` PROCEDURE `sp_setSignIn`(
	IN userId int,
	IN signedInTime datetime
)
BEGIN
    START TRANSACTION;
	
    UPDATE users
	SET lastSignIn = signedInTime
    WHERE id = userId;
    
	COMMIT;
END$$
DELIMITER ;
DELIMITER $$
CREATE DEFINER=`root`@`%` PROCEDURE `sp_validateUser`(
   IN username varchar(255),
   IN userPassword varchar(255)
)
BEGIN 
   DECLARE userId int;
   
   CREATE TEMPORARY TABLE userData as
   SELECT id, username FROM users 
   WHERE users.username =  username
   AND users.password = userPassword
   LIMIT 1;
   
   SET userId = (SELECT id FROM userData);
   IF userId IS NOT NULL THEN
		CALL sp_setSignIn(userId, NOW());
   END IF;
   
   SELECT id, username FROM userData;
   
   DROP TEMPORARY TABLE userData;
END$$
DELIMITER ;
DELIMITER $$
CREATE DEFINER=`root`@`%` PROCEDURE `sp_postMessage`(
	IN textMessage NVARCHAR(140),
    IN userId INT
)
BEGIN
	DECLARE insertedId INT;
    START TRANSACTION;
	INSERT INTO messages (text, userId) VALUES (textMessage, userId);
	SET insertedId = (SELECT LAST_INSERT_ID());
    COMMIT;
    
    SELECT m.id, m.text, m.createDate, m.userId, u.username
    FROM messages m
    JOIN users u ON u.id = m.userId
    WHERE m.id = insertedId;
END$$
DELIMITER ;
DELIMITER $$
CREATE DEFINER=`root`@`%` PROCEDURE `sp_addFollower`(
	in followerUserId int,
    in followingUserId int
)
BEGIN
    START TRANSACTION;
	INSERT INTO followers(followerUserId, followingUserId) VALUES (followerUserId, followingUserId)
	ON DUPLICATE KEY UPDATE followerUserId=followerUserId;
	COMMIT;
    
    SELECT followerUserId, followingUserId, followingSince 
    FROM followers f
    WHERE f.followerUserId = followerUserId AND f.followingUserId = followingUserId;
END$$
DELIMITER ;
DELIMITER $$
CREATE DEFINER=`root`@`%` PROCEDURE `sp_getFollowFeed`(
	in userId int
)
BEGIN
	SELECT m.id, m.text, u.username, m.createDate, m.userId 
    FROM messages m
    JOIN followers f on m.userId = f.followingUserId
    JOIN users u on u.id = f.followingUserId
    WHERE f.followerUserId = userId
    ORDER BY m.createDate DESC;
END$$
DELIMITER ;
DELIMITER $$
CREATE DEFINER=`root`@`%` PROCEDURE `sp_getGlobalFeed`()
BEGIN
	SELECT m.id, m.text, u.username, m.createDate, m.userId 
    FROM messages m
    JOIN users u on u.id = m.userId
    ORDER BY m.createDate DESC;
END$$
DELIMITER ;
DELIMITER $$
CREATE DEFINER=`root`@`%` PROCEDURE `sp_getSignedInUsers`(
   IN hoursBack int
)
BEGIN 
   DECLARE fromTime DATETIME;
   SET fromTime = DATE_SUB(NOW(), INTERVAL hoursBack HOUR);
   
   SELECT id, lastSignIn
   FROM users
   WHERE lastSignIn > fromTime;
END$$
DELIMITER ;

-- 4. Add data (users)
INSERT INTO users(username, password) VALUE ('user1', 'MTIzNDU2QEBAdXNlZGZvcmVuY3J5cHRpbmd1c2VycGFzc3dvcmRz');
INSERT INTO users(username, password) VALUE ('user2', 'MTIzNDU2QEBAdXNlZGZvcmVuY3J5cHRpbmd1c2VycGFzc3dvcmRz');
INSERT INTO users(username, password) VALUE ('user3', 'MTIzNDU2QEBAdXNlZGZvcmVuY3J5cHRpbmd1c2VycGFzc3dvcmRz');