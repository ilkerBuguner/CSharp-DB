ALTER TABLE Users
ADD CONSTRAINT Df_LastLoginTime
DEFAULT GETDATE() FOR LastLoginTime