
CREATE FUNCTION [dbo].[F_SQLSERVER_SPLIT](@Long_str varchar(8000),@split_str varchar(100))    
RETURNS  @tmp TABLE(        
    ID          inT     IDENTITY PRIMARY KEY,      
    short_str   varchar(8000)    
)    
AS   
BEGIN   
    DECLARE @long_str_Tmp varchar(8000),@short_str varchar(8000),@split_str_length int   
    SET @split_str_length = LEN(@split_str)    
    IF CHARINDEX(@split_str,@Long_str)=1 
         SET @long_str_Tmp=SUBSTRING(@Long_str,@split_str_length+1,LEN(@Long_str)-@split_str_length)
    ELSE
         SET @long_str_Tmp=@Long_str
    IF CHARINDEX(REVERSE(@split_str),REVERSE(@long_str_Tmp))>1    
        SET @long_str_Tmp=@long_str_Tmp+@split_str    
    ELSE   
        SET @long_str_Tmp=@long_str_Tmp    
    IF CHARINDEX(@split_str,@long_str_Tmp)=0
        Insert INTO @tmp select @long_str_Tmp 
    ELSE
        BEGIN
            WHILE CHARINDEX(@split_str,@long_str_Tmp)>0    
                BEGIN   
                    SET @short_str=SUBSTRING(@long_str_Tmp,1,CHARINDEX(@split_str,@long_str_Tmp)-1)    
                    DECLARE @long_str_Tmp_LEN INT,@split_str_Position_END int   
                    SET @long_str_Tmp_LEN = LEN(@long_str_Tmp)    
                    SET @split_str_Position_END = LEN(@short_str)+@split_str_length    
                    SET @long_str_Tmp=REVERSE(SUBSTRING(REVERSE(@long_str_Tmp),1,@long_str_Tmp_LEN-@split_str_Position_END))
                    IF @short_str<>'' Insert INTO @tmp select @short_str    
                END           
        END
    RETURN     
END