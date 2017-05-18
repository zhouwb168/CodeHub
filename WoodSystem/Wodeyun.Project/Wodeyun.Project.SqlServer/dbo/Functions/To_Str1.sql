CREATE function To_Str1(@carid varchar(20),@startime datetime,@endtime datetime )
RETURNS varchar(8000)
as 
begin
	declare @v as varchar(2000)
	select @v = ''
	select @v=@v + ',' +  convert(varchar, (t1.FullVolume - isnull(t2.EmptyVolume,0)))  from FullPound t1
inner join EmptyPound t2 on t1.WoodID = t2.WoodID
inner join WoodBang t3 on t1.License like '%' + t3.carCID and abs( DATEDIFF(mi, t1.WeighTime , t3.Bang_Time)) <=2
where t3.carCID = @carid and t1.WeighTime between @startime and @endtime
	return @v
end