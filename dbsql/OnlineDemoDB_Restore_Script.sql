-- Run this sql file before running the project.

--
-- Script was generated by Devart dbForge Studio for MySQL, Version 7.4.201.0
-- Product home page: http://www.devart.com/dbforge/mysql/studio
-- Script date 2018/5/17 下午 09:51:17
-- Server version: 5.5.5-10.2.13-MariaDB-10.2.13+maria~xenial-log
-- Client version: 4.1
--


-- 
-- Set SQL mode
-- 
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 
-- Set character set the client will use to send SQL statements to the server
--
SET NAMES 'utf8';

DROP DATABASE IF EXISTS OnlineShopDemoDB;

CREATE DATABASE IF NOT EXISTS OnlineShopDemoDB
	CHARACTER SET utf8
	COLLATE utf8_general_ci;

--
-- Set default database
--
USE OnlineShopDemoDB;

--
-- Create table `Category`
--
CREATE TABLE IF NOT EXISTS Category (
  CategoryID INT(11) NOT NULL,
  CategoryName VARCHAR(30) NOT NULL,
  CategoryIntroduction TEXT DEFAULT NULL,
  DBAddDate DATE DEFAULT NULL,
  PRIMARY KEY (CategoryID)
)
ENGINE = INNODB,
AVG_ROW_LENGTH = 4096,
CHARACTER SET utf8,
COLLATE utf8_general_ci;

--
-- Create table `Products`
--
CREATE TABLE IF NOT EXISTS Products (
  ProductID INT(11) NOT NULL,
  ProductName VARCHAR(30) NOT NULL,
  Introduction TEXT DEFAULT NULL,
  Price DOUBLE DEFAULT NULL,
  PackageSize VARCHAR(12) DEFAULT NULL,
  Weight DOUBLE DEFAULT NULL,
  Status VARCHAR(5) DEFAULT NULL,
  PublishDate DATE DEFAULT NULL,
  DBAddDate DATE DEFAULT NULL,
  Notes VARCHAR(100) DEFAULT NULL,
  CategoryID INT(11) NOT NULL,
  PRIMARY KEY (ProductID, CategoryID)
)
ENGINE = INNODB,
AVG_ROW_LENGTH = 1024,
CHARACTER SET utf8,
COLLATE utf8_general_ci;

--
-- Create foreign key
--
ALTER TABLE Products 
  ADD CONSTRAINT `Belongs to` FOREIGN KEY (CategoryID)
    REFERENCES Category(CategoryID) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Create table `ShoppingCartPool`
