cd Wodeyun.Project.GsmArea
call GsmAreaDelete.bat
cd..

cd Wodeyun.Project.GsmLine
call GsmLineDelete.bat
cd..

cd Wodeyun.Project.GsmMessage
call GsmMessageDelete.bat
cd..

cd Wodeyun.Project.GsmOrigin
call GsmOriginDelete.bat
cd..

cd Wodeyun.Project.GsmReport
call GsmReportDelete.bat
cd..

cd Wodeyun.Project.GsmSupplier
call GsmSupplierDelete.bat
cd..

cd Wodeyun.Project.GsmTree
call GsmTreeDelete.bat
cd..

appcmd delete apppool "Wodeyun.Project.Www"
appcmd delete site "Wodeyun.Project.Www"
