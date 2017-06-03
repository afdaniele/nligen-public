-- phpMyAdmin SQL Dump
-- version 4.0.10
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Jun 03, 2017 at 05:22 PM
-- Server version: 5.5.53-0ubuntu0.14.04.1
-- PHP Version: 5.5.9-1ubuntu4.20

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `nligen_sim`
--

-- --------------------------------------------------------

--
-- Table structure for table `consent`
--

CREATE TABLE IF NOT EXISTS `consent` (
  `workerId` varchar(50) NOT NULL,
  `granted` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`workerId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `experiment`
--

CREATE TABLE IF NOT EXISTS `experiment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ex_id` int(11) NOT NULL,
  `who` enum('robot','human') NOT NULL,
  `label` varchar(32) NOT NULL,
  `data` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=101 ;

-- --------------------------------------------------------

--
-- Stand-in structure for view `get_distinct_ex_ids`
--
CREATE TABLE IF NOT EXISTS `get_distinct_ex_ids` (
`ex_id` int(11)
);
-- --------------------------------------------------------

--
-- Stand-in structure for view `get_exId_H_R`
--
CREATE TABLE IF NOT EXISTS `get_exId_H_R` (
`ex_id` int(11)
,`human` bigint(21)
,`machine` bigint(21)
);
-- --------------------------------------------------------

--
-- Stand-in structure for view `get_exId_who_counter`
--
CREATE TABLE IF NOT EXISTS `get_exId_who_counter` (
`ex_id` int(11)
,`who` enum('robot','human')
,`counter` bigint(21)
);
-- --------------------------------------------------------

--
-- Stand-in structure for view `H_submissions`
--
CREATE TABLE IF NOT EXISTS `H_submissions` (
`workerId` varchar(50)
,`experimentId` tinyint(3) unsigned
,`hitId` varchar(50)
,`assignmentId` varchar(50)
,`submissionTime` timestamp
,`resultData` text
);
-- --------------------------------------------------------

--
-- Stand-in structure for view `H_survey`
--
CREATE TABLE IF NOT EXISTS `H_survey` (
`workerId` varchar(50)
,`experimentId` tinyint(3) unsigned
,`hitId` varchar(50)
,`assignmentId` varchar(50)
,`submissionTime` timestamp
,`native` tinyint(3) unsigned
,`gender` tinyint(3) unsigned
,`age` tinyint(3) unsigned
,`difficulty` tinyint(3) unsigned
,`backtrack` tinyint(3) unsigned
,`who` tinyint(3) unsigned
,`information` tinyint(3) unsigned
,`confidence` tinyint(3) unsigned
,`suggestions` text
);
-- --------------------------------------------------------

--
-- Stand-in structure for view `R_submissions`
--
CREATE TABLE IF NOT EXISTS `R_submissions` (
`workerId` varchar(50)
,`experimentId` tinyint(3) unsigned
,`hitId` varchar(50)
,`assignmentId` varchar(50)
,`submissionTime` timestamp
,`resultData` text
);
-- --------------------------------------------------------

--
-- Stand-in structure for view `R_survey`
--
CREATE TABLE IF NOT EXISTS `R_survey` (
`workerId` varchar(50)
,`experimentId` tinyint(3) unsigned
,`hitId` varchar(50)
,`assignmentId` varchar(50)
,`submissionTime` timestamp
,`native` tinyint(3) unsigned
,`gender` tinyint(3) unsigned
,`age` tinyint(3) unsigned
,`difficulty` tinyint(3) unsigned
,`backtrack` tinyint(3) unsigned
,`who` tinyint(3) unsigned
,`information` tinyint(3) unsigned
,`confidence` tinyint(3) unsigned
,`suggestions` text
);
-- --------------------------------------------------------

--
-- Table structure for table `submission`
--

CREATE TABLE IF NOT EXISTS `submission` (
  `workerId` varchar(50) NOT NULL,
  `experimentId` tinyint(3) unsigned NOT NULL,
  `hitId` varchar(50) NOT NULL,
  `assignmentId` varchar(50) NOT NULL,
  `submissionTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `resultData` text NOT NULL,
  PRIMARY KEY (`workerId`,`experimentId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `submission_amt`
--

CREATE TABLE IF NOT EXISTS `submission_amt` (
  `workerId` varchar(50) NOT NULL,
  `experimentId` tinyint(3) unsigned NOT NULL,
  `hitId` varchar(50) NOT NULL,
  `assignmentId` varchar(50) NOT NULL,
  `submissionTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `resultData` text NOT NULL,
  PRIMARY KEY (`workerId`,`experimentId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `survey`
--

CREATE TABLE IF NOT EXISTS `survey` (
  `workerId` varchar(50) NOT NULL,
  `experimentId` tinyint(3) unsigned NOT NULL,
  `hitId` varchar(50) NOT NULL,
  `assignmentId` varchar(50) NOT NULL,
  `submissionTime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `native` tinyint(3) unsigned NOT NULL,
  `gender` tinyint(3) unsigned NOT NULL,
  `age` tinyint(3) unsigned NOT NULL,
  `difficulty` tinyint(3) unsigned NOT NULL,
  `backtrack` tinyint(3) unsigned NOT NULL,
  `who` tinyint(3) unsigned NOT NULL,
  `information` tinyint(3) unsigned NOT NULL,
  `confidence` tinyint(3) unsigned NOT NULL,
  `suggestions` text NOT NULL,
  PRIMARY KEY (`workerId`,`experimentId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Structure for view `get_distinct_ex_ids`
--
DROP TABLE IF EXISTS `get_distinct_ex_ids`;

CREATE ALGORITHM=UNDEFINED DEFINER=`nligen_sim`@`%` SQL SECURITY DEFINER VIEW `get_distinct_ex_ids` AS select distinct `experiment`.`ex_id` AS `ex_id` from `experiment`;

-- --------------------------------------------------------

--
-- Structure for view `get_exId_H_R`
--
DROP TABLE IF EXISTS `get_exId_H_R`;

CREATE ALGORITHM=UNDEFINED DEFINER=`nligen_sim`@`%` SQL SECURITY DEFINER VIEW `get_exId_H_R` AS select `z`.`ex_id` AS `ex_id`,(select `c1`.`counter` from `get_exId_who_counter` `c1` where ((`c1`.`ex_id` = `z`.`ex_id`) and (`c1`.`who` = 'human'))) AS `human`,(select `c2`.`counter` from `get_exId_who_counter` `c2` where ((`c2`.`ex_id` = `z`.`ex_id`) and (`c2`.`who` = 'robot'))) AS `machine` from `get_distinct_ex_ids` `z`;

-- --------------------------------------------------------

--
-- Structure for view `get_exId_who_counter`
--
DROP TABLE IF EXISTS `get_exId_who_counter`;

CREATE ALGORITHM=UNDEFINED DEFINER=`nligen_sim`@`%` SQL SECURITY DEFINER VIEW `get_exId_who_counter` AS select `z`.`ex_id` AS `ex_id`,`z`.`who` AS `who`,count(0) AS `counter` from (`submission` `s` join `experiment` `z`) where (`s`.`experimentId` = `z`.`id`) group by `z`.`ex_id`,`z`.`who`;

-- --------------------------------------------------------

--
-- Structure for view `H_submissions`
--
DROP TABLE IF EXISTS `H_submissions`;

CREATE ALGORITHM=UNDEFINED DEFINER=`nligen_sim`@`%` SQL SECURITY DEFINER VIEW `H_submissions` AS select `s`.`workerId` AS `workerId`,`s`.`experimentId` AS `experimentId`,`s`.`hitId` AS `hitId`,`s`.`assignmentId` AS `assignmentId`,`s`.`submissionTime` AS `submissionTime`,`s`.`resultData` AS `resultData` from (`submission` `s` join `experiment` `e`) where ((`s`.`experimentId` = `e`.`id`) and (`e`.`who` = 'human'));

-- --------------------------------------------------------

--
-- Structure for view `H_survey`
--
DROP TABLE IF EXISTS `H_survey`;

CREATE ALGORITHM=UNDEFINED DEFINER=`nligen_sim`@`%` SQL SECURITY DEFINER VIEW `H_survey` AS select `z`.`workerId` AS `workerId`,`z`.`experimentId` AS `experimentId`,`z`.`hitId` AS `hitId`,`z`.`assignmentId` AS `assignmentId`,`z`.`submissionTime` AS `submissionTime`,`z`.`native` AS `native`,`z`.`gender` AS `gender`,`z`.`age` AS `age`,`z`.`difficulty` AS `difficulty`,`z`.`backtrack` AS `backtrack`,`z`.`who` AS `who`,`z`.`information` AS `information`,`z`.`confidence` AS `confidence`,`z`.`suggestions` AS `suggestions` from (`survey` `z` join `experiment` `e`) where ((`z`.`experimentId` = `e`.`id`) and (`e`.`who` = 'human'));

-- --------------------------------------------------------

--
-- Structure for view `R_submissions`
--
DROP TABLE IF EXISTS `R_submissions`;

CREATE ALGORITHM=UNDEFINED DEFINER=`nligen_sim`@`%` SQL SECURITY DEFINER VIEW `R_submissions` AS select `s`.`workerId` AS `workerId`,`s`.`experimentId` AS `experimentId`,`s`.`hitId` AS `hitId`,`s`.`assignmentId` AS `assignmentId`,`s`.`submissionTime` AS `submissionTime`,`s`.`resultData` AS `resultData` from (`submission` `s` join `experiment` `e`) where ((`s`.`experimentId` = `e`.`id`) and (`e`.`who` = 'robot'));

-- --------------------------------------------------------

--
-- Structure for view `R_survey`
--
DROP TABLE IF EXISTS `R_survey`;

CREATE ALGORITHM=UNDEFINED DEFINER=`nligen_sim`@`%` SQL SECURITY DEFINER VIEW `R_survey` AS select `z`.`workerId` AS `workerId`,`z`.`experimentId` AS `experimentId`,`z`.`hitId` AS `hitId`,`z`.`assignmentId` AS `assignmentId`,`z`.`submissionTime` AS `submissionTime`,`z`.`native` AS `native`,`z`.`gender` AS `gender`,`z`.`age` AS `age`,`z`.`difficulty` AS `difficulty`,`z`.`backtrack` AS `backtrack`,`z`.`who` AS `who`,`z`.`information` AS `information`,`z`.`confidence` AS `confidence`,`z`.`suggestions` AS `suggestions` from (`survey` `z` join `experiment` `e`) where ((`z`.`experimentId` = `e`.`id`) and (`e`.`who` = 'robot'));

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