--
CREATE TABLE IF NOT EXISTS ShoppingCartPool (
  ItemID VARCHAR(40) NOT NULL,
  UserID VARCHAR(40) NOT NULL,
  Quantity INT(11) NOT NULL,
  RecordCreateDate TIMESTAMP(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE CURRENT_TIMESTAMP,
  ProductID INT(11) NOT NULL,
  CategoryID INT(11) NOT NULL,
  IsOrdered TINYINT(1) NOT NULL DEFAULT 0,
  IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (ItemID, ProductID, CategoryID)
)
ENGINE = INNODB,
AVG_ROW_LENGTH = 585,
CHARACTER SET utf8,
COLLATE utf8_general_ci;

--
-- Create index `ItemID` on table `ShoppingCartPool`
--
ALTER TABLE ShoppingCartPool 
  ADD UNIQUE INDEX ItemID(ItemID);

--
-- Create foreign key
--
ALTER TABLE ShoppingCartPool 
  ADD CONSTRAINT PoolItemProduct FOREIGN KEY (ProductID, CategoryID)
    REFERENCES Products(ProductID, CategoryID) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Create table `ProductSetRecord`
--
CREATE TABLE IF NOT EXISTS ProductSetRecord (
  ProductID INT(11) NOT NULL,
  CategoryID INT(11) NOT NULL,
  ProductSetID INT(11) NOT NULL,
  SetCategory INT(11) NOT NULL,
  Quantity INT(11) NOT NULL,
  PRIMARY KEY (ProductID, CategoryID, ProductSetID, SetCategory)
)
ENGINE = INNODB,
AVG_ROW_LENGTH = 2730,
CHARACTER SET utf8,
COLLATE utf8_general_ci;

--
-- Create foreign key
--
ALTER TABLE ProductSetRecord 
  ADD CONSTRAINT ProductsBelongsToSst FOREIGN KEY (ProductSetID, SetCategory)
    REFERENCES Products(ProductID, CategoryID) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Create foreign key
--
ALTER TABLE ProductSetRecord 
  ADD CONSTRAINT ProductsInRecord FOREIGN KEY (ProductID, CategoryID)
    REFERENCES Products(ProductID, CategoryID) ON DELETE NO ACTION ON UPDATE CASCADE;

--
-- Create table `OrderRecord`
--
CREATE TABLE IF NOT EXISTS OrderRecord (
  OrderID VARCHAR(40) NOT NULL,
  OrderCreateDate TIMESTAMP(6) NOT NULL DEFAULT current_timestamp(6) ON UPDATE CURRENT_TIMESTAMP,
  UserName VARCHAR(30) NOT NULL,
  OrdererName VARCHAR(10) NOT NULL,
  PhoneNumber VARCHAR(15) NOT NULL,
  ShippingAddress VARCHAR(100) NOT NULL,
  EmailAddress VARCHAR(100) DEFAULT NULL,
  TotalPrice DECIMAL(18, 0) NOT NULL,
  IsShipped TINYINT(1) NOT NULL DEFAULT 0,
  ShippingDate TIMESTAMP(6) NULL DEFAULT NULL,
  PaymentMethod VARCHAR(10) NOT NULL,
  Note VARCHAR(255) DEFAULT NULL,
  IsCanceled TINYINT(1) DEFAULT 0,
  ShopNote VARCHAR(255) DEFAULT NULL,
  PRIMARY KEY (OrderID)
)
ENGINE = INNODB,
AVG_ROW_LENGTH = 3276,
CHARACTER SET utf8,
COLLATE utf8_general_ci;

--
-- Create index `OrderID` on table `OrderRecord`
--
ALTER TABLE OrderRecord 
  ADD UNIQUE INDEX OrderID(OrderID);

--
-- Create table `OrderItemPool`
--
CREATE TABLE IF NOT EXISTS OrderItemPool (
  ItemRecordID INT(11) NOT NULL,
  OrderID VARCHAR(40) NOT NULL,
  Quanitiy INT(11) NOT NULL,
  UserName VARCHAR(40) NOT NULL,
  OrderedPrice DECIMAL(18, 0) NOT NULL,
  ProductName VARCHAR(30) NOT NULL,
  PRIMARY KEY (ItemRecordID, OrderID)
)
ENGINE = INNODB,
AVG_ROW_LENGTH = 364,
CHARACTER SET utf8,
COLLATE utf8_general_ci;

--
-- Create index `ItemRecordID` on table `OrderItemPool`
--
ALTER TABLE OrderItemPool 
  ADD UNIQUE INDEX ItemRecordID(ItemRecordID);

--
-- Create foreign key
--
ALTER TABLE OrderItemPool 
  ADD CONSTRAINT `OrderItems Belonging` FOREIGN KEY (OrderID)
    REFERENCES OrderRecord(OrderID) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- 
-- Dumping data for table Category
--
INSERT INTO Category VALUES
(1, 'Category 1', 'Category 1', '2016-10-20'),
(2, 'Category 2', 'Category 2', '2016-10-20'),
(3, 'Category 3', 'Category 3', '2016-10-20');

-- 
-- Dumping data for table Products
--
INSERT INTO Products VALUES
(1, 'Product 1', 'Product 1', 280, NULL, NULL, '下架', '0001-01-01', NULL, '每一盒10包', 1),
(2, 'Product 2', 'Product 2', 450, NULL, NULL, '上市', '0001-01-01', NULL, '每一盒10包', 1),
(3, 'Product 3', 'Product 3', 220, NULL, 227, '下架', '0001-01-01', NULL, NULL, 1),
(4, 'Product 4', 'Product 4', 300, NULL, 227, '上市', '0001-01-01', NULL, NULL, 1),
(5, 'Product 5', 'Product 5', 550, NULL, 227, '上市', '0001-01-01', NULL, NULL, 1),
(6, 'Product 6', 'Product 6', 650, NULL, 227, '上市', '0001-01-01', NULL, NULL, 1),
(7, 'Product 7', 'Product 7', 800, NULL, 227, '上市', '0001-01-01', NULL, NULL, 1),
(8, 'Product 8', 'Product 8', 1000, NULL, 227, '缺貨', '0001-01-01', NULL, NULL, 1),
(9, 'Product 9', 'Product 9', 1100, NULL, 227, '缺貨', '0001-01-01', NULL, NULL, 1),
(10, 'Product 10 (Set 1)', 'set 1', 970, NULL, NULL, '下架', '0001-01-01', NULL, NULL, 2),
(11, 'Product 11 (Set 2)', 'set 2', 620, NULL, NULL, '下架', '0001-01-01', NULL, NULL, 2),
(12, 'Product 12 (Set 3)', 'set 3', 860, NULL, NULL, '下架', '0001-01-01', NULL, NULL, 2),
(13, 'Product 13', 'Product 13', 180, NULL, 85, '上市', NULL, NULL, NULL, 3),
(14, 'Product 14', 'Product 14', 180, NULL, 85, '上市', NULL, NULL, NULL, 3),
(15, 'Product 15', 'Product 15', 180, NULL, 85, '上市', NULL, NULL, NULL, 3),
(16, 'Product 16', 'Product 16', 180, NULL, 85, '上市', NULL, NULL, NULL, 3),
(17, 'Product 17', 'Product 17', 180, NULL, 85, '上市', NULL, NULL, NULL, 3),
(18, 'Product 18', 'Product 18', 180, NULL, 85, '上市', NULL, NULL, NULL, 3),
(19, 'Product 19', 'Product 19', 800, NULL, NULL, '下架', '2017-08-01', NULL, NULL, 2),
(20, 'Product 20', 'Product 20', 350, '15 unit', NULL, '下架', '2017-08-01', NULL, NULL, 2);


-- 
-- Dumping data for table ProductSetRecord
--
INSERT INTO ProductSetRecord VALUES
(2, 1, 10, 2, 6),
(2, 1, 11, 2, 6),
(2, 1, 12, 2, 18),
(4, 1, 11, 2, 1),
(6, 1, 10, 2, 1);

-- 
-- Restore previous SQL mode
-- 
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;