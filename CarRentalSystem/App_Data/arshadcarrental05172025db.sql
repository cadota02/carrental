-- --------------------------------------------------------
-- Host:                         localhost
-- Server version:               8.0.41 - MySQL Community Server - GPL
-- Server OS:                    Win64
-- HeidiSQL Version:             12.10.0.7000
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for carrentaldb
CREATE DATABASE IF NOT EXISTS `carrentaldb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `carrentaldb`;

-- Dumping structure for table carrentaldb.bill
CREATE TABLE IF NOT EXISTS `bill` (
  `billid` int NOT NULL AUTO_INCREMENT,
  `bookingid` int DEFAULT NULL,
  `clientid` int DEFAULT NULL,
  `billno` varchar(30) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `billdate` date DEFAULT NULL,
  `remarks` varchar(45) DEFAULT NULL,
  `cash` decimal(10,2) DEFAULT NULL,
  `change` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`billid`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table carrentaldb.bill: ~4 rows (approximately)
INSERT INTO `bill` (`billid`, `bookingid`, `clientid`, `billno`, `billdate`, `remarks`, `cash`, `change`) VALUES
	(4, 7, 7, 'BILL15373009', '2025-05-17', 'test', 25000.00, 3500.00),
	(10, 12, 7, 'BILL63759379', '2025-05-17', '', 0.00, 0.00),
	(13, 14, 7, 'BILL31531248', '2025-05-17', '', 7000.00, 1600.00),
	(14, 9, 7, 'BILL47829190', '2025-05-17', '', 0.00, 0.00);

-- Dumping structure for table carrentaldb.billitems
CREATE TABLE IF NOT EXISTS `billitems` (
  `billtemid` int NOT NULL AUTO_INCREMENT,
  `billid` int NOT NULL,
  `serviceid` int DEFAULT NULL,
  `name` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `description` varchar(255) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `qty` int DEFAULT '0',
  `price` decimal(10,2) DEFAULT '0.00',
  `datelog` datetime DEFAULT NULL,
  PRIMARY KEY (`billtemid`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table carrentaldb.billitems: ~8 rows (approximately)
INSERT INTO `billitems` (`billtemid`, `billid`, `serviceid`, `name`, `description`, `qty`, `price`, `datelog`) VALUES
	(7, 4, 0, 'TOYOTA WIGO booked (May 16–19, 2025  4 day/s)', '', 4, 5000.00, '2025-05-17 09:31:22'),
	(9, 4, NULL, 'Penalty Fee', '', 1, 1500.00, '2025-05-17 11:31:37'),
	(15, 10, 0, 'TOYOTA WIGO 2 booked (May 17–20, 2025  4 day/s)', '', 4, 2000.00, '2025-05-17 11:27:57'),
	(16, 10, NULL, 'Package 1', '', 1, 5000.00, '2025-05-17 11:28:03'),
	(17, 10, NULL, 'Penalty Fee', '', 1, 1500.00, '2025-05-17 11:28:10'),
	(20, 13, 0, 'TOYOTA INNOVA booked (May 17–19, 2025  3 day/s)', '', 3, 1800.00, '2025-05-17 11:39:33'),
	(21, 14, 0, 'TOYOTA VIOS booked (May 16–18, 2025  3 day/s)', '', 3, 0.00, '2025-05-17 11:49:35'),
	(22, 14, NULL, 'Penalty Fee', '', 1, 1500.00, '2025-05-17 11:49:43');

-- Dumping structure for table carrentaldb.bookings
CREATE TABLE IF NOT EXISTS `bookings` (
  `booking_id` int NOT NULL AUTO_INCREMENT,
  `bookingno` varchar(40) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `car_id` int NOT NULL,
  `booking_date` date NOT NULL,
  `return_date` date NOT NULL,
  `status` enum('pending','approved','rejected') DEFAULT 'pending',
  `admin_remarks` text,
  `client_remarks` text,
  `client_name` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `client_address` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `client_contactno` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `client_email` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `userid` int DEFAULT '0',
  PRIMARY KEY (`booking_id`),
  KEY `car_id` (`car_id`),
  CONSTRAINT `bookings_ibfk_1` FOREIGN KEY (`car_id`) REFERENCES `cars` (`car_id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table carrentaldb.bookings: ~10 rows (approximately)
