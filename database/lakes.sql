CREATE TABLE `lakes` (
  `Id` varchar(36) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `USGSSiteId` varchar(255) DEFAULT NULL,
  `WQDataSiteId` int DEFAULT NULL,
  `Latitude` decimal(10,8) DEFAULT NULL,
  `Longitude` decimal(11,8) DEFAULT NULL,
  `CreatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdatedAt` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  `WaterTempUSGSSiteId` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci