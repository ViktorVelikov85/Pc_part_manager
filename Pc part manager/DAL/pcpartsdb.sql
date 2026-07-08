-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jul 08, 2026 at 08:40 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `pcpartsdb`
--

-- --------------------------------------------------------

--
-- Table structure for table `categories`
--

CREATE TABLE `categories` (
  `id` int(11) NOT NULL,
  `name` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `categories`
--

INSERT INTO `categories` (`id`, `name`) VALUES
(1, 'CPU'),
(2, 'GPU'),
(4, 'Motherboard'),
(3, 'RAM'),
(5, 'SSD');

-- --------------------------------------------------------

--
-- Table structure for table `computer_parts`
--

CREATE TABLE `computer_parts` (
  `id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL,
  `manufacturer` varchar(50) NOT NULL,
  `price` decimal(10,2) NOT NULL,
  `quantity` int(11) NOT NULL DEFAULT 0,
  `category_id` int(11) NOT NULL,
  `part_type` varchar(20) NOT NULL,
  `socket` varchar(20) DEFAULT NULL,
  `cores` int(11) DEFAULT NULL,
  `vram_size_gb` int(11) DEFAULT NULL,
  `memory_type` varchar(20) DEFAULT NULL,
  `capacity_gb` int(11) DEFAULT NULL,
  `speed_mhz` int(11) DEFAULT NULL,
  `generation` varchar(10) DEFAULT NULL,
  `chipset` varchar(20) DEFAULT NULL,
  `form_factor` varchar(20) DEFAULT NULL,
  `read_speed_mb` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `computer_parts`
--

INSERT INTO `computer_parts` (`id`, `name`, `manufacturer`, `price`, `quantity`, `category_id`, `part_type`, `socket`, `cores`, `vram_size_gb`, `memory_type`, `capacity_gb`, `speed_mhz`, `generation`, `chipset`, `form_factor`, `read_speed_mb`) VALUES
(1, 'Core i5-13600K', 'Intel', 640.00, 12, 1, 'CPU', 'LGA1700', 14, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(2, 'Ryzen 7 7800X3D', 'AMD', 820.00, 8, 1, 'CPU', 'AM5', 8, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL),
(3, 'GeForce RTX 4070 Ti Super', 'NVIDIA', 1850.00, 5, 2, 'GPU', NULL, NULL, 16, 'GDDR6X', NULL, NULL, NULL, NULL, NULL, NULL),
(4, 'Radeon RX 7800 XT', 'AMD', 1150.00, 7, 2, 'GPU', NULL, NULL, 16, 'GDDR6', NULL, NULL, NULL, NULL, NULL, NULL),
(5, 'Vengeance RGB', 'Corsair', 240.00, 15, 3, 'RAM', NULL, NULL, NULL, NULL, 32, 6000, 'DDR5', NULL, NULL, NULL),
(6, 'Fury Beast', 'Kingston', 130.00, 20, 3, 'RAM', NULL, NULL, NULL, NULL, 16, 3200, 'DDR4', NULL, NULL, NULL),
(7, 'ROG STRIX B650-A GAMING WIFI', 'ASUS', 460.00, 4, 4, 'Motherboard', 'AM5', NULL, NULL, NULL, NULL, NULL, NULL, 'B650', 'ATX', NULL),
(8, 'PRIME Z790-P', 'ASUS', 390.00, 6, 4, 'Motherboard', 'LGA1700', NULL, NULL, NULL, NULL, NULL, NULL, 'Z790', 'ATX', NULL),
(9, '990 PRO M.2 NVMe', 'Samsung', 280.00, 25, 5, 'SSD', NULL, NULL, NULL, NULL, 1000, NULL, NULL, NULL, 'M.2', 7450),
(10, 'Crucial P3 Plus', 'Crucial', 160.00, 18, 5, 'SSD', NULL, NULL, NULL, NULL, 2000, NULL, NULL, NULL, 'M.2', 5000);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `categories`
--
ALTER TABLE `categories`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `name` (`name`);

--
-- Indexes for table `computer_parts`
--
ALTER TABLE `computer_parts`
  ADD PRIMARY KEY (`id`),
  ADD KEY `category_id` (`category_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `categories`
--
ALTER TABLE `categories`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `computer_parts`
--
ALTER TABLE `computer_parts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `computer_parts`
--
ALTER TABLE `computer_parts`
  ADD CONSTRAINT `computer_parts_ibfk_1` FOREIGN KEY (`category_id`) REFERENCES `categories` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
