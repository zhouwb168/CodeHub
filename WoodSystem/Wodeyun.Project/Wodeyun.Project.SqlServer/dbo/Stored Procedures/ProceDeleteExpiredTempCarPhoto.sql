-- =============================================
-- Author:		黄永莹
-- Create date: 2013.09.25
-- Description:	删除临时的汽车照片数据表里的过期记录，记录的有效期为2天
-- =============================================
CREATE PROCEDURE ProceDeleteExpiredTempCarPhoto
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM [WoodTempPhoto] WHERE DATEDIFF(day, [PhotoTime], GETDATE()) > 1
END