INSERT INTO `bookings` (`booking_id`, `bookingno`, `car_id`, `booking_date`, `return_date`, `status`, `admin_remarks`, `client_remarks`, `client_name`, `client_address`, `client_contactno`, `client_email`, `created_at`, `updated_at`, `userid`) VALUES
	(2, '250400002', 6, '2025-04-13', '2025-04-15', 'approved', 'testasdadadadad', 'asdasdasdadadadadad', 'as', 'dadad', 'adada', 'adadad@gmail.com', '2025-04-13 14:08:12', '2025-04-15 06:01:50', 0),
	(3, '250400003', 5, '2025-04-13', '2025-04-13', 'pending', NULL, 'adsadasd', 'asd', 'adada', 'dadad', 'adadad@gmail.com', '2025-04-13 15:06:19', '2025-04-13 15:06:19', 0),
	(4, '250400004', 2, '2025-04-13', '2025-04-13', 'rejected', 'ASDAD', 'adsadasd', 'asd', 'adada', 'dadad', 'adadad@gmail.com', '2025-04-13 15:07:00', '2025-04-14 05:16:30', 0),
	(5, '250400004', 7, '2025-04-21', '2025-04-25', 'pending', NULL, 'adadadad', 'asd', 'ada', 'dadad', 'adad@gmail.com', '2025-04-15 06:04:54', '2025-04-15 06:04:54', 0),
	(6, '250400005', 2, '2025-04-02', '2025-04-23', 'pending', NULL, 'asdadad', 'as', 'dad', 'adadada', 'adsa@gmail.com', '2025-04-20 13:14:01', '2025-04-20 13:14:01', 0),
	(7, '250500001', 7, '2025-05-16', '2025-05-19', 'pending', NULL, 'test', 'aaa C. bb', '12321313', '12313', 'cccc@gmail.com', '2025-05-16 11:39:54', '2025-05-16 22:46:05', 7),
	(9, '250500002', 4, '2025-05-16', '2025-05-18', 'pending', NULL, '', 'aaa c. bb', '12321313', '12313', 'cccc@gmail.com', '2025-05-16 13:29:44', '2025-05-16 13:29:44', 7),
	(12, '250500004', 8, '2025-05-17', '2025-05-20', 'approved', 'test', 'test message', 'as d. aa', 'asdad', 'adadad', 'asd@gmail.com', '2025-05-16 23:09:04', '2025-05-16 23:14:36', 7),
	(13, '250500005', 5, '2025-05-19', '2025-05-23', 'pending', NULL, 'test', 'aaa c. bb', '12321313', '12313', 'cccc@gmail.com', '2025-05-17 03:25:39', '2025-05-17 03:25:39', 7),
	(14, '250500005', 2, '2025-05-17', '2025-05-19', 'pending', NULL, 'test', 'aaa c. bb', '12321313', '12313', 'cccc@gmail.com', '2025-05-17 03:38:38', '2025-05-17 03:38:38', 7);

