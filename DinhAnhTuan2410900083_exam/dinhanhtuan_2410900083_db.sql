-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1
-- Thời gian đã tạo: Th9 26, 2026 lúc 04:02 AM
-- Phiên bản máy phục vụ: 10.4.32-MariaDB
-- Phiên bản PHP: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `dinhanhtuan_2410900083_db`
--

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `datstudent`
--

CREATE TABLE `datstudent` (
  `Id` int(11) NOT NULL,
  `DATName` varchar(100) NOT NULL,
  `DATGender` tinyint(1) DEFAULT 1 COMMENT '1: Nam, 0: Nữ',
  `DATBirthDay` date DEFAULT NULL,
  `DATEmail` varchar(100) NOT NULL,
  `DATPhone` varchar(15) DEFAULT NULL,
  `DATActive` bit(1) DEFAULT b'1'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `datstudent`
--

INSERT INTO `datstudent` (`Id`, `DATName`, `DATGender`, `DATBirthDay`, `DATEmail`, `DATPhone`, `DATActive`) VALUES
(1, 'Định Anh Tuấn', 1, '2004-01-01', 'anhtuan@gmail.com', '0912345678', b'1'),
(2, 'Nguyễn Văn A', 1, '2003-05-15', 'nguyenvana@gmail.com', '0987654321', b'1');

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `datstudent`
--
ALTER TABLE `datstudent`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `DATEmail` (`DATEmail`);

--
-- AUTO_INCREMENT cho các bảng đã đổ
--

--
-- AUTO_INCREMENT cho bảng `datstudent`
--
ALTER TABLE `datstudent`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
