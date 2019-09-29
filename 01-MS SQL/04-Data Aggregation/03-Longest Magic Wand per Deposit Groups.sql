SELECT DepositGroup, MAX(MagicWandSize) AS LongestMagicWant
FROM WizzardDeposits AS w
GROUP BY DepositGroup