-- Dumping structure for table carrentaldb.booking_services
CREATE TABLE IF NOT EXISTS `booking_services` (
  `booking_service_id` int NOT NULL AUTO_INCREMENT,
  `booking_id` int NOT NULL,
  `service_id` int NOT NULL,
  PRIMARY KEY (`booking_service_id`),
  KEY `booking_id` (`booking_id`),
  KEY `service_id` (`service_id`),
  CONSTRAINT `booking_services_ibfk_1` FOREIGN KEY (`booking_id`) REFERENCES `bookings` (`booking_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `booking_services_ibfk_2` FOREIGN KEY (`service_id`) REFERENCES `services` (`service_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table carrentaldb.booking_services: ~0 rows (approximately)

-- Dumping structure for table carrentaldb.cars
CREATE TABLE IF NOT EXISTS `cars` (
  `car_id` int NOT NULL AUTO_INCREMENT,
  `car_name` varchar(100) NOT NULL,
  `description` text,
  `image_path` varchar(255) DEFAULT NULL,
  `contact_person` varchar(100) DEFAULT NULL,
  `contact_no` varchar(20) DEFAULT NULL,
  `is_available` tinyint(1) DEFAULT '1',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `RatePerDay` decimal(10,2) DEFAULT '0.00',
  PRIMARY KEY (`car_id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table carrentaldb.cars: ~5 rows (approximately)
INSERT INTO `cars` (`car_id`, `car_name`, `description`, `image_path`, `contact_person`, `contact_no`, `is_available`, `created_at`, `RatePerDay`) VALUES
	(2, 'TOYOTA INNOVA', '<p>Multi-Purpose Vehicle (Toyota Innova 2022 or above)<br />\r\nup to 6 passengers<br />\r\nup to 2 small luggages<br />\r\nRental rates:<br />\r\n4,500 transfer (pick-up &amp; drop-off) w/in Metro Manila<br />\r\n6,000 anywhere within Metro Manila for 8 hours<br />\r\n&bull; 9,000 w/in MM, Cavite, Laguna, Bulacan &amp; Rizal for 8 hours<br />\r\n&bull; 12,000 anywhere within Luzon island for 8 hours<br />\r\n&bull; 900 per extra hour</p>\r\n', '~/uploads/dfbf578a-4eec-4fb0-b518-4d866d8dbf21.jpg', 'adada', 'dadad', 1, '2025-04-09 14:59:33', 1800.00),
	(4, 'TOYOTA VIOS', '<ul>\r\n	<li>Compact Sedan (Toyota Vios 2022 or above)<br />\r\n	up to 4 passengers<br />\r\n	up to 2 medium luggages<br />\r\n	Rental rates:<br />\r\n	&bull; 3,000 transfer (pick-up &amp; drop-off) w/in Metro Manila<br />\r\n	&bull; 4,000 anywhere within Metro Manila for 8 hours<br />\r\n	&bull; 6,000 w/in MM, Cavite, Laguna, Bulacan &amp; Rizal for 8 hours<br />\r\n	&bull; 8,000 anywhere within Luzon island for 8 hours<br />\r\n	600 per extra hour</li>\r\n</ul>\r\n', '~/uploads/ec8fb7bc-4d70-485c-89fd-04114d198bc2.jpg', 'dad', 'adada', 1, '2025-04-09 15:22:06', 0.00),
	(5, 'TOYOTA FORTUNER', '<p>Mid-size SUV (Toyota Fortuner 2024 or above)<br />\r\nup to 6 passengers<br />\r\nup to 2 small luggages<br />\r\nRental rates:<br />\r\n&bull; 5,250 transfer (pick-up &amp; drop-off) w/in Metro Manila<br />\r\n&bull; 7,000 anywhere within Metro Manila for 8 hours<br />\r\n&bull; 10,500 w/in MM, Cavite, Laguna, Bulacan &amp; Rizal for 8 hours<br />\r\n&bull; 14,000 anywhere within Luzon island for 8 hours<br />\r\n&bull; 1,050 per extra hour</p>\r\n', '~/uploads/033de970-61cb-47c5-a0ec-2c5f24a0056b.jpg', 'ASD', 'SADAD', 1, '2025-04-11 15:54:36', 0.00),
	(6, 'TOYOTA HIACE GRANDIA', '<p>Luxury Van (2024 Toyota Hiace Super Grandia<br />\r\nElite)<br />\r\nup to 9 passengers<br />\r\nup to 4 medium luggages<br />\r\nRental rates:<br />\r\n6,000 transfer (pick-up &amp; drop-off) w/in Metro Manila<br />\r\n8,000 anywhere within Metro Manila for 8 hours<br />\r\n12,000 w/in MM, Cavite, Laguna, Bulacan &amp;<br />\r\nRizal for 8 hours<br />\r\n&bull; 16,000 anywhere within Luzon island for 8 hours<br />\r\n1,200 per extra hour</p>\r\n', '~/uploads/4a8c0182-0d2a-4515-a872-8bab172eaf82.jpg', 'AA', 'SDAD', 1, '2025-04-11 15:55:14', 2500.00),
	(7, 'TOYOTA WIGO', '<p>Multi-Purpose Vehicle (Toyota Wigo 2022 or above)<br />\r\nup to 4 passengers<br />\r\nup to 2 small luggages<br />\r\nRental rates:<br />\r\n4,500 transfer (pick-up &amp; drop-off) w/in Metro Manila<br />\r\n6,000 anywhere within Metro Manila for 8 hours<br />\r\n&bull; 9,000 w/in MM, Cavite, Laguna, Bulacan &amp; Rizal for 8 hours<br />\r\n&bull; 12,000 anywhere within Luzon island for 8 hours<br />\r\n&bull; 900 per extra hour</p>\r\n', '~/uploads/cff36d5e-d9c8-4907-ad8b-dd82598afdbd.jpg', 'dad123131', '01231313', 1, '2025-04-15 05:57:17', 5000.00),
	(8, 'TOYOTA WIGO 2', '<ol>\r\n	<li>adsa adsadada</li>\r\n	<li>asdsadadad</li>\r\n	<li>adsada</li>\r\n</ol>\r\n', '~/uploads/9e5dcabd-92a3-4b8b-bd6e-ec836669afae.jpg', 'adas', 'asd12313', 1, '2025-05-10 13:34:41', 2000.00);

-- Dumping structure for table carrentaldb.contact_us
CREATE TABLE IF NOT EXISTS `contact_us` (
  `contact_id` int NOT NULL AUTO_INCREMENT,
  `full_name` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `subject` varchar(150) DEFAULT NULL,
  `message` text NOT NULL,
  `phone` varchar(20) DEFAULT NULL,
  `status` enum('new','read','archived') DEFAULT 'new',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`contact_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table carrentaldb.contact_us: ~2 rows (approximately)
INSERT INTO `contact_us` (`contact_id`, `full_name`, `email`, `subject`, `message`, `phone`, `status`, `created_at`) VALUES
	(1, 'asda', 'dadad@gmail.com', 'adadsa', 'adadadada d asdadasd', 'dadadad', 'archived', '2025-04-15 04:54:09'),
	(2, 'sd', 'adada@gmail.com', 'adada', 'dadsdadad', 'dada', 'read', '2025-04-15 05:46:55');

-- Dumping structure for table carrentaldb.services
CREATE TABLE IF NOT EXISTS `services` (
  `service_id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `description` text,
  `price` decimal(10,2) NOT NULL,
  `is_active` tinyint(1) DEFAULT '1',
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`service_id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table carrentaldb.services: ~2 rows (approximately)
INSERT INTO `services` (`service_id`, `name`, `description`, `price`, `is_active`, `created_at`) VALUES
	(3, 'Extended Rental', 'extension of rent', 2500.00, 1, '2025-04-09 15:52:57'),
	(4, 'Penalty Fee', 'Fees exceed hour duration', 1500.00, 1, '2025-05-10 13:49:38'),
	(5, 'Package 1', 'COTABATO - GENSAN 5DAYS', 5000.00, 1, '2025-05-16 08:44:13');

-- Dumping structure for table carrentaldb.users
CREATE TABLE IF NOT EXISTS `users` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Firstname` varchar(255) DEFAULT NULL,
  `Lastname` varchar(255) DEFAULT NULL,
  `Middlename` varchar(255) DEFAULT NULL,
  `UserPosition` varchar(255) DEFAULT NULL,
  `Username` varchar(100) DEFAULT NULL,
  `PasswordHash` varchar(255) DEFAULT NULL,
  `Address` text,
  `ContactNo` varchar(15) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `Role` enum('Admin','Client','Cashier') DEFAULT 'Client',
  `IsApproved` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`ID`),
  UNIQUE KEY `Username` (`Username`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=latin1;

-- Dumping data for table carrentaldb.users: ~4 rows (approximately)
INSERT INTO `users` (`ID`, `Firstname`, `Lastname`, `Middlename`, `UserPosition`, `Username`, `PasswordHash`, `Address`, `ContactNo`, `Email`, `Role`, `IsApproved`) VALUES
	(2, 'AdminFirst', 'AdminLast', 'AdminMI', 'Administrator', 'admin', '$2a$11$g0f07n5ZrybHitHx8pigHuLjLG3F9HkGCVXgPK1n0TfpY311wHO.i', '', '09123213', 'admin@example.com', 'Admin', 1),
	(6, 'user1', 'user1', 'user1', 'user1', 'user1', '$2a$11$X39zUJEQ3Xq.osLn6OG9MOYKopKRjB5D8AZaQiIorUHz73Sp5CSi.', 'jhjfhjjjjfjfhjfhh', '099878', 'user1@gmail.com', 'Client', 1),
	(7, 'aaa', 'bb', 'ccc', 'Customer', 'user2', '$2a$11$vCES.FWPzj9fbXXja6Kmw.WFlW4na1p7Hl1NOlXCgTgfCqFmwceTC', '12321313', '12313', 'cccc@gmail.com', 'Client', 1),
	(8, 'IMELDA', 'SANTOS', 'ASD', 'CASHIER', 'cashier1', '$2a$11$j7LnRRfHi1sJOB2MflwiSe78Eogjblohn8iHerlBWZkyVyuz7C8R6', '1232', '12313123', '12132@GMAIL.COM', 'Cashier', 1);

-- Dumping structure for view carrentaldb.vw_bill
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `vw_bill` (
	`billid` INT NULL,
	`bookingid` INT NULL,
	`clientid` INT NULL,
	`billno` VARCHAR(1) NULL COLLATE 'utf8mb3_general_ci',
	`billdate` DATE NULL,
	`remarks` VARCHAR(1) NULL COLLATE 'utf8mb4_0900_ai_ci',
	`cash` DECIMAL(10,2) NULL,
	`change` DECIMAL(10,2) NULL,
	`fullname` TEXT NOT NULL COLLATE 'latin1_swedish_ci',
	`amount` DECIMAL(42,2) NULL,
	`bookinfo` VARCHAR(1) NULL COLLATE 'utf8mb4_0900_ai_ci'
) ENGINE=MyISAM;

-- Dumping structure for view carrentaldb.vw_booking
-- Creating temporary table to overcome VIEW dependency errors
CREATE TABLE `vw_booking` (
	`booking_id` INT NOT NULL,
	`bookingno` VARCHAR(1) NULL COLLATE 'utf8mb3_general_ci',
	`car_id` INT NOT NULL,
	`booking_date` DATE NOT NULL,
	`return_date` DATE NOT NULL,
	`status` ENUM('pending','approved','rejected') NULL COLLATE 'utf8mb4_0900_ai_ci',
	`admin_remarks` TEXT NULL COLLATE 'utf8mb4_0900_ai_ci',
	`client_remarks` TEXT NULL COLLATE 'utf8mb4_0900_ai_ci',
	`client_name` VARCHAR(1) NULL COLLATE 'utf8mb3_general_ci',
	`client_address` VARCHAR(1) NULL COLLATE 'utf8mb3_general_ci',
	`client_contactno` VARCHAR(1) NULL COLLATE 'utf8mb3_general_ci',
	`client_email` VARCHAR(1) NULL COLLATE 'utf8mb3_general_ci',
	`created_at` TIMESTAMP NULL,
	`updated_at` TIMESTAMP NULL,
	`car_name` VARCHAR(1) NULL COLLATE 'utf8mb4_0900_ai_ci',
	`description` TEXT NULL COLLATE 'utf8mb4_0900_ai_ci',
	`image_path` VARCHAR(1) NULL COLLATE 'utf8mb4_0900_ai_ci',
	`contact_no` VARCHAR(1) NULL COLLATE 'utf8mb4_0900_ai_ci',
	`contact_person` VARCHAR(1) NULL COLLATE 'utf8mb4_0900_ai_ci',
	`is_available` TINYINT(1) NULL,
	`dayscount` INT NULL,
	`time_ago` VARCHAR(1) NULL COLLATE 'utf8mb4_0900_ai_ci',
	`userid` INT NULL,
	`formatted_date` VARCHAR(1) NULL COLLATE 'utf8mb4_0900_ai_ci',
	`duration_days` BIGINT NULL,
	`RatePerDay` DECIMAL(10,2) NULL,
	`billid` BIGINT NOT NULL
) ENGINE=MyISAM;

-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `vw_bill`;
CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `vw_bill` AS select `a`.`billid` AS `billid`,`a`.`bookingid` AS `bookingid`,`a`.`clientid` AS `clientid`,`a`.`billno` AS `billno`,`a`.`billdate` AS `billdate`,`a`.`remarks` AS `remarks`,`a`.`cash` AS `cash`,`a`.`change` AS `change`,ifnull(concat(`b`.`Firstname`,' ',upper(left(`b`.`Middlename`,1)),'. ',`b`.`Lastname`),'Walk-In') AS `fullname`,(select sum((`z`.`qty` * `z`.`price`)) from `billitems` `z` where (`z`.`billid` = `a`.`billid`)) AS `amount`,concat(`c`.`car_name`,', ',`c`.`formatted_date`,' ',`c`.`duration_days`,' day/s') AS `bookinfo` from ((`bill` `a` left join `users` `b` on((`b`.`ID` = `a`.`clientid`))) left join `vw_booking` `c` on((`c`.`billid` = `a`.`billid`)))
;

-- Removing temporary table and create final VIEW structure
DROP TABLE IF EXISTS `vw_booking`;
CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `vw_booking` AS select `a`.`booking_id` AS `booking_id`,`a`.`bookingno` AS `bookingno`,`a`.`car_id` AS `car_id`,`a`.`booking_date` AS `booking_date`,`a`.`return_date` AS `return_date`,`a`.`status` AS `status`,`a`.`admin_remarks` AS `admin_remarks`,`a`.`client_remarks` AS `client_remarks`,`a`.`client_name` AS `client_name`,`a`.`client_address` AS `client_address`,`a`.`client_contactno` AS `client_contactno`,`a`.`client_email` AS `client_email`,`a`.`created_at` AS `created_at`,`a`.`updated_at` AS `updated_at`,`b`.`car_name` AS `car_name`,`b`.`description` AS `description`,`b`.`image_path` AS `image_path`,`b`.`contact_no` AS `contact_no`,`b`.`contact_person` AS `contact_person`,`b`.`is_available` AS `is_available`,(to_days(curdate()) - to_days(cast(`a`.`created_at` as date))) AS `dayscount`,(case when (timestampdiff(HOUR,`a`.`created_at`,now()) < 1) then 'Just now' when (timestampdiff(HOUR,`a`.`created_at`,now()) < 24) then concat(timestampdiff(HOUR,`a`.`created_at`,now()),' hour(s) ago') when ((to_days(curdate()) - to_days(cast(`a`.`created_at` as date))) = 1) then 'Yesterday' when ((to_days(curdate()) - to_days(cast(`a`.`created_at` as date))) = 2) then '2 days ago' else concat((to_days(curdate()) - to_days(cast(`a`.`created_at` as date))),' days ago') end) AS `time_ago`,`a`.`userid` AS `userid`,(case when (month(`a`.`booking_date`) = month(`a`.`return_date`)) then concat(date_format(`a`.`booking_date`,'%b '),dayofmonth(`a`.`booking_date`),'–',dayofmonth(`a`.`return_date`),', ',year(`a`.`booking_date`)) else concat(date_format(`a`.`booking_date`,'%b '),dayofmonth(`a`.`booking_date`),'–',date_format(`a`.`return_date`,'%b '),dayofmonth(`a`.`return_date`),', ',year(`a`.`booking_date`)) end) AS `formatted_date`,((to_days(`a`.`return_date`) - to_days(`a`.`booking_date`)) + 1) AS `duration_days`,`b`.`RatePerDay` AS `RatePerDay`,ifnull(`c`.`billid`,0) AS `billid` from ((`bookings` `a` left join `cars` `b` on((`b`.`car_id` = `a`.`car_id`))) left join `bill` `c` on((`c`.`bookingid` = `a`.`booking_id`)))
;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